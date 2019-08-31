// standard namespaces
using System;
using System.Collections.Generic;
using System.IO;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc
{
    public partial class
    IrcClient : IDisposable
    {
        #region Delegates

        /// <summary>
        /// The method signature of a IRC message handler.
        /// </summary>
        /// <param name="message_irc"></param>
        public delegate void MessageHandler(in IrcMessage message_irc);

        #endregion

        #region Fields

        /// <summary>
        /// The partial or full list of channel names that have joined a channel.
        /// Used with commands '353' and '366'.
        /// </summary>
        protected            Dictionary<string, List<string>>        names;

        /// <summary>
        /// The message handlers.
        /// </summary>
        protected            Dictionary<string, MessageHandler>      handlers;

        /// <summary>
        /// <para>Raised data is receieved from the server.</para>
        /// </summary>
        public virtual event EventHandler<DataEventArgs>            OnDataReceived;

        /// <summary>
        /// <para>Raised when data is sent by the client.</para>
        /// </summary>
        public virtual event EventHandler<DataEventArgs>            OnDataSent;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received from the server.</para>
        /// </summary>
        public virtual event EventHandler<IrcMessageEventArgs>      OnIrcMessageReceived;

        /// <summary>
        /// <para>
        /// Raised when an the internal socket establishes a connection to the IRC server.
        /// Signifies that the client is ready to send and receive messages.
        /// </para>
        /// <para>        
        /// This occurs before the client logs into the IRC server and before <see cref="OnConnected"/> is raised.
        /// At this point, <see cref="state"/> is still equal to <see cref="ClientState.Connecting"/> since the client is not yet logged into the IRC.
        /// </para>
        /// </summary>
        public virtual event EventHandler<EventArgs>                OnSocketConnected;        

        /// <summary>
        /// Raised when an <see cref="IrcMessage"/> is received with the command 001, RPL_WELCOME.
        /// Signifies that the <see cref="IrcClient"/> has successfully registered and connected to the IRC server.
        /// </summary>
        public virtual event EventHandler<NumericReplyEventArgs>    OnConnected;

        /// <summary>
        /// <para>
        /// Raised when an the internal socket disconnects from the IRC server.
        /// Signifies that the client can no longer send and receive messages.
        /// </para>
        /// <para>        
        /// This occurs before the reader finishes processing the current data, before all currently raised events have finihshed, and before <see cref="OnDisconnected"/> is raised.
        /// At this point, <see cref="state"/> is still equal to <see cref="ClientState.Disconnecting"/> since the reader is still waiting for all event to finish executing.
        /// </para>
        /// </summary>
        public virtual event EventHandler<EventArgs>                OnSocketDisconnected;

        /// <summary>
        /// <para>Raised when the <see cref="IrcClient"/> disconnects from the IRC server.</para>
        /// <para>
        /// This is only raised when a client has chosen to manually disconnect.
        /// This is not raised when the connection is terminated by the server because no message is sent signifying the end of the connection. 
        /// </para>
        /// </summary>
        public virtual event EventHandler<EventArgs>                OnDisconnected;

        /// <summary>
        /// Raised when the <see cref="IrcClient"/> is disposed.
        /// </summary>
        public virtual event EventHandler<EventArgs>                OnDisposed;

        /// <summary>
        /// Raised when an <see cref="IrcMessage"/> is received with the command 002, RPL_YOURHOST.
        /// Gives information about the IRC server the <see cref="IrcClient"/> has connected too.
        /// This is part of the post-registration process.
        /// </summary>
        public virtual event EventHandler<NumericReplyEventArgs>    OnYourHost;

        /// <summary>
        /// <para>
        /// Raised when an <see cref="IrcMessage"/> is received with the command 003, RPL_CREATED.
        /// Gives information about when the IRC server was started or created.
        /// This is part of the post-registration process.
        /// </para>        
        /// </summary>
        public virtual event EventHandler<NumericReplyEventArgs>    OnCreated;

        /// <summary>
        /// Raised when an <see cref="IrcMessage"/> is received with the command 004, RPL_MYINFO.
        /// Gives information about the available modes that are available to use.
        /// This is part of the post-registration process.
        /// </summary>
        public virtual event EventHandler<NumericReplyEventArgs>    OnMyInfo;

        /// <summary>
        /// Raised when an <see cref="IrcMessage"/> is received with the command 353, RPL_NAMREPLY.
        /// Lists all clients that haved joined a channel.
        /// </summary>
        public virtual event EventHandler<NamReplyEventArgs>        OnNamReply;

        /// <summary>
        /// Raised when an <see cref="IrcMessage"/> is received with the command 366, RPL_ENDOFNAMES.
        /// Signifies the end of receiving a list of channel names that have joined a channel.
        /// </summary>
        public virtual event EventHandler<EndOfNamesEventArgs>      OnEndOfNames;

        /// <summary>
        /// Raised when an <see cref="IrcMessage"/> is received with the command 372, RPL_MOTD.
        /// Contains the Motd, message of the day, of the server.
        /// </summary>
        public virtual event EventHandler<MotdEventArgs>            OnMotd;

        /// <summary>
        /// Raised when an <see cref="IrcMessage"/> is received with the command 375, RPL_MOTDSTART.
        /// Signifies the start of the Motd, message of the day.
        /// </summary>
        public virtual event EventHandler<NumericReplyEventArgs>    OnMotdStart;

        /// <summary>
        /// Raised when an <see cref="IrcMessage"/> is received with the command 376, RPL_ENDOFMOTD.
        /// Signifies the end of the Motd, message of the day.
        /// </summary>
        public virtual event EventHandler<NumericReplyEventArgs>    OnEndOfMotd;

        /// <summary>
        /// Raised when an <see cref="IrcMessage"/> is received with the command 421, ERR_UNKNOWNCOMMAND.
        /// Signifies that the command trying to be used is not suported by the server
        /// </summary>
        public virtual event EventHandler<UnknownCommandEventArgs>  OnUnknownCommand;

        /// <summary>
        /// Raised when an <see cref="IrcMessage"/> is received with the command JOIN.
        /// Signifies that a user has joined a channel.
        /// </summary>
        public virtual event EventHandler<JoinEventArgs>            OnJoin;

        /// <summary>
        /// Raised when an <see cref="IrcMessage"/> is received with the command MODE.
        /// Signifies that a MODE change occured to a client's channel.
        /// </summary>
        public virtual event EventHandler<ChannelModeEventArgs>     OnChannelMode;

        /// <summary>
        /// <para>
        /// Raised when an <see cref="IrcMessage"/> is received with the command MODE.
        /// Signifies that a user gained or lost operator (moderator) status in a channel.
        /// </para>
        /// <para>Requires /membership.</para>
        /// </summary>
        public event EventHandler<ChannelOperatorEventArgs>         OnChannelOperator;

        /// <summary>
        /// Raised when an <see cref="IrcMessage"/> is received with the command MODE.
        /// Signifies that a MODE change occured to a specific user.
        /// </summary>
        public virtual event EventHandler<UserModeEventArgs>        OnUserMode;

        /// <summary>
        /// Raised when an <see cref="IrcMessage"/> is received with the command PART.
        /// Signifies that a user has left a channel.
        /// </summary>
        public virtual event EventHandler<PartEventArgs>            OnPart;

        /// <summary>
        /// Raised when an <see cref="IrcMessage"/> is received with the command PING.
        /// Sent by the server to test to see if a connected client is still active.
        /// The client should then respong with the appropriate PONG message as soon as possible for the connectin to remain active.
        /// </summary>
        public virtual event EventHandler<IrcMessageEventArgs>      OnPing;

        /// <summary>
        /// Raised when an <see cref="IrcMessage"/> is received with the command PRIVMSG.
        /// </summary>
        public virtual event EventHandler<PrivmsgEventArgs>         OnPrivmsg;

        /// <summary>
        /// Raised when the socket or stream encounters an error.</para>
        /// It is strongly recommended to reconnect to the IRC server if a network error is encountered.
        /// </summary>
        public virtual event EventHandler<ErrorEventArgs>           OnNetworkError;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE in a stream chat.</para>
        /// <para>These are general server notices sent specifically to the client, with a few excpections that are sent to all users in a channel's stream chat regardless of the source.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<NoticeEventArgs> OnNotice;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.AlreadyBanned"/> msg-id tag in a stream chat.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that a user was attempted to be banned/timed out in a channel's stream chat, but is already banned/timed out.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<AlreadyBannedEventArgs> OnAlreadyBanned;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.BadHostHosting"/> msg-id tag.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that a user was attempted to be hosted, but is already being hosted.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BadHostHostingEventArgs> OnBadHostHosting;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.BadHostRateExceeded"/> msg-id tag.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that more than the maximum number of users was attempted to be hosted in half an hour.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BadHostRateExceededEventArgs> OnBadHostRateExceeded;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.BadModMod"/> msg-id tag in a stream chat.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that a user was attempted to be modded in a channel's stream chat, but is already modded.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BadModModEventArgs> OnBadModMod;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.BadUnmodMod"/> msg-id tag in a stream chat.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that a user was attempted to be unmodded in a channel's stream chat, but is not a moderator.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BadUnmodModEventArgs> OnBadUnmodMod;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.BadUnbanNoBan"/> msg-id tag in a stream chat.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that a user was attempted to be unbanned/untimed out in a channel's stream chat, but is not banned/timed out.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BadUnbanNoBanEventArgs> OnBadUnbanNoBan;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.CmdsAvailable"/> msg-id tag in a stream chat.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Contains the chat commands that can be used in a channel's stream chat.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<CmdsAvailableEventArgs> OnCmdsAvailable;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.HostsRemaining"/> msg-id tag.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Contains how many hosts can be used until the value resets.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<HostsRemainingEventArgs> OnHostsRemaining;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.InvalidUser"/> msg-id tag in a stream chat.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that an invalid user nick was provided when trying to use a chat command in a channel's stream chat.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<InvalidUserEventArgs> OnInvalidUser;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.RoomMods"/> msg-id tag in a stream chat.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Contains a list of a channel's moderators.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<RoomModsEventArgs> OnRoomMods;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.UnbanSuccess"/> msg-id tag in a stream chat.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnNotice"/>.
        /// Signifies that a user was unbanned from a channel's stream chat.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<UnbanSuccessEventArgs> OnUnbanSuccess;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command NOTICE with the <see cref="NoticeType.UnsupportedChatRoomsCmd"/> msg-id tag.</para>
        /// <para>
        /// A secondary event raised after <see cref="OnChatRoomNotice"/>.
        /// Signifies that a chat command was attempted to be used in a channel's chat room, but is not supported in chat rooms.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<UnsupportedChatRoomCmdEventArgs> OnUnsupportedChatRoomCmd;

        #endregion

        #region Messange handling

        /// <summary>
        /// Sets all <see cref="IrcMessage"/> handlers back to the default methods.
        /// </summary>
        public virtual void
        ResetHandlers()
        {
            handlers    = new Dictionary<string, MessageHandler>();
            names       = new Dictionary<string, List<string>>();

            SetHandler("001", HandleWelcome);
            SetHandler("002", HandleYourHost);
            SetHandler("003", HandleCreated);
            SetHandler("004", HandleMyInfo);
            SetHandler("353", HandleNamReply);
            SetHandler("366", HandleEndOfNames);
            SetHandler("372", HandleMotd);
            SetHandler("375", HandleMotdStart);
            SetHandler("376", HandleEndOfMotd);
            SetHandler("421", HandleUnknownCommand);

            SetHandler("JOIN", HandleJoin);
            SetHandler("PART", HandlePart);
            SetHandler("MODE", HandleMode);
            SetHandler("PING", HandlePing);
            SetHandler("PRIVMSG", HandlePrivmsg);
            SetHandler("NOTICE", HandleNotice);
        }

        /// <summary>
        /// Sets an IRC message handler.
        /// </summary>
        /// <param name="command">The irc command.</param>
        /// <param name="handler">The message handler.</param>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="command"/> is null, empty, or whitespace.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="handler"/> is null.</exception>
        public void
        SetHandler(string command, MessageHandler handler)
        {
            ExceptionUtil.ThrowIfInvalid(command, nameof(command));
            ExceptionUtil.ThrowIfNull(handler, nameof(handler));

            if (handlers.IsNull())
            {
                return;
            }

            handlers[command] = handler;
        }

        /// <summary>
        /// Removes an IRC message handler.
        /// </summary>
        /// <param name="command">The irc command.</param>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="command"/> is null, empty, or whitespace.</exception>
        public void
        RemoveHandler(string command)
        {
            ExceptionUtil.ThrowIfInvalid(command, nameof(command));

            if (handlers.IsNull())
            {
                return;
            }

            if (!handlers.ContainsKey(command))
            {
                return;
            }

            handlers.Remove(command);
        }

        /// <summary>
        /// Executes an IRC message handler 
        /// </summary>
        /// <param name="message"></param>
        private void
        RunHandler(in IrcMessage message)
        {
            if (handlers.IsNull())
            {
                return;
            }

            if (!handlers.ContainsKey(message.command))
            {
                return;
            }

            handlers[message.command](message);
        }

        #endregion

        #region Numeric handlers

        /// <summary>
        /// Handles the IRC message with the command 001, RPL_WELCOME.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        protected virtual void
        HandleWelcome(in IrcMessage message)
        {
            SetState(ClientState.Connected);

            OnConnected.Raise(this, new NumericReplyEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command 002, RPL_YOURHOST.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        protected virtual void
        HandleYourHost(in IrcMessage message)
        {
            OnYourHost.Raise(this, new NumericReplyEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command 003, RPL_CREATED.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        protected virtual void
        HandleCreated(in IrcMessage message)
        {
            OnCreated.Raise(this, new NumericReplyEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command 004, RPL_MYINFO.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        protected virtual void
        HandleMyInfo(in IrcMessage message)
        {
            OnMyInfo.Raise(this, new NumericReplyEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command 353, RPL_NAMREPLY.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        protected virtual void
        HandleNamReply(in IrcMessage message)
        {
            NamReplyEventArgs args = new NamReplyEventArgs(message);

            if (!names.ContainsKey(args.channel))
            {
                names[args.channel] = new List<string>();
            }

            if (args.names.IsValid())
            {
                names[args.channel].AddRange(args.names);
            }

            OnNamReply.Raise(this, args);
        }

        /// <summary>
        /// Handles the IRC message with the command 366, RPL_ENDOFNAMES.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        protected virtual void
        HandleEndOfNames(in IrcMessage message)
        {
            EndOfNamesEventArgs args = new EndOfNamesEventArgs(message, names);
            if (names.ContainsKey(args.channel))
            {
                names.Remove(args.channel);
            }

            OnEndOfNames.Raise(this, args);
        }

        /// <summary>
        /// Handles the IRC message with the command 372, RPL_MOTD.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        protected virtual void
        HandleMotd(in IrcMessage message)
        {
            OnMotd.Raise(this, new MotdEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command 375, RPL_MOTDSTART.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        protected virtual void
        HandleMotdStart(in IrcMessage message)
        {
            OnMotdStart.Raise(this, new NumericReplyEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command 376, RPL_ENDOFMOTD.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        protected virtual void
        HandleEndOfMotd(in IrcMessage message)
        {
            OnEndOfMotd.Raise(this, new NumericReplyEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command 421, ERR_UNKNOWNCOMMAND.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        protected virtual void
        HandleUnknownCommand(in IrcMessage message)
        {
            OnUnknownCommand.Raise(this, new UnknownCommandEventArgs(message));
        }

        #endregion

        #region Other handlers        

        /// <summary>
        /// Handles the IRC message with the command JOIN.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        protected virtual void
        HandleJoin(in IrcMessage message)
        {
            OnJoin.Raise(this, new JoinEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command MODE.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        protected virtual void
        HandleMode(in IrcMessage message)
        {
            if (!message.parameters.IsValid() || !message.parameters[0].IsValid())
            {
                return;
            }

            if(message.parameters[0][0] == '#' || message.parameters[0][0] == '&')
            {
                ChannelModeEventArgs channel_mode_args = new ChannelModeEventArgs(message);
                OnChannelMode.Raise(this, channel_mode_args);

                if (channel_mode_args.mode == 'o')
                {
                    OnChannelOperator.Raise(this, new ChannelOperatorEventArgs(channel_mode_args));
                }
            }
            else
            {
                OnUserMode.Raise(this, new UserModeEventArgs(message));
            }
        }

        /// <summary> 
        /// Handles the IRC message with the command PART.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        protected virtual void
        HandlePart(in IrcMessage message)
        {
            OnPart.Raise(this, new PartEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command PING.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        protected virtual void
        HandlePing(in IrcMessage message)
        {
            if (auto_pong)
            {
                Pong(message.trailing);
            }

            OnPing.Raise(this, new IrcMessageEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command PRIVMSG.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        protected virtual void
        HandlePrivmsg(in IrcMessage message)
        {
            OnPrivmsg.Raise(this, new PrivmsgEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command NOTICE.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        private void
        HandleNotice(in IrcMessage message)
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

                case NoticeType.UnsupportedChatRoomsCmd:
                    {
                        OnUnsupportedChatRoomCmd.Raise(this, new UnsupportedChatRoomCmdEventArgs(args));
                    }
                    break;
            }
        }

        #endregion        
    }
}