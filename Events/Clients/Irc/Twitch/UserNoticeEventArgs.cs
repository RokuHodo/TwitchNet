// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;
using TwitchNet.Models.Clients.Irc.Twitch;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    UserNoticeEventArgs : IrcMessageEventArgs
    {        
        /// <summary>
        /// <para>The messsage sent by the user.</para>
        /// <para>This is empty if the user did not send a message.</para>
        /// </summary>
        public string           body    { get; protected set; }

        /// <summary>
        /// The channel that the user notice was sent in.
        /// Always valid.
        /// </summary>
        public string           channel { get; protected set; }

        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// <para>Check the <code>is_valid</code> property to determine if tags were attached to the message.</para>
        /// </summary>
        public UserNoticeTags   tags    { get; protected set; }

        public UserNoticeEventArgs(IrcMessage message) : base(message)
        {
            if (message.parameters.IsValid())
            {
                channel =  message.parameters[0];
            }

            body = message.trailing;

            tags = new UserNoticeTags(message);
        }
    }
}
