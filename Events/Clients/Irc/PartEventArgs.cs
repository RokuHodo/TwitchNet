// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;

namespace
TwitchNet.Events.Clients.Irc
{
    public class
    PartEventArgs : IrcMessageEventArgs
    {
        public string nick      { get; protected set; }
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
