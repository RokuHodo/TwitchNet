﻿// standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    ChatRoonEndOfNamesEventArgs : EndOfNamesEventArgs
    {
        /// <summary>
        /// The id of the user who the chat room belongs to.
        /// </summary>
        public string channel_user_id { get; protected set; }

        /// <summary>
        /// The unique UUID of the chat room.
        /// </summary>
        public string channel_uuid { get; protected set; }

        public ChatRoonEndOfNamesEventArgs(IrcMessage message, Dictionary<string, List<string>> names) : base(message, names)
        {
            channel_user_id = channel.TextBetween(':', ':');

            int index = channel.LastIndexOf(':');
            if (index != -1)
            {
                channel_uuid = channel.TextAfter(':', index);
            }
        }
    }
}
