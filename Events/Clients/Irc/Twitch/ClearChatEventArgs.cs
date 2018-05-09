// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;
using TwitchNet.Models.Clients.Irc.Twitch;
using TwitchNet.Utilities;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    ClearChatEventArgs : IrcMessageEventArgs
    {      
        /// <summary>
        /// <para>The channel the user was banned in.</para>
        /// </summary>
        public string           channel { get; protected set; }

        /// <summary>
        /// The user who got banned or timed out.
        /// If no user was timed out or banned, the entire chat was cleared.
        /// </summary>
        public string           user    { get; protected set; }

        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// <para>Check the <code>is_valid</code> property to determine if tags were attached to the message.</para>
        /// </summary>
        public ClearChatTags    tags    { get; protected set; }

        public ClearChatEventArgs(IrcMessage message) : base(message)
        {
            if (message.parameters.IsValid())
            {
                channel = message.parameters[0];
            }

            user = message.trailing;

            tags = new ClearChatTags(message);
            TagsUtil.ValidateTags(tags, irc_message.tags);
        }
    }
}
