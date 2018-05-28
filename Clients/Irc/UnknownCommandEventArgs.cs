// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;

namespace
TwitchNet.Clients.Irc
{
    public class
    UnknownCommandEventArgs : NumericReplyEventArgs
    {
        /// <summary>
        /// The unsupported IRC command.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string command       { get; protected set; }

        /// <summary>
        /// The error description.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string description   { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="UnknownCommandEventArgs"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public UnknownCommandEventArgs(IrcMessage message) : base(message)
        {
            description = message.trailing;

            if (!message.parameters.IsValid() || message.parameters.Length < 2)
            {
                return;
            }

            command = message.parameters[1];
        }
    }
}
