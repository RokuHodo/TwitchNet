namespace
TwitchNet.Clients.Irc.Twitch
{
    public interface
    ISharedRoomStateTags : ITags
    {
        /// <summary>
        /// Whether or not the room is in emote only mode.
        /// </summary>
        bool    emote_only  { get; }

        /// <summary>
        /// Whether or r9k mode is enabled.
        /// When enabled, messages 9 characters or longer must be unique from other messages.
        /// </summary>
        bool    r9k         { get; }

        /// <summary>
        /// <para>How frequently, in seconds, non-elevated users can send messages.</para>
        /// <para>Set to 0 if slow mode is disabled.</para>
        /// </summary>
        uint    slow        { get; }

        /// <summary>
        /// The id of the room whose state has changed and/or the client has joined.
        /// </summary>
        string  room_id     { get; }
    }
}