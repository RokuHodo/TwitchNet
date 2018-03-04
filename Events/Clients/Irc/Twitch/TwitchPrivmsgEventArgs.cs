// project namespaces
using TwitchNet.Models.Clients.Irc.Twitch;

namespace
TwitchNet.Events.Clients.Irc.Twitch
{
    public class
    TwitchPrivmsgEventArgs : PrivmsgEventArgs
    {
        /// <summary>
        /// The Twitch message send.
        /// </summary>
        public TwitchPrivmsg message_twitch_privmsg { get; protected set; }

        public TwitchPrivmsgEventArgs(PrivmsgEventArgs args) : base(args)
        {
            message_twitch_privmsg = new TwitchPrivmsg(args.message_privmsg);
        }
    }
}
