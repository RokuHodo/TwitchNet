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
        /// The partial or full list of channel names that have joined an IRC channel.
        /// Used with commands '353' and '366'.
        /// </summary>
        protected            Dictionary<string, List<string>>        names;

        /// <summary>
        /// The message handlers.
        /// </summary>
        protected            Dictionary<string, MessageHandler>      handlers;

        #endregion

        #region Events        

        /// <summary>
        /// <para>Raised when a message is sent by the client to the server.</para>
        /// </summary>
        public virtual event EventHandler<DataEventArgs>            OnDataSent;

        /// <summary>
        /// <para>
        /// Raised when a complete message is receieved from the server.
        /// The message is unparsed.
        /// </para>
        /// </summary>
        public virtual event EventHandler<DataEventArgs>            OnDataReceived;

        /// <summary>
        /// <para>Raised when a message is received from the server.</para>
        /// </summary>
        public virtual event EventHandler<IrcMessageEventArgs>      OnIrcMessageReceived;

        /// <summary>
        /// <para>Raised when an the internal socket establishes a connection to the IRC server.
        /// Signifies that the client is ready to send and receive messages.
        /// </para>
        /// <para>
        /// This occurs before the client logs into the IRC server and before <see cref="OnConnected"/> is raised.
        /// At this point, the state is set to <see cref="ClientState.Connecting"/> since the client is not yet logged into the IRC.
        /// </para>
        /// </summary>
        public virtual event EventHandler<EventArgs>                OnSocketConnected;

        /// <summary>
        /// <para>
        /// Raised when a message is received with the command 001, RPL_WELCOME.
        /// Signifies that the <see cref="IrcClient"/> has successfully registered and connected to the IRC server.
        /// </para>
        /// <para>At this point, the state is set to <see cref="ClientState.Connected"/>.</para>
        /// </summary>
        public virtual event EventHandler<NumericReplyEventArgs>    OnConnected;

        /// <summary>
        /// <para>
        /// Raised when an the internal socket disconnects from the IRC server.
        /// Signifies that the client can no longer send and receive messages.
        /// </para>
        /// <para>
        /// This occurs before the reader finishes processing the current data, before all currently raised events have finihshed, and before <see cref="OnDisconnected"/> is raised.
        /// At this point, the state is set to <see cref="ClientState.Disconnecting"/>, the entire stream buffer has been read to the end, and managed resources still need to be disposed.
        /// </para>
        /// </summary>
        public virtual event EventHandler<EventArgs>                OnSocketDisconnected;

        /// <summary>
        /// <para>
        /// Raised when the <see cref="IrcClient"/> disconnects from the IRC server.
        /// This is only raised when a client has chosen to manually disconnect.
        /// </para>
        /// <para>At this point, the state is set to <see cref="ClientState.Disconnected"/>.</para>
        /// </summary>
        public virtual event EventHandler<EventArgs>                OnDisconnected;

        /// <summary>
        /// Raised when the client is disposed.
        /// </summary>
        public virtual event EventHandler<EventArgs>                OnDisposed;

        /// <summary>
        /// Raised when the socket or stream encounters an error.
        /// It is strongly recommended to reconnect to the IRC server if a network error is encountered.
        /// </summary>
        public virtual event EventHandler<ErrorEventArgs>           OnNetworkError;

        /// <summary>
        /// <para>
        /// Raised when a message is received with the command 002, RPL_YOURHOST.
        /// Gives information about the IRC server the client has connected too.
        /// </para>
        /// <para>This is part of the post-registration process.</para>
        /// </summary>
        public virtual event EventHandler<NumericReplyEventArgs>    OnYourHost;

        /// <summary>
        /// <para>
        /// Raised when a message is received with the command 003, RPL_CREATED.
        /// Gives information about when the IRC server was started or created.
        /// </para>        
        /// <para>This is part of the post-registration process.</para>
        /// </summary>
        public virtual event EventHandler<NumericReplyEventArgs>    OnCreated;

        /// <summary>
        /// Raised when a message is received with the command 004, RPL_MYINFO.
        /// Gives information about the available modes that are available to use.
        /// </summary>
        public virtual event EventHandler<NumericReplyEventArgs>    OnMyInfo;

        /// <summary>
        /// Raised when a message is received with the command 353, RPL_NAMREPLY.
        /// Lists all clients that haved joined a channel.
        /// </summary>
        public virtual event EventHandler<NamReplyEventArgs>        OnNamReply;

        /// <summary>
        /// Raised when a mesage is received with the command 366, RPL_ENDOFNAMES.
        /// Signifies the end of receiving a list of channel names that have joined a channel.
        /// </summary>
        public virtual event EventHandler<EndOfNamesEventArgs>      OnEndOfNames;

        /// <summary>
        /// Raised when a message is received with the command 372, RPL_MOTD.
        /// Contains the Motd, message of the day, of the server.
        /// </summary>
        public virtual event EventHandler<MotdEventArgs>            OnMotd;

        /// <summary>
        /// Raised when a message is received with the command 375, RPL_MOTDSTART.
        /// Signifies the start of the Motd, message of the day.
        /// </summary>
        public virtual event EventHandler<NumericReplyEventArgs>    OnMotdStart;

        /// <summary>
        /// Raised when a message is received with the command 376, RPL_ENDOFMOTD.
        /// Signifies the end of the Motd, message of the day.
        /// </summary>
        public virtual event EventHandler<NumericReplyEventArgs>    OnEndOfMotd;

        /// <summary>
        /// Raised when a message is received with the command 421, ERR_UNKNOWNCOMMAND.
        /// Signifies that the command trying to be used is not suported by the server
        /// </summary>
        public virtual event EventHandler<UnknownCommandEventArgs>  OnUnknownCommand;

        /// <summary>
        /// Raised when a message is received with the command JOIN.
        /// Signifies that a user has joined a channel.
        /// </summary>
        public virtual event EventHandler<JoinEventArgs>            OnJoin;

        /// <summary>
        /// Raised when a message is received with the command MODE.
        /// Signifies that a MODE change occured to a client's channel.
        /// </summary>
        public virtual event EventHandler<ChannelModeEventArgs>     OnChannelMode;

        /// <summary>
        /// <para>
        /// Raised when a message is received with the command MODE.
        /// Signifies that a user gained or lost operator (moderator) status in a channel.
        /// </para>
        /// <para>Requires /membership.</para>
        /// </summary>
        public event EventHandler<ChannelOperatorEventArgs>         OnChannelOperator;

        /// <summary>
        /// Raised when a message is received with the command MODE.
        /// Signifies that a MODE change occured to a specific user.
        /// </summary>
        public virtual event EventHandler<UserModeEventArgs>        OnUserMode;

        /// <summary>
        /// Raised when a message is received with the command PART.
        /// Signifies that a user has left a channel.
        /// </summary>
        public virtual event EventHandler<PartEventArgs>            OnPart;

        /// <summary>
        /// <para>
        /// Raised when a message is received with the command PING.
        /// Sent by the server to test to see if a connected client is still active.
        /// </para>
        /// <para>The client should then respong with the appropriate PONG message as soon as possible for the connectin to remain active.</para>
        /// </summary>
        public virtual event EventHandler<IrcMessageEventArgs>      OnPing;

        /// <summary>
        /// <para>Raised when a message is received with the command PRIVMSG.</para>
        /// <para>These are messages sent by users.</para>
        /// </summary>
        public virtual event EventHandler<PrivmsgEventArgs>         OnPrivmsg;        

        /// <summary>
        /// <para>Raised when a message is received with the command NOTICE in a stream chat.</para>
        /// <para>
        /// These are general server notices sent specifically to the client.
        /// There are a few excpections where notices are sent to all users that have joined the IRC channel, regardless of the notice source.</para>
        /// <para>
        /// Requires /commands.
        /// Supplementary tags can be added to the message by requesting /tags.
        /// </para>
        /// </summary>
        public event EventHandler<NoticeEventArgs>                  OnNotice;

        /*
        /// <summary>
        /// <para>
        /// A secondary event to <see cref="OnNotice"/>.
        /// Signifies that a user was attempted and failed to be banned from an IRC channel.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BanFailedEventArgs> OnNotice_BanFailed;

        /// <summary>
        /// <para>
        /// A secondary event to <see cref="OnNotice"/>.
        /// Signifies that a user was banned in an IRC channel.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BanSuccessEventArgs> OnNotice_BanSuccess;

        /// <summary>
        /// <para>
        /// A secondary event to <see cref="OnNotice"/>.
        /// Signifies that a user was attempted and failed to be unbanned from an IRC channel.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<UnbanFailedEventArgs> OnNotice_UnbanFailed;

        /// <summary>
        /// <para>
        /// A secondary event to <see cref="OnNotice"/>.
        /// Signifies that a user was unbanned from an IRC channel.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<UnbanSuccessEventArgs> OnNotice_UnbanSuccess;

        /// <summary>
        /// <para>
        /// A secondary event to <see cref="OnNotice"/>.
        /// Signifies that the /mods command was used and contains the list of moderators in an IRC channel.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<ModsEventArgs> OnNotice_Mods;

        /// <summary>
        /// <para>
        /// A secondary event to <see cref="OnNotice"/>.
        /// Raised when a message is received with the command NOTICE and the msg-id tag is set to <see cref="NoticeType.BadHostHosting"/>.
        /// Signifies that a user was attempted to be hosted, but is already being hosted.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BadHostHostingEventArgs> OnBadHostHosting;

        /// <summary>
        /// <para>
        /// A secondary event to <see cref="OnNotice"/>.
        /// Raised when a message is received with the command NOTICE and the msg-id tag is set to <see cref="NoticeType.BadHostRateExceeded"/>.
        /// Signifies that a user exceeded the macimum number of hosts they are allowed to perform for a given time frame.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BadHostRateExceededEventArgs> OnBadHostRateExceeded;

        /// <summary>
        /// <para>
        /// A secondary event to <see cref="OnNotice"/>.
        /// Raised when a message is received with the command NOTICE and the msg-id tag is set to <see cref="NoticeType.BadModMod"/>.
        /// Signifies that a user was attempted to be modded in an IRC channel but is already modded.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BadModModEventArgs> OnBadModMod;

        /// <summary>
        /// <para>
        /// A secondary event to <see cref="OnNotice"/>.
        /// Raised when a message is received with the command NOTICE and the msg-id tag is set to <see cref="NoticeType.BadUnmodMod"/>.
        /// Signifies that a user was attempted to be unmodded in an IRC channel but is not a moderator.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<BadUnmodModEventArgs> OnBadUnmodMod;

        /// <summary>
        /// <para>
        /// A secondary event to <see cref="OnNotice"/>.
        /// Raised when a message is received with the command NOTICE and the msg-id tag is set to <see cref="NoticeType.CmdsAvailable"/>.
        /// Signifies that the /help command was used and contains the commands that can be used in an IRC channel.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<CmdsAvailableEventArgs> OnCmdsAvailable;

        /// <summary>
        /// <para>
        /// A secondary event to <see cref="OnNotice"/>.
        /// Raised when a message is received with the command NOTICE and the msg-id tag is set to <see cref="NoticeType.HostsRemaining"/>.
        /// Contains how many hosts can be used until the value resets.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<HostsRemainingEventArgs> OnHostsRemaining;

        /// <summary>
        /// <para>
        /// A secondary event to <see cref="OnNotice"/>.
        /// Raised when a message is received with the command NOTICE and the msg-id tag is set to <see cref="NoticeType.InvalidUser"/>.
        /// Signifies that an invalid user nick was provided when trying to use a chat command in an IRC channel.
        /// </para>
        /// <para>Requires /commands and /tags.</para>
        /// </summary>
        public event EventHandler<InvalidUserEventArgs> OnInvalidUser;
        */
        #endregion

        #region Handlers

        /// <summary>
        /// Sets all <see cref="IrcMessage"/> handlers back to the default methods.
        /// </summary>
        public virtual void
        ResetHandlers()
        {
            handlers    = new Dictionary<string, MessageHandler>();
            names       = new Dictionary<string, List<string>>();

            SetHandler("001",       Handle_001_Welcome);
            SetHandler("002",       Handle_002_YourHost);
            SetHandler("003",       Handle_003_Created);
            SetHandler("004",       Handle_004_MyInfo);
            SetHandler("353",       Handle_353_NamReply);
            SetHandler("366",       Handle_366_EndOfNames);
            SetHandler("372",       Handle_372_Motd);
            SetHandler("375",       Handle_375_MotdStart);
            SetHandler("376",       Handle_376_EndOfMotd);
            SetHandler("421",       Handle_421_UnknownCommand);

            SetHandler("JOIN",      Handle_Join);
            SetHandler("PART",      Handle_Part);
            SetHandler("MODE",      Handle_Mode);
            SetHandler("PING",      Handle_Ping);
            SetHandler("PRIVMSG",   Handle_Privmsg);
            SetHandler("NOTICE",    Handle_Notice);
        }

        /// <summary>
        /// Sets an IRC message handler.
        /// </summary>
        /// <param name="command">The IRC command.</param>
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
        /// <param name="command">The IRC command.</param>
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

        /// <summary>
        /// Handles the IRC message with the command 001, RPL_WELCOME.
        /// </summary>
        /// <param name="message">The IRC message to be handled.</param>
        protected virtual void
        Handle_001_Welcome(in IrcMessage message)
        {
            SetState(ClientState.Connected);

            OnConnected.Raise(this, new NumericReplyEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command 002, RPL_YOURHOST.
        /// </summary>
        /// <param name="message">The IRC message to be handled.</param>
        protected virtual void
        Handle_002_YourHost(in IrcMessage message)
        {
            OnYourHost.Raise(this, new NumericReplyEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command 003, RPL_CREATED.
        /// </summary>
        /// <param name="message">The IRC message to be handled.</param>
        protected virtual void
        Handle_003_Created(in IrcMessage message)
        {
            OnCreated.Raise(this, new NumericReplyEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command 004, RPL_MYINFO.
        /// </summary>
        /// <param name="message">The IRC message to be handled.</param>
        protected virtual void
        Handle_004_MyInfo(in IrcMessage message)
        {
            OnMyInfo.Raise(this, new NumericReplyEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command 353, RPL_NAMREPLY.
        /// </summary>
        /// <param name="message">The IRC message to be handled.</param>
        protected virtual void
        Handle_353_NamReply(in IrcMessage message)
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
        /// <param name="message">The IRC message to be handled.</param>
        protected virtual void
        Handle_366_EndOfNames(in IrcMessage message)
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
        /// <param name="message">The IRC message to be handled.</param>
        protected virtual void
        Handle_372_Motd(in IrcMessage message)
        {
            OnMotd.Raise(this, new MotdEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command 375, RPL_MOTDSTART.
        /// </summary>
        /// <param name="message">The IRC message to be handled.</param>
        protected virtual void
        Handle_375_MotdStart(in IrcMessage message)
        {
            OnMotdStart.Raise(this, new NumericReplyEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command 376, RPL_ENDOFMOTD.
        /// </summary>
        /// <param name="message">The IRC message to be handled.</param>
        protected virtual void
        Handle_376_EndOfMotd(in IrcMessage message)
        {
            OnEndOfMotd.Raise(this, new NumericReplyEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command 421, ERR_UNKNOWNCOMMAND.
        /// </summary>
        /// <param name="message">The IRC message to be handled.</param>
        protected virtual void
        Handle_421_UnknownCommand(in IrcMessage message)
        {
            OnUnknownCommand.Raise(this, new UnknownCommandEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command JOIN.
        /// </summary>
        /// <param name="message">The IRC message to be handled.</param>
        protected virtual void
        Handle_Join(in IrcMessage message)
        {
            OnJoin.Raise(this, new JoinEventArgs(message));
        }

        /// <summary> 
        /// Handles the IRC message with the command PART.
        /// </summary>
        /// <param name="message">The IRC message to be handled.</param>
        protected virtual void
        Handle_Part(in IrcMessage message)
        {
            OnPart.Raise(this, new PartEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command MODE.
        /// </summary>
        /// <param name="message">The IRC message to be handled.</param>
        protected virtual void
        Handle_Mode(in IrcMessage message)
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
        /// Handles the IRC message with the command PING.
        /// </summary>
        /// <param name="message">The IRC message to be handled.</param>
        protected virtual void
        Handle_Ping(in IrcMessage message)
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
        /// <param name="message">The IRC message to be handled.</param>
        protected virtual void
        Handle_Privmsg(in IrcMessage message)
        {
            OnPrivmsg.Raise(this, new PrivmsgEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command NOTICE.
        /// </summary>
        /// <param name="message">The IRC message to be handled.</param>
        private void
        Handle_Notice(in IrcMessage message)
        {
            NoticeEventArgs args = new NoticeEventArgs(message);
            OnNotice.Raise(this, args);
                
            /*
            TODO: Reimplement OnNotice sub-events in the future?

            This is very much a 'nice'-to-have rather than a very loose 'need'.
            This would also bloat the IRC library even more than it already is.

            switch (args.tags.msg_id)
            {
                // Updated 
                case NoticeType.AlreadyBanned:
                case NoticeType.BadBanAdmin:
                case NoticeType.BadBanAnon:
                case NoticeType.BadBanBroadcaster:
                case NoticeType.BadBanGlobalMod:
                case NoticeType.BadBanMod:
                case NoticeType.BadBanSelf:
                case NoticeType.BadBanStaff:                OnNotice_BanFailed.Raise(this, new BanFailedEventArgs(args));                       break;
                case NoticeType.BanSuccess:                 OnNotice_BanSuccess.Raise(this, new BanSuccessEventArgs(args));                     break;

                case NoticeType.BadUnbanNoBan:              OnNotice_UnbanFailed.Raise(this, new UnbanFailedEventArgs(args));                   break;
                case NoticeType.UnbanSuccess:               OnNotice_UnbanSuccess.Raise(this, new UnbanSuccessEventArgs(args));                 break;

                case NoticeType.NoMods:
                case NoticeType.RoomMods:                   OnNotice_Mods.Raise(this, new ModsEventArgs(args));                                 break;

                // To be updated
                case NoticeType.BadHostHosting:             OnBadHostHosting.Raise(this, new BadHostHostingEventArgs(args));                    break;
                case NoticeType.BadHostRateExceeded:        OnBadHostRateExceeded.Raise(this, new BadHostRateExceededEventArgs(args));          break;
                case NoticeType.HostsRemaining:             OnHostsRemaining.Raise(this, new HostsRemainingEventArgs(args));                    break;
                case NoticeType.BadModMod:                  OnBadModMod.Raise(this, new BadModModEventArgs(args));                              break;
                case NoticeType.BadUnmodMod:                OnBadUnmodMod.Raise(this, new BadUnmodModEventArgs(args));                          break;
                case NoticeType.CmdsAvailable:              OnCmdsAvailable.Raise(this, new CmdsAvailableEventArgs(args));                      break;
                case NoticeType.InvalidUser:                OnInvalidUser.Raise(this, new InvalidUserEventArgs(args));                          break;
            }
            */
        }

        #endregion        
    }
}