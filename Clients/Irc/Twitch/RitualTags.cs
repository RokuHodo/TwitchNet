// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    RitualTags : UserNoticeTags
    {
        /// <summary>
        /// The ritual type.
        /// </summary>
        [IrcTag("msg-param-ritual-name")]
        public RitualType msg_param_ritual_name { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="RitualTags"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public RitualTags(in IrcMessage message) : base(message)
        {
            msg_param_ritual_name = TwitchIrcUtil.Tags.ToRitualType(message, "msg-param-ritual-name");
        }
    }
}