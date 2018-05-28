// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    NoticeTags : ITags
    {
        /// <summary>
        /// Whether or not tags were attached to the message;
        /// </summary>
        public bool         is_valid    { get; protected set; }

        /// <summary>
        /// The id that describes the notice from the server.
        /// <para>Set to <see cref="UserNoticeType.None"/> if the id could not be parsed.</para>
        /// </summary>
        [ValidateTag("msg-id")]
        public NoticeType   msg_id      { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="NoticeTags"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public NoticeTags(IrcMessage message)
        {
            is_valid = message.tags.IsValid();
            if (!is_valid)
            {
                return;
            }

            msg_id = TagsUtil.ToNoticeType(message.tags, "msg-id");
        }
    }
}