// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    NoticeTags
    {
        /// <summary>
        /// The id that describes the notice from the server.
        /// <para>Set to <see cref="UserNoticeType.Other"/> if the id could not be parsed.</para>
        /// </summary>
        [IrcTag("msg-id")]
        public NoticeType   msg_id  { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="NoticeTags"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public NoticeTags(in IrcMessage message)
        {
            msg_id = TwitchIrcUtil.Tags.ToNoticeType(message, "msg-id");
        }
    }
}