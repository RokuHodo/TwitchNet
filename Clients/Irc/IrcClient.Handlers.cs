// standard namespaces
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Debug;
using TwitchNet.Enums.Clients;
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc
{
    public partial class
    IrcClient
    {
        public delegate void MessageHandler(string message_raw, IrcMessage message_irc);

        private Dictionary<string, MessageHandler> handlers;

        public virtual void
        DefaultHandlers()
        {
            handlers.Clear();

            SetHandler("PING", HandlePing);
        }

        public void
        SetHandler(string command, MessageHandler handler)
        {
            ExceptionUtil.ThrowIfInvalid(command, nameof(command));
            ExceptionUtil.ThrowIfNull(handler, nameof(handler));

            if (handlers.IsNull())
            {
                return;
            }

            handlers[command] = handler;
        }

        public void
        RemoveHandler(string command)
        {
            ExceptionUtil.ThrowIfInvalid(command, nameof(command));

            if (handlers.IsNull())
            {
                return;
            }

            if (!handlers.ContainsKey(command))
            {
                return;
            }

            handlers.Remove(command);
        }

        private void
        RunHandler(string message_raw, IrcMessage message_irc)
        {
            if (handlers.IsNull())
            {
                return;
            }

            if (!handlers.ContainsKey(message_irc.command))
            {
                return;
            }

            handlers[message_irc.command](message_raw, message_irc);
        }

        private void
        HandlePing(string message_raw, IrcMessage message_irc)
        {
            Pong(message_irc);
        }
    }
}
