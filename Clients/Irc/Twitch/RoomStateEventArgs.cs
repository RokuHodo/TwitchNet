// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    RoomStateEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The channel whose state has changed and/or the client has joined.
        /// Always valid.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string           channel { get; protected set; }

        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// <para>Check the <code>exist</code> property to determine if tags were attached to the message.</para>
        /// </summary>
        [ValidateMember(Check.TagsMissing)]
        public RoomStateTags    tags    { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="RoomStateEventArgs"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public RoomStateEventArgs(in IrcMessage message) : base(message)
        {
            if (message.parameters.IsValid())
            {
                channel = message.parameters[0];
            }

            tags = new RoomStateTags(message);
        }
    }
}
