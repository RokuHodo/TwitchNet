// standard namespaces
using System;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;
using TwitchNet.Utilities;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    ClearChatEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// <para>The length of the ban, in seconds.</para>
        /// <para>Set to zero if the ban is permanent.</para>
        /// </summary>
        public uint     ban_duration    { get; protected set; }

        /// <summary>
        /// <para>The moderator’s reason for the timeout or ban.</para>
        /// <para>Empty if the moderator gave no reason.</para>
        /// </summary>
        public string   ban_reason      { get; protected set; }

        /// <summary>
        /// The id of the room where the user got timed out or banned.
        /// </summary>
        public string   room_id { get; protected set; }

        /// <summary>
        /// The if of the user who got banned or timed out.
        /// </summary>
        public string   target_user_id { get; protected set; }

        /// <summary>
        /// The time message was sent.
        /// </summary>
        public DateTime tmi_sent_ts { get; protected set; }

        /// <summary>
        /// <para>The channel the user was banned in.</para>
        /// <para>This does not include the preceding '#' and only includes the channel login.</para>
        /// </summary>
        public string   channel_name    { get; protected set; }

        /// <summary>
        /// The user who got banned or timed out.
        /// </summary>
        public string   user_name { get; protected set; }

        public ClearChatEventArgs(IrcMessage message) : base(message)
        {
            if (message.parameters.IsValid())
            {
                channel_name = message.parameters[0].TextAfter('#');
            }

            user_name = message.trailing;

            if (message.tags.IsValid())
            {
                ban_duration    = TagsUtil.ToUInt32(message.tags, "ban-duration");
                ban_reason      = TagsUtil.ToString(message.tags, "ban-reason").Replace("\\s", " ");

                room_id         = TagsUtil.ToString(message.tags, "room-id");
                target_user_id  = TagsUtil.ToString(message.tags, "target-user-id");

                tmi_sent_ts     = TagsUtil.FromUnixEpoch(message.tags, "tmi-sent-ts");
            }
        }
    }
}
