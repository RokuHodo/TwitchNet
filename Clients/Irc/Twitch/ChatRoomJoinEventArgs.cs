// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    ChatRoomJoinEventArgs : JoinEventArgs
    {
        /// <summary>
        /// The id of the user who the chat room belongs to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel_user_id   { get; protected set; }

        /// <summary>
        /// The unique UUID of the chat room.
        /// </summary>
        [ValidateMember(Check.RegexIsMatch, TwitchIrcUtil.REGEX_PATTERN_UUID)]
        public string channel_uuid      { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="ChatRoomJoinEventArgs"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public ChatRoomJoinEventArgs(in IrcMessage message) : base(message)
        {
            channel_user_id = channel.TextBetween(':', ':');

            int index = channel.LastIndexOf(':');
            if (index != -1)
            {
                channel_uuid = channel.TextAfter(':', index);
            }
        }
    }
}
