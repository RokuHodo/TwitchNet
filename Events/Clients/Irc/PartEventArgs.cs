// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;

namespace
TwitchNet.Events.Clients.Irc
{
    public class
    PartEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The nick of the client who left a channel.
        /// </summary>
        public string nick      { get; protected set; }

        /// <summary>
        /// The channel the client has left.
        /// </summary>
        public string channel   { get; protected set; }

        public PartEventArgs(IrcMessage message) : base(message)
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
