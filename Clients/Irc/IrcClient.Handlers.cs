// standard namespaces
using System;
using System.Collections.Generic;
using System.IO;

// project namespaces
using TwitchNet.Debug;
using TwitchNet.Enums.Clients;
using TwitchNet.Events.Clients.Irc;
using TwitchNet.Extensions;
using TwitchNet.Models.Clients.Irc;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc
{
    public partial class
    IrcClient
    {
        #region Delegates

        /// <summary>
        /// The method signature of the IRC message handler.
        /// </summary>
        /// <param name="message_irc"></param>
        public delegate void MessageHandler(IrcMessage message_irc);

        #endregion

        #region Fields

        /// <summary>
        /// The partial or full list of channel names that have joined a channel.
        /// Used with commands '353' and '366'.
        /// </summary>
        private Dictionary<string, List<string>>            names;

        /// <summary>
        /// The message handlers.
        /// </summary>
        private Dictionary<string, MessageHandler>          handlers;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received.</para>
        /// </summary>
        public event EventHandler<IrcMessageEventArgs>      OnIrcMessage;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command 001, RPL_WELCOME.</para>
        /// <para>Signifies that the <see cref="IrcClient"/> has successfully registered and connected to the IRC server.</para>
        /// </summary>
        public event EventHandler<NumericReplyEventArgs>    OnConnected;

        /// <summary>
        /// <para>Raised when the <see cref="IrcClient"/> disconnects from the IRC server.</para>
        /// <para>
        /// This is only raised when <see cref="Disconnect"/> or <see cref="DisconnectAsync"/> is called by the client.
        /// This is not raised when the connection is terminated by the server because no message is sent signifying the end of the connection. 
        /// </para>
        /// </summary>
        public event EventHandler<EventArgs>                OnDisconnected;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command 002, RPL_YOURHOST.</para>
        /// <para>
        /// Gives information about the IRC server the <see cref="IrcClient"/> has connected too.
        /// This is part of the post-registration process.
        /// </para>
        /// </summary>
        public event EventHandler<NumericReplyEventArgs>    OnYourHost;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command 003, RPL_CREATED.</para>
        /// <para>
        /// Gives information about when the IRC server was started or created.
        /// This is part of the post-registration process.
        /// </para>
        /// </summary>
        public event EventHandler<NumericReplyEventArgs>    OnCreated;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command 004, RPL_MYINFO.</para>
        /// <para>
        /// Gives information about the available modes that are available to use.
        /// This is part of the post-registration process.
        /// </para>
        /// </summary>
        public event EventHandler<NumericReplyEventArgs>    OnMyInfo;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command 353, RPL_NAMREPLY.</para>
        /// <para>Lists all clients that haved joined a channel.</para>
        /// </summary>
        public event EventHandler<NamReplyEventArgs>        OnNamReply;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command 366, RPL_ENDOFNAMES.</para>
        /// <para>Signifies the end of receiving a list of channel names that have joined a channel.</para>
        /// </summary>
        public event EventHandler<EndOfNamesEventArgs>      OnEndOfNames;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command 372, RPL_MOTD.</para>
        /// <para>Contains the Motd, message of the day, of the server.</para>
        /// </summary>
        public event EventHandler<MotdEventArgs>            OnMotd;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command 375, RPL_MOTDSTART.</para>
        /// <para>Signifies the start of the Motd, message of the day.</para>
        /// </summary>
        public event EventHandler<NumericReplyEventArgs>    OnMotdStart;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command 376, RPL_ENDOFMOTD.</para>
        /// <para>Signifies the end of the Motd, message of the day.</para>
        /// </summary>
        public event EventHandler<NumericReplyEventArgs>    OnEndOfMotd;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command 421, ERR_UNKNOWNCOMMAND.</para>
        /// <para>Signifies that the command trying to be used is not suported by the server.</para>
        /// </summary>
        public event EventHandler<UnknownCommandEventArgs>  OnUnknownCommand;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command JOIN.</para>
        /// <para>Signifies that a user has joined a channel.</para>
        /// </summary>
        public event EventHandler<JoinEventArgs>            OnJoin;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command PART.</para>
        /// <para>Signifies that a user has left a channel.</para>
        /// </summary>
        public event EventHandler<PartEventArgs>            OnPart;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command PING.</para>
        /// <para>
        /// Sent by the server to test to see if a connected client is still active.
        /// The client should then respong with the appropriate PONG message as soon as possible for the connectin to not be terminated.
        /// </para>
        /// </summary>
        public event EventHandler<IrcMessageEventArgs>      OnPing;

        /// <summary>
        /// <para>Raised when an <see cref="IrcMessage"/> is received with the command PRIVMSG.</para>
        /// <para>Sent when a user sends a message in a channel.</para>
        /// </summary>
        public event EventHandler<PrivmsgEventArgs>         OnPrivmsg;

        /// <summary>
        /// <para>Raised when the socket or stream encounters an error.</para>
        /// <para>It is strongly recommended to reconnect to the IRC server if a network error is encountered.</para>
        /// </summary>
        public event EventHandler<ErrorEventArgs>           OnNetworkError;

        #endregion

        #region Messange handling

        public virtual void
        DefaultHandlers()
        {
            handlers = new Dictionary<string, MessageHandler>();
            names = new Dictionary<string, List<string>>();

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
            SetHandler("PING", HandlePing);
            SetHandler("PRIVMSG", HandlePrivmsg);
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
        RunHandler(IrcMessage message)
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
        private void
        HandleWelcome(IrcMessage message)
        {
            SetState(ClientState.Connected);

            OnConnected.Raise(this, new NumericReplyEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command 002, RPL_YOURHOST.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        private void
        HandleYourHost(IrcMessage message)
        {
            OnYourHost.Raise(this, new NumericReplyEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command 003, RPL_CREATED.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        private void
        HandleCreated(IrcMessage message)
        {
            OnCreated.Raise(this, new NumericReplyEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command 004, RPL_MYINFO.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        private void
        HandleMyInfo(IrcMessage message)
        {
            OnMyInfo.Raise(this, new NumericReplyEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command 353, RPL_NAMREPLY.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        private void
        HandleNamReply(IrcMessage message)
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
        private void
        HandleEndOfNames(IrcMessage message)
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
        private void
        HandleMotd(IrcMessage message)
        {
            OnMotd.Raise(this, new MotdEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command 375, RPL_MOTDSTART.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        private void
        HandleMotdStart(IrcMessage message)
        {
            OnMotdStart.Raise(this, new NumericReplyEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command 376, RPL_ENDOFMOTD.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        private void
        HandleEndOfMotd(IrcMessage message)
        {
            OnEndOfMotd.Raise(this, new NumericReplyEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command 421, ERR_UNKNOWNCOMMAND.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        private void
        HandleUnknownCommand(IrcMessage message)
        {
            OnUnknownCommand.Raise(this, new UnknownCommandEventArgs(message));
        }

        #endregion

        #region Other handlers        

        /// <summary>
        /// Handles the IRC message with the command JOIN.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        private void
        HandleJoin(IrcMessage message)
        {
            OnJoin.Raise(this, new JoinEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command PART.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        private void
        HandlePart(IrcMessage message)
        {
            OnPart.Raise(this, new PartEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command PING.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        private void
        HandlePing(IrcMessage message)
        {
            Pong(message.trailing);

            OnPing.Raise(this, new IrcMessageEventArgs(message));
        }

        /// <summary>
        /// Handles the IRC message with the command PRIVMSG.
        /// </summary>
        /// <param name="message">The irc message to be handled.</param>
        private void
        HandlePrivmsg(IrcMessage message)
        {
            Log.PrintLine(message.raw);

            OnPrivmsg.Raise(this, new PrivmsgEventArgs(message));
        }

        #endregion        
    }
}