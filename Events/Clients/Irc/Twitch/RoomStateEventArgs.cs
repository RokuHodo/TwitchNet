// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;
using TwitchNet.Models.Clients.Irc.Twitch;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    RoomStateEventArgs : IrcMessageEventArgs
    {        
        /// <summary>
        /// The channel whose state has changed and/or the client has joined.
        /// Always valid.
        /// </summary>
        public string           channel             { get; protected set; }

        /// <summary>
        /// <para>The tags attached to the message, if any.</para>
        /// <para>Check the <code>is_valid</code> property to determine if tags were attached to the message.</para>
        /// </summary>
        public RoomStateTags    tags { get; protected set; }

        public RoomStateEventArgs(IrcMessage message) : base(message)
        {
            if (message.parameters.IsValid())
            {
                channel = message.parameters[0];
            }

            tags = new RoomStateTags(message);
        }
    }
}
