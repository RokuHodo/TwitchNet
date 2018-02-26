// project namespaces
using TwitchNet.Models.Clients.Irc;

namespace
TwitchNet.Events.Clients.Irc
{
    public class
    PrivmsgEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The parsed IRC privmsg.
        /// </summary>
        public Privmsg message_privmsg { get; protected set; }

        public PrivmsgEventArgs(IrcMessage message) : base(message)
        {
            message_privmsg = new Privmsg(message);
        }

        public PrivmsgEventArgs(PrivmsgEventArgs args) : base(args.message_irc)
        {
            message_privmsg = args.message_privmsg;
        }
    }
}
