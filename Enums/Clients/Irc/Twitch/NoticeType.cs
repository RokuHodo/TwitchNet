// standard namespaces
using System.Runtime.Serialization;

namespace TwitchNet.Enums.Clients.Irc.Twitch
{
    public enum
    NoticeType
    {
        //TODO: Test for undocumented values, mostly due to chat commands (like /color) and improper use of chat commands

        /// <summary>
        /// The msg-id tag could not be parsed.
        /// </summary>
        [EnumMember(Value = "")]
        None                    = 0,

        // TODO: Raise separate event for this and parse relevant extra data?
        /// <summary>
        /// A user was attempted to be banned that is already banned.
        /// </summary>
        [EnumMember(Value = "already_banned")]
        AlreadyBanned           = 1,

        /// <summary>
        /// Emote only mode was attempted to be turned off but is already off.
        /// </summary>
        [EnumMember(Value = "already_emote_only_off")]
        AlreadyEmoteOnlyOff     = 2,

        /// <summary>
        /// Emote only mode was attempted to be turned on but is already on.
        /// </summary>
        [EnumMember(Value = "already_emote_only_on")]
        AlreadyEmoteOnlyOn      = 3,

        /// <summary>
        /// R9K mode was attempted to be turned off but is already off.
        /// </summary>
        [EnumMember(Value = "already_r9k_off")]
        AlreadyR9kOff           = 4,

        /// <summary>
        /// R9K mode was attempted to be turned on but is already on.
        /// </summary>
        [EnumMember(Value = "already_r9k_on")]
        AlreadyR9kOn            = 5,

        /// <summary>
        /// Subscriber only mode was attempted to be turned off but is already off.
        /// </summary>
        [EnumMember(Value = "already_subs_off")]
        AlreadySubsOff          = 6,

        /// <summary>
        /// Subscriber only mode was attempted to be turned on but is already on.
        /// </summary>
        [EnumMember(Value = "already_subs_on")]
        AlreadySubsOn           = 7,

        // TODO: Raise separate event for this and parse relevant extra data?
        /// <summary>
        /// Attempted to host a user that you are already hosting.
        /// </summary>
        [EnumMember(Value = "bad_host_hosting")]
        BadHostHosting          = 8,

        // TODO: Raise separate event for this and parse relevant extra data?
        /// <summary>
        /// A user was banned.
        /// </summary>
        [EnumMember(Value = "ban_success")]
        BanSuccess              = 9,

        // TODO: Raise separate event for this and parse relevant extra data?
        /// <summary>
        /// A user was attempted to be banned but is already banned.
        /// </summary>
        [EnumMember(Value = "bad_unban_no_ban")]
        BadUnbanNoBan           = 10,

        /// <summary>
        /// Emote only mode in a room was turned off.
        /// </summary>
        [EnumMember(Value = "emote_only_off")]
        EmoteOnlyOff            = 11,

        /// <summary>
        /// Emote only mode in a room was turned on.
        /// </summary>
        [EnumMember(Value = "emote_only_on")]
        EmoteOnlyOn             = 12,

        /// <summary>
        /// A channel has exited host mode.
        /// </summary>
        [EnumMember(Value = "host_off")]
        HostOff                 = 13,

        /// <summary>
        /// A channel has started hosting another user.
        /// </summary>
        [EnumMember(Value = "host_on")]
        HostOn                  = 14,

        // TODO: Raise separate event for this and parse relevant extra data?
        /// <summary>
        /// A channel has started hosting another user and has 'n' number of hosts remaining.
        /// </summary>
        [EnumMember(Value = "hosts_remaining")]
        HostsRemaining          = 15,

        /// <summary>
        /// The client has attempted to join a channel that was either suspended or deleted.
        /// </summary>
        [EnumMember(Value = "msg_channel_suspended")]
        MsgChannelSuspended     = 16,

        /// <summary>
        /// The client has attempted to join a chat room that does not exist.
        /// </summary>
        [EnumMember(Value = "msg_room_not_found")]
        MsgRoomNotFound         = 17,

        /// <summary>
        /// The client has attempted to perform an action it does not have the permission to do.
        /// </summary>
        [EnumMember(Value = "no_permission")]
        NoPermission            = 18,

        /// <summary>
        /// R9K mode was turned off.
        /// </summary>
        [EnumMember(Value = "r9k_off")]
        R9kOff                  = 19,

        /// <summary>
        /// R9K mode was turned on.
        /// </summary>
        [EnumMember(Value = "r9k_on")]
        R9kOn                   = 20,

        /// <summary>
        /// Slow mode was turned off.
        /// </summary>
        [EnumMember(Value = "slow_off")]
        SlowOff                 = 21,

        // TODO: Raise separate event for this and parse relevant extra data?
        /// <summary>
        /// Slow mode was turned on.
        /// </summary>
        [EnumMember(Value = "slow_on")]
        SlowOn                  = 22,

        /// <summary>
        /// Subscriber only mode was turned off.
        /// </summary>
        [EnumMember(Value = "subs_off")]
        SubsOff                 = 23,

        /// <summary>
        /// Subscriber only mode was turned on.
        /// </summary>
        [EnumMember(Value = "subs_on")]
        SubsOn                  = 24,

        /// <summary>
        /// A user was timed out.
        /// </summary>
        [EnumMember(Value = "timeout_success")]
        TimeoutSuccess          = 25,

        // TODO: Raise separate event for this and parse relevant extra data?
        /// <summary>
        /// A user was unbanned.
        /// </summary>
        [EnumMember(Value = "unban_success")]
        UnbanSuccess            = 26,

        // TODO: Raise separate event for this and parse relevant extra data?
        /// <summary>
        /// An unrecognized Twitch chat command was attempted to be used.
        /// </summary>
        [EnumMember(Value = "unrecognized_cmd")]
        UnrecognizedCmd         = 27,

        // TODO: Raise separate event for this and parse relevant extra data?
        /// <summary>
        /// An unsupported chat room coomand was attempted to be used.
        /// </summary>
        [EnumMember(Value = "unsupported_chatrooms_cmd")]
        UnsupportedChatRoomsCmd = 28,
    }
}