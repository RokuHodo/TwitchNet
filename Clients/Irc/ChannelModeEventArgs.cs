// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;

namespace
TwitchNet.Clients.Irc
{
    public class
    ChannelModeEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// Denotes the whether the mode was added '+', or removed '-'.
        /// </summary>
        [ValidateMember(Check.IsNotNullOrDefault)]
        public char     modifier    { get; protected set; }

        /// <summary>
        /// The change that occured to either the channel or the user.
        /// </summary>
        [ValidateMember(Check.IsNotNullOrDefault)]
        public char     mode        { get; protected set; }

        /// <summary>
        /// A combination of the 'modifier' and the 'mode'.
        /// The complete change that occured.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string   mode_set    { get; protected set; }

        /// <summary>
        /// The IRC channel whose mode was changed.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string   channel     { get; protected set; }

        // TODO: Change to an array since this could be none or all 3 arguments,

        /// <summary>
        /// Arguments, if any, associated with the mode change.
        /// These inckude a ban mask, limit, and/or an IRC user.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string   arguments   { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="ChannelModeEventArgs"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public ChannelModeEventArgs(in IrcMessage message) : base(message)
        {
            if (!message.parameters.IsValid() || message.parameters.Length < 3)
            {
                return;
            }

            channel = message.parameters[0];

            // This assumes only one argument after the mode set.
            // This is fine for Twitch, but change this to an array because it *could* be up to 3 parameters after the mode set.
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
