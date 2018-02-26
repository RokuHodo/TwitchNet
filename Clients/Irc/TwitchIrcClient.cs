// standard namespaces
using System;
using System.Text.RegularExpressions;

// project namespaces
using TwitchNet.Api;
using TwitchNet.Events.Clients.Irc;
using TwitchNet.Events.Clients.Irc.Twitch;
using TwitchNet.Extensions;
using TwitchNet.Interfaces.Api;
using TwitchNet.Models.Api;
using TwitchNet.Models.Api.Users;
using TwitchNet.Models.Clients.Irc;

namespace
TwitchNet.Clients.Irc
{
    public class
    TwitchIrcClient : IrcClient
    {
        /// <summary>
        /// Raised when a user gains operator status.
        /// </summary>
        public event EventHandler<ChannelOperatorEventArgs> OnUserModded;

        /// <summary>
        /// Raised when a user looses operator status.
        /// </summary>
        public event EventHandler<ChannelOperatorEventArgs> OnUserUnmodded;

        /// <summary>
        /// Raised when a Twitch message was sent.
        /// </summary>
        public event EventHandler<TwitchPrivmsgEventArgs>   OnTwitchPrivmsg;

        /// <summary>
        /// The information of the Twitch irc user.
        /// </summary>
        public User twitch_user { get; private set; }

        public TwitchIrcClient(ushort port, IrcUser irc_user) : base("irc.chat.twitch.tv", port, irc_user)
        {
            // allow upper case to not be too strict and just ToLower() it later
            Regex regex = new Regex("^[a-zA-Z][a-zA-Z0-9_]{3,24}$");
            if (!regex.IsMatch(irc_user.nick))
            {
                throw new ArgumentException(nameof(irc_user.nick) + " can only contain alpha-numeric characters and must be at least 3 characters long.", nameof(irc_user.nick));
            }

            IApiResponse<Data<User>> _twitch_user = TwitchApiBearer.GetUser(irc_user.pass);
            if (!_twitch_user.result.data.IsValid())
            {
                throw new Exception("Could not get the user associated with the Bearer token");
            }

            twitch_user = _twitch_user.result.data[0];

            OnChannelMode += new EventHandler<ChannelModeEventArgs>(Callback_OnChannelMode);
            OnPrivmsg += new EventHandler<PrivmsgEventArgs>(Callback_OnPrivmsg);
        }

        #region Sending

        /// <summary>
        /// <para>Adds membership state event data.</para>
        /// <para>
        /// JOIN and PART messages will be received when users join or leave rooms.
        /// MODE messages will be received when a users gains or looses mod status.
        /// 353 and 366 messages will be populated with current chatters in a room.
        /// </para>
        /// </summary>
        public void
        RequestMembership()
        {
            Send("CAP REQ :twitch.tv/membership");
        }

        /// <summary>
        /// Adds IRC V3 message tags to several commands.
        /// </summary>
        public void
        RequestTags()
        {
            Send("CAP REQ :twitch.tv/tags");
        }

        /// <summary>
        /// Enables Twitch specific commands, such as HOSTTARGET.
        /// </summary>
        public void
        RequestCommands()
        {
            Send("CAP REQ :twitch.tv/commands");
        }

        #endregion

        #region Event callbacks

        /// <summary>
        /// Callback for the <see cref="IrcClient.OnChannelMode"/> event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        private void
        Callback_OnChannelMode(object sender, ChannelModeEventArgs args)
        {            
            if(args.mode_set == "+o")
            {
                OnUserModded.Raise(this, new ChannelOperatorEventArgs(args));
            }
            else if (args.mode_set == "-o")
            {
                OnUserUnmodded.Raise(this, new ChannelOperatorEventArgs(args));
            }
        }

        /// <summary>
        /// Callback for the <see cref="IrcClient.OnPrivmsg"/> event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        private void
        Callback_OnPrivmsg(object sender, PrivmsgEventArgs args)
        {
            OnTwitchPrivmsg.Raise(this, new TwitchPrivmsgEventArgs(args));
            // TODO: Parse into a TwitchPrivmsg and raise that event
        }

        #endregion
    }
}
