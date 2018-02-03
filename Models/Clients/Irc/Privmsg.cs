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
        /// <summary>
        /// The optional tags prefixed to the message.
        /// </summary>
        public Dictionary<string, string>   tags    { get; protected set; }

        /// <summary>
        /// The nick of the client who sent the message.
        /// </summary>
        public string                       nick    { get; protected set; }

        /// <summary>
        /// The irc user.
        /// </summary>
        public string                       user    { get; protected set; }

        /// <summary>
        /// The irc host.
        /// </summary>
        public string                       host    { get; protected set; }

        /// <summary>
        /// The channel the message was sent in.
        /// </summary>
        public string                       channel { get; protected set; }

        /// <summary>
        /// The body of the message.
        /// </summary>
        public string                       body    { get; protected set; }

        public Privmsg(IrcMessage message)
        {
            tags = message.tags;
            nick = message.server_or_nick;
            user = message.user;
            host = message.host;

            if (message.parameters.IsValid())
            {
                channel = message.parameters[0];
            }

            body = message.trailing;
        }
    }
}
