// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    ChatRoomRoomStateTags : ISharedRoomStateTags
    {
        /// <summary>
        /// Whether or not tags were attached to the message;
        /// </summary>
        public bool             exist           { get; protected set; }

        /// <summary>
        /// Whether or not the room is in emote only mode.
        /// </summary>
        [IrcTag("emote-only")]
        public bool             emote_only      { get; protected set; }

        /// <summary>
        /// Whether or r9k mode is enabled.
        /// When enabled, messages 9 characters or longer must be unique from other messages.
        /// </summary>
        [IrcTag("r9k")]
        public bool             r9k             { get; protected set; }

        /// <summary>
        /// <para>How frequently, in seconds, non-elevated users can send messages.</para>
        /// <para>Set to 0 if slow mode is disabled.</para>
        /// </summary>
        [IrcTag("slow")]
        public uint             slow            { get; protected set; }

        /// <summary>
        /// The id of the room whose state has changed and/or the client has joined.
        /// </summary>
        [IrcTag("room-id")]
        public string           room_id         { get; protected set; }

        /// <summary>
        /// <para>
        /// Bitfield enum. Contains the room state(s) that changed.
        /// Check this to see which room state fields are valid.
        /// </para>
        /// <para>Set to <see cref="RoomStateType.None"/> if no room states have changed. This should never be the case.</para>
        /// </summary>
        public RoomStateType    changed_states  { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="ChatRoomRoomStateTags"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public ChatRoomRoomStateTags(in IrcMessage message)
        {
            exist = message.tags.IsValid();
            if (!exist)
            {
                return;
            }

            if (TwitchIrcUtil.Tags.IsTagValid(message, "emote-only"))
            {
                emote_only = TwitchIrcUtil.Tags.ToBool(message, "emote-only");
                changed_states |= RoomStateType.EmoteOnly;
            }

            if (TwitchIrcUtil.Tags.IsTagValid(message, "r9k"))
            {
                r9k = TwitchIrcUtil.Tags.ToBool(message, "r9k");
                changed_states |= RoomStateType.R9K;
            }

            if (TwitchIrcUtil.Tags.IsTagValid(message, "slow"))
            {
                slow = TwitchIrcUtil.Tags.ToUInt32(message, "slow");
                changed_states |= RoomStateType.Slow;
            }

            room_id = TwitchIrcUtil.Tags.ToString(message, "room-id");
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ChatRoomRoomStateTags"/> class.
        /// </summary>
        /// <param name="tags">The tags to copy the values from.</param>
        public ChatRoomRoomStateTags(RoomStateTags tags)
        {
            exist = tags.exist;
            if (!exist)
            {
                return;
            }

            emote_only      = tags.emote_only;
            r9k             = tags.r9k;
            slow            = tags.slow;

            changed_states  = tags.changed_states;

            room_id         = tags.room_id;
        }
    }
}