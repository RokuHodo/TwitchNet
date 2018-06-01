// standard namespaces
using System;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    HostTargetEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// <para>The number of viewers watching hosting channel.</para>
        /// <para>Set to -1 if this value was not inlcuded in the message.</para>
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, -1)]
        public int              viewer_count    { get; protected set; }

        /// <summary>
        /// <para>The user that is being hosted.</para>
        /// <para>This is empty if the hosting channel stops hosting.</para>
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string           target_user     { get; protected set; }

        /// <summary>
        /// The channel that started or stopped hosting.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string           hosting_channel { get; protected set; }

        /// <summary>
        /// <para>Whether the hosting channel started or stopped hosting a channel.</para>
        /// <para>Set to <see cref="HostTargetType.None"/> if the message could not be parsed.</para>
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, HostTargetType.None)]
        public HostTargetType   target_type     { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="HostTargetEventArgs"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public HostTargetEventArgs(IrcMessage message) : base(message)
        {
            target_type = HostTargetType.None;

            if (!message.parameters.IsValid())
            {
                return;
            }

            hosting_channel = message.parameters[0];

            if (!message.trailing.IsValid())
            {
                return;
            }

            if (message.trailing[0] == '-')
            {
                target_type = HostTargetType.Stop;

                target_user = string.Empty;
            }
            else
            {
                target_type = HostTargetType.Start;

                target_user = message.trailing.TextBefore(' ');
                if (!target_user.IsValid())
                {
                    target_user = message.trailing;
                }
            }

            viewer_count = Int32.TryParse(message.trailing.TextAfter(' '), out int _viewer_count) ? _viewer_count : -1;
        }
    }
}