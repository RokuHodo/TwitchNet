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
using TwitchNet.Debugger;
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

        private bool    reading;

        private ushort  _port;
        private string  _host;

        private Socket  socket;
        private Stream  stream;

        private Thread  reader_thread;

        private Mutex   state_mutex;

        #endregion

        #region Properties

        /// <summary>
        /// The port number of the remote host.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the port is null or equal to zero.</exception>
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

        /// <summary>
        /// The name of the remote host.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the host is null, empty, or whitespace.</exception>
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

        /// <summary>
        /// The IRC user's credentials.
        /// </summary>
        public IrcUser irc_user     { get; private set; }

        /// <summary>
        /// The current state of the IRC client.
        /// </summary>
        public ClientState state    { get; private set; }

        /// <summary>
        /// Determines whether or not to automatically respond to a PING with a PONG.
        /// </summary>
        public bool auto_pong       { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="IrcClient"/> class that is ready to connect to the IRC server.
        /// </summary>
        /// <param name="host">The name of the remote host.</param>
        /// <param name="port">The port number of the remote host.</param>
        /// <param name="irc_user">The IRC user's credentials.</param>
        public
        IrcClient(string host, ushort port, IrcUser user) : this()
        {
            this.host = host;
            this.port = port;

            irc_user = user;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="IrcClient"/> class.
        /// </summary>
        public
        IrcClient()
        {
            state_mutex = new Mutex();
            SetState(ClientState.Disconnected);

            reading = false;

            DefaultSettings();

            handlers = new Dictionary<string, MessageHandler>();
            names = new Dictionary<string, List<string>>();
            DefaultHandlers();
        }

        #endregion

        #region Settings methods

        /// <summary>
        /// Sets all client settings to their default values.
        /// </summary>
        public virtual void
        DefaultSettings()
        {
            auto_pong = false;
        }

        #endregion        

        #region Connection handling

        /// <summary>
        /// Establish a connection to a remote host using the <see cref="ProtocolType.Tcp"/> protocol and log into the IRC server.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the host is null, empty, or whitespace.</exception>
        /// <exception cref="ArgumentNullException">Thrown id the port is null or equal to zero.</exception>
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

        /// <summary>
        /// Asynchronously establish a connection to a rmeote host using the <see cref="ProtocolType.Tcp"/> protocol and log into the IRC server.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the host is null, empty, or whitespace.</exception>
        /// <exception cref="ArgumentNullException">Thrown id the port is null or equal to zero.</exception>
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

        /// <summary>
        /// Finish asynchronously connecting to a remote host.
        /// </summary>
        /// <param name="result">The result of the async operation.</param>
        private void
        Callback_OnBeginConnect(IAsyncResult result)
        {
            socket.EndConnect(result);

            Login();
        }

        /// <summary>
        /// Log into the IRC server.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the irc_user is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the nick or pass are null, empty, or whitespace.</exception>
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

            OnSocketConnected.Raise(this, EventArgs.Empty);

            reader_thread = new Thread(new ThreadStart(ReadStream));
            reader_thread.Start();

            do
            {
                Thread.Sleep(5);
            }
            while (!reading);

            Send("PASS oauth:" + irc_user.pass);
            Send("NICK " + irc_user.nick);
        }

        /// <summary>
        /// <para>Force closes the connection to the remote host without waiting for the reader thread to stop.</para>
        /// <para>
        /// This method is generally unsafe and should only be called before the socket connection is fully established and the reader thread is started.
        /// Otherwise, <see cref="Disconnect()"/> or <see cref="DisconnectAsync()"/> should be called.
        /// </para>
        /// </summary>
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

        /// <summary>
        /// Closes the connection from a remote host and disconnect from the IRC server.
        /// </summary>
        public void
        Disconnect()
        {
            if (!SetState(ClientState.Disconnecting))
            {
                return;
            }

            Quit();

            stream.Close();
            stream = null;

            socket.Shutdown(SocketShutdown.Both);
            socket.Disconnect(false);
            socket = null;

            OnSocketDisconnected.Raise(this, EventArgs.Empty);

            do
            {
                Thread.Sleep(5);
            }
            while (reading);

            SetState(ClientState.Disconnected);

            OnDisconnected.Raise(this, EventArgs.Empty);
        }

        /// <summary>
        /// Asynchronously closes the connection from a remote host and disconnect from the IRC server.
        /// </summary>
        public void
        DisconnectAsync()
        {
            if (!SetState(ClientState.Disconnecting))
            {
                return;
            }

            Quit();

            stream.Close();
            stream = null;

            socket.Shutdown(SocketShutdown.Both);
            socket.BeginDisconnect(false, Callback_OnBeginDisconnect, null);            
        }

        /// <summary>
        /// Finish asynchronously disconnecting from the remote host.
        /// </summary>
        /// <param name="result">The result of the async operation.</param>
        private void
        Callback_OnBeginDisconnect(IAsyncResult result)
        {
            OnSocketDisconnected.Raise(this, EventArgs.Empty);

            while (reading)
            {
                Thread.Sleep(5);
            }

            socket.EndDisconnect(result);
            socket = null;

            SetState(ClientState.Disconnected);

            OnDisconnected.Raise(this, EventArgs.Empty);
        }

        #endregion

        #region State handling

        /// <summary>
        /// <para>Overrides the client's state, regardless the client's current state or current operation.</para>
        /// <para>
        /// This is generally unsafe and should only be used when overriding the state won't cause adverse side effects to occur.
        /// Otherwise, use <see cref="SetState(ClientState)"/> to safely change the client state.
        /// </para>
        /// </summary>
        /// <param name="override_state"></param>
        private void
        OverrideState(ClientState override_state)
        {
            state_mutex.WaitOne();
            state = override_state;
            state_mutex.ReleaseMutex();
        }

        /// <summary>
        /// Sets the client's state.
        /// </summary>
        /// <param name="transition_state"></param>
        /// <returns></returns>
        private bool
        SetState(ClientState transition_state)
        {
            bool success = false;

            state_mutex.WaitOne();
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

            if (success)
            {
                state = transition_state;
            }
            state_mutex.ReleaseMutex();

            return success;
        }

        /// <summary>
        /// Checks to see if it is safe to connect.
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
                        Debug.WriteLine("Cannot connect to " + host + ": already connected");
                }
                break;

                case ClientState.Connecting:
                {
                        Debug.WriteLine("Cannot connect to " + host + ": currently connecting");
                }
                break;

                case ClientState.Disconnecting:
                {
                        Debug.WriteLine("Cannot connect to " + host + ": currently disconnecting");
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool
        CanDisconnect()
        {
            bool result = false;

            switch (state)
            {
                case ClientState.Connecting:
                {
                        Debug.WriteLine("Cannot disconnect from " + host + ": currently connecting");
                }
                break;

                case ClientState.Disconnecting:
                {
                        Debug.WriteLine("Cannot disconnect from " + host + ": already connecting");
                }
                break;

                case ClientState.Disconnected:
                {
                        Debug.WriteLine("Cannot disconnect from " + host + ": already disconnected");
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

        #region Command wrappers

        /// <summary>
        /// Sends a raw PONG command to the IRC server.
        /// </summary>
        /// <param name="trailing">The trailing paramater to attach to the message.</param>
        public void
        Pong(string trailing = "")
        {
            string message = "PONG";
            if (trailing.IsValid())
            {
                message += " :" + trailing;

            }

            Send(message);
        }

        /// <summary>
        /// Ends a client session with the IRC server.
        /// </summary>
        /// <param name="trailing">The trailing paramater to attach to the message.</param>
        public void
        Quit(string trailing = "")
        {
            string message = "QUIT";
            if (trailing.IsValid())
            {
                message += ": " + trailing;

            }

            Send(message);
        }

        // TODO: Manually track which channels you join/leave?

        /// <summary>
        /// Joins one or more IRC channels.
        /// </summary>
        /// <param name="channels">The IRC channel(s) to join.</param>
        /// <exception cref="ArgumentException">Thrown if the channel params are null or empty.</exception>
        public void
        Join(params string[] channels)
        {
            ExceptionUtil.ThrowIfInvalid(channels, nameof(channels));

            string format = channels.Length == 1 ? channels[0] : string.Join(",", channels);
            Send("JOIN " + format);
        }

        /// <summary>
        /// Leaves one or more IRC channels.        
        /// </summary>
        /// <param name="channels">The IRC channel(s) to leave.</param>
        /// <exception cref="ArgumentException">Thrown if the channel params are null or empty.</exception>
        public void
        Part(params string[] channels)
        {
            ExceptionUtil.ThrowIfInvalid(channels, nameof(channels));

            string format = channels.Length == 1 ? channels[0] : string.Join(",", channels);
            Send("PART " + format);
        }

        /// <summary>
        /// Sends a private message in an IRC channel.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        /// <param name="format">
        /// The message to send.
        /// This can be a normal string and does not need to include variable formats.
        /// </param>
        /// <param name="arguments">Optional format variable arugments.</param>
        /// <exception cref="ArgumentException">Thrown if the channel or format are null, empty, or whitespace.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void
        SendPrivmsg(string channel, string format, params object[] arguments)
        {
            ExceptionUtil.ThrowIfInvalid(channel, nameof(channel));
            ExceptionUtil.ThrowIfInvalid(format, nameof(format));

            string trailing = !arguments.IsValid() ? format : string.Format(format, arguments);
            Send("PRIVMSG " + channel + " :" + trailing);
        }

        #endregion

        #region Sending

        /// <summary>
        /// Sends a raw string.
        /// </summary>
        /// <param name="format">
        /// The string to send.
        /// This can be a normal string and does not need to include variable formats.
        /// </param>
        /// <param name="arguments">Optional format arugments.</param>
        /// <exception cref="ArgumentException">Thrown if the format is null, empty, or whitespace.</exception>
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

            OnDataSent.Raise(this, new DataEventArgs(bytes, message));
        }

        /// <summary>
        /// Asynchronously sends a raw string.
        /// </summary>
        /// <param name="format">
        /// The string to send.
        /// This can be a normal string and does not need to include variable formats.
        /// </param>
        /// <param name="arguments">Optional format arugments.</param>
        /// <exception cref="ArgumentException">Thrown if the format is null, empty, or whitespace.</exception>
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

            OnDataSent.Raise(this, new DataEventArgs(bytes, message));
        }

        /// <summary>
        /// Checks to see if a message can be sent.
        /// </summary>
        /// <param name="message">The message to check.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Reads and incoming data from the IRC via a <see cref="NetworkStream"/>.
        /// </summary>
        private async void
        ReadStream()
        {
            bool polling = true;

            int bytes_count = 0;
            byte[] buffer = new byte[1024];

            List<byte> data = new List<byte>();

            reading = true;
            while (polling && !socket.IsNull() && !stream.IsNull())
            {
                try
                {
                    bytes_count = await stream.ReadAsync(buffer, 0, buffer.Length);
                }
                catch (ObjectDisposedException exception)
                {
                    polling = false;

                    if(state != ClientState.Disconnecting)
                    {
                        OnNetworkError.Raise(this, new ErrorEventArgs(exception));
                    }

                    continue;
                }

                if (bytes_count == 0 || !buffer.IsValid())
                {
                    Exception exception = new Exception("Null or empty data received from the stream.");
                    OnNetworkError.Raise(this, new ErrorEventArgs(exception));

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

                        ProcessData(data.ToArray());
                        data.Clear();

                        // This doesn't take into account boundary cases where buffer[1023] == '\n' and then buffer[0] == '\r'.
                        // Need to "fix" this, although all it would do now is send a blank message.
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
                        data.Add(buffer[index]);
                    }
                }

                Array.Clear(buffer, 0, bytes_count);
            }

            reading = false;
        }

        /// <summary>
        /// Processes message encoded by the <see cref="NetworkStream"/>.
        /// </summary>
        /// <param name="data">The byte data received form the server.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void
        ProcessData(byte[] data)
        {
            if (!data.IsValid())
            {
                return;
            }

            string raw = Encoding.UTF8.GetString(data, 0, data.Length);
            if (!raw.IsValid())
            {
                return;
            }

            Debug.WriteLine(raw);
            OnDataReceived.Raise(this, new DataEventArgs(data, raw));

            IrcMessage irc_message = new IrcMessage(data, raw);
            if (!irc_message.command.IsValid())
            {
                return;
            }

            OnIrcMessageReceived.Raise(this, new IrcMessageEventArgs(irc_message));

            RunHandler(irc_message);
        }

        #endregion
    }
}
