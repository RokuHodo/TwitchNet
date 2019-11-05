// standard namespaces
using System;

// project namespaces
using TwitchNet.Extensions;

namespace
TwitchNet.Clients.Irc
{
    public partial class
    TwitchIrcClient : IrcClient
    {
        #region Events        

        /// <summary>
        /// <para>
        /// Raised when an <see cref="IrcMessage"/> is received with the command CLEARCHAT in a stream chat.
        /// Signifies that a channel's stream chat was cleared or that a user was timed out/banned in the stream chat.
        /// </para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<ClearChatEventArgs>       OnClearChat;

        /// <summary>
        /// <para>
        /// Raised when an <see cref="IrcMessage"/> is received with the command GLOBALUSERSTATE.
        /// Signifies that the client successfully logged into the IRC server.
        /// </para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<GlobalUserStateEventArgs> OnGlobalUserstate;

        /// <summary>
        /// <para>Raised when a channel starts or stops hosting another channel.</para>
        /// <para>Requires /commands.</para>
        /// </summary>
        public event EventHandler<HostTargetEventArgs>      OnHostTarget;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command RECONNECT.</para>
        /// <para>Signifies that the client should reconnect to the server.</para>
        /// <para>Requires /commands.</para>
        /// </summary>
        public event EventHandler<IrcMessageEventArgs>      OnReconnect;

        /// <summary>
        /// <para>
        /// Raised when an <see cref="IrcMessage"/> is received with the command ROOMSTATE in a stream chat.
        /// Signifies that the client joined a channel's stream chat or a stream chat's setting was changed.
        /// </para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<RoomStateEventArgs>       OnRoomState;

        /// <summary>
        /// <para>
        /// Raised when an <see cref="IrcMessage"/> is received with the command USERNOTICE and tags were not requested.
        /// Signfies that a user subscribed, resubscribed, gifted a subscription to another user, is raiding another user, or conducted a ritual.
        /// </para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<UserNoticeEventArgs>      OnUserNotice;

        /// <summary>
        /// <para>
        /// A secondary event to <see cref="OnUserNotice"/>.
        /// Raised when an <see cref="IrcMessage"/> is received with the command USERNOTICE with the any subscription msg-id tag.
        /// Signifies that a user subscribed or resubscribed to a channel.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<SubscriptionEventArgs>    OnSubscription;

        /// <summary>
        /// <para>
        /// A secondary event to <see cref="OnUserNotice"/>.
        /// Raised when an <see cref="IrcMessage"/> is received with the command USERNOTICE with the <see cref="UserNoticeType.Raid"/> msg-id tag.
        /// Signifies that a user raided another user.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<RaidEventArgs>            OnRaid;

        /// <summary>
        /// <para>
        /// A secondary event to <see cref="OnUserNotice"/>.
        /// Raised when an <see cref="IrcMessage"/> is received with the command USERNOTICE with the <see cref="UserNoticeType.Ritual"/> msg-id tag.
        /// Signifies that a ritual occurred.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<RitualEventArgs>          OnRitual;

        /// <summary>
        /// <para>
        /// A secondary event to <see cref="OnUserNotice"/>.
        /// Raised when an <see cref="IrcMessage"/> is received with the command USERNOTICE with the <see cref="UserNoticeType.BitsBadgeTier"/> msg-id tag.
        /// Signifies that a user earned a new Bits badge.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BitsBadgeEventArgs>       OnBitsBadge;        

        /// <summary>
        /// <para>
        /// Raised when an <see cref="IrcMessage"/> is received with the command USERSTATE in a stream chat.
        /// Signifies that the client joined a channel's stream chat or sent a PRIVMSG to a user in a stream chat.
        /// </para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<UserStateEventArgs>       OnUserState;        

        /// <summary>
        /// <para>
        /// Raised when an <see cref="IrcMessage"/> is received with the command WHISPER.
        /// Signifies that a direct chat message was received from another user.
        /// </para>
        /// <para>Supplementary tags can be added to the message by requesting /tags.</para>
        /// </summary>
        public event EventHandler<WhisperEventArgs>         OnWhisper;        

        #endregion

        #region Base event callbacks

        /// <summary>
        /// Callback for the <see cref="IrcClient.OnSocketConnected"/> event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        private void
        Callback_OnSocketConnected(object sender, EventArgs args)
        {
            if (request_user_info)
            {
                /*
                IHelixResponse<Data<User>> _twitch_user = TwitchApiBearer.GetUser(IRC_user.pass);
                if (_twitch_user.result.data.IsValid())
                {
                    twitch_user = _twitch_user.result.data[0];
                }
                */
            }

            if (request_commands)
            {
                RequestCommands();
            }

            if (request_membership)
            {
                RequestMembership();
            }

            if (request_tags)
            {
                RequestTags();
            }
        }

        #endregion

        #region Handlers

        /// <summary>
        /// Sets all <see cref="IrcMessage"/> handlers back to the default methods.
        /// </summary>
        public override void
        ResetHandlers()
        {
            base.ResetHandlers();

            // Twitch handlers
            SetHandler("CLEARCHAT",         Handle_ClearChat);
            SetHandler("GLOBALUSERSTATE",   Handle_GlobalUserState);
            SetHandler("HOSTTARGET",        Handle_HostTarget);
            SetHandler("RECONNECT",         Handle_Reconnect);
            SetHandler("ROOMSTATE",         Handle_RoomState);
            SetHandler("USERNOTICE",        Handle_UserNotice);
            SetHandler("USERSTATE",         Handle_UserState);
            SetHandler("WHISPER",           Handle_Whisper);
        }        

        /// <summary>
        /// Handles the IRC message with the command CLEARCHAT.
        /// </summary>
        /// <param name="message">The IRC message to be handled.</param>
        private void
        Handle_ClearChat(in IrcMessage message)
        {
            OnClearChat.Raise(this, new ClearChatEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command GLOBALUSERSTATE.
        /// </summary>
        /// <param name="message">The IRC message to be handled.</param>
        private void
        Handle_GlobalUserState(in IrcMessage message)
        {
            OnGlobalUserstate.Raise(this, new GlobalUserStateEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command HOSTTARGET.
        /// </summary>
        /// <param name="message">The IRC message to be handled.</param>
        private void
        Handle_HostTarget(in IrcMessage message)
        {
            OnHostTarget.Raise(this, new HostTargetEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command RECONNECT.
        /// </summary>
        /// <param name="message">The IRC message to be handled.</param>
        private void
        Handle_Reconnect(in IrcMessage message)
        {
            OnReconnect.Raise(this, new IrcMessageEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command ROOMSTATE.
        /// </summary>
        /// <param name="message">The IRC message to be handled.</param>
        private void
        Handle_RoomState(in IrcMessage message)
        {
            OnRoomState.Raise(this, new RoomStateEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command USERNOTICE.
        /// </summary>
        /// <param name="message">The IRC message to be handled.</param>
        private void
        Handle_UserNotice(in IrcMessage message)
        {
            UserNoticeEventArgs args = new UserNoticeEventArgs(message);
            OnUserNotice.Raise(this, args);

            if (!args.tags_exist)
            {
                return;
            }

            switch (args.tags.msg_id)
            {
                case UserNoticeType.Sub:
                case UserNoticeType.Resub:
                case UserNoticeType.SubGift:
                case UserNoticeType.AnonSubGift:
                case UserNoticeType.SubMysteryGift:
                case UserNoticeType.GiftPaidUpgrade:
                case UserNoticeType.AnonGiftPaidUpgrade:
                {
                    OnSubscription.Raise(this, new SubscriptionEventArgs(args));
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

                case UserNoticeType.BitsBadgeTier:
                {
                    OnBitsBadge.Raise(this, new BitsBadgeEventArgs(args));
                }
                break;
            }
        }

        /// <summary>
        /// Handles the IRC message with the command USERSTATE.
        /// </summary>
        /// <param name="message">The IRC message to be handled.</param>
        private void
        Handle_UserState(in IrcMessage message)
        {
            OnUserState.Raise(this, new UserStateEventArgs(message));
        }        

        /// <summary>
        /// Handles the IRC message with the command WHISPER.
        /// </summary>
        /// <param name="message">The IRC message to be handled.</param>
        private void
        Handle_Whisper(in IrcMessage message)
        {
            OnWhisper.Raise(this, new WhisperEventArgs(message));
        }

        #endregion
    }
}