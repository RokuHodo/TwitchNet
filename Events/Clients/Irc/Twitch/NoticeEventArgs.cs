// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;
using TwitchNet.Models.Clients.Irc.Twitch;
using TwitchNet.Utilities;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    NoticeEventArgs : IrcMessageEventArgs
    {
        

        /// <summary>
        /// <para>The channel notice was sent in.</para>
        /// </summary>
        public string       channel { get; protected set; }

        /// <summary>
        /// The notice message sent by the server.
        /// </summary>
        public string       body    { get; protected set; }

        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// <para>Check the <code>is_valid</code> property to determine if tags were attached to the message.</para>
        /// </summary>
        public NoticeTags   tags    { get; protected set; }

        public NoticeEventArgs(IrcMessage message) : base(message)
        {
            if (message.parameters.IsValid())
            {
                channel = message.parameters[0];
            }

            body = message.trailing;

            tags = new NoticeTags(message);
            TagsUtil.ValidateTags(tags, irc_message.tags);
        }
    }
}