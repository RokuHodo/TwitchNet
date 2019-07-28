// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    RoomStateTags
    {
        /// <summary>
        /// Whether or not tags were attached to the message;
        /// </summary>
        public bool                 exist               { get; protected set; }

        /// <summary>
        /// Whether or not the room is in emote only mode.
        /// </summary>
        [IrcTag("emote-only")]
        public bool                 emote_only          { get; protected set; }

        /// <summary>
        /// Whether or r9k mode is enabled.
        /// When enabled, messages 9 characters or longer must be unique from other messages.
        /// </summary>
        [IrcTag("r9k")]
        public bool                 r9k                 { get; protected set; }

        /// <summary>
        /// Whether or not rituals are enabled.
        /// </summary>
        [IrcTag("rituals")]
        public bool                 rituals             { get; protected set; }

        /// <summary>
        /// Whether or not the room is in sub only mode.
        /// When enabled, only subs can sent chat messages.
        /// </summary>
        [IrcTag("subs-only")]
        public bool                 subs_only           { get; protected set; }

        /// <summary>
        /// <para>How long, in minutes, the user only needs to be following.</para>
        /// <para>
        /// Set to 0 when no duration is required and a user only needs to be following.
        /// Set to -1 when follower only mode is disabled.
        /// </para>
        /// </summary>
        [IrcTag("followers-only")]
        public int                  followers_only      { get; protected set; }

        /// <summary>
        /// <para>How frequently, in seconds, non-elevated users can send messages.</para>
        /// <para>Set to 0 if slow mode is disabled.</para>
        /// </summary>
        [IrcTag("slow")]
        public uint                 slow                { get; protected set; }

        /// <summary>
        /// The id of the room whose state has changed and/or the client has joined.
        /// </summary>
        [IrcTag("room-id")]
        public string               room_id             { get; protected set; }

        /// <summary>
        /// <para>
        /// The language the chat is restricted to.
        /// This is the language selected in the Twitch dashboard or in the video information editor, not the language selected at the home page.
        /// </para>
        /// <para>Set to <see cref="BroadcasterLanguage.None"/> if the room is not language restricted.</para>
        /// </summary>
        [IrcTag("broadcaster-lang")]
        public BroadcasterLanguage  broadcaster_lang    { get; protected set; }

        /// <summary>
        /// <para>
        /// Bitfield enum. Contains the room state(s) that changed.
        /// Check this to see which room state fields are valid.
        /// </para>
        /// <para>Set to <see cref="RoomStateType.None"/> if no room states have changed. This should never be the case.</para>
        /// </summary>
        public RoomStateType        changed_states      { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="RoomStateTags"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public RoomStateTags(in IrcMessage message)
        {
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

            if (TwitchIrcUtil.Tags.IsTagValid(message, "rituals"))
            {
                rituals = TwitchIrcUtil.Tags.ToBool(message, "rituals");
                changed_states |= RoomStateType.Rituals;
            }

            if (TwitchIrcUtil.Tags.IsTagValid(message, "subs-only"))
            {
                subs_only = TwitchIrcUtil.Tags.ToBool(message, "subs-only");
                changed_states |= RoomStateType.SubsOnly;
            }

            if (TwitchIrcUtil.Tags.IsTagValid(message, "followers-only"))
            {
                followers_only = TwitchIrcUtil.Tags.ToInt32(message, "followers-only");
                changed_states |= RoomStateType.FollowersOnly;
            }

            if (TwitchIrcUtil.Tags.IsTagValid(message, "slow"))
            {
                slow = TwitchIrcUtil.Tags.ToUInt32(message, "slow");
                changed_states |= RoomStateType.Slow;
            }

            if (TwitchIrcUtil.Tags.IsTagValid(message, "broadcaster-lang"))
            {
                broadcaster_lang = TwitchIrcUtil.Tags.ToBroadcasterLanguage(message, "broadcaster-lang");
                changed_states |= RoomStateType.BroadcasterLang;
            }

            room_id = TwitchIrcUtil.Tags.ToString(message, "room-id");
        }
    }
}