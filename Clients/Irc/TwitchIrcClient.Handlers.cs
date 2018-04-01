// standard namespaces
using System;
using System.Collections.Generic;

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
        public override event EventHandler<NamReplyEventArgs>       OnNamReply;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command 353, RPL_NAMREPLY.</para>
        /// <para>Contains a partial list of users that haved joined a channel's chat room.</para>
        /// </summary>
        public event EventHandler<ChatRoomNamReplyEventArgs>        OnChatRoomNamReply;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command 366, RPL_ENDOFNAMES.</para>
        /// <para>Contains a complete list of users that haved joined a channel's stream chat.</para>
        /// </summary>
        public override event EventHandler<EndOfNamesEventArgs>     OnEndOfNames;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command 366, RPL_ENDOFNAMES.</para>
        /// <para>Contains a complete list of users that haved joined a channel's chat room.</para>
        /// </summary>
        public event EventHandler<ChatRoonEndOfNamesEventArgs>      OnChatRoomOnEndOfNames;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command JOIN.</para>
        /// <para>Signifies that a user has joined a channel's stream chat.</para>
        /// </summary>
        public override event EventHandler<JoinEventArgs>           OnJoin;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command JOIN.</para>
        /// <para>Signifies that a user has joined a channel's chat room.</para>
        /// </summary>
        public event EventHandler<ChatRoomJoinEventArgs>            OnChatRoomJoin;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command PART.</para>
        /// <para>Signifies that a user has left a channel's stream chat.</para>
        /// </summary>
        public override event EventHandler<PartEventArgs>           OnPart;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command PART.</para>
        /// <para>Signifies that a user has left a channel's chat room.</para>
        /// </summary>
        public event EventHandler<ChatRoomPartEventArgs>            OnChatRoomPart;

        #endregion

        #region Custom twitch events

        /// <summary>
        /// <para>Raised when a user gains or loses operator (mod) status in a channel.</para>
        /// <para>Requires /membership.</para>
        /// </summary>
        public event EventHandler<ChannelOperatorEventArgs>         OnChannelOperator;

        /// <summary>
        /// <para>Raised when a message was sent in a chanel's stream chat.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<StreamChatPrivmsgEventArgs>       OnStreamChatPrivmsg;

        /// <summary>
        /// <para>Raised when a message was sent in a channel's chat room.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomPrivmsgEventArgs>         OnChatRoomPrivmsg;

        /// <summary>
        /// <para>Raised when the client recieves a whisper.</para>
        /// <para>Supplementary tags can be added to the message by requesting /tags.</para>
        /// </summary>
        public event EventHandler<WhisperEventArgs>                 OnWhisper;

        /// <summary>
        /// <para>Raised when a channel's stream chat gets cleared or when a user gets timed out or banned in a stream chat.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<ClearChatEventArgs>               OnClearChat;

        /// <summary>
        /// <para>Raised when the a user gets timed out or banned in a channel's chat room.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomClearChatEventArgs>       OnChatRoomClearchat;

        /// <summary>
        /// <para>Raised after the client successfully logs in.</para>
        /// <para>
        /// Requires /commands and must be requested before the client logs in.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<GlobalUserStateEventArgs>         OnGlobalUserstate;

        /// <summary>
        /// <para>Raised when the client joins a channel's stream chat or a room setting is changed.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<RoomStateEventArgs>               OnRoomState;

        /// <summary>
        /// <para>Raised when the client joins a channel's chat room or a room setting is changed.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomRoomStateEventArgs>       OnChatRoomRoomState;

        /// <summary>
        /// <para>Raised when a user subscribes or resubscribes to a channel.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<SubscriberEventArgs>              OnSubscriber;

        /// <summary>
        /// <para>Raised when a user gifts a subscription to another user.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<GiftedSubscriberEventArgs>        OnGiftedSubscriber;

        /// <summary>
        /// <para>Raised when a channel raids another channel.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<RaidEventArgs>                    OnRaid;

        /// <summary>
        /// <para>Raised when a ritual occurs.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<RitualEventArgs>                  OnRitual;

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
        public event EventHandler<UserNoticeEventArgs>              OnUserNotice;

        /// <summary>
        /// <para>Raised when the client joins a channel's stream chat or sends a PRIVMSG to a user in a strem chat.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<UserStateEventArgs>               OnUserState;

        /// <summary>
        /// <para>Raised when the client joins channel's a chat room or sends a PRIVMSG in a chat room.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomUserStateEventArgs>       OnChatRoomUserState;

        /// <summary>
        /// <para>Raised when a channel starts or stops hosting another channel.</para>
        /// <para>Requires /commands.</para>
        /// </summary>
        public event EventHandler<HostTargetEventArgs>              OnHostTarget;

        /// <summary>
        /// <para>
        /// Raised when message is receieved to reconnect to the server.
        /// Clients should reconnect as soon as possible and rejoin all channels that they have joined.
        /// </para>
        /// <para>Requires /commands.</para>
        /// </summary>
        public event EventHandler<IrcMessageEventArgs>              OnReconnect;

        /// <summary>
        /// <para>Raised when a NOTICE message is sent in a channel's stream chat.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<NoticeEventArgs>                  OnNotice;

        /// <summary>
        /// <para>Raised when a NOTICE message is sent in a channel's chat room.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomNoticeEventArgs>          OnChatRoomNotice;

        /// <summary>
        /// <para>
        /// Raised when a NOTICE message is sent in a channel's stream chat and contains the <see cref="NoticeType.AlreadyBanned"/> msg-id tag.
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that a user was attempted to be banned or timed out in a channel's stream chat, but is already banned or timed out.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<AlreadyBannedEventArgs>           OnAlreadyBanned;

        /// <summary>
        /// <para>
        /// Raised when a NOTICE message is sent in a channel's chat room and contains the <see cref="NoticeType.AlreadyBanned"/> msg-id tag.
        /// A secondary event raised after <see cref="OnChatRoomNotice"/>.
        /// Signifies that a user was attempted to be banned or timed out in a channel's stream chat, but is already banned or timed out.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomAlreadyBannedEventArgs>   OnChatRoomAlreadyBanned;

        /// <summary>
        /// <para>
        /// Raised when a NOTICE message is sent in a channel's stream chat and contains the <see cref="NoticeType.BadModMod"/> msg-id tag.
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that a user was attempted to be modded in a channel's stream chat, but is already modded.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BadModModEventArgs>               OnBadModMod;

        /// <summary>
        /// <para>
        /// Raised when a NOTICE message is sent in a channel's stream chat and contains the <see cref="NoticeType.BadModMod"/> msg-id tag.
        /// A secondary event raised after <see cref="OnChatRoomNotice"/>.
        /// Signifies that a user was attempted to be modded in a channel's chat room, but is already modded.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomBadModModEventArgs>       OnChatRoomBadModMod;

        /// <summary>
        /// <para>
        /// Raised when a NOTICE message is sent in a channel's stream chat and contains the <see cref="NoticeType.BadHostHosting"/> msg-id tag.
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that a user was attempted to be hosted, but is already being hosted.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BadHostHostingEventArgs>          OnBadHostHosting;        

        /// <summary>
        /// <para>
        /// Raised when a NOTICE message is sent in a channel's stream chat and contains the <see cref="NoticeType.BadHostRateExceeded"/> msg-id tag.
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that more than the maximum number of users was attempted to be hosted in half an hour.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BadHostRateExceededEventArgs>     OnBadHostRateExceeded;

        /// <summary>
        /// <para>
        /// Raised when a NOTICE message is sent in a channel's stream chat and contains the <see cref="NoticeType.BadUnbanNoBan"/> msg-id tag.
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that a user was attempted to be unbanned or untimed out in a channel's stream chat, but is not banned or timed out.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BadUnbanNoBanEventArgs>           OnBadUnbanNoBan;

        /// <summary>
        /// <para>
        /// Raised when a NOTICE message is sent in a channel's stream chat and contains the <see cref="NoticeType.BadUnbanNoBan"/> msg-id tag.
        /// A secondary event raised after <see cref="OnChatRoomNotice"/>.
        /// Signifies that a user was attempted to be unbanned or untimed out in a channel's chat room, but is not banned or timed out.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomBadUnbanNoBanEventArgs>   OnChatRoomBadUnbanNoBan;

        /// <summary>
        /// <para>
        /// Raised when a NOTICE message is sent in a channel's stream chat and contains the <see cref="NoticeType.BadUnmodMod"/> msg-id tag.
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that a user was attempted to be unmodded in a channel's stream chat, but is not a moderator.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BadUnmodModEventArgs>             OnBadUnmodMod;

        /// <summary>
        /// <para>
        /// Raised when a NOTICE message is sent in a channel's stream chat and contains the <see cref="NoticeType.BadUnmodMod"/> msg-id tag.
        /// A secondary event raised after <see cref="OnChatRoomNotice"/>.
        /// Signifies that a user was attempted to be unmodded in a channel's stream chat, but is not a moderator.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomBadUnmodModEventArgs>     OnChatRoomBadUnmodMod;

        /// <summary>
        /// <para>
        /// Raised when a NOTICE message is sent in a channel's stream chat and contains the <see cref="NoticeType.CmdsAvailable"/> msg-id tag.
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Contains the chat commands that can be used in a channel's stream chat.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<CmdsAvailableEventArgs>           OnCmdsAvailable;

        /// <summary>
        /// <para>
        /// Raised when a NOTICE message is sent in a channel's stream chat and contains the <see cref="NoticeType.CmdsAvailable"/> msg-id tag.
        /// A secondary event raised after <see cref="OnChatRoomNotice"/>.
        /// Contains the chat commands that can be used in a channel's chat room.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomCmdsAvailableEventArgs>   OnChatRoomCmdsAvailable;

        /// <summary>
        /// <para>
        /// Raised when a NOTICE message is sent in a channel's stream chat and contains the <see cref="NoticeType.HostsRemaining"/> msg-id tag.
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Contains how many hosts can be used until the value resets.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<HostsRemainingEventArgs>          OnHostsRemaining;

        /// <summary>
        /// <para>
        /// Raised when a NOTICE message is sent in a channel's stream chat and contains the <see cref="NoticeType.InvalidUser"/> msg-id tag.
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that an invalid user nick was provided when trying to use a chat command in a channel's stream chat.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<InvalidUserEventArgs>             OnInvalidUser;

        /// <summary>
        /// <para>
        /// Raised when a NOTICE message is sent in a channel's stream chat and contains the <see cref="NoticeType.InvalidUser"/> msg-id tag.
        /// A secondary event raised after <see cref="OnChatRoomNotice"/>.
        /// Signifies that an invalid user nick was provided when trying to use a chat command in a channel's stream chat.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomInvalidUserEventArgs>     OnChatRoomInvalidUser;

        /// <summary>
        /// <para>
        /// Raised when a NOTICE message is sent in a channel's stream chat and contains the <see cref="NoticeType.RoomMods"/> msg-id tag.
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Contains a list of a room's moderators.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<RoomModsEventArgs>                OnRoomMods;

        /// <summary>
        /// <para>
        /// Raised when a NOTICE message is sent in a channel's stream chat and contains the <see cref="NoticeType.RoomMods"/> msg-id tag.
        /// A secondary event raised after <see cref="OnChatRoomNotice"/>.
        /// Contains a list of a room's moderators.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomRoomModsEventArgs>        OnChatRoomRoomMods;

        /// <summary>
        /// <para>
        /// Raised when a NOTICE message is sent in a channel's stream chat and contains the <see cref="NoticeType.UnbanSuccess"/> msg-id tag.
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that a user was unbanned from a channel's stream chat.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<UnbanSuccessEventArgs>            OnUnbanSuccess;

        /// <summary>
        /// <para>
        /// Raised when a NOTICE message is sent in a channel's stream chat and contains the <see cref="NoticeType.UnbanSuccess"/> msg-id tag.
        /// A secondary event raised after <see cref="OnChatRoomNotice"/>.
        /// Signifies that a user was unbanned from a channel's chat room.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomUnbanSuccessEventArgs>    OnChatRoomUnbanSuccess;

        /// <summary>
        /// <para>
        /// Raised when a NOTICE message is sent in a channel's chat room and contains the <see cref="NoticeType.UnsupportedChatRoomsCmd"/> msg-id tag.
        /// A secondary event raised after <see cref="OnChatRoomNotice"/>.
        /// Signifies that a chat command was attempted to be used in a channel's chat room, but is not supported in chat rooms.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<UnsupportedChatRoomCmdEventArgs>  OnUnsupportedChatRoomCmd;

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
                IApiResponse<Data<User>> _twitch_user = TwitchApiBearer.GetUser(irc_user.pass);
                if (!_twitch_user.result.data.IsValid())
                {
                    return;
                }
                twitch_user = _twitch_user.result.data[0];
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
                ChatRoomNoticeEventArgs args = new ChatRoomNoticeEventArgs(message);
                OnChatRoomNotice.Raise(this, args);

                // TODO: Need to find a better way of doing this
                switch (args.tags.msg_id)
                {
                    case NoticeType.AlreadyBanned:
                    {
                        OnChatRoomAlreadyBanned.Raise(this, new ChatRoomAlreadyBannedEventArgs(args));
                    }
                    break;

                    case NoticeType.BadModMod:
                    {
                        OnChatRoomBadModMod.Raise(this, new ChatRoomBadModModEventArgs(args));
                    }
                    break;

                    case NoticeType.BadUnbanNoBan:
                    {
                        OnChatRoomBadUnbanNoBan.Raise(this, new ChatRoomBadUnbanNoBanEventArgs(args));
                    }
                    break;

                    case NoticeType.BadUnmodMod:
                    {
                        OnChatRoomBadUnmodMod.Raise(this, new ChatRoomBadUnmodModEventArgs(args));
                    }
                    break;

                    case NoticeType.CmdsAvailable:
                    {
                        OnChatRoomCmdsAvailable.Raise(this, new ChatRoomCmdsAvailableEventArgs(args));
                    }
                    break;

                    case NoticeType.InvalidUser:
                    {
                        OnChatRoomInvalidUser.Raise(this, new ChatRoomInvalidUserEventArgs(args));
                    }
                    break;

                    case NoticeType.RoomMods:
                    {
                        OnChatRoomRoomMods.Raise(this, new ChatRoomRoomModsEventArgs(args));
                    }
                    break;

                    case NoticeType.UnbanSuccess:
                    {
                        OnChatRoomUnbanSuccess.Raise(this, new ChatRoomUnbanSuccessEventArgs(args));
                    }
                    break;

                    case NoticeType.UnsupportedChatRoomsCmd:
                    {
                        OnUnsupportedChatRoomCmd.Raise(this, new UnsupportedChatRoomCmdEventArgs(args));
                    }
                    break;
                }
            }
            else
            {
                NoticeEventArgs args = new NoticeEventArgs(message);
                OnNotice.Raise(this, args);

                // TODO: Need to find a better way of doing this
                switch (args.tags.msg_id)
                {
                    case NoticeType.AlreadyBanned:
                    {
                        OnAlreadyBanned.Raise(this, new AlreadyBannedEventArgs(args));
                    }
                    break;

                    case NoticeType.BadHostHosting:
                    {
                        OnBadHostHosting.Raise(this, new BadHostHostingEventArgs(args));
                    }
                    break;

                    case NoticeType.BadHostRateExceeded:
                    {
                        OnBadHostRateExceeded.Raise(this, new BadHostRateExceededEventArgs(args));
                    }
                    break;

                    case NoticeType.BadModMod:
                    {
                        OnBadModMod.Raise(this, new BadModModEventArgs(args));
                    }
                    break;

                    case NoticeType.BadUnbanNoBan:
                    {
                        OnBadUnbanNoBan.Raise(this, new BadUnbanNoBanEventArgs(args));
                    }
                    break;

                    case NoticeType.BadUnmodMod:
                    {
                        OnBadUnmodMod.Raise(this, new BadUnmodModEventArgs(args));
                    }
                    break;

                    case NoticeType.CmdsAvailable:
                    {
                        OnCmdsAvailable.Raise(this, new CmdsAvailableEventArgs(args));
                    }
                    break;

                    case NoticeType.HostsRemaining:
                    {
                        OnHostsRemaining.Raise(this, new HostsRemainingEventArgs(args));
                    }
                    break;

                    case NoticeType.InvalidUser:
                    {
                        OnInvalidUser.Raise(this, new InvalidUserEventArgs(args));
                    }
                    break;

                    case NoticeType.RoomMods:
                    {
                        OnRoomMods.Raise(this, new RoomModsEventArgs(args));
                    }
                    break;

                    case NoticeType.UnbanSuccess:
                    {
                        OnUnbanSuccess.Raise(this, new UnbanSuccessEventArgs(args));
                    }
                    break;
                }
            }
        }

        #endregion        
    }
}