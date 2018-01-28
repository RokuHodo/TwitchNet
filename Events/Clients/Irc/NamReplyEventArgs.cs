// standard namespaces
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
        public char     status      { get; protected set; }

        public string   channel     { get; protected set; }

        public string[] names       { get; protected set; }

        public bool     is_public   { get; protected set; }
        public bool     is_secret   { get; protected set; }
        public bool     is_private  { get; protected set; }

        public NamReplyEventArgs(IrcMessage message) : base(message)
        {
            if (!message.parameters.IsValid())
            {
                return;
            }

            if(message.parameters.Length < 3)
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

            names = message.trailing.StringToArray<string>(' ');
        }
    }
}
