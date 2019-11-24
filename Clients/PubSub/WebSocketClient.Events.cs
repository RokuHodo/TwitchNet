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

        public virtual event EventHandler<FrameEventArgs>                   OnFrame;

        public virtual event EventHandler<FrameEventArgs>                   OnFrameClose;

        public virtual event EventHandler<FrameEventArgs>                   OnFramePing;

        public virtual event EventHandler<FrameEventArgs>                   OnFramePong;

        public virtual event EventHandler<MessageTextEventArgs>             OnMessageText;

        public virtual event EventHandler<WebSocketErrorEventArgs>          OnError;

        public virtual event EventHandler<WebSocketNetworkErrorEventArgs>   OnNetworkError;
    }

    public class
    WebSocketEventArgs : EventArgs
    {
        public DateTime time { get; }

        public Uri uri { get; }

        public string nonce { get; }

        public
        WebSocketEventArgs(in DateTime time, Uri uri, string nonce = "")
        {
            this.time = time;
            this.uri = uri;
            this.nonce = nonce;
        }
    }

    public class
    FrameEventArgs : WebSocketEventArgs
    {
        public WebSocketFrame frame { get; }

        public
        FrameEventArgs(in DateTime time, Uri uri, in WebSocketFrame frame, string nonce = "") : base(time, uri, nonce)
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
        FrameCloseEventArgs(in DateTime time, Uri uri, in WebSocketFrame frame, string nonce = "") : base(time, uri, frame, nonce)
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
        MessageTextEventArgs(in DateTime time, Uri uri, in WebSocketFrame[] frames, Opcode opcode, in byte[] data, string message, string nonce = "") : base(time, uri, nonce)
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
        CloseEventArgs(in DateTime time, Uri uri, in WebSocketFrame frame, FrameSource source, string nonce = "") : base(time, uri, frame, nonce)
        {
            this.source = source;
        }
    }

    public class
    WebSocketErrorEventArgs : WebSocketEventArgs
    {
        public WebSocketException exception { get; }

        public
        WebSocketErrorEventArgs(in DateTime time, Uri uri, WebSocketException exception, string nonce = "") : base(time, uri, nonce)
        {
            this.exception = exception;
        }
    }

    public class
    WebSocketNetworkErrorEventArgs : WebSocketEventArgs
    {
        public WebSocketNetworkException exception { get; }

        public
        WebSocketNetworkErrorEventArgs(in DateTime time, Uri uri, WebSocketNetworkException exception, string nonce = "") : base(time, uri, nonce)
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