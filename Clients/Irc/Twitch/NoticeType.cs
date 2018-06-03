namespace
TwitchNet.Clients.Irc.Twitch
{
    public enum
    NoticeType
    {
        /// <summary>
        /// Unsupported msg-id tag.
        /// </summary>
        Other = 0,

        // @msg-id = already_banned :tmi.twitch.tv NOTICE #rokuhodo_ :RokuBotto is already banned in this channel.
        /// <summary>
        /// A user was attempted to be banned that is already banned.
        /// </summary>
        AlreadyBanned,

        /// <summary>
        /// Emote only mode was attempted to be turned off but is already off.
        /// </summary>
        AlreadyEmoteOnlyOff,

        /// <summary>
        /// Emote only mode was attempted to be turned on but is already on.
        /// </summary>
        AlreadyEmoteOnlyOn,

        /// <summary>
        /// R9K mode was attempted to be turned off but is already off.
        /// </summary>
        AlreadyR9kOff,

        /// <summary>
        /// R9K mode was attempted to be turned on but is already on.
        /// </summary>
        AlreadyR9kOn,

        /// <summary>
        /// Subscriber only mode was attempted to be turned off but is already off.
        /// </summary>
        AlreadySubsOff,

        /// <summary>
        /// Subscriber only mode was attempted to be turned on but is already on.
        /// </summary>
        AlreadySubsOn,

        /// <summary>
        /// The commercial failed to start playing.
        /// </summary>
        BadCommercialError,

        // @msg-id=bad_host_hosting :tmi.twitch.tv NOTICE #rokuhodo_ :This channel is already hosting handmade_hero.
        /// <summary>
        /// Attempted to host a user that you are already hosting.
        /// </summary>
        BadHostHosting,

        // @msg-id=bad_host_rate_exceeded :tmi.twitch.tv NOTICE #rokuhodo_ :Host target cannot be changed more than 3 times every half hour.
        /// <summary>
        /// An attempt was made to host more than 'n' different users in a half hour time span.
        /// </summary>
        BadHostRateExceeded,

        // @msg-id = bad_mod_mod :tmi.twitch.tv NOTICE #rokuhodo_ :RokuBotto is already a moderator of this channel.
        /// <summary>
        /// An attempt was made to give a user moderator status, but is already a moderator.
        /// </summary>
        BadModMod,

        /// <summary>
        /// Slow mode was enabled with a duration larger than the allowed maximum.
        /// </summary>
        BadSlowDuration,

        // @msg-id = bad_unmod_mod :tmi.twitch.tv NOTICE #rokuhodo_ :RokuBotto is not a moderator of this channel.
        /// <summary>
        /// An attempt was made to remove a user's a moderator status, but is not a moderator.
        /// </summary>
        BadUnmodMod,

        /// <summary>
        /// A user was banned.
        /// </summary>
        BanSuccess,

        // @msg-id=bad_unban_no_ban :tmi.twitch.tv NOTICE #rokuhodo_ :RokuBotto is not banned from this channel.
        /// <summary>
        /// A user was attempted to be banned but is already banned.
        /// </summary>
        BadUnbanNoBan,

        // @msg-id=cmds_available :tmi.twitch.tv NOTICE #rokuhodo_ :Commands available to you in this room (use /help <command> for details): /help /w /me /disconnect /mods /color /commercial /mod /unmod /ban /unban /timeout /untimeout /slow /slowoff /r9kbeta /r9kbetaoff /emoteonly /emoteonlyoff /clear /subscribers /subscribersoff /followers /followersoff /host /unhost
        /// <summary>
        /// The command /help was use din chat with no comman specified.
        /// Lists the available chat commands that can used in a chat.
        /// </summary>
        CmdsAvailable,

        /// <summary>
        /// The display name color was changed.
        /// </summary>
        ColorChanged,

        /// <summary>
        /// Emote only mode in a room was turned off.
        /// </summary>
        EmoteOnlyOff,

        /// <summary>
        /// Emote only mode in a room was turned on.
        /// </summary>
        EmoteOnlyOn,

        /// <summary>
        /// A channel has exited host mode.
        /// </summary>
        HostOff,

        /// <summary>
        /// A channel has started hosting another user.
        /// </summary>
        HostOn,

        // @msg-id=hosts_remaining :tmi.twitch.tv NOTICE #rokuhodo_ :2 host commands remaining this half hour.
        /// <summary>
        /// A channel has started hosting another user and has 'n' number of hosts remaining.
        /// </summary>
        HostsRemaining,

        // @msg-id=invalid_user :tmi.twitch.tv NOTICE rokuhodo_ :Invalid username: jhksdfjhsfdjhgsfd
        /// <summary>
        /// The user nick provided when using a chat command was invalid or does not exist.
        /// </summary>
        InvalidUser,

        /// <summary>
        /// A user was modded.
        /// </summary>
        ModSuccess,

        /// <summary>
        /// Attempted to join a channel that was either suspended or deleted.
        /// </summary>
        MsgChannelSuspended,

        /// <summary>
        /// Attempted to join a chat room that does not exist.
        /// </summary>
        MsgRoomNotFound,

        /// <summary>
        /// Help was requested for a command, but no help is available.
        /// </summary>
        NoHelp,

        /// <summary>
        /// Attempted to use a command that the client doesn't have permission to use.
        /// </summary>
        NoPermission,

        /// <summary>
        /// R9K mode was turned off.
        /// </summary>
        R9kOff,

        /// <summary>
        /// R9K mode was turned on.
        /// </summary>
        R9kOn,

        // @msg-id=room_mods :tmi.twitch.tv NOTICE rokuhodo_ :The moderators of this channel are: dumj01, guyfromuranus, hoshinokamikagaseo, jents217, odizzle94, rokuhodo_, thewalkthroughking, usernameop
        /// <summary>
        /// The list of moderators for a room was requested.
        /// </summary>
        RoomMods,

        /// <summary>
        /// Slow mode was turned off.
        /// </summary>
        SlowOff,

        /// <summary>
        /// Slow mode was turned on.
        /// </summary>
        SlowOn,

        /// <summary>
        /// Subscriber only mode was turned off.
        /// </summary>
        SubsOff,

        /// <summary>
        /// Subscriber only mode was turned on.
        /// </summary>
        SubsOn,

        /// <summary>
        /// A user was timed out.
        /// </summary>
        TimeoutSuccess,

        /// <summary>
        /// Attempted to change the display name color using HTML color notation when it does not have Turbo or Twitch Prime.
        /// </summary>
        TurboOnlyColor,

        // @msg-id=unban_success :tmi.twitch.tv NOTICE #rokuhodo_ :RokuBotto is no longer banned from this channel.
        /// <summary>
        /// A user was unbanned or untimed out.
        /// </summary>
        UnbanSuccess,

        /// <summary>
        /// A user was unmodded.
        /// </summary>
        UnmodSuccess,

        /// <summary>
        /// An unrecognized Twitch chat command was attempted to be used or was attempted to get help on.
        /// </summary>
        UnrecognizedCmd,

        /// <summary>
        /// Help was requested for the command /color, or the syntax used with /color was incorrect.
        /// </summary>
        UsageColor,

        /// <summary>
        /// Help was requested for the command, /disconnect.
        /// </summary>
        UsageDisconnect,

        /// <summary>
        /// Help was requested for the command, /help.
        /// </summary>
        UsageHelp,

        /// <summary>
        /// Help was requested for the command, /me.
        /// </summary>
        UsageMe,

        /// <summary>
        /// Help was requested for the command, /mods.
        /// </summary>
        UsageMods,

        /// <summary>
        /// Help was requested for the command /ban, or /ban was used without a specifying a user nick.
        /// </summary>
        UsageBan,

        /// <summary>
        /// Help was requested for the command /timeout, or /timeout was used with syntactically correct parameters but may be out of the valid ranges.
        /// </summary>
        UsageTimeout,

        /// <summary>
        /// Help was requested for the command /unban, or /unban was used without a specifying a user nick.
        /// </summary>
        UsageUnban,

        /// <summary>
        /// Help was requested for the command /untimeout, or /untimeout was used without a specifying a user nick.
        /// </summary>
        UsageUntimeout,

        /// <summary>
        /// Help was requested for the command /clear, or /clear was used with additional characters after the command.
        /// </summary>
        UsageClear,

        /// <summary>
        /// Help was requested for the command /emoteonly, or /emoteonly was used with additional characters after the command.
        /// </summary>
        UsageEmoteOnlyOn,

        /// <summary>
        /// Help was requested for the command /emoteonlyoff, or /emoteonlyoff was used with additional characters after the command.
        /// </summary>
        UsageEmoteOnlyOff,

        /// <summary>
        /// Help was requested for the command /followers, or /followers was used with additional characters after the command.
        /// </summary>
        UsageFollowersOn,

        /// <summary>
        /// Help was requested for the command, /followersoff, or /followersoff was used with additional characters after the command.
        /// </summary>
        UsageFollowersOff,

        /// <summary>
        /// Help was requested for the command /r9kbeta, or /r9kbeta was used with additional characters after the command.
        /// </summary>
        UsageR9kOn,

        /// <summary>
        /// Help was requested for the command /r9kbetaoff, or /r9kbetaoff was used with additional characters after the command.
        /// </summary>
        UsageR9kOff,

        /// <summary>
        /// Help was requested for the command /slow, or /slow was used with a duration that could not be parsed.
        /// </summary>
        UsageSlowOn,

        /// <summary>
        /// Help was requested for the command /slowoff, or /slowoff was used with additional characters after the command.
        /// </summary>
        UsageSlowOff,

        /// <summary>
        /// Help was requested for the command /subscribers, or /subscribers was used with additional characters after the command.
        /// </summary>
        UsageSubsOn,

        /// <summary>
        /// Help was requested for the command /subscribersoff, or /subscribersoff was used with additional characters after the command.
        /// </summary>
        UsageSubsOff,

        /// <summary>
        /// Help was requested for the command /commercial, or /commercial was used with a length that was not valid or could not be parsed.
        /// </summary>
        UsageCommercial,

        /// <summary>
        /// Help was requested for the command /host, or /host was used without a specifying a user nick.
        /// </summary>
        UsageHost,

        /// <summary>
        /// Help was requested for the command /unhost, or /unhost was used with additional characters after the command.
        /// </summary>
        UsageUnhost,

        // @msg-id=unsupported_chatrooms_cmd :tmi.twitch.tv NOTICE #chatrooms:45947671:e0cb36e1-29b1-481a-a85f-a01bc9a36646 :The command /commercial cannot be used in a chatroom
        /// <summary>
        /// An unsupported chat room coomand was attempted to be used.
        /// </summary>
        UnsupportedChatRoomsCmd,

        /// <summary>
        /// A whisper was attempted to be sent with incorrect syntax.
        /// </summary>
        WhisperInvalidArgs,
    }
}