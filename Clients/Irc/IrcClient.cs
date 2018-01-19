// standard namespaces
using System;
using System.IO;
using System.Net.Sockets;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Text;
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

        private bool        reading;

        private ushort      _port;

        private string      _host;

        private Encoding    encoding;
        private Socket      socket;
        private Stream      stream;
        private Thread      reader_thread;

        private Mutex       state_mutex;        

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
            ExceptionUtil.ThrowIfInvalid(host, nameof(host));
            ExceptionUtil.ThrowIfNullOrDefault(port, nameof(port));

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

            // TODO: First check to see if these are true to dispose/close and null the socket and then set state to disconnected.
            ExceptionUtil.ThrowIfInvalid(host, nameof(host));
            ExceptionUtil.ThrowIfNullOrDefault(port, nameof(port));

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.BeginConnect(host, port, Callback_OnBeginConnect, null);
        }

        private void
        Callback_OnBeginConnect(IAsyncResult result)
        {
            if (socket.IsNull() || result.IsNull())
            {
                //TODO: Force disconnect, clean up, and set to disconnected

                return;
            }

            if (result.IsCompleted)
            {
                Login();
            }
            else
            {
                //TODO: Force disconnect, clean up, and set to disconnected
            }

            socket.EndConnect(result);
        }

        private void
        Login()
        {
            // TODO: First check to see if these are true to dispose/close and null the socket and then set state to disconnected.
            ExceptionUtil.ThrowIfNull(irc_user, nameof(irc_user));
            ExceptionUtil.ThrowIfInvalid(irc_user.nick, nameof(irc_user.nick));
            ExceptionUtil.ThrowIfInvalid(irc_user.pass, nameof(irc_user.pass));

            stream = new NetworkStream(socket);

            if (port == 443)
            {
                stream = new SslStream(stream, false);
                ((SslStream)stream).AuthenticateAsClient(host);
            }

            reader_thread = new Thread(new ThreadStart(ReadStream));
            reader_thread.Start();

            Send("PASS oauth:" + irc_user.pass);
            Send("NICK " + irc_user.nick);

            // NOTE: only for tetsing
            SetState(ClientState.Connected);
        }

        //private void
        //ForceDisconnect()
        //{
        //    if (!stream.IsNull())
        //    {
        //        stream.Close();
        //        stream = null;
        //    }

        //    if (!socket.IsNull())
        //    {
        //        if (socket.Connected)
        //        {
        //            socket.Shutdown(SocketShutdown.Both);
        //            socket.Disconnect(false);
        //        }
        //        socket = null;
        //    }

        //    OverrideState(ClientState.Disconnected);
        //}

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

            if (!result.IsCompleted)
            {
                socket = null;

                SetState(ClientState.Disconnected);
            }
            else
            {
                // is the socket still open and connected if it fails? reconnect if failed?
                // create a new stream
                // start the read stream thread again
                // set state back to connected
            }

            socket.EndDisconnect(result);
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

        private async void
        ReadStream()
        {
            int byte_count = 0;
            byte[] buffer = buffer = new byte[1024 * 2];

            reading = true;
            while (reading && !socket.IsNull() && !stream.IsNull())
            {
                try
                {
                    byte_count = await stream.ReadAsync(buffer, 0, 2048);
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

                if (!buffer.IsValid() || byte_count <= 0)
                {
                    continue;
                }

                do
                {
                    // 0x0A = '\n'
                    int byte_index = Array.IndexOf<byte>(buffer, 0x0A, 0, buffer.Length);
                    if (byte_index < 0)
                    {
                        break;
                    }

                    byte_index++;

                    string message = encoding.GetString(buffer, 0, byte_index - 2);
                    Log.PrintLine(message);
                    // ProcessIrcMessage(message);

                    Buffer.BlockCopy(buffer, byte_index, buffer, 0, byte_count - byte_index);
                    byte_count -= byte_index;
                }
                while (byte_count > 0);

                await Task.Delay(50);
            }
        }


        #endregion
    }
}
