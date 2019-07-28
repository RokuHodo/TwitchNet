// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    RaidTags : UserNoticeTags
    {
        /// <summary>
        /// The number of viewers participating the the raid.
        /// </summary>
        [IrcTag("msg-param-viewer-count")]
        public ulong    msg_param_viewer_count  { get; protected set; }

        /// <summary>
        /// <para>The display name of the channel that is raiding.</para>
        /// <para>This is empty if it was never set by the raider.</para>
        /// </summary>
        [IrcTag("msg-param-display-name")]
        public string   msg_param_display_name  { get; protected set; }

        /// <summary>
        /// The login name of the channel that is raiding.
        /// </summary>
        [IrcTag("msg-param-login")]
        public string   msg_param_login         { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="RaidTags"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public RaidTags(in IrcMessage message) : base(message)
        {
            msg_param_viewer_count  = TwitchIrcUtil.Tags.ToUInt32(message, "msg-param-viewerCount");
            msg_param_display_name  = TwitchIrcUtil.Tags.ToString(message, "msg-param-displayName");
            msg_param_login         = TwitchIrcUtil.Tags.ToString(message, "msg-param-login");
        }
    }
}