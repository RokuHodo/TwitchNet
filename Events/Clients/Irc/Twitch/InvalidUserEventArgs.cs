// project namespaces
using TwitchNet.Extensions;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    InvalidUserEventArgs : IrcMessageEventArgs
    {        
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        public string channel   { get; protected set; }

        /// <summary>
        /// The invalid user nick
        /// </summary>
        public string user_nick { get; protected set; }

        public InvalidUserEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            user_nick = args.body.TextAfter(':').TrimStart(' ');
        }
    }
}