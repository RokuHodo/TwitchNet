// standard namespaces
using System;
using System.Collections.Generic;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc.Twitch
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
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command USERNOTICE with the <see cref="UserNoticeType.Sub"/> or <see cref="UserNoticeType.Resub"/> msg-id tag</para>
        /// <para>Signifies that a user subscribed or resubscribed to a channel.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<SubscriberEventArgs>              OnSubscriber;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command USERNOTICE with the <see cref="UserNoticeType.GiftSub"/> msg-id tag</para>
        /// <para>Signifies that a user gifted a subscription to another user.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<GiftedSubscriberEventArgs>        OnGiftedSubscriber;

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

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE in a stream chat.</para>
        /// <para>These are general server notices sent specifically to the client, with a few excpections that are sent to all users in a channel's stream chat regardless of the source.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<NoticeEventArgs>                  OnNotice;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE in a chat room.</para>
        /// <para>These are general server notices sent specifically to the client, with a few excpections that are sent to all users in a channel's chat room regardless of the source.</para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomNoticeEventArgs>          OnChatRoomNotice;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.AlreadyBanned"/> msg-id tag in a stream chat.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that a user was attempted to be banned/timed out in a channel's stream chat, but is already banned/timed out.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<AlreadyBannedEventArgs>           OnAlreadyBanned;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.AlreadyBanned"/> msg-id tag in a chat room.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnChatRoomNotice"/>.
        /// Signifies that a user was attempted to be banned/timed out in a channel's stream chat, but is already banned/timed out.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomAlreadyBannedEventArgs>   OnChatRoomAlreadyBanned;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.BadModMod"/> msg-id tag in a stream chat.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that a user was attempted to be modded in a channel's stream chat, but is already modded.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BadModModEventArgs>               OnBadModMod;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.BadModMod"/> msg-id tag in a chat room.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnChatRoomNotice"/>.
        /// Signifies that a user was attempted to be modded in a channel's chat room, but is already modded.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomBadModModEventArgs>       OnChatRoomBadModMod;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.BadHostHosting"/> msg-id tag.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that a user was attempted to be hosted, but is already being hosted.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BadHostHostingEventArgs>          OnBadHostHosting;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.BadHostRateExceeded"/> msg-id tag.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that more than the maximum number of users was attempted to be hosted in half an hour.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BadHostRateExceededEventArgs>     OnBadHostRateExceeded;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.BadUnbanNoBan"/> msg-id tag in a stream chat.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that a user was attempted to be unbanned/untimed out in a channel's stream chat, but is not banned/timed out.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BadUnbanNoBanEventArgs>           OnBadUnbanNoBan;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.BadUnbanNoBan"/> msg-id tag in a chat room.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnChatRoomNotice"/>.
        /// Signifies that a user was attempted to be unbanned/untimed out in a channel's chat room, but is not banned/timed out.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomBadUnbanNoBanEventArgs>   OnChatRoomBadUnbanNoBan;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.BadUnmodMod"/> msg-id tag in a stream chat.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that a user was attempted to be unmodded in a channel's stream chat, but is not a moderator.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BadUnmodModEventArgs>             OnBadUnmodMod;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.BadUnmodMod"/> msg-id tag in a chat room.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnChatRoomNotice"/>.
        /// Signifies that a user was attempted to be unmodded in a channel's stream chat, but is not a moderator.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomBadUnmodModEventArgs>     OnChatRoomBadUnmodMod;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.CmdsAvailable"/> msg-id tag in a stream chat.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Contains the chat commands that can be used in a channel's stream chat.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<CmdsAvailableEventArgs>           OnCmdsAvailable;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.CmdsAvailable"/> msg-id tag in a chat room.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnChatRoomNotice"/>.
        /// Contains the chat commands that can be used in a channel's chat room.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomCmdsAvailableEventArgs>   OnChatRoomCmdsAvailable;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.HostsRemaining"/> msg-id tag.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Contains how many hosts can be used until the value resets.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<HostsRemainingEventArgs>          OnHostsRemaining;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.InvalidUser"/> msg-id tag in a stream chat.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that an invalid user nick was provided when trying to use a chat command in a channel's stream chat.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<InvalidUserEventArgs>             OnInvalidUser;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.InvalidUser"/> msg-id tag in a chat room.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnChatRoomNotice"/>.
        /// Signifies that an invalid user nick was provided when trying to use a chat command in a channel's stream chat.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomInvalidUserEventArgs>     OnChatRoomInvalidUser;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.RoomMods"/> msg-id tag in a stream chat.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Contains a list of a channel's moderators.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<RoomModsEventArgs>                OnRoomMods;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.RoomMods"/> msg-id tag in a chat room.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnChatRoomNotice"/>.
        /// Contains a list of a channel's moderators.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomRoomModsEventArgs>        OnChatRoomRoomMods;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.UnbanSuccess"/> msg-id tag in a stream chat.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that a user was unbanned from a channel's stream chat.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<UnbanSuccessEventArgs>            OnUnbanSuccess;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.UnbanSuccess"/> msg-id tag in a chat room.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnChatRoomNotice"/>.
        /// Signifies that a user was unbanned from a channel's chat room.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ChatRoomUnbanSuccessEventArgs>    OnChatRoomUnbanSuccess;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.UnsupportedChatRoomsCmd"/> msg-id tag.</para>
        /// <para>
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

            // TODO: Move to IrcClient.EvenArgs. This is a native IRC command.
            SetHandler("NOTICE", HandleNotice);
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
            UserNoticeType type = TwitchIrcUtil.Tags.ToUserNoticeType(message, "msg-id");
            switch (type)
            {
                case UserNoticeType.Sub:
                case UserNoticeType.Resub:
                {
                    OnSubscriber.Raise(this, new SubscriberEventArgs(message));
                }
                break;

                case UserNoticeType.SubGift:
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

                case UserNoticeType.Other:
                default:
                {
                    OnUserNotice.Raise(this, new UserNoticeEventArgs(message));
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

        /// <summary>
        /// Handles the IRC message with the command NOTICE.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        private void
        HandleNotice(in IrcMessage message)
        {
            if (IsChatRoom(message))
            {
                ChatRoomNoticeEventArgs args = new ChatRoomNoticeEventArgs(message);
                OnChatRoomNotice.Raise(this, args);

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

        #region Helpers

        /// <summary>
        /// Checks if the message originated from a chat room.
        /// </summary>
        /// <param name="channel">The IRC channel to check.</param>
        /// <returns>
        /// Returns false if channel is null, empty, or whitepsace, or if the channel does not start with #chatrooms.
        /// Returns true if the parameter element starts with #chatrooms.
        /// </returns>
        internal static bool
        IsChatRoom(string channel)
        {
            bool result = !channel.IsNull() && channel.TextBefore(':') == "#chatrooms";

            return result;
        }

        /// <summary>
        /// Checks if the message originated from a chat room.
        /// </summary>
        /// <param name="message">The message to check.</param>
        /// <param name="index">The index of the message parameters to check.</param>
        /// <returns>
        /// Returns false if message is null or default, if the message parameters are null or empty, if the index is out of range, or if the parameter element does not start with #chatrooms.
        /// Returns true if the parameter element starts with #chatrooms.
        /// </returns>
        private bool
        IsChatRoom(IrcMessage message, int index = 0)
        {
            if (message.IsNullOrDefault())
            {
                return false;
            }

            if (index < 0)
            {
                return false;
            }

            if (!message.parameters.IsValid())
            {
                return false;
            }

            if (!index.IsInRange(0, message.parameters.Length - 1))
            {
                return false;
            }

            bool result = message.parameters[index].TextBefore(':') == "#chatrooms";

            return result;
        }        

        #endregion
    }
}