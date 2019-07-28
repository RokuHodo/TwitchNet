// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    ChatRoomRoomStateEventArgs : RoomStateEventArgs
    {
        /// <summary>
        /// The id of the user who the chat room belongs to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string                       channel_user_id { get; protected set; }

        /// <summary>
        /// The unique UUID of the chat room.
        /// </summary>
        [ValidateMember(Check.RegexIsMatch, TwitchIrcUtil.REGEX_PATTERN_UUID)]
        public string                       channel_uuid    { get; protected set; }

        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// <para>Check the <code>exist</code> property to determine if tags were attached to the message.</para>
        /// </summary>
        [ValidateMember(Check.TagsMissing)]
        public new ChatRoomRoomStateTags    tags            { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="ChatRoomRoomStateEventArgs"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public ChatRoomRoomStateEventArgs(in IrcMessage message) : base(message)
        {
            channel_user_id = channel.TextBetween(':', ':');

            int index = channel.LastIndexOf(':');
            if (index != -1)
            {
                channel_uuid = channel.TextAfter(':', index);
            }

            tags = new ChatRoomRoomStateTags(base.tags);
        }
    }
}
