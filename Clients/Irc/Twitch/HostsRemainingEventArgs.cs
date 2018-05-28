// standard namespaces
using System;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    HostsRemainingEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string   channel     { get; protected set; }

        /// <summary>
        /// The remaining number of hosts that can be used until it resets.
        /// Set to -1 if the value could not be parsed.
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, -1)]
        public int      remaining   { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="HostsRemainingEventArgs"/> class.
        /// </summary>
        /// <param name="args">The event arguments to parse.</param>
        public HostsRemainingEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            remaining = Int32.TryParse(args.body.TextBefore(' '), out int _hosts_remaining) ? _hosts_remaining : -1;
        }
    }
}