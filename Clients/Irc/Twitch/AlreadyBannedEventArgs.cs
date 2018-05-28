// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    AlreadyBannedEventArgs : IrcMessageEventArgs
    {        
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel   { get; protected set; }

        /// <summary>
        /// The user who was attempted to be banned or timed out, but is already banned or timed out.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user_nick { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="AlreadyBannedEventArgs"/> class.
        /// </summary>
        /// <param name="args">The event arguments to parse.</param>
        public AlreadyBannedEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel     = args.channel;
            user_nick   = args.body.TextBefore(' ');
        }
    }
}