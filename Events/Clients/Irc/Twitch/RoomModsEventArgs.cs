// standard namespaces
using System;

// project namespaces
using TwitchNet.Extensions;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    RoomModsEventArgs : IrcMessageEventArgs
    {        
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        public string   channel     { get; protected set; }

        /// <summary>
        /// The room's moderators.
        /// </summary>
        public string[] user_nicks  { get; protected set; }

        public RoomModsEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            string nicks = args.body.TextAfter(':').Trim(' ');
            user_nicks = nicks.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}