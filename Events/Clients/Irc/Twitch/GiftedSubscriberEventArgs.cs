// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Enums.Debugger;
using TwitchNet.Models.Clients.Irc;
using TwitchNet.Models.Clients.Irc.Twitch;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    GiftedSubscriberEventArgs : SubscriberEventArgs
    {
        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// <para>Check the <code>is_valid</code> property to determine if tags were attached to the message.</para>
        /// </summary>
        [ValidateMember(Check.Tags)]
        public new GiftedSubscriberTags tags { get; protected set; }

        public GiftedSubscriberEventArgs(IrcMessage message) : base(message)
        {
            tags = new GiftedSubscriberTags(message);
        }
    }
}
