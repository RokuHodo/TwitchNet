// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Enums.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;
using TwitchNet.Models.Clients.Irc.Twitch;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    WhisperEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The user who sent the whisper.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string       sender      { get; protected set; }

        /// <summary>
        /// The user who the whisper was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string       recipient   { get; protected set; }

        /// <summary>
        /// The body of the message.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string       body        { get; protected set; }

        /// <summary>
        /// <para>The tags attached to the whisper, if any.</para>
        /// <para>Check the <code>is_valid</code> property to determine if tags were attached to the message.</para>
        /// </summary>
        [ValidateMember(Check.Tags)]
        public WhisperTags  tags        { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="WhisperEventArgs"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public WhisperEventArgs(IrcMessage message) : base(message)
        {
            sender = message.server_or_nick;

            if (message.parameters.IsValid())
            {
                recipient = message.parameters[0];
            }

            body = message.trailing;

            tags = new WhisperTags(message);
        }
    }
} 