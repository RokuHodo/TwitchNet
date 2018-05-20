﻿// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Enums.Debugger;
using TwitchNet.Extensions;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    BadHostHostingEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel   { get; protected set; }

        /// <summary>
        /// The user that was attempted to be hosted, but is already being hosted.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user_nick { get; protected set; }

        public BadHostHostingEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            int index = args.body.LastIndexOf(' ');
            if(index != 1)
            {
                user_nick = args.body.TextBetween(' ', '.', index);
            }
        }
    }
}