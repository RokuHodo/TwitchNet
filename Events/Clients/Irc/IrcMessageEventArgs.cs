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
        public IrcMessage message { get; protected set; }

        public IrcMessageEventArgs(IrcMessage message)
        {
            this.message = message;
        }
    }
}
