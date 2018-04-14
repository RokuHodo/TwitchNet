// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc.Twitch;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    ChatRoomPrivmsgEventArgs : StreamChatPrivmsgEventArgs
    {
        /// <summary>
        /// The id of the user who the chat room belongs to.
        /// </summary>
        public      string              channel_user_id { get; protected set; }

        /// <summary>
        /// The unique UUID of the chat room.
        /// </summary>
        public      string              channel_uuid    { get; protected set; }

        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// <para>Check the <code>is_valid</code> property to determine if tags were attached to the message.</para>
        /// </summary>
        public new  ChatRoomPrivmsgTags tags            { get; protected set; }

        public ChatRoomPrivmsgEventArgs(PrivmsgEventArgs args) : base(args)
        {
            channel_user_id = channel.TextBetween(':', ':');

            int index = channel.LastIndexOf(':');
            if (index != -1)
            {
                channel_uuid = channel.TextAfter(':', index);
            }

            tags = new ChatRoomPrivmsgTags(base.tags);
        }
    }
}