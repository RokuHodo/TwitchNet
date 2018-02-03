// standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;

namespace
TwitchNet.Events.Clients.Irc
{
    public class
    EndOfNamesEventArgs : NumericReplyEventArgs
    {
        /// <summary>
        /// The channel that the clients have joined.
        /// </summary>
        public string   channel     { get; protected set; }

        /// <summary>
        /// The complete list of client nicks that have joined the channel.
        /// </summary>
        public string[] names       { get; protected set; }

        public EndOfNamesEventArgs(IrcMessage message, Dictionary<string, List<string>> names) : base(message)
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
