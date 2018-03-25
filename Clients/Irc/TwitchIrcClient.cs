// standard namespaces
using System;
using System.Drawing;
using System.Runtime.CompilerServices;

// project namespaces
using TwitchNet.Api;
using TwitchNet.Enums.Clients.Irc.Twitch;
using TwitchNet.Events.Clients.Irc;
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
        #region  Properties

        /// <summary>
        /// The information of the Twitch irc user.
        /// </summary>
        public User twitch_user { get; private set; }

        #endregion

        #region Constructors

        public TwitchIrcClient(ushort port, IrcUser irc_user) : base("irc.chat.twitch.tv", port, irc_user)
        {
            if (!TwitchUtil.IsValidNick(irc_user.nick))
            {
                throw new ArgumentException(nameof(irc_user.nick) + " can only contain lower case lpha-numeric characters and must be between 2 and 24 characters long.", nameof(irc_user.nick));
            }

            // TODO: Make this an optional setting, don't make an unintuitive request unless the user opts into it
            IApiResponse<Data<User>> _twitch_user = TwitchApiBearer.GetUser(irc_user.pass);
            if (!_twitch_user.result.data.IsValid())
            {
                throw new Exception("Could not get the user associated with the Bearer token");
            }
            twitch_user = _twitch_user.result.data[0];

            DefaultHandlers();

            OnChannelMode   += new EventHandler<ChannelModeEventArgs>(Callback_OnChannelMode);
            OnPrivmsg       += new EventHandler<PrivmsgEventArgs>(Callback_OnPrivmsg);
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

        // TODO: Make wrappers for commands that work in chat room.

        public void
        ChangeDisplayNameColor(string channel, DisplayNameColor color)
        {
            string trialing = "/color " + EnumCacheUtil.FromDisplayNameColor(color);
            SendPrivmsg(channel, trialing);
        }

        public void
        ChangeDisplayNameColor(string channel, Color color)
        {
            if (color.IsEmpty)
            {
                return;
            }

            string trialing = "/color " + ColorTranslator.ToHtml(color);
            SendPrivmsg(channel, trialing);
        }

        public void
        ChangeDisplayNameColor(string channel, string html_color)
        {
            if (!TwitchUtil.IsValidHtmlColor(html_color))
            {
                throw new Exception(nameof(html_color) + " is not a valid HTML color name");
            }

            string trialing = "/color " + html_color;
            SendPrivmsg(channel, trialing);
        }

        public void
        ChatDisconnect(string channel)
        {
            SendPrivmsg(channel, "/disconnect");
        }

        public void
        ChatDisconnect(string user_id, string uuid)
        {
            SendChatRoomPrivmsg(user_id, uuid, "/disconnect");
        }

        public void
        GetHelp(string channel)
        {
            // This lists the commands available for use when no command is specified with /help
            string trailing = "/help";
            SendPrivmsg(channel, trailing);
        }

        public void
        GetHelp(string channel, ChatCommand command)
        {
            string trailing = "/help " + EnumCacheUtil.FromChatCommand(command).TextAfter('/');
            SendPrivmsg(channel, trailing);
        }

        // TODO: Make this regular overload for SendPrivmsg?
        public void
        SendPrivmsgMe(string channel, string body)
        {
            ExceptionUtil.ThrowIfInvalid(body, nameof(body));

            string trailing = "/me " + body;
            SendPrivmsg(channel, trailing);
        }

        public void
        Mods(string channel)
        {
            string trailing = "/mods";
            SendPrivmsg(channel, trailing);
        }

        public void
        Ban(string channel, string user_nick, string reason = "")
        {
            if (!TwitchUtil.IsValidNick(user_nick))
            {
                throw new ArgumentException("Invalid " + nameof(user_nick) + ". " + nameof(user_nick) + " can only contain lower case alpha-numeric characters and must be between 2 and 24 characters long.", nameof(user_nick));
            }

            string trailing = "/ban " + user_nick;
            if (reason.IsValid())
            {
                trailing += " " + reason;
            }
            SendPrivmsg(channel, trailing);
        }

        public void
        Unban(string channel, string user_nick)
        {
            if (!TwitchUtil.IsValidNick(user_nick))
            {
                throw new ArgumentException("Invalid " + nameof(user_nick) + ". " + nameof(user_nick) + " can only contain lower case alpha-numeric characters and must be between 2 and 24 characters long.", nameof(user_nick));
            }

            string trailing = "/unban " + user_nick;
            SendPrivmsg(channel, trailing);
        }

        public void
        Purge(string channel, string user_nick, string reason = "")
        {
            Timeout(channel, user_nick, 1, reason);
        }

        public void
        Timeout(string channel, string user_nick, string reason = "")
        {
            Timeout(channel, user_nick, 600, reason);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void
        Timeout(string channel, string user_nick, ulong length_seconds, string reason = "")
        {
            if (!TwitchUtil.IsValidNick(user_nick))
            {
                throw new ArgumentException("Invalid " + nameof(user_nick) + ". " + nameof(user_nick) + " can only contain lower case alpha-numeric characters and must be between 2 and 24 characters long.", nameof(user_nick));
            }

            // TODO: Make an overload that uses timespan?
            // 1209600 = 14 days (2 weeks)
            length_seconds = length_seconds.Clamp<ulong>(1, 1209600);

            string trailing = "/timeout " + user_nick + " " + length_seconds;
            if (reason.IsValid())
            {
                trailing += " " + reason;
            }
            SendPrivmsg(channel, trailing);
        }

        public void
        Untimeout(string channel, string user_nick)
        {
            if (!TwitchUtil.IsValidNick(user_nick))
            {
                throw new ArgumentException("Invalid " + nameof(user_nick) + ". " + nameof(user_nick) + " can only contain lower case alpha-numeric characters and must be between 2 and 24 characters long.", nameof(user_nick));
            }

            string trailing = "/untimeout " + user_nick;
            SendPrivmsg(channel, trailing);
        }

        public void
        ClearChat(string channel)
        {
            string trailing = "/clear";
            SendPrivmsg(channel, trailing);
        }

        public void
        EnableEmoteOnlyMode(string channel)
        {
            string trailing = "/emoteonly";
            SendPrivmsg(channel, trailing);
        }

        public void
        DisableEmoteOnlyMode(string channel)
        {
            string trailing = "/emoteonlyoff";
            SendPrivmsg(channel, trailing);
        }

        public void
        EnableFollowerOnlyMode(string channel)
        {
            string trailing = "/followers";
            SendPrivmsg(channel, trailing);
        }

        public void
        DisableFollowerOnlyMode(string channel)
        {
            string trailing = "/followersoff";
            SendPrivmsg(channel, trailing);
        }

        public void
        EnableR9KBetaMode(string channel)
        {
            string trailing = "/r9kbeta";
            SendPrivmsg(channel, trailing);
        }

        public void
        DisableR9KBetaMode(string channel)
        {
            string trailing = "/r9kbetaoff";
            SendPrivmsg(channel, trailing);
        }

        public void
        EnableSlowMode(string channel, ulong frequency_seconds)
        {
            // TODO: Make an overload that uses timespan?
            // 86400 = 1 day
            frequency_seconds = frequency_seconds.Clamp<ulong>(1, 86400);

            string trailing = "/slow " + frequency_seconds;
            SendPrivmsg(channel, trailing);
        }

        public void
        DisableSlowMode(string channel)
        {
            string trailing = "/slowoff";
            SendPrivmsg(channel, trailing);
        }

        public void
        EnableSubscriberOnlyMode(string channel)
        {
            string trailing = "/subscribers";
            SendPrivmsg(channel, trailing);
        }

        public void
        DisableSubscriberOnlyMode(string channel)
        {
            string trailing = "/subscribersoff";
            SendPrivmsg(channel, trailing);
        }

        public void
        StartCommercial(string channel, CommercialLength length = CommercialLength.Seconds30)
        {
            string trailing = "/commercial " + EnumCacheUtil.FromCommercialLength(length);
            SendPrivmsg(channel, trailing);
        }

        public void
        Host(string channel, string user_nick, string message)
        {
            if (!TwitchUtil.IsValidNick(user_nick))
            {
                throw new ArgumentException("Invalid " + nameof(user_nick) + ". " + nameof(user_nick) + " can only contain lower case alpha-numeric characters and must be between 2 and 24 characters long.", nameof(user_nick));
            }

            string trailing = "/host " + user_nick;
            if (message.IsValid())
            {
                trailing += " " + message;
            }
            SendPrivmsg(channel, trailing);
        }

        public void
        Unhost(string channel)
        {
            string trailing = "/unhost";
            SendPrivmsg(channel, trailing);
        }

        public void
        Raid(string channel, string user_nick)
        {
            if (!TwitchUtil.IsValidNick(user_nick))
            {
                throw new ArgumentException("Invalid " + nameof(user_nick) + ". " + nameof(user_nick) + " can only contain lower case alpha-numeric characters and must be between 2 and 24 characters long.", nameof(user_nick));
            }

            string trailing = "/raid " + user_nick;
            SendPrivmsg(channel, trailing);
        }

        public void
        Unraid(string channel)
        {
            string trailing = "/raid";
            SendPrivmsg(channel, trailing);
        }

        #endregion
    }
}