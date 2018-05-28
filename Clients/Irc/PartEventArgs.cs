// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;

namespace
TwitchNet.Clients.Irc
{
    public class
    PartEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The nick of the client who left the channel.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string nick      { get; protected set; }

        /// <summary>
        /// The IRC channel the client has left.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel   { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="PartEventArgs"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public PartEventArgs(IrcMessage message) : base(message)
        {
            nick = message.server_or_nick;

            if (!message.parameters.IsValid())
            {
                return;
            }

            channel = message.parameters[0];
        }
    }
}
