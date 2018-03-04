﻿// standard namespaces
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
        /// <para>Raised when a user gains or loses operator (mod) status in a channel.</para>
        /// <para>Requires /membership.</para>
        /// </summary>
        public event EventHandler<ChannelOperatorEventArgs> OnChannelOperator;

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
        /// This event only contains the common tags between all three user notices.
        /// </para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<UserNoticeEventArgs>      OnUserNotice;

        /// <summary>
        /// <para>Raised when a user joins a channel or sends a PRIVMSG to a channel.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<UserStateEventArgs>       OnUserState;

        /// <summary>
        /// <para>Raised when a channel starts or stops hosting another channel.</para>
        /// <para>Requires /commands.</para>
        /// </summary>
        public event EventHandler<HostTargetEventArgs>      OnHostTarget;

        /// <summary>
        /// <para>
        /// Raised when Twitch sends a notification for IRC clients to reconnect.
        /// Clients should reconnect and rejoin all channels that they have joined.
        /// </para>
        /// <para>Requires /commands.</para>
        /// </summary>
        public event EventHandler<IrcMessageEventArgs>      OnReconnect;

        /// <summary>
        /// <para>
        /// Raised when Twitch sends a NOTICE message.
        /// </para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<NoticeEventArgs>          OnNotice;

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
            SetHandler("USERSTATE", HandleUserState);
            SetHandler("HOSTTARGET", HandleHostTarget);
            SetHandler("RECONNECT", HandleReconnect);
            SetHandler("NOTICE", HandleNotice);
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
            OnChannelOperator.Raise(this, new ChannelOperatorEventArgs(args));
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
            OnUserNotice.Raise(this, args);

            switch (args.tags.msg_id)
            {
                // TODO: Look into gifted subs, knowing Twitch they probably don't follow either of these
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
            }
        }

        private void
        HandleUserState(IrcMessage message)
        {
            OnUserState.Raise(this, new UserStateEventArgs(message));
        }

        private void
        HandleHostTarget(IrcMessage message)
        {
            OnHostTarget.Raise(this, new HostTargetEventArgs(message));
        }

        private void
        HandleReconnect(IrcMessage message)
        {
            OnReconnect.Raise(this, new IrcMessageEventArgs(message));
        }
        private void
        HandleNotice(IrcMessage message)
        {
            OnNotice.Raise(this, new NoticeEventArgs(message));
        }

        #endregion
    }
}
