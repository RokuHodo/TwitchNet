// standard namespaces
using TwitchNet.Debugger;
using TwitchNet.Enums.Debugger;
using TwitchNet.Models.Clients.Irc;

namespace
TwitchNet.Events.Clients.Irc
{
    public class
    MotdEventArgs : NumericReplyEventArgs
    {
        /// <summary>
        /// The IRC server's message of the day.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string motd { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="MotdEventArgs"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public MotdEventArgs(IrcMessage message) : base(message)
        {
            motd = message.trailing;
        }
    }
}
