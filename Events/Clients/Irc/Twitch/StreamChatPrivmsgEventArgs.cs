// project namespaces
using TwitchNet.Models.Clients.Irc.Twitch;
using TwitchNet.Utilities;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    StreamChatPrivmsgEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The user who sent the message.
        /// </summary>
        public string                   sender      { get; protected set; }

        /// <summary>
        /// The channel the message was sent in.
        /// </summary>
        public string                   channel     { get; protected set; }        

        /// <summary>
        /// The body of the message.
        /// </summary>
        public string                   body        { get; protected set; }

        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// <para>Check the <code>is_valid</code> property to determine if tags were attached to the message.</para>
        /// </summary>
        public StreamChatPrivmsgTags    tags        { get; protected set; }

        public StreamChatPrivmsgEventArgs(PrivmsgEventArgs args) : base(args.irc_message)
        {
            sender  = args.nick;
            channel = args.channel;

            body    = args.body;

            tags    = new StreamChatPrivmsgTags(args);
            TagsUtil.ValidateTags(tags, args.irc_message.tags);
        }
    }
}
