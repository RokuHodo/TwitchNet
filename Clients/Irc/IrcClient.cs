// standard namespaces
using System;
using System.Collections.Generic;
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
using TwitchNet.Events.Clients.Irc;
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc
{
    public partial class
    IrcClient
    {
        #region Fields

        private bool                    reading;

        private ushort                  _port;

        private string                  _host;

        private Socket                  socket;
        private Stream                  stream;

        private Thread                  reader_thread;

        private Mutex                   state_mutex;

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

            state_mutex = new Mutex();

            names = new Dictionary<string, List<string>>();

            handlers = new Dictionary<string, MessageHandler>();
            DefaultHandlers();

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
            
            Send("PASS oauth:" + irc_user.pass);
            Send("NICK " + irc_user.nick);
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

            switch (state)
            {
                case ClientState.Connected:
                {
                    Log.PrintLine("Cannot connect to " + host + ": already connected");
                }
                break;

                case ClientState.Connecting:
                {
                    Log.PrintLine("Cannot connect to " + host + ": currently connecting");
                }
                break;

                case ClientState.Disconnecting:
                {
                    Log.PrintLine("Cannot connect to " + host + ": currently disconnecting");
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

            switch (state)
            {
                case ClientState.Connecting:
                {
                    Log.PrintLine("Cannot disconnect from " + host + ": currently connecting");
                }
                break;

                case ClientState.Disconnecting:
                {
                    Log.PrintLine("Cannot disconnect from " + host + ": already connecting");
                }
                break;

                case ClientState.Disconnected:
                {
                    Log.PrintLine("Cannot disconnect from " + host + ": already disconnected");
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

            switch (state)
            {
                case ClientState.Connecting:
                {
                    Log.PrintLine("Cannot reconnect to " + host + ": currently connecting");
                }
                break;

                case ClientState.Disconnecting:
                {
                    Log.PrintLine("Cannot reconnect to " + host + ": currently discnnecting");
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
        Pong()
        {
            Send("PONG");
        }

        public async Task
        PongAsync()
        {
            await SendAsync("PONG");
        }

        public void
        Pong(string trailing)
        {
            ExceptionUtil.ThrowIfInvalid(trailing, nameof(trailing));

            Send("PONG :" + trailing);
        }

        public async Task
        PongAsync(string trailing)
        {
            ExceptionUtil.ThrowIfInvalid(trailing, nameof(trailing));

            await SendAsync("PONG :" + trailing);
        }

        public void
        Pong(IrcMessage message)
        {
            ExceptionUtil.ThrowIfNull(message, nameof(message));
            ExceptionUtil.ThrowIfInvalid(message.trailing, nameof(message.trailing));

            Send("PONG :" + message.trailing);
        }

        public async void
        PongAsync(IrcMessage message)
        {
            ExceptionUtil.ThrowIfNull(message, nameof(message));
            ExceptionUtil.ThrowIfInvalid(message.trailing, nameof(message.trailing));

            await SendAsync("PONG :" + message.trailing);
        }

        public void
        Join(params string[] channels)
        {
            ExceptionUtil.ThrowIfInvalid(channels, nameof(channels));

            string format = channels.Length == 1 ? channels[0] : string.Join(",", channels);
            Send("JOIN " + format.ToLower());
        }

        public async void
        JoinAsync(params string[] channels)
        {
            ExceptionUtil.ThrowIfInvalid(channels, nameof(channels));

            string format = channels.Length == 1 ? channels[0] : string.Join(",", channels);
            await SendAsync("JOIN " + format.ToLower());
        }

        public void
        Part(params string[] channels)
        {
            ExceptionUtil.ThrowIfInvalid(channels, nameof(channels));

            string format = channels.Length == 1 ? channels[0] : string.Join(",", channels);
            Send("PART " + format.ToLower());
        }

        public async void
        PartAsync(params string[] channels)
        {
            ExceptionUtil.ThrowIfInvalid(channels, nameof(channels));

            string format = channels.Length == 1 ? channels[0] : string.Join(",", channels);
            await SendAsync("PART " + format.ToLower());
        }

        public void
        SendPrivmsg(string channel, string format, params string[] arguments)
        {
            ExceptionUtil.ThrowIfInvalid(channel, nameof(channel));
            ExceptionUtil.ThrowIfInvalid(format, nameof(format));

            string trailing = !arguments.IsValid() ? format : string.Format(format, arguments);
            Send("PRIVMSG " + channel + " :" + trailing);
        }

        public async void
        SendPrivmsgAsync(string channel, string format, params string[] arguments)
        {
            ExceptionUtil.ThrowIfInvalid(channel, nameof(channel));
            ExceptionUtil.ThrowIfInvalid(format, nameof(format));

            string trailing = !arguments.IsValid() ? format : string.Format(format, arguments);
            await SendAsync("PRIVMSG " + channel + " :" + trailing);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void
        Send(string format, params object[] arguments)
        {
            ExceptionUtil.ThrowIfInvalid(format, nameof(format));

            string message = !arguments.IsValid() ? format : string.Format(format, arguments);
            if (!CanSend(message))
            {
                return;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(message + "\r\n");            
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async Task
        SendAsync(string format, params object[] arguments)
        {
            ExceptionUtil.ThrowIfInvalid(format, nameof(format));

            string message = !arguments.IsValid() ? format : string.Format(format, arguments);
            if (!CanSend(message))
            {
                return;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(message + "\r\n");
            await stream.WriteAsync(bytes, 0, bytes.Length);
            stream.Flush();
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

        #region Reading and processing        

        private async void
        ReadStream()
        {
            int bytes_count = 0;
            byte[] buffer = new byte[1024];

            List<byte> line_bytes = new List<byte>();

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

                for(int index = 0; index < buffer.Length; ++index)
                {
                    if (buffer[index] == 0x0)
                    {
                        continue;
                    }                                       

                    // 0x0D = '\r', 0x0A = '\n'
                    if (buffer[index] == 0x0D || buffer[index] == 0x0A)
                    {
                        string message_raw = Encoding.UTF8.GetString(line_bytes.ToArray(), 0, line_bytes.Count);
                        line_bytes.Clear();

                        ProcessMessage(message_raw);

                        int next_index = index + 1;
                        if (buffer[index] == 0x0D && next_index < buffer.Length)
                        {
                            if(buffer[next_index] == 0x0A)
                            {
                                index++;
                            }
                        }
                    }
                    else
                    {
                        line_bytes.Add(buffer[index]);
                    }
                }

                Array.Clear(buffer, 0, bytes_count);
            }
        }

        private void
        ProcessMessage(string message_raw)
        {
            if (!message_raw.IsValid())
            {
                return;
            }

            IrcMessage message_irc = new IrcMessage(message_raw);
            if (!message_irc.command.IsValid())
            {
                return;
            }

            OnIrcMessage.Raise(this, new IrcMessageEventArgs(message_irc));

            if (message_irc.command != "PRIVMSG")
            {
                Log.PrintLine(message_irc.raw);
            }

            RunHandler(message_irc);
        }

        #endregion
    }
}
