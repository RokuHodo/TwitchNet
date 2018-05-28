// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    UserStateTags : ChatRoomUserStateTags, ISharedUserStateTags
    {
        /// <summary>
        /// Whether or not the user is subscribed to the channel.
        /// </summary>
        [ValidateTag("subscriber")]
        public bool     subscriber  { get; protected set; }

        /// <summary>
        /// <para>The chat badges that the user has, if any.</para>
        /// <para>The array is empty if the user has no chat badges.</para>
        /// </summary>
        [ValidateTag("badges")]
        public Badge[]  badges      { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="UserStateTags"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public UserStateTags(IrcMessage message) : base(message)
        {
            is_valid = message.tags.IsValid();
            if (!is_valid)
            {
                return;
            }

            subscriber  = TagsUtil.ToBool(message.tags, "subscriber");

            badges      = TagsUtil.ToBadges(message.tags, "badges");
        }
    }
}