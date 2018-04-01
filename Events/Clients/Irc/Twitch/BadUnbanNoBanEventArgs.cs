// project namespaces
using TwitchNet.Extensions;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    BadUnbanNoBanEventArgs : IrcMessageEventArgs
    {        
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        public string channel   { get; protected set; }

        /// <summary>
        /// The user who was attempted to be unbanned or untimed out, but is not banned or timed out.
        /// </summary>
        public string user_nick { get; protected set; }

        public BadUnbanNoBanEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            user_nick = args.body.TextBefore(' ');
        }
    }
}