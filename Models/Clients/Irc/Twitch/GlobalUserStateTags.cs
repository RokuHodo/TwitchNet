// standard namespaces
using System.Drawing;

// project namespaces
using TwitchNet.Enums;
using TwitchNet.Extensions;
using TwitchNet.Interfaces.Clients.Irc;
using TwitchNet.Utilities;

namespace
TwitchNet.Models.Clients.Irc.Twitch
{
    public class
    GlobalUserStateTags : ITags
    {
        /// <summary>
        /// Whether or not tags were attached to the message;
        /// </summary>
        public bool     is_valid        { get; protected set; }

        /// <summary>
        /// The id of the user.
        /// </summary>
        [Tag("user-id")]
        public string   user_id         { get; protected set; }

        /// <summary>
        /// <para>The display name of the user.</para>
        /// <para>This is empty if it was never set by the user.</para>
        /// </summary>
        [Tag("display-name")]
        public string   display_name    { get; protected set; }

        /// <summary>
        /// The emote sets that are available for the user to use.
        /// </summary>
        [Tag("emote-sets")]
        public string[] emote_sets      { get; protected set; }

        /// <summary>
        /// <para>The user's type</para>
        /// <para>Set to <see cref="UserType.None"/> if the user has no elevated privileges.</para>
        /// </summary>
        [Tag("user-type")]
        public UserType user_type       { get; protected set; }

        /// <summary>
        /// <para>The color of the user's display name.</para>
        /// <para>The color is <see cref="Color.Empty"/> if it was never set by the user.</para>
        /// </summary>
        [Tag("color")]
        public Color    color           { get; protected set; }

        /// <summary>
        /// <para>The chat badges that the user has, if any.</para>
        /// <para>The array is empty if the user has no chat badges.</para>
        /// </summary>
        [Tag("badges")]
        public Badge[] badges           { get; protected set; }

        public GlobalUserStateTags(IrcMessage message)
        {
            is_valid = message.tags.IsValid();
            if (!is_valid)
            {
                return;
            }

            user_id         = TagsUtil.ToString(message.tags, "user-id");
            display_name    = TagsUtil.ToString(message.tags, "display-name");
            emote_sets      = TagsUtil.ToStringArray(message.tags, "emote-sets", ',');

            user_type       = TagsUtil.ToUserType(message.tags, "user-type");

            color           = TagsUtil.FromtHtml(message.tags, "color");

            badges          = TagsUtil.ToBadges(message.tags, "badges");
        }
    }
}