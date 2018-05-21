// project namespaces
using TwitchNet.Utilities;

namespace
TwitchNet.Models.Clients.Irc
{
    public struct
    IrcUser
    {
        private string _nick;
        private string _pass;

        /// <summary>
        /// The nick name of the Irc user.
        /// </summary>
        /// <exception cref="FormatException">Thrown if the string is not between 2 and 24 characters long, and does not only contian alpha-numeric characters.</exception>
        public string nick
        {
            get
            {
                ExceptionUtil.ThrowIfInvalidNick(_nick);
                return _nick;
            }
            set
            {
                _nick = value;
            }
        }

        /// <summary>
        /// The password of the Irc user.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the pass is null, emtpy, or whitespace.</exception>
        public string pass
        {
            get
            {
                ExceptionUtil.ThrowIfInvalid(_pass, nameof(_pass));
                return _pass;
            }
            set
            {
                _pass = value;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="IrcUser"/> struct.
        /// </summary>
        /// <param name="user_nick">The client nick.</param>
        /// <param name="user_pass">The client pass.</param>
        public IrcUser(string user_nick, string user_pass)
        {
            _nick   = user_nick;
            _pass   = user_pass;

            nick    = _nick;
            pass    = user_pass;
        }
    }
}
