// standard namespaces

// project namespaces
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
        public string motd { get; protected set; }

        public MotdEventArgs(IrcMessage message) : base(message)
        {
            motd = message.trailing;
        }
    }
}
