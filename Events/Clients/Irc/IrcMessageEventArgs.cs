﻿// standard namespaces
using System;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Enums.Debugger;
using TwitchNet.Models.Clients.Irc;

namespace
TwitchNet.Events.Clients.Irc
{
    public class
    IrcMessageEventArgs : EventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        [ValidateMember(Check.IsNotNullOrDefault)]
        public IrcMessage irc_message { get; protected set; }

        public IrcMessageEventArgs(IrcMessage message)
        {
            irc_message = message;
        }
    }
}
