// standard namespaces
using System;
using System.Collections.Generic;
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
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc
{
    public class
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

        /// <summary>
        /// The information of the Twitch irc user.
        /// </summary>
        public User twitch_user { get; private set; }

        public TwitchIrcClient(ushort port, IrcUser irc_user) : base("irc.chat.twitch.tv", port, irc_user)
        {
            // allow upper case to not be too strict and just ToLower() it later
            Regex regex = new Regex("^[a-zA-Z][a-zA-Z0-9_]{2,24}$");
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

        #region Command wrappers

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

        public void
        SendWhisper(string recipient, string format, params string[] arguments)
        {
            ExceptionUtil.ThrowIfInvalid(recipient, nameof(recipient));
            ExceptionUtil.ThrowIfInvalid(format, nameof(format));

            string trailing = "/w " + recipient.ToLower() + " " + (!arguments.IsValid() ? format : string.Format(format, arguments));
            Send("PRIVMSG #jtv :" + trailing);
        }

        public void
        JoinChatRoom(string user_id, string uuid)
        {
            ExceptionUtil.ThrowIfInvalid(user_id, nameof(user_id));
            ExceptionUtil.ThrowIfInvalid(uuid, nameof(uuid));

            Send("JOIN #chatrooms:" + user_id + ":" + uuid);
        }

        public void
        PartChatRoom(string user_id, string uuid)
        {
            ExceptionUtil.ThrowIfInvalid(user_id, nameof(user_id));
            ExceptionUtil.ThrowIfInvalid(uuid, nameof(uuid));

            Send("PART #chatrooms:" + user_id + ":" + uuid);
        }

        public void
        SendChatRoomPrivmsg(string user_id, string uuid, string format, params string[] arguments)
        {
            ExceptionUtil.ThrowIfInvalid(user_id, nameof(user_id));
            ExceptionUtil.ThrowIfInvalid(uuid, nameof(uuid));
            ExceptionUtil.ThrowIfInvalid(format, nameof(format));

            string trailing = !arguments.IsValid() ? format : string.Format(format, arguments);
            Send("PRIVMSG #chatrooms:" + user_id + ":" + uuid + " :" + trailing);
        }

        #endregion

        #region IRC event callbacks

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

        #region IRC override handlers

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