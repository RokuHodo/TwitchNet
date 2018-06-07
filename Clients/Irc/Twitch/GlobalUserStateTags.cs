// standard namespaces
using System.Drawing;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    GlobalUserStateTags : ITags
    {
        /// <summary>
        /// Whether or not tags were attached to the message;
        /// </summary>
        public bool     exist           { get; protected set; }

        /// <summary>
        /// The id of the user.
        /// </summary>
        [ValidateTag("user-id")]
        public string   user_id         { get; protected set; }

        /// <summary>
        /// <para>The display name of the user.</para>
        /// <para>This is empty if it was never set by the user.</para>
        /// </summary>
        [ValidateTag("display-name")]
        public string   display_name    { get; protected set; }

        /// <summary>
        /// The emote sets that are available for the user to use.
        /// </summary>
        [ValidateTag("emote-sets")]
        public string[] emote_sets      { get; protected set; }

        /// <summary>
        /// <para>The user's type</para>
        /// <para>Set to <see cref="UserType.None"/> if the user has no elevated privileges.</para>
        /// </summary>
        [ValidateTag("user-type")]
        public UserType user_type       { get; protected set; }

        /// <summary>
        /// <para>The color of the user's display name.</para>
        /// <para>The color is <see cref="Color.Empty"/> if it was never set by the user.</para>
        /// </summary>
        [ValidateTag("color")]
        public Color    color           { get; protected set; }

        /// <summary>
        /// <para>The chat badges that the user has, if any.</para>
        /// <para>The array is empty if the user has no chat badges.</para>
        /// </summary>
        [ValidateTag("badges")]
        public Badge[]  badges          { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="GlobalUserStateTags"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public GlobalUserStateTags(in IrcMessage message)
        {
            exist = message.tags.IsValid();
            if (!exist)
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