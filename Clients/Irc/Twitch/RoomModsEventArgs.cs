﻿// standard namespaces
using System;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    RoomModsEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string   channel     { get; protected set; }

        /// <summary>
        /// The room's moderators.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string[] user_nicks  { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="RoomModsEventArgs"/> class.
        /// </summary>
        /// <param name="args">The event arguments to parse.</param>
        public RoomModsEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            string nicks = args.body.TextAfter(':').Trim(' ');
            user_nicks = nicks.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}