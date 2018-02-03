// project namespaces
using TwitchNet.Models.Clients.Irc;

namespace
TwitchNet.Events.Clients.Irc
{
    public class
    PrivmsgEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The parsed private message.
        /// </summary>
        public Privmsg message_privmsg { get; protected set; }

        public PrivmsgEventArgs(IrcMessage message) : base(message)
        {
            message_privmsg = new Privmsg(message);
        }
    }
}
