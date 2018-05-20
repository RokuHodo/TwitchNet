// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Enums.Debugger;

namespace
TwitchNet.Events.Clients.Irc
{
    public class
    ChannelOperatorEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// Whether or not the user is an operator in the IRC channel.
        /// </summary>
        [ValidateMember(Check.IsNotNull)]
        public bool     is_operator { get; protected set; }

        /// <summary>
        /// The user nick that gained or lost operator status.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string   user        { get; protected set; }

        /// <summary>
        /// The IRC channel where the user's operator status changed.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string   channel     { get; protected set; }

        public ChannelOperatorEventArgs(ChannelModeEventArgs args) : base(args.irc_message)
        {
            is_operator = args.modifier == '+' ? true : false;
            user        = args.arguments;
            channel     = args.channel;
        }
    }
}
