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

        public MotdEventArgs(IrcMessage message) : base(message)
        {
            motd = message.trailing;
        }
    }
}
