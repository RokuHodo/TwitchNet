// standard namespaces
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
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

        private Stream stream;
        private Socket socket;

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

        public void
        Connect()
        {
            if (!SetState(ClientState.Connecting))
            {
                return;
            }

            WebSocket.RegisterPrefixes();

            //stream = new TcpClient(PUB_SUB_URI.DnsSafeHost, PUB_SUB_URI.Port).GetStream();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(PUB_SUB_URI.Host, PUB_SUB_URI.Port);

            //stream = new NetworkStream(socket);
            stream = new TcpClient(PUB_SUB_URI.DnsSafeHost, PUB_SUB_URI.Port).GetStream();
            if (PUB_SUB_URI.Port == 443)
            {
                SslStream ssl_stream = new SslStream(stream, false);
                ssl_stream.AuthenticateAsClient(PUB_SUB_URI.DnsSafeHost, new X509CertificateCollection(), SslProtocols.Default, false);

                stream = ssl_stream;
            }

            HttpWebRequest request = WebRequest.Create(PUB_SUB_URI) as HttpWebRequest;

            HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().Result;
            Debug.WriteLine((int)response.StatusCode + ": " + response.StatusDescription);
            foreach(string header in response.Headers.AllKeys)
            {
                Debug.WriteLine(header + ": " + response.Headers[header]);
            }
            Debug.WriteLine();

            reader_thread = new Thread(new ThreadStart(ReadStream));
            reader_thread.Start();

            SetState(ClientState.Connected);            
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
            // ForceDisconnectAsync().Wait();

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

        public bool
        Send(string message)
        {
            message = message.Trim();
            byte[] bytes = Encoding.UTF8.GetBytes(message);

            Send(Opcode.Text, new MemoryStream(bytes));

            // OnDataSent.Raise(this, new DataEventArgs(bytes, message));

            return true;
        }

        private bool
        Send(Opcode opcode, Stream stream)
        {
            long length = stream.Length;
            if (length == 0)
            {
                return Send(Fin.Final, opcode, new byte[0]);
            }

            int fragment_length = 1016;
            byte[] buffer = length < fragment_length ? new byte[length] : new byte[fragment_length];

            // ---------------------------------------------
            // The entire message can be sent as one message
            // ---------------------------------------------

            if (buffer.Length <= fragment_length)
            {
                return stream.Read(buffer, 0, buffer.Length) == buffer.Length && Send(Fin.Final, opcode, buffer);
            }

            // ---------------------------------------------
            // The message needs to sent in fragments
            // ---------------------------------------------

            Fin fin = Fin.Fragment;

            // Figure out how many fragments needs to be sent
            long fragments_count = (buffer.Length / fragment_length);
            int remainder = buffer.Length % fragment_length;
            if(remainder > 0)
            {
                ++fragments_count;
            }

            int buffer_bytes_read_count = 0;
            for (int index = 0; index < fragments_count; ++index)
            {
                if (index != 0)
                {
                    opcode = Opcode.Continuation; 
                }

                if (index == fragments_count - 1)
                {
                    fin = Fin.Final;
                    buffer = new byte[remainder];
                }

                buffer_bytes_read_count = stream.Read(buffer, 0, buffer.Length);
                if (buffer_bytes_read_count != buffer.Length || !Send(fin, opcode, buffer))
                {
                    return false;
                }                

                Array.Clear(buffer, 0, buffer_bytes_read_count);
            }

            return true;
        }

        private bool
        Send(Fin fin, Opcode opcode, byte[] data)
        {
            if (state != ClientState.Connected)
            {
                return false;
            }

            WebSocketFrame frame = new WebSocketFrame(fin, opcode, data);

            return Send(frame.ToByteArray());
        }

        private bool
        Send(byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();

            return true;
        }

        #region Reading

        /// <summary>
        /// Reads and incoming data from the IRC via a <see cref="NetworkStream"/>.
        /// </summary>
        private async void
        ReadStream()
        {
            byte[] buffer = new byte[1024];
            int buffer_index_peek = 0;
            int buffer_bytes_read_count = 0;

            List<byte> data = new List<byte>(buffer.Length);

            byte[] message_bytes = new byte[buffer.Length];
            string message_string = string.Empty;

            reading = true;
            while (polling && !socket.IsNull() && !stream.IsNull())
            {
                try
                {
                    buffer_bytes_read_count = await stream.ReadAsync(buffer, 0, buffer.Length);
                }
                catch (ObjectDisposedException exception)
                {
                    polling = false;
                    reading = false;

                    // OnNetworkError.Raise(this, new ErrorEventArgs(exception));

                    continue;
                }

                // We reached the end of the stream and there is nothing to process
                if (buffer_bytes_read_count == 0 && buffer.Length == 0 && data.Count == 0)
                {
                    continue;
                }

                for (int index = 0; index < buffer.Length; ++index)
                {
                    if (buffer[index] == 0x0)
                    {
                        continue;
                    }

                    // 0x0D = '\r', 0x0A = '\n', 
                    if (buffer[index] == 0x0D || buffer[index] == 0x0A)
                    {
                        // Any instance of buffer[1023] == '\r' from the previous read and buffer[0] == '\n' from this read will be caught here.
                        // The list of data to be encoded will empty, so nothing will actually be encoded.
                        message_bytes = data.ToArray();
                        if (message_bytes.Length == 0)
                        {
                            continue;
                        }

                        message_string = Encoding.UTF8.GetString(message_bytes, 0, message_bytes.Length);
                        Debug.WriteLine(message_string);

                        // OnDataReceived.Raise(this, new DataEventArgs(message_bytes, message_string));

                        data.Clear();

                        buffer_index_peek = index + 1;
                        if (buffer[index] == 0x0D && buffer_index_peek < buffer.Length)
                        {
                            if (buffer[buffer_index_peek] == 0x0A)
                            {
                                index++;
                            }
                        }
                    }
                    else
                    {
                        data.Add(buffer[index]);
                    }
                }

                Array.Clear(buffer, 0, buffer_bytes_read_count);
            }

            reading = false;
        }

        #endregion
    }

    // Send(Opcode.Text, new MemoryStream(bytes));

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

    public readonly struct
    WebSocketFrame
    {
        public readonly byte[] data;

        public readonly Fin fin;

        public readonly RSV rsv_1;
        public readonly RSV rsv_2;
        public readonly RSV rsv_3;

        public readonly Opcode opcode;

        public readonly Mask mask;
        public readonly byte[] mask_key;

        public readonly long length;
        public readonly byte length_payload;
        public readonly byte[] length_payload_extended;

        public
        WebSocketFrame(Fin fin, Opcode opcode, byte[] data, bool use_mask = true)
        {
            this.data = data;

            this.fin = fin;

            rsv_1 = RSV.Off;    // This will always be uncompressed data so this will never be 'On'
            rsv_2 = RSV.Off;
            rsv_3 = RSV.Off;

            this.opcode = opcode;

            length = data.LongLength;
            if (length < 126)
            {
                length_payload = (byte)length;
                length_payload_extended = new byte[0];
            }
            else if (length <= ushort.MaxValue)
            {
                length_payload = 126;
                length_payload_extended = BitConverter.GetBytes((ushort)length);
            }
            else
            {
                length_payload = 127;
                length_payload_extended = BitConverter.GetBytes(length);
            }

            if (use_mask)
            {
                mask = Mask.On;
                mask_key = CreateMaskingKey();
                MaskData(mask_key);
            }
            else
            {
                mask = Mask.Off;
                mask_key = new byte[0];
            }
        }

        private static byte[]
        CreateMaskingKey()
        {
            byte[] key = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(key);

            return key;
        }

        private void
        MaskData(byte[] key)
        {
            //byte[] result = new byte[data.Length];

            for (long index = 0; index < length; ++index)
            {
                data[index] = (byte)(data[index] ^ key[index % 4]);
            }

            //return result;
        }

        public byte[]
        ToByteArray()
        {
            MemoryStream buffer = new MemoryStream();

            int header = (int)fin;
            header = (header << 1) + (int)rsv_1;
            header = (header << 1) + (int)rsv_2;
            header = (header << 1) + (int)rsv_3;
            header = (header << 4) + (int)opcode;
            header = (header << 1) + (int)mask;
            header = (header << 7) + (int)length_payload;

            buffer.Write(BitConverter.GetBytes((ushort)header), 0, 2);

            if (length_payload > 125)
            {
                // length_payload == 126 -> length_payload_length = 2 bytes
                // length_payload == 127 -> length_payload_length = 8 bytes
                int count = length_payload == 126 ? 2 : 8;
                buffer.Write(length_payload_extended, 0, count);
            }

            if (mask == Mask.On)
            {
                buffer.Write(mask_key, 0, 4);
            }

            if (length_payload > 0)
            {
                if (length_payload < 127)
                {
                    buffer.Write(data, 0, data.Length);
                }
                else
                {
                    buffer.WriteBytes(data, 1024);
                }
            }

            buffer.Close();

            return buffer.ToArray();
        }
    }

    public enum
    Fin : byte
    {
        Fragment = 0x0,

        Final = 0x1
    }

    public enum
    Opcode : byte
    {
        Continuation = 0x0,

        Text = 0x1,

        Binary = 0x2,

        Close = 0x8,

        Ping = 0x9,

        Pong = 0xA
    }

    public enum
    RSV : byte
    {
        Off = 0x0,

        On = 0x1
    }

    public enum
    Mask : byte
    {
        Off = 0x0,

        On = 0x1
    }    
}
