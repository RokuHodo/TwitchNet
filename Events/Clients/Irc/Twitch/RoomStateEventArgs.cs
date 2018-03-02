// project namespaces
using TwitchNet.Enums.Api.Videos;
using TwitchNet.Enums.Clients.Irc.Twitch;
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;
using TwitchNet.Utilities;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    RoomStateEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// Whether or not the room is in emote only mode.
        /// </summary>
        public bool                 emote_only          { get; protected set; }

        /// <summary>
        /// Whether or not an ancient emote mode is enabled.
        /// Legend has it, someone actually knows what this is.
        /// </summary>
        public bool                 mercury             { get; protected set; }

        /// <summary>
        /// Whether or r9k mode is enabled.
        /// When enabled, messages 9 characters or longer must be unique from other messages.
        /// </summary>
        public bool                 r9k                 { get; protected set; }

        /// <summary>
        /// Whether or not rituals are enabled.
        /// </summary>
        public bool                 rituals             { get; protected set; }

        /// <summary>
        /// Whether or not the room is in sub only mode.
        /// When enabled, only subs can sent chat messages.
        /// </summary>
        public bool                 subs_only           { get; protected set; }

        /// <summary>
        /// <para>How long, in minutes, the user only needs to be following.</para>
        /// <para>
        /// Set to 0 when no duration is required and a user only needs to be following.
        /// Set to -1 when follower only mode is disabled.
        /// </para>
        /// </summary>
        public int                  followers_only      { get; protected set; }

        /// <summary>
        /// <para>How frequently, in seconds, non-elevated users can send messages.</para>
        /// <para>Set to 0 if slow mode is disabled.</para>
        /// </summary>
        public uint                 slow                { get; protected set; }

        /// <summary>
        /// The id of the room whose state has changed and/or the client has joined.
        /// </summary>
        public string               room_id             { get; protected set; }        

        /// <summary>
        /// <para>
        /// The language the chat is restricted to.
        /// This is the language selected in the Twitch dashboard or in the video information editor, not the language selected at the home page.
        /// </para>
        /// <para>Set to <see cref="BroadcasterLanguage.None"/> if the room is not language restricted.</para>
        /// </summary>
        public BroadcasterLanguage  broadcaster_lang    { get; protected set; }

        /// <summary>
        /// <para>
        /// Bitfield enum. Contains the room state(s) that changed.
        /// Check this to see which room state fields are valid.
        /// </para>
        /// <para>Set to <see cref="RoomState.None"/> if no room states have changed. This should never be the case.</para>
        /// </summary>
        public RoomState            changed_states      { get; protected set; }

        /// <summary>
        /// The channel whose state has changed and/or the client has joined.
        /// Always valid.
        /// </summary>
        public string               channel             { get; protected set; }

        public RoomStateEventArgs(IrcMessage message) : base(message)
        {
            if (message.parameters.IsValid())
            {
                channel = message.parameters[0];
            }

            if (message.tags.IsValid())
            {
                if (TagsUtil.IsTagValid(message.tags, "emote-only"))
                {
                    emote_only = TagsUtil.ToBool(message.tags, "emote-only");
                    changed_states |= RoomState.EmoteOnly;
                }

                if (TagsUtil.IsTagValid(message.tags, "mercury"))
                {
                    mercury = TagsUtil.ToBool(message.tags, "mercury");
                    changed_states |= RoomState.Mercury;
                }

                if (TagsUtil.IsTagValid(message.tags, "r9k"))
                {
                    r9k = TagsUtil.ToBool(message.tags, "r9k");
                    changed_states |= RoomState.R9K;
                }

                if (TagsUtil.IsTagValid(message.tags, "rituals"))
                {
                    rituals = TagsUtil.ToBool(message.tags, "rituals");
                    changed_states |= RoomState.Rituals;
                }

                if (TagsUtil.IsTagValid(message.tags, "subs-only"))
                {
                    subs_only = TagsUtil.ToBool(message.tags, "subs-only");
                    changed_states |= RoomState.SubsOnly;
                }

                if (TagsUtil.IsTagValid(message.tags, "followers-only"))
                {
                    followers_only = TagsUtil.ToInt32(message.tags, "followers-only");
                    changed_states |= RoomState.FollowersOnly;
                }

                if (TagsUtil.IsTagValid(message.tags, "slow"))
                {
                    slow = TagsUtil.ToUInt32(message.tags, "slow");
                    changed_states |= RoomState.Slow;
                }

                if (TagsUtil.IsTagValid(message.tags, "broadcaster-lang"))
                {
                    broadcaster_lang = TagsUtil.FromBroadcasterLanguage(message.tags, "broadcaster-lang");
                    changed_states |= RoomState.BroadcasterLang;
                }

                room_id = TagsUtil.ToString(message.tags, "room-id");
            }
        }
    }
}
