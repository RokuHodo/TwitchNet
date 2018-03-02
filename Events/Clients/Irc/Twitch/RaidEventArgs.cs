// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    RaidEventArgs : UserNoticeEventArgs
    {
        /// <summary>
        /// The number of viewers participating the the raid.
        /// </summary>
        public ulong msg_param_viewer_count     { get; protected set; }

        /// <summary>
        /// <para>The display name of the channel that is raiding.</para>
        /// <para>This is empty if it was never set by the raider.</para>
        /// </summary>
        public string msg_param_display_name    { get; protected set; }

        /// <summary>
        /// The login name of the channel that is raiding.
        /// </summary>
        public string msg_param_login           { get; protected set; }

        public RaidEventArgs(UserNoticeEventArgs args) : base(args)
        {
            if (args.message_irc.tags.IsValid())
            {
                msg_param_viewer_count  = TagsUtil.ToUInt32(message_irc.tags, "msg-param-viewerCount");
                msg_param_display_name  = TagsUtil.ToString(message_irc.tags, "msg-param-displayName");
                msg_param_login         = TagsUtil.ToString(message_irc.tags, "msg-param-login");
            }
        }
    }
}
