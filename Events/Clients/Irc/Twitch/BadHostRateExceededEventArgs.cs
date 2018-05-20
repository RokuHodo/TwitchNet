// standard namespaces
using System;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Enums.Debugger;
using TwitchNet.Extensions;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    BadHostRateExceededEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string   channel         { get; protected set; }

        /// <summary>
        /// The maximum number of users that can be hosted in half an hour.
        /// Set to -1 if the value could not be parsed.
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, -1)]
        public int      host_rate_limit { get; protected set; }

        public BadHostRateExceededEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            host_rate_limit = Int32.TryParse(args.body.TextBetween("than ", " times"), out int _host_rate_limit) ? _host_rate_limit : -1;
        }
    }
}