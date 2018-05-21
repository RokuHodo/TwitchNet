// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Enums.Debugger;
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
        [ValidateMember(Check.IsValid)]
        public string client { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="NumericReplyEventArgs"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
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
