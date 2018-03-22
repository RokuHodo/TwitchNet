// standard namespaces
using System;
using System.Collections.Generic;

// project namespaces
using TwitchNet.Enums.Clients.Irc.Twitch;
using TwitchNet.Events.Clients.Irc;
using TwitchNet.Events.Clients.Irc.Twitch;
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc
{
    public partial class
    TwitchIrcClient : IrcClient
    {
        #region IRC event overrides

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command 353, RPL_NAMREPLY.</para>
        /// <para>Contains a partial list of users that haved joined a channel's stream chat.</para>
        /// </summary>
        public override event EventHandler<NamReplyEventArgs>   OnNamReply;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command 353, RPL_NAMREPLY.</para>
        /// <para>Contains a partial list of users that haved joined a channel's chat room.</para>
        /// </summary>
        public event EventHandler<ChatRoomNamReplyEventArgs>    OnChatRoomNamReply;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command 366, RPL_ENDOFNAMES.</para>
        /// <para>Contains a complete list of users that haved joined a channel's stream chat.</para>
        /// </summary>
        public override event EventHandler<EndOfNamesEventArgs> OnEndOfNames;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command 366, RPL_ENDOFNAMES.</para>
        /// <para>Contains a complete list of users that haved joined a channel's chat room.</para>
        /// </summary>
        public event EventHandler<ChatRoonEndOfNamesEventArgs>  OnChatRoomOnEndOfNames;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command JOIN.</para>
        /// <para>Signifies that a user has joined a channel's stream chat.</para>
        /// </summary>
        public override event EventHandler<JoinEventArgs>       OnJoin;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command JOIN.</para>
        /// <para>Signifies that a user has joined a channel's chat room.</para>
        /// </summary>
        public event EventHandler<ChatRoomJoinEventArgs>        OnChatRoomJoin;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command PART.</para>
        /// <para>Signifies that a user has left a channel's stream chat.</para>
        /// </summary>
        public override event EventHandler<PartEventArgs>       OnPart;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command PART.</para>
        /// <para>Signifies that a user has left a channel's chat room.</para>
        /// </summary>
        public event EventHandler<ChatRoomPartEventArgs>        OnChatRoomPart;

        #endregion

        #region Custom twitch events

        /// <summary>
        /// <para>Raised when a user gains or loses operator (mod) status in a channel.</para>
        /// <para>Requires /membership.</para>
        /// </summary>
        public event EventHandler<ChannelOperatorEventArgs>     OnChannelOperator;

        /// <summary>
        /// <para>Raised when a message was sent in a chanel's stream chat.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<StreamChatPrivmsgEventArgs>   OnStreamChatPrivmsg;

        /// <summary>
        /// <para>Raised when a message was sent in a channel's chat room.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomPrivmsgEventArgs>     OnChatRoomPrivmsg;

        /// <summary>
        /// <para>Raised when the client recieves a whisper.</para>
        /// <para>Supplementary tags can be added to the message by requesting /tags.</para>
        /// </summary>
        public event EventHandler<WhisperEventArgs>             OnWhisper;

        /// <summary>
        /// <para>Raised when a channel's stream chat gets cleared or when a user gets timed out or banned in a stream chat.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<ClearChatEventArgs>           OnClearChat;

        /// <summary>
        /// <para>Raised when the a user gets timed out or banned in a channel's chat room.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomClearChatEventArgs>   OnChatRoomClearchat;

        /// <summary>
        /// <para>Raised after the client successfully logs in.</para>
        /// <para>
        /// Requires /commands and must be requested before the client logs in.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<GlobalUserStateEventArgs>     OnGlobalUserstate;

        /// <summary>
        /// <para>Raised when the client joins a channel's stream chat or a room setting is changed.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<RoomStateEventArgs>           OnRoomState;

        /// <summary>
        /// <para>Raised when the client joins a channel's chat room or a room setting is changed.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomRoomStateEventArgs>   OnChatRoomRoomState;

        /// <summary>
        /// <para>Raised when a user subscribes or resubscribes to a channel.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<SubscriberEventArgs>          OnSubscriber;

        /// <summary>
        /// <para>Raised when a user gifts a subscription to another user.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<GiftedSubscriberEventArgs>    OnGiftedSubscriber;

        /// <summary>
        /// <para>Raised when a channel raids another channel.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<RaidEventArgs>                OnRaid;

        /// <summary>
        /// <para>Raised when a ritual occurs.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<RitualEventArgs>              OnRitual;

        /// <summary>
        /// <para>
        /// Raised as a default event when <see cref="OnSubscriber"/>, <see cref="OnGiftedSubscriber"/>, <see cref="OnRaid"/>, or <see cref="OnRitual"/> failed to be raised.
        /// This normally occures when /tags was not requested.
        /// </para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// This event only contains the common tags between all three user notices.
        /// </para>
        /// </summary>
        public event EventHandler<UserNoticeEventArgs>          OnUserNotice;

        /// <summary>
        /// <para>Raised when the client joins a channel's stream chat or sends a PRIVMSG to a user in a strem chat.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<UserStateEventArgs>           OnUserState;

        /// <summary>
        /// <para>Raised when the client joins channel's a chat room or sends a PRIVMSG in a chat room.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomUserStateEventArgs>   OnChatRoomUserState;

        /// <summary>
        /// <para>Raised when a channel starts or stops hosting another channel.</para>
        /// <para>Requires /commands.</para>
        /// </summary>
        public event EventHandler<HostTargetEventArgs>          OnHostTarget;

        /// <summary>
        /// <para>
        /// Raised when message is receieved to reconnect to the server.
        /// Clients should reconnect as soon as possible and rejoin all channels that they have joined.
        /// </para>
        /// <para>Requires /commands.</para>
        /// </summary>
        public event EventHandler<IrcMessageEventArgs>          OnReconnect;

        /// <summary>
        /// <para>Raised when a NOTICE message is sent in a channel's stream chat.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<NoticeEventArgs>              OnNotice;

        /// <summary>
        /// <para>Raised when a NOTICE message is sent in a channel's chat room.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomNoticeEventArgs>      OnChatRoomNotice;                                

        #endregion

        #region Base event callbacks

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
            if (args.channel.TextBefore(':') == "#chatrooms")
            {
                OnChatRoomPrivmsg.Raise(this, new ChatRoomPrivmsgEventArgs(args));
            }
            else
            {
                OnStreamChatPrivmsg.Raise(this, new StreamChatPrivmsgEventArgs(args));
            }
        }

        #endregion

        #region Base override handlers

        protected override void
        HandleNamReply(IrcMessage message)
        {
            if (message.parameters.Length < 3 || !message.parameters[2].IsValid())
            {
                return;
            }

            if (message.parameters[2].TextBefore(':') == "#chatrooms")
            {
                ChatRoomNamReplyEventArgs args = new ChatRoomNamReplyEventArgs(message);
                AddNames(args.channel, args.names);

                OnChatRoomNamReply.Raise(this, args);
            }
            else
            {
                NamReplyEventArgs args = new NamReplyEventArgs(message);
                AddNames(args.channel, args.names);

                OnNamReply.Raise(this, args);
            }            

            void
            AddNames(string key, string[] list)
            {
                if (!names.ContainsKey(key))
                {
                    names[key] = new List<string>();
                }

                if (list.IsValid())
                {
                    names[key].AddRange(list);
                }
            }            
        }

        protected override void
        HandleEndOfNames(IrcMessage message)
        {
            if (message.parameters[1].TextBefore(':') == "#chatrooms")
            {
                ChatRoonEndOfNamesEventArgs args = new ChatRoonEndOfNamesEventArgs(message, names);
                RemoveNames(args.channel);

                OnChatRoomOnEndOfNames.Raise(this, args);
            }
            else
            {
                EndOfNamesEventArgs args = new EndOfNamesEventArgs(message, names);
                RemoveNames(args.channel);

                OnEndOfNames.Raise(this, args);
            }

            void
            RemoveNames(string key)
            {
                if (names.ContainsKey(key))
                {
                    names.Remove(key);
                }
            }
        }

        protected override void
        HandleJoin(IrcMessage message)
        {
            if (!message.parameters[0].IsValid())
            {
                return;
            }

            if (message.parameters[0].TextBefore(':') == "#chatrooms")
            {
                OnChatRoomJoin.Raise(this, new ChatRoomJoinEventArgs(message));
            }
            else
            {
                OnJoin.Raise(this, new JoinEventArgs(message));
            }
        }

        protected override void
        HandlePart(IrcMessage message)
        {
            if (!message.parameters[0].IsValid())
            {
                return;
            }

            if (message.parameters[0].TextBefore(':') == "#chatrooms")
            {
                OnChatRoomPart.Raise(this, new ChatRoomPartEventArgs(message));
            }
            else
            {
                OnPart.Raise(this, new PartEventArgs(message));
            }
        }

        #endregion

        #region Twitch handlers

        public override void
        DefaultHandlers()
        {
            if (handlers.IsNull())
            {
                return;
            }

            // This is redundant on sart up since it will be called first in the base constructor,
            // But it's easier than manually managing all base handlers that we don't override
            base.DefaultHandlers();

            // IRC override handlers
            SetHandler("353", HandleNamReply);
            SetHandler("366", HandleEndOfNames);

            SetHandler("JOIN", HandleJoin);
            SetHandler("PART", HandlePart);

            // Twitch handlers
            SetHandler("WHISPER", HandleWhisper);
            SetHandler("CLEARCHAT", HandleClearChat);
            SetHandler("GLOBALUSERSTATE", HandleGlobalUserState);
            SetHandler("ROOMSTATE", HandleRoomState);
            SetHandler("USERNOTICE", HandleUserNotice);
            SetHandler("USERSTATE", HandleUserState);
            SetHandler("HOSTTARGET", HandleHostTarget);
            SetHandler("RECONNECT", HandleReconnect);
            SetHandler("NOTICE", HandleNotice);
        }

        private void
        HandleWhisper(IrcMessage message)
        {
            OnWhisper.Raise(this, new WhisperEventArgs(message));
        }

        private void
        HandleClearChat(IrcMessage message)
        {
            if (!message.parameters[0].IsValid())
            {
                return;
            }

            if (message.parameters[0].TextBefore(':') == "#chatrooms")
            {
                OnChatRoomClearchat.Raise(this, new ChatRoomClearChatEventArgs(message));
            }
            else
            {
                OnClearChat.Raise(this, new ClearChatEventArgs(message));
            }

        }

        private void
        HandleGlobalUserState(IrcMessage message)
        {
            OnGlobalUserstate.Raise(this, new GlobalUserStateEventArgs(message));
        }

        private void
        HandleRoomState(IrcMessage message)
        {
            if (!message.parameters[0].IsValid())
            {
                return;
            }

            if (message.parameters[0].TextBefore(':') == "#chatrooms")
            {
                OnChatRoomRoomState.Raise(this, new ChatRoomRoomStateEventArgs(message));
            }
            else
            {
                OnRoomState.Raise(this, new RoomStateEventArgs(message));
            }
        }

        private void
        HandleUserNotice(IrcMessage message)
        {
            UserNoticeType type = TagsUtil.ToUserNoticeType(message.tags, "msg-id");
            switch (type)
            {
                case UserNoticeType.Sub:
                case UserNoticeType.Resub:
                {
                    OnSubscriber.Raise(this, new SubscriberEventArgs(message));
                }
                break;

                case UserNoticeType.GiftSub:
                {
                    OnGiftedSubscriber.Raise(this, new GiftedSubscriberEventArgs(message));
                }
                break;

                case UserNoticeType.Raid:
                {
                    OnRaid.Raise(this, new RaidEventArgs(message));
                }
                break;

                case UserNoticeType.Ritual:
                {
                    OnRitual.Raise(this, new RitualEventArgs(message));
                }
                break;

                case UserNoticeType.None:
                default:
                {
                    OnUserNotice.Raise(this, new UserNoticeEventArgs(message));
                    break;
                }
            }
        }

        private void
        HandleUserState(IrcMessage message)
        {
            if (!message.parameters[0].IsValid())
            {
                return;
            }

            if (message.parameters[0].TextBefore(':') == "#chatrooms")
            {
                OnChatRoomUserState.Raise(this, new ChatRoomUserStateEventArgs(message));
            }
            else
            {
                OnUserState.Raise(this, new UserStateEventArgs(message));
            }
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
            if (!message.parameters[0].IsValid())
            {
                return;
            }

            if (message.parameters[0].TextBefore(':') == "#chatrooms")
            {
                OnChatRoomNotice.Raise(this, new ChatRoomNoticeEventArgs(message));
            }
            else
            {
                OnNotice.Raise(this, new NoticeEventArgs(message));
            }
        }

        #endregion        
    }
}