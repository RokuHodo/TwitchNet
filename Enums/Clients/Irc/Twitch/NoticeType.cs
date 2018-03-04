// standard namespaces
using System.Runtime.Serialization;

namespace TwitchNet.Enums.Clients.Irc.Twitch
{
    public enum
    NoticeType
    {
        //TODO: Test for undocumented values, mostly due to inproper use of chat functions

        /// <summary>
        /// The msg-id tag could not be parsed.
        /// </summary>
        [EnumMember(Value = "")]
        None                = 0,

        // TODO: Raise separate event for this and parse relevant extra data
        /// <summary>
        /// A user was attempted to be banned that is already banned.
        /// </summary>
        [EnumMember(Value = "already_banned")]
        AlreadyBanned       = 1,

        /// <summary>
        /// Emote only mode was attempted to be turned off but is already off.
        /// </summary>
        [EnumMember(Value = "already_emote_only_off")]
        AlreadyEmoteOnlyOff = 2,

        /// <summary>
        /// Emote only mode was attempted to be turned on but is already on.
        /// </summary>
        [EnumMember(Value = "already_emote_only_on")]
        AlreadyEmoteOnlyOn  = 3,

        /// <summary>
        /// R9K mode was attempted to be turned off but is already off.
        /// </summary>
        [EnumMember(Value = "already_r9k_off")]
        AlreadyR9kOff       = 4,

        /// <summary>
        /// R9K mode was attempted to be turned on but is already on.
        /// </summary>
        [EnumMember(Value = "already_r9k_on")]
        AlreadyR9kOn        = 5,

        /// <summary>
        /// Subscriber only mode was attempted to be turned off but is already off.
        /// </summary>
        [EnumMember(Value = "already_subs_off")]
        AlreadySubsOff      = 6,

        /// <summary>
        /// Subscriber only mode was attempted to be turned on but is already on.
        /// </summary>
        [EnumMember(Value = "already_subs_on")]
        AlreadySubsOn       = 7,

        // TODO: Raise separate event for this and parse relevant extra data
        /// <summary>
        /// Attempted to host a user that you are already hosting.
        /// </summary>
        [EnumMember(Value = "bad_host_hosting")]
        BadHostHosting      = 8,

        // TODO: Raise separate event for this and parse relevant extra data
        /// <summary>
        /// A user was attempted to be banned but is already banned.
        /// </summary>
        [EnumMember(Value = "bad_unban_no_ban")]
        BadUnbanNoBan       = 9,

        // TODO: Raise separate event for this and parse relevant extra data
        /// <summary>
        /// A user was banned.
        /// </summary>
        [EnumMember(Value = "ban_success")]
        BanSuccess          = 10,

        /// <summary>
        /// Emote only mode in a room was turned off.
        /// </summary>
        [EnumMember(Value = "emote_only_off")]
        EmoteOnlyOff        = 11,

        /// <summary>
        /// Emote only mode in a room was turned on.
        /// </summary>
        [EnumMember(Value = "emote_only_on")]
        EmoteOnlyOn         = 12,

        /// <summary>
        /// A channel has exited host mode.
        /// </summary>
        [EnumMember(Value = "host_off")]
        HostOff             = 13,

        // TODO: Raise separate event for this and parse relevant extra data?
        /// <summary>
        /// A channel has started hosting another user.
        /// </summary>
        [EnumMember(Value = "host_on")]
        HostOn              = 14,

        // TODO: Raise separate event for this and parse relevant extra data
        /// <summary>
        /// A channel has started hosting another user and has 'n' number of hosts remaining.
        /// </summary>
        [EnumMember(Value = "hosts_remaining")]
        HostsRemaining      = 15,

        /// <summary>
        /// The client has attempted to join a channel that was either suspended or deleted.
        /// </summary>
        [EnumMember(Value = "msg_channel_suspended")]
        MsgChannelSuspended = 16,

        /// <summary>
        /// R9K mode was turned off.
        /// </summary>
        [EnumMember(Value = "r9k_off")]
        R9kOff              = 17,

        /// <summary>
        /// R9K mode was turned on.
        /// </summary>
        [EnumMember(Value = "r9k_on")]
        R9kOn               = 18,

        /// <summary>
        /// Slow mode was turned off.
        /// </summary>
        [EnumMember(Value = "slow_off")]
        SlowOff             = 19,

        // TODO: Raise separate event for this and parse relevant extra data?
        /// <summary>
        /// Slow mode was turned on.
        /// </summary>
        [EnumMember(Value = "slow_on")]
        SlowOn              = 20,

        /// <summary>
        /// Subscriber only mode was turned off.
        /// </summary>
        [EnumMember(Value = "subs_off")]
        SubsOff             = 21,

        /// <summary>
        /// Subscriber only mode was turned on.
        /// </summary>
        [EnumMember(Value = "subs_on")]
        SubsOn              = 22,

        // TODO: Raise separate event for this and parse relevant extra data
        /// <summary>
        /// A user was timed out.
        /// </summary>
        [EnumMember(Value = "timeout_success")]
        TimeoutSuccess      = 23,

        // TODO: Raise separate event for this and parse relevant extra data
        /// <summary>
        /// A user was unbanned.
        /// </summary>
        [EnumMember(Value = "unban_success")]
        UnbanSuccess        = 24,

        // TODO: Raise separate event for this and parse relevant extra data
        /// <summary>
        /// A unrecognized Twitch chat command was attempted to be used.
        /// </summary>
        [EnumMember(Value = "unrecognized_cmd")]
        UnrecognizedCmd     = 25,
    }
}
