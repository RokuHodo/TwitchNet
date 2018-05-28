// standrad namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;

namespace
TwitchNet.Clients.Irc
{
    public class
    PrivmsgEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The optional tags prefixed to the message.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public Dictionary<string, string>   tags    { get; protected set; }

        /// <summary>
        /// The nick of the client who sent the message.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string                       nick    { get; protected set; }

        /// <summary>
        /// The irc user.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string                       user    { get; protected set; }

        /// <summary>
        /// The irc host.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string                       host    { get; protected set; }

        /// <summary>
        /// The channel the message was sent in.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string                       channel { get; protected set; }

        /// <summary>
        /// The body of the message.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string                       body    { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="PrivmsgEventArgs"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public PrivmsgEventArgs(IrcMessage message) : base(message)
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
