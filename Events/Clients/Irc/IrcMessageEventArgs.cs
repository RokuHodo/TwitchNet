// standard namespaces
using System;

// project namespaces
using TwitchNet.Models.Clients.Irc;

namespace
TwitchNet.Events.Clients.Irc
{
    public class
    IrcMessageEventArgs : EventArgs
    {
        /// <summary>
        /// The irc message.
        /// </summary>
        public IrcMessage message_irc { get; protected set; }

        public IrcMessageEventArgs(IrcMessage message)
        {
            message_irc = message;
        }
    }
}
