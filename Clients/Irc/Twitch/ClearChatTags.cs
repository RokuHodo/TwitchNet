// standard namespaces
using System;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    ClearChatTags
    {
        /// <summary>
        /// <para>The length of the ban, in seconds.</para>
        /// <para>Set to <see cref="TimeSpan.Zero"/> if the ban is permanent.</para>
        /// </summary>
        [IrcTag("ban-duration")]
        public TimeSpan ban_duration    { get; protected set; }

        /// <summary>
        /// <para>The moderator’s reason for the timeout or ban.</para>
        /// <para>Empty if the moderator gave no reason.</para>
        /// </summary>
        [IrcTag("ban-reason")]
        public string   ban_reason      { get; protected set; }

        /// <summary>
        /// The id of the room where the user got timed out or banned.
        /// </summary>
        [IrcTag("room-id")]
        public string   room_id         { get; protected set; }

        /// <summary>
        /// The id of the user who got timed out or banned.
        /// </summary>
        [IrcTag("target-user-id")]
        public string   target_user_id  { get; protected set; }

        /// <summary>
        /// The time message was sent.
        /// </summary>
        [IrcTag("tmi-sent-ts")]
        public DateTime tmi_sent_ts     { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="ClearChatTags"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public ClearChatTags(in IrcMessage message)
        {
            ban_duration    = TwitchIrcUtil.Tags.ToTimeSpanFromSeconds(message, "ban-duration");
            ban_reason      = TwitchIrcUtil.Tags.ToString(message, "ban-reason").Replace("\\s", " ");

            room_id         = TwitchIrcUtil.Tags.ToString(message, "room-id");
            target_user_id  = TwitchIrcUtil.Tags.ToString(message, "target-user-id");

            tmi_sent_ts     = TwitchIrcUtil.Tags.FromUnixEpochMilliseconds(message, "tmi-sent-ts");
        }
    }
}