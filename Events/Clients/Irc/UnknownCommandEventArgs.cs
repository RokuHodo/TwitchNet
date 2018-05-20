// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Enums.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;

namespace
TwitchNet.Events.Clients.Irc
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
