// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    UserNoticeEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// <para>The messsage sent by the user.</para>
        /// <para>This is empty if the user did not send a message.</para>
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string           body    { get; protected set; }

        /// <summary>
        /// The channel that the user notice was sent in.
        /// Always valid.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string           channel { get; protected set; }

        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// <para>Check the <code>exist</code> property to determine if tags were attached to the message.</para>
        /// </summary>
        [ValidateMember(Check.Tags)]
        public UserNoticeTags   tags    { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="UserNoticeEventArgs"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
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
