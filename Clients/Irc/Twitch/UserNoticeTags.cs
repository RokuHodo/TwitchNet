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
    UserNoticeTags
    {
        /// <summary>
        /// Whether or not tags were attached to the message;
        /// </summary>
        public bool             exist        { get; protected set; }

        /// <summary>
        /// Whether or not the user is a moderator.
        /// </summary>
        [IrcTag("mod")]
        public bool             mod             { get; protected set; }

        /// <summary>
        /// Whether or not the user is subscribed to the channel.
        /// </summary>
        [IrcTag("subscriber")]
        public bool             subscriber      { get; protected set; }

        /// <summary>
        /// Whether or not the user has Twitch turbo.
        /// </summary>
        [IrcTag("turbo")]
        public bool             turbo           { get; protected set; }

        /// <summary>
        /// <para>The display name of the user.</para>
        /// <para>This is empty if it was never set by the user.</para>
        /// </summary>
        [IrcTag("display-name")]
        public string           display_name    { get; protected set; }

        /// <summary>
        /// The unique message id.
        /// </summary>
        [IrcTag("id")]
        public string           id              { get; protected set; }

        /// <summary>
        /// The login name of the user.
        /// </summary>
        [IrcTag("login")]
        public string           login           { get; protected set; }

        /// <summary>
        /// The id of the channel the user notice was sent in.
        /// </summary>
        [IrcTag("room-id")]
        public string           room_id         { get; protected set; }

        /// <summary>
        /// The message printed in chat along with its notice.
        /// </summary>
        [IrcTag("system-msg")]
        public string           system_msg      { get; protected set; }

        /// <summary>
        /// The id of the user who triggered the user notice.
        /// </summary>
        [IrcTag("user-id")]
        public string           user_id         { get; protected set; }

        /// <summary>
        /// <para>The type of the user notice.</para>
        /// <para>Set to <see cref="UserNoticeType.Other"/> if the tag could not be parsed.</para>
        /// </summary>
        [IrcTag("msg-id")]
        public UserNoticeType   msg_id          { get; protected set; }

        /// <summary>
        /// <para>The type of the user.</para>
        /// <para>Set to <see cref="UserType.None"/> if the user has no elevated privileges.</para>
        /// </summary>
        [IrcTag("user-type")]
        public UserType         user_type       { get; protected set; }

        /// <summary>
        /// <para>The color of the user's display name.</para>
        /// <para>The color is <see cref="Color.Empty"/> if it was never set by the sender.</para>
        /// </summary>
        [IrcTag("color")]
        public Color            color           { get; protected set; }

        /// <summary>
        /// The time the user notice was sent.
        /// </summary>
        [IrcTag("tmi-sent-ts")]
        public DateTime         tmi_sent_ts     { get; protected set; }

        /// <summary>
        /// <para>The chat badges that the user has, if any.</para>
        /// <para>The array is empty if the user has no chat badges.</para>
        /// </summary>
        [IrcTag("badges")]
        public Badge[]          badges          { get; protected set; }

        /// <summary>
        /// <para>The emotes the user used in the message, if any.</para>
        /// <para>The array is empty if the user did not use any emotes.</para>
        /// </summary>
        [IrcTag("emotes")]
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

            mod             = TwitchIrcUtil.Tags.ToBool(message, "mod");
            subscriber      = TwitchIrcUtil.Tags.ToBool(message, "subscriber");
            turbo           = TwitchIrcUtil.Tags.ToBool(message, "turbo");

            display_name    = TwitchIrcUtil.Tags.ToString(message, "display-name");
            id              = TwitchIrcUtil.Tags.ToString(message, "id");
            login           = TwitchIrcUtil.Tags.ToString(message, "login");
            room_id         = TwitchIrcUtil.Tags.ToString(message, "room-id");
            system_msg      = TwitchIrcUtil.Tags.ToString(message, "system-msg").Replace("\\s", " ");
            user_id         = TwitchIrcUtil.Tags.ToString(message, "user-id");

            msg_id          = TwitchIrcUtil.Tags.ToUserNoticeType(message, "msg-id");
            user_type       = TwitchIrcUtil.Tags.ToUserType(message, "user-type");

            color           = TwitchIrcUtil.Tags.FromtHtml(message, "color");
            tmi_sent_ts     = TwitchIrcUtil.Tags.FromUnixEpochMilliseconds(message, "tmi-sent-ts");

            badges          = TwitchIrcUtil.Tags.ToBadges(message, "badges");
            emotes          = TwitchIrcUtil.Tags.ToEmotes(message, "emotes");
        }
    }
}