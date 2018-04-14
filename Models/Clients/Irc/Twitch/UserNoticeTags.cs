// standard namespaces
using System;
using System.Drawing;

// project namespaces
using TwitchNet.Enums;
using TwitchNet.Enums.Clients.Irc.Twitch;
using TwitchNet.Events.Clients.Irc.Twitch;
using TwitchNet.Extensions;
using TwitchNet.Interfaces.Clients.Irc;
using TwitchNet.Utilities;

namespace
TwitchNet.Models.Clients.Irc.Twitch
{
    public class
    UserNoticeTags : ITags
    {
        /// <summary>
        /// Whether or not tags were attached to the message;
        /// </summary>
        public bool             is_valid        { get; protected set; }

        /// <summary>
        /// Whether or not the user is a moderator.
        /// </summary>
        public bool             mod             { get; protected set; }

        /// <summary>
        /// Whether or not the user is subscribed to the channel.
        /// </summary>
        public bool             subscriber      { get; protected set; }

        /// <summary>
        /// Whether or not the user has Twitch turbo.
        /// </summary>
        public bool             turbo           { get; protected set; }

        /// <summary>
        /// <para>The display name of the user.</para>
        /// <para>This is empty if it was never set by the user.</para>
        /// </summary>
        public string           display_name    { get; protected set; }

        /// <summary>
        /// The unique message id.
        /// </summary>
        public string           id              { get; protected set; }

        /// <summary>
        /// The login name of the user.
        /// </summary>
        public string           login           { get; protected set; }

        /// <summary>
        /// The id of the channel the user notice was sent in.
        /// </summary>
        public string           room_id         { get; protected set; }

        /// <summary>
        /// The message printed in chat along with its notice.
        /// </summary>
        public string           system_msg      { get; protected set; }

        /// <summary>
        /// The id of the user who triggered the user notice.
        /// </summary>
        public string           user_id         { get; protected set; }

        /// <summary>
        /// <para>The type of the user notice.</para>
        /// <para>Set to <see cref="UserNoticeType.None"/> if the tag could not be parsed.</para>
        /// </summary>
        public UserNoticeType   msg_id          { get; protected set; }

        /// <summary>
        /// <para>The type of the user.</para>
        /// <para>Set to <see cref="UserType.None"/> if the user has no elevated privileges.</para>
        /// </summary>
        public UserType         user_type       { get; protected set; }

        /// <summary>
        /// <para>The color of the user's display name.</para>
        /// <para>The color is <see cref="Color.Empty"/> if it was never set by the sender.</para>
        /// </summary>
        public Color            color           { get; protected set; }

        /// <summary>
        /// The time the user notice was sent.
        /// </summary>
        public DateTime         tmi_sent_ts     { get; protected set; }

        /// <summary>
        /// <para>The chat badges that the user has, if any.</para>
        /// <para>The array is empty if the user has no chat badges.</para>
        /// </summary>
        public Badge[]          badges          { get; protected set; }

        /// <summary>
        /// <para>The emotes the user used in the message, if any.</para>
        /// <para>The array is empty if the user did not use any emotes.</para>
        /// </summary>
        public Emote[]          emotes          { get; protected set; }

        public UserNoticeTags(IrcMessage message)
        {
            is_valid = message.tags.IsValid();
            if (!is_valid)
            {
                return;
            }

            mod             = TagsUtil.ToBool(message.tags, "mod");
            subscriber      = TagsUtil.ToBool(message.tags, "subscriber");
            turbo           = TagsUtil.ToBool(message.tags, "turbo");

            display_name    = TagsUtil.ToString(message.tags, "display-name");
            id              = TagsUtil.ToString(message.tags, "id");
            login           = TagsUtil.ToString(message.tags, "login");
            room_id         = TagsUtil.ToString(message.tags, "room-id");
            system_msg      = TagsUtil.ToString(message.tags, "system-msg").Replace("\\s", " ");
            user_id         = TagsUtil.ToString(message.tags, "user-id");

            msg_id          = TagsUtil.ToUserNoticeType(message.tags, "msg-id");
            user_type       = TagsUtil.ToUserType(message.tags, "user-type");

            color           = TagsUtil.FromtHtml(message.tags, "color");
            tmi_sent_ts     = TagsUtil.FromUnixEpoch(message.tags, "tmi-sent-ts");

            badges          = TagsUtil.ToBadges(message.tags, "badges");
            emotes          = TagsUtil.ToEmotes(message.tags, "emotes");
        }
    }
}