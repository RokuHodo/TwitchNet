// standard namespaces
using System.Drawing;

// project namespaces
using TwitchNet.Enums;
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;
using TwitchNet.Models.Clients.Irc.Twitch;
using TwitchNet.Utilities;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    GlobalUserStateEventArgs : IrcMessageEventArgs
    {

        /// <summary>
        /// The user id of the user.
        /// </summary>
        public string user_id { get; protected set; }

        /// <summary>
        /// <para>The display name of the user.</para>
        /// <para>This is empty if it was never set by the user.</para>
        /// </summary>
        public string display_name { get; protected set; }

        /// <summary>
        /// The emote sets that are available for the user to use.
        /// </summary>
        public string[] emote_sets { get; protected set; }

        /// <summary>
        /// <para>The user's type</para>
        /// <para>Set to <see cref="UserType.None"/> if the user has no elevated privileges.</para>
        /// </summary>
        public UserType user_type { get; protected set; }

        /// <summary>
        /// <para>The color of the user's display name.</para>
        /// <para>The color is <see cref="Color.Empty"/> if it was never set by the user.</para>
        /// </summary>
        public Color color { get; protected set; }

        /// <summary>
        /// <para>The chat badges that the user has, if any.</para>
        /// <para>The array is empty if the user has no chat badges.</para>
        /// </summary>
        public Badge[] badges { get; protected set; }

        public GlobalUserStateEventArgs(IrcMessage message) : base(message)
        {
            if (message.tags.IsValid())
            {
                user_id         = TagsUtil.ToString(message.tags, "user-id");
                display_name    = TagsUtil.ToString(message.tags, "display-name");
                emote_sets      = TagsUtil.ToArray<string>(message.tags, "emote-sets", ',');

                user_type       = TagsUtil.ToUserType(message.tags, "user-type");

                color           = TagsUtil.FromtHtml(message.tags, "color");

                badges          = TagsUtil.ToBadges(message.tags, "badges");
            }
        }
    }
}
