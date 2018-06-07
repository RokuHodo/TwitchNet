// standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;

namespace
TwitchNet.Clients.Irc
{
    public class
    EndOfNamesEventArgs : NumericReplyEventArgs
    {
        /// <summary>
        /// The IRC channel that the clients have joined.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string   channel { get; protected set; }

        /// <summary>
        /// The complete list of client nicks that have joined the channel.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string[] names   { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="EndOfNamesEventArgs"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        /// <param name="names">The complete list of client nicks that have joined the channel.</param>
        public EndOfNamesEventArgs(in IrcMessage message, Dictionary<string, List<string>> names) : base(message)
        {
            if (!message.parameters.IsValid() || message.parameters.Length < 2)
            {
                return;
            }

            channel = message.parameters[1];

            if (names.ContainsKey(channel))
            {
                this.names = names[channel].ToArray();
            }
        }
    }
}
