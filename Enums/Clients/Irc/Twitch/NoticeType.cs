// standard namespaces
using System.Runtime.Serialization;

namespace TwitchNet.Enums.Clients.Irc.Twitch
{
    public enum
    NoticeType
    {
        /// <summary>
        /// The msg-id tag could not be parsed.
        /// </summary>
        [EnumMember(Value = "")]
        Other                    = 0,

        // TODO: Raise separate event for this and parse relevant extra data?
        /// <summary>
        /// A user was attempted to be banned that is already banned.
        /// </summary>
        [EnumMember(Value = "already_banned")]
        AlreadyBanned,

        /// <summary>
        /// Emote only mode was attempted to be turned off but is already off.
        /// </summary>
        [EnumMember(Value = "already_emote_only_off")]
        AlreadyEmoteOnlyOff,

        /// <summary>
        /// Emote only mode was attempted to be turned on but is already on.
        /// </summary>
        [EnumMember(Value = "already_emote_only_on")]
        AlreadyEmoteOnlyOn,

        /// <summary>
        /// R9K mode was attempted to be turned off but is already off.
        /// </summary>
        [EnumMember(Value = "already_r9k_off")]
        AlreadyR9kOff,

        /// <summary>
        /// R9K mode was attempted to be turned on but is already on.
        /// </summary>
        [EnumMember(Value = "already_r9k_on")]
        AlreadyR9kOn,

        /// <summary>
        /// Subscriber only mode was attempted to be turned off but is already off.
        /// </summary>
        [EnumMember(Value = "already_subs_off")]
        AlreadySubsOff,

        /// <summary>
        /// Subscriber only mode was attempted to be turned on but is already on.
        /// </summary>
        [EnumMember(Value = "already_subs_on")]
        AlreadySubsOn,

        // TODO: Add to enum cache
        /// <summary>
        /// The commercial failed to start playing.
        /// </summary>
        [EnumMember(Value = "bad_commercial_error")]
        BadCommercialError,

        // TODO: Raise separate event for this and parse relevant extra data?
        /// <summary>
        /// Attempted to host a user that you are already hosting.
        /// </summary>
        [EnumMember(Value = "bad_host_hosting")]
        BadHostHosting,

        // TODO: Add to enum cache
        /// <summary>
        /// An attempt was made to host more than 3 different users in a half hour time span.
        /// </summary>
        [EnumMember(Value = "bad_host_rate_exceeded")]
        BadHostRateExceeded,

        // TODO: Add to enum cache
        /// <summary>
        /// Slow mode was enabled with a duration larger than allowed maximum.
        /// </summary>
        [EnumMember(Value = "bad_slow_duration")]
        BadSlowDuration,

        // TODO: Raise separate event for this and parse relevant extra data?
        /// <summary>
        /// A user was banned.
        /// </summary>
        [EnumMember(Value = "ban_success")]
        BanSuccess,

        // TODO: Raise separate event for this and parse relevant extra data?
        /// <summary>
        /// A user was attempted to be banned but is already banned.
        /// </summary>
        [EnumMember(Value = "bad_unban_no_ban")]
        BadUnbanNoBan,

        // TODO: Raise separate event for this and parse relevant extra data?
        // TODO: Add to enum cache.
        // @msg-id=cmds_available :tmi.twitch.tv NOTICE #rokuhodo_ :Commands available to you in this room (use /help <command> for details): /help /w /me /disconnect /mods /color /commercial /mod /unmod /ban /unban /timeout /untimeout /slow /slowoff /r9kbeta /r9kbetaoff /emoteonly /emoteonlyoff /clear /subscribers /subscribersoff /followers /followersoff /host /unhost
        /// <summary>
        /// The command /help was use din chat with no comman specified.
        /// Lists the available chat commands that can used in a chat.
        /// </summary>
        [EnumMember(Value = "cmds_available")]
        CmdsAvailable,

        // TODO: Add to enum cache.
        /// <summary>
        /// The display name color was changed.
        /// </summary>
        [EnumMember(Value = "color_changed")]
        ColorChanged,

        /// <summary>
        /// Emote only mode in a room was turned off.
        /// </summary>
        [EnumMember(Value = "emote_only_off")]
        EmoteOnlyOff,

        /// <summary>
        /// Emote only mode in a room was turned on.
        /// </summary>
        [EnumMember(Value = "emote_only_on")]
        EmoteOnlyOn,

        /// <summary>
        /// A channel has exited host mode.
        /// </summary>
        [EnumMember(Value = "host_off")]
        HostOff,

        /// <summary>
        /// A channel has started hosting another user.
        /// </summary>
        [EnumMember(Value = "host_on")]
        HostOn,

        // TODO: Raise separate event for this and parse relevant extra data?
        /// <summary>
        /// A channel has started hosting another user and has 'n' number of hosts remaining.
        /// </summary>
        [EnumMember(Value = "hosts_remaining")]
        HostsRemaining,

        // TODO: Raise separate event for this and parse relevant extra data?
        // TODO: Add to enum cache
        // @msg-id=invalid_user :tmi.twitch.tv NOTICE rokuhodo_ :Invalid username: jhksdfjhsfdjhgsfd
        /// <summary>
        /// The user nick provided when using a chat command was invalid or does not exist.
        /// </summary>
        [EnumMember(Value = "invalid_user")]
        InvalidUser,

        /// <summary>
        /// Attempted to join a channel that was either suspended or deleted.
        /// </summary>
        [EnumMember(Value = "msg_channel_suspended")]
        MsgChannelSuspended,

        /// <summary>
        /// Attempted to join a chat room that does not exist.
        /// </summary>
        [EnumMember(Value = "msg_room_not_found")]
        MsgRoomNotFound,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for a command, but no help is available.
        /// </summary>
        [EnumMember(Value = "no_help")]
        NoHelp,

        /// <summary>
        /// Attempted to use a command that the client doesn't have permission to use.
        /// </summary>
        [EnumMember(Value = "no_permission")]
        NoPermission,

        /// <summary>
        /// R9K mode was turned off.
        /// </summary>
        [EnumMember(Value = "r9k_off")]
        R9kOff,

        /// <summary>
        /// R9K mode was turned on.
        /// </summary>
        [EnumMember(Value = "r9k_on")]
        R9kOn,

        // TODO: Raise separate event for this and parse relevant extra data?
        // TODO: Add to enum cache.
        // @msg-id=room_mods :tmi.twitch.tv NOTICE rokuhodo_ :The moderators of this channel are: dumj01, guyfromuranus, hoshinokamikagaseo, jents217, odizzle94, rokuhodo_, thewalkthroughking, usernameop
        /// <summary>
        /// The list of moderators for a room was requested.
        /// </summary>
        [EnumMember(Value = "room_mods")]
        RoomMods,

        /// <summary>
        /// Slow mode was turned off.
        /// </summary>
        [EnumMember(Value = "slow_off")]
        SlowOff,

        /// <summary>
        /// Slow mode was turned on.
        /// </summary>
        [EnumMember(Value = "slow_on")]
        SlowOn,

        /// <summary>
        /// Subscriber only mode was turned off.
        /// </summary>
        [EnumMember(Value = "subs_off")]
        SubsOff,

        /// <summary>
        /// Subscriber only mode was turned on.
        /// </summary>
        [EnumMember(Value = "subs_on")]
        SubsOn,

        /// <summary>
        /// A user was timed out.
        /// </summary>
        [EnumMember(Value = "timeout_success")]
        TimeoutSuccess,

        // TODO: Add to enum cache
        /// <summary>
        /// Attempted to change the display name color using HTML color notation when it does not have Turbo or Twitch Prime.
        /// </summary>
        [EnumMember(Value = "turbo_only_color")]
        TurboOnlyColor,

        // TODO: Raise separate event for this and parse relevant extra data?
        /// <summary>
        /// A user was unbanned.
        /// </summary>
        [EnumMember(Value = "unban_success")]
        UnbanSuccess,

        // TODO: Raise separate event for this and parse relevant extra data?
        /// <summary>
        /// An unrecognized Twitch chat command was attempted to be used or was attempted to get help on.
        /// </summary>
        [EnumMember(Value = "unrecognized_cmd")]
        UnrecognizedCmd,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command /color, or the syntax used with /color was incorrect.
        /// </summary>
        [EnumMember(Value = "usage_color")]
        UsageColor,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command, /disconnect.
        /// </summary>
        [EnumMember(Value = "usage_disconnect")]
        UsageDisconnect,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command, /help.
        /// </summary>
        [EnumMember(Value = "usage_help")]
        UsageHelp,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command, /me.
        /// </summary>
        [EnumMember(Value = "usage_me")]
        UsageMe,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command, /mods.
        /// </summary>
        [EnumMember(Value = "usage_mods")]
        UsageMods,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command /ban, or /ban was used without a specifying a user nick.
        /// </summary>
        [EnumMember(Value = "usage_ban")]
        UsageBan,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command /timeout, or /timeout was used with syntactically correct parameters but may be out of the valid ranges.
        /// </summary>
        [EnumMember(Value = "usage_timeout")]
        UsageTimeout,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command /unban, or /unban was used without a specifying a user nick.
        /// </summary>
        [EnumMember(Value = "usage_unban")]
        UsageUnban,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command /untimeout, or /untimeout was used without a specifying a user nick.
        /// </summary>
        [EnumMember(Value = "usage_untimeout")]
        UsageUntimeout,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command /clear, or /clear was used with additional characters after the command.
        /// </summary>
        [EnumMember(Value = "usage_clear")]
        UsageClear,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command /emoteonly, or /emoteonly was used with additional characters after the command.
        /// </summary>
        [EnumMember(Value = "usage_emote_only_on")]
        UsageEmoteOnlyOn,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command /emoteonlyoff, or /emoteonlyoff was used with additional characters after the command.
        /// </summary>
        [EnumMember(Value = "usage_emote_only_off")]
        UsageEmoteOnlyOff,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command /followers, or /followers was used with additional characters after the command.
        /// </summary>
        [EnumMember(Value = "usage_followers_on")]
        UsageFollowersOn,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command, /followersoff, or /followersoff was used with additional characters after the command.
        /// </summary>
        [EnumMember(Value = "usage_followers_off")]
        UsageFollowersOff,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command /r9kbeta, or /r9kbeta was used with additional characters after the command.
        /// </summary>
        [EnumMember(Value = "usage_r9k_on")]
        UsageR9kOn,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command /r9kbetaoff, or /r9kbetaoff was used with additional characters after the command.
        /// </summary>
        [EnumMember(Value = "usage_r9k_off")]
        UsageR9kOff,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command /slow, or /slow was used with a duration that could not be parsed.
        /// </summary>
        [EnumMember(Value = "usage_slow_on")]
        UsageSlowOn,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command /slowoff, or /slowoff was used with additional characters after the command.
        /// </summary>
        [EnumMember(Value = "usage_slow_off")]
        UsageSlowOff,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command /subscribers, or /subscribers was used with additional characters after the command.
        /// </summary>
        [EnumMember(Value = "usage_subs_on")]
        UsageSubsOn,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command /subscribersoff, or /subscribersoff was used with additional characters after the command.
        /// </summary>
        [EnumMember(Value = "usage_subs_off")]
        UsageSubsOff,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command /commercial, or /commercial was used with a length that was not valid or could not be parsed.
        /// </summary>
        [EnumMember(Value = "usage_commercial")]
        UsageCommercial,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command /host, or /host was used without a specifying a user nick.
        /// </summary>
        [EnumMember(Value = "usage_host")]
        UsageHost,

        // TODO: Add to enum cache
        /// <summary>
        /// Help was requested for the command /unhost, or /unhost was used with additional characters after the command.
        /// </summary>
        [EnumMember(Value = "usage_unhost")]
        UsageUnhost,

        // TODO: Raise separate event for this and parse relevant extra data?
        /// <summary>
        /// An unsupported chat room coomand was attempted to be used.
        /// </summary>
        [EnumMember(Value = "unsupported_chatrooms_cmd")]
        UnsupportedChatRoomsCmd,

        // TODO: Add to enum cache
        /// <summary>
        /// A whisper was attempted to be sent with incorrect syntax.
        /// </summary>
        [EnumMember(Value = "whisper_invalid_args")]
        WhisperInvalidArgs,
    }
}