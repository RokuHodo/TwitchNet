// project namespaces
using TwitchNet.Debugger;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    RaidEventArgs : UserNoticeEventArgs
    {
        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// <para>Check the <code>exist</code> property to determine if tags were attached to the message.</para>
        /// </summary>
        [ValidateMember(Check.Tags)]
        public new RaidTags tags { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="RaidEventArgs"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public RaidEventArgs(IrcMessage message) : base(message)
        {
            tags = new RaidTags(message);
        }
    }
}
