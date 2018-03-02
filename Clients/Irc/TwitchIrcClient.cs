// standard namespaces
using System;
using System.Text.RegularExpressions;

// project namespaces
using TwitchNet.Api;
using TwitchNet.Enums.Clients.Irc.Twitch;
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
        /// <para>Raised when a user gains operator status.</para>
        /// <para>Requires /membership.</para>
        /// </summary>
        public event EventHandler<ChannelOperatorEventArgs> OnUserModded;

        /// <summary>
        /// <para>Raised when a user looses operator status,</para>
        /// <para>Requires /membership.</para>
        /// </summary>
        public event EventHandler<ChannelOperatorEventArgs> OnUserUnmodded;

        /// <summary>
        /// <para>Raised when a Twitch message was sent.</para>
        /// <para>Supplementary tags can be added to the message by requesting /tags.</para>
        /// </summary>
        public event EventHandler<TwitchPrivmsgEventArgs>   OnTwitchPrivmsg;

        /// <summary>
        /// <para>Raised when a user gets timed out or banned.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<ClearChatEventArgs>       OnClearChat;

        /// <summary>
        /// <para>Raised after the client successfully logs in.</para>
        /// <para>
        /// Requires /commands.
        /// Must be requested before the client logs in.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<GlobalUserStateEventArgs> OnGlobalUserstate;

        /// <summary>
        /// <para>Raised when a user joins a channel or a room setting is changed.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<RoomStateEventArgs>       OnRoomState;

        /// <summary>
        /// <para>Raised when a user subscribes or resubscribes to a channel.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// This event will not work properly without first requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<SubscriberEventArgs>      OnSubscriber;

        /// <summary>
        /// <para>Raised when a channel raids another channel.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// This event will not work properly without first requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<RaidEventArgs>            OnRaid;

        /// <summary>
        /// <para>Raised when a ritual occurs.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// This event will not work properly without first requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<RitualEventArgs>          OnRitual;

        /// <summary>
        /// <para>
        /// Raised when a user subscribes or resubscribes to a channel, a channel raids another channel, or a ritual occurs.
        /// This will only be raised if the events <see cref="OnSubscriber"/>, <see cref="OnRitual"/>, or <see cref="OnRaid"/> fail to parse the message.
        /// Failure to parse the message into its proper sub event should only happen when /tags was not requested.
        /// </para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// This event will not work properly without first requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<UserNoticeEventArgs>      OnUserNotice;

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

            SetHandler("CLEARCHAT", HandleClearChat);
            SetHandler("GLOBALUSERSTATE", HandleGlobalUserState);
            SetHandler("ROOMSTATE", HandleRoomState);
            SetHandler("USERNOTICE", HandleUserNotice);
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
        }

        #endregion

        #region Handlers

        private void
        HandleClearChat(IrcMessage message)
        {
            OnClearChat.Raise(this, new ClearChatEventArgs(message));
        }

        private void
        HandleGlobalUserState(IrcMessage message)
        {
            OnGlobalUserstate.Raise(this, new GlobalUserStateEventArgs(message));
        }

        private void
        HandleRoomState(IrcMessage message)
        {
            OnRoomState.Raise(this, new RoomStateEventArgs(message));
        }

        private void
        HandleUserNotice(IrcMessage message)
        {
            UserNoticeEventArgs args = new UserNoticeEventArgs(message);
            switch (args.msg_id)
            {
                case UserNoticeType.Sub:
                case UserNoticeType.Resub:
                {
                    OnSubscriber.Raise(this, new SubscriberEventArgs(args));
                }
                break;

                case UserNoticeType.Raid:
                {
                    OnRaid.Raise(this, new RaidEventArgs(args));
                }
                break;

                case UserNoticeType.Ritual:
                {
                    OnRitual.Raise(this, new RitualEventArgs(args));
                }
                break;

                case UserNoticeType.None:
                default:
                {
                    OnUserNotice.Raise(this, args);
                }
                break;
            }
        }

        #endregion
    }
}
