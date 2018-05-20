// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Enums.Debugger;
using TwitchNet.Extensions;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    [ValidateObject]
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

        public AlreadyBannedEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel     = args.channel;
            user_nick   = args.body.TextBefore(' ');
        }
    }
}