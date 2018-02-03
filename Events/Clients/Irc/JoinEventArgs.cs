// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;

namespace
TwitchNet.Events.Clients.Irc
{
    public class
    JoinEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The nick of the client who joined a channel.
        /// </summary>
        public string nick      { get; protected set; }

        /// <summary>
        /// The channel the client has joined.
        /// </summary>
        public string channel   { get; protected set; }

        public JoinEventArgs(IrcMessage message) : base(message)
        {
            nick = message.server_or_nick;

            if (!message.parameters.IsValid())
            {
                return;
            }

            channel = message.parameters[0];
        }
    }
}
