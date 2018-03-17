// standard namespaces
using System.Drawing;

// project namespaces
using TwitchNet.Enums;

namespace TwitchNet.Interfaces.Clients.Irc.Twitch
{
    public interface
    ISharedUserStateTags : ITags
    {
        /// <summary>
        /// Whether or not the user is a moderator.
        /// </summary>
        bool mod { get; }

        /// <summary>
        /// <para>The display name of the user.</para>
        /// <para>This is empty if it was never set by the user.</para>
        /// </summary>
        string display_name { get; }

        /// <summary>
        /// The emote sets that are available for the user to use.
        /// </summary>
        string[] emote_sets { get; }

        /// <summary>
        /// <para>The user's type</para>
        /// <para>Set to <see cref="UserType.None"/> if the user has no elevated privileges.</para>
        /// </summary>
        UserType user_type { get; }

        /// <summary>
        /// <para>The color of the user's display name.</para>
        /// <para>The color is <see cref="Color.Empty"/> if it was never set by the user.</para>
        /// </summary>
        Color color { get; }
    }
}