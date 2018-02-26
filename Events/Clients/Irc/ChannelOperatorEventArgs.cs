namespace
TwitchNet.Events.Clients.Irc
{
    public class
    ChannelOperatorEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// Whether or not the user is an operator in the IRC channel.
        /// </summary>
        public bool     is_operator { get; protected set; }

        /// <summary>
        /// The user that gained or lost operator status.
        /// </summary>
        public string   user        { get; protected set; }

        /// <summary>
        /// The IRC channel where the user's operator status changed.
        /// </summary>
        public string   channel     { get; protected set; }

        public ChannelOperatorEventArgs(ChannelModeEventArgs args) : base(args.message_irc)
        {
            is_operator = args.modifier == '+' ? true : false;
            user        = args.arguments;
            channel     = args.channel;
        }
    }
}
