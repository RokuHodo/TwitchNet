// standard namespaces
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Rest.Api.Users;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc.Twitch
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
            OnChannelMode       += new EventHandler<ChannelModeEventArgs>(Callback_OnChannelMode);
            OnPrivmsg           += new EventHandler<PrivmsgEventArgs>(Callback_OnPrivmsg);
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
        /// <param name="user_nick">The recipient's nick.</param>
        /// <param name="format">
        /// The string to send.
        /// This can be a normal string and does not need to include variable formats.
        /// </param>
        /// <param name="arguments">Optional format variable arugments.</param>
        public void
        SendWhisper(string user_nick, string format, params string[] arguments)
        {
            ExceptionUtil.ThrowIfInvalid(user_nick, nameof(user_nick));
            ExceptionUtil.ThrowIfInvalid(format, nameof(format));

            string trailing = "/w " + user_nick.ToLower() + " " + (!arguments.IsValid() ? format : string.Format(format, arguments));
            SendPrivmsg("#jtv", trailing);
        }

        /// <summary>
        /// Joins a channel's chat room.
        /// </summary>
        /// <param name="user_id">The user_id of the channel. The owner of the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room.</param>
        public void
        JoinChatRoom(string user_id, string uuid)
        {
            ExceptionUtil.ThrowIfInvalid(user_id, nameof(user_id));
            ExceptionUtil.ThrowIfInvalid(uuid, nameof(uuid));

            Send("JOIN #chatrooms:" + user_id + ":" + uuid);
        }

        /// <summary>
        /// Leave a channel's chat room.
        /// </summary>
        /// <param name="user_id">The user_id of the channel. The owner of the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room.</param>
        public void
        PartChatRoom(string user_id, string uuid)
        {
            ExceptionUtil.ThrowIfInvalid(user_id, nameof(user_id));
            ExceptionUtil.ThrowIfInvalid(uuid, nameof(uuid));

            Send("PART #chatrooms:" + user_id + ":" + uuid);
        }

        /// <summary>
        /// Sends a private message in an IRC channel's chat room.
        /// </summary>
        /// <param name="user_id">The user_id of the channel. The owner of the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room.</param>
        /// <param name="format">
        /// The message to send.
        /// This can be a normal string and does not need to include variable formats.
        /// </param>
        /// <param name="arguments">Optional format variable arugments.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void
        SendChatRoomPrivmsg(string user_id, string uuid, string format, params string[] arguments)
        {
            ExceptionUtil.ThrowIfInvalid(user_id, nameof(user_id));
            ExceptionUtil.ThrowIfInvalid(uuid, nameof(uuid));
            Regex regex = new Regex(RegexPatternUtil.UUID);
            if (!regex.IsMatch(uuid))
            {
                throw new FormatException("The argument " + nameof(uuid).WrapQuotes() + " must match the regex pattern " + RegexPatternUtil.UUID.WrapQuotes());
            }
            ExceptionUtil.ThrowIfInvalid(format, nameof(format));

            string trailing = !arguments.IsValid() ? format : string.Format(format, arguments);
            SendPrivmsg("#chatrooms:" + user_id + ":" + uuid, trailing);
        }

        #endregion

        #region Chat command wrappers

        /// <summary>
        /// Change the client's display name color.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        /// <param name="color">The display name color.</param>
        public void
        ChangeDisplayNameColor(string channel, DisplayNameColor color)
        {
            SendChatCommand(channel, ChatCommand.Color, EnumUtil.GetName(color));
        }

        /// <summary>
        /// Change the client's display name color.
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        /// <param name="color">The display name color.</param>
        public void
        ChangeDisplayNameColor(string user_id, string uuid, DisplayNameColor color)
        {
            SendChatCommand(user_id, uuid, ChatCommand.Color, EnumUtil.GetName(color));
        }

        /// <summary>
        /// <para>Change the client's display name color.</para>
        /// <para>Requires Twitch Prime or Turbo to be used.</para>
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        /// <param name="color">The display name color.</param>
        public void
        ChangeDisplayNameColor(string channel, Color color)
        {
            if (color.IsEmpty)
            {
                return;
            }

            SendChatCommand(channel, ChatCommand.Color, ColorTranslator.ToHtml(color));
        }

        /// <summary>
        /// <para>Change the client's display name color.</para>
        /// <para>Requires Twitch Prime or Turbo to be used.</para>
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        /// <param name="color">The display name color.</param>
        public void
        ChangeDisplayNameColor(string user_id, string uuid, Color color)
        {
            if (color.IsEmpty)
            {
                return;
            }

            SendChatCommand(user_id, uuid, ChatCommand.Color, ColorTranslator.ToHtml(color));
        }

        /// <summary>
        /// <para>Change the client's display name color.</para>
        /// <para>Requires Twitch Prime or Turbo to be used.</para>
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        /// <param name="html_color">The display name color in hex (HTML) format.</param>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="html_color"/> is not in hex (HTML) format.</exception>
        public void
        ChangeDisplayNameColor(string channel, string html_color)
        {
            if (!html_color.IsValidHtmlColor())
            {
                throw new ArgumentException(nameof(html_color) + " is not a valid HTML color name", nameof(html_color));
            }

            SendChatCommand(channel, ChatCommand.Color, html_color);
        }

        /// <summary>
        /// <para>Change the client's display name color.</para>
        /// <para>Requires Twitch Prime or Turbo to be used.</para>
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        /// <param name="html_color">The display name color in hex (HTML) format.</param>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="html_color"/> is not in hex (HTML) format.</exception>
        public void
        ChangeDisplayNameColor(string user_id, string uuid, string html_color)
        {
            if (!html_color.IsValidHtmlColor())
            {
                throw new ArgumentException(nameof(html_color) + " is not a valid HTML color name", nameof(html_color));
            }

            SendChatCommand(user_id, uuid, ChatCommand.Color, html_color);
        }

        /// <summary>
        /// Reconnects to an IRC channel.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        public void
        ChatDisconnect(string channel)
        {
            SendChatCommand(channel, ChatCommand.Disconnect);
        }

        /// <summary>
        /// Reconnects to a chat room.
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        public void
        ChatDisconnect(string user_id, string uuid)
        {
            SendChatCommand(user_id, uuid, ChatCommand.Disconnect);
        }

        /// <summary>
        /// <para>Prints a list of chat commands that can be used by the client in an IRC channel.</para>
        /// <para>The list can be retrieved by using <see cref="OnCmdsAvailable"/>.</para>
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        public void
        PrintAvailableCommands(string channel)
        {
            SendChatCommand(channel, ChatCommand.Help);
        }

        /// <summary>
        /// <para>Prints a list of chat commands that can be used by the client in a chat room.</para>
        /// <para>The list can be retrieved by using <see cref="OnChatRoomCmdsAvailable"/>.</para>
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        public void
        PrintAvailableCommands(string user_id, string uuid)
        {
            SendChatCommand(user_id, uuid, ChatCommand.Help);
        }

        /// <summary>
        /// Prints help for a chat command that can be used in an IRC channel, if any help exists.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        /// <param name="command">The chat command to print help for.</param>
        public void
        PrintHelp(string channel, ChatCommand command)
        {
            SendChatCommand(channel, ChatCommand.Help, EnumUtil.GetName(command).TextAfter('/'));
        }

        /// <summary>
        /// Prints help for a chat command that can be used in a chat room, if any help exists.
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        /// <param name="command">The chat command to print help for.</param>
        public void
        PrintHelp(string user_id, string uuid, ChatCommand command)
        {
            SendChatCommand(user_id, uuid, ChatCommand.Help, EnumUtil.GetName(command).TextAfter('/'));
        }

        /// <summary>
        /// Sends a private message in an IRC channel using the /me chat command.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        /// <param name="format">
        /// The message to send.
        /// This can be a normal string and does not need to include variable formats.
        /// </param>
        /// <param name="arguments">Optional format variable arugments.</param>
        /// <exception cref="ArgumentException">Thrown if the format is null, empty, or whitespace.</exception>
        public void
        SendPrivmsgMe(string channel, string format, params string[] arguments)
        {
            // Check format here since only sending /me is not allowed
            ExceptionUtil.ThrowIfInvalid(format, nameof(format));

            string trailing = EnumUtil.GetName(ChatCommand.Me) + ' ' + format;
            SendPrivmsg(channel, trailing, arguments);
        }

        /// <summary>
        /// Sends a private message in a chat room using the /me chat command.
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        /// <param name="format">
        /// The message to send.
        /// This can be a normal string and does not need to include variable formats.
        /// </param>
        /// <param name="arguments">Optional format variable arugments.</param>
        /// <exception cref="ArgumentException">Thrown if the format is null, empty, or whitespace.</exception>
        public void
        SendChatRoomPrivmsgMe(string user_id, string uuid, string format, params string[] arguments)
        {
            // Check format here since only sending /me is not allowed
            ExceptionUtil.ThrowIfInvalid(format, nameof(format));

            string trailing = EnumUtil.GetName(ChatCommand.Me) + ' ' + format;
            SendChatRoomPrivmsg(user_id, uuid, trailing, arguments);
        }

        /// <summary>
        /// Grants a user operator (moderator) status.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        /// <param name="user_nick">The user to grant operator status.</param>
        /// <exception cref="FormatException">Thrown if the <paramref name="user_nick"/> does not match Twitch's user name requirements.</exception>
        public void Mod(string channel, string user_nick)
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick);

            SendChatCommand(channel, ChatCommand.Mod, user_nick);
        }

        /// <summary>
        /// Grants a user operator (moderator) status.
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        /// <param name="user_nick">The user to grant operator status.</param>
        /// <exception cref="FormatException">Thrown if the <paramref name="user_nick"/> does not match Twitch's user name requirements.</exception>
        public void Mod(string user_id, string uuid, string user_nick)
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick);

            SendChatCommand(user_id, uuid, ChatCommand.Mod, user_nick);
        }

        /// <summary>
        /// Removes a user's operator (moderator) status.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        /// <param name="user_nick">The user to grant operator status.</param>
        /// <exception cref="FormatException">Thrown if the <paramref name="user_nick"/> does not match Twitch's user name requirements.</exception>
        public void Unmod(string channel, string user_nick)
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick);

            SendChatCommand(channel, ChatCommand.Unmod, user_nick);
        }

        /// <summary>
        /// Removes a user's operator (moderator) status.
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        /// <param name="user_nick">The user to grant operator status.</param>
        /// <exception cref="FormatException">Thrown if the <paramref name="user_nick"/> does not match Twitch's user name requirements.</exception>
        public void Unmod(string user_id, string uuid, string user_nick)
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick);

            SendChatCommand(user_id, uuid, ChatCommand.Unmod, user_nick);
        }

        /// <summary>
        /// <para>Prints a list of oeprators (moderators) in an IRC channel.</para>
        /// <para>The list can be retrieved by using <see cref="OnRoomMods"/>.</para>
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        public void
        PrintMods(string channel)
        {
            SendChatCommand(channel, ChatCommand.Mods);
        }

        /// <summary>
        /// <para>Prints a list of oeprators (moderators) in a chat room.</para>
        /// <para>The list can be retrieved by using <see cref="OnChatRoomRoomMods"/>.</para>
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        public void
        PrintMods(string user_id, string uuid)
        {
            SendChatCommand(user_id, uuid, ChatCommand.Mods);
        }

        /// <summary>
        /// Bans a user.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        /// <param name="user_nick">The user to ban.</param>
        /// <param name="reason">The optional reason for the ban.</param>
        /// <exception cref="FormatException">Thrown if the <paramref name="user_nick"/> does not match Twitch's user name requirements.</exception>
        public void
        Ban(string channel, string user_nick, string reason = "")
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick);

            SendChatCommand(channel, ChatCommand.Ban, user_nick, reason);
        }

        /// <summary>
        /// Bans a user.
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        /// <param name="user_nick">The user to ban.</param>
        /// <param name="reason">The optional reason for the ban.</param>
        /// <exception cref="FormatException">Thrown if the <paramref name="user_nick"/> does not match Twitch's user name requirements.</exception>
        public void
        Ban(string user_id, string uuid, string user_nick, string reason = "")
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick);

            SendChatCommand(user_id, uuid, ChatCommand.Ban, user_nick, reason);
        }

        /// <summary>
        /// Unbans a user.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        /// <param name="user_nick">The user to unban.</param>
        /// <exception cref="FormatException">Thrown if the <paramref name="user_nick"/> does not match Twitch's user name requirements.</exception>
        public void
        Unban(string channel, string user_nick)
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick);

            SendChatCommand(channel, ChatCommand.Unban, user_nick);
        }

        /// <summary>
        /// Unbans a user.
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        /// <param name="user_nick">The user to unban.</param>
        /// <exception cref="FormatException">Thrown if the <paramref name="user_nick"/> does not match Twitch's user name requirements.</exception>
        public void
        Unban(string user_id, string uuid, string user_nick)
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick);

            SendChatCommand(user_id, uuid, ChatCommand.Unban, user_nick);
        }

        /// <summary>
        /// Time out a user for 1 second.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        /// <param name="user_nick">The user to purge.</param>
        /// <param name="reason">The optional reason for the purge.</param>
        public void
        Purge(string channel, string user_nick, string reason = "")
        {
            Timeout(channel, user_nick, 1, reason);
        }

        /// <summary>
        /// Time out a user for 1 second.
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        /// <param name="user_nick">The user to purge.</param>
        /// <param name="reason">The optional reason for the purge.</param>
        public void
        Purge(string user_id, string uuid, string user_nick, string reason = "")
        {
            Timeout(user_id, uuid, user_nick, 1, reason);
        }

        /// <summary>
        /// Time out a user for 5 minutes.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        /// <param name="user_nick">The user to purge.</param>
        /// <param name="reason">The optional reason for the time out.</param>
        public void
        Timeout(string channel, string user_nick, string reason = "")
        {
            Timeout(channel, user_nick, 600, reason);
        }

        /// <summary>
        /// Time out a user for 5 minutes.
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        /// <param name="user_nick">The user to purge.</param>
        /// <param name="reason">The optional reason for the time out.</param>
        public void
        Timeout(string user_id, string uuid, string user_nick, string reason = "")
        {
            Timeout(user_id, uuid, user_nick, 600, reason);
        }

        /// <summary>
        /// Time out a user for a specified amount of time.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
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
        public void
        Timeout(string channel, string user_nick, TimeSpan duration, string reason = "")
        {
            // Clamp the value up here to prevent a possible overflow exception with Convert.ToUint32()
            double length_seconds = duration.TotalSeconds.Clamp(1, 1209600);

            Timeout(channel, user_nick, Convert.ToUInt32(length_seconds), reason);
        }

        /// <summary>
        /// Time out a user for a specified amount of time.
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
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
        public void
        Timeout(string user_id, string uuid, string user_nick, TimeSpan length, string reason = "")
        {
            // Clamp the value up here to prevent a possible overflow exception with Convert.ToUint32()
            double length_seconds = length.TotalSeconds.Clamp(1, 1209600);

            Timeout(user_id, uuid, user_nick, Convert.ToUInt32(length_seconds), reason);
        }

        /// <summary>
        /// Time out a user for a specified amount of time.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
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
        /// <exception cref="FormatException">Thrown if the <paramref name="user_nick"/> does not match Twitch's user name requirements.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void
        Timeout(string channel, string user_nick, uint duration_seconds, string reason = "")
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick);

            // 1209600 seconds = 14 days (2 weeks)
            SendChatCommand(channel, ChatCommand.Timeout, user_nick, duration_seconds.Clamp<uint>(1, 1209600), reason);
        }

        /// <summary>
        /// Time out a user for a specified amount of time.
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        /// <param name="user_nick">The user to purge.</param>
        /// <param name="duration_seconds">
        /// <para>
        /// How long to time out the user for, in seconds.
        /// Clamped between the minimum and the maximum values.
        /// </para>
        /// <para>
        /// Min: 1 second.
        /// Max: 1209600 seconds (2 weeks).
        /// </para>
        /// </param>
        /// <param name="reason">The optional reason for the time out.</param>
        /// <exception cref="FormatException">Thrown if the <paramref name="user_nick"/> does not match Twitch's user name requirements.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void
        Timeout(string user_id, string uuid, string user_nick, uint duration_seconds, string reason = "")
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick);

            // 1209600 seconds = 14 days (2 weeks)
            SendChatCommand(user_id, uuid, ChatCommand.Timeout, user_nick, duration_seconds.Clamp<uint>(1, 1209600), reason);
        }

        /// <summary>
        /// Un-time out a user.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        /// <param name="user_nick">The user to un-time out.</param>
        /// <exception cref="FormatException">Thrown if the <paramref name="user_nick"/> does not match Twitch's user name requirements.</exception>
        public void
        Untimeout(string channel, string user_nick)
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick);

            SendChatCommand(channel, ChatCommand.Untimeout, user_nick);
        }

        /// <summary>
        /// Un-time out a user.
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        /// <param name="user_nick">The user to un-time out.</param>
        /// <exception cref="FormatException">Thrown if the <paramref name="user_nick"/> does not match Twitch's user name requirements.</exception>
        public void
        Untimeout(string user_id, string uuid, string user_nick)
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick);

            SendChatCommand(user_id, uuid, ChatCommand.Untimeout, user_nick);
        }

        /// <summary>
        /// Clears all chat history from an IRC channel.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        public void
        ClearChat(string channel)
        {
            SendChatCommand(channel, ChatCommand.Clear);
        }

        /// <summary>
        /// <para>Enables emote only mode in an IRC channel.</para>
        /// <para>
        /// Makes it so only emotes can be sent in chat.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        public void
        EnableEmoteOnlyMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.EmoteOnly);
        }

        /// <summary>
        /// <para>Enables emote only mode in a chat room.</para>
        /// <para>
        /// Makes it so only emotes can be sent in chat.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        public void
        EnableEmoteOnlyMode(string user_id, string uuid)
        {
            SendChatCommand(user_id, uuid, ChatCommand.EmoteOnly);
        }

        /// <summary>
        /// Disables emote only mode in an IRC channel.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        public void
        DisableEmoteOnlyMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.EmoteOnlyOff);
        }

        /// <summary>
        /// Disables emote only mode in a chat room.
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        public void
        DisableEmoteOnlyMode(string user_id, string uuid)
        {
            SendChatCommand(user_id, uuid, ChatCommand.EmoteOnlyOff);
        }

        /// <summary>
        /// <para>Enables follow olny mode in an IRC channel.</para>
        /// <para>
        /// Makes it so only followers can send messages in chat.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
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
        /// <param name="channel">The IRC channel. Where to send the message.</param>
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
        /// <exception cref="FormatException">Thrown if the string does not meet Twitch's /followers duration format requirements.</exception>
        public void
        EnableFollowersOnlyMode(string channel, string duration)
        {
            ExceptionUtil.ThrowIfInvalidFollowersDuration(duration);

            if (!TwitchUtil.TryConvertToFollowerDuratrion(duration, out TimeSpan time_span_duration))
            {
                // If the format is valid and it couldn't be converted, the value overflowed. Just continue using the max duration.
                if (TwitchUtil.IsValidFollowersDurationFormat(duration))
                {
                    time_span_duration = new TimeSpan(90, 0, 0, 0, 0);
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
        /// <param name="channel">The IRC channel. Where to send the message.</param>
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
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        public void
        DisableFollowersOnlyMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.FollowersOff);
        }

        /// <summary>
        /// <para>Enables R9K mode in an IRC channel.</para>
        /// <para>
        /// Makes it so messages longer than 9 non-symbol unicode chacter messages must be unquie to be sent.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        public void
        EnableR9KBetaMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.R9kBeta);
        }

        /// <summary>
        /// <para>Enables R9K mode in achat room.</para>
        /// <para>
        /// Makes it so messages longer than 9 non-symbol unicode chacter messages must be unquie to be sent.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        public void
        EnableR9KBetaMode(string user_id, string uuid)
        {
            SendChatCommand(user_id, uuid, ChatCommand.R9kBeta);
        }

        /// <summary>
        /// Disables R9K mode in an IRC channel.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        public void
        DisableR9KBetaMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.R9kBetaOff);
        }

        /// <summary>
        /// Disables R9K mode in a chat room.
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        public void
        DisableR9KBetaMode(string user_id, string uuid)
        {
            SendChatCommand(user_id, uuid, ChatCommand.R9kBetaOff);
        }

        /// <summary>
        /// <para>Enables slow mode in an IRC channel.</para>
        /// <para>
        /// Makes it so users can only send messages every 2 minutes.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        public void
        EnableSlowMode(string channel)
        {
            EnableSlowMode(channel, 120);
        }

        /// <summary>
        /// <para>Enables slow mode in a chat room.</para>
        /// <para>
        /// Makes it so users can only send messages every 2 minutes.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        public void
        EnableSlowMode(string user_id, string uuid)
        {
            EnableSlowMode(user_id, uuid, 120);
        }

        /// <summary>
        /// <para>Enables slow mode in an IRC channel.</para>
        /// <para>
        /// Makes it so users can only send messages every so often.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
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
        public void
        EnableSlowMode(string channel, TimeSpan frequency)
        {
            // Clamp the value up here to prevent a possible overflow exception with Convert.ToUint32()
            double frequency_seconds = frequency.TotalSeconds.Clamp(1, 86400);

            EnableSlowMode(channel, Convert.ToUInt32(frequency_seconds));
        }

        /// <summary>
        /// <para>Enables slow mode in a chat room.</para>
        /// <para>
        /// Makes it so users can only send messages every so often.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
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
        public void
        EnableSlowMode(string user_id, string uuid, TimeSpan frequency)
        {
            // Clamp the value up here to prevent a possible overflow exception with Convert.ToUint32()
            double frequency_seconds = frequency.TotalSeconds.Clamp(1, 86400);

            EnableSlowMode(user_id, uuid, Convert.ToUInt32(frequency_seconds));
        }

        /// <summary>
        /// <para>Enables slow mode in an IRC channel.</para>
        /// <para>
        /// Makes it so users can only send messages every so often.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
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
        public void
        EnableSlowMode(string channel, uint frequency_seconds)
        {
            // 86400 seconds = 1 day
            SendChatCommand(channel, ChatCommand.Slow, frequency_seconds.Clamp<uint>(1, 86400));
        }

        /// <summary>
        /// <para>Enables slow mode in a chat room.</para>
        /// <para>
        /// Makes it so users can only send messages every so often.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
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
        public void
        EnableSlowMode(string user_id, string uuid, uint frequency_seconds)
        {
            // 86400 seconds = 1 day
            SendChatCommand(user_id, uuid, ChatCommand.Slow, frequency_seconds.Clamp<uint>(1, 86400));
        }

        /// <summary>
        /// Disables slow mode in an IRC channel.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        public void
        DisableSlowMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.SlowOff);
        }

        /// <summary>
        /// Disables slow mode in a chat room.
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        public void
        DisableSlowMode(string user_id, string uuid)
        {
            SendChatCommand(user_id, uuid, ChatCommand.SlowOff);
        }

        /// <summary>
        /// <para>Enables subscriber only mode in an IRc channel.</para>
        /// <para>
        /// Makes it so only subscribers can send messages in chat.
        /// The broadcaster and moderators are exempt from this command.
        /// </para>
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        public void
        EnableSubscribersOnlyMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.Subscribers);
        }

        /// <summary>
        /// Disables subscriber only mode in an IRC channel.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        public void
        DisableSubscribersOnlyMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.SubscribersOff);
        }

        /// <summary>
        /// Runs a commercial.
        /// This is apartner only command.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        /// <param name="length">
        /// <para>The length of the commercial.</para>
        /// <para>Default: 30 seconds.</para>
        /// </param>
        public void
        StartCommercial(string channel, CommercialLength length = CommercialLength.Seconds30)
        {
            SendChatCommand(channel, ChatCommand.Commercial, EnumUtil.GetName(length));
        }

        /// <summary>
        /// Starts hosting another user who is streaming.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        /// <param name="user_nick">The user to host.</param>
        /// <param name="tagline">A message shown to viewing parties.</param>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="user_nick"/> does not match Twitch's user name requirements.</exception>
        public void
        Host(string channel, string user_nick, string tagline = "")
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick);

            SendChatCommand(channel, ChatCommand.Host, user_nick, tagline);
        }

        /// <summary>
        /// Stops hosting a user.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        public void
        StopHost(string channel)
        {
            SendChatCommand(channel, ChatCommand.Unhost);
        }

        /// <summary>
        /// Raid another user who is streaming.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        /// <param name="user_nick">The user to raid.</param>
        /// <exception cref="FormatException">Thrown if the <paramref name="user_nick"/> does not match Twitch's user name requirements.</exception>
        public void
        Raid(string channel, string user_nick)
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick);

            SendChatCommand(channel, ChatCommand.Raid, user_nick);
        }

        /// <summary>
        /// Stops a raid in progress from happening.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        public void
        StopRaid(string channel)
        {
            SendChatCommand(channel, ChatCommand.Unraid);
        }

        /// <summary>
        /// Sends a chat command in an IRC channel.
        /// </summary>
        /// <param name="channel">The IRC channel. Where to send the message.</param>
        /// <param name="command">The command to send.</param>
        /// <param name="arguments">Optional command arguments</param>
        public void
        SendChatCommand(string channel, ChatCommand command, params object[] arguments)
        {
            string trailing = EnumUtil.GetName(command);
            if (arguments.IsValid())
            {
                trailing += ' ' + string.Join(" ", arguments);
            }
            SendPrivmsg(channel, trailing);
        }

        /// <summary>
        /// Sends a chat command in a chat room.
        /// </summary>
        /// <param name="user_id">The id of the user who owns the chat room.</param>
        /// <param name="uuid">The unique uuid of the chat room. Where to send the message.</param>
        /// <param name="command">
        /// <para>The command to send. The following chat commands are not allowed/supported in chat rooms:</para>
        /// <para>
        /// <see cref="ChatCommand.Commercial"/>,
        /// <see cref="ChatCommand.Host"/>,
        /// <see cref="ChatCommand.Unhost"/>,
        /// <see cref="ChatCommand.Raid"/>,
        /// <see cref="ChatCommand.Unraid"/>,
        /// <see cref="ChatCommand.Clear"/>,
        /// <see cref="ChatCommand.Followers"/>,
        /// <see cref="ChatCommand.FollowersOff"/>.
        /// </para>
        /// </param>
        /// <param name="arguments">Optional command arguments</param>
        /// <exception cref="NotSupportedException">Thrown if an unsupported chat command is attempted to be used.</exception>
        public void
        SendChatCommand(string user_id, string uuid, ChatCommand command, params object[] arguments)
        {
            switch (command)
            {
                case ChatCommand.Commercial:
                case ChatCommand.Host:
                case ChatCommand.Unhost:
                case ChatCommand.Raid:
                case ChatCommand.Unraid:
                case ChatCommand.Clear:
                case ChatCommand.Followers:
                case ChatCommand.FollowersOff:
                {
                    throw new NotSupportedException("The command " + EnumUtil.GetName(command) + " cannot be used in a chatroom.");
                }
            }

            string trailing = EnumUtil.GetName(command);
            if (arguments.IsValid())
            {
                trailing += ' ' + string.Join(" ", arguments);
            }
            SendChatRoomPrivmsg(user_id, uuid, trailing);
        }

        #endregion
    }
}