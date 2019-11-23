// standard namespaces
using System;
using System.Collections.Generic;
using System.Text;

// projetc namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.PubSub
{
    public partial class
    PubSubClient : IDisposable
    {
        public delegate void MessageHandler(in DateTime time, in WebSocketFrame[] frames, Opcode opcode, in byte[] buffer);

        Dictionary<Opcode, MessageHandler> handlers = new Dictionary<Opcode, MessageHandler>();

        public virtual event EventHandler<EventArgs> OnOpen;

        public virtual event EventHandler<CloseEventArgs> OnClose;

        public virtual event EventHandler<EventArgs> OnDisposed;

        public virtual event EventHandler<FrameEventArgs> OnFrameSent;

        public virtual event EventHandler<FrameEventArgs> OnFrameReceived;

        public virtual event EventHandler<MessageEventArgs> OnMessage;

        public virtual event EventHandler<MessageTextEventArgs> OnMessageText;

        public virtual event EventHandler<MessageCloseEventArgs> OnMessageClose;

        public virtual event EventHandler<NetworkErrorEventArgs> OnNetworkError;

        public void
        ResetMessageHandlers()
        {
            handlers = new Dictionary<Opcode, MessageHandler>();

            SetMessageHandler(Opcode.Text,  MessageHandler_Text);
            SetMessageHandler(Opcode.Close, MessageHandler_Close);
        }

        public bool
        SetMessageHandler(Opcode opcode, MessageHandler handler)
        {
            ExceptionUtil.ThrowIfNull(handler, nameof(handler));

            if (handlers.IsNull())
            {
                return false;
            }

            handlers[opcode] = handler;

            return true;
        }

        public bool
        RemoveMessageHandler(Opcode opcode)
        {
            if (handlers.IsNull())
            {
                return true;
            }

            if (!handlers.ContainsKey(opcode))
            {
                return false;
            }

            return handlers.Remove(opcode);
        }

        private void
        RunHandler(Opcode opcode, MessageEventArgs args)
        {
            if (handlers.IsNull())
            {
                return;
            }

            if (!handlers.ContainsKey(opcode))
            {
                return;
            }

            if (args.frames.Length < 0)
            {
                return;
            }

            handlers[opcode](args.time, args.frames, opcode, args.data);
        }

        private void
        MessageHandler_Text(in DateTime time, in WebSocketFrame[] frames, Opcode opcode, in byte[] buffer)
        {
            string message = Encoding.UTF8.GetString(buffer);
            Debug.WriteLine(message);

            OnMessageText.Raise(this, new MessageTextEventArgs(time, frames, opcode, buffer, message));
        }

        private void
        MessageHandler_Close(in DateTime time, in WebSocketFrame[] frames, Opcode opcode, in byte[] buffer)
        {
            if(frames.Length > 1)
            {
                Debug.WriteWarning(ErrorLevel.Minor, "More than one close frame was sent from the web socket server.");
            }

            ushort status_code = 0;
            string reason = string.Empty;

            if (buffer.Length >= 2)
            {
                status_code = buffer.Slice(0, 2).ToUint16FromBigEndian();

                if (buffer.Length > 2)
                {
                    reason = Encoding.UTF8.GetString(buffer.Slice(2));
                }
            }

            OnMessageClose.Raise(this, new MessageCloseEventArgs(time, frames, opcode, buffer, status_code, reason));

            frames[0].payload.close_status_code = (CloseStatusCode)status_code;
            frames[0].payload.close_reason = reason;

            Close(FrameSource.Server, true, frames[0]);
        }
    }

    public class
    FrameEventArgs : EventArgs
    {
        public DateTime time { get; }

        public WebSocketFrame frame { get; }

        public
        FrameEventArgs(in DateTime time, in WebSocketFrame frame)
        {
            this.time = time;
            this.frame = frame;
        }
    }

    public class
    MessageEventArgs : EventArgs
    {
        public DateTime time { get; }

        public WebSocketFrame[] frames { get; }

        public Opcode opcode { get; }

        public byte[] data { get; }

        public
        MessageEventArgs(in DateTime time, in WebSocketFrame[] frames, Opcode opcode, in byte[] data)
        {
            this.time = time;
            this.frames = frames;
            this.opcode = opcode;
            this.data = data;
        }
    }

    public class
    MessageTextEventArgs : MessageEventArgs
    {
        public string message { get; }

        public
        MessageTextEventArgs(in DateTime time, in WebSocketFrame[] frames, Opcode opcode, in byte[] data, string message) : base(time, frames, opcode, data)
        {
            this.message = message;
        }
    }

    public class
    MessageCloseEventArgs : MessageEventArgs
    {
        public ushort code { get; }

        public string reason { get; }

        public
        MessageCloseEventArgs(in DateTime time, in WebSocketFrame[] frames, Opcode opcode, in byte[] data, ushort code, string reason) : base(time, frames, opcode, data)
        {
            this.code = code;
            this.reason = reason;
        }
    }

    public class
    CloseEventArgs : EventArgs
    {
        public DateTime time { get; }

        public WebSocketFrame frame { get; }

        public FrameSource source { get; }

        public
        CloseEventArgs(in DateTime time, in WebSocketFrame frame, FrameSource source)
        {
            this.time = time;
            this.frame = frame;
            this.source = source;
        }
    }

    public class
    NetworkErrorEventArgs : EventArgs
    {
        public DateTime time { get; }

        public WebSocketNetworkException exception { get; }

        public
        NetworkErrorEventArgs(in DateTime time, WebSocketNetworkException exception)
        {
            this.time = time;
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