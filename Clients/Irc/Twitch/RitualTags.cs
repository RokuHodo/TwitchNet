// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    RitualTags : UserNoticeTags, ITags
    {
        /// <summary>
        /// The ritual type.
        /// </summary>
        [ValidateTag("msg-param-ritual-name")]
        public RitualType msg_param_ritual_name { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="RitualTags"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public RitualTags(IrcMessage message) : base(message)
        {
            if (!is_valid)
            {
                return;
            }

            msg_param_ritual_name = TagsUtil.ToRitualType(message.tags, "msg-param-ritual-name");
        }
    }
}