// project namespaces
using TwitchNet.Extensions;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    ChatRoomRoomModsEventArgs : RoomModsEventArgs
    {
        /// <summary>
        /// The id of the user who the chat room belongs to.
        /// </summary>
        public string channel_user_id { get; protected set; }

        /// <summary>
        /// The unique UUID of the chat room.
        /// </summary>
        public string channel_uuid { get; protected set; }

        public ChatRoomRoomModsEventArgs(NoticeEventArgs args) : base(args)
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