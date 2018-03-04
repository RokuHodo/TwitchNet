// project namespaces
using TwitchNet.Models.Clients.Irc.Twitch;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    RaidEventArgs : UserNoticeEventArgs
    {
        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// <para>Check the <code>is_valid</code> property to determine if tags were attached to the message.</para>
        /// </summary>
        public new RaidTags tags { get; protected set; }

        public RaidEventArgs(UserNoticeEventArgs args) : base(args)
        {
            tags = new RaidTags(args);
        }
    }
}
