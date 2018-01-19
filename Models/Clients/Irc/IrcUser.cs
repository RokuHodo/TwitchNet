﻿// project namespaces
using TwitchNet.Utilities;

namespace
TwitchNet.Models.Clients.Irc
{
    public class
    IrcUser
    {
        private string _nick;
        private string _pass;

        /// <summary>
        /// The nick name of the Irc user.
        /// </summary>
        public string nick
        {
            get
            {
                return _nick;
            }
            set
            {
                ExceptionUtil.ThrowIfInvalid(value, nameof(nick));
                _nick = value;
            }
        }

        /// <summary>
        /// The password of the Irc user.
        /// </summary>
        public string pass
        {
            get
            {
                return _pass;
            }
            set
            {
                ExceptionUtil.ThrowIfInvalid(value, nameof(pass));
                _pass = value;
            }
        }

        public IrcUser(string nick, string pass)
        {
            this.nick = nick;
            this.pass = pass;
        }

        public IrcUser()
        {

        }
    }
}