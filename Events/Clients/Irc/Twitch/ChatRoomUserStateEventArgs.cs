// project namespaces
using TwitchNet.Models.Clients.Irc;
using TwitchNet.Models.Clients.Irc.Twitch;
using TwitchNet.Extensions;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    ChatRoomUserStateEventArgs : UserStateEventArgs
    {
        /// <summary>
        /// The id of the user who the chat room belongs to.
        /// </summary>
        public string channel_user_id { get; protected set; }

        /// <summary>
        /// The unique UUID of the chat room.
        /// </summary>
        public string channel_uuid { get; protected set; }

        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// <para>Check the <code>is_valid</code> property to determine if tags were attached to the message.</para>
        /// </summary>
        public new ChatRoomUserStateTags tags { get; protected set; }

        public ChatRoomUserStateEventArgs(IrcMessage message) : base(message)
        {
            channel_user_id = channel.TextBetween(':', ':');

            int index = channel.LastIndexOf(':');
            if (index != -1)
            {
                channel_uuid = channel.TextAfter(':', index);
            }

            tags = new ChatRoomUserStateTags(base.tags);
        }
    }
}