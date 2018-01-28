// standard namespaces
using System;

// project namespaces
using TwitchNet.Models.Clients.Irc;

namespace
TwitchNet.Events.Clients.Irc
{
    public class
    MotdEventArgs : NumericReplyEventArgs
    {
        public string motd { get; protected set; }

        public MotdEventArgs(IrcMessage message) : base(message)
        {
            motd = message.trailing;
        }
    }
}
