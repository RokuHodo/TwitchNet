// standard namespaces
using System;
using System.Text.RegularExpressions;

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
        /// <summary>
        /// The information of the Twitch irc user.
        /// </summary>
        public User twitch_user { get; private set; }

        public TwitchIrcClient(ushort port, IrcUser irc_user) : base("irc.chat.twitch.tv", port, irc_user)
        {
            // TODO: Move this regex check into the into the class on set{}
            // allow upper case to not be too strict and just ToLower() it later
            Regex regex = new Regex("^[a-zA-Z][a-zA-Z0-9_]{2,24}$");
            if (!regex.IsMatch(irc_user.nick))
            {
                throw new ArgumentException(nameof(irc_user.nick) + " can only contain alpha-numeric characters and must be at least 3 characters long.", nameof(irc_user.nick));
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

        #region Twitch command wrappers

        // TODO: Implement wrappers for all chat commands for stream chat and chat rooms and any neccessary events for them

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

        public void
        Color(DisplayNameColor color)
        {
            string trialing = ".color " + EnumCacheUtil.FromDisplayNameColor(color);
            SendPrivmsg("#jtv", trialing);
        }

        #endregion
    }
}