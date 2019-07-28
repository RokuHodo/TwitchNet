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
    GlobalUserStateTags
    {
        /// <summary>
        /// The id of the user.
        /// </summary>
        [IrcTag("user-id")]
        public string   user_id         { get; protected set; }

        /// <summary>
        /// <para>The display name of the user.</para>
        /// <para>This is empty if it was never set by the user.</para>
        /// </summary>
        [IrcTag("display-name")]
        public string   display_name    { get; protected set; }

        /// <summary>
        /// The emote sets that are available for the user to use.
        /// </summary>
        [IrcTag("emote-sets")]
        public string[] emote_sets      { get; protected set; }

        /// <summary>
        /// <para>The user's type</para>
        /// <para>Set to <see cref="UserType.None"/> if the user has no elevated privileges.</para>
        /// </summary>
        [IrcTag("user-type")]
        public UserType user_type       { get; protected set; }

        /// <summary>
        /// <para>The color of the user's display name.</para>
        /// <para>The color is <see cref="Color.Empty"/> if it was never set by the user.</para>
        /// </summary>
        [IrcTag("color")]
        public Color    color           { get; protected set; }

        /// <summary>
        /// <para>The chat badges that the user has, if any.</para>
        /// <para>The array is empty if the user has no chat badges.</para>
        /// </summary>
        [IrcTag("badges")]
        public Badge[]  badges          { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="GlobalUserStateTags"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public GlobalUserStateTags(in IrcMessage message)
        {
            user_id         = TwitchIrcUtil.Tags.ToString(message, "user-id");
            display_name    = TwitchIrcUtil.Tags.ToString(message, "display-name");
            emote_sets      = TwitchIrcUtil.Tags.ToStringArray(message, "emote-sets", ',');

            user_type       = TwitchIrcUtil.Tags.ToUserType(message, "user-type");

            color           = TwitchIrcUtil.Tags.FromtHtml(message, "color");

            badges          = TwitchIrcUtil.Tags.ToBadges(message, "badges");
        }
    }
}