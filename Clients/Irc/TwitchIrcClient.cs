// standard namespaces
using System;
using System.Drawing;
using System.Runtime.CompilerServices;

// project namespaces
using TwitchNet.Enums.Clients.Irc.Twitch;
using TwitchNet.Events.Clients.Irc;
using TwitchNet.Extensions;
using TwitchNet.Models.Api.Users;
using TwitchNet.Models.Clients.Irc;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc
{
    public partial class
    TwitchIrcClient : IrcClient
    {
        #region  Properties

        /// <summary>
        /// Determines whether or not to retrieve the Twitch user information after the socket connects to the server.
        /// </summary>
        public bool request_user_info   { get; set; }

        /// <summary>
        /// Determines whether or not to request /commands after the socket connects to the server.
        /// </summary>
        public bool request_commands    { get; set; }

        /// <summary>
        /// Determines whether or not to request /membership after the socket connects to the server.
        /// </summary>
        public bool request_membership  { get; set; }

        /// <summary>
        /// Determines whether or not to request /tags after the socket connects to the server.
        /// </summary>
        public bool request_tags        { get; set; }        

        /// <summary>
        /// The information of the Twitch irc user.
        /// This is only retrieved if <see cref="request_user_info"/> is set to true.
        /// </summary>
        public User twitch_user         { get; private set; }

        #endregion

        #region Constructors

        public TwitchIrcClient(ushort port, IrcUser irc_user) : base("irc.chat.twitch.tv", port, irc_user)
        {
            ExceptionUtil.ThrowIfInvalidNick(irc_user.nick, nameof(irc_user.nick));            

            DefaultHandlers();

            OnSocketConnected   += new EventHandler<EventArgs>(Callback_OnSocketConnected);
            OnChannelMode       += new EventHandler<ChannelModeEventArgs>(Callback_OnChannelMode);
            OnPrivmsg           += new EventHandler<PrivmsgEventArgs>(Callback_OnPrivmsg);
        }

        #endregion

        #region Twitch IRC command wrappers

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
        SendWhisper(string recipient_nick, string format, params string[] arguments)
        {
            ExceptionUtil.ThrowIfInvalid(recipient_nick, nameof(recipient_nick));
            ExceptionUtil.ThrowIfInvalid(format, nameof(format));

            string trailing = "/w " + recipient_nick.ToLower() + " " + (!arguments.IsValid() ? format : string.Format(format, arguments));
            SendPrivmsg("#jtv", trailing);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void
        SendChatRoomPrivmsg(string user_id, string uuid, string format, params string[] arguments)
        {
            ExceptionUtil.ThrowIfInvalid(user_id, nameof(user_id));
            ExceptionUtil.ThrowIfInvalid(uuid, nameof(uuid));
            ExceptionUtil.ThrowIfInvalid(format, nameof(format));

            string trailing = !arguments.IsValid() ? format : string.Format(format, arguments);
            SendPrivmsg("#chatrooms:" + user_id + ":" + uuid, trailing);
        }

        #endregion

        #region Twitch chat command wrappers

        public void
        ChangeDisplayNameColor(string channel, DisplayNameColor color)
        {
            SendChatCommand(channel, ChatCommand.Color, EnumCacheUtil.FromDisplayNameColor(color));
        }

        public void
        ChangeDisplayNameColor(string user_id, string uuid, DisplayNameColor color)
        {
            SendChatCommand(user_id, uuid, ChatCommand.Color, EnumCacheUtil.FromDisplayNameColor(color));
        }

        public void
        ChangeDisplayNameColor(string channel, Color color)
        {
            if (color.IsEmpty)
            {
                return;
            }

            SendChatCommand(channel, ChatCommand.Color, ColorTranslator.ToHtml(color));
        }

        public void
        ChangeDisplayNameColor(string user_id, string uuid, Color color)
        {
            if (color.IsEmpty)
            {
                return;
            }

            SendChatCommand(user_id, uuid, ChatCommand.Color, ColorTranslator.ToHtml(color));
        }

        public void
        ChangeDisplayNameColor(string channel, string html_color)
        {
            if (!TwitchUtil.IsValidHtmlColor(html_color))
            {
                throw new Exception(nameof(html_color) + " is not a valid HTML color name");
            }

            SendChatCommand(channel, ChatCommand.Color, html_color);
        }

        public void
        ChangeDisplayNameColor(string user_id, string uuid, string html_color)
        {
            if (!TwitchUtil.IsValidHtmlColor(html_color))
            {
                throw new Exception(nameof(html_color) + " is not a valid HTML color name");
            }

            SendChatCommand(user_id, uuid, ChatCommand.Color, html_color);
        }

        public void
        ChatDisconnect(string channel)
        {
            SendChatCommand(channel, ChatCommand.Disconnect);
        }

        public void
        ChatDisconnect(string user_id, string uuid)
        {
            SendChatCommand(user_id, uuid, ChatCommand.Disconnect);
        }

        public void
        PrintAvailableCommands(string channel)
        {
            SendChatCommand(channel, ChatCommand.Help);
        }

        public void
        PrintAvailableCommands(string user_id, string uuid)
        {
            SendChatCommand(user_id, uuid, ChatCommand.Help);
        }

        public void
        PrintHelp(string channel, ChatCommand command)
        {
            SendChatCommand(channel, ChatCommand.Help, EnumCacheUtil.FromChatCommand(command).TextAfter('/'));
        }

        public void
        PrintHelp(string user_id, string uuid, ChatCommand command)
        {
            SendChatCommand(user_id, uuid, ChatCommand.Help, EnumCacheUtil.FromChatCommand(command).TextAfter('/'));
        }

        public void
        SendPrivmsgMe(string channel, string format, params string[] arguments)
        {
            // Check format here since only sending /me is not allowed
            ExceptionUtil.ThrowIfInvalid(format, nameof(format));

            string trailing = EnumCacheUtil.FromChatCommand(ChatCommand.Me) + ' ' + format;
            SendPrivmsg(channel, trailing, arguments);
        }

        public void
        SendChatRoomPrivmsgMe(string user_id, string uuid, string format, params string[] arguments)
        {
            // Check format here since only sending /me is not allowed
            ExceptionUtil.ThrowIfInvalid(format, nameof(format));

            string trailing = EnumCacheUtil.FromChatCommand(ChatCommand.Me) + ' ' + format;
            SendChatRoomPrivmsg(user_id, uuid, trailing, arguments);
        }

        public void
        Mods(string channel)
        {
            SendChatCommand(channel, ChatCommand.Mods);
        }

        public void
        Mods(string user_id, string uuid)
        {
            SendChatCommand(user_id, uuid, ChatCommand.Mods);
        }

        public void
        Ban(string channel, string user_nick, string reason = "")
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick, nameof(user_nick));

            SendChatCommand(channel, ChatCommand.Ban, user_nick, reason);
        }

        public void
        Ban(string user_id, string uuid, string user_nick, string reason = "")
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick, nameof(user_nick));

            SendChatCommand(user_id, uuid, ChatCommand.Ban, user_nick, reason);
        }

        public void
        Unban(string channel, string user_nick)
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick, nameof(user_nick));

            SendChatCommand(channel, ChatCommand.Unban, user_nick);
        }

        public void
        Unban(string user_id, string uuid, string user_nick)
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick, nameof(user_nick));

            SendChatCommand(user_id, uuid, ChatCommand.Unban, user_nick);
        }

        public void
        Purge(string channel, string user_nick, string reason = "")
        {
            Timeout(channel, user_nick, 1, reason);
        }

        public void
        Purge(string user_id, string uuid, string user_nick, string reason = "")
        {
            Timeout(user_id, uuid, user_nick, 1, reason);
        }

        public void
        Timeout(string channel, string user_nick, string reason = "")
        {
            Timeout(channel, user_nick, 600, reason);
        }

        public void
        Timeout(string user_id, string uuid, string user_nick, string reason = "")
        {
            Timeout(user_id, uuid, user_nick, 600, reason);
        }

        public void
        Timeout(string channel, string user_nick, TimeSpan length, string reason = "")
        {
            // Clamp the value up here to prevent a possible overflow exception with Convert.ToUint32()
            double length_seconds = length.TotalSeconds.Clamp(1, 1209600);

            Timeout(channel, user_nick, Convert.ToUInt32(length_seconds), reason);
        }

        public void
        Timeout(string user_id, string uuid, string user_nick, TimeSpan length, string reason = "")
        {
            // Clamp the value up here to prevent a possible overflow exception with Convert.ToUint32()
            double length_seconds = length.TotalSeconds.Clamp(1, 1209600);

            Timeout(user_id, uuid, user_nick, Convert.ToUInt32(length_seconds), reason);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void
        Timeout(string channel, string user_nick, uint length_seconds, string reason = "")
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick, nameof(user_nick));

            // 1209600 seconds = 14 days (2 weeks)
            SendChatCommand(channel, ChatCommand.Timeout, user_nick, length_seconds.Clamp<uint>(1, 1209600), reason);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void
        Timeout(string user_id, string uuid, string user_nick, uint length_seconds, string reason = "")
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick, nameof(user_nick));

            // 1209600 seconds = 14 days (2 weeks)
            SendChatCommand(user_id, uuid, ChatCommand.Timeout, user_nick, length_seconds.Clamp<uint>(1, 1209600), reason);
        }

        public void
        Untimeout(string channel, string user_nick)
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick, nameof(user_nick));

            SendChatCommand(channel, ChatCommand.Untimeout, user_nick);
        }

        public void
        Untimeout(string user_id, string uuid, string user_nick)
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick, nameof(user_nick));

            SendChatCommand(user_id, uuid, ChatCommand.Untimeout, user_nick);
        }

        public void
        ClearChat(string channel)
        {
            SendChatCommand(channel, ChatCommand.Clear);
        }

        public void
        EnableEmoteOnlyMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.EmoteOnly);
        }

        public void
        EnableEmoteOnlyMode(string user_id, string uuid)
        {
            SendChatCommand(user_id, uuid, ChatCommand.EmoteOnly);
        }

        public void
        DisableEmoteOnlyMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.EmoteOnlyOff);
        }

        public void
        DisableEmoteOnlyMode(string user_id, string uuid)
        {
            SendChatCommand(user_id, uuid, ChatCommand.EmoteOnlyOff);
        }

        public void
        EnableFollowerOnlyMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.Followers);
        }

        public void
        DisableFollowerOnlyMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.FollowersOff);
        }

        public void
        EnableR9KBetaMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.R9kBeta);
        }

        public void
        EnableR9KBetaMode(string user_id, string uuid)
        {
            SendChatCommand(user_id, uuid, ChatCommand.R9kBeta);
        }

        public void
        DisableR9KBetaMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.R9kBetaOff);
        }

        public void
        DisableR9KBetaMode(string user_id, string uuid)
        {
            SendChatCommand(user_id, uuid, ChatCommand.R9kBetaOff);
        }

        public void
        EnableSlowMode(string channel)
        {
            EnableSlowMode(channel, 120);
        }

        public void
        EnableSlowMode(string user_id, string uuid)
        {
            EnableSlowMode(user_id, uuid, 120);
        }

        public void
        EnableSlowMode(string channel, TimeSpan frequency)
        {
            // Clamp the value up here to prevent a possible overflow exception with Convert.ToUint32()
            double frequency_seconds = frequency.TotalSeconds.Clamp(1, 86400);

            EnableSlowMode(channel, Convert.ToUInt32(frequency_seconds));
        }

        public void
        EnableSlowMode(string channel, uint frequency_seconds)
        {
            // 86400 seconds = 1 day
            SendChatCommand(channel, ChatCommand.Slow, frequency_seconds.Clamp<uint>(1, 86400));
        }

        public void
        EnableSlowMode(string user_id, string uuid, TimeSpan frequency)
        {
            // Clamp the value up here to prevent a possible overflow exception with Convert.ToUint32()
            double frequency_seconds = frequency.TotalSeconds.Clamp(1, 86400);

            EnableSlowMode(user_id, uuid, Convert.ToUInt32(frequency_seconds));
        }

        public void
        EnableSlowMode(string user_id, string uuid, uint frequency_seconds)
        {
            // 86400 seconds = 1 day
            SendChatCommand(user_id, uuid, ChatCommand.Slow, frequency_seconds.Clamp<uint>(1, 86400));
        }

        public void
        DisableSlowMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.SlowOff);
        }

        public void
        DisableSlowMode(string user_id, string uuid)
        {
            SendChatCommand(user_id, uuid, ChatCommand.SlowOff);
        }

        public void
        EnableSubscriberOnlyMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.Subscribers);
        }

        public void
        DisableSubscriberOnlyMode(string channel)
        {
            SendChatCommand(channel, ChatCommand.SubscribersOff);
        }

        public void
        StartCommercial(string channel, CommercialLength length = CommercialLength.Seconds30)
        {
            SendChatCommand(channel, ChatCommand.Commercial, EnumCacheUtil.FromCommercialLength(length));
        }

        public void
        Host(string channel, string user_nick, string message = "")
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick, nameof(user_nick));

            SendChatCommand(channel, ChatCommand.Host, user_nick, message);
        }

        public void
        StopHost(string channel)
        {
            SendChatCommand(channel, ChatCommand.Unhost);
        }

        public void
        Raid(string channel, string user_nick)
        {
            ExceptionUtil.ThrowIfInvalidNick(user_nick, nameof(user_nick));

            SendChatCommand(channel, ChatCommand.Raid, user_nick);
        }

        public void
        StopRaid(string channel)
        {
            SendChatCommand(channel, ChatCommand.Unraid);
        }

        public void
        SendChatCommand(string channel, ChatCommand command, params object[] arguments)
        {
            string trailing = EnumCacheUtil.FromChatCommand(command);
            if (arguments.IsValid())
            {
                trailing += ' ' + string.Join(" ", arguments);
            }
            SendPrivmsg(channel, trailing);
        }

        public void
        SendChatCommand(string user_id, string uuid, ChatCommand command, params object[] arguments)
        {
            string trailing = EnumCacheUtil.FromChatCommand(command);
            if (arguments.IsValid())
            {
                trailing += ' ' + string.Join(" ", arguments);
            }
            SendPrivmsg(user_id, uuid, trailing);
        }

        #endregion
    }
}