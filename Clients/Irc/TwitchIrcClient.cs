// standard namespaces
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Rest.Helix;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc
{
    public partial class
    TwitchIrcClient : IrcClient
    {
        #region  Properties

        /// <summary>
        /// Determines whether or not to automatically retrieve the Twitch user information after the socket connects to the server.
        /// </summary>
        public bool request_user_info   { get; set; }

        /// <summary>
        /// Determines whether or not to automatically request /commands after the socket connects to the server.
        /// </summary>
        public bool request_commands    { get; set; }

        /// <summary>
        /// Determines whether or not to automatically request /membership after the socket connects to the server.
        /// </summary>
        public bool request_membership  { get; set; }

        /// <summary>
        /// Determines whether or not to automatically request /tags after the socket connects to the server.
        /// </summary>
        public bool request_tags        { get; set; }

        /// <summary>
        /// The client's Twitch information retrieved from the Helix API via the client's pass.
        /// This is only retrieved if <see cref="request_user_info"/> is set to true.
        /// </summary>
        public User twitch_user         { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="TwitchIrcClient"/> class.
        /// </summary>
        /// <param name="port">The port number of the remote host.</param>
        /// <param name="irc_user">The IRC user's credentials.</param>
        public TwitchIrcClient(ushort port, IrcUser irc_user) : base("irc.chat.twitch.tv", port, irc_user)
        {
            OnSocketConnected   += new EventHandler<EventArgs>(Callback_OnSocketConnected);
        }

        #endregion

        #region Settings methods

        /// <summary>
        /// Sets all client settings to their default values.
        /// </summary>
        public override void
        ResetSettings()
        {
            base.ResetSettings();

            request_user_info   = false;
            request_commands    = false;
            request_membership  = false;
            request_tags        = false;
        }

        #endregion        

        #region IRC command wrappers

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
        /// Enables custom Twitch IRC commands.
        /// </summary>
        public void
        RequestCommands()
        {
            Send("CAP REQ :twitch.tv/commands");
        }

        /// <summary>
        /// Sends a direct chat message to another user.
        /// </summary>
        /// <param name="user_nick">The recipient's IRC nick.</param>
        /// <param name="format">
        /// The string to send without the CR-LF (\r\n), the CR-LF is automatically appended.
        /// This can be a normal string and does not need to include variable formats.
        /// </param>
        /// <param name="arguments">Optional format variable arugments.</param>
        /// <exception cref="ArgumentException">Thrown if the format is null, empty, or contains only whitespace.</exception>
        /// <exception cref="FormatException">Thrown if the nick is not between 2 and 24 characters long or contians any non-alpha-numeric characters.</exception>
        public void
        SendWhisper(string user_nick, string format, params string[] arguments)
        {
            ExceptionUtil.ThrowIfInvalid(user_nick, nameof(user_nick));
            ExceptionUtil.ThrowIfInvalid(format, nameof(format));

            SendPrivmsg("#jtv", "/w " + user_nick.ToLower() + " " + format, arguments);
        }

        #endregion

        #region Chat command wrappers

        // TODO: Add these chat commands.

        // /poll - requires access to a web page, no point in really adding this one
        // /endpoll

        // /block - does this still not work when done via IRC?
        // /unblock
        // /marker
        // /vip
        // /unvip
        // /vips
        // /vote

        /// <summary>
        /// Change the client's display name color.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <param name="color">The display name color.</param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public bool
        ChangeDisplayNameColor(string channel, DisplayNameColor color)
        {
            return SendChatCommand(channel, ChatCommand.Color, EnumUtil.GetName(color));
        }

        /// <summary>
        /// <para>Change the client's display name color.</para>
        /// <para>Requires Twitch Prime or Turbo to be used.</para>
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <param name="color">The display name color.</param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false if the color is empty.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public bool
        ChangeDisplayNameColor(string channel, Color color)
        {
            if (color.IsEmpty)
            {
                return false;
            }

            return SendChatCommand(channel, ChatCommand.Color, ColorTranslator.ToHtml(color));
        }

        /// <summary>
        /// <para>Change the client's display name color.</para>
        /// <para>Requires Twitch Prime or Turbo to be used.</para>
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <param name="html_color">The display name color in hex (HTML) format.</param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        /// <exception cref="FormatException">Thrown if the <paramref name="html_color"/> is not in hex (HTML) format.</exception>
        public bool
        ChangeDisplayNameColor(string channel, string html_color)
        {
            if (!html_color.IsValidHtmlColor())
            {
                throw new FormatException(nameof(html_color) + " is not a valid HTML color name");
            }

            return SendChatCommand(channel, ChatCommand.Color, html_color);
        }

        /// <summary>
        /// Reconnects to an IRC channel.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public bool
        ChatDisconnect(string channel)
        {
            return SendChatCommand(channel, ChatCommand.Disconnect);
        }

        /// <summary>
        /// <para>Prints a list of chat commands that can be used by the client in an IRC channel.</para>
        /// <para>The list can be retrieved by using <see cref="OnCmdsAvailable"/>.</para>
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public bool
        PrintAvailableCommands(string channel)
        {
            return SendChatCommand(channel, ChatCommand.Help);
        }

        /// <summary>
        /// Prints help for a chat command that can be used in an IRC channel, if any help exists.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <param name="command">The chat command to print help for.</param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public bool
        PrintHelp(string channel, ChatCommand command)
        {
            return SendChatCommand(channel, ChatCommand.Help, EnumUtil.GetName(command).TextAfter('/'));
        }

        /// <summary>
        /// Sends a private message in an IRC channel using the /me chat command.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <param name="format">
        /// The message to send.
        /// This can be a normal string and does not need to include variable formats.
        /// </param>
        /// <param name="arguments">Optional format variable arugments.</param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel or format  is null, empty, or contains only whitespace.</exception>        
        public bool
        SendPrivmsgMe(string channel, string format, params string[] arguments)
        {
            // Check format here since only sending /me is not allowed
            ExceptionUtil.ThrowIfInvalid(format, nameof(format));

            string trailing = EnumUtil.GetName(ChatCommand.Me) + ' ' + format;
            return SendPrivmsg(channel, trailing, arguments);
        }

        /// <summary>
        /// Grants a user operator (moderator) status.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <param name="user_nick">The user to grant operator status.</param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        /// <exception cref="FormatException">Thrown if the nick is not between 2 and 24 characters long or contians any non-alpha-numeric characters.</exception>
        public bool
        Mod(string channel, string user_nick)
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick);

            return SendChatCommand(channel, ChatCommand.Mod, user_nick);
        }

        /// <summary>
        /// Removes a user's operator (moderator) status.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <param name="user_nick">The user to grant operator status.</param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        /// <exception cref="FormatException">Thrown if the nick is not between 2 and 24 characters long or contians any non-alpha-numeric characters.</exception>
        public bool
        Unmod(string channel, string user_nick)
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick);

            return SendChatCommand(channel, ChatCommand.Unmod, user_nick);
        }

        /// <summary>
        /// <para>Prints a list of oeprators (moderators) in an IRC channel.</para>
        /// <para>The list can be retrieved by using <see cref="OnRoomMods"/>.</para>
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public bool
        PrintMods(string channel)
        {
            return SendChatCommand(channel, ChatCommand.Mods);
        }

        /// <summary>
        /// Bans a user.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <param name="user_nick">The user to ban.</param>
        /// <param name="reason">The optional reason for the ban.</param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        /// <exception cref="FormatException">Thrown if the nick is not between 2 and 24 characters long or contians any non-alpha-numeric characters.</exception>        
        public bool
        Ban(string channel, string user_nick, string reason = "")
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick);

            return SendChatCommand(channel, ChatCommand.Ban, user_nick, reason);
        }

        /// <summary>
        /// Unbans a user.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <param name="user_nick">The user to unban.</param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        /// <exception cref="FormatException">Thrown if the nick is not between 2 and 24 characters long or contians any non-alpha-numeric characters.</exception>
        public bool
        Unban(string channel, string user_nick)
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick);

            return SendChatCommand(channel, ChatCommand.Unban, user_nick);
        }

        /// <summary>
        /// Time out a user for 1 second.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <param name="user_nick">The user to purge.</param>
        /// <param name="reason">The optional reason for the purge.</param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        /// <exception cref="FormatException">Thrown if the nick is not between 2 and 24 characters long or contians any non-alpha-numeric characters.</exception>
        public bool
        Purge(string channel, string user_nick, string reason = "")
        {
            return Timeout(channel, user_nick, 1, reason);
        }

        /// <summary>
        /// Time out a user for 5 minutes.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        /// <param name="user_nick">The user to purge.</param>
        /// <param name="reason">The optional reason for the time out.</param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        /// <exception cref="FormatException">Thrown if the nick is not between 2 and 24 characters long or contians any non-alpha-numeric characters.</exception>
        public bool
        Timeout(string channel, string user_nick, string reason = "")
        {
            return Timeout(channel, user_nick, 600, reason);
        }

        /// <summary>
        /// Time out a user for a specified amount of time.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <param name="user_nick">The user to purge.</param>
        /// <param name="duration">
        /// <para>
        /// How long to time out the user for.
        /// Clamped between the minimum and the maximum values.
        /// </para>
        /// <para>
        /// Min: 1 second.
        /// Max: 1209600 seconds (2 weeks).
        /// </para>
        /// </param>
        /// <param name="reason">The optional reason for the time out.</param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        /// <exception cref="FormatException">Thrown if the nick is not between 2 and 24 characters long or contians any non-alpha-numeric characters.</exception>
        public bool
        Timeout(string channel, string user_nick, ref TimeSpan duration, string reason = "")
        {
            // Clamp the value up here to prevent a possible overflow exception with Convert.ToUint32()
            double length_seconds = duration.TotalSeconds.Clamp(1, 1209600);

            return Timeout(channel, user_nick, Convert.ToUInt32(length_seconds), reason);
        }

        /// <summary>
        /// Time out a user for a specified amount of time.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <param name="user_nick">The user to purge.</param>
        /// <param name="duration_seconds">
        /// <para>
        /// How long to time out the user for, in seconds.
        /// Clamped between the minimum and the maximum values.
        /// </para>
        /// <para>
        /// Min: 1 second.
        /// Max: 1,209,600 seconds (2 weeks).
        /// </para>
        /// </param>
        /// <param name="reason">The optional reason for the time out.</param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        /// <exception cref="FormatException">Thrown if the nick is not between 2 and 24 characters long or contians any non-alpha-numeric characters.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool
        Timeout(string channel, string user_nick, uint duration_seconds, string reason = "")
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick);

            // 1209600 seconds = 14 days (2 weeks)
            return SendChatCommand(channel, ChatCommand.Timeout, user_nick, duration_seconds.Clamp<uint>(1, 1209600), reason);
        }

        /// <summary>
        /// Un-time out a user.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <param name="user_nick">The user to un-time out.</param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        /// <exception cref="FormatException">Thrown if the nick is not between 2 and 24 characters long or contians any non-alpha-numeric characters.</exception>
        public bool
        Untimeout(string channel, string user_nick)
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick);

            return SendChatCommand(channel, ChatCommand.Untimeout, user_nick);
        }

        /// <summary>
        /// Clears all chat history from an IRC channel.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public bool
        ClearChat(string channel)
        {
            return SendChatCommand(channel, ChatCommand.Clear);
        }

        /// <summary>
        /// <para>Enables emote only mode in an IRC channel.</para>
        /// <para>
        /// Makes it so only emotes can be sent in chat.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public void
        EnableEmoteOnlyMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.EmoteOnly);
        }


        /// <summary>
        /// Disables emote only mode in an IRC channel.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public void
        DisableEmoteOnlyMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.EmoteOnlyOff);
        }

        /// <summary>
        /// <para>Enables follow olny mode in an IRC channel.</para>
        /// <para>
        /// Makes it so only followers can send messages in chat.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public void
        EnableFollowersOnlyMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.Followers);
        }

        /// <summary>
        /// <para>Enables follow olny mode in an IRC channel.</para>
        /// <para>
        /// Makes it so only users who have ben following the channel for a certain amouint of time can send messages in chat.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <param name="duration">
        /// <para>
        /// The amount of time the user must have been following the channel to send messages.
        /// Clamped between the minimum and the maximum values.
        /// </para>
        /// <para>
        /// Min: 0 seconds.
        /// Max: 3 months (90 days).
        /// </para>
        /// </param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        /// <exception cref="FormatException">Thrown if the string does not meet Twitch's /followers duration format requirements.</exception>
        public void
        EnableFollowersOnlyMode(string channel, string duration)
        {
            if (!TwitchIrcUtil.TryConvertToFollowerDuratrion(duration, out TimeSpan time_span_duration))
            {
                // If the format is valid and it couldn't be converted, the value overflowed. Just continue using the max duration.
                if (TwitchIrcUtil.IsValidFollowersDurationFormat(duration))
                {
                    time_span_duration = new TimeSpan(90, 0, 0, 0, 0);
                }
                else
                {
                    throw new FormatException("Invalid /followers duration format: " + duration + ".");
                }
            }

            EnableFollowersOnlyMode(channel, time_span_duration);
        }

        /// <summary>
        /// <para>Enables follow olny mode in an IRC channel.</para>
        /// <para>
        /// Makes it so only users who have ben following the channel for a certain amouint of time can send messages in chat.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <param name="duration">
        /// <para>
        /// The amount of time the user must have been following the channel to send messages.
        /// Clamped between the minimum and the maximum values.
        /// </para>
        /// <para>
        /// Min: 0 seconds.
        /// Max: 3 months (90 days).
        /// </para>
        /// </param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public void
        EnableFollowersOnlyMode(string channel, TimeSpan duration)
        {
            duration = duration.Clamp(TimeSpan.Zero, new TimeSpan(90, 0, 0, 0, 0));

            // manually calculate seconds in case the user used ms because Twitch doesn't support ms as a parameter.
            int seconds = duration.Seconds + duration.Milliseconds / 1000;
            string _duration = duration.Days + "d" + duration.Hours + "h" + duration.Minutes + "m" + seconds + "s";

            SendChatCommand(channel, ChatCommand.Followers, _duration);
        }

        /// <summary>
        /// Disables follower only mode.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public void
        DisableFollowersOnlyMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.FollowersOff);
        }

        // TODO: Change to /uniquechat and /uniquechatoff

        /// <summary>
        /// <para>Enables R9K mode in an IRC channel.</para>
        /// <para>
        /// Makes it so messages longer than 9 non-symbol unicode chacter messages must be unquie to be sent.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public void
        EnableR9KBetaMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.R9kBeta);
        }

        /// <summary>
        /// Disables R9K mode in an IRC channel.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public void
        DisableR9KBetaMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.R9kBetaOff);
        }

        /// <summary>
        /// <para>Enables slow mode in an IRC channel.</para>
        /// <para>
        /// Makes it so users can only send messages every 2 minutes.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public void
        EnableSlowMode(string channel)
        {
            EnableSlowMode(channel, 120);
        }

        /// <summary>
        /// <para>Enables slow mode in an IRC channel.</para>
        /// <para>
        /// Makes it so users can only send messages every so often.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <param name="frequency">
        /// <para>
        /// How frequent users can send chat messages.
        /// Clamped between the minimum and the maximum values.
        /// </para>
        /// <para>
        /// Min: 1 second.
        /// Max: 1 day.
        /// </para>
        /// </param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public void
        EnableSlowMode(string channel, TimeSpan frequency)
        {
            // Clamp the value up here to prevent a possible overflow exception with Convert.ToUint32()
            double frequency_seconds = frequency.TotalSeconds.Clamp(1, 86400);

            EnableSlowMode(channel, Convert.ToUInt32(frequency_seconds));
        }

        /// <summary>
        /// <para>Enables slow mode in an IRC channel.</para>
        /// <para>
        /// Makes it so users can only send messages every so often.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <param name="frequency_seconds">
        /// <para>
        /// How frequent users can send chat messages, in seconds.
        /// Clamped between the minimum and the maximum values.
        /// </para>
        /// <para>
        /// Min: 1 second.
        /// Max: 86,400 seconds (1 day).
        /// </para>
        /// </param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public void
        EnableSlowMode(string channel, uint frequency_seconds)
        {
            // 86400 seconds = 1 day
            SendChatCommand(channel, ChatCommand.Slow, frequency_seconds.Clamp<uint>(1, 86400));
        }

        /// <summary>
        /// Disables slow mode in an IRC channel.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public void
        DisableSlowMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.SlowOff);
        }

        /// <summary>
        /// <para>Enables subscriber only mode in an IRc channel.</para>
        /// <para>
        /// Makes it so only subscribers can send messages in chat.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public void
        EnableSubscribersOnlyMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.Subscribers);
        }

        /// <summary>
        /// Disables subscriber only mode in an IRC channel.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public bool
        DisableSubscribersOnlyMode(string channel)
        {
            return SendChatCommand(channel, ChatCommand.SubscribersOff);
        }

        /// <summary>
        /// Runs a commercial.
        /// This is apartner only command.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <param name="length">
        /// <para>The length of the commercial.</para>
        /// <para>Default: 30 seconds.</para>
        /// </param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public bool
        StartCommercial(string channel, CommercialLength length = CommercialLength.Seconds30)
        {
            return SendChatCommand(channel, ChatCommand.Commercial, EnumUtil.GetName(length));
        }

        /// <summary>
        /// Starts hosting another user who is streaming.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <param name="user_nick">The user to host.</param>
        /// <param name="tagline">A message shown to viewing parties.</param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        /// <exception cref="FormatException">Thrown if the nick is not between 2 and 24 characters long or contians any non-alpha-numeric characters.</exception>
        public bool
        Host(string channel, string user_nick, string tagline = "")
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick);

            return SendChatCommand(channel, ChatCommand.Host, user_nick, tagline);
        }

        /// <summary>
        /// Stops hosting a user.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public bool
        StopHost(string channel)
        {
            return SendChatCommand(channel, ChatCommand.Unhost);
        }

        /// <summary>
        /// Raid another user who is streaming.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <param name="user_nick">The IRC user's nick to raid.</param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        /// <exception cref="FormatException">Thrown if the nick is not between 2 and 24 characters long or contians any non-alpha-numeric characters.</exception>
        public bool
        Raid(string channel, string user_nick)
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick);

            return SendChatCommand(channel, ChatCommand.Raid, user_nick);
        }

        /// <summary>
        /// Stops a raid in progress from happening.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public bool
        StopRaid(string channel)
        {
            return SendChatCommand(channel, ChatCommand.Unraid);
        }

        /// <summary>
        /// Sends a chat command in an IRC channel.
        /// </summary>
        /// <param name="channel">
        /// The IRC channel to send the message in.
        /// The channel must be prefixed with the appropiate '#' or ampersand.
        /// </param>        
        /// <param name="command">The command to send.</param>
        /// <param name="arguments">Optional command arguments</param>
        /// <returns>
        /// Returns true if the message was sent to the IRC server.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the IRC channel is null, empty, or contains only whitespace.</exception>
        public bool
        SendChatCommand(string channel, ChatCommand command, params object[] arguments)
        {
            string trailing = EnumUtil.GetName(command);
            if (arguments.IsValid())
            {
                trailing += ' ' + string.Join(" ", arguments);
            }

            return SendPrivmsg(channel, trailing);
        }

        #endregion
    }

    public enum
    DisplayNameColor
    {
        /// <summary>
        /// Unsupported display name color.
        /// </summary>
        [EnumMember(Value = "")]
        Other = 0,

        /// <summary>
        /// Blue.
        /// </summary>
        [EnumMember(Value = "blue")]
        Blue,

        /// <summary>
        /// Blue violet.
        /// </summary>
        [EnumMember(Value = "blueviolet")]
        BlueViolet,

        /// <summary>
        /// Cadet blue.
        /// </summary>
        [EnumMember(Value = "cadetblue")]
        CadetBlue,

        /// <summary>
        /// Chocolate.
        /// </summary>
        [EnumMember(Value = "chocloate")]
        Chocloate,

        /// <summary>
        /// Coral.
        /// </summary>
        [EnumMember(Value = "coral")]
        Coral,

        /// <summary>
        /// Dodger blue.
        /// </summary>
        [EnumMember(Value = "dodgerblue")]
        DodgerBlue,

        /// <summary>
        /// Fire brick.
        /// </summary>
        [EnumMember(Value = "firebrick")]
        FireBrick,

        /// <summary>
        /// Golden rod.
        /// </summary>
        [EnumMember(Value = "goldenrod")]
        GoldenRod,

        /// <summary>
        /// Green.
        /// </summary>
        [EnumMember(Value = "green")]
        Green,

        /// <summary>
        /// Hot pink.
        /// </summary>
        [EnumMember(Value = "hotpink")]
        HotPink,

        /// <summary>
        /// Orange red.
        /// </summary>
        [EnumMember(Value = "orangered")]
        OrangeRed,

        /// <summary>
        /// Red.
        /// </summary>
        [EnumMember(Value = "red")]
        Red,

        /// <summary>
        /// Sea green.
        /// </summary>
        [EnumMember(Value = "seagreen")]
        SeaGreen,

        /// <summary>
        /// Spring green.
        /// </summary>
        [EnumMember(Value = "springgreen")]
        SpringGreen,

        /// <summary>
        /// Yellow green.
        /// </summary>
        [EnumMember(Value = "yellowgreen")]
        YellowGreen,
    }

    public enum
    ChatCommand
    {
        #region Other

        /// <summary>
        /// Unsupported chat command.
        /// </summary>
        [EnumMember(Value = "")]
        Other = 0,

        #endregion

        #region General commands

        /// <summary>
        /// <para>Changes the client's display name color.</para>
        /// <para>Command: /color</para>
        /// <para>
        /// Usage (Non-Turbo/Prime): /color {color_name}.
        /// Usage (Turbo/Prime): /color {html_color} OR /color {color_name}.
        /// </para>
        /// </summary>
        [EnumMember(Value = "/color")]
        Color,

        /// <summary>
        /// <para>Disconnects the client from a chat room.</para>
        /// <para>Command: /disconnect</para>
        /// <para>Usage: /disconnect</para>
        /// </summary>
        [EnumMember(Value = "/disconnect")]
        Disconnect,

        /// <summary>
        /// <para>Gets information on how to use a specirfic command.</para>
        /// <para>Command: /help</para>
        /// <para>Usage: /help {command}</para>
        /// </summary>
        [EnumMember(Value = "/help")]
        Help,

        /// <summary>
        /// <para>Makes all text in a message the client's display name color.</para>
        /// <para>Command: /me</para>
        /// <para>Command: /me {message}</para>
        /// </summary>
        [EnumMember(Value = "/me")]
        Me,

        /// <summary>
        /// <para>Prints a list of all the mods in a specific chat.</para>
        /// <para>Command: /mods</para>
        /// <para>Usage: /mods</para>
        /// </summary>
        [EnumMember(Value = "/mods")]
        Mods,

        /// <summary>
        /// <para>Sends a private message to another user.</para>
        /// <para>Command: /w {user_nick} {message}</para>
        /// </summary>
        [EnumMember(Value = "/w")]
        Whisper,

        #endregion

        #region User moderation commands

        /// <summary>
        /// <para>Bans a user in a chat.</para>
        /// <para>Command: /ban</para>
        /// <para>Usage: /ban {user_nick}</para>
        /// </summary>
        [EnumMember(Value = "/ban")]
        Ban,

        /// <summary>
        /// <para>Grants a user moderator status.</para>
        /// <para>Command: /mod</para>
        /// <para>Usage: /mod {user_nick}</para>
        /// </summary>
        [EnumMember(Value = "/mod")]
        Mod,

        /// <summary>
        /// <para>Times out a user for a certain amount of time in a chat.</para>
        /// <para>Command: /timeout</para>
        /// <para>Usage: /timeout {useR_nick} [{length_seconds}] [{reason}]</para>
        /// </summary>
        [EnumMember(Value = "/timeout")]
        Timeout,

        /// <summary>
        /// <para>Unbans a user from a chat.</para>
        /// <para>Command: /unban</para>
        /// <para>Usage: /unban {user_nick}</para>
        /// </summary>
        [EnumMember(Value = "/unban")]
        Unban,

        /// <summary>
        /// <para>MRemoves a user's moderator status.</para>
        /// <para>Command: /unmod</para>
        /// <para>Usage: /unmod {user_nick}</para>
        /// </summary>
        [EnumMember(Value = "/unmod")]
        Unmod,

        /// <summary>
        /// <para>Untimesout a user from a chat.</para>
        /// <para>Command: /untimeout</para>
        /// <para>Usage: /untimeout {user_nick}</para>
        /// </summary>
        [EnumMember(Value = "/untimeout")]
        Untimeout,

        #endregion

        #region Room moderation commands

        /// <summary>
        /// <para>
        /// Clears all messages and chat history from a chat.
        /// This command does not work in a chat room.
        /// </para>
        /// <para>Command: /clear</para>
        /// <para>Usage: /clear</para>
        /// </summary>
        [EnumMember(Value = "/clear")]
        Clear,

        /// <summary>
        /// <para>
        /// Makes it so only emotes can be sent in chat.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// <para>Command: /emoteonly</para>
        /// <para>Usage: /emoteonly</para>
        /// </summary>
        [EnumMember(Value = "/emoteonly")]
        EmoteOnly,

        /// <summary>
        /// <para>Disables emote only mode.</para>
        /// <para>Command: /emoteonlyoff</para>
        /// <para>Usage: /emoteonlyoff</para>
        /// </summary>
        [EnumMember(Value = "/emoteonlyoff")]
        EmoteOnlyOff,

        /// <summary>
        /// <para>
        /// Makes it so only followers can send messages in chat.
        /// This can require they be following in general or for a certain amount of time.
        /// The broadcaster and moderators are exempt from this command.
        /// This command does not work in a chat room.
        /// </para>
        /// <para>Command: /followers</para>
        /// <para>Usage: /followers [{length_custom}]</para>
        /// </summary>
        [EnumMember(Value = "/followers")]
        Followers,

        /// <summary>
        /// <para>
        /// Disables follower only mode.
        /// This command does not work in a chat room.
        /// </para>
        /// <para>Command: /followersoff</para>
        /// <para>Usage: /followersoff</para>
        /// </summary>
        [EnumMember(Value = "/followersoff")]
        FollowersOff,

        /// <summary>
        /// <para>
        /// Makes it so messages longer than 9 non-symbol unicode chacter messages must be unquie to be sent.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// <para>Command: /r9kbeta</para>
        /// <para>Usage: /r9kbeta</para>
        /// </summary>
        [EnumMember(Value = "/r9kbeta")]
        R9kBeta,

        /// <summary>
        /// <para>Disables r9k beta mode.</para>
        /// <para>Command: /r9kbetaoff</para>
        /// <para>Usage: /r9kbetaoff</para>
        /// </summary>
        [EnumMember(Value = "/r9kbetaoff")]
        R9kBetaOff,

        /// <summary>
        /// <para>
        /// Makes it so users can only send messages every so often (rate limiting).
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// <para>Command: /slow</para>
        /// <para>Usage: /slow [{length_seconds}]</para>
        /// </summary>
        [EnumMember(Value = "/slow")]
        Slow,

        /// <summary>
        /// <para>Disables slow mode.</para>
        /// <para>Command: /slowoff</para>
        /// <para>Usage: /slowoff</para>
        /// </summary>
        [EnumMember(Value = "/slowoff")]
        SlowOff,

        /// <summary>
        /// <para>
        /// Makes it so only subscribers can send messages in chat.
        /// The broadcaster and moderators are exempt from this command.
        /// This command does not work in a chat room.
        /// </para>
        /// <para>Command: /subscribers</para>
        /// <para>Usage: /subscribers</para>
        /// </summary>
        [EnumMember(Value = "/subscribers")]
        Subscribers,

        /// <summary>
        /// <para>
        /// Disables subsdriber only mode.
        /// This command does not work in a chat room.
        /// </para>
        /// <para>Command: /subscribersoff</para>
        /// <para>Usage: /subscribersoff</para>
        /// </summary>
        [EnumMember(Value = "/subscribersoff")]
        SubscribersOff,

        #endregion

        #region Broadcaster and editor commands

        /// <summary>
        /// <para>
        /// Runs a commercial.
        /// This is a partner only command.
        /// This command does not work in a chat room.
        /// </para>
        /// <para>Command: /commercial</para>
        /// <para>Usage: /commercial [{length_seconds}]</para>
        /// </summary>
        [EnumMember(Value = "/commercial")]
        Commercial,

        /// <summary>
        /// <para>
        /// Hosts another user who is streaming.
        /// This command does not work in a chat room.
        /// </para>
        /// <para>Command: /host</para>
        /// <para>Usage: /host {user_nick} [{message}]</para>
        /// </summary>
        [EnumMember(Value = "/host")]
        Host,

        /// <summary>
        /// <para>
        /// Raids another user who is streaming.
        /// This command does not work in a chat room.
        /// </para>
        /// <para>Command: /raid</para>
        /// <para>Usage: /raid {user_nick}</para>
        /// </summary>
        [EnumMember(Value = "/raid")]
        Raid,

        /// <summary>
        /// <para>
        /// Stops hosting a user.
        /// This command does not work in a chat room.
        /// </para>
        /// <para>Command: /unhost</para>
        /// <para>Usage: /unhost</para>
        /// </summary>
        [EnumMember(Value = "/unhost")]
        Unhost,

        /// <summary>
        /// <para>
        /// Stops a raid in progress from happening.
        /// This command does not work in a chat room.
        /// </para>
        /// <para>Command: /unraid</para>
        /// <para>Usage: /unraid</para>
        /// </summary>
        [EnumMember(Value = "/unraid")]
        Unraid,

        // TODO: /marker 

        #endregion
    }

    public enum
    CommercialLength
    {
        /// <summary>
        /// Unsupported commercial length.
        /// </summary>
        [EnumMember(Value = "")]
        Other = 0,

        /// <summary>
        /// 30 seconds.
        /// </summary>
        [EnumMember(Value = "30")]
        Seconds30 = 30,

        /// <summary>
        /// 1 minute.
        /// </summary>
        [EnumMember(Value = "60")]
        Seconds60 = 60,

        /// <summary>
        /// 1 minute, 30 seconds
        /// </summary>
        [EnumMember(Value = "90")]
        Seconds90 = 90,

        /// <summary>
        /// 2 minutes
        /// </summary>
        [EnumMember(Value = "120")]
        Seconds120 = 120,

        /// <summary>
        /// 2 minutes, 30 seconds
        /// </summary>
        [EnumMember(Value = "150")]
        Seconds150 = 150,

        /// <summary>
        /// 3 minutes
        /// </summary>
        [EnumMember(Value = "180")]
        Seconds180 = 180
    }
}