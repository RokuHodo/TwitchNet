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

    public class
    PubSubClient : IDisposable
    {
        private volatile bool polling;
        private volatile bool reading;
        private volatile bool handshake_initiated;

        private bool disposing;
        private bool disposed;

        private readonly string UUID;

        private readonly Uri PUB_SUB_URI;

        private PubSubSettings _settings;

        private Stream stream;
        private Socket socket;

        private Mutex state_mutex;

        private Thread reader_thread;

        public WebSocketState state { get; private set; }


        public IPubSubSettings settings
        {
            get
            {
                return _settings;
            }
            set
            {
                value = settings;
            }
        }

        public
        PubSubClient()
        {
            state_mutex = new Mutex();
            SetState(WebSocketState.Connecting);

            polling = false;
            reading = false;
            handshake_initiated = false;

            disposing = false;
            disposed = false;

            UUID = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

            PUB_SUB_URI = new Uri("wss://pubsub-edge.twitch.tv");

            _settings = new PubSubSettings();

            WebSocket.RegisterPrefixes();
        }

        #region Connection and Connection Validation

        public void
        Connect()
        {
            if (_settings.connect_retry_count > _settings.connect_retry_count_limit)
            {
                return;
            }

            if (!SetState(WebSocketState.Connecting, true))
            {
                return;
            }

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(PUB_SUB_URI.Host, PUB_SUB_URI.Port);

            stream = new NetworkStream(socket);
            if (PUB_SUB_URI.Port == 443)
            {
                SslStream ssl_stream = new SslStream(stream, false, Callback_CertificateValidation, Callback_CertificateSelection);
                ssl_stream.AuthenticateAsClient(PUB_SUB_URI.DnsSafeHost, null, SslProtocols.Default, false);

                stream = ssl_stream;
            }

            WebSocketNetworkException exception;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(PUB_SUB_URI);
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

            _settings.connect_retry_count = 0;

            stream = response.GetResponseStream();

            reader_thread = new Thread(new ThreadStart(ReadStream));
            reader_thread.Start();

            SetState(WebSocketState.Open);            
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
        CallBack_FailedToConnect(WebSocketNetworkException exception)
        {
            stream.Close();
            stream.Dispose();
            stream = null;

            socket.Shutdown(SocketShutdown.Both);
            socket.Disconnect(false);
            socket.Close();
            socket = null;

            SetState(WebSocketState.Connecting, false);

            if(settings.handling_failed_to_connect == RetryHandling.Retry)
            {
                ++_settings.connect_retry_count;
                if (_settings.connect_retry_count > _settings.connect_retry_count_limit)
                {
                    // No further clean up needed since all other managed resources have been freed (except the state mutex).
                    OverrideState(WebSocketState.Closed);

                    _settings.SetNetworkError(new WebSocketNetworkException(NetworkError.RetryConnectLimitReached, "The maximum amount of failed connection attempts has been reached. Limit: " + _settings.connect_retry_count_limit, exception));
                }
                else
                {
                    _settings.WaitConnectionDelay();
                    Connect();
                }
            }
            else
            {
                OverrideState(WebSocketState.Closed);

                _settings.SetNetworkError(exception);
            }
        }

        public void
        Close(bool force_disconnect, bool reuse_client = false)
        {
            if (!SetState(WebSocketState.Closing, force_disconnect))
            {
                return;
            }

            WebSocketFrame frame = WebSocketFrame.CreateCloseFrame();
            Send(frame.EncodeData());

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

            // OnSocketDisconnected.Raise(this, EventArgs.Empty);

            Dispose(!reuse_client);

            SetState(WebSocketState.Closed);
            // OnDisconnected.Raise(this, EventArgs.Empty);
        }

        private bool
        ValidateHandShakeRequest(HttpWebRequest request, out WebSocketNetworkException exception)
        {
            exception = default;

            // These checks really shouldn't be needed, but just double check to make sure Microsoft didn't mess up.
            if (request.ProtocolVersion.Major < 1 || request.ProtocolVersion.Minor < 1)
            {
                exception = new WebSocketNetworkException(NetworkError.Hanshake_RequestProtocolVersion, "The handshake request protocol version must be at least HTTP 1.1.");

                return false;
            }

            string header_upgrade = request.Headers["Upgrade"];
            if (!header_upgrade.HasContent() || header_upgrade != "websocket")
            {
                exception = new WebSocketNetworkException(NetworkError.Hanshake_RequestHeader, "The handshake request did not contain the header \"Upgrade\", or the header was not set to \"websocket\".");

                return false;
            }

            string header_connection = request.Headers["Connection"];
            if (!header_connection.HasContent() || header_connection != "Upgrade")
            {
                exception = new WebSocketNetworkException(NetworkError.Hanshake_RequestHeader, "The handshake request did not contain the header \"Connection\", or the header was not set to \"Upgrade\".");

                return false;
            }

            string header_web_socket_key = request.Headers["Sec-WebSocket-Key"];
            if (!header_web_socket_key.HasContent())
            {
                exception = new WebSocketNetworkException(NetworkError.Hanshake_RequestHeader, "The handshake request did not contain the header \"Sec-WebSocket-Key\", or the header was empty or contianed only white space.");

                return false;
            }

            string header_web_socket_version = request.Headers["Sec-WebSocket-Version"];
            if (!header_web_socket_version.HasContent() || header_web_socket_version != "13")
            {
                exception = new WebSocketNetworkException(NetworkError.Hanshake_RequestHeader, "The handshake request did not contain the header \"Sec-WebSocket-Version\", or the header was not set to \"13\".");

                return false;
            }

            return true;
        }

        private bool
        ValidateHandShakeResponse(HttpWebRequest request, HttpWebResponse response, string uuid, out WebSocketNetworkException exception)
        {
            exception = default;

            if (response.StatusCode != HttpStatusCode.SwitchingProtocols)
            {
                exception = new WebSocketNetworkException(NetworkError.Hanshake_ResponseStatusCode, "The handshake response status code from " + PUB_SUB_URI.Host + " was not equal to \"101 - Switching Protocols\".");

                return false;
            }

            if (response.ProtocolVersion.Major < 1 || response.ProtocolVersion.Minor < 1)
            {
                exception = new WebSocketNetworkException(NetworkError.Hanshake_ResponseProtocolVersion, "The handshake response protocol version must be at least HTTP 1.1.");

                return false;
            }

            string server_key = response.Headers["Sec-WebSocket-Accept"];
            if (!server_key.HasContent())
            {
                exception = new WebSocketNetworkException(NetworkError.Hanshake_ResponseHeader, "The handshake response from " + PUB_SUB_URI.Host + " did not contain the header \"Sec-WebSocket-Accept\", or the header was empty or contianed only white space.");

                return false;
            }

            string client_key = request.Headers["Sec-WebSocket-Key"];
            string server_key_expected = CreateServerResponseKey(client_key, uuid);
            if (server_key != server_key_expected)
            {
                exception = new WebSocketNetworkException(NetworkError.Hanshake_ResponseHeader, "The handshake response header \"Sec-WebSocket-Accept\" did not matach the expected value: " + server_key_expected);

                return false;
            }

            string header_web_socket_version = response.Headers["Sec-WebSocket-Version"];
            if (!header_web_socket_version.IsNull() &&(!header_web_socket_version.HasContent() || header_web_socket_version != "13"))
            {
                exception = new WebSocketNetworkException(NetworkError.Hanshake_ResponseHeader, "The handshake response from " + PUB_SUB_URI.Host + " included the header \"Sec-WebSocket-Version\" and was not set to \"13\".");

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

            /*
            if (!socket.IsNull())
            {
                socket.Dispose();
            }
            */
            
            // This is the last step before the client fully disconnects, it's safe to call this here. 
            OverrideState(WebSocketState.Closed);

            if (!state_mutex.IsNull())
            {
                state_mutex.Dispose();
                state_mutex = null;
            }

            disposed = true;
            // OnDisposed.Raise(this, EventArgs.Empty);
        }

        #endregion

        #region State Handling

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

            bool success = false;

            if (!override_state)
            {
                switch (transition_state)
                {
                    case WebSocketState.Connecting: success = CanConnect();                         break;
                    case WebSocketState.Closing:    success = CanDisconnect();                      break;
                    case WebSocketState.Open:       success = state == WebSocketState.Connecting;   break;
                    case WebSocketState.Closed:      success = state == WebSocketState.Closing;      break;
                }
            }

            this.handshake_initiated = handshake_initiated;

            if (success)
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

            return success;
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
                        Debug.WriteLine("Cannot connect to " + PUB_SUB_URI.Host + ": currently connecting");
                        return false;
                    }

                    return true;
                }
                case WebSocketState.Open:       Debug.WriteLine("Cannot connect to " + PUB_SUB_URI.Host + ": already connected");       return false;
                case WebSocketState.Closing:    Debug.WriteLine("Cannot connect to " + PUB_SUB_URI.Host + ": currently disconnecting"); return false;
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
                case WebSocketState.Connecting: Debug.WriteLine("Cannot disconnect from " + PUB_SUB_URI.Host + ": currently connecting");   return false;
                case WebSocketState.Closing:    Debug.WriteLine("Cannot disconnect from " + PUB_SUB_URI.Host + ": already connecting");     return false;
                case WebSocketState.Closed:     Debug.WriteLine("Cannot disconnect from " + PUB_SUB_URI.Host + ": already disconnected");   return false;
            }

            return true;
        }

        #endregion

        #region Writing

        public bool
        Send(string message)
        {
            message = message.Trim();
            byte[] bytes = Encoding.UTF8.GetBytes(message);

            Send(Opcode.Text, new MemoryStream(bytes));

            // OnDataSent.Raise(this, new DataEventArgs(bytes, message));

            return true;
        }

        public bool
        Send(Opcode opcode, Stream stream)
        {
            long length = stream.Length;
            if (length == 0)
            {
                return Send(Fin.Final, opcode, new byte[0]);
            }

            int fragment_length = settings.frame_fragment_length_write;
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
            long fragments_count = (buffer.LongLength / fragment_length);
            long remainder = buffer.LongLength % fragment_length;
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

        public bool
        Send(Fin fin, Opcode opcode, byte[] data)
        {
            if (state != WebSocketState.Open)
            {
                return false;
            }

            WebSocketFrame frame = new WebSocketFrame(fin, opcode, data);

            return Send(frame.EncodeData());
        }

        public bool
        Send(byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();

            return true;
        }

        #endregion

        #region Reading

        private async void
        ReadStream()
        {
            // Header
            byte[] buffer_header = new byte[2];

            Fin fin;
            RSV rsv_1;
            RSV rsv_2;
            RSV rsv_3;
            Opcode opcode;
            Mask mask;
            byte length_payload;

            // Extended payload
            int length_payload_extended_byte_count = 0;
            byte[] buffer_extended_payload_length = new byte[0];

            // mask key
            byte[] buffer_mask_key = new byte[0];

            // data
            ulong data_length = 0;
            byte[] buffer_data = new byte[0];
            string data_string = string.Empty;

            Tuple<bool, int> read_result;

            reading = true;
            while (polling && !socket.IsNull() && !stream.IsNull())
            {
                // NOTE: This entire routine is one giant hack-fest. 
                //       This assumes that an entire message is sent with every frame, only does the most basic checking, and is in a very non-user-friendly format.
                //       All of this was to just get things working and off the ground. Now it's time to do everything properly.

                // ---------------------------------------------
                // Header Decoding
                // ---------------------------------------------

                read_result = await ReadStreamAsync(stream, buffer_header);
                if(!read_result.Item1)
                {
                    continue;
                }

                fin             = (buffer_header[0] & 0x80) == 0x80 ? Fin.Final : Fin.Fragment;
                rsv_1           = (buffer_header[0] & 0x40) == 0x40 ? RSV.On : RSV.Off;
                rsv_2           = (buffer_header[0] & 0x20) == 0x20 ? RSV.On : RSV.Off;
                rsv_3           = (buffer_header[0] & 0x10) == 0x10 ? RSV.On : RSV.Off;
                opcode          = (Opcode)(buffer_header[0] & 0x0f);
                mask            = (buffer_header[1] & 0x80) == 0x80 ? Mask.On : Mask.Off;
                length_payload  = (byte)(buffer_header[1] & 0x7f);

                // ---------------------------------------------
                // Extended Payload Length Decoding
                // ---------------------------------------------

                if (length_payload < 126)
                {
                    buffer_extended_payload_length = new byte[0];
                }
                else
                {
                    length_payload_extended_byte_count = length_payload == 126 ? 2 : 8;
                    buffer_extended_payload_length = new byte[length_payload_extended_byte_count];

                    read_result = await ReadStreamAsync(stream, buffer_extended_payload_length);
                    if (!read_result.Item1)
                    {
                        continue;
                    }
                }

                // ---------------------------------------------
                // Mask Key Decoding
                // ---------------------------------------------

                if (mask == Mask.Off)
                {
                    buffer_mask_key = new byte[0];
                }
                else
                {
                    buffer_mask_key = new byte[4];

                    read_result = await ReadStreamAsync(stream, buffer_mask_key);
                    if (!read_result.Item1)
                    {
                        continue;
                    }
                }

                // ---------------------------------------------
                // Data Decoding
                // ---------------------------------------------

                if (length_payload < 126)
                {
                    data_length = length_payload;
                }
                else if (length_payload == 126)
                {
                    data_length = buffer_extended_payload_length.ToUint16FromBigEndian();
                }
                else
                {
                    data_length = buffer_extended_payload_length.ToUint64FromBigEndian();
                }

                if (data_length == 0)
                {
                    buffer_data = new byte[0];

                    continue;
                }

                buffer_data = new byte[data_length];

                int fragment_length = settings.frame_fragment_length_read;
                if (buffer_data.LongLength <= fragment_length)
                {
                    read_result = await ReadStreamAsync(stream, buffer_data);
                    if (!read_result.Item1)
                    {
                        continue;
                    }
                }
                else
                {
                    long fragments_count = (buffer_data.LongLength / fragment_length);
                    if (buffer_data.LongLength % fragment_length > 0)
                    {
                        ++fragments_count;
                    }

                    bool success = false;

                    int offset = 0;
                    for (long index = 0; index < fragments_count; ++index)
                    {
                        read_result = await ReadStreamAsync(stream, buffer_data, offset, fragment_length);
                        if (!read_result.Item1)
                        {
                            break;
                        }

                        offset += fragment_length;

                        int temp = buffer_data.Length - offset;
                        fragment_length = temp < fragment_length ? temp : fragment_length;
                    }

                    if (!success)
                    {
                        continue;
                    }
                }                

                // TODO: Since this is a client, it should *never* receieve masked data from the server. Throw an error and disconnect if this is ever true.
                if (mask == Mask.On)
                {
                    for (long index = 0; index < buffer_data.LongLength; ++index)
                    {
                        buffer_data[index] = (byte)(buffer_data[index] ^ buffer_mask_key[index % 4]);
                    }
                }

                data_string = Encoding.UTF8.GetString(buffer_data);
                Debug.WriteLine(data_string);
            }

            reading = false;
        }

        private async Task<Tuple<bool, int>>
        ReadStreamAsync(Stream stream, byte[] buffer)
        {
            return await ReadStreamAsync(stream, buffer, 0, buffer.Length);
        }

        private async Task<Tuple<bool, int>>
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
                    _settings.SetNetworkError(new WebSocketNetworkException(NetworkError.Stream_Disposed, "The stream was disposed while attempting to read data.", exception));
                }

                return new Tuple<bool, int>(false, 0);
            }

            if (buffer_bytes_read_count == 0) 
            {
                _settings.SetNetworkError(new WebSocketNetworkException(NetworkError.Stream_ZeroBytesRead, "Zero bytes were ready from the buffer instead of the expected " + buffer.Length + " bytes. It is highly recommended to reconnect to the web socket."));

                return new Tuple<bool, int>(false, 0);
            }

            // This normally isn't inherantly an error, but for a web socket it's bad if the correct number of bytes isn't read.
            if (buffer_bytes_read_count != count)
            {
                _settings.SetNetworkError(new WebSocketNetworkException(NetworkError.Stream_BytesReadCount, buffer_bytes_read_count + " bytes were ready from the buffer instead of the expected " + buffer.Length + " bytes."));

                return new Tuple<bool, int>(false, buffer_bytes_read_count);
            }

            return new Tuple<bool, int>(true, buffer_bytes_read_count);
        }

        #endregion
    }

    #region Data Structures

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

        public readonly ulong length;
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

            length = (ulong)data.LongLength;
            if (length < 126)
            {
                length_payload = (byte)length;
                length_payload_extended = new byte[0];
            }
            else if (length <= ushort.MaxValue)
            {
                length_payload = 126;
                length_payload_extended = ((ushort)length).ToBigEndianByteArray();
            }
            else
            {
                length_payload = 127;
                length_payload_extended = length.ToBigEndianByteArray();
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

        public void
        MaskData(byte[] key)
        {
            for (ulong index = 0; index < length; ++index)
            {
                data[index] = (byte)(data[index] ^ key[index % 4]);
            }
        }

        public static WebSocketFrame
        CreateCloseFrame(string reason = "")
        {
            return CreateCloseFrame(1005, reason);
        }

        public static WebSocketFrame
        CreateCloseFrame(ushort code, string reason = "")
        {
            byte[] data = code.ToBigEndianByteArray();
            if (!reason.HasContent())
            {
                return new WebSocketFrame(Fin.Final, Opcode.Close, data);
            }

            List<byte> _data = new List<byte>(data.Length + reason.Length);
            _data.AddRange(data);
            _data.AddRange(Encoding.UTF8.GetBytes(reason));

            return new WebSocketFrame(Fin.Final, Opcode.Close, _data.ToArray());
        }

        public byte[]
        EncodeData()
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
    
    public interface
    IPubSubSettings
    {
        int frame_fragment_length_read { get; set; }
        int frame_fragment_length_write { get; set; }

        RetryHandling handling_failed_to_connect { get; set; }
        ErrorHandling handling_network_error { get; set; }
    }

    public class
    PubSubSettings : IPubSubSettings
    {
        private int[] connect_retry_delay_seconds;

        internal int connect_retry_count;
        internal int connect_retry_count_limit;

        public int frame_fragment_length_read { get; set; }
        public int frame_fragment_length_write { get; set; }

        public RetryHandling handling_failed_to_connect { get; set; }
        public ErrorHandling handling_network_error { get; set; }

        public PubSubSettings()
        {
            frame_fragment_length_read = 1024;
            frame_fragment_length_write = 1024;

            connect_retry_delay_seconds = new int[] { 1, 2, 4, 8, 16, 32, 64, 120 };

            connect_retry_count = 0;
            connect_retry_count_limit = connect_retry_delay_seconds.Length;

            handling_failed_to_connect = RetryHandling.Retry;
        }

        public void
        WaitConnectionDelay()
        {
            if (connect_retry_count < 1)
            {
                return;
            }

            connect_retry_count = connect_retry_count.ClampMax(connect_retry_count_limit);

            Thread.Sleep(connect_retry_delay_seconds[connect_retry_count - 1] * 1000);
        }

        public async Task
        WaitConnectionDelayAsync()
        {
            if (connect_retry_count < 1)
            {
                return;
            }

            connect_retry_count = connect_retry_count.ClampMax(connect_retry_count_limit);

            await Task.Delay(connect_retry_delay_seconds[connect_retry_count - 1] * 1000);
        }

        internal void
        SetNetworkError(WebSocketNetworkException exception)
        {
            if (handling_network_error == ErrorHandling.Return)
            {
                return;
            }

            throw exception;
        }
    }

    #endregion

    #region Errors and Erro Handling

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
    NetworkError
    {
        None = 0,

        Stream_Disposed,

        Stream_ZeroBytesRead,

        Stream_BytesReadCount,

        Hanshake_RequestProtocolVersion,

        Hanshake_RequestHeader,

        Hanshake_ResponseStatusCode,

        Hanshake_ResponseProtocolVersion,

        Hanshake_ResponseHeader,

        RetryConnectLimitReached
    }

    public class
    WebSocketNetworkException : Exception
    {
        public NetworkError error;

        public WebSocketNetworkException(NetworkError error, string message) : base(message)
        {
            this.error = error;
        }

        public WebSocketNetworkException(NetworkError error, string message, Exception inner_exception) : base(message, inner_exception)
        {
            this.error = error;
        }
    }

    #endregion
}
