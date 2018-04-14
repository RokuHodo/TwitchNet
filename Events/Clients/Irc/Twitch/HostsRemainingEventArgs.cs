// standard namespaces
using System;

// project namespaces
using TwitchNet.Extensions;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    HostsRemainingEventArgs : IrcMessageEventArgs
    {        
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        public string   channel         { get; protected set; }

        /// <summary>
        /// The remaining number of hosts that can be used until it resets.
        /// Set to -1 if the value could not be parsed.
        /// </summary>
        public int      remaining { get; protected set; }

        public HostsRemainingEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            remaining = Int32.TryParse(args.body.TextBefore(' '), out int _hosts_remaining) ? _hosts_remaining : -1;
        }
    }
}