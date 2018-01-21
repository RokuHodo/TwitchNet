// standard namespaces
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Debug;
using TwitchNet.Enums.Clients;
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc
{
    public class
    IrcClient
    {
        #region Fields

        private bool                    reading;

        private ushort                  _port;

        private string                  _host;

        private Encoding                encoding;
        private Socket                  socket;
        private Stream                  stream;
        private Thread                  reader_thread;

        private Mutex                   state_mutex;

        private System.Timers.Timer     processing_timer;

        private ConcurrentQueue<string> processing_queue_normal;
        private ConcurrentQueue<string> processing_queue_priority;

        #endregion

        #region Properties

        public ushort port
        {
            get
            {
                return _port;
            }
            set
            {
                ExceptionUtil.ThrowIfNullOrDefault(value, nameof(port));
                _port = value;
            }
        }

        public string host
        {
            get
            {
                return _host;
            }
            set
            {
                ExceptionUtil.ThrowIfInvalid(value, nameof(host));
                _host = value;
            }
        }

        public ClientState state { get; private set; }

        public IrcUser irc_user { get; set; }

        #endregion

        #region Constructors

        public
        IrcClient(string host, ushort port, IrcUser irc_user)
        {
            this.host = host;
            this.port = port;

            ExceptionUtil.ThrowIfNull(irc_user, nameof(irc_user));
            this.irc_user = irc_user;

            reading = false;

            encoding = Encoding.UTF8;
            state_mutex = new Mutex();

            processing_timer = new System.Timers.Timer(1);
            processing_timer.Elapsed += Callback_ProcessingTimer;

            processing_queue_normal = new ConcurrentQueue<string>();

            SetState(ClientState.Disconnected);
        }

        #endregion

        #region Connection handling

        public void
        Connect()
        {
            if (!SetState(ClientState.Connecting))
            {
                return;
            }

            // TODO: First check to see if these are true to dispose/close and null the socket and then set state to disconnected.
            ExceptionUtil.ThrowIfInvalid(host, nameof(host), QuickDisconnect);
            ExceptionUtil.ThrowIfNullOrDefault(port, nameof(port), QuickDisconnect);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(host, port);

            Login();
        }

        public void
        ConnectAsync()
        {
            if (!SetState(ClientState.Connecting))
            {
                return;
            }

            ExceptionUtil.ThrowIfInvalid(host, nameof(host), QuickDisconnect);
            ExceptionUtil.ThrowIfNullOrDefault(port, nameof(port), QuickDisconnect);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.BeginConnect(host, port, Callback_OnBeginConnect, null);
        }

        private void
        Callback_OnBeginConnect(IAsyncResult result)
        {
            socket.EndConnect(result);

            Login();
        }

        private void
        Login()
        {
            ExceptionUtil.ThrowIfNull(irc_user, nameof(irc_user), QuickDisconnect);
            ExceptionUtil.ThrowIfInvalid(irc_user.nick, nameof(irc_user.nick), QuickDisconnect);
            ExceptionUtil.ThrowIfInvalid(irc_user.pass, nameof(irc_user.pass), QuickDisconnect);

            stream = new NetworkStream(socket);

            if (port == 443)
            {
                stream = new SslStream(stream, false);
                ((SslStream)stream).AuthenticateAsClient(host);
            }

            reader_thread = new Thread(new ThreadStart(ReadStream));
            reader_thread.Start();

            processing_timer.Start();

            Send("PASS oauth:" + irc_user.pass);
            Send("NICK " + irc_user.nick);

            // NOTE: only for tetsing
            // Send("JOIN #distortion2");
            SetState(ClientState.Connected);
        }

        private void
        QuickDisconnect()
        {
            OverrideState(ClientState.Disconnecting);

            if (!stream.IsNull())
            {
                stream.Close();
                stream = null;
            }

            if (!socket.IsNull())
            {
                if (socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Disconnect(false);
                }
                socket = null;
            }

            OverrideState(ClientState.Disconnected);
        }

        public void
        Disconnect()
        {
            if (!SetState(ClientState.Disconnecting))
            {
                return;
            }

            stream.Close();
            stream = null;

            socket.Shutdown(SocketShutdown.Both);
            socket.Disconnect(false);
            socket = null;

            do
            {
                Thread.Sleep(25);
            }
            while (reading);

            SetState(ClientState.Disconnected);

            // OnDisconnected.Raise(this, EventArgs.Empty);
        }

        public async void
        DisconnectAsync()
        {
            if (!SetState(ClientState.Disconnecting))
            {
                return;
            }

            stream.Close();
            stream = null;

            socket.Shutdown(SocketShutdown.Both);
            socket.BeginDisconnect(false, Callback_OnBeginDisconnect, null);

            do
            {
                await Task.Delay(25);
            }
            while (reading);

            SetState(ClientState.Disconnected);

            // OnDisconnected.Raise(this, EventArgs.Empty);
        }

        private void
        Callback_OnBeginDisconnect(IAsyncResult result)
        {
            socket.EndDisconnect(result);
            socket = null;

            SetState(ClientState.Disconnected);

            // OnDisconnected.Raise(this, EventArgs.Empty);
        }

        #endregion

        #region State handling

        private void
        OverrideState(ClientState override_state)
        {
            state_mutex.WaitOne();
            state = override_state;
            state_mutex.ReleaseMutex();
        }

        private bool
        SetState(ClientState transition_state, bool attempting_reconnect = false)
        {
            bool success = false;

            state_mutex.WaitOne();
            switch (transition_state)
            {
                case ClientState.Connecting:
                {
                    success = attempting_reconnect ? CanReconnect() : CanConnect();
                }
                break;

                case ClientState.Disconnecting:
                {
                    success = attempting_reconnect ? CanReconnect() : CanDisconnect();
                }
                break;

                case ClientState.Connected:
                case ClientState.Disconnected:
                {
                    success = attempting_reconnect ? CanReconnect() : true;
                }
                break;
            }

            if (success)
            {
                state = transition_state;
            }
            state_mutex.ReleaseMutex();

            return success;
        }

        /// <summary>
        /// Checks to see if it is safe to connect to the IRC.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool
        CanConnect()
        {
            bool result = false;

            string error = "Cannot connect to " + host + ": {0}";

            switch (state)
            {
                case ClientState.Connected:
                {
                    Log.Warning(string.Format(error, "already connected"));
                }
                break;

                case ClientState.Connecting:
                {
                    Log.Warning(string.Format(error, "already connecting"));
                }
                break;

                case ClientState.Disconnecting:
                {
                    Log.Warning(string.Format(error, "currently disconnecting"));
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
        /// Checks to see if it is safe to disconnect from the Twitch IRC.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool
        CanDisconnect()
        {
            bool result = false;

            string error = "Cannot disconnect to " + host + ": {0}";

            switch (state)
            {
                case ClientState.Connecting:
                {
                    Log.Warning(string.Format(error, "currently connecting"));
                }
                break;

                case ClientState.Disconnecting:
                {
                    Log.Warning(string.Format(error, "currently disconnecting"));
                }
                break;

                case ClientState.Disconnected:
                {
                    Log.Warning(string.Format(error, "already disconnected"));
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

        /// <summary>
        /// Checks to see if it is safe to reconnect to the Twitch IRC.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool
        CanReconnect()
        {
            bool result = false;

            string error = "Cannot reconnect to " + host + ": {0}";

            switch (state)
            {
                case ClientState.Connecting:
                {
                    Log.Warning(string.Format(error, "currently connecting"));
                }
                break;

                case ClientState.Disconnecting:
                {
                    Log.Warning(string.Format(error, "currently disconnecting"));
                }
                break;

                case ClientState.Connected:
                case ClientState.Disconnected:
                {
                    result = true;
                }
                break;
            }
            return result;
        }

        #endregion

        #region Sending

        public void
        Send(string format, params object[] arguments)
        {
            if (!format.IsValid())
            {
                return;
            }

            string message = string.Format(format, arguments);
            if (!CanSend(message))
            {
                return;
            }

            byte[] bytes = encoding.GetBytes(message + "\r\n");
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();

            // OnMessageSent.Raise(this, new MessageEventArgs(message));
        }

        public async void
        SendAsync(string format, params object[] arguments)
        {
            if (!format.IsValid())
            {
                return;
            }

            string message = string.Format(format, arguments);
            if (!CanSend(message))
            {
                return;
            }

            byte[] bytes = encoding.GetBytes(message + "\r\n");
            await stream.WriteAsync(bytes, 0, bytes.Length);
            stream.Flush();

            // OnMessageSent.Raise(this, new MessageEventArgs(message));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool
        CanSend(string message)
        {
            bool result = true;

            if (!socket.Connected || socket.IsNull())
            {
                result = false;
            }
            else if (stream.IsNull())
            {
                result = false;
            }
            else if (!message.IsValid())
            {
                result = false;
            }
            else if (message.Length > 512)
            {
                result = false;
            }

            return result;
        }

        #endregion

        #region Reading

        private void
        ProcessMessages()
        {
            if (!processing_queue_priority.TryDequeue(out string message))
            {
                // As long as there are priority messages, don't process any normal messages.
                // There was probably just an issue dequeueing the message, or a priority message was just enqueued by the reader.
                if(processing_queue_priority.Count != 0)
                {
                    return;
                }

                if (!processing_queue_normal.TryDequeue(out message))
                {
                    return;
                }
            }

            Log.PrintLine(message);
        }

        private void
        Callback_ProcessingTimer(object sender, ElapsedEventArgs e)
        {
            if (!processing_queue_normal.TryDequeue(out string message))
            {
                return;
            }

            Log.PrintLine(message);
        }

        private async void
        ReadStream()
        {
            int bytes_count = 0;
            byte[] buffer = buffer = new byte[1024];

            List<byte> line = new List<byte>();

            reading = true;

            while (reading && !socket.IsNull() && !stream.IsNull())
            {
                try
                {
                    bytes_count = await stream.ReadAsync(buffer, 0, buffer.Length);
                }
                catch (ObjectDisposedException exception)
                {
                    if(state == ClientState.Disconnecting)
                    {
                        reading = false;

                        continue;
                    }

                    throw exception;
                }

                if (bytes_count == 0 || !buffer.IsValid())
                {
                    continue;
                }

                foreach (byte element in buffer)
                {
                    if(element == 0x0)
                    {
                        continue;
                    }

                    line.Add(element);
                        
                    // 0x0A = '\n', 0x0D = '\r'
                    if(element == 0x0A)
                    {
                        // This is assumes that the line terminates with \r\n and not \n\r, or just either \r or \n
                        // Safe assumption on windows and that the RFC 1459 spec requires it, but we should probably handle other cases to be safe
                        string message = encoding.GetString(line.ToArray(), 0, line.Count - 2);
                        Log.PrintLine(message);
                        if (message.IsValid())
                        {
                            processing_queue_normal.Enqueue(message);
                        }

                        line.Clear();
                    }
                }

                Array.Clear(buffer, 0, bytes_count);
            }
        }

        #endregion
    }
}
