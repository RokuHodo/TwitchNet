﻿// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;

namespace
TwitchNet.Events.Clients.Irc
{
    public class
    NumericReplyEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The IRC client nick.
        /// </summary>
        public string client { get; protected set; }

        public NumericReplyEventArgs(IrcMessage message) : base(message)
        {
            if (!message.parameters.IsValid())
            {
                return;
            }

            client = message.parameters[0];
        }
    }
}
