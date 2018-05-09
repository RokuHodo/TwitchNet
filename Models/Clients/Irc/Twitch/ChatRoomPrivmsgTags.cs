// standard namespaces
using System;
using System.Drawing;

// project namespaces
using TwitchNet.Enums;
using TwitchNet.Extensions;
using TwitchNet.Events.Clients.Irc;
using TwitchNet.Interfaces.Clients.Irc.Twitch;
using TwitchNet.Utilities;

namespace
TwitchNet.Models.Clients.Irc.Twitch
{
    public class
    ChatRoomPrivmsgTags : ISharedPrivmsgTags
    {
        /// <summary>
        /// Whether or not tags are atached to the message.
        /// </summary>
        public bool     is_valid        { get; protected set; }

        /// <summary>
        /// Whether or not the sender is a moderator.
        /// </summary>
        [Tag("mod")]
        public bool     mod             { get; protected set; }

        /// <summary>
        /// Whether or not the sender is subscribed to the channel.
        /// </summary>
        [Tag("subscriber")]
        public bool     subscriber      { get; protected set; }

        /// <summary>
        /// Whether or not the sender has Twitch turbo.
        /// </summary>
        [Tag("turbo")]
        public bool     turbo           { get; protected set; }

        /// <summary>
        /// Whether or not the body of the message only contains emotes.
        /// </summary>
        [Tag("emote-only")]
        public bool     emote_only      { get; protected set; }

        /// <summary>
        /// The unique message id.
        /// </summary>
        [Tag("id")]
        public string   id              { get; protected set; }

        /// <summary>
        /// <para>The display name of the sender.</para>
        /// <para>This is empty if it was never set by the sender.</para>
        /// </summary>
        [Tag("display-name")]
        public string   display_name    { get; protected set; }

        /// <summary>
        /// The user id of the sender.
        /// </summary>
        [Tag("user-id")]
        public string   user_id         { get; protected set; }

        /// <summary>
        /// The user id of the channel the message was sent in.
        /// </summary>
        [Tag("room-id")]
        public string   room_id         { get; protected set; }

        /// <summary>
        /// <para>The sender's user type</para>
        /// <para>Set to <see cref="UserType.None"/> if the sender has no elevated privileges.</para>
        /// </summary>
        [Tag("user-type")]
        public UserType user_type       { get; protected set; }

        /// <summary>
        /// <para>The color of the sender's display name.</para>
        /// <para>The color is <see cref="Color.Empty"/> if it was never set by the sender.</para>
        /// </summary>
        [Tag("color")]
        public Color    color           { get; protected set; }

        /// <summary>
        /// The time the message was sent.
        /// </summary>
        [Tag("tmi-sent-ts")]
        public DateTime tmi_sent_ts     { get; protected set; }

        /// <summary>
        /// <para>The chat badges that the sender has, if any.</para>
        /// <para>The array is empty if the sender has no chat badges.</para>
        /// </summary>
        [Tag("badges")]
        public Badge[]  badges          { get; protected set; }

        /// <summary>
        /// <para>The emotes the sender used in the message, if any.</para>
        /// <para>The array is empty if the sender did not use any emotes.</para>
        /// </summary>
        [Tag("emotes")]
        public Emote[]  emotes          { get; protected set; }

        public ChatRoomPrivmsgTags(PrivmsgEventArgs args)
        {
            is_valid = args.tags.IsValid();
            if (!is_valid)
            {
                return;
            }

            mod             = TagsUtil.ToBool(args.tags, "mod");
            subscriber      = TagsUtil.ToBool(args.tags, "subscriber");
            turbo           = TagsUtil.ToBool(args.tags, "turbo");
            emote_only      = TagsUtil.ToBool(args.tags, "emote-only");

            id              = TagsUtil.ToString(args.tags, "id");
            display_name    = TagsUtil.ToString(args.tags, "display-name");
            user_id         = TagsUtil.ToString(args.tags, "user-id");
            room_id         = TagsUtil.ToString(args.tags, "room-id");

            user_type       = TagsUtil.ToUserType(args.tags, "user-type");

            color           = TagsUtil.FromtHtml(args.tags, "color");
            tmi_sent_ts     = TagsUtil.FromUnixEpoch(args.tags, "tmi-sent-ts");

            badges          = TagsUtil.ToBadges(args.tags, "badges");
            emotes          = TagsUtil.ToEmotes(args.tags, "emotes");
        }

        public ChatRoomPrivmsgTags(StreamChatPrivmsgTags tags)
        {
            if (!tags.is_valid)
            {
                return;
            }

            mod             = tags.mod;
            subscriber      = tags.subscriber;
            turbo           = tags.turbo;
            emote_only      = tags.emote_only;

            id              = tags.id;
            display_name    = tags.display_name;
            user_id         = tags.user_id;
            room_id         = tags.room_id;

            user_type       = tags.user_type;

            color           = tags.color;
            tmi_sent_ts     = tags.tmi_sent_ts;

            badges          = tags.badges;
            emotes          = tags.emotes;
        }
    }
}