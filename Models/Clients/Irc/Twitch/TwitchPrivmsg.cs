namespace
TwitchNet.Models.Clients.Irc.Twitch
{
    public class
    TwitchPrivmsg
    {
        /// <summary>
        /// The user who sent the message.
        /// </summary>
        public string           sender  { get; protected set; }

        /// <summary>
        /// The channel the message was sent in.
        /// </summary>
        public string           channel { get; protected set; }

        /// <summary>
        /// The body of the message.
        /// </summary>
        public string           body    { get; protected set; }

        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// <para>Check the <code>is_valid</code> property to determine if tags were attached to the message.</para>
        /// </summary>
        public TwitchPrivmsgTags tags   { get; protected set; }

        public TwitchPrivmsg(Privmsg message)
        {
            sender = message.nick;
            channel = message.channel;

            body = message.body;

            tags = new TwitchPrivmsgTags(message);
        }
    }
}