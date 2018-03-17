namespace TwitchNet.Enums.Clients.Irc.Twitch
{
    public enum
    MessageSource
    {
        /// <summary>
        /// The message was sent from a place other than the stream chat or a chat room.
        /// </summary>
        Other       = 0,

        /// <summary>
        /// The privmsg was sent from the strema chat.
        /// </summary>
        StreamChat  = 1,

        /// <summary>
        /// The privmsg was sent from a chat room.
        /// </summary>
        ChatRoom    = 2
    }
}
