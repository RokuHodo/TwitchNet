﻿// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;

namespace
TwitchNet.Events.Clients.Irc
{
    public class
    UnknownCommandEventArgs : NumericReplyEventArgs
    {
        public string command       { get; protected set; }

        public string description   { get; protected set; }

        public UnknownCommandEventArgs(IrcMessage message) : base(message)
        {
            description = message.trailing;

            if (!message.parameters.IsValid())
            {
                return;
            }

            if(message.parameters.Length < 2)
            {
                return;
            }

            command = message.parameters[1];
        }
    }
}
