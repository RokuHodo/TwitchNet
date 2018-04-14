// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Interfaces.Clients.Irc.Twitch;
using TwitchNet.Utilities;

namespace
TwitchNet.Models.Clients.Irc.Twitch
{
    public class
    UserStateTags : ChatRoomUserStateTags, ISharedUserStateTags
    {
        /// <summary>
        /// Whether or not the user is subscribed to the channel.
        /// </summary>
        public bool     subscriber      { get; protected set; }

        /// <summary>
        /// <para>The chat badges that the user has, if any.</para>
        /// <para>The array is empty if the user has no chat badges.</para>
        /// </summary>
        public Badge[]  badges          { get; protected set; }

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

/*
color=#FF0000;
display-name=RokuHodo_;
emote-sets=0,33,140,168,1570,1630,2963,4391,16595,19194,32154,33563;
mod=1;
user-type=mod
:tmi.twitch.tv USERSTATE #chatrooms:45947671:3361582d-4944-4110-9ea3-506ade6867ff
*/