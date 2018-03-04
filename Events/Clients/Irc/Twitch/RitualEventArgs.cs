// project namespaces
using TwitchNet.Models.Clients.Irc.Twitch;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    RitualEventArgs : UserNoticeEventArgs
    {
        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// <para>Check the <code>is_valid</code> property to determine if tags were attached to the message.</para>
        /// </summary>
        public new RitualTags tags { get; protected set; }

        public RitualEventArgs(UserNoticeEventArgs args) : base(args)
        {
            tags = new RitualTags(args);
        }
    }
}
