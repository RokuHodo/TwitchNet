// project namespaces
using TwitchNet.Enums.Clients.Irc.Twitch;
using TwitchNet.Events.Clients.Irc.Twitch;
using TwitchNet.Interfaces.Clients.Irc;
using TwitchNet.Utilities;

namespace
TwitchNet.Models.Clients.Irc.Twitch
{
    public class
    RitualTags : UserNoticeTags, ITags
    {
        /// <summary>
        /// The ritual type.
        /// </summary>
        public RitualType msg_param_ritual_name { get; protected set; }

        public RitualTags(UserNoticeEventArgs args) : base(args)
        {
            if (!is_valid)
            {
                return;
            }

            msg_param_ritual_name = TagsUtil.ToRitualType(args.message_irc.tags, "msg-param-ritual-name");
        }
    }
}