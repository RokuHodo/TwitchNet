// standard namespaces
using System;

// project namespaces
using TwitchNet.Debugger;

namespace
TwitchNet.Clients.Irc
{
    public class
    IrcMessageEventArgs : EventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        [ValidateMember(Check.IsNotNullOrDefault)]
        public IrcMessage irc_message { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="IrcMessageEventArgs"/> class.
        /// </summary>
        /// <param name="message">The parsed IRC message.</param>
        public IrcMessageEventArgs(in IrcMessage message)
        {
            irc_message = message;
        }
    }
}
