// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    ChatRoomJoinEventArgs : JoinEventArgs
    {
        /// <summary>
        /// The id of the user who the chat room belongs to.
        /// </summary>
        public string channel_user_id   { get; protected set; }

        /// <summary>
        /// The unique UUID of the chat room.
        /// </summary>
        public string channel_uuid      { get; protected set; }

        public ChatRoomJoinEventArgs(IrcMessage message) : base(message)
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
