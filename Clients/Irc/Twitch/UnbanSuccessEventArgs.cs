// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    UnbanSuccessEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel   { get; protected set; }

        /// <summary>
        /// The user who was unbanned.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user_nick { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="UnbanSuccessEventArgs"/> class.
        /// </summary>
        /// <param name="args">The event arguments to parse.</param>
        public UnbanSuccessEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            user_nick = args.body.TextBefore(' ');
        }
    }
}