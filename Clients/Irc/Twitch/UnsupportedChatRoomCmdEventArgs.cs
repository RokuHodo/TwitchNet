﻿// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public class
    UnsupportedChatRoomCmdEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]

        public string       channel         { get; protected set; }

        /// <summary>
        /// The id of the user who the chat room belongs to.
        /// </summary>
        [ValidateMember(Check.IsValid)]

        public string       channel_user_id { get; protected set; }

        /// <summary>
        /// The unique UUID of the chat room.
        /// </summary>
        [ValidateMember(Check.IsValid)]

        public string       channel_uuid    { get; protected set; }

        /// <summary>
        /// The unsupported chat command that was attempted to be used in a chat room.
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, ChatCommand.Other)]
        public ChatCommand  command         { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="UnsupportedChatRoomCmdEventArgs"/> class.
        /// </summary>
        /// <param name="args">The event arguments to parse.</param>
        public UnsupportedChatRoomCmdEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;
            channel_user_id = channel.TextBetween(':', ':');

            int index = channel.LastIndexOf(':');
            if (index != -1)
            {
                channel_uuid = channel.TextAfter(':', index);
            }

            command = EnumCacheUtil.ToChatCommand(args.body.TextBetween("command ", " cannot"));
        }
    }
}