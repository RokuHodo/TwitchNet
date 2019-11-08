// standard namespaces
using System;
using System.Collections;
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
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc
{
    public struct
    IrcUser
    {
        private string _nick;
        private string _pass;

        /// <summary>
        /// <para>The IRC user's nick.</para>
        /// <para>The IRC nick must between 2 and 24 characters long and can only contain alpha-numeric characters.</para>
        /// </summary>
        /// <exception cref="FormatException">Thrown when setting the value if the string length is not between 2 and 24 characters long or contians any non-alpha-numeric characters.</exception>
        public string nick
        {
            get
            {
                return _nick;
            }
            set
            {
                ExceptionUtil.ThrowIfInvalidNick(value);
                _nick = value;
            }
        }

        /// <summary>
        /// The IRC user's password. This is tyically an OAuth token.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the pass is null, emtpy, or contains only whitespace.</exception>
        public string pass
        {
            get
            {
                return _pass;
            }
            set
            {
                ExceptionUtil.ThrowIfInvalid(value, nameof(_pass));
                _pass = value;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="IrcUser"/> struct.
        /// </summary>
        /// <param name="nick">The IRC user's nick.</param>
        /// <param name="pass">The IRC user's pass.</param>
        public IrcUser(string nick, string pass)
        {
            _nick = nick;
            _pass = pass;

            this.nick = _nick;
            this.pass = _pass;
        }
    }

    public partial class
    IrcClient : IDisposable
    {
        #region Fields

        private volatile bool   polling;
        private volatile bool   reading;

        private bool            disposing;
        private bool            disposed;

        private ushort          _port;
        private string          _host;

        private Socket          socket;
        private Stream          stream;

        private Encoding        _encoding;

        private Thread          reader_thread;

        private Mutex           state_mutex;

        #endregion

        #region Properties

        /// <summary>
        /// The port number of the remote host.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the port is equal to zero.</exception>
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
        /// <exception cref="ArgumentNullException">Thrown if the host is null, empty, or contains only whitespace.</exception>
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
        /// The IRC user's login credentials.
        /// </summary>
        public IrcUser irc_user     { get; private set; }

        /// <summary>
        /// The current state of the IRC client.
        /// </summary>
        public ClientState state    { get; private set; }

        /// <summary>
        /// <para>Determines whether or not to automatically respond to a PING with a PONG.</para>
        /// <para>Default: <see cref="true"/>.</para>
        /// </summary>
        public bool auto_pong       { get; set; }

        /// <summary>
        /// <para>The type of encoding to be used when reading and writing data.</para>
        /// <para>Default: <see cref="Encoding.UTF8"/>.</para>
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the encoding is set to null.</exception>
        public Encoding encoding
        {
            get
            {
                return _encoding;
            }
            set
            {
                ExceptionUtil.ThrowIfNullOrDefault(value, nameof(encoding));
                _encoding = value;
            }
        }

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

            reading     = false;
            disposing   = false;

            // The only *internal* difference between being disposed and not disposed is whether the client maintains its state, for now.
            // When the client disconnects, the socket and stream are both disposed of automatically, but the state mutex remains. 
            // This may change at some point, but for now, this is how it works.
            disposed    = false;    

            ResetSettings();            
            ResetHandlers();
        }

        #endregion

        #region Settings methods

        /// <summary>
        /// Sets all client settings to their default values.
        /// </summary>
        public virtual void
        ResetSettings()
        {
            auto_pong = true;

            encoding = Encoding.UTF8;
        }

        #endregion        

        #region Connection and state handling

        /// <summary>
        /// Establish a connection to a remote host using the <see cref="ProtocolType.Tcp"/> protocol and log into the IRC server.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the host is null, empty, or contains only whitespace.</exception>
        /// <exception cref="ArgumentNullException">Thrown id the port is null or equal to zero.</exception>
        public void
        Connect()
        {
            if (!SetState(ClientState.Connecting))
            {
                return;
            }

            ExceptionUtil.ThrowIfInvalid(host, nameof(host), Callback_InternalFailedToConnect);
            ExceptionUtil.ThrowIfNullOrDefault(port, nameof(port), Callback_InternalFailedToConnect);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(host, port);

            Login();
        }

        /// <summary>
        /// Asynchronously establish a connection to a rmeote host using the <see cref="ProtocolType.Tcp"/> protocol and log into the IRC server.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the host is null, empty, or contains whitespace.</exception>
        /// <exception cref="ArgumentNullException">Thrown id the port is null or equal to zero.</exception>
        public void
        ConnectAsync()
        {
            if (!SetState(ClientState.Connecting))
            {
                return;
            }

            ExceptionUtil.ThrowIfInvalid(host, nameof(host), Callback_InternalFailedToConnect);
            ExceptionUtil.ThrowIfNullOrDefault(port, nameof(port), Callback_InternalFailedToConnect);

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
        /// <exception cref="ArgumentException">Thrown if the nick or pass are null, empty, or contains only whitespace.</exception>
        private void
        Login()
        {
            ExceptionUtil.ThrowIfInvalid(irc_user.nick, nameof(irc_user.nick), Callback_InternalFailedToConnect);
            ExceptionUtil.ThrowIfInvalid(irc_user.pass, nameof(irc_user.pass), Callback_InternalFailedToConnect);

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
                Thread.Sleep(1);
            }
            while (!reading);

            Send("PASS oauth:" + irc_user.pass);
            Send("NICK " + irc_user.nick);
        }

        /// <summary>
        /// Internally force disconnects.
        /// </summary>
        private void
        Callback_InternalFailedToConnect()
        {
            ForceDisconnect(true);
        }

        /// <summary>
        /// Logs out from the IRC server and closes the connection from the remote host, regardless of the current state..
        /// </summary>
        /// <param name="reuse_client">
        /// <para>Allows reuse of the client.</para>
        /// <para>
        /// When set to true, the client will maintain its state and is ready to reconnect.
        /// When set to false, all managed resources will be freed the client will need to be re-instantiated to reconnect.
        /// </para>
        /// </param>
        public void
        ForceDisconnect(bool reuse_client = false)
        {
            Disconnect(true, reuse_client);
        }

        /// <summary>
        /// Logs out from the IRC server and closes the connection from the remote host.
        /// </summary>
        /// <param name="reuse_client">
        /// <para>Allows reuse of the client.</para>
        /// <para>
        /// When set to true, the client will maintain its state and is ready to reconnect.
        /// When set to false, all managed resources will be freed the client will need to be re-instantiated to reconnect.
        /// </para>
        /// </param>
        public void
        Disconnect(bool reuse_client = false)
        {
            Disconnect(false, reuse_client);
        }

        /// <summary>
        /// Logs out from the IRC server and closes the connection from the remote host.
        /// </summary>
        /// <param name="force_disconnect">Whether or not to disconnect regardless of the current state.</param>
        /// <param name="reuse_client">
        /// <para>Whether or not to dispose of all managed resources.</para>
        /// <para>
        /// When set to true, the client will maintain its state and is ready to reconnect.
        /// When set to false, all managed resources will be freed the client will need to be re-instantiated to reconnect.
        /// </para>
        /// </param>
        public void
        Disconnect(bool force_disconnect, bool reuse_client = false)
        {
            if (!SetState(ClientState.Disconnecting, force_disconnect))
            {
                return;
            }

            Quit();            

            while (reading)
            {
                Thread.Sleep(1);
            }

            stream.Dispose();
            stream = null;

            socket.Shutdown(SocketShutdown.Both);
            socket.Disconnect(false);
            socket.Close();
            socket = null;

            OnSocketDisconnected.Raise(this, EventArgs.Empty);

            Dispose(!reuse_client);

            SetState(ClientState.Disconnected);
            OnDisconnected.Raise(this, EventArgs.Empty);
        }

        /// <summary>
        /// Asynchronously logs out from the IRC server and closes the connection from the remote host, regardless of the current state.
        /// </summary>
        /// <param name="reuse_client">
        /// <para>Allows reuse of the client.</para>
        /// <para>
        /// When set to true, the client will maintain its state and is ready to reconnect.
        /// When set to false, all managed resources will be freed the client will need to be re-instantiated to reconnect.
        /// </para>
        /// </param>
        private void
        ForceDisconnectAsync(bool reuse_client = false)
        {
            DisconnectAsync(true, reuse_client);
        }

        /// <summary>
        /// Asynchronously logs out from the IRC server and closes the connection from the remote host.
        /// </summary>
        /// <param name="reuse_client">
        /// <para>Allows reuse of the client.</para>
        /// <para>
        /// When set to true, the client will maintain its state and is ready to reconnect.
        /// When set to false, all managed resources will be freed the client will need to be re-instantiated to reconnect.
        /// </para>
        /// </param>
        public void
        DisconnectAsync(bool reuse_client = false)
        {
            DisconnectAsync(false, reuse_client);
        }

        /// <summary>
        /// Asynchronously closes the connection from a remote host and disconnect from the IRC server.
        /// </summary>
        /// <param name="force_disconnect">Whether or not to disconnect regardless of the current state.</param>
        /// <param name="reuse_client">
        /// <para>Allows reuse of the client.</para>
        /// <para>
        /// When set to true, the client will maintain its state and is ready to reconnect.
        /// When set to false, all managed resources will be freed the client will need to be re-instantiated to reconnect.
        /// </para>
        /// </param>
        public void
        DisconnectAsync(bool force_disconnect, bool reuse_client = false)
        {
            if (!SetState(ClientState.Disconnecting, force_disconnect))
            {
                return;
            }

            Quit();

            while (reading)
            {
                Task.Delay(1);
            }

            socket.Shutdown(SocketShutdown.Both);
            socket.BeginDisconnect(false, Callback_OnBeginDisconnect, Tuple.Create(force_disconnect, reuse_client));            
        }        

        /// <summary>
        /// Finish asynchronously disconnecting from the remote host.
        /// </summary>
        /// <param name="result">The result of the async operation.</param>
        private void
        Callback_OnBeginDisconnect(IAsyncResult result)
        {
            stream.Close();
            stream = null;

            socket.EndDisconnect(result);
            socket.Close();
            socket = null;

            OnSocketDisconnected.Raise(this, EventArgs.Empty);            

            Tuple<bool, bool> tuple = (Tuple<bool, bool>)result.AsyncState;

            Dispose(!tuple.Item2);
            SetState(ClientState.Disconnected, tuple.Item1);

            OnDisconnected.Raise(this, EventArgs.Empty);
        }

        /// <summary>
        /// <para>Force disconnects and frees all managed resources. The client will need to be re-instantiated to reconnect.</para>
        /// <para>
        /// Calling this method directly is not recommended.
        /// Call <see cref="Disconnect(bool)"/> or <see cref="DisconnectAsync(bool)"/> to safely disconnect and dispose all resources.
        /// If this method is called, it should only be done after disconnecting from the IRC server.
        /// </para>
        /// </summary>
        public void
        Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// <para>Force disconnects and frees all managed resources. The client will need to be re-instantiated to reconnect.</para>
        /// </summary>
        /// <param name="dispose">Whether or not to dispose all managed resources.</param>
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
            ForceDisconnect();

            if (!stream.IsNull())
            {
                stream.Dispose();
                stream = null;
            }

            if (!socket.IsNull())
            {
                if (socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Disconnect(false);
                }
                socket.Close();
                socket = null;
            }

            // This is the last step before the client fully disconnects, it's safe to call this here. 
            SetState(ClientState.Disconnected, true);

            if (!state_mutex.IsNull())
            {
                state_mutex.Dispose();
                state_mutex = null;
            }

            disposed = true;
            OnDisposed.Raise(this, EventArgs.Empty);
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
                // The only time the mutex is null is when the client is disposed.
                // In that case we are 100% guaranteed to be disconnected so it's safe to set the state without the mutex.
                // And because the mutext is disposed and we can't even use it, duh.
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
                if(state == ClientState.Connecting)
                {
                    polling = true;
                }
                else if(state == ClientState.Disconnecting)
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

        #region Writing

        /// <summary>
        /// Sends a PONG command to the IRC server.
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

        /// <summary>
        /// Joins one or more IRC channels.
        /// </summary>
        /// <param name="channels">
        /// The IRC channel(s) to join.      
        /// Each channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel params are null or empty.</exception>
        public bool
        Join(params string[] channels)
        {
            ExceptionUtil.ThrowIfInvalid(channels, nameof(channels));

            string format = channels.Length == 1 ? channels[0] : string.Join(",", channels);
            return Send("JOIN " + format);
        }

        /// <summary>
        /// Leaves one or more IRC channels.        
        /// </summary>
        /// <param name="channels">
        /// The IRC channel(s) to leave.
        /// Each channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel params are null or empty.</exception>
        public bool
        Part(params string[] channels)
        {
            ExceptionUtil.ThrowIfInvalid(channels, nameof(channels));

            string format = channels.Length == 1 ? channels[0] : string.Join(",", channels);
            return Send("PART " + format);
        }

        /// <summary>
        /// Sends a private message in an IRC channel.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <param name="format">
        /// The string to send without the CR-LF (\r\n), the CR-LF is automatically appended.
        /// This can be a normal string and does not need to include variable formats.
        /// </param>
        /// <param name="arguments">Optional format variable arugments.</param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel or format are null, empty, or contains only whitespace.</exception>
        public bool
        SendPrivmsg(string channel, string format, params object[] arguments)
        {
            ExceptionUtil.ThrowIfInvalid(channel, nameof(channel));
            ExceptionUtil.ThrowIfInvalid(format, nameof(format));

            return Send("PRIVMSG " + channel + " :" + format, arguments);
        }

        /// <summary>
        /// Sends a message to the IRC server.
        /// </summary>
        /// <param name="format">
        /// The string to send without the CR-LF (\r\n), the CR-LF is automatically appended.
        /// This can be a normal string and does not need to include variable formats.
        /// </param>
        /// <param name="arguments">Optional format arugments.</param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        public bool
        Send(string format, params object[] arguments)
        {
            string message = !arguments.IsValid() ? format : string.Format(format, arguments);
            message = message.Trim();
            if (!CanSend(message))
            {
                return false;
            }

            byte[] bytes = encoding.GetBytes(message + "\r\n");            
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();

            OnDataSent.Raise(this, new DataEventArgs(bytes, message));

            return true;
        }


        /// <summary>
        /// Asynchronously sends a message to the IRC server.
        /// </summary>
        /// <param name="format">
        /// The string to send without the CR-LF (\r\n), the CR-LF is automatically appended.
        /// This can be a normal string and does not need to include variable formats.
        /// </param>
        /// <param name="arguments">Optional format arugments.</param>        
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        public async Task<bool>
        SendAsync(string format, params object[] arguments)
        {
            string message = !arguments.IsValid() ? format : string.Format(format, arguments);
            message = message.Trim();
            if (!CanSend(message))
            {
                return false;
            }

            byte[] bytes = encoding.GetBytes(message + "\r\n");
            await stream.WriteAsync(bytes, 0, bytes.Length);
            stream.Flush();

            OnDataSent.Raise(this, new DataEventArgs(bytes, message));

            return true;
        }

        /// <summary>
        /// Checks to see if a message can be sent.
        /// </summary>
        /// <param name="message">The message to check.</param>
        /// <returns>
        /// Returns false if the IRc client is disposed.
        /// Returns false if socket is not connected or is null.
        /// Returns false if the stream is null.
        /// Returns false if message length is more than 510 bytes, excluding the mandatory CR-LF (\r\n).
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool
        CanSend(string message)
        {
            bool result = true;

            if (disposed)
            {
                result = false;
            }
            else if (!socket.Connected || socket.IsNull())
            {
                result = false;
            }
            else if (stream.IsNull())
            {
                result = false;
            }
            // 510 instead of 512 since two bytes are reserved for the \r\n appended to the message
            else if (message.Length > 510)
            {
                result = false;
            }

            return result;
        }

        #endregion

        #region Reading

        /// <summary>
        /// Reads and incoming data from the IRC via a <see cref="NetworkStream"/>.
        /// </summary>
        private async void
        ReadStream()
        {
            byte[]      buffer                  = new byte[1024];
            int         buffer_index_peek       = 0;
            int         buffer_bytes_read_count = 0;

            List<byte>  data                    = new List<byte>(buffer.Length);

            byte[]      message_bytes           = new byte[buffer.Length];
            string      message_string          = string.Empty;
            IrcMessage  message_irc             = default;

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

                    OnNetworkError.Raise(this, new ErrorEventArgs(exception));

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

                        message_string = encoding.GetString(message_bytes, 0, message_bytes.Length);
                        Debug.WriteLine(message_string);

                        OnDataReceived.Raise(this, new DataEventArgs(message_bytes, message_string));

                        message_irc = new IrcMessage(message_bytes, message_string);
                        if (message_irc.command.Length == 0)
                        {
                            continue;
                        }

                        OnIrcMessageReceived.Raise(this, new IrcMessageEventArgs(message_irc));
                        RunHandler(message_irc);

                        data.Clear();

                        buffer_index_peek = index + 1;
                        if (buffer[index] == 0x0D && buffer_index_peek < buffer.Length)
                        {
                            if(buffer[buffer_index_peek] == 0x0A)
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

    public readonly struct
    IrcMessage
    {
        #region Properties

        /// <summary>
        /// The byte data received from the socket.
        /// </summary>
        public readonly byte[]                      data;

        /// <summary>
        /// The UTF-8 encoded byte data received from the socket.
        /// </summary>
        public readonly string                      raw;

        /// <summary>
        /// Whether or not tags were sent with the message.
        /// </summary>
        public readonly bool                        tags_exist;

        /// <summary>
        /// The optional tags prefixed to the message.
        /// </summary>
        public readonly IrcTags                     tags;

        /// <summary>
        /// An optional part of the message.
        /// If the prefix is provided, the server name or nick is always provided, and the user and/or host may also be included.
        /// </summary>
        public readonly string                      prefix;

        /// <summary>
        /// The server name or the nick of the user.
        /// Contained within the prefix.
        /// </summary>
        public readonly string                      server_or_nick;

        /// <summary>
        /// The IRC user.
        /// Contained within the prefix.
        /// </summary>
        public readonly string                      user;

        /// <summary>
        /// The host of the IRC.
        /// Contained within the prefix.
        /// </summary>
        public readonly string                      host;

        /// <summary>
        /// The IRC command.
        /// </summary>
        public readonly string                      command;

        /// <summary>
        /// A message parameter.
        /// Any, possibly empty, sequence of octets not including NUL or CR or LF.
        /// </summary>
        public readonly string                      trailing;

        /// <summary>
        /// An array of message parameters.
        /// Any non-empty sequence of octets not including SPACE or NUL or CR or LF.
        /// </summary>
        public readonly string[]                    middle;

        /// <summary>
        /// An array of all middle parameters and trailing.
        /// </summary>
        public readonly string[]                    parameters;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="IrcMessage"/> class.
        /// </summary>
        /// <param name="data">The data received from the socket.</param>
        /// <param name="raw">The UTF-8 encoded byte data received from the socket.</param>
        public IrcMessage(byte[] data, string raw)
        {
            this.data                   = data;
            this.raw                    = raw;

            tags_exist                  = false;
            tags                        = new IrcTags();

            prefix                      = string.Empty;
            server_or_nick              = string.Empty;
            user                        = string.Empty;
            host                        = string.Empty;

            command                     = string.Empty;

            middle                      = new string[0];
            trailing                    = string.Empty;
            parameters                  = new string[0];

            string message_post_tags    = ParseTags(raw, ref tags, ref tags_exist);
            string message_post_prefix  = ParsePrefix(message_post_tags, ref prefix, ref server_or_nick, ref user, ref host);
            string message_post_command = ParseCommand(message_post_prefix, ref command);

            parameters = ParseParameters(message_post_command, ref middle, ref trailing);
        }

        #endregion        

        #region Parsing

        /// <summary>
        /// Parses an irc message for tags, if present.
        /// </summary>
        /// <param name="message">The irc message to parse.</param>
        /// <returns>Returns the irc message after the tags.</returns>
        private string
        ParseTags(string message, ref IrcTags tags, ref bool tags_exist)
        {
            // IRC messages only conmtain tags when it is prefixed with "@"
            if (message[0] != '@')
            {
                return message;
            }

            tags_exist = true;
            tags = new IrcTags(ref message);

            // Get rid of the tags to make later parsing easier
            return message.TextAfter(' ').TrimStart(' ');
        }

        /// <summary>
        /// Parses an irc message for the prefix, if present.
        /// </summary>
        /// <param name="message_post_tags">The irc message after the tags.</param>
        /// <returns>Returns the irc message after the prefix.</returns>
        public string
        ParsePrefix(string message_post_tags, ref string prefix, ref string server_or_nick, ref string user, ref string host)
        {
            if (message_post_tags.Length == 0)
            {
                return string.Empty;
            }

            if (message_post_tags[0] != ':')
            {
                return message_post_tags;
            }

            prefix = message_post_tags.TextBetween(':', ' ');

            int user_index = prefix.IndexOf('!');
            int host_index = prefix.IndexOf('@');

            if (user_index < 0 && host_index < 0)
            {
                server_or_nick = prefix;
            }
            else if (user_index != -1 && host_index < 0)
            {
                server_or_nick  = prefix.TextBefore('!');
                user            = prefix.TextAfter('!');
            }
            else
            {
                server_or_nick  = prefix.TextBefore('!');
                user            = prefix.TextBetween('!', '@');
                host            = prefix.TextAfter('@');
            }

            return message_post_tags.TextAfter(' ').TrimStart(' ');
        }

        /// <summary>
        /// Parses an irc message for the commmand.
        /// </summary>
        /// <param name="message_post_prefix">The irc message after the prefix.</param>
        /// <returns>Returns the irc message after the command.</returns>
        private string
        ParseCommand(string message_post_prefix, ref string command)
        {
            if (message_post_prefix.Length == 0)
            {
                return string.Empty;
            }

            command = message_post_prefix.TextBefore(' ');
            if (command.Length == 0)
            {
                //If there's no space after the command, it's the end of the message
                command = message_post_prefix;

                return string.Empty;
            }

            return message_post_prefix.TextAfter(' ').TrimStart(' ');
        }

        /// <summary>
        /// Parses an irc message for the parameters (middle and trailing).
        /// </summary>
        /// <param name="message_post_command">The irc message after the command.</param>
        /// <returns>Returns an middle array of parameters.</returns>
        private string[]
        ParseParameters(string message_post_command, ref string[] middle, ref string trailing)
        {
            if(message_post_command.Length == 0)
            {
                return new string[0];
            }

            string[] temp = message_post_command.Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries);    
            if(temp.Length == 0)
            {
                return new string[0];
            }

            string[] parameters;

            middle = temp[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length > 1)
            {
                trailing = temp[1].Trim();

                parameters = new string[middle.Length + 1];
                Array.Copy(middle, parameters, middle.Length);
                parameters[parameters.Length - 1] = trailing;
            }
            else
            {
                parameters = middle;
            }

            return parameters;
        }

        #endregion
    }

    public readonly struct
    IrcTags : IEnumerable
    {
        private readonly IrcTag[] tags;

        /// <summary>
        /// The number of pased tags.
        /// </summary>
        public readonly int count;

        /// <summary>
        /// The parsed tag keys.
        /// </summary>
        public readonly string[] keys;

        /// <summary>
        /// The parsed tag values.
        /// </summary>
        public readonly string[] values;

        public
        IrcTags(ref string message)
        {
            if(message.IsNull() || message[0] != '@')
            {
                count = 0;

                tags = new IrcTag[0];
                keys = new string[0];
                values = new string[0];

                return;
            }

            string[] pairs = message.TextBetween('@', ' ').Split(';');

            count = pairs.Length;

            tags = new IrcTag[pairs.Length];
            keys = new string[pairs.Length];
            values = new string[pairs.Length];

            // Right now the parser assumes that there will never be be a tag with an empty key.
            // This should always be the case, but account for the possibility of it happening?
            // Otherwise, there will be empty indecies when that happens which would be bad.
            foreach (string pair in pairs)
            {
                string[] tag = pair.Split('=');
                if (tag.Length == 0 || !tag[0].HasContent())
                {
                    continue;
                }

                SetValue(tag[0], tag[1]);
            }
        }

        /// <summary>
        /// Gets the value of a tag key.
        /// </summary>
        /// <param name="key">The name of the key to get.</param>
        /// <returns>Returns the value of the tag.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the tag key could not be found.</exception>
        public string this[string key]
        {
            get
            {
                return GetValue(key);
            }
            private set
            {
                SetValue(key, value);
            }
        }

        /// <summary>
        /// Gets the value of a tag key.
        /// </summary>
        /// <param name="key">The name of the key to get.</param>
        /// <returns>Returns the value of the tag.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the tag key could not be found.</exception>
        private string
        GetValue(string key)
        {
            int index = GetIndex(key);
            if (index < 0)
            {
                throw new KeyNotFoundException("The tag " + key.WrapQuotes() + " could not be found.");
            }

            return tags[index].value;
        }

        /// <summary>
        /// Attempts to get the value of a tag key.
        /// </summary>
        /// <param name="key">The name of the key to get.</param>
        /// <param name="value">
        /// The value of the tag.
        /// Set to an empty string if the tag key could not be found.
        /// </param>
        /// <returns>
        /// Returns true of the tag key was found.
        /// Returns false otherwise.
        /// </returns>
        public bool
        TryGetValue(string key, out string value)
        {
            value = string.Empty;

            int index = GetIndex(key);
            if (index < 0)
            {
                return false;
            }

            value = tags[index].value;

            return true;
        }

        /// <summary>
        /// Set and/or adds the value of a tag.
        /// </summary>
        /// <param name="key">The tag key.</param>
        /// <param name="value">The tag value.</param>
        /// <exception cref="ArgumentException">Thrown if the tag key is null or an empty string.</exception>
        private void
        SetValue(string key, string value)
        {
            if (key.IsNull() || key.Length == 0)
            {
                throw new ArgumentException("The tag key cannot be null or an empty string.");
            }

            value = value ?? string.Empty;

            // First check to see if the key is overriding an existing key, just in case the IRC server messed up and sent duplicate tags.
            int hash_code = key.GetHashCode() & 0x7FFFFFFF;
            for (int index = 0; index < tags.Length; ++index)
            {
                if (hash_code == tags[index].hash_code && tags[index].key == key)
                {
                    tags[index] = new IrcTag(key, value, hash_code);
                    values[index] = value;

                    return;
                }

                if (tags[index].key.IsNull() || tags[index].key.Length == 0)
                {
                    tags[index] = new IrcTag(key, value, hash_code);
                    keys[index] = key;
                    values[index] = value;

                    return;
                }
            }
        }                

        /// <summary>
        /// Gets the index of a tag key.
        /// This represents the order at which the tag was parsed and added.
        /// </summary>
        /// <param name="key">The tag key.</param>
        /// <returns>
        /// Returns the index of the tag key if it was found.
        /// Returns false otherwsie.
        /// </returns>
        public int
        GetIndex(string key)
        {
            if(count == 0 || key.IsNull() || key.Length == 0)
            {
                return -1;
            }

            int hash_code = key.GetHashCode() & 0x7FFFFFFF;
            for(int index = 0; index < tags.Length; ++index)
            {
                if(tags[index].hash_code == hash_code && tags[index].key == key)
                {
                    return index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Whether or not the tag key exists.
        /// </summary>
        /// <param name="key">The tag key.</param>
        /// <returns>
        /// Returns true if the tag key was found.
        /// Returns false otherwsie.
        /// </returns>
        public bool
        ContainsKey(string key)
        {
            return GetIndex(key) > 0;
        }

        /// <summary>
        /// The enumerator for the <see cref="IrcTags"/>.
        /// Enambles the use of the foreach.
        /// </summary>
        /// <returns>Returns the enumerator for <see cref="IrcTags"/></returns>
        IEnumerator
        IEnumerable.GetEnumerator()
        {
            foreach (IrcTag tag in tags)
            {
                yield return tag;
            }
        }
    }

    public readonly struct
    IrcTag
    {
        /// <summary>
        /// The hash code of the tag key.
        /// </summary>
        public readonly int hash_code;

        /// <summary>
        /// The tag key.
        /// </summary>
        public readonly string key;

        /// <summary>
        /// The tag value.
        /// </summary>
        public readonly string value;

        public
        IrcTag(string key, string value, int hash_code)
        {
            this.key = key;
            this.value = value;

            this.hash_code = hash_code;
        }
    }
}