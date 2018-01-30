// standrad namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Extensions;

namespace
TwitchNet.Models.Clients.Irc
{
    public class
    Privmsg
    {
        public Dictionary<string, string>   tags    { get; protected set; }

        public string                       client  { get; protected set; }
        public string                       user    { get; protected set; }
        public string                       host    { get; protected set; }
        public string                       channel { get; protected set; }
        public string                       body    { get; protected set; }

        public Privmsg(IrcMessage message)
        {
            tags    = message.tags;
            client  = message.server_or_nick;
            user    = message.user;
            host    = message.host;

            if (message.parameters.IsValid())
            {
                channel = message.parameters[0];
            }

            body = message.trailing;
        }
    }
}
