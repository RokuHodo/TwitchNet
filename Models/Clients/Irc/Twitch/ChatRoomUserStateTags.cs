// standard namespaces
using System.Drawing;

// project namespaces
using TwitchNet.Enums;
using TwitchNet.Extensions;
using TwitchNet.Interfaces.Clients.Irc.Twitch;
using TwitchNet.Utilities;

namespace
TwitchNet.Models.Clients.Irc.Twitch
{
    public class
    ChatRoomUserStateTags : ISharedUserStateTags
    {
        /// <summary>
        /// Whether or not tags were attached to the message;
        /// </summary>
        public bool     is_valid        { get; protected set; }

        /// <summary>
        /// Whether or not the user is a moderator.
        /// </summary>
        public bool     mod             { get; protected set; }

        /// <summary>
        /// <para>The display name of the user.</para>
        /// <para>This is empty if it was never set by the user.</para>
        /// </summary>
        public string   display_name    { get; protected set; }

        /// <summary>
        /// The emote sets that are available for the user to use.
        /// </summary>
        public string[] emote_sets      { get; protected set; }

        /// <summary>
        /// <para>The user's type</para>
        /// <para>Set to <see cref="UserType.None"/> if the user has no elevated privileges.</para>
        /// </summary>
        public UserType user_type       { get; protected set; }

        /// <summary>
        /// <para>The color of the user's display name.</para>
        /// <para>The color is <see cref="Color.Empty"/> if it was never set by the user.</para>
        /// </summary>
        public Color    color           { get; protected set; }

        public ChatRoomUserStateTags(IrcMessage message)
        {
            is_valid = message.tags.IsValid();
            if (!is_valid)
            {
                return;
            }

            mod             = TagsUtil.ToBool(message.tags, "mod");

            display_name    = TagsUtil.ToString(message.tags, "display-name");
            emote_sets      = TagsUtil.ToArray<string>(message.tags, "emote-sets", ',');

            user_type       = TagsUtil.ToUserType(message.tags, "user-type");

            color           = TagsUtil.FromtHtml(message.tags, "color");
        }

        public ChatRoomUserStateTags(UserStateTags tags)
        {
            is_valid = tags.is_valid;
            if (!is_valid)
            {
                return;
            }

            mod             = tags.mod;

            display_name    = tags.display_name;
            emote_sets      = tags.emote_sets;

            user_type       = tags.user_type;

            color           = tags.color;
        }
    }
}