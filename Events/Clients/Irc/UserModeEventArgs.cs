// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;

namespace
TwitchNet.Events.Clients.Irc
{
    public class
    UserModeEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The name of the user whose mode was changed.
        /// </summary>
        public string   nick     { get; protected set; }

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

        public UserModeEventArgs(IrcMessage message) : base(message)
        {
            if (!message.parameters.IsValid() || message.parameters.Length < 2)
            {
                return;
            }

            nick = message.parameters[0];

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
