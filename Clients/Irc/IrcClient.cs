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
        /// The nick name of the Irc user.
        /// </summary>
        /// <exception cref="FormatException">Thrown if the string is not between 2 and 24 characters long, and does not only contian alpha-numeric characters.</exception>
        public string nick
        {
            get
            {
                ExceptionUtil.ThrowIfInvalidNick(_nick);
                return _nick;
            }
            set
            {
                _nick = value;
            }
        }

        /// <summary>
        /// The password of the Irc user.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the pass is null, emtpy, or whitespace.</exception>
        public string pass
        {
            get
            {
                ExceptionUtil.ThrowIfInvalid(_pass, nameof(_pass));
                return _pass;
            }
            set
            {
                _pass = value;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="IrcUser"/> struct.
        /// </summary>
        /// <param name="user_nick">The client nick.</param>
        /// <param name="user_pass">The client pass.</param>
        public IrcUser(string user_nick, string user_pass)
        {
            _nick = user_nick;
            _pass = user_pass;

            nick = _nick;
            pass = user_pass;
        }
    }

    public partial class
    IrcClient : IDisposable
    {
        #region Fields

        private bool    reading;
        private bool    disposing;
        private bool    disposed;

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

            reading     = false;
            disposing   = false;

            // The only *internal* difference between being disposed and not disposed is whether the client has state, for now.
            // When the client disconnects, the socket and stream are both disposed of automatically, but the state mutex remains. 
            // This may change at some point, but for now, this is how it works.
            disposed = false;    

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

            ExceptionUtil.ThrowIfInvalid(host, nameof(host), Callback_InternalFailedToConnect);
            ExceptionUtil.ThrowIfNullOrDefault(port, nameof(port), Callback_InternalFailedToConnect);

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
            socket. EndConnect(result);

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
            ExceptionUtil.ThrowIfNull(irc_user, nameof(irc_user), Callback_InternalFailedToConnect);
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
                Thread.Sleep(5);
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

            stream.Dispose();
            stream = null;

            socket.Shutdown(SocketShutdown.Both);
            socket.Disconnect(false);
            socket.Close();
            socket = null;

            OnSocketDisconnected.Raise(this, EventArgs.Empty);

            while (reading)
            {
                Thread.Sleep(5);
            }

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
        private void
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

            stream.Close();
            stream = null;

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
            OnSocketDisconnected.Raise(this, EventArgs.Empty);

            while (reading)
            {
                Thread.Sleep(5);
            }

            socket.EndDisconnect(result);
            socket.Close();
            socket = null;

            Tuple<bool, bool> tuple = (Tuple<bool, bool>)result.AsyncState;

            Dispose(!tuple.Item2);
            SetState(ClientState.Disconnected, tuple.Item1);

            OnDisconnected.Raise(this, EventArgs.Empty);
        }

        /// <summary>
        /// <para>Force disconnects and frees all managed resources. The client will need to be re-instantiated to reconnect.</para>
        /// <para>Calling this method directly not recommended. Call <see cref="Disconnect(bool)"/> or <see cref="DisconnectAsync(bool)"/> to safely disconnect and dispose all resources.</para>
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

        #endregion

        #region State handling

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
                    reading = false;

                    if(state != ClientState.Disconnecting)
                    {
                        OnNetworkError.Raise(this, new ErrorEventArgs(exception));
                    }

                    continue;
                }

                if (bytes_count == 0 || !buffer.IsValid())
                {
                    polling = false;
                    reading = false;

                    Exception exception = new Exception("Null or empty data received from the stream.");
                    OnNetworkError.Raise(this, new ErrorEventArgs(exception));

                    continue;
                }

                for (int index = 0; index < buffer.Length; ++index)
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
            if (data.Length == 0)
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
        public readonly Dictionary<string, string>  tags;

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
            tags                        = new Dictionary<string, string>();

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

            middle                      = ParseParameters(message_post_command, ref trailing).ToArray();
            parameters                  = AssembleParameters(middle, trailing);
        }

        #endregion        

        #region Parsing

        /// <summary>
        /// Parses an irc message for tags, if present.
        /// </summary>
        /// <param name="message">The irc message to parse.</param>
        /// <returns>Returns the irc message after the tags.</returns>
        private string
        ParseTags(string message, ref Dictionary<string, string> tags, ref bool tags_exist)
        {
            string message_no_tags = message;

            // irc message only conmtains tags when it is preceeded with "@"
            if (message[0] != '@')
            {
                return message_no_tags;
            }

            tags_exist = true;

            string all_tags = message.TextBetween('@', ' ');
            string[] array = all_tags.Split(';');
            foreach (string element in array)
            {
                string tag = element.TextBefore('=');
                string value = element.TextAfter('=');
                if (!tag.IsValid())
                {
                    continue;
                }

                tags[tag] = value;
            }

            // Get rid of the tags to make later parsing easier
            message_no_tags = message.TextAfter(' ').TrimStart(' ');

            return message_no_tags;
        }

        /// <summary>
        /// Parses an irc message for the prefix, if present.
        /// </summary>
        /// <param name="message_post_tags">The irc message after the tags.</param>
        /// <returns>Returns the irc message after the prefix.</returns>
        public string
        ParsePrefix(string message_post_tags, ref string prefix, ref string server_or_nick, ref string user, ref string host)
        {
            string message_post_prefix = string.Empty;

            if (!message_post_tags.IsValid())
            {
                return message_post_prefix;
            }

            if (message_post_tags[0] != ':')
            {
                message_post_prefix = message_post_tags;

                return message_post_prefix;
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

            message_post_prefix = message_post_tags.TextAfter(' ').TrimStart(' ');

            return message_post_prefix;
        }

        /// <summary>
        /// Parses an irc message for the commmand.
        /// </summary>
        /// <param name="message_post_prefix">The irc message after the prefix.</param>
        /// <returns>Returns the irc message after the command.</returns>
        private string
        ParseCommand(string message_post_prefix, ref string command)
        {
            string message_post_command = string.Empty;

            if (!message_post_prefix.IsValid())
            {
                return message_post_command;
            }

            command = message_post_prefix.TextBefore(' ');
            if (!command.IsValid())
            {
                //If there's no space after the command, it's the end of the message
                command = message_post_prefix;
            }
            else
            {
                message_post_command = message_post_prefix.TextAfter(' ').TrimStart(' ');
            }

            return message_post_command;
        }

        /// <summary>
        /// Parses an irc message for the parameters (middle and trailing).
        /// </summary>
        /// <param name="message_post_command">The irc message after the command.</param>
        /// <returns>Returns an middle array of parameters.</returns>
        private List<string>
        ParseParameters(string message_post_command, ref string trailing)
        {
            List<string> _middle = new List<string>();

            if (!message_post_command.IsValid())
            {
                return _middle;
            }

            if (message_post_command[0] == ':')
            {
                string parameter = message_post_command.TextAfter(':');

                trailing = parameter;
            }
            else
            {
                string parameter = message_post_command.TextBefore(' ');
                if (parameter.IsValid())
                {
                    _middle.Add(parameter);

                    message_post_command = message_post_command.TextAfter(' ').TrimStart(' ');

                    List<string> temp = ParseParameters(message_post_command, ref trailing);
                    if (temp.IsValid())
                    {
                        _middle.AddRange(temp);
                    }
                }
                else
                {
                    _middle.Add(message_post_command);
                }
            }

            return _middle;
        }

        /// <summary>
        /// Combines the middle and trailing into a single parameters array.
        /// </summary>
        /// <param name="middle">The array of middle parameters.</param>
        /// <param name="trailing">The trailing parameter.</param>
        /// <returns>The combined parameters array.</returns>
        private string[]
        AssembleParameters(string[] middle, string trailing)
        {
            List<string> parameters = new List<string>();

            foreach (string element in middle)
            {
                parameters.Add(element);
            }

            parameters.Add(trailing);

            return parameters.ToArray();
        }

        #endregion
    }
}