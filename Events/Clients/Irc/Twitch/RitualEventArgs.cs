// project namespaces
using TwitchNet.Enums.Clients.Irc.Twitch;
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    RitualEventArgs : UserNoticeEventArgs
    {
        /// <summary>
        /// The ritual type.
        /// </summary>
        public RitualType msg_param_ritual_name { get; protected set; }

        public RitualEventArgs(UserNoticeEventArgs args) : base(args)
        {
            if (args.message_irc.tags.IsValid())
            {
                msg_param_ritual_name = TagsUtil.ToRitualType(message_irc.tags, "msg-param-ritual-name");
            }
        }
    }
}
