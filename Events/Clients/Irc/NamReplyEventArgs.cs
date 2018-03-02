﻿// standard namespaces
using System;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;

namespace
TwitchNet.Events.Clients.Irc
{
    public class
    NamReplyEventArgs : NumericReplyEventArgs
    {
        /// <summary>
        /// The character that specifies if the channel is public, secret, or private.
        /// </summary>
        public char     status      { get; protected set; }

        /// <summary>
        /// The IRC channel that the clients have joined.
        /// </summary>
        public string   channel     { get; protected set; }

        /// <summary>
        /// A partial or complete list of client nicks that have joined the channel.
        /// </summary>
        public string[] names       { get; protected set; }

        /// <summary>
        /// The chnanel is public if the status is equal to '='.
        /// </summary>
        public bool     is_public   { get; protected set; }

        /// <summary>
        /// The channel is secret if the status is equal to '@'.
        /// </summary>
        public bool     is_secret   { get; protected set; }

        /// <summary>
        /// The channel is private if the status is equal to '*'.
        /// </summary>
        public bool     is_private  { get; protected set; }

        public NamReplyEventArgs(IrcMessage message) : base(message)
        {
            names = message.trailing.StringToArray<string>(' ');

            if (!message.parameters.IsValid() || message.parameters.Length < 3)
            {
                return;
            }

            status = Convert.ToChar(message.parameters[1]);
            if(status == '=')
            {
                is_public = true;
            }
            else if(status == '@')
            {
                is_secret = true;
            }
            else if(status == '*')
            {
                is_private = true;
            }

            channel = message.parameters[2];
        }
    }
}
