// project namespaces
using TwitchNet.Models.Clients.Irc.Twitch;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    SubscriberEventArgs : UserNoticeEventArgs
    {
        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// <para>Check the <code>is_valid</code> property to determine if tags were attached to the message.</para>
        /// </summary>
        public new SubscriberTags tags { get; protected set; }

        public SubscriberEventArgs(UserNoticeEventArgs args) : base(args)
        {
            tags = new SubscriberTags(args);
        }
    }
}
