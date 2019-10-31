// standard namespaces
using System;
using System.Collections.Generic;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc
{
    public partial class
    TwitchIrcClient : IrcClient
    {
        #region Twitch events

        /// <summary>
        /// <para>
        /// Raised when an <see cref="IrcMessage"/> is received with the command WHISPER.
        /// Signifies that a direct chat message was received from another user.
        /// </para>
        /// <para>Supplementary tags can be added to the message by requesting /tags.</para>
        /// </summary>
        public event EventHandler<WhisperEventArgs>                 OnWhisper;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command CLEARCHAT in a stream chat.</para>
        /// <para>Signifies that a channel's stream chat was cleared or that a user was timed out/banned in the stream chat.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<ClearChatEventArgs>               OnClearChat;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command GLOBALUSERSTATE.</para>
        /// <para>Signifies that the client successfully logged into the irc server.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<GlobalUserStateEventArgs>         OnGlobalUserstate;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command ROOMSTATE in a stream chat.</para>
        /// <para>Signifies that the client joined a channel's stream chat or a stream chat's setting was changed.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<RoomStateEventArgs>               OnRoomState;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command USERNOTICE with the any subscription msg-id tag</para>
        /// <para>Signifies that a user subscribed or resubscribed to a channel.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<SubscriptionEventArgs>            OnSubscription;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command USERNOTICE with the <see cref="UserNoticeType.Raid"/> msg-id tag</para>
        /// <para>Signifies that a user raided another user.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<RaidEventArgs>                    OnRaid;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command USERNOTICE with the <see cref="UserNoticeType.Ritual"/> msg-id tag</para>
        /// <para>Signifies that a ritual occurred.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<RitualEventArgs>                  OnRitual;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command USERNOTICE with the <see cref="UserNoticeType.BitsBadgeTier"/> msg-id tag</para>
        /// <para>Signifies that a user earned a new Bits badge.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BitsBadgeEventArgs>               OnBitsBadge;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command USERNOTICE and tags were not requested.</para>
        /// <para>
        /// Signfies that a user subscribed, resubscribed, gifted a subscription to another user, is raiding another user, conducted a ritual, or an unknown user notice occurred.
        /// The exact action cannot be determined because tags were not requested or the user notice was not found in <see cref="UserNoticeType"/>.
        /// </para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<UserNoticeEventArgs>              OnUserNotice;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command USERSTATE in a stream chat.</para>
        /// <para>Signifies that the client joined a channel's stream chat or sent a PRIVMSG to a user in a stream chat.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<UserStateEventArgs>               OnUserState;

        /// <summary>
        /// <para>Raised when a channel starts or stops hosting another channel.</para>
        /// <para>Requires /commands.</para>
        /// </summary>
        public event EventHandler<HostTargetEventArgs>              OnHostTarget;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command RECONNECT.</para>
        /// <para>Signifies that the client should reconnect to the server.</para>
        /// <para>Requires /commands.</para>
        /// </summary>
        public event EventHandler<IrcMessageEventArgs>              OnReconnect;                                        

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
                IHelixResponse<Data<User>> _twitch_user = TwitchApiBearer.GetUser(irc_user.pass);
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

        #region Twitch handlers

        /// <summary>
        /// Sets all <see cref="IrcMessage"/> handlers back to the default methods.
        /// </summary>
        public override void
        ResetHandlers()
        {
            base.ResetHandlers();

            // Twitch handlers
            SetHandler("WHISPER", HandleWhisper);
            SetHandler("CLEARCHAT", HandleClearChat);
            SetHandler("GLOBALUSERSTATE", HandleGlobalUserState);
            SetHandler("ROOMSTATE", HandleRoomState);
            SetHandler("USERNOTICE", HandleUserNotice);
            SetHandler("USERSTATE", HandleUserState);
            SetHandler("HOSTTARGET", HandleHostTarget);
            SetHandler("RECONNECT", HandleReconnect);
        }

        /// <summary>
        /// Handles the IRC message with the command WHISPER.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        private void
        HandleWhisper(in IrcMessage message)
        {
            OnWhisper.Raise(this, new WhisperEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command CLEARCHAT.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        private void
        HandleClearChat(in IrcMessage message)
        {
            OnClearChat.Raise(this, new ClearChatEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command GLOBALUSERSTATE.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        private void
        HandleGlobalUserState(in IrcMessage message)
        {
            OnGlobalUserstate.Raise(this, new GlobalUserStateEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command ROOMSTATE.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        private void
        HandleRoomState(in IrcMessage message)
        {
            OnRoomState.Raise(this, new RoomStateEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command USERNOTICE.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        private void
        HandleUserNotice(in IrcMessage message)
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
        /// <param name="message">The irc message to be handled.</param>
        private void
        HandleUserState(in IrcMessage message)
        {
            OnUserState.Raise(this, new UserStateEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command HOSTTARGET.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        private void
        HandleHostTarget(in IrcMessage message)
        {
            OnHostTarget.Raise(this, new HostTargetEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command RECONNECT.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        private void
        HandleReconnect(in IrcMessage message)
        {
            OnReconnect.Raise(this, new IrcMessageEventArgs(message));
        }

        #endregion
    }
}