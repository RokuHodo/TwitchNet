// standard namespaces
using System;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Interfaces.Clients.Irc;
using TwitchNet.Utilities;

namespace
TwitchNet.Models.Clients.Irc.Twitch
{
    public class
    ClearChatTags : ITags
    {
        /// <summary>
        /// Whether or not tags were attached to the message;
        /// </summary>
        public bool     is_valid        { get; protected set; }

        // TODO: Change type to TimeSpan?
        /// <summary>
        /// <para>The length of the ban, in seconds.</para>
        /// <para>Set to zero if the ban is permanent.</para>
        /// </summary>
        [ValidateTag("ban-duration")]
        public uint     ban_duration    { get; protected set; }

        /// <summary>
        /// <para>The moderator’s reason for the timeout or ban.</para>
        /// <para>Empty if the moderator gave no reason.</para>
        /// </summary>
        [ValidateTag("ban-reason")]
        public string   ban_reason      { get; protected set; }

        /// <summary>
        /// The id of the room where the user got timed out or banned.
        /// </summary>
        [ValidateTag("room-id")]
        public string   room_id         { get; protected set; }

        /// <summary>
        /// The id of the user who got timed out or banned.
        /// </summary>
        [ValidateTag("target-user-id")]
        public string   target_user_id  { get; protected set; }

        /// <summary>
        /// The time message was sent.
        /// </summary>
        [ValidateTag("tmi-sent-ts")]
        public DateTime tmi_sent_ts     { get; protected set; }

        public ClearChatTags(IrcMessage message)
        {
            is_valid = message.tags.IsValid();
            if (!is_valid)
            {
                return;
            }

            ban_duration    = TagsUtil.ToUInt32(message.tags, "ban-duration");
            ban_reason      = TagsUtil.ToString(message.tags, "ban-reason").Replace("\\s", " ");

            room_id         = TagsUtil.ToString(message.tags, "room-id");
            target_user_id  = TagsUtil.ToString(message.tags, "target-user-id");

            tmi_sent_ts     = TagsUtil.FromUnixEpoch(message.tags, "tmi-sent-ts");
        }
    }
}