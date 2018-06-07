// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    RaidTags : UserNoticeTags, ITags
    {
        /// <summary>
        /// The number of viewers participating the the raid.
        /// </summary>
        [ValidateTag("msg-param-viewer-count")]
        public ulong    msg_param_viewer_count  { get; protected set; }

        /// <summary>
        /// <para>The display name of the channel that is raiding.</para>
        /// <para>This is empty if it was never set by the raider.</para>
        /// </summary>
        [ValidateTag("msg-param-display-name")]
        public string   msg_param_display_name  { get; protected set; }

        /// <summary>
        /// The login name of the channel that is raiding.
        /// </summary>
        [ValidateTag("msg-param-login")]
        public string   msg_param_login         { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="RaidTags"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public RaidTags(in IrcMessage message) : base(message)
        {
            if (!exist)
            {
                return;
            }

            msg_param_viewer_count  = TagsUtil.ToUInt32(message.tags, "msg-param-viewerCount");
            msg_param_display_name  = TagsUtil.ToString(message.tags, "msg-param-displayName");
            msg_param_login         = TagsUtil.ToString(message.tags, "msg-param-login");
        }
    }
}