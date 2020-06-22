// standard namespaces
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Runtime.CompilerServices;
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
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.PubSub
{   
    public partial class
    WebSocketClient : IDisposable
    {
        protected volatile bool polling;
        protected volatile bool reading;
        protected volatile bool handshake_initiated;

        protected bool disposing;
        protected bool disposed;

        /// <summary>
        /// <para>The constant and universal UUID used in the handshake process to validate the "Sec-WebSocket-Accept" header.</para>
        /// <para>See https://tools.ietf.org/html/rfc6455#section-1.3 for more information.</para>
        /// </summary>
        public readonly string UUID;

        private Stream stream;
        private Socket socket;

        private Mutex state_mutex;

        private Thread reader_thread;

        /// <summary>
        /// The WebSocket URI.
        /// </summary>
        public Uri URI;

        /// <summary>
        /// A unique ID set by the user in order to differentiate one client from each other if multiple WebSocket clients use the same event callback functions.
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// The current state of the WebSocket client.
        /// </summary>
        public WebSocketState state { get; private set; }

        /// <summary>
        /// Information used to detemrtine how to handle errors, connection retry attempts, etc.
        /// </summary>
        public WebSocketSettings settings { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="WebSocketClient"/> class.
        /// </summary>
        /// <param name="id">A unique ID set by the user in order to differentiate one client from each other if multiple WebSocket clients use the same event callback functions.</param>
        public
        WebSocketClient(string id = "")
        {
            state_mutex = new Mutex();
            SetState(WebSocketState.Connecting);

            polling = false;
            reading = false;
            handshake_initiated = false;

            disposing = false;
            disposed = false;

            this.id = id ?? string.Empty;

            UUID = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

            settings = new WebSocketSettings();

            WebSocket.RegisterPrefixes();

            ResetWebSocketHandlers();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="WebSocketClient"/> class.
        /// </summary>
        /// <param name="uri">The WebSocket URI.</param>
        /// <param name="id">A unique ID set by the user in order to differentiate one client from each other if multiple WebSocket clients use the same event callback functions.</param>
        public
        WebSocketClient(Uri uri, string id = "") : this(id)
        {
            URI = uri;
        }

        #region Connection and Connection Validation

        /// <summary>
        /// Establishes a connection to the WebSocket.
        /// </summary>
        /// <exception cref="WebSocketException">
        /// Thrown if an error was encountered with the WebSocket URI, the handshake request, the handshake response, or if the connection retry limit was reached.
        /// Potential errors are:
        /// <see cref="WebSocketError.Handshake_Open_Uri"/>,
        /// <see cref="WebSocketError.Handshake_Open_UriPort"/>,
        /// <see cref="WebSocketError.Handshake_Open_UriHost"/>,
        /// <see cref="WebSocketError.Handshake_Open_RequestProtocolVersion"/>,
        /// <see cref="WebSocketError.Handshake_Open_RequestHeader"/>,
        /// <see cref="WebSocketError.Handshake_Open_ResponseStatusCode"/>.
        /// <see cref="WebSocketError.Handshake_Open_ResponseProtocolVersion"/>,
        /// <see cref="WebSocketError.Handshake_Open_ResponseHeader"/>, and
        /// <see cref="WebSocketError.Handshake_Open_RetryLimitReached"/>.
        /// See each error description for more informatrion.
        /// </exception>
        public void
        Connect()
        {
            if (settings.connect_retry_count > settings.connect_retry_count_limit)
            {
                return;
            }

            if (!SetState(WebSocketState.Connecting, true))
            {
                return;
            }

            WebSocketException exception;

            if (!ValidateURI(URI, out exception))
            {
                // No sense in retrying to connect since it will be guaranteed to fail 100% if any of these are true.
                // Just let the user know they fucked up and move on.
                SetState(WebSocketState.Connecting, false);
                RaiseAndMaybeThrowError(exception);

                return;
            }

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(URI.Host, URI.Port);

            stream = new NetworkStream(socket);
            if (URI.Port == 443)
            {
                SslStream ssl_stream = new SslStream(stream, false, Callback_CertificateValidation, Callback_CertificateSelection);
                ssl_stream.AuthenticateAsClient(URI.DnsSafeHost, null, SslProtocols.Default, false);

                stream = ssl_stream;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URI);
            if (!ValidateHandShakeRequest(request, out exception))
            {
                CallBack_FailedToConnect(exception);

                return;
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (!ValidateHandShakeResponse(request, response, UUID, out exception))
            {
                response.Close();
                response.Dispose();

                CallBack_FailedToConnect(exception);

                return;
            }

            settings.connect_retry_count = 0;

            stream = response.GetResponseStream();

            reader_thread = new Thread(new ThreadStart(ReadStream));
            reader_thread.Start();

            SetState(WebSocketState.Open);

            OnOpened.Raise(this, new WebSocketEventArgs(DateTime.Now, URI, id));
        }

        /// <summary>
        /// Asynchronouslyes establishes a connection to the WebSocket.
        /// </summary>
        /// <exception cref="WebSocketException">
        /// Thrown if an error was encountered with the WebSocket URI, the handshake request, the handshake response, or if the connection retry limit was reached.
        /// Potential errors are:
        /// <see cref="WebSocketError.Handshake_Open_Uri"/>,
        /// <see cref="WebSocketError.Handshake_Open_UriPort"/>,
        /// <see cref="WebSocketError.Handshake_Open_UriHost"/>,
        /// <see cref="WebSocketError.Handshake_Open_RequestProtocolVersion"/>,
        /// <see cref="WebSocketError.Handshake_Open_RequestHeader"/>,
        /// <see cref="WebSocketError.Handshake_Open_ResponseStatusCode"/>.
        /// <see cref="WebSocketError.Handshake_Open_ResponseProtocolVersion"/>,
        /// <see cref="WebSocketError.Handshake_Open_ResponseHeader"/>, and
        /// <see cref="WebSocketError.Handshake_Open_RetryLimitReached"/>.
        /// See each error description for more informatrion.
        /// </exception>
        public void
        ConnectAsync()
        {
            if (settings.connect_retry_count > settings.connect_retry_count_limit)
            {
                return;
            }

            if (!SetState(WebSocketState.Connecting, true))
            {
                return;
            }

            if (!ValidateURI(URI, out WebSocketException exception))
            {
                // No sense in retrying to connect since it will be guaranteed to fail 100% if any of these are true.
                // Just let the user know they fucked up and move on.
                SetState(WebSocketState.Connecting, false);
                RaiseAndMaybeThrowError(exception);

                return;
            }

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.BeginConnect(URI.DnsSafeHost, URI.Port, Callback_OnBeginConnect, null);
        }

        private async void
        Callback_OnBeginConnect(IAsyncResult result)
        {
            socket.EndConnect(result);

            stream = new NetworkStream(socket);
            if (URI.Port == 443)
            {
                SslStream ssl_stream = new SslStream(stream, false, Callback_CertificateValidation, Callback_CertificateSelection);
                ssl_stream.AuthenticateAsClient(URI.DnsSafeHost, null, SslProtocols.Default, false);

                stream = ssl_stream;
            }

            WebSocketException exception;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URI);
            if (!ValidateHandShakeRequest(request, out exception))
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.BeginDisconnect(false, CallBack_FailedToConnectAsync, exception);

                return;
            }

            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            if (!ValidateHandShakeResponse(request, response, UUID, out exception))
            {
                response.Close();
                response.Dispose();

                socket.Shutdown(SocketShutdown.Both);
                socket.BeginDisconnect(false, CallBack_FailedToConnectAsync, exception);

                return;
            }

            settings.connect_retry_count = 0;

            stream = response.GetResponseStream();

            reader_thread = new Thread(new ThreadStart(ReadStream));
            reader_thread.Start();

            SetState(WebSocketState.Open);

            OnOpened.Raise(this, new WebSocketEventArgs(DateTime.Now, URI, id));
        }

        private bool
        ValidateURI(Uri uri, out WebSocketException exception)
        {
            exception = default;

            if (URI.IsNull())
            {
                exception = new WebSocketException(WebSocketStatusCode.TlsHandshakeFailure, WebSocketError.Handshake_Open_Uri, "The WebSocket URI cannot be null or default.");

                return false;
            }

            if (URI.Host.IsNull())
            {
                exception = new WebSocketException(WebSocketStatusCode.TlsHandshakeFailure, WebSocketError.Handshake_Open_UriHost, "The WebSocket URI host cannot be null.");

                return false;
            }

            if (URI.Port < IPEndPoint.MinPort || URI.Port > IPEndPoint.MaxPort)
            {
                exception = new WebSocketException(WebSocketStatusCode.TlsHandshakeFailure, WebSocketError.Handshake_Open_UriHost, "The WebSocket URI port must be between " + IPEndPoint.MinPort + " and " + IPEndPoint.MaxPort + ", inclusive.");

                return false;
            }

            return true;
        }

        private bool
        ValidateHandShakeRequest(HttpWebRequest request, out WebSocketException exception)
        {
            exception = default;

            // These checks really shouldn't be needed, but just double check to make sure Microsoft didn't mess up.
            if (request.ProtocolVersion.Major < 1 && request.ProtocolVersion.Minor < 1)
            {
                exception = new WebSocketException(WebSocketStatusCode.TlsHandshakeFailure, WebSocketError.Handshake_Open_RequestProtocolVersion, "The handshake request protocol version must be at least HTTP 1.1.");

                return false;
            }

            string header_upgrade = request.Headers["Upgrade"];
            if (!header_upgrade.HasContent() || header_upgrade != "websocket")
            {
                exception = new WebSocketException(WebSocketStatusCode.TlsHandshakeFailure, WebSocketError.Handshake_Open_RequestHeader, "The handshake request did not contain the header \"Upgrade\", or the header was not set to \"websocket\".");

                return false;
            }

            string header_connection = request.Headers["Connection"];
            if (!header_connection.HasContent() || header_connection != "Upgrade")
            {
                exception = new WebSocketException(WebSocketStatusCode.TlsHandshakeFailure, WebSocketError.Handshake_Open_RequestHeader, "The handshake request did not contain the header \"Connection\", or the header was not set to \"Upgrade\".");

                return false;
            }

            string header_web_socket_key = request.Headers["Sec-WebSocket-Key"];
            if (!header_web_socket_key.HasContent())
            {
                exception = new WebSocketException(WebSocketStatusCode.TlsHandshakeFailure, WebSocketError.Handshake_Open_RequestHeader, "The handshake request did not contain the header \"Sec-WebSocket-Key\", or the header was empty or contianed only white space.");

                return false;
            }

            string header_web_socket_version = request.Headers["Sec-WebSocket-Version"];
            if (!header_web_socket_version.HasContent() || header_web_socket_version != "13")
            {
                exception = new WebSocketException(WebSocketStatusCode.TlsHandshakeFailure, WebSocketError.Handshake_Open_RequestHeader, "The handshake request did not contain the header \"Sec-WebSocket-Version\", or the header was not set to \"13\".");

                return false;
            }

            return true;
        }

        private bool
        ValidateHandShakeResponse(HttpWebRequest request, HttpWebResponse response, string uuid, out WebSocketException exception)
        {
            exception = default;

            if (response.StatusCode != HttpStatusCode.SwitchingProtocols)
            {
                exception = new WebSocketException(WebSocketStatusCode.TlsHandshakeFailure, WebSocketError.Handshake_Open_ResponseStatusCode, "The handshake response status code from " + URI.Host + " was not equal to \"101 - Switching Protocols\".");

                return false;
            }

            if (response.ProtocolVersion.Major < 1 && response.ProtocolVersion.Minor < 1)
            {
                exception = new WebSocketException(WebSocketStatusCode.TlsHandshakeFailure, WebSocketError.Handshake_Open_ResponseProtocolVersion, "The handshake response protocol version must be at least HTTP 1.1.");

                return false;
            }

            string server_key = response.Headers["Sec-WebSocket-Accept"];
            if (!server_key.HasContent())
            {
                exception = new WebSocketException(WebSocketStatusCode.TlsHandshakeFailure, WebSocketError.Handshake_Open_ResponseHeader, "The handshake response from " + URI.Host + " did not contain the header \"Sec-WebSocket-Accept\", or the header was empty or contianed only white space.");

                return false;
            }

            string client_key = request.Headers["Sec-WebSocket-Key"];
            string server_key_expected = CreateServerResponseKey(client_key, uuid);
            if (server_key != server_key_expected)
            {
                exception = new WebSocketException(WebSocketStatusCode.TlsHandshakeFailure, WebSocketError.Handshake_Open_ResponseHeader, "The handshake response header \"Sec-WebSocket-Accept\" did not matach the expected value: " + server_key_expected);

                return false;
            }

            string header_web_socket_version = response.Headers["Sec-WebSocket-Version"];
            if (!header_web_socket_version.IsNull() &&(!header_web_socket_version.HasContent() || header_web_socket_version != "13"))
            {
                exception = new WebSocketException(WebSocketStatusCode.TlsHandshakeFailure, WebSocketError.Handshake_Open_ResponseHeader, "The handshake response from " + URI.Host + " included the header \"Sec-WebSocket-Version\" and was not set to \"13\".");

                return false;
            }

            return true;
        }

        private string
        CreateServerResponseKey(string client_key, string uuid)
        {
            StringBuilder buffer = new StringBuilder(client_key, 64);
            buffer.Append(uuid);

            SHA1 client_key_sha_1 = new SHA1CryptoServiceProvider();

            byte[] server_key_bytes = Encoding.UTF8.GetBytes(buffer.ToString());
            server_key_bytes = client_key_sha_1.ComputeHash(server_key_bytes, 0, server_key_bytes.Length);

            return Convert.ToBase64String(server_key_bytes);
        }

        private static bool
        Callback_CertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        private static X509Certificate
        Callback_CertificateSelection(object sender, string target_host, X509CertificateCollection client_certificates, X509Certificate server_certificate, string[] acceptable_issuers)
        {
            return null;
        }

        private void
        CallBack_FailedToConnect(WebSocketException exception)
        {
            WebSocketFrame frame = WebSocketFrame.CreateCloseFrame(WebSocketStatusCode.ProtocolError);
            Send(frame.EncodeFrame());

            stream.Close();
            stream.Dispose();
            stream = null;

            socket.Shutdown(SocketShutdown.Both);
            socket.Disconnect(false);
            socket.Close();
            socket = null;

            SetState(WebSocketState.Connecting, false);

            if (settings.handling_failed_to_connect == RetryHandling.Retry)
            {
                ++settings.connect_retry_count;
                if (settings.connect_retry_count > settings.connect_retry_count_limit)
                {
                    // No further clean up needed since all other managed resources have been freed (except the state mutex).
                    OverrideState(WebSocketState.Closed);

                    RaiseAndMaybeThrowError(new WebSocketException(WebSocketStatusCode.TlsHandshakeFailure, WebSocketError.Handshake_Open_RetryLimitReached, "The maximum amount of failed connection attempts has been reached. Limit: " + settings.connect_retry_count_limit, exception));
                }
                else
                {
                    settings.WaitConnectionDelay();
                    Connect();
                }
            }
            else
            {
                OverrideState(WebSocketState.Closed);

                RaiseAndMaybeThrowError(exception);
            }
        }

        private async void
        CallBack_FailedToConnectAsync(IAsyncResult result)
        {
            WebSocketFrame frame = WebSocketFrame.CreateCloseFrame(WebSocketStatusCode.ProtocolError);
            await SendAsync(frame.EncodeFrame());

            stream.Close();
            stream.Dispose();
            stream = null;

            socket.EndDisconnect(result);
            socket.Close();
            socket = null;

            SetState(WebSocketState.Connecting, false);

            WebSocketException exception = (WebSocketException)result.AsyncState;
            if (settings.handling_failed_to_connect == RetryHandling.Retry)
            {
                ++settings.connect_retry_count;
                if (settings.connect_retry_count > settings.connect_retry_count_limit)
                {
                    // No further clean up needed since all other managed resources have been freed (except the state mutex).
                    OverrideState(WebSocketState.Closed);

                    RaiseAndMaybeThrowError(new WebSocketException(WebSocketStatusCode.TlsHandshakeFailure, WebSocketError.Handshake_Open_RetryLimitReached, "The maximum amount of failed connection attempts has been reached. Limit: " + settings.connect_retry_count_limit, exception));
                }
                else
                {
                    await settings.WaitConnectionDelayAsync();
                    ConnectAsync();
                }
            }
            else
            {
                OverrideState(WebSocketState.Closed);

                RaiseAndMaybeThrowError(exception);
            }
        }

        /// <summary>
        /// Closes the underlying stream and disconnects from the WebSocket.
        /// </summary>
        /// <param name="force_disconnect">Whether or not to disconnect regardless of the current state.</param>
        public void
        Close(bool force_disconnect = false)
        {
            Close(WebSocketStatusCode.NoStatusReceieved, true);
        }

        /// <summary>
        /// Closes the underlying stream and disconnects from the WebSocket.
        /// </summary>
        /// <param name="code">The satus code to include in the closing frame.</param>
        /// <param name="force_disconnect">Whether or not to disconnect regardless of the current state.</param>
        /// <exception cref="WebSocketException">
        /// Thrown if any of the following status codes are provided in the closing frame:
        /// <see cref="WebSocketStatusCode.Reserved"/>, 
        /// <see cref="WebSocketStatusCode.AbnormalClosure"/>, or 
        /// <see cref="WebSocketStatusCode.TlsHandshakeFailure"/>.
        /// </exception>
        public void
        Close(WebSocketStatusCode code, bool force_disconnect = false)
        {
            Close(code, string.Empty, force_disconnect);
        }

        /// <summary>
        /// Closes the underlying stream and disconnects from the WebSocket.
        /// </summary>
        /// <param name="code">The satus code to include in the closing frame.</param>
        /// <param name="reason">The reason for the closure.</param>
        /// <param name="force_disconnect">Whether or not to disconnect regardless of the current state.</param>
        /// <exception cref="WebSocketException">
        /// Thrown if any of the following status codes are provided in the closing frame:
        /// <see cref="WebSocketStatusCode.Reserved"/>, 
        /// <see cref="WebSocketStatusCode.AbnormalClosure"/>, or 
        /// <see cref="WebSocketStatusCode.TlsHandshakeFailure"/>.
        /// </exception>
        public void
        Close(WebSocketStatusCode code, string reason, bool force_disconnect = false)
        {
            WebSocketFrame frame;
            if(code == WebSocketStatusCode.NoStatusReceieved)
            {
                frame = WebSocketFrame.CreateCloseFrame();
            }
            else if (!ValidateCloseStatusCode(code, out WebSocketException exception))
            {
                RaiseAndMaybeThrowError(exception);

                return;
            }
            else
            {
                frame = WebSocketFrame.CreateCloseFrame(code, reason);
            }

            Close(FrameSource.Client, ref frame, force_disconnect);
        }

        internal void
        Close(FrameSource source, ref WebSocketFrame frame, bool force_disconnect = false)
        {
            if (!SetState(WebSocketState.Closing, force_disconnect))
            {
                return;
            }

            if (source == FrameSource.Client)
            {
                Send(frame.EncodeFrame());
            }

            stream.Close();

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

            SetState(WebSocketState.Closed, force_disconnect);
            OnClosed.Raise(this, new CloseEventArgs(DateTime.Now, URI, frame, source, id));
        }

        /// <summary>
        /// Asynchronously closes the underlying stream and disconnects from the WebSocket.
        /// </summary>
        /// <param name="force_disconnect">Whether or not to disconnect regardless of the current state.</param>
        public async Task
        CloseAsync(bool force_disconnect = false)
        {
            await CloseAsync(WebSocketStatusCode.NoStatusReceieved, true);
        }

        /// <summary>
        /// Asynchronously closes the underlying stream and disconnects from the WebSocket.
        /// </summary>
        /// <param name="code">The satus code to include in the closing frame.</param>
        /// <param name="force_disconnect">Whether or not to disconnect regardless of the current state.</param>
        /// <exception cref="WebSocketException">
        /// Thrown if any of the following status codes are provided in the closing frame:
        /// <see cref="WebSocketStatusCode.Reserved"/>, 
        /// <see cref="WebSocketStatusCode.AbnormalClosure"/>, or 
        /// <see cref="WebSocketStatusCode.TlsHandshakeFailure"/>.
        /// </exception>
        public async Task 
        CloseAsync(WebSocketStatusCode code, bool force_disconnect = false)
        {
            await CloseAsync(code, string.Empty, force_disconnect);
        }

        /// <summary>
        /// Asynchronously closes the underlying stream and disconnects from the WebSocket.
        /// </summary>
        /// <param name="code">The satus code to include in the closing frame.</param>
        /// <param name="reason">The reason for the closure.</param>
        /// <param name="force_disconnect">Whether or not to disconnect regardless of the current state.</param>
        /// <exception cref="WebSocketException">
        /// Thrown if any of the following status codes are provided in the closing frame:
        /// <see cref="WebSocketStatusCode.Reserved"/>, 
        /// <see cref="WebSocketStatusCode.AbnormalClosure"/>, or 
        /// <see cref="WebSocketStatusCode.TlsHandshakeFailure"/>.
        /// </exception>
        public async Task
        CloseAsync(WebSocketStatusCode code, string reason, bool force_disconnect = false)
        {
            WebSocketFrame frame;
            if (code == WebSocketStatusCode.NoStatusReceieved)
            {
                frame = WebSocketFrame.CreateCloseFrame();
            }
            else if (!ValidateCloseStatusCode(code, out WebSocketException exception))
            {
                RaiseAndMaybeThrowError(exception);

                return;
            }
            else
            {
                frame = WebSocketFrame.CreateCloseFrame(code, reason);
            }

            await CloseAsync(FrameSource.Client, frame, force_disconnect);
        }

        internal async Task
        CloseAsync(FrameSource source, WebSocketFrame frame, bool force_disconnect = false)
        {
            if (!SetState(WebSocketState.Closing, force_disconnect))
            {
                return;
            }

            if (source == FrameSource.Client)
            {
                await SendAsync(frame.EncodeFrame());
            }

            stream.Close();

            while (reading)
            {
                await Task.Delay(1);
            }

            socket.Shutdown(SocketShutdown.Both);
            socket.BeginDisconnect(false, Callback_OnBeginDisconnect, Tuple.Create(source, force_disconnect, frame));
        }

        private void
        Callback_OnBeginDisconnect(IAsyncResult result)
        {
            Tuple<FrameSource, bool, WebSocketFrame> _result = (Tuple<FrameSource, bool, WebSocketFrame>)result.AsyncState;

            stream.Dispose();
            stream = null;

            socket.EndDisconnect(result);
            socket.Close();
            socket = null;

            SetState(WebSocketState.Closed, _result.Item2);
            OnClosed.Raise(this, new CloseEventArgs(DateTime.Now, URI, _result.Item3, _result.Item1, id));
        }

        private bool
        ValidateCloseStatusCode(WebSocketStatusCode code, out WebSocketException exception)
        {
            exception = default;

            switch (code)
            {
                case WebSocketStatusCode.Reserved:
                case WebSocketStatusCode.AbnormalClosure:
                case WebSocketStatusCode.NoStatusReceieved:
                case WebSocketStatusCode.TlsHandshakeFailure:
                {
                    exception = new WebSocketException(WebSocketStatusCode.ProtocolError, WebSocketError.Handshake_Close_StatusCode, "The closing status code cannot be set to: " + EnumUtil.GetName(code) + ".");

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Force closes and frees all managed resources.
        /// The client will need to be re-instantiated to reconnect.
        /// This method should only be called after disconnecting from the WebSocket.
        /// </summary>
        public virtual void
        Dispose()
        {
            Dispose(true);
        }

        protected void
        Dispose(bool dispose, Action callback = null)
        {
            if (disposing || !dispose || disposed)
            {
                Debug.WriteLine("Already disposing or disposed.");

                return;
            }

            disposing = true;

            Debug.WriteLine("Disposing");

            // If we're disposing we're closing, period. Make sure this is always true.
            // If the client is already in the process of closing, this will effectively do nothing.
            Close(true);

            if (!state_mutex.IsNull())
            {
                state_mutex.Dispose();
                state_mutex = null;
            }

            disposed = true;

            if (!callback.IsNull())
            {
                callback();
            }

            Debug.WriteLine("Disposed");

            OnDisposed.Raise(this, EventArgs.Empty);
        }

        #endregion

        #region State Handling

        public enum
        WebSocketState
        {
            /// <summary>
            /// The client is either connecting or has just been instantiated and is in its idle state.
            /// </summary>
            Connecting = 0,

            /// <summary>
            /// The client is connected.
            /// </summary>
            Open = 1,

            /// <summary>
            /// The client is currently disconnecting.
            /// </summary>
            Closing = 2,

            /// <summary>
            /// The client is disconnected.        
            /// </summary>
            Closed = 3,
        }

        private bool
        OverrideState(WebSocketState state)
        {
            return SetState(state, true);
        }

        private bool
        SetState(WebSocketState state)
        {
            return SetState(state, false, false);
        }

        private bool
        SetState(WebSocketState transition_state, bool handshake_initiated, bool override_state = false)
        {
            // Since the default state of the web socket is 'Connecting', we need a special case for this state.
            if (state == transition_state && state != WebSocketState.Connecting)
            {
                return false;
            }

            if (state_mutex.IsNull())
            {
                state = WebSocketState.Closed;

                return false;
            }

            state_mutex.WaitOne();

            bool change_state = false;

            if (!override_state)
            {
                switch (transition_state)
                {
                    case WebSocketState.Connecting: change_state = CanConnect();                         break;
                    case WebSocketState.Closing:    change_state = CanDisconnect();                      break;
                    case WebSocketState.Open:       change_state = state == WebSocketState.Connecting;   break;
                    case WebSocketState.Closed:     change_state = state == WebSocketState.Closing;      break;
                }
            }

            this.handshake_initiated = handshake_initiated;

            if (change_state)
            {
                state = transition_state;
                if (state == WebSocketState.Connecting)
                {
                    handshake_initiated = true;
                    polling = true;
                }
                else if (state == WebSocketState.Closing)
                {
                    polling = false;
                }
            }

            state_mutex.ReleaseMutex();

            return change_state;
        }

        /// <summary>
        /// Checks to see if it is safe to connect.
        /// </summary>
        private bool
        CanConnect()
        {
            switch (state)
            {
                case WebSocketState.Connecting:
                {
                    if (handshake_initiated)
                    {
                        Debug.WriteLine("Cannot connect to " + URI.Host + ": currently connecting");
                        return false;
                    }

                    return true;
                }
                case WebSocketState.Open:       Debug.WriteLine("Cannot connect to " + URI.Host + ": already connected");       return false;
                case WebSocketState.Closing:    Debug.WriteLine("Cannot connect to " + URI.Host + ": currently disconnecting"); return false;
            }

            return true;
        }

        /// <summary>
        /// Checks to see if it is safe to disconnect.
        /// </summary>
        private bool
        CanDisconnect()
        {
            switch (state)
            {
                case WebSocketState.Connecting: Debug.WriteLine("Cannot disconnect from " + URI.Host + ": currently connecting");   return false;
                case WebSocketState.Closing:    Debug.WriteLine("Cannot disconnect from " + URI.Host + ": already connecting");     return false;
                case WebSocketState.Closed:     Debug.WriteLine("Cannot disconnect from " + URI.Host + ": already disconnected");   return false;
            }

            return true;
        }

        #endregion

        #region Writing

        /// <summary>
        /// Sends a message to the WebSocket.
        /// </summary>
        /// <param name="format">
        /// The string to send to the WebSocket.
        /// This can be a normal string and does not need to include variable formats.
        /// </param>
        /// <param name="arguments">Optional format arugments.</param>
        /// <returns>
        /// Returns true if the message was successfully sent to the WebSocket.
        /// Returns false otherwise.
        /// </returns>
        public bool
        Send(string format, params object[] arguments)
        {
            string message = !arguments.IsValid() ? format : string.Format(format, arguments);
            if (!CanSend(ref message))
            {
                return false;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(message);

            return Send(Opcode.Text, new MemoryStream(bytes));;
        }

        /// <summary>
        ///  sends a message to the WebSocket.
        /// </summary>
        /// <param name="format">
        /// The string to send to the WebSocket.
        /// This can be a normal string and does not need to include variable formats.
        /// </param>
        /// <param name="arguments">Optional format arugments.</param>
        /// <returns>
        /// Returns true if the message was successfully sent to the WebSocket.
        /// Returns false otherwise.
        /// </returns>
        public async Task<bool>
        SendAsync(string format, params object[] arguments)
        {
            string message = !arguments.IsValid() ? format : string.Format(format, arguments);
            if (!CanSend(ref message))
            {
                return false;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(message);

            return await SendAsync(Opcode.Text, new MemoryStream(bytes));
        }

        /// <summary>
        /// Sends a stream of byte data to the WebSocket.
        /// The data should not an encoded Websocket frame.
        /// </summary>
        /// <param name="opcode">The type of frame being sent.</param>
        /// <param name="data">
        /// The stream of data to be sent to the WebSocket.
        /// The data should not be en encoded WebSocket frame and should just be the raw byte data to be included in the WebSocket frame.
        /// </param>
        /// </summary>
        /// Returns true if the buffer was successfully sent to the WebSocket.
        /// Returns false otherwise.
        /// </returns>
        public bool
        Send(Opcode opcode, Stream stream)
        {
            if (!CanSend(stream))
            {
                return false;
            }

            ulong buffer_length = (ulong)stream.Length;
            if (buffer_length == 0)
            {
                return Send(Fin.Final, opcode, new byte[0]);
            }

            ulong fragment_length = (ulong)settings.frame_fragment_length_write;
            byte[] buffer = buffer_length < fragment_length ? new byte[buffer_length] : new byte[fragment_length];

            // ---------------------------------------------
            // The entire message can be sent as one message
            // ---------------------------------------------

            if (buffer_length <= fragment_length)
            {
                return stream.Read(buffer, 0, buffer.Length) == buffer.Length && Send(Fin.Final, opcode, buffer);
            }

            // ---------------------------------------------
            // The message needs to sent in fragments
            // ---------------------------------------------

            Fin fin = Fin.Fragment;

            // Figure out how many fragments needs to be sent
            ulong fragments_count = buffer_length / fragment_length;
            ulong remainder = buffer_length % fragment_length;
            if(remainder > 0)
            {
                ++fragments_count;
            }

            int buffer_bytes_read_count = 0;
            for (ulong index = 0; index < fragments_count; ++index)
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

        /// <summary>
        /// Asynchronously sends a stream of byte data to the WebSocket.
        /// The data should not an encoded Websocket frame.
        /// </summary>
        /// <param name="opcode">The type of frame being sent.</param>
        /// <param name="data">
        /// The stream of data to be sent to the WebSocket.
        /// The data should not be en encoded WebSocket frame and should just be the raw byte data to be included in the WebSocket frame.
        /// </param>
        /// </summary>
        /// Returns true if the buffer was successfully sent to the WebSocket.
        /// Returns false otherwise.
        /// </returns>
        public async Task<bool>
        SendAsync(Opcode opcode, Stream stream)
        {
            if (!CanSend(stream))
            {
                return false;
            }

            ulong buffer_length = (ulong)stream.Length;
            if (buffer_length == 0)
            {
                return await SendAsync(Fin.Final, opcode, new byte[0]);
            }

            ulong fragment_length = (ulong)settings.frame_fragment_length_write;
            byte[] buffer = buffer_length < fragment_length ? new byte[buffer_length] : new byte[fragment_length];

            // ---------------------------------------------
            // The entire message can be sent as one message
            // ---------------------------------------------

            if (buffer_length <= fragment_length)
            {
                return stream.Read(buffer, 0, buffer.Length) == buffer.Length && await SendAsync(Fin.Final, opcode, buffer);
            }

            // ---------------------------------------------
            // The message needs to sent in fragments
            // ---------------------------------------------

            Fin fin = Fin.Fragment;

            // Figure out how many fragments needs to be sent
            ulong fragments_count = buffer_length / fragment_length;
            ulong remainder = buffer_length % fragment_length;
            if (remainder > 0)
            {
                ++fragments_count;
            }

            int buffer_bytes_read_count = 0;
            for (ulong index = 0; index < fragments_count; ++index)
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
                if (buffer_bytes_read_count != buffer.Length || !await SendAsync(fin, opcode, buffer))
                {
                    return false;
                }

                Array.Clear(buffer, 0, buffer_bytes_read_count);
            }

            return true;
        }

        /// <summary>
        /// Sends byte data to the WebSocket.
        /// The data should not an encoded Websocket frame.
        /// </summary>
        /// <param name="fin">Whether or not this is ihe final gragment being sent.</param>
        /// <param name="opcode">The type of frame being sent.</param>
        /// <param name="data">
        /// The data to be sent to the WebSocket.
        /// The data should not be en encoded WebSocket frame and should just be the raw byte data to be included in the WebSocket frame.
        /// </param>
        /// </summary>
        /// Returns true if the buffer was successfully sent to the WebSocket.
        /// Returns false otherwise.
        /// </returns>
        public bool
        Send(Fin fin, Opcode opcode, byte[] data)
        {
            PayloadData payload = new PayloadData(data);
            WebSocketFrame frame = new WebSocketFrame(fin, opcode, payload);

            return Send(frame.encoded);
        }

        /// <summary>
        /// Asynchronously sends byte data to the WebSocket.
        /// The data should not an encoded Websocket frame.
        /// </summary>
        /// <param name="fin">Whether or not this is ihe final gragment being sent.</param>
        /// <param name="opcode">The type of frame being sent.</param>
        /// <param name="data">
        /// The data to be sent to the WebSocket.
        /// The data should not be en encoded WebSocket frame and should just be the raw byte data to be included in the WebSocket frame.
        /// </param>
        /// </summary>
        /// Returns true if the buffer was successfully sent to the WebSocket.
        /// Returns false otherwise.
        /// </returns>
        public async Task<bool>
        SendAsync(Fin fin, Opcode opcode, byte[] data)
        {
            PayloadData payload = new PayloadData(data);
            WebSocketFrame frame = new WebSocketFrame(fin, opcode, payload);

            return await SendAsync(frame.encoded);
        }

        /// <summary>
        /// Sends a byte buffer to the WebSocket.
        /// </summary>
        /// <param name="buffer">
        /// The buffer to be sent to the WebSocket.
        /// Thebuffer needs to be a properly encoded Websocket frame.
        /// </param>
        /// Returns true if the buffer was successfully sent to the WebSocket.
        /// Returns false otherwise.
        /// </returns>
        public bool
        Send(byte[] buffer)
        {
            if (!CanSend(buffer))
            {
                return false;
            }

            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            return true;
        }

        /// <summary>
        /// Asynchronously sends a byte buffer to the WebSocket.
        /// The buffer needs to be a properly encoded Websocket frame.
        /// </summary>
        /// Returns true if the buffer was successfully sent to the WebSocket.
        /// Returns false otherwise.
        /// </returns>
        public async Task<bool>
        SendAsync(byte[] buffer)
        {
            if (!CanSend(buffer))
            {
                return false;
            }

            await stream.WriteAsync(buffer, 0, buffer.Length);
            await stream.FlushAsync();

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool
        IsAlive()
        {
            return state == WebSocketState.Open && !socket.IsNull() && socket.Connected && !stream.IsNull();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool
        CanSend(ref string message)
        {
            // We don't want to explicitly check for an empty string
            // It is legal to send a an empty frame, which may be the desired behavior by the user.
            // The required frame header will automatically be added later down the pipe.
            return IsAlive() && !message.IsNull();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool
        CanSend(Stream stream)
        {
            // We don't want to explicitly check for an empty stream/buffer.
            // It is legal to send a an empty frame, which may be the desired behavior by the user.
            // The required frame header will automatically be added later down the pipe.
            return IsAlive() && !stream.IsNull() && !stream.IsNull();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool
        CanSend(in byte[] data)
        {
            // Now is where we need to check against the minimum required buffer length.
            // Even an empty frame at a minimum is requried to have a header which is 2 bytes long.
            return IsAlive() && !data.IsNull() && data.Length >= 2;
        }

        #endregion

        #region Reading

        private async void
        ReadStream()
        {
            reading = true;

            Opcode opcode;

            DateTime time;
            WebSocketFrame frame;
            List<WebSocketFrame> frames = new List<WebSocketFrame>();

            byte[] buffer_frame_header = new byte[2];
            List<byte> buffer_message_data = new List<byte>(settings.frame_fragment_length_read);
            string message = string.Empty;

            Tuple<bool, WebSocketFrame> result;

            while (polling && !socket.IsNull() && !stream.IsNull())
            {
                Array.Clear(buffer_frame_header, 0, buffer_frame_header.Length);

                result = await ReadFrameAsync(stream, buffer_frame_header);
                if (!result.Item1)
                {
                    await CloseAsync(WebSocketStatusCode.ProtocolError, true);

                    break;
                }

                time = DateTime.Now;

                frame = result.Item2;
                OnWebSocketFrame.Raise(this, new FrameEventArgs(time, URI, frame, id));

                frames.Add(frame);

                if (frame.payload.length > 0)
                {
                    buffer_message_data.AddRange(frame.payload.data);
                }

                if (frame.fin == Fin.Final)
                {
                    opcode = frames[0].opcode;

                    RunMessageHandler(opcode, time, frames.ToArray(), buffer_message_data.ToArray());

                    frames.Clear();
                    buffer_message_data.Clear();
                }                
            }

            reading = false;
        }

        private async Task<Tuple<bool, WebSocketFrame>>
        ReadFrameAsync(Stream stream, byte[] buffer_header)
        {
            Tuple<bool, WebSocketFrame> result_frame_fail = Tuple.Create(false, default(WebSocketFrame));

            WebSocketFrame frame = new WebSocketFrame();

            // *********************************************
            // *                                           *
            // *             Header Decoding               *
            // *                                           *
            // *********************************************

            if (!await ReadStreamAsync(stream, buffer_header))
            {
                return result_frame_fail;
            }

            // Hex  |  Binary   | Shift
            // -----|-----------|---------
            // 0x80 | 1000 0000 | (1 << 7)
            // 0x40 | 0100 0000 | (1 << 6)
            // 0x20 | 0010 0000 | (1 << 5)
            // 0x10 | 0001 0000 | (1 << 4)

            frame.fin = (buffer_header[0] & 0x80) == 0x80 ? Fin.Final : Fin.Fragment;

            frame.rsv_1 = (buffer_header[0] & 0x40) == 0x40 ? RSV.On : RSV.Off;
            frame.rsv_2 = (buffer_header[0] & 0x20) == 0x20 ? RSV.On : RSV.Off;
            frame.rsv_3 = (buffer_header[0] & 0x10) == 0x10 ? RSV.On : RSV.Off;

            frame.opcode = (Opcode)(buffer_header[0] & 0x0f);

            frame.mask = (buffer_header[1] & 0x80) == 0x80 ? Mask.On : Mask.Off;

            // Hex  |  Binary   | Decimal
            // -----|-----------|---------
            // 0x7F | 0111 1111 | 127
            frame.length_payload = (byte)(buffer_header[1] & 0x7F);

            // *********************************************
            // *                                           *
            // *     Extended Payload Length Decoding      *
            // *                                           *
            // *********************************************

            // If the payload data < 126, this is the size of the payload
            if (frame.length_payload < 126)
            {
                frame.length_payload_extended = new byte[0];
            }
            // Otherside, the real size of the payload is stored in either the next 2 or 8 bytes depending on length_payload's value
            else
            {
                int length_payload_extended_count = frame.length_payload == 126 ? 2 : 8;

                frame.length_payload_extended = new byte[length_payload_extended_count];
                if (!await ReadStreamAsync(stream, frame.length_payload_extended))
                {
                    return result_frame_fail;
                }
            }

            // *********************************************
            // *                                           *
            // *            Mask Key Decoding              *
            // *                                           *
            // *********************************************

            // Because we are a lways a client, the payload data should *NEVER* be masked
            // But still handle both cases just in case
            if (frame.mask == Mask.Off)
            {
                frame.mask_key = new byte[0];
            }
            else
            {
                frame.mask_key = new byte[4];

                if (!await ReadStreamAsync(stream, frame.mask_key))
                {
                    return result_frame_fail;
                }
            }

            // *********************************************
            // *                                           *
            // *             Payload Decoding              *
            // *                                           *
            // *********************************************

            PayloadData payload = new PayloadData();

            switch(frame.length_payload)
            {
                case 126:   payload.length = frame.length_payload_extended.ToUint16FromBigEndian(); break;
                case 127:   payload.length = frame.length_payload_extended.ToUint64FromBigEndian(); break;
                default:    payload.length = frame.length_payload;                                  break;
            }

            // Not an error, just an empty frame
            if (payload.length == 0)
            {
                frame.payload = PayloadData.Empty;

                return Tuple.Create(true, frame);
            }

            payload.data = new byte[payload.length];

            int fragment_length_read = settings.frame_fragment_length_read;

            // The entire payload can be read in one go
            if (payload.length <= (ulong)fragment_length_read)
            {
                if (!await ReadStreamAsync(stream, payload.data))
                {
                    return result_frame_fail;
                }
            }
            // We need to read multple times to get the entire payload 
            else
            {
                ulong fragments_count = payload.length / (ulong)fragment_length_read;

                // The above calc essentially acts as a Floor()
                // We need to see if there are any remaining bytes to be read
                if (payload.length % (ulong)fragment_length_read > 0)
                {
                    ++fragments_count;
                }

                int offset = 0;
                for (ulong index = 0; index < fragments_count; ++index)
                {
                    if (!await ReadStreamAsync(stream, payload.data, offset, fragment_length_read))
                    {
                        return result_frame_fail;
                    }

                    offset += fragment_length_read;

                    int bytes_remaining = payload.data.Length - offset;
                    fragment_length_read = bytes_remaining < fragment_length_read ? bytes_remaining : fragment_length_read;
                }
            }

            frame.payload = payload;

            if (frame.mask == Mask.On)
            {
                frame.payload.Mask(frame.mask_key);
            }

            if (!ValidateReceivedFrame(frame, out WebSocketException exception))
            {
                RaiseError(exception);

                return result_frame_fail;
            }

            return Tuple.Create(true, frame);
        }

        private bool
        ValidateReceivedFrame(in WebSocketFrame frame, out WebSocketException exception)
        {
            exception = default;

            if (frame.mask == Mask.On)
            {
                exception = new WebSocketException(WebSocketStatusCode.ProtocolError, WebSocketError.Protocol_FrameMask, frame, "A masked frame was recieved from the WebSocket server.");

                return false;
            }

            // TODO: (ValidateReceivedFrame) - Look into the handshake process a little more and see how this can be confirmed and not just assumed.
            // This is fine for now since Twitch doesn't negotiate any extensions, but just for the sake of completion.
            if (frame.rsv_1 == RSV.On || frame.rsv_2 == RSV.On || frame.rsv_2 == RSV.On)
            {
                exception = new WebSocketException(WebSocketStatusCode.ProtocolError, WebSocketError.Protocol_FrameRSV, frame, "One of the RSV header bits was set to a non-zero value where such no such behavior was negotiated during the handshake process.");

                return false;
            }

            if (frame.is_control)
            {
                if (frame.fin != Fin.Final)
                {
                    exception = new WebSocketException(WebSocketStatusCode.ProtocolError, WebSocketError.Protocol_FrameControl, frame, "A control frame was sent as a gragment.");

                    return false;
                }

                if (frame.length_payload >= 126)
                {
                    exception = new WebSocketException(WebSocketStatusCode.ProtocolError, WebSocketError.Protocol_FrameControl, frame, "A control frame was with a payload length of " + frame.length_payload + ". The maximum size of a control frame can be is 125 bytes.");

                    return false;
                }
            }

            return true;
        }

        private async Task<bool>
        ReadStreamAsync(Stream stream, byte[] buffer)
        {
            return await ReadStreamAsync(stream, buffer, 0, buffer.Length);
        }

        private async Task<bool>
        ReadStreamAsync(Stream stream, byte[] buffer, int offset, int count)
        {
            int buffer_bytes_read_count = 0;

            try
            {
                buffer_bytes_read_count = await stream.ReadAsync(buffer, offset, count);
            }
            catch (ObjectDisposedException exception)
            {
                if (state != WebSocketState.Closing)
                {
                    RaiseError(new WebSocketException(WebSocketStatusCode.PolicyViolation, WebSocketError.Stream_Disposed, "The stream was disposed while attempting to read from it.", exception));
                }

                return false;
            }

            // Usually this means that the underlying stream/socket has been disconnected without prior warning.
            // If we were disconnected without prior notice, i.e., we never received a Close control frame, this is technially violating protocal.
            // Which, suprise, Twitch does quite frequently.
            if (buffer_bytes_read_count == 0) 
            {
                RaiseError(new WebSocketException(WebSocketStatusCode.ProtocolError, WebSocketError.Stream_BytesReadCount, "Zero bytes were read from the stream instead of the expected " + count + " bytes."));

                return false;
            }

            // This normally isn't inherantly an error, but for a web socket it's bad if the correct number of bytes isn't read.
            if (buffer_bytes_read_count != count)
            {
                RaiseError(new WebSocketException(WebSocketStatusCode.ProtocolError, WebSocketError.Stream_BytesReadCount, buffer_bytes_read_count + " bytes were ready from the stream instead of the expected " + count + " bytes."));

                return false;
            }

            return true;
        }

        #endregion

        #region Error Handling

        private void
        RaiseError(WebSocketException exception)
        {
            OnWebSocketError.Raise(this, new WebSocketErrorEventArgs(DateTime.Now, URI, exception, id));
        }

        private void
        RaiseAndMaybeThrowError(WebSocketException exception)
        {
            OnWebSocketError.Raise(this, new WebSocketErrorEventArgs(DateTime.Now, URI, exception, id));

            if (settings.handling_error == ErrorHandling.Return)
            {
                return;
            }

            throw exception;
        }

        #endregion
    }

    #region Data Structures

    public struct
    WebSocketFrame
    {
        public Fin fin;

        public RSV rsv_1;
        public RSV rsv_2;
        public RSV rsv_3;

        public Opcode opcode;

        public Mask mask;
        
        public byte length_payload;
        public byte[] length_payload_extended;

        public byte[] mask_key;

        public PayloadData payload;

        private byte[] _encoded;

        public byte[] encoded
        {
            get
            {
                // Don't encode the frame over and over again if we don't have to.
                return _encoded.Length == 0 ? EncodeFrame() : _encoded;
            }
        }

        public bool is_control
        {
            get
            {
                return opcode >= Opcode.Close;
            }
        }

        public
        WebSocketFrame(Fin fin, Opcode opcode, in PayloadData payload, bool use_mask = true)
        {
            this.payload = payload;
            _encoded = new byte[0];

            this.fin = fin;

            rsv_1 = RSV.Off;    // This will always be uncompressed data so this will never be 'On'
            rsv_2 = RSV.Off;
            rsv_3 = RSV.Off;

            this.opcode = opcode;

            if (payload.length < 126)
            {
                length_payload = (byte)payload.length;
                length_payload_extended = new byte[0];
            }
            else if (payload.length <= ushort.MaxValue)
            {
                length_payload = 126;
                length_payload_extended = ((ushort)payload.length).ToBigEndianByteArray();
            }
            else
            {
                length_payload = 127;
                length_payload_extended = payload.length.ToBigEndianByteArray();
            }

            if (use_mask)
            {
                mask = Mask.On;
                mask_key = CreateMaskingKey();
                payload.Mask(mask_key);
            }
            else
            {
                mask = Mask.Off;
                mask_key = new byte[0];
            }

            _encoded = EncodeFrame();
        }

        private static byte[]
        CreateMaskingKey()
        {
            byte[] key = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(key);

            return key;
        }

        public static WebSocketFrame
        CreateCloseFrame()
        {
            return new WebSocketFrame(Fin.Final, Opcode.Close, PayloadData.Empty);
        }

        public static WebSocketFrame
        CreateCloseFrame(WebSocketStatusCode status_code, string reason = "")
        {
            PayloadData payload = new PayloadData(status_code, reason);
            return new WebSocketFrame(Fin.Final, Opcode.Close, payload);
        }

        public static WebSocketFrame
        CreatePingFrame()
        {
            return new WebSocketFrame(Fin.Final, Opcode.Ping, PayloadData.Empty);
        }

        public static WebSocketFrame
        CreatePingFrame(byte[] data)
        {
            if (!data.IsValid())
            {
                return CreatePingFrame();
            }

            return new WebSocketFrame(Fin.Final, Opcode.Ping, new PayloadData(data));
        }

        public static WebSocketFrame
        CreatePongFrame()
        {
            return new WebSocketFrame(Fin.Final, Opcode.Pong, PayloadData.Empty);
        }

        public static WebSocketFrame
        CreatePongFrame(byte[] data)
        {
            if (!data.IsValid())
            {
                return CreatePongFrame();
            }

            return new WebSocketFrame(Fin.Final, Opcode.Pong, new PayloadData(data));
        }

        public byte[]
        EncodeFrame()
        {
            MemoryStream buffer = new MemoryStream();

            int header = (int)fin;
            header = (header << 1) + (int)rsv_1;
            header = (header << 1) + (int)rsv_2;
            header = (header << 1) + (int)rsv_3;
            header = (header << 4) + (int)opcode;
            header = (header << 1) + (int)mask;
            header = (header << 7) + (int)length_payload;

            buffer.Write(((ushort)header).ToBigEndianByteArray(), 0, 2);

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
                buffer.Write(payload.data, 0, payload.data.Length);
            }

            buffer.Close();

            _encoded = buffer.ToArray();

            return _encoded;
        }
    }

    public struct
    PayloadData
    {
        public static readonly PayloadData Empty;

        public WebSocketStatusCode close_status_code;

        public string close_reason;

        public byte[] data;

        public ulong length;

        static
        PayloadData()
        {
            Empty = new PayloadData(new byte[0]);
        }

        public
        PayloadData(byte[] data)
        {
            close_status_code = 0;
            close_reason = string.Empty;

            this.data = data;

            length = (ulong)data.LongLength;
        }

        public
        PayloadData(WebSocketStatusCode status_code, string reason)
        {
            close_status_code = 0;
            close_reason = reason;

            data = new byte[2];
            length = (ulong)data.LongLength;

            if (!reason.HasContent())
            {
                byte[] data_code = ((ushort)status_code).ToBigEndianByteArray();
                byte[] data_reason = Encoding.UTF8.GetBytes(reason);

                data = new byte[data_code.LongLength + data_reason.LongLength];

                int index = 0;
                foreach(byte element in data_code)
                {
                    data[index] = element;
                    ++index;
                }

                foreach (byte element in data_code)
                {
                    data[index] = element;
                    ++index;
                }
            }
            else
            {
                data = ((ushort)status_code).ToBigEndianByteArray();
                length = (ulong)data.LongLength;
            }
        }

        public void
        Mask(byte[] key)
        {
            if (key.Length == 0 || length == 0)
            {
                return;
            }

            for (ulong index = 0; index < length; ++index)
            {
                data[index] = (byte)(data[index] ^ key[index % 4]);
            }
        }

        public void
        Unmask(ref Mask mask, ref byte[] key)
        {
            if (mask == PubSub.Mask.Off)
            {
                return;
            }

            mask = PubSub.Mask.Off;

            Mask(data);
            key = new byte[0];
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

    public enum
    WebSocketStatusCode : ushort
    {
        /// <summary>
        /// <para>Code: 1000</para>
        /// <para>The WebSoket connection has been closed normally and the purpose of the connection was fulfilled.</para>
        /// </summary>
        NormalClosure = 1000,

        /// <summary>
        /// <para>Code: 1001</para>
        /// <para>Indicates that the WebSoket endpoint is going away, i.e. the WebSocket is shutting down, a browser navigated away from a page, etc.</para>
        /// </summary>
        GoingAway = 1001,

        /// <summary>
        /// <para>Code: 1002</para>
        /// <para>Indicates that the WebSoket endpoint, client or server, is terminating the connection due to a protocol error/violation.</para>
        /// </summary>
        ProtocolError = 1002,

        /// <summary>
        /// <para>Code: 1003</para>
        /// <para>Indicates that the WebSoket endpoint, client or server, is terminating the connection because it receieved a type of data it was not expecting or cannot support.</para>
        /// </summary>
        UnsupportedData = 1003,

        /// <summary>
        /// <para>Code: 1004</para>
        /// <para>This status code is reserved and has no meaning.</para>
        /// </summary>
        Reserved = 1004,

        /// <summary>
        /// <para>Code: 1005</para>
        /// <para>
        /// The status code is reserved and must not be used in a Close control frame by either WebSocket endpoint, client or server.
        /// Indicates that status a code is expected but no status code is actually present.
        /// </para>
        /// </summary>
        NoStatusReceieved = 1005,

        /// <summary>
        /// <para>Code: 1006</para>
        /// <para>
        /// The status code is reserved and must not be used in a close control frame by either WebSocket endpoint, client or server.
        /// Indicates that the connection was closed abnormally, i.e., no Close control frame was received.
        /// </para>
        /// </summary>
        AbnormalClosure = 1006,

        /// <summary>
        /// <para>Code: 1007</para>
        /// <para>
        /// Indicates that the WebSoket endpoint, client or server, is terminating the connection because it receieved a message whose data type is not consistent with the decalred data type.
        /// ,i.e, non-UTF8 encoded data being provided with a Text message.
        /// </para>
        /// </summary>
        InvalidPayloadData = 1007,

        /// <summary>
        /// <para>Code: 1008</para>
        /// <para>
        /// Indicates that the WebSoket endpoint, client or server, is terminating the connection because it receieved a message that violates its own policy.
        /// This is a generic status code that is used when a more specific one does not apply or when there is a need to hide specific details about the policy.
        /// </para>
        /// </summary>
        PolicyViolation = 1008,

        /// <summary>
        /// <para>Code: 1009</para>
        /// <para>Indicates that the WebSoket endpoint, client or server, is terminating the connection because it receieved a message that is too big for it process.</para>
        /// </summary>
        MessageTooBig = 1009,

        /// <summary>
        /// <para>Code: 1010</para>
        /// <para>Indicates that the WebSoket client endpoint is terminating the connection because it expected the server to negotiate one or more extension, but were never returned during the handshake.</para>
        /// </summary>
        ExtensionsExpected = 1010,

        /// <summary>
        /// <para>Code: 1011</para>
        /// <para>Indicates that the WebSoket server endpoint is terminating the connection because they encountered an unexpected error that prevented from fulfilling the connecion reuest.</para>
        /// </summary>
        InternalServerError = 1011,

        /// <summary>
        /// <para>Code: 1015</para>
        /// <para>
        /// The status code is reserved and must not be used in a Close control frame by either WebSocket endpoint, client or server.
        /// Indicates that the connection was closed due to a TLS handhake failure.
        /// </para>
        /// </summary>
        TlsHandshakeFailure = 1015
    }

    public interface
    IWebSocketSettings
    {
        int frame_fragment_length_read { get; set; }
        int frame_fragment_length_write { get; set; }

        int connect_retry_count_limit { get; set; }

        RetryHandling handling_failed_to_connect { get; set; }
        ErrorHandling handling_error { get; set; }
    }

    public class
    WebSocketSettings : IWebSocketSettings
    {
        internal int connect_retry_count;
        public int connect_retry_count_limit { get; set; }

        public int frame_fragment_length_read { get; set; }
        public int frame_fragment_length_write { get; set; }

        public RetryHandling handling_failed_to_connect { get; set; }
        public ErrorHandling handling_error { get; set; }

        public WebSocketSettings()
        {
            // Copy paste from Reset() to avoid duplicate instantiation when derived classes instantiated.
            // Keep these synchronized at all times!

            frame_fragment_length_read = 1024;
            frame_fragment_length_write = 1024;

            connect_retry_count = 0;
            connect_retry_count_limit = 5;

            handling_failed_to_connect = RetryHandling.Retry;
            handling_error = ErrorHandling.Error;
        }

        public virtual void
        Reset()
        {
            frame_fragment_length_read = 1024;
            frame_fragment_length_write = 1024;

            connect_retry_count = 0;
            connect_retry_count_limit = 5;

            handling_failed_to_connect = RetryHandling.Retry;
            handling_error = ErrorHandling.Error;
        }

        internal void
        WaitConnectionDelay()
        {
            if (connect_retry_count < 1)
            {
                return;
            }

            connect_retry_count = connect_retry_count.ClampMax(connect_retry_count_limit);

            // If this actually overflows to where we need to clamp the value, the hell is the user doing?
            int time = ((int)Math.Pow(2, (connect_retry_count - 1) * 1000)).Clamp(1000, Int32.MaxValue);
            Thread.Sleep(time);
        }

        internal async Task
        WaitConnectionDelayAsync()
        {
            if (connect_retry_count < 1)
            {
                return;
            }

            // If this actually overflows to where we need to clamp the value, the hell is the user doing?
            int time = ((int)Math.Pow(2, (connect_retry_count - 1) * 1000)).Clamp(1000, Int32.MaxValue);
            await Task.Delay(time);
        }
    }

    #endregion

    #region Error Types

    public enum
    RetryHandling
    {
        Retry = 0,

        Return,
    }

    public enum
    ErrorHandling
    {
        Error = 0,

        Return,
    }

    public enum
    WebSocketError
    {
        None = 0,

        /// <summary>
        /// The WebSocket URI was null.
        /// </summary>
        Handshake_Open_Uri,

        /// <summary>
        /// The WebSocket URI host was null.
        /// </summary>
        Handshake_Open_UriHost,

        /// <summary>
        /// The WebSocket URI port out of the expected range, 0 to 65535.
        /// </summary>
        Handshake_Open_UriPort,

        /// <summary>
        /// The handshake request protocol version was older than HTTP 1.1.
        /// </summary>
        Handshake_Open_RequestProtocolVersion,

        /// <summary>
        /// <para>One of the following handshake request header errors was encountered:</para
        /// <para>The handshake request did not contain the header \"Upgrade\", or the header was not set to \"websocket\".</para>
        /// <para>The handshake request did not contain the header \"Connection\", or the header was not set to \"Upgrade\".</para>
        /// <para>The handshake request did not contain the header \"Sec-WebSocket-Key\", or the header was empty or contianed only white space.</para>
        /// <para>The handshake request did not contain the header \"Sec-WebSocket-Version\", or the header was not set to \"13\".</para>
        /// </summary>
        Handshake_Open_RequestHeader,

        /// <summary>
        /// The handshake response status code was not equal to "101 - Switching Protocols".
        /// </summary>
        Handshake_Open_ResponseStatusCode,

        /// <summary>
        /// The handshake response protocol version must be at least HTTP 1.1.
        /// </summary>
        Handshake_Open_ResponseProtocolVersion,

        /// <summary>
        /// <para>One of the following handshake response header errors was encountered:</para>
        /// <para>The handshake response did not contain the header "Sec-WebSocket-Accept", or the header was empty or contianed only white space.</para>
        /// <para>The handshake response header "Sec-WebSocket-Accept" did not matach the expected value.</para>
        /// <para>The handshake response included the header "Sec-WebSocket-Version" and was not set to "13".</para>
        /// </summary>
        Handshake_Open_ResponseHeader,

        /// <summary>
        /// The maximum number of connection retry attempts was exceeded.
        /// </summary>
        Handshake_Open_RetryLimitReached,

        /// <summary>
        /// The provided status code during the closing handshake was one of the following:
        /// <para><see cref="WebSocketStatusCode.Reserved"/></para>
        /// <para><see cref="WebSocketStatusCode.AbnormalClosure"/></para>
        /// <para><see cref="WebSocketStatusCode.TlsHandshakeFailure"/></para>
        /// </summary>
        Handshake_Close_StatusCode,

        /// <summary>
        /// A server sent a masked frame to a client and can never do so.
        /// </summary>
        Protocol_FrameMask,

        /// <summary>
        /// One of the RSV header bits was set to a non-zero value where such no such behavior was negotiated during the handshake process.
        /// </summary>
        Protocol_FrameRSV,

        /// <summary>
        /// <para>a control frame was received and one of the following errors was encountered:</para>
        /// <para>The frame was sent as a fragment.</para>
        /// <para>The frame payload length was greater than or equal to 126 bytes.</para>
        /// </summary>
        Protocol_FrameControl,

        /// <summary>
        /// The stream was disposed while attempting to read data from the WebSocket.
        /// </summary>
        Stream_Disposed,

        /// <summary>
        /// <para>One of the following errors was encountered while attempting to read from the stream:</para>
        /// <para>Zero bytes were read from the WebSocket buffer instead of the expected number of bytes.</para>
        /// <para>The number of bytes that were read from the WebSocket buffer was not equal to the expected number of bytes.</para>
        /// </summary>
        Stream_BytesReadCount,
    }

    public class
    WebSocketException : Exception
    {
        public WebSocketStatusCode status_code;

        public WebSocketError error;

        public WebSocketFrame frame;

        public WebSocketException(WebSocketStatusCode status_code, WebSocketError error, string message) : base(message)
        {
            this.status_code = status_code;
            this.error = error;
        }

        public WebSocketException(WebSocketStatusCode status_code, WebSocketError error, WebSocketFrame frame, string message) : base(message)
        {
            this.status_code = status_code;
            this.error = error;
            this.frame = frame;
        }

        public WebSocketException(WebSocketStatusCode status_code, WebSocketError error, string message, Exception inner_exception) : base(message, inner_exception)
        {
            this.status_code = status_code;
            this.error = error;
        }

        public WebSocketException(WebSocketStatusCode status_code, WebSocketError error, WebSocketFrame frame, string message, Exception inner_exception) : base(message, inner_exception)
        {
            this.status_code = status_code;
            this.error = error;
            this.frame = frame;
        }
    }

    #endregion
}
 