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
        public delegate void WebSocketHandlerOpcode(in DateTime time, in WebSocketFrame[] frames, Opcode opcode, in byte[] buffer);

        private Dictionary<Opcode, WebSocketHandlerOpcode> handlers_web_socket_opcode;

        public void
        ResetWebSocketHandlers()
        {
            handlers_web_socket_opcode = new Dictionary<Opcode, WebSocketHandlerOpcode>();

            SetWebSocketHandler(Opcode.Text,    WebSocketHandler_Text);
            SetWebSocketHandler(Opcode.Close,   WebSocketHandler_Close);
            SetWebSocketHandler(Opcode.Ping,    WebSocketHandler_Ping);
            SetWebSocketHandler(Opcode.Pong,    WebSocketHandler_Pong);
        }

        public bool
        SetWebSocketHandler(Opcode opcode, WebSocketHandlerOpcode handler)
        {
            ExceptionUtil.ThrowIfNull(handler, nameof(handler));

            if (handlers_web_socket_opcode.IsNull())
            {
                return false;
            }

            handlers_web_socket_opcode[opcode] = handler;

            return true;
        }

        public bool
        RemoveWebSocketHandler(Opcode opcode)
        {
            if (handlers_web_socket_opcode.IsNull())
            {
                return true;
            }

            if (!handlers_web_socket_opcode.ContainsKey(opcode))
            {
                return false;
            }

            return handlers_web_socket_opcode.Remove(opcode);
        }

        private void
        RunMessageHandler(Opcode opcode, in DateTime time, in WebSocketFrame[] frames, in byte[] buffer)
        {
            if (handlers_web_socket_opcode.IsNull())
            {
                return;
            }

            if (!handlers_web_socket_opcode.ContainsKey(opcode))
            {
                return;
            }

            handlers_web_socket_opcode[opcode](time, frames, opcode, buffer);
        }

        private void
        WebSocketHandler_Text(in DateTime time, in WebSocketFrame[] frames, Opcode opcode, in byte[] buffer)
        {
            string message = Encoding.UTF8.GetString(buffer);
            Debug.WriteLine(message);

            OnWebSocketText.Raise(this, new MessageTextEventArgs(time, URI, frames, opcode, buffer, message, id));
        }

        private void
        WebSocketHandler_Close(in DateTime time, in WebSocketFrame[] frames, Opcode opcode, in byte[] buffer)
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

            OnWebsocketFrameClose.Raise(this, new FrameCloseEventArgs(time, URI, frames[0], id));

            Close(FrameSource.Server, true, frames[0]);
        }

        private void
        WebSocketHandler_Ping(in DateTime time, in WebSocketFrame[] frames, Opcode opcode, in byte[] buffer)
        {
            OnWebSocketFramePing.Raise(this, new FrameEventArgs(time, URI, frames[0], id));
        }

        private void
        WebSocketHandler_Pong(in DateTime time, in WebSocketFrame[] frames, Opcode opcode, in byte[] buffer)
        {
            OnWebSocketFramePong.Raise(this, new FrameEventArgs(time, URI, frames[0], id));
        }
    }
}