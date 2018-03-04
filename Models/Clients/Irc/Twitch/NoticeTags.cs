// project namespaces
using TwitchNet.Enums.Clients.Irc.Twitch;
using TwitchNet.Extensions;
using TwitchNet.Interfaces.Clients.Irc;
using TwitchNet.Utilities;

namespace
TwitchNet.Models.Clients.Irc.Twitch
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
        public NoticeType   msg_id      { get; protected set; }

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