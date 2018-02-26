// standard namespaces
using System;
using System.Drawing;

// project namespaces
using TwitchNet.Enums;
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc.Twitch;
using TwitchNet.Utilities;

namespace
TwitchNet.Models.Clients.Irc
{
    public class
    TwitchPrivmsg
    {
        #region Properties

        /// <summary>
        /// Whether or not the sender is a mod.
        /// </summary>
        public bool     mod             { get; protected set; }

        /// <summary>
        /// Whether or not the sender is subscribed to the channel.
        /// </summary>
        public bool     subscriber      { get; protected set; }

        /// <summary>
        /// Whether or not the sender has Twitch turbo.
        /// </summary>
        public bool     turbo           { get; protected set; }

        /// <summary>
        /// Whether or not the body of the message only contains emotes.
        /// </summary>
        public bool     emote_only      { get; protected set; }

        /// <summary>
        /// <para>The amount of bits the sender cheered, if any.</para>
        /// <para>Set to 0 if the sender did not cheer.</para>
        /// </summary>
        public uint     bits            { get; protected set; }

        /// <summary>
        /// The message id.
        /// </summary>
        public string   id              { get; protected set; }

        /// <summary>
        /// <para>The display name of the sender.</para>
        /// <para>This is empty if it was never set by the sender.</para>
        /// </summary>
        public string   display_name    { get; protected set; }

        /// <summary>
        /// The user id of the sender.
        /// </summary>
        public string   user_id         { get; protected set; }

        /// <summary>
        /// The user id of the channel the message was sent in.
        /// </summary>
        public string   room_id         { get; protected set; }

        /// <summary>
        /// <para>The color of the sender's display name.</para>
        /// <para>The color is <see cref="Color.Empty"/> if it was never set by the sender.</para>
        /// </summary>
        public Color    color           { get; protected set; }

        /// <summary>
        /// The time the message was sent.
        /// </summary>
        public DateTime tmi_sent_ts    { get; protected set; }

        /// <summary>
        /// <para>The chat badges that the sender has, if any.</para>
        /// <para>The array is empty if the sender has no chat badges.</para>
        /// </summary>
        public Badge[]  badges          { get; protected set; }

        /// <summary>
        /// <para>The emotes the sender used in the message, if any.</para>
        /// <para>The array is empty if the sender did not use any emotes.</para>
        /// </summary>
        public Emote[]  emotes          { get; protected set; }

        /// <summary>
        /// <para>The user's type</para>
        /// <para>Set to <see cref="UserType.None"/> if the user has no elevated user type.</para>
        /// </summary>
        public UserType user_type       { get; protected set; }

        /// <summary>
        /// The Twitch user who sent the message.
        /// </summary>
        public string sender_name { get; protected set; }

        /// <summary>
        /// The Twitch channel the message was sent in.
        /// </summary>
        public string channel_name { get; protected set; }

        /// <summary>
        /// The body of the message.
        /// </summary>
        public string body { get; protected set; }

        #endregion

        #region Constructors

        public TwitchPrivmsg(Privmsg message)
        {
            sender_name = message.nick;
            channel_name = message.channel.TextAfter('#');

            body = message.body;

            if (message.tags.IsValid())
            {
                mod             = TagsUtil.ToBool(message.tags, "mod");
                subscriber      = TagsUtil.ToBool(message.tags, "subscriber");
                turbo           = TagsUtil.ToBool(message.tags, "turbo");
                emote_only      = TagsUtil.ToBool(message.tags, "emote-only");

                bits            = TagsUtil.ToUInt32(message.tags, "bits");

                id              = TagsUtil.ToString(message.tags, "id");
                display_name    = TagsUtil.ToString(message.tags, "display-name");
                user_id         = TagsUtil.ToString(message.tags, "user-id");
                room_id         = TagsUtil.ToString(message.tags, "room-id");

                color           = TagsUtil.FromtHtml(message.tags, "color");

                tmi_sent_ts     = TagsUtil.FromUnixEpoch(message.tags, "tmi-sent-ts");

                badges          = TagsUtil.ToBadges(message.tags, "badges");

                emotes          = TagsUtil.ToEmotes(message.tags, "emotes");

                user_type       = TagsUtil.ToUserType(message.tags, "user-type");
            }
        }

        #endregion
    }
}