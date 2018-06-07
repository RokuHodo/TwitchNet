// standard namespaces
using System;
using System.Drawing;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    UserNoticeTags : ITags
    {
        /// <summary>
        /// Whether or not tags were attached to the message;
        /// </summary>
        public bool             exist        { get; protected set; }

        /// <summary>
        /// Whether or not the user is a moderator.
        /// </summary>
        [ValidateTag("mod")]
        public bool             mod             { get; protected set; }

        /// <summary>
        /// Whether or not the user is subscribed to the channel.
        /// </summary>
        [ValidateTag("subscriber")]
        public bool             subscriber      { get; protected set; }

        /// <summary>
        /// Whether or not the user has Twitch turbo.
        /// </summary>
        [ValidateTag("turbo")]
        public bool             turbo           { get; protected set; }

        /// <summary>
        /// <para>The display name of the user.</para>
        /// <para>This is empty if it was never set by the user.</para>
        /// </summary>
        [ValidateTag("display-name")]
        public string           display_name    { get; protected set; }

        /// <summary>
        /// The unique message id.
        /// </summary>
        [ValidateTag("id")]
        public string           id              { get; protected set; }

        /// <summary>
        /// The login name of the user.
        /// </summary>
        [ValidateTag("login")]
        public string           login           { get; protected set; }

        /// <summary>
        /// The id of the channel the user notice was sent in.
        /// </summary>
        [ValidateTag("room-id")]
        public string           room_id         { get; protected set; }

        /// <summary>
        /// The message printed in chat along with its notice.
        /// </summary>
        [ValidateTag("system-msg")]
        public string           system_msg      { get; protected set; }

        /// <summary>
        /// The id of the user who triggered the user notice.
        /// </summary>
        [ValidateTag("user-id")]
        public string           user_id         { get; protected set; }

        /// <summary>
        /// <para>The type of the user notice.</para>
        /// <para>Set to <see cref="UserNoticeType.Other"/> if the tag could not be parsed.</para>
        /// </summary>
        [ValidateTag("msg-id")]
        public UserNoticeType   msg_id          { get; protected set; }

        /// <summary>
        /// <para>The type of the user.</para>
        /// <para>Set to <see cref="UserType.None"/> if the user has no elevated privileges.</para>
        /// </summary>
        [ValidateTag("user-type")]
        public UserType         user_type       { get; protected set; }

        /// <summary>
        /// <para>The color of the user's display name.</para>
        /// <para>The color is <see cref="Color.Empty"/> if it was never set by the sender.</para>
        /// </summary>
        [ValidateTag("color")]
        public Color            color           { get; protected set; }

        /// <summary>
        /// The time the user notice was sent.
        /// </summary>
        [ValidateTag("tmi-sent-ts")]
        public DateTime         tmi_sent_ts     { get; protected set; }

        /// <summary>
        /// <para>The chat badges that the user has, if any.</para>
        /// <para>The array is empty if the user has no chat badges.</para>
        /// </summary>
        [ValidateTag("badges")]
        public Badge[]          badges          { get; protected set; }

        /// <summary>
        /// <para>The emotes the user used in the message, if any.</para>
        /// <para>The array is empty if the user did not use any emotes.</para>
        /// </summary>
        [ValidateTag("emotes")]
        public Emote[]          emotes          { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="UserNoticeTags"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public UserNoticeTags(in IrcMessage message)
        {
            exist = message.tags.IsValid();
            if (!exist)
            {
                return;
            }

            mod             = TagsUtil.ToBool(message, "mod");
            subscriber      = TagsUtil.ToBool(message, "subscriber");
            turbo           = TagsUtil.ToBool(message, "turbo");

            display_name    = TagsUtil.ToString(message, "display-name");
            id              = TagsUtil.ToString(message, "id");
            login           = TagsUtil.ToString(message, "login");
            room_id         = TagsUtil.ToString(message, "room-id");
            system_msg      = TagsUtil.ToString(message, "system-msg").Replace("\\s", " ");
            user_id         = TagsUtil.ToString(message, "user-id");

            msg_id          = TagsUtil.ToUserNoticeType(message, "msg-id");
            user_type       = TagsUtil.ToUserType(message, "user-type");

            color           = TagsUtil.FromtHtml(message, "color");
            tmi_sent_ts     = TagsUtil.FromUnixEpoch(message, "tmi-sent-ts");

            badges          = TagsUtil.ToBadges(message, "badges");
            emotes          = TagsUtil.ToEmotes(message, "emotes");
        }
    }
}