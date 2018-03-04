// project namespaces
using TwitchNet.Events.Clients.Irc.Twitch;
using TwitchNet.Interfaces.Clients.Irc;
using TwitchNet.Utilities;

namespace
TwitchNet.Models.Clients.Irc.Twitch
{
    public class
    RaidTags : UserNoticeTags, ITags
    {
        /// <summary>
        /// The number of viewers participating the the raid.
        /// </summary>
        public ulong    msg_param_viewer_count  { get; protected set; }

        /// <summary>
        /// <para>The display name of the channel that is raiding.</para>
        /// <para>This is empty if it was never set by the raider.</para>
        /// </summary>
        public string   msg_param_display_name  { get; protected set; }

        /// <summary>
        /// The login name of the channel that is raiding.
        /// </summary>
        public string   msg_param_login         { get; protected set; }

        public RaidTags(UserNoticeEventArgs args) : base(args)
        {
            if (!is_valid)
            {
                return;
            }

            msg_param_viewer_count  = TagsUtil.ToUInt32(args.message_irc.tags, "msg-param-viewerCount");
            msg_param_display_name  = TagsUtil.ToString(args.message_irc.tags, "msg-param-displayName");
            msg_param_login         = TagsUtil.ToString(args.message_irc.tags, "msg-param-login");
        }
    }
}