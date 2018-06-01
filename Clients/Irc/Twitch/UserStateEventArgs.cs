// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    UserStateEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The channel that the user has joined or sent sent a message in.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string           channel { get; protected set; }

        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// <para>Check the <code>exist</code> property to determine if tags were attached to the message.</para>
        /// </summary>
        [ValidateMember(Check.Tags)]
        public UserStateTags    tags    { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="UserStateEventArgs"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public UserStateEventArgs(IrcMessage message) : base(message)
        {
            if (message.parameters.IsValid())
            {
                channel = message.parameters[0];
            }

            tags = new UserStateTags(message);
        }
    }
}