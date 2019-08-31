// standard namespaces
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Extensions;
using TwitchNet.Clients.Irc;
using TwitchNet.Utilities;

namespace
TwitchNet.Clients.Irc
{
    // -------------------------------------------------------------------------------------------------------------
    //
    // - This file contains all IRC messages that are natively supported and documented in the RFC 1459 Specification.
    //   RFC 1459 Spec: https://tools.ietf.org/html/rfc1459.html
    // - Any message that is specific to Twitch can be found in its own file, TwitchIrcClient.EventArgs.
    //
    // - However, these messages do contain extra parsed members that pertain only to Twitch. These members are separated 
    //   clearly from the native IRC spec members witin each data structure.
    //
    // -------------------------------------------------------------------------------------------------------------

    #region Command: NOTICE

    public class
    NoticeEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// Whether or not IRC tags were sent with the message.
        /// </summary>
        public bool tags_exist { get; private set; }

        /// <summary>
        /// <para>The converted IRC tags attached to the message.</para>
        /// <para>Set to null if no tags were sent with the message.</para>
        /// </summary>
        [ValidateMember(Check.TagsMissing)]
        public NoticeTags tags { get; protected set; }

        /// <summary>
        /// <para>The channel notice was sent in.</para>
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The notice message sent by the server.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string body { get; protected set; }        

        /// <summary>
        /// Creates a new instance of the <see cref="NoticeEventArgs"/> class.
        /// </summary>
        /// <param name="message">The IRC message to parse.</param>
        public NoticeEventArgs(in IrcMessage message) : base(message)
        {
            tags_exist = message.tags_exist;
            if (tags_exist)
            {
                tags = new NoticeTags(message);
            }

            if (message.parameters.Length > 0)
            {
                channel = message.parameters[0];
            }

            body = message.trailing;
        }
    }

    public class
    NoticeTags
    {
        /// <summary>
        /// The id that describes the notice from the server.
        /// <para>Set to <see cref="UserNoticeType.Other"/> if the id could not be parsed.</para>
        /// </summary>
        [IrcTag("msg-id")]
        public NoticeType msg_id { get; protected set; }

        public
        NoticeTags(in IrcMessage message)
        {
            msg_id = TwitchIrcUtil.Tags.ToNoticeType(message, "msg-id");
        }
    }

    public enum
    NoticeType
    {
        /// <summary>
        /// Unsupported msg-id tag.
        /// </summary>
        [EnumMember(Value = "")]
        Other = 0,

        // @msg-id = already_banned :tmi.twitch.tv NOTICE #rokuhodo_ :RokuBotto is already banned in this channel.
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

        /// <summary>
        /// The commercial failed to start playing.
        /// </summary>
        [EnumMember(Value = "bad_commercial_error")]
        BadCommercialError,

        // @msg-id=bad_host_hosting :tmi.twitch.tv NOTICE #rokuhodo_ :This channel is already hosting handmade_hero.
        /// <summary>
        /// Attempted to host a user that you are already hosting.
        /// </summary>
        [EnumMember(Value = "bad_host_hosting")]
        BadHostHosting,

        // @msg-id=bad_host_rate_exceeded :tmi.twitch.tv NOTICE #rokuhodo_ :Host target cannot be changed more than 3 times every half hour.
        /// <summary>
        /// An attempt was made to host more than 'n' different users in a half hour time span.
        /// </summary>
        [EnumMember(Value = "bad_host_rate_exceeded")]
        BadHostRateExceeded,

        // @msg-id = bad_mod_mod :tmi.twitch.tv NOTICE #rokuhodo_ :RokuBotto is already a moderator of this channel.
        /// <summary>
        /// An attempt was made to give a user moderator status, but is already a moderator.
        /// </summary>
        [EnumMember(Value = "bad_mod_mod")]
        BadModMod,

        /// <summary>
        /// Slow mode was enabled with a duration larger than the allowed maximum.
        /// </summary>
        [EnumMember(Value = "bad_slow_duration")]
        BadSlowDuration,

        // @msg-id = bad_unmod_mod :tmi.twitch.tv NOTICE #rokuhodo_ :RokuBotto is not a moderator of this channel.
        /// <summary>
        /// An attempt was made to remove a user's a moderator status, but is not a moderator.
        /// </summary>
        [EnumMember(Value = "bad_unmod_mod")]
        BadUnmodMod,

        /// <summary>
        /// A user was banned.
        /// </summary>
        [EnumMember(Value = "ban_success")]
        BanSuccess,

        // @msg-id=bad_unban_no_ban :tmi.twitch.tv NOTICE #rokuhodo_ :RokuBotto is not banned from this channel.
        /// <summary>
        /// A user was attempted to be banned but is already banned.
        /// </summary>
        [EnumMember(Value = "bad_unban_no_ban")]
        BadUnbanNoBan,

        // @msg-id=cmds_available :tmi.twitch.tv NOTICE #rokuhodo_ :Commands available to you in this room (use /help <command> for details): /help /w /me /disconnect /mods /color /commercial /mod /unmod /ban /unban /timeout /untimeout /slow /slowoff /r9kbeta /r9kbetaoff /emoteonly /emoteonlyoff /clear /subscribers /subscribersoff /followers /followersoff /host /unhost
        /// <summary>
        /// The command /help was use din chat with no comman specified.
        /// Lists the available chat commands that can used in a chat.
        /// </summary>
        [EnumMember(Value = "cmds_available")]
        CmdsAvailable,

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

        // @msg-id=hosts_remaining :tmi.twitch.tv NOTICE #rokuhodo_ :2 host commands remaining this half hour.
        /// <summary>
        /// A channel has started hosting another user and has 'n' number of hosts remaining.
        /// </summary>
        [EnumMember(Value = "hosts_remaining")]
        HostsRemaining,

        // @msg-id=invalid_user :tmi.twitch.tv NOTICE rokuhodo_ :Invalid username: jhksdfjhsfdjhgsfd
        /// <summary>
        /// The user nick provided when using a chat command was invalid or does not exist.
        /// </summary>
        [EnumMember(Value = "invalid_user")]
        InvalidUser,

        /// <summary>
        /// A user was modded.
        /// </summary>
        [EnumMember(Value = "mod_success")]
        ModSuccess,

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

        /// <summary>
        /// Attempted to change the display name color using HTML color notation when it does not have Turbo or Twitch Prime.
        /// </summary>
        [EnumMember(Value = "turbo_only_color")]
        TurboOnlyColor,

        // @msg-id=unban_success :tmi.twitch.tv NOTICE #rokuhodo_ :RokuBotto is no longer banned from this channel.
        /// <summary>
        /// A user was unbanned or untimed out.
        /// </summary>
        [EnumMember(Value = "unban_success")]
        UnbanSuccess,

        /// <summary>
        /// A user was unmodded.
        /// </summary>
        [EnumMember(Value = "unmod_success")]
        UnmodSuccess,

        /// <summary>
        /// An unrecognized Twitch chat command was attempted to be used or was attempted to get help on.
        /// </summary>
        [EnumMember(Value = "unrecognized_cmd")]
        UnrecognizedCmd,

        /// <summary>
        /// Help was requested for the command /color, or the syntax used with /color was incorrect.
        /// </summary>
        [EnumMember(Value = "usage_color")]
        UsageColor,

        /// <summary>
        /// Help was requested for the command, /disconnect.
        /// </summary>
        [EnumMember(Value = "usage_disconnect")]
        UsageDisconnect,

        /// <summary>
        /// Help was requested for the command, /help.
        /// </summary>
        [EnumMember(Value = "usage_help")]
        UsageHelp,

        /// <summary>
        /// Help was requested for the command, /me.
        /// </summary>
        [EnumMember(Value = "usage_me")]
        UsageMe,

        /// <summary>
        /// Help was requested for the command, /mods.
        /// </summary>
        [EnumMember(Value = "usage_mods")]
        UsageMods,

        /// <summary>
        /// Help was requested for the command /ban, or /ban was used without a specifying a user nick.
        /// </summary>
        [EnumMember(Value = "usage_ban")]
        UsageBan,

        /// <summary>
        /// Help was requested for the command /timeout, or /timeout was used with syntactically correct parameters but may be out of the valid ranges.
        /// </summary>
        [EnumMember(Value = "usage_timeout")]
        UsageTimeout,

        /// <summary>
        /// Help was requested for the command /unban, or /unban was used without a specifying a user nick.
        /// </summary>
        [EnumMember(Value = "usage_unban")]
        UsageUnban,

        /// <summary>
        /// Help was requested for the command /untimeout, or /untimeout was used without a specifying a user nick.
        /// </summary>
        [EnumMember(Value = "usage_untimeout")]
        UsageUntimeout,

        /// <summary>
        /// Help was requested for the command /clear, or /clear was used with additional characters after the command.
        /// </summary>
        [EnumMember(Value = "usage_clear")]
        UsageClear,

        /// <summary>
        /// Help was requested for the command /emoteonly, or /emoteonly was used with additional characters after the command.
        /// </summary>
        [EnumMember(Value = "usage_emote_only_on")]
        UsageEmoteOnlyOn,

        /// <summary>
        /// Help was requested for the command /emoteonlyoff, or /emoteonlyoff was used with additional characters after the command.
        /// </summary>
        [EnumMember(Value = "usage_emote_only_off")]
        UsageEmoteOnlyOff,

        /// <summary>
        /// Help was requested for the command /followers, or /followers was used with additional characters after the command.
        /// </summary>
        [EnumMember(Value = "usage_followers_on")]
        UsageFollowersOn,

        /// <summary>
        /// Help was requested for the command, /followersoff, or /followersoff was used with additional characters after the command.
        /// </summary>
        [EnumMember(Value = "usage_followers_off")]
        UsageFollowersOff,

        /// <summary>
        /// Help was requested for the command /r9kbeta, or /r9kbeta was used with additional characters after the command.
        /// </summary>
        [EnumMember(Value = "usage_r9k_on")]
        UsageR9kOn,

        /// <summary>
        /// Help was requested for the command /r9kbetaoff, or /r9kbetaoff was used with additional characters after the command.
        /// </summary>
        [EnumMember(Value = "usage_r9k_off")]
        UsageR9kOff,

        /// <summary>
        /// Help was requested for the command /slow, or /slow was used with a duration that could not be parsed.
        /// </summary>
        [EnumMember(Value = "usage_slow_on")]
        UsageSlowOn,

        /// <summary>
        /// Help was requested for the command /slowoff, or /slowoff was used with additional characters after the command.
        /// </summary>
        [EnumMember(Value = "usage_slow_off")]
        UsageSlowOff,

        /// <summary>
        /// Help was requested for the command /subscribers, or /subscribers was used with additional characters after the command.
        /// </summary>
        [EnumMember(Value = "usage_subs_on")]
        UsageSubsOn,

        /// <summary>
        /// Help was requested for the command /subscribersoff, or /subscribersoff was used with additional characters after the command.
        /// </summary>
        [EnumMember(Value = "usage_subs_off")]
        UsageSubsOff,

        /// <summary>
        /// Help was requested for the command /commercial, or /commercial was used with a length that was not valid or could not be parsed.
        /// </summary>
        [EnumMember(Value = "usage_commercial")]
        UsageCommercial,

        /// <summary>
        /// Help was requested for the command /host, or /host was used without a specifying a user nick.
        /// </summary>
        [EnumMember(Value = "usage_host")]
        UsageHost,

        /// <summary>
        /// Help was requested for the command /unhost, or /unhost was used with additional characters after the command.
        /// </summary>
        [EnumMember(Value = "usage_unhost")]
        UsageUnhost,

        // @msg-id=unsupported_chatrooms_cmd :tmi.twitch.tv NOTICE #chatrooms:45947671:e0cb36e1-29b1-481a-a85f-a01bc9a36646 :The command /commercial cannot be used in a chatroom
        /// <summary>
        /// An unsupported chat room coomand was attempted to be used.
        /// </summary>
        [EnumMember(Value = "unsupported_chatrooms_cmd")]
        UnsupportedChatRoomsCmd,

        /// <summary>
        /// A whisper was attempted to be sent with incorrect syntax.
        /// </summary>
        [EnumMember(Value = "whisper_invalid_args")]
        WhisperInvalidArgs,
    }

    public class
    AlreadyBannedEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The user who was attempted to be banned or timed out, but is already banned or timed out.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user_nick { get; protected set; }

        public
        AlreadyBannedEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;
            user_nick = args.body.TextBefore(' ');
        }
    }

    public class
    BadHostHostingEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The user that was attempted to be hosted, but is already being hosted.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user_nick { get; protected set; }

        public
        BadHostHostingEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            int index = args.body.LastIndexOf(' ');
            if (index != 1)
            {
                user_nick = args.body.TextBetween(' ', '.', index);
            }
        }
    }

    public class
    BadHostRateExceededEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The maximum number of users that can be hosted in half an hour.
        /// Set to -1 if the value could not be parsed.
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, -1)]
        public int host_rate_limit { get; protected set; }

        public
        BadHostRateExceededEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            host_rate_limit = Int32.TryParse(args.body.TextBetween("than ", " times"), out int _host_rate_limit) ? _host_rate_limit : -1;
        }
    }

    public class
    BadModModEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The user who was attempted to be modded but is modded.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user_nick { get; protected set; }

        public
        BadModModEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            user_nick = args.body.TextBefore(' ');
        }
    }

    public class
    BadUnmodModEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The user who was attempted to be unmodded but is not modded.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user_nick { get; protected set; }

        public
        BadUnmodModEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            user_nick = args.body.TextBefore(' ');
        }
    }

    public class
    BadUnbanNoBanEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The user who was attempted to be unbanned or untimed out, but is not banned or timed out.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user_nick { get; protected set; }

        public
        BadUnbanNoBanEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            user_nick = args.body.TextBefore(' ');
        }
    }

    public class
    CmdsAvailableEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The commands that can be used in chat.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public ChatCommand[] commands { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="CmdsAvailableEventArgs"/> class.
        /// </summary>
        /// <param name="args">The event arguments to parse.</param>
        public CmdsAvailableEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            string _commands = args.body.TextAfter(':').Trim(' ');
            string[] array = _commands.Split(' ');

            List<ChatCommand> list = new List<ChatCommand>();
            foreach (string element in array)
            {
                EnumUtil.TryParse(element, out ChatCommand command);
                list.Add(command);
            }
            commands = list.ToArray();
        }
    }

    public class
    HostsRemainingEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The remaining number of hosts that can be used until it resets.
        /// Set to -1 if the value could not be parsed.
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, -1)]
        public int remaining { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="HostsRemainingEventArgs"/> class.
        /// </summary>
        /// <param name="args">The event arguments to parse.</param>
        public HostsRemainingEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            remaining = Int32.TryParse(args.body.TextBefore(' '), out int _hosts_remaining) ? _hosts_remaining : -1;
        }
    }

    public class
    InvalidUserEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The invalid user nick
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user_nick { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="InvalidUserEventArgs"/> class.
        /// </summary>
        /// <param name="args">The event arguments to parse.</param>
        public InvalidUserEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            user_nick = args.body.TextAfter(':').TrimStart(' ');
        }
    }

    public class
    RoomModsEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The room's moderators.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string[] user_nicks { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="RoomModsEventArgs"/> class.
        /// </summary>
        /// <param name="args">The event arguments to parse.</param>
        public RoomModsEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            string nicks = args.body.TextAfter(':').Trim(' ');
            user_nicks = nicks.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
        }
    }

    public class
    UnbanSuccessEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The user who was unbanned.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user_nick { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="UnbanSuccessEventArgs"/> class.
        /// </summary>
        /// <param name="args">The event arguments to parse.</param>
        public UnbanSuccessEventArgs(NoticeEventArgs args) : base(args.irc_message)
        {
            channel = args.channel;

            user_nick = args.body.TextBefore(' ');
        }
    }

    public class
    UnsupportedChatRoomCmdEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The channel that the NOTICE was sent to.
        /// </summary>
        [ValidateMember(Check.IsValid)]

        public string channel { get; protected set; }

        /// <summary>
        /// The id of the user who the chat room belongs to.
        /// </summary>
        [ValidateMember(Check.IsValid)]

        public string channel_user_id { get; protected set; }

        /// <summary>
        /// The unique UUID of the chat room.
        /// </summary>
        [ValidateMember(Check.IsValid)]

        public string channel_uuid { get; protected set; }

        /// <summary>
        /// The unsupported chat command that was attempted to be used in a chat room.
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, ChatCommand.Other)]
        public ChatCommand command { get; protected set; }

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

            EnumUtil.TryParse(args.body.TextBetween("command ", " cannot"), out ChatCommand _command);
            command = _command;
        }
    }

    #endregion

    public class
    DataEventArgs : EventArgs
    {
        /// <summary>
        /// The byte data.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public byte[] data { get; protected set; }

        /// <summary>
        /// The UTF-8 encoded data.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string message { get; protected set; }

        public DataEventArgs(byte[] data, string message)
        {
            this.data = data;

            this.message = message;
        }
    }

    public class
    IrcMessageEventArgs : EventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        public IrcMessage irc_message { get; protected set; }

        public IrcMessageEventArgs(in IrcMessage message)
        {
            irc_message = message;
        }
    }

    public class
    NumericReplyEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// The IRC client nick.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string client { get; protected set; }

        public NumericReplyEventArgs(in IrcMessage message) : base(message)
        {
            if (message.parameters.Length > 0)
            {
                client = message.parameters[0];
            }
        }
    }

    public class
    NamReplyEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// The character that specifies if the IRC channel is public, secret, or private.
        /// </summary>
        [ValidateMember(Check.IsNotNullOrDefault)]
        public char status { get; protected set; }

        /// <summary>
        /// The IRC client nick.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string client { get; protected set; }

        /// <summary>
        /// The IRC channel that the clients have joined.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// A partial or complete list of client nicks that have joined the IRC channel.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string[] names { get; protected set; }

        /// <summary>
        /// Whether or not the IRC channel is public.
        /// The channel is public if the status is equal to '='.
        /// </summary>
        [ValidateMember(Check.IsNotNull)]
        public bool is_public { get; protected set; }

        /// <summary>
        /// Whether or not the IRC channel is secret.
        /// The channel is secret if the status is equal to '@'.
        /// </summary>
        [ValidateMember(Check.IsNotNull)]
        public bool is_secret { get; protected set; }

        /// <summary>
        /// Whether or not the IRC channel is private.
        /// The channel is private if the status is equal to '*'.
        /// </summary>
        [ValidateMember(Check.IsNotNull)]
        public bool is_private { get; protected set; }

        public NamReplyEventArgs(in IrcMessage message) : base(message, 2)
        {
            // Native IRC aprsing
            if (message.parameters.Length > 2)
            {
                client = message.parameters[0];

                status = message.parameters[1][0];
                if (status == '=')
                {
                    is_public = true;
                }
                else if (status == '@')
                {
                    is_secret = true;
                }
                else if (status == '*')
                {
                    is_private = true;
                }

                channel = message.parameters[2];
            }

            names = message.trailing.Split(' ');
        }
    }

    public class
    EndOfNamesEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// The IRC client nick.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string client { get; protected set; }

        /// <summary>
        /// The IRC channel that the clients have joined.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The complete list of client nicks that have joined the IRC channel.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string[] names { get; protected set; }

        public EndOfNamesEventArgs(in IrcMessage message, Dictionary<string, List<string>> names) : base(message, 1)
        {
            if (message.parameters.Length > 1)
            {
                client = message.parameters[0];
                channel = message.parameters[1];
            }

            this.names = names[channel].ToArray();
        }
    }

    public class
    MotdEventArgs : EventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        public IrcMessage irc_message { get; protected set; }

        /// <summary>
        /// The IRC client nick.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string client { get; protected set; }

        /// <summary>
        /// The IRC server's message of the day.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string motd { get; protected set; }

        public MotdEventArgs(in IrcMessage message)
        {
            irc_message = message;

            if (message.parameters.Length > 0)
            {
                client = message.parameters[0];
            }

            motd = message.trailing;
        }
    }

    public class
    UnknownCommandEventArgs : EventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        public IrcMessage irc_message { get; protected set; }

        /// <summary>
        /// The IRC client nick.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string client { get; protected set; }

        /// <summary>
        /// The unsupported IRC command.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string command { get; protected set; }

        /// <summary>
        /// The error description.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string description { get; protected set; }

        public UnknownCommandEventArgs(in IrcMessage message)
        {
            irc_message = message;


            if (message.parameters.Length > 1)
            {
                client = message.parameters[0];
                command = message.parameters[1];
            }

            description = message.trailing;
        }
    }

    public class
    JoinEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        public IrcMessage irc_message { get; protected set; }

        /// <summary>
        /// The nick of the client who joined the channel.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string nick { get; protected set; }

        /// <summary>
        /// The IRC channel the client has joined.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        public JoinEventArgs(in IrcMessage message) : base(message)
        {
            irc_message = message;

            nick = message.server_or_nick;

            if (message.parameters.Length > 0)
            {
                channel = message.parameters[0];
            }
		}
    }

    public class
    PartEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        public IrcMessage irc_message { get; protected set; }

        /// <summary>
        /// The nick of the client who left the channel.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string nick { get; protected set; }

        /// <summary>
        /// The IRC channel the client has left.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        public PartEventArgs(in IrcMessage message) : base(message)
        {
            irc_message = message;

            nick = message.server_or_nick;

            if (message.parameters.Length > 0)
            {
                channel = message.parameters[0];
            }
        }		
    }

    public class
    ChannelModeEventArgs : EventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        public IrcMessage irc_message { get; protected set; }

        /// <summary>
        /// Denotes the whether the mode was added '+', or removed '-'.
        /// </summary>
        [ValidateMember(Check.IsNotNullOrDefault)]
        public char modifier { get; protected set; }

        /// <summary>
        /// The change that occured to either the channel or the user.
        /// </summary>
        [ValidateMember(Check.IsNotNullOrDefault)]
        public char mode { get; protected set; }

        /// <summary>
        /// A combination of the 'modifier' and the 'mode'.
        /// The complete change that occured.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string mode_set { get; protected set; }

        /// <summary>
        /// The IRC channel whose mode was changed.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        // TODO: Change to an array since this could be none or all 3 arguments,

        /// <summary>
        /// Arguments, if any, associated with the mode change.
        /// These inckude a ban mask, limit, and/or an IRC user.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string arguments { get; protected set; }

        public ChannelModeEventArgs(in IrcMessage message)
        {
            irc_message = message;

            if (message.parameters.Length > 2)
            {
                channel = message.parameters[0];

                // This assumes only one argument after the mode set.
                // This is fine for Twitch, but change this to an array because it *could* be up to 3 parameters after the mode set.
                arguments = message.parameters[2];

                mode_set = message.parameters[1];
                if (message.parameters[1].Length > 1)
                {
                    modifier = message.parameters[1][0];
                    mode = message.parameters[1][1];
                }
            }
        }
    }

    public class
    ChannelOperatorEventArgs : EventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        public IrcMessage irc_message { get; protected set; }

        /// <summary>
        /// Whether or not the user is an operator in the IRC channel.
        /// </summary>
        [ValidateMember(Check.IsNotNull)]
        public bool is_operator { get; protected set; }

        /// <summary>
        /// The user nick that gained or lost operator status.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user { get; protected set; }

        /// <summary>
        /// The IRC channel where the user's operator status changed.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        public ChannelOperatorEventArgs(ChannelModeEventArgs args)
        {
            irc_message = args.irc_message;

            is_operator = args.modifier == '+' ? true : false;
            user = args.arguments;
            channel = args.channel;
        }
    }

    public class
    UserModeEventArgs : EventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        public IrcMessage irc_message { get; protected set; }

        /// <summary>
        /// The name of the user whose mode was changed.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string nick { get; protected set; }

        /// <summary>
        /// Denotes the whether the mode was added '+', or removed '-'.
        /// </summary>
        [ValidateMember(Check.IsNotNullOrDefault)]
        public char modifier { get; protected set; }

        /// <summary>
        /// The change that occured to either the channel or the user.
        /// </summary>
        [ValidateMember(Check.IsNotNullOrDefault)]
        public char mode { get; protected set; }

        /// <summary>
        /// A combination of the 'modifier' and the 'mode_set'.
        /// The complete change that occured.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string mode_set { get; protected set; }

        public UserModeEventArgs(in IrcMessage message)
        {
            irc_message = message;

            if (message.parameters.Length > 1)
            {
                nick = message.parameters[0];

                mode_set = message.parameters[1];
                if (message.parameters[1].Length > 1)
                {
                    modifier = message.parameters[1][0];
                    mode = message.parameters[1][1];
                }
            }
        }
    }

    public class
    PrivmsgEventArgs : ChatRoomSupportedMessageEventArgs
    {
        /// <summary>
        /// The parsed IRC message.
        /// </summary>
        public IrcMessage irc_message { get; protected set; }

        // Native RFC 1459 propperties

        /// <summary>
        /// Whether or not IRC tags were sent with the message.
        /// </summary>
        public bool tags_exist { get; private set; }

        /// <summary>
        /// The nick of the client who sent the message.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string nick { get; protected set; }

        /// <summary>
        /// The IRC user who sent the message.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string user { get; protected set; }

        /// <summary>
        /// The IRC host name.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string host { get; protected set; }

        /// <summary>
        /// The IRC channel the message was sent in.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel { get; protected set; }

        /// <summary>
        /// The body of the message.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string body { get; protected set; }

        // Twitch specific propertties

        /// <summary>
        /// Whether or not the message started with '\u0001ACTION', i.e., the '/me' command was used with the message.
        /// </summary>
        public bool action { get; protected set; }

        /// <summary>
        /// <para>The converted IRC tags attached to the message.</para>
        /// <para>Set to null if the message source was not from a chat room.</para>
        /// </summary>
        [ValidateMember]
        [ValidateMember(Check.TagsMissing)]
        // [ValidateMember(Check.TagsExtra)]
        public ChatRoomPrivmsgTags tags_chat_room { get; protected set; }

        /// <summary>
        /// <para>The converted IRC tags attached to the message.</para>
        /// <para>Set to null if the message source was not from a stream chat.</para>
        /// </summary>
        [ValidateMember]
        [ValidateMember(Check.TagsMissing)]
        // [ValidateMember(Check.TagsExtra)]
        public StreamChatPrivmsgTags tags_stream_chat { get; protected set; }

        public PrivmsgEventArgs(in IrcMessage message) : base(message)
        {
            irc_message = message;

            // RFC 1459 parsing
            tags_exist = irc_message.tags_exist;

            nick = message.server_or_nick;
            user = message.user;
            host = message.host;

            channel = message.parameters[0];

            body = message.trailing;            

            // Twitch specific parsing
            action = body.StartsWith(TwitchIrcUtil.ACTION_PREFIX) ? true : false;
            if (action)
            {
                body = body.TextBetween(TwitchIrcUtil.ACTION_PREFIX, TwitchIrcUtil.ACTION_SUFFIX).Trim();
            }

            if (tags_exist)
            {
                if (source == MessageSource.ChatRoom)
                {
                    tags_chat_room = new ChatRoomPrivmsgTags(irc_message);
                }
                else if (source == MessageSource.StreamChat)
                {
                    tags_stream_chat = new StreamChatPrivmsgTags(irc_message);
                }
            }
        }
    }

    public class
    ChatRoomPrivmsgTags
    {
        /// <summary>
        /// Whether or not the sender is a moderator.
        /// </summary>
        [IrcTag("mod")]
        public bool mod { get; protected set; }

        /// <summary>
        /// Whether or not the sender is subscribed to the channel.
        /// </summary>
        [Obsolete("This tag is obsolte and can be deleted at any time. Use the 'badges' tag to look for this information instraad")]
        [IrcTag("subscriber")]
        public bool subscriber { get; protected set; }

        /// <summary>
        /// Whether or not the sender has Twitch turbo.
        /// </summary>
        [Obsolete("This tag has been deprecated and can be deleted at any time. Use the 'badges' tag to look for this information instead")]
        [IrcTag("turbo")]
        public bool turbo { get; protected set; }

        /// <summary>
        /// Whether or not the body of the message only contains emotes.
        /// </summary>
        [IrcTag("emote-only")]
        public bool emote_only { get; protected set; }

        /// <summary>
        /// The unique message ID.
        /// </summary>
        [IrcTag("id")]
        public string id { get; protected set; }

        /// <summary>
        /// <para>The display name of the sender.</para>
        /// <para>Set to an empty string if the sender never explicitly set their display name.</para>
        /// </summary>
        [IrcTag("display-name")]
        public string display_name { get; protected set; }

        /// <summary>
        /// The user ID of the sender.
        /// </summary>
        [IrcTag("user-id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// The user ID of the channel the message was sent in.
        /// </summary>
        [IrcTag("room-id")]
        public string room_id { get; protected set; }

        /// <summary>
        /// <para>The sender's user type</para>
        /// <para>Set to <see cref="UserType.None"/> if the sender has no elevated privileges.</para>
        /// </summary>
        [Obsolete("This tag has been deprecated and can be deleted at any time. Use the 'badges' tag to look for this information instead")]
        [IrcTag("user-type")]
        public UserType user_type { get; protected set; }

        /// <summary>
        /// <para>The color of the sender's display name.</para>
        /// <para>Set to <see cref="Color.Empty"/> if the sender never explicitly set their display name color.</para>
        /// </summary>
        [IrcTag("color")]
        public Color color { get; protected set; }

        /// <summary>
        /// The time the message was sent.
        /// </summary>
        [IrcTag("tmi-sent-ts")]
        public DateTime tmi_sent_ts { get; protected set; }

        /// <summary>
        /// <para>The chat badges that the sender has, if any.</para>
        /// <para>Set to an empty array if the sender has no chat badges.</para>
        /// </summary>
        [ValidateMember]
        [IrcTag("badges")]
        public Badge[] badges { get; protected set; }

        /// <summary>
        /// <para>
        /// Detailed information on badge tenure.
        /// Currently, this only returns information for the subscriber badge.
        /// </para>
        /// <para>Set to an empty array if the sender has no chat badges.</para>
        /// </summary>
        [ValidateMember]
        [IrcTag("badge-info")]
        public BadgeInfo[] badge_info { get; protected set; }

        /// <summary>
        /// <para>The emotes the sender used in the message, if any.</para>
        /// <para>Set to an empty array if the sender did not use any emotes in the message.</para>
        /// </summary>
        [ValidateMember]
        [IrcTag("emotes")]
        public Emote[] emotes { get; protected set; }

        public ChatRoomPrivmsgTags(IrcMessage message)
        {
            mod = TwitchIrcUtil.Tags.ToBool(message, "mod");
            subscriber = TwitchIrcUtil.Tags.ToBool(message, "subscriber");
            turbo = TwitchIrcUtil.Tags.ToBool(message, "turbo");
            emote_only = TwitchIrcUtil.Tags.ToBool(message, "emote-only");

            id = TwitchIrcUtil.Tags.ToString(message, "id");
            display_name = TwitchIrcUtil.Tags.ToString(message, "display-name");
            user_id = TwitchIrcUtil.Tags.ToString(message, "user-id");
            room_id = TwitchIrcUtil.Tags.ToString(message, "room-id");

            user_type = TwitchIrcUtil.Tags.ToUserType(message, "user-type");

            color = TwitchIrcUtil.Tags.FromtHtml(message, "color");
            tmi_sent_ts = TwitchIrcUtil.Tags.FromUnixEpochMilliseconds(message, "tmi-sent-ts");

            badges = TwitchIrcUtil.Tags.ToBadges(message, "badges");
            badge_info = TwitchIrcUtil.Tags.ToBadgeInfo(message, "badge-info");
            emotes = TwitchIrcUtil.Tags.ToEmotes(message, "emotes");
        }
    }

    public class
    StreamChatPrivmsgTags
    {
        /// <summary>
        /// <para>The amount of bits the sender cheered, if any.</para>
        /// <para>Set to 0 if the sender did not cheer.</para>
        /// </summary>
        [IrcTag("bits")]
        public uint bits { get; protected set; }

        /// <summary>
        /// Whether or not the sender is a moderator.
        /// </summary>
        [IrcTag("mod")]
        public bool mod { get; protected set; }

        /// <summary>
        /// Whether or not the sender is subscribed to the channel.
        /// </summary>
        [Obsolete("This tag is obsolte and can be deleted at any time. Use the 'badges' tag to look for this information instraad")]
        [IrcTag("subscriber")]
        public bool subscriber { get; protected set; }

        /// <summary>
        /// Whether or not the sender has Twitch turbo.
        /// </summary>
        [Obsolete("This tag has been deprecated and can be deleted at any time. Use the 'badges' tag to look for this information instead")]
        [IrcTag("turbo")]
        public bool turbo { get; protected set; }

        /// <summary>
        /// Whether or not the body of the message only contains emotes.
        /// </summary>
        [IrcTag("emote-only")]
        public bool emote_only { get; protected set; }

        /// <summary>
        /// The unique message ID.
        /// </summary>
        [IrcTag("id")]
        public string id { get; protected set; }

        /// <summary>
        /// <para>The display name of the sender.</para>
        /// <para>Set to an empty string if the sender never explicitly set their display name.</para>
        /// </summary>
        [IrcTag("display-name")]
        public string display_name { get; protected set; }

        /// <summary>
        /// The user ID of the sender.
        /// </summary>
        [IrcTag("user-id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// The user ID of the channel the message was sent in.
        /// </summary>
        [IrcTag("room-id")]
        public string room_id { get; protected set; }

        /// <summary>
        /// <para>The sender's user type</para>
        /// <para>Set to <see cref="UserType.None"/> if the sender has no elevated privileges.</para>
        /// </summary>
        [Obsolete("This tag has been deprecated and can be deleted at any time. Use the 'badges' tag to look for this information instead")]
        [IrcTag("user-type")]
        public UserType user_type { get; protected set; }

        /// <summary>
        /// <para>The color of the sender's display name.</para>
        /// <para>Set to <see cref="Color.Empty"/> if the sender never explicitly set their display name color.</para>
        /// </summary>
        [IrcTag("color")]
        public Color color { get; protected set; }

        /// <summary>
        /// The time the message was sent.
        /// </summary>
        [IrcTag("tmi-sent-ts")]
        public DateTime tmi_sent_ts { get; protected set; }

        /// <summary>
        /// <para>The chat badges that the sender has, if any.</para>
        /// <para>Set to an empty array if the sender has no chat badges.</para>
        /// </summary>
        [ValidateMember]
        [IrcTag("badges")]
        public Badge[] badges { get; protected set; }

        /// <summary>
        /// <para>
        /// Detailed information on badge tenure.
        /// Currently, this only returns information for the subscriber badge.
        /// </para>
        /// <para>Set to an empty array if the sender has no chat badges.</para>
        /// </summary>
        [ValidateMember]
        [IrcTag("badge-info")]
        public BadgeInfo[] badge_info { get; protected set; }

        /// <summary>
        /// <para>The emotes the sender used in the message, if any.</para>
        /// <para>Set to an empty array if the sender did not use any emotes in the message.</para>
        /// </summary>
        [ValidateMember]
        [IrcTag("emotes")]
        public Emote[] emotes { get; protected set; }

        public StreamChatPrivmsgTags(IrcMessage message)
        {
            bits = TwitchIrcUtil.Tags.ToUInt32(message, "bits");

            mod = TwitchIrcUtil.Tags.ToBool(message, "mod");
            subscriber = TwitchIrcUtil.Tags.ToBool(message, "subscriber");
            turbo = TwitchIrcUtil.Tags.ToBool(message, "turbo");
            emote_only = TwitchIrcUtil.Tags.ToBool(message, "emote-only");

            id = TwitchIrcUtil.Tags.ToString(message, "id");
            display_name = TwitchIrcUtil.Tags.ToString(message, "display-name");
            user_id = TwitchIrcUtil.Tags.ToString(message, "user-id");
            room_id = TwitchIrcUtil.Tags.ToString(message, "room-id");

            user_type = TwitchIrcUtil.Tags.ToUserType(message, "user-type");

            color = TwitchIrcUtil.Tags.FromtHtml(message, "color");
            tmi_sent_ts = TwitchIrcUtil.Tags.FromUnixEpochMilliseconds(message, "tmi-sent-ts");

            badges = TwitchIrcUtil.Tags.ToBadges(message, "badges");
            badge_info = TwitchIrcUtil.Tags.ToBadgeInfo(message, "badge-info");
            emotes = TwitchIrcUtil.Tags.ToEmotes(message, "emotes");
        }
    }

    public class
    Badge
    {
        /// <summary>
        /// <para>The badge verison.</para>
        /// <para>Set to -1 when <see cref="type"/> is equal to <see cref="BadgeType.Other"/>.</para>
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, -1)]
        public int version { get; protected set; }

        /// <summary>
        /// <para>The badge type.</para>
        /// <para>Set to <see cref="BadgeType.Other"/> if no supported badge type is found.</para>
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, BadgeType.Other)]
        public BadgeType type { get; protected set; }

        public Badge(string pair)
        {
            char separator = '/';

            if (!Int32.TryParse(pair.TextAfter(separator), out int _version))
            {
                _version = -1;
            }
            version = _version;

            EnumUtil.TryParse(pair.TextBefore('/'), out BadgeType _type);
            type = _type;
        }
    }

    public class
    BadgeInfo
    {
        /// <summary>
        /// <para>How many months the user has been subscribed.</para>
        /// <para>Set to -1 when <see cref="type"/> is equal to <see cref="BadgeType.Other"/>.</para>
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, -1)]
        public int tenure { get; protected set; }

        /// <summary>
        /// <para>
        /// The badge type.
        /// Currently, only <see cref="BadgeType.Subscriber"/> is the only valid value.
        /// </para>
        /// <para>Set to <see cref="BadgeType.Other"/> if no supported badge type is found.</para>
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, BadgeType.Other)]
        public BadgeType type { get; protected set; }

        public BadgeInfo(string pair)
        {
            char separator = '/';

            if (!Int32.TryParse(pair.TextAfter(separator), out int _tenure))
            {
                _tenure = -1;
            }
            tenure = _tenure;

            EnumUtil.TryParse(pair.TextBefore('/'), out BadgeType _type);
            type = _type;
        }
    }

    public enum
    BadgeType
    {
        #region Default badge

        /// <summary>
        /// Unsupported or unknown badge type.
        /// </summary>
        [EnumMember(Value = "")]
        Other = 0,

        #endregion  

        #region Normal badges

        /// <summary>
        /// Admin badge.
        /// </summary>
        [EnumMember(Value = "admin")]
        Admin,

        /// <summary>
        /// Bits badge.
        /// </summary>
        [EnumMember(Value = "bits")]
        Bits,

        /// <summary>
        /// Bits charity badge.
        /// </summary>
        [EnumMember(Value = "bits-charity")]
        BitsCharity,

        /// <summary>
        /// Broadcaster badge.
        /// </summary>
        [EnumMember(Value = "broadcaster")]
        Broadcaster,

        /// <summary>
        /// Global mod badge.
        /// </summary>
        [EnumMember(Value = "global_mod")]
        GlobalMod,

        /// <summary>
        /// Moderator badge.
        /// </summary>
        [EnumMember(Value = "moderator")]
        Moderator,

        /// <summary>
        /// Subscriber badge.
        /// </summary>
        [EnumMember(Value = "subscriber")]
        Subscriber,

        /// <summary>
        /// Staff badge.
        /// </summary>
        [EnumMember(Value = "staff")]
        Staff,

        /// <summary>
        /// Trwitch prime badge.
        /// </summary>
        [EnumMember(Value = "premium")]
        Premium,

        /// <summary>
        /// Twitch turbo badge.
        /// </summary>
        [EnumMember(Value = "turbo")]
        Turbo,

        /// <summary>
        /// Twitch partner badge.
        /// </summary>
        [EnumMember(Value = "partner")]
        Partner,

        #endregion

        #region Other badges

        /// <summary>
        /// Twitch partner badge.
        /// </summary>
        [EnumMember(Value = "sub-gifter")]
        SubGifter,

        /// <summary>
        /// Clip champ badge.
        /// </summary>
        [EnumMember(Value = "clip-champ")]
        ClipChamp,

        /// <summary>
        /// Clip champ badge.
        /// </summary>
        [EnumMember(Value = "twitchcon2017")]
        Twitchcon2017,

        /// <summary>
        /// Clip champ badge.
        /// </summary>
        [EnumMember(Value = "overwatch-league-insider_1")]
        OverwatchLeagueInsider,

        #endregion        
    }

    public class
    Emote
    {
        /// <summary>
        /// The emote ID.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string id { get; protected set; }

        /// <summary>
        /// The character index range(s) in the message where the emote was used.
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public EmoteRange[] ranges { get; protected set; }

        public Emote(string pair)
        {
            if (pair.IsValid())
            {
                id = pair.TextBefore(':');

                List<EmoteRange> ranges_list = new List<EmoteRange>();

                string[] _ranges = pair.TextAfter(':').Split(',');
                foreach (string _range in _ranges)
                {
                    EmoteRange range = new EmoteRange(_range);
                    ranges_list.Add(range);
                }

                ranges = ranges_list.ToArray();
            }
        }
    }

    public class
    EmoteRange
    {
        /// <summary>
        /// <para>The index in the message where the first emote character is located.</para>
        /// <para>Set to -1 if the index could not be parsed.</para>
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, -1)]
        public int index_start { get; protected set; }

        /// <summary>
        /// <para>The index in the message where the last emote character is located.</para>
        /// <para>Set to -1 if the index could not be parsed.</para>
        /// </summary>
        [ValidateMember(Check.IsNotEqualTo, -1)]
        public int index_end { get; protected set; }

        public EmoteRange(string range_pair)
        {
            if (!Int32.TryParse(range_pair.TextBefore('-'), out int _index_start))
            {
                _index_start = -1;
            }
            index_start = _index_start;

            if (!Int32.TryParse(range_pair.TextAfter('-'), out int _index_end))
            {
                _index_end = -1;
            }
            index_end = _index_end;
        }
    }

    public class
    ChatRoomSupportedMessageEventArgs : IrcMessageEventArgs
    {
        /// <summary>
        /// Where the message was sent from.
        /// </summary>
        public MessageSource source { get; private set; }

        /// <summary>
        /// <para>The ID of the user who owns the chat room.</para>
        /// <para>Set to an empty string if the message source was not from a chat room.</para>
        /// </summary>
        [ValidateMember(Check.IsValid)]
        public string channel_user_id { get; protected set; }

        /// <summary>
        /// <para>The UUID of the chat room.</para>
        /// <para>Set to an empty string if the message source was not from a chat room.</para>
        /// </summary>
        [ValidateMember(Check.RegexIsMatch, TwitchIrcUtil.REGEX_PATTERN_UUID)]
        public string channel_uuid { get; protected set; }

        public ChatRoomSupportedMessageEventArgs(in IrcMessage message, uint channel_index = 0) : base(message)
        {
            source = TwitchIrcUtil.GetMessageSource(message.parameters[channel_index]);

            channel_user_id = string.Empty;
            channel_uuid = string.Empty;
            if (source == MessageSource.ChatRoom)
            {
                channel_user_id = message.parameters[channel_index].TextBetween(':', ':');

                int index = message.parameters[channel_index].LastIndexOf(':');
                if (index != -1)
                {
                    channel_uuid = message.parameters[channel_index].TextAfter(':', index);
                }
            }
        }
    }        
}