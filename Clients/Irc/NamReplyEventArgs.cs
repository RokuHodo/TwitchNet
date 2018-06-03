// standard namespaces
using System;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;

namespace
TwitchNet.Clients.Irc
{
    public class
    NamReplyEventArgs : NumericReplyEventArgs
    {
        /// <summary>
        /// The character that specifies if the channel is public, secret, or private.
        /// </summary>
        [ValidateMember(Check.IsNotNullOrDefault)]
        public char     status      { get; protected set; }

        /// <summary>
        /// The IRC channel that the clients have joined.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string   channel     { get; protected set; }

        /// <summary>
        /// A partial or complete list of client nicks that have joined the channel.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string[] names       { get; protected set; }

        /// <summary>
        /// The chnanel is public if the status is equal to '='.
        /// </summary>
        [ValidateMember(Check.IsNotNull)]
        public bool     is_public   { get; protected set; }

        /// <summary>
        /// The channel is secret if the status is equal to '@'.
        /// </summary>
        [ValidateMember(Check.IsNotNull)]
        public bool     is_secret   { get; protected set; }

        /// <summary>
        /// The channel is private if the status is equal to '*'.
        /// </summary>
        [ValidateMember(Check.IsNotNull)]
        public bool     is_private  { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="NamReplyEventArgs"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public NamReplyEventArgs(IrcMessage message) : base(message)
        {
            names = message.trailing.Split(' ');

            if (!message.parameters.IsValid() || message.parameters.Length < 3)
            {
                return;
            }

            status = message.parameters[1][0];
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
