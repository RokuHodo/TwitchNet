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
        [IrcTag("subscriber")]
        public bool     subscriber  { get; protected set; }

        /// <summary>
        /// <para>The chat badges that the user has, if any.</para>
        /// <para>The array is empty if the user has no chat badges.</para>
        /// </summary>
        [IrcTag("badges")]
        public Badge[]  badges      { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="UserStateTags"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public UserStateTags(in IrcMessage message) : base(message)
        {
            exist = message.tags.IsValid();
            if (!exist)
            {
                return;
            }

            subscriber  = TwitchIrcUtil.Tags.ToBool(message, "subscriber");

            badges      = TwitchIrcUtil.Tags.ToBadges(message, "badges");
        }
    }
}