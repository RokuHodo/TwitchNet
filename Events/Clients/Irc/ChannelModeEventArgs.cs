﻿// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;

namespace
TwitchNet.Events.Clients.Irc
{
    public class
    ChannelModeEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// Denotes the whether the mode was added '+', or removed '-'.
        /// </summary>
        public char     modifier    { get; protected set; }

        /// <summary>
        /// The change that occured to either the channel or the user.
        /// </summary>
        public char     mode        { get; protected set; }

        /// <summary>
        /// A combination of the 'modifier' and the 'mode_set'.
        /// The complete change that occured.
        /// </summary>
        public string   mode_set    { get; protected set; }

        /// <summary>
        /// The IRC channel whose mode was changed.
        /// </summary>
        public string   channel     { get; protected set; }

        /// <summary>
        /// Any arguments, if any, associated with the mode such as masks or specific values.
        /// </summary>
        public string   arguments   { get; protected set; }

        public ChannelModeEventArgs(IrcMessage message) : base(message)
        {
            if (!message.parameters.IsValid() || message.parameters.Length < 3)
            {
                return;
            }

            channel = message.parameters[0];
            arguments = message.parameters[2];

            mode_set = message.parameters[1];
            if(message.parameters[1].Length < 2)
            {
                return;
            }
            modifier = message.parameters[1][0];
            mode = message.parameters[1][1];
        }
    }
}
