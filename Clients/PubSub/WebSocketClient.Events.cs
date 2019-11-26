// standard namespaces
using System;

namespace
TwitchNet.Clients.PubSub
{
    public partial class
    WebSocketClient : IDisposable
    {
        public virtual event EventHandler<WebSocketEventArgs>               OnOpen;

        public virtual event EventHandler<CloseEventArgs>                   OnClose;

        public virtual event EventHandler<EventArgs>                        OnDisposed;

        public virtual event EventHandler<FrameEventArgs>                   OnWebSocketFrame;

        public virtual event EventHandler<FrameEventArgs>                   OnWebsocketFrameClose;

        public virtual event EventHandler<FrameEventArgs>                   OnWebSocketFramePing;

        public virtual event EventHandler<FrameEventArgs>                   OnWebSocketFramePong;

        public virtual event EventHandler<MessageTextEventArgs>             OnWebSocketText;

        public virtual event EventHandler<WebSocketErrorEventArgs>          OnError;

        public virtual event EventHandler<WebSocketNetworkErrorEventArgs>   OnNetworkError;
    }

    public class
    WebSocketEventArgs : EventArgs
    {
        public DateTime time { get; }

        public Uri uri { get; }

        public string web_socket_id { get; }

        public
        WebSocketEventArgs(in DateTime time, Uri uri, string web_socket_id = "")
        {
            this.time = time;
            this.uri = uri;
            this.web_socket_id = web_socket_id;
        }

        public
        WebSocketEventArgs(WebSocketEventArgs agrs)
        {
            time = agrs.time;
            uri = agrs.uri;
            web_socket_id = agrs.web_socket_id;
        }
    }

    public class
    FrameEventArgs : WebSocketEventArgs
    {
        public WebSocketFrame frame { get; }

        public
        FrameEventArgs(in DateTime time, Uri uri, in WebSocketFrame frame, string id = "") : base(time, uri, id)
        {
            this.frame = frame;
        }
    }

    public class
    FrameCloseEventArgs : FrameEventArgs
    {
        public CloseStatusCode status_code { get; }

        public string reason { get; }

        public
        FrameCloseEventArgs(in DateTime time, Uri uri, in WebSocketFrame frame, string id = "") : base(time, uri, frame, id)
        {
            status_code = frame.payload.close_status_code;
            reason = frame.payload.close_reason;
        }
    }

    public class
    MessageTextEventArgs : WebSocketEventArgs
    {
        public WebSocketFrame[] frames { get; }

        public Opcode opcode { get; }

        public byte[] data { get; }

        public string message { get; }

        public
        MessageTextEventArgs(in DateTime time, Uri uri, in WebSocketFrame[] frames, Opcode opcode, in byte[] data, string message, string id = "") : base(time, uri, id)
        {
            this.frames = frames;
            this.opcode = opcode;
            this.data = data;
            this.message = message;
        }
    }

    public class
    CloseEventArgs : FrameCloseEventArgs
    {
        public FrameSource source { get; }

        public
        CloseEventArgs(in DateTime time, Uri uri, in WebSocketFrame frame, FrameSource source, string id = "") : base(time, uri, frame, id)
        {
            this.source = source;
        }
    }

    public class
    WebSocketErrorEventArgs : WebSocketEventArgs
    {
        public WebSocketException exception { get; }

        public
        WebSocketErrorEventArgs(in DateTime time, Uri uri, WebSocketException exception, string id = "") : base(time, uri, id)
        {
            this.exception = exception;
        }
    }

    public class
    WebSocketNetworkErrorEventArgs : WebSocketEventArgs
    {
        public WebSocketNetworkException exception { get; }

        public
        WebSocketNetworkErrorEventArgs(in DateTime time, Uri uri, WebSocketNetworkException exception, string id = "") : base(time, uri, id)
        {
            this.exception = exception;
        }
    }

    public enum
    FrameSource
    {
        Client = 0,

        Server
    }
}