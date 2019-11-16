// standard namespaces
using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Clients.PubSub
{
    public class
    PubSubClient
    {
        private volatile bool polling;
        private volatile bool reading;

        private bool disposing;
        private bool disposed;

        private readonly Uri PUB_SUB_URI = new Uri("wss://pubsub-edge.twitch.tv");

        private Mutex state_mutex;
        private ClientWebSocket socket;

        private Thread reader_thread;

        public ClientState state { get; private set; }

        public
        PubSubClient()
        {
            state_mutex = new Mutex();            
            SetState(ClientState.Disconnected);

            reading = false;
            disposing = false;

            disposed = false;
        }

        #region Connection and state handling

        public async Task
        ConnectAsync(CancellationToken token = default)
        {
            if (!SetState(ClientState.Connecting))
            {
                return;
            }

            if (token == default)
            {
                token = CancellationToken.None;
            }


            socket = new ClientWebSocket();
            await socket.ConnectAsync(PUB_SUB_URI, token);

            reader_thread = new Thread(new ThreadStart(ReadSocket));
            reader_thread.Start();

            SetState(ClientState.Connected);

            ListenTopic topic = new ListenTopic();
            topic.type = "LISTEN";
            topic.nonce = "1234";
            topic.data.topics.Add("whispers.45947671");
            topic.data.auth_token = "feam5hsenkf2afolx4394znllq18s7";

            string serialized = JsonConvert.SerializeObject(topic);
            byte[] array = Encoding.UTF8.GetBytes(serialized);

            ArraySegment<byte> segamnt = new ArraySegment<byte>(array);

            await socket.SendAsync(segamnt, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task
        ForceDisconnectAsync(CancellationToken token = default)
        {
            await DisconnectAsync(true, token);
        }

        public async Task
        DisconnectAsync(CancellationToken token = default)
        {
            await DisconnectAsync(false, token);
        }

        public async Task
        DisconnectAsync(bool force_disconnect, CancellationToken token = default)
        {
            if (!SetState(ClientState.Disconnecting, force_disconnect))
            {
                return;
            }

            if (token == default)
            {
                token = CancellationToken.None;
            }

            await socket.CloseAsync(socket.CloseStatus.Value, socket.CloseStatusDescription, token);
            socket.Dispose();
            socket = null;

            SetState(ClientState.Disconnected);
        }

        public void
        Dispose()
        {
            Dispose(true);
        }

        private void
        Dispose(bool dispose)
        {
            if (disposing || !dispose || disposed)
            {
                Debug.WriteLine("Already disposing or disposed.");

                return;
            }

            disposing = true;

            Debug.WriteLine("Disposing");

            // If we're disposing we're disconnecting, period. Make sure this is always true.
            // If the client is already in the process of disconnecting, this will effectively do nothing.
            ForceDisconnectAsync().Wait();

            if (!socket.IsNull())
            {
                socket.Dispose();
            }

            // This is the last step before the client fully disconnects, it's safe to call this here. 
            SetState(ClientState.Disconnected, true);

            if (!state_mutex.IsNull())
            {
                state_mutex.Dispose();
                state_mutex = null;
            }

            disposed = true;
            // OnDisposed.Raise(this, EventArgs.Empty);
        }

        /// <summary>
        /// Sets the client's state.
        /// </summary>
        /// <param name="transition_state">The new state to set.</param>
        /// <param name="override_state">Whether or not to ignore the checks to see if the state can safely be changed.</param>
        /// <returns>
        /// Returns true if the state was successfully changed to the new state.
        /// Returns false otherwise.
        /// </returns>
        private bool
        SetState(ClientState transition_state, bool override_state = false)
        {
            bool success = false;

            if (state == transition_state)
            {
                return success;
            }

            if (state_mutex.IsNull())
            {
                state = ClientState.Disconnected;

                return success;
            }

            state_mutex.WaitOne();
            success = override_state;
            if (!override_state)
            {
                switch (transition_state)
                {
                    case ClientState.Connecting:
                    {
                        success = CanConnect();
                    }
                    break;

                    case ClientState.Disconnecting:
                    {
                        success = CanDisconnect();
                    }
                    break;

                    case ClientState.Connected:
                    case ClientState.Disconnected:
                    {
                        success = true;
                    }
                    break;
                }
            }

            if (success)
            {
                state = transition_state;
                if (state == ClientState.Connecting)
                {
                    polling = true;
                }
                else if (state == ClientState.Disconnecting)
                {
                    polling = false;
                }
            }
            state_mutex.ReleaseMutex();

            return success;
        }

        /// <summary>
        /// Checks to see if it is safe to connect.
        /// </summary>
        private bool
        CanConnect()
        {
            bool result = false;

            switch (state)
            {
                case ClientState.Connected:
                {
                    Debug.WriteLine("Cannot connect to " + PUB_SUB_URI.Host + ": already connected");
                }
                break;

                case ClientState.Connecting:
                {
                    Debug.WriteLine("Cannot connect to " + PUB_SUB_URI.Host + ": currently connecting");
                }
                break;

                case ClientState.Disconnecting:
                {
                    Debug.WriteLine("Cannot connect to " + PUB_SUB_URI.Host + ": currently disconnecting");
                }
                break;

                case ClientState.Disconnected:
                {
                    result = true;
                }
                break;
            }

            return result;
        }

        /// <summary>
        /// Checks to see if it is safe to disconnect.
        /// </summary>
        private bool
        CanDisconnect()
        {
            bool result = false;

            switch (state)
            {
                case ClientState.Connecting:
                {
                    Debug.WriteLine("Cannot disconnect from " + PUB_SUB_URI.Host + ": currently connecting");
                }
                break;

                case ClientState.Disconnecting:
                {
                    Debug.WriteLine("Cannot disconnect from " + PUB_SUB_URI.Host + ": already connecting");
                }
                break;

                case ClientState.Disconnected:
                {
                    Debug.WriteLine("Cannot disconnect from " + PUB_SUB_URI.Host + ": already disconnected");
                }
                break;

                case ClientState.Connected:
                {
                    result = true;
                }
                break;
            }

            return result;
        }

        #endregion

        #region Reading

        private async void
        ReadSocket()
        {
            int buffer_size = 1024;

            WebSocketReceiveResult result;

            List<byte> data = new List<byte>(buffer_size);

            reading = true;
            while (polling && !socket.IsNull())
            {
                ArraySegment<byte> buffer_segment = WebSocket.CreateClientBuffer(buffer_size, buffer_size);
                result = await socket.ReceiveAsync(buffer_segment, CancellationToken.None);

                if (buffer_segment.Count == 0 && data.Count == 0)
                {
                    continue;
                }                

                foreach (byte element in buffer_segment.Array)
                {
                    if (element == 0x0)
                    {
                        continue;
                    }

                    data.Add(element);
                }

                if (!result.EndOfMessage)
                {
                    continue;
                }

                string message = Encoding.UTF8.GetString(data.ToArray(), 0, data.Count);
                Console.Write(message);

                data.Clear();
            }

            reading = false;
        }

        #endregion

        public class
        ListenTopic
        {
            [JsonProperty("type")]
            public string type;

            [JsonProperty("nonce")]
            public string nonce;

            [JsonProperty("data")]
            public TopicData data;

            public ListenTopic()
            {
                data = new TopicData();
            }
        }

        public class
        TopicData
        {
            [JsonProperty("topics")]
            public List<string> topics;

            [JsonProperty("auth_token")]
            public string auth_token;

            public TopicData()
            {
                topics = new List<string>();
            }
        }
    }
}
