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
    WebSocketClient : IDisposable
    {
        public delegate void MessageHandler(in DateTime time, in WebSocketFrame[] frames, Opcode opcode, in byte[] buffer);

        Dictionary<Opcode, MessageHandler> handlers = new Dictionary<Opcode, MessageHandler>();

        public void
        ResetMessageHandlers()
        {
            handlers = new Dictionary<Opcode, MessageHandler>();

            SetMessageHandler(Opcode.Text,  MessageHandler_Text);
            SetMessageHandler(Opcode.Close, MessageHandler_Close);
            SetMessageHandler(Opcode.Ping,  MessageHandler_Ping);
            SetMessageHandler(Opcode.Pong,  MessageHandler_Pong);
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
        RunMessageHandler(Opcode opcode, in DateTime time, in WebSocketFrame[] frames, in byte[] buffer)
        {
            if (handlers.IsNull())
            {
                return;
            }

            if (!handlers.ContainsKey(opcode))
            {
                return;
            }

            handlers[opcode](time, frames, opcode, buffer);
        }

        private void
        MessageHandler_Text(in DateTime time, in WebSocketFrame[] frames, Opcode opcode, in byte[] buffer)
        {
            string message = Encoding.UTF8.GetString(buffer);
            Debug.WriteLine(message);

            OnMessageText.Raise(this, new MessageTextEventArgs(time, URI, frames, opcode, buffer, message, id));
        }

        private void
        MessageHandler_Close(in DateTime time, in WebSocketFrame[] frames, Opcode opcode, in byte[] buffer)
        {
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

            frames[0].payload.close_status_code = (CloseStatusCode)status_code;
            frames[0].payload.close_reason = reason;

            OnFrameClose.Raise(this, new FrameCloseEventArgs(time, URI, frames[0], id));

            Close(FrameSource.Server, true, frames[0]);
        }

        private void
        MessageHandler_Ping(in DateTime time, in WebSocketFrame[] frames, Opcode opcode, in byte[] buffer)
        {
            OnFramePing.Raise(this, new FrameEventArgs(time, URI, frames[0], id));
        }

        private void
        MessageHandler_Pong(in DateTime time, in WebSocketFrame[] frames, Opcode opcode, in byte[] buffer)
        {
            OnFramePong.Raise(this, new FrameEventArgs(time, URI, frames[0], id));
        }
    }
}