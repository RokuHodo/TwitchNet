// standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Enums;
using TwitchNet.Enums.Api.Videos;
using TwitchNet.Enums.Clients.Irc.Twitch;
using TwitchNet.Extensions;

namespace TwitchNet.Utilities
{
    public static class
    EnumCacheUtil
    {
        #region Caches

        private static
        Dictionary<string, BroadcasterLanguage> to_broadcaster_language_cache = new Dictionary<string, BroadcasterLanguage>
        {
            { "",       BroadcasterLanguage.None },
            { "en",     BroadcasterLanguage.En },
            { "da",     BroadcasterLanguage.Da },
            { "de",     BroadcasterLanguage.De },
            { "es",     BroadcasterLanguage.Es },
            { "fr",     BroadcasterLanguage.Fr },
            { "it",     BroadcasterLanguage.It },
            { "hu",     BroadcasterLanguage.Hu },
            { "nl",     BroadcasterLanguage.Nl },
            { "no",     BroadcasterLanguage.No },
            { "pl",     BroadcasterLanguage.Pl },
            { "pt",     BroadcasterLanguage.Pt },
            { "sk",     BroadcasterLanguage.Sk },
            { "fi",     BroadcasterLanguage.Fi },
            { "sv",     BroadcasterLanguage.Sv },
            { "vi",     BroadcasterLanguage.Vi },
            { "tr",     BroadcasterLanguage.Tr },
            { "cs",     BroadcasterLanguage.Cs },
            { "el",     BroadcasterLanguage.El },
            { "bg",     BroadcasterLanguage.Bg },
            { "ru",     BroadcasterLanguage.Ru },
            { "ar",     BroadcasterLanguage.Ar },
            { "th",     BroadcasterLanguage.Th },
            { "zh",     BroadcasterLanguage.Zh },
            { "zh-hk",  BroadcasterLanguage.Zh_Hk },
            { "ja",     BroadcasterLanguage.Ja },
            { "ko",     BroadcasterLanguage.Ko },
            { "asl",    BroadcasterLanguage.Asl },
            { "other",  BroadcasterLanguage.Other },
        };

        private static
        Dictionary<string, BadgeType> to_badge_cache = new Dictionary<string, BadgeType>
        {
            { "",               BadgeType.None },
            { "admin",          BadgeType.Admin },
            { "bits",           BadgeType.Bits },
            { "broadcaster",    BadgeType.Broadcaster },
            { "global_mod",     BadgeType.GlobalMod },
            { "moderator",      BadgeType.Moderator },
            { "subscriber",     BadgeType.Subscriber },
            { "premium",        BadgeType.Premium },
            { "turbo",          BadgeType.Turbo },
            { "staff",          BadgeType.Staff },
        };

        private static
        Dictionary<string, UserType> to_user_type_cache = new Dictionary<string, UserType>
        {
            { "",           UserType.None },
            { "mod",        UserType.Mod },
            { "global_mod", UserType.GlobalMod },
            { "admin",      UserType.Admin },
            { "staff",      UserType.Staff },
        };

        private static
        Dictionary<string, UserNoticeType> to_user_notice_type_cache = new Dictionary<string, UserNoticeType>
        {
            { "",       UserNoticeType.None },
            { "sub",    UserNoticeType.Sub },
            { "resub",  UserNoticeType.Resub },
            { "raid",   UserNoticeType.Raid },
            { "ritual", UserNoticeType.Ritual },
        };

        private static
        Dictionary<string, SubscriptionPlan> to_subscription_plan_cache = new Dictionary<string, SubscriptionPlan>
        {
            { "",       SubscriptionPlan.None },
            { "Prime",  SubscriptionPlan.Prime },
            { "1000",   SubscriptionPlan.Tier1 },
            { "2000",   SubscriptionPlan.Tier2 },
            { "3000",   SubscriptionPlan.Tier3 },
        };

        private static
        Dictionary<string, RitualType> to_ritual_type_cache = new Dictionary<string, RitualType>
        {
            { "",               RitualType.None },
            { "new_chatter",    RitualType.NewChatter },
        };

        private static
        Dictionary<string, NoticeType> to_notice_type_cache = new Dictionary<string, NoticeType>
        {
            { "",                           NoticeType.Other },
            { "already_banned",             NoticeType.AlreadyBanned },
            { "already_emote_only_off",     NoticeType.AlreadyEmoteOnlyOff },
            { "already_emote_only_on",      NoticeType.AlreadyEmoteOnlyOn },
            { "already_r9k_off",            NoticeType.AlreadyR9kOff },
            { "already_r9k_on",             NoticeType.AlreadyR9kOn },
            { "already_subs_off",           NoticeType.AlreadySubsOff },
            { "already_subs_on",            NoticeType.AlreadySubsOn },
            { "bad_commercial_error",       NoticeType.BadCommercialError },
            { "bad_host_hosting",           NoticeType.BadHostHosting },
            { "bad_host_rate_exceeded",     NoticeType.BadHostRateExceeded },
            { "bad_mod_mod",                NoticeType.BadModMod },
            { "bad_slow_duration",          NoticeType.BadSlowDuration },
            { "bad_unmod_mod",              NoticeType.BadUnmodMod},
            { "ban_success",                NoticeType.BanSuccess },
            { "bad_unban_no_ban",           NoticeType.BadUnbanNoBan },
            { "cmds_available",             NoticeType.CmdsAvailable },
            { "color_changed",              NoticeType.ColorChanged },
            { "emote_only_off",             NoticeType.EmoteOnlyOff },
            { "emote_only_on",              NoticeType.EmoteOnlyOn },
            { "host_off",                   NoticeType.HostOff },
            { "host_on",                    NoticeType.HostOn },
            { "hosts_remaining",            NoticeType.HostsRemaining },
            { "invalid_user",               NoticeType.InvalidUser },
            { "mod_success",                NoticeType.ModSuccess },
            { "msg_channel_suspended",      NoticeType.MsgChannelSuspended },
            { "msg_room_not_found",         NoticeType.MsgRoomNotFound },
            { "no_help",                    NoticeType.NoHelp },
            { "no_permission",              NoticeType.NoPermission },
            { "r9k_off",                    NoticeType.R9kOff },
            { "r9k_on",                     NoticeType.R9kOn },
            { "room_mods",                  NoticeType.RoomMods },
            { "slow_off",                   NoticeType.SlowOff },
            { "slow_on",                    NoticeType.SlowOn },
            { "subs_off",                   NoticeType.SubsOff },
            { "subs_on",                    NoticeType.SubsOn },
            { "timeout_success",            NoticeType.TimeoutSuccess },
            { "turbo_only_color",           NoticeType.TurboOnlyColor },
            { "unban_success",              NoticeType.UnbanSuccess },
            { "unmod_success",              NoticeType.UnmodSuccess },
            { "unrecognized_cmd",           NoticeType.UnrecognizedCmd },
            { "usage_color",                NoticeType.UsageColor },
            { "usage_disconnect",           NoticeType.UsageDisconnect },
            { "usage_help",                 NoticeType.UsageHelp },
            { "usage_me",                   NoticeType.UsageMe },
            { "usage_mods",                 NoticeType.UsageMods },
            { "usage_ban",                  NoticeType.UsageBan },
            { "usage_timeout",              NoticeType.UsageTimeout },
            { "usage_unban",                NoticeType.UsageUnban },
            { "usage_untimeout",            NoticeType.UsageUntimeout },
            { "usage_clear",                NoticeType.UsageClear },
            { "usage_emote_only_on",        NoticeType.UsageEmoteOnlyOn },
            { "usage_emote_only_off",       NoticeType.UsageEmoteOnlyOff },
            { "usage_followers_on",         NoticeType.UsageFollowersOn },
            { "usage_followers_off",        NoticeType.UsageFollowersOff },
            { "usage_r9k_on",               NoticeType.UsageR9kOn },
            { "usage_slow_on",              NoticeType.UsageSlowOn },
            { "usage_slow_off",             NoticeType.UsageSlowOff },
            { "usage_subs_on",              NoticeType.UsageSubsOn },
            { "usage_subs_off",             NoticeType.UsageSubsOff },
            { "usage_commercial",           NoticeType.UsageCommercial },
            { "usage_host",                 NoticeType.UsageHost },
            { "usage_unhost",               NoticeType.UsageUnhost },
            { "unsupported_chatrooms_cmd",  NoticeType.UnsupportedChatRoomsCmd },
            { "whisper_invalid_args",       NoticeType.WhisperInvalidArgs },
        };

        private static
        Dictionary<DisplayNameColor, string> from_display_name_color_cache = new Dictionary<DisplayNameColor, string>
        {
            { DisplayNameColor.Blue,        "blue" },
            { DisplayNameColor.BlueViolet,  "blueviolet" },
            { DisplayNameColor.CadetBlue,   "cadetblue" },
            { DisplayNameColor.Chocloate,   "chocloate" },
            { DisplayNameColor.Coral,       "coral" },
            { DisplayNameColor.DodgerBlue,  "dodgerblue" },
            { DisplayNameColor.FireBrick,   "firebrick" },
            { DisplayNameColor.GoldenRod,   "goldenrod" },
            { DisplayNameColor.Green,       "green" },
            { DisplayNameColor.HotPink,     "hotpink" },
            { DisplayNameColor.OrangeRed,   "orangered" },
            { DisplayNameColor.Red,         "red" },
            { DisplayNameColor.SeaGreen,    "seagreen" },
            { DisplayNameColor.SpringGreen, "springgreen" },
            { DisplayNameColor.YellowGreen, "yellowgreen" },
        };

        private static
        Dictionary<ChatCommand, string> from_chat_command_cache = new Dictionary<ChatCommand, string>
        {
            { ChatCommand.Color,            "/color" },
            { ChatCommand.Disconnect,       "/disconnect" },
            { ChatCommand.Help,             "/help" },
            { ChatCommand.Me,               "/me" },
            { ChatCommand.Mods,             "/mods" },
            { ChatCommand.Whisper,          "/w" },

            { ChatCommand.Ban,              "/ban" },
            { ChatCommand.Mod,              "/mod" },
            { ChatCommand.Timeout,          "/timeout" },
            { ChatCommand.Unban,            "/unban" },
            { ChatCommand.Unmod,            "/unmod" },
            { ChatCommand.Untimeout,        "/untimeout" },

            { ChatCommand.Clear,            "/clear" },
            { ChatCommand.EmoteOnly,        "/emoteonly" },
            { ChatCommand.EmoteOnlyOff,     "/emoteonlyoff" },
            { ChatCommand.Followers,        "/followers" },
            { ChatCommand.FollowersOff,     "/followersoff" },
            { ChatCommand.R9kBeta,          "/r9kbeta" },
            { ChatCommand.R9kBetaOff,       "/r9kbetaoff" },
            { ChatCommand.Slow,             "/slow" },
            { ChatCommand.SlowOff,          "/slowoff" },
            { ChatCommand.Subscribers,      "/subscribers" },
            { ChatCommand.SubscribersOff,   "/Subscribersoff" },

            { ChatCommand.Commercial,       "/commercial" },
            { ChatCommand.Host,             "/host" },
            { ChatCommand.Raid,             "/raid" },
            { ChatCommand.Unhost,           "/unhost" },
            { ChatCommand.Unraid,           "/unraid" },
        };

        private static
        Dictionary<string, ChatCommand> to_chat_command_cache = new Dictionary<string, ChatCommand>
        {
            { "/color",             ChatCommand.Color },
            { "/disconnect",        ChatCommand.Disconnect },
            { "/help",              ChatCommand.Help },
            { "/me",                ChatCommand.Me },
            { "/mods",              ChatCommand.Mods },
            { "/w",                 ChatCommand.Whisper },

            { "/ban",               ChatCommand.Ban },
            { "/mod",               ChatCommand.Mod },
            { "/timeout",           ChatCommand.Timeout },
            { "/unban",             ChatCommand.Unban },
            { "/unmod",             ChatCommand.Unmod },
            { "/untimeout",         ChatCommand.Untimeout },

            { "/clear",             ChatCommand.Clear },
            { "/emoteonly",         ChatCommand.EmoteOnly },
            { "/emoteonlyoff",      ChatCommand.EmoteOnlyOff },
            { "/followers",         ChatCommand.Followers },
            { "/followersoff",      ChatCommand.FollowersOff },
            { "/r9kbeta",           ChatCommand.R9kBeta },
            { "/r9kbetaoff",        ChatCommand.R9kBetaOff },
            { "/slow",              ChatCommand.Slow },
            { "/slowoff",           ChatCommand.SlowOff },
            { "/subscribers",       ChatCommand.Subscribers },
            { "/Subscribersoff",    ChatCommand.SubscribersOff },

            { "/commercial",        ChatCommand.Commercial },
            { "/host",              ChatCommand.Host },
            { "/raid",              ChatCommand.Raid },
            { "/unhost",            ChatCommand.Unhost },
            { "/unraid",            ChatCommand.Unraid },            
        };

        private static
        Dictionary<CommercialLength, string> from_commercial_length_cache = new Dictionary<CommercialLength, string>
        {
            { CommercialLength.Seconds30,   "30" },
            { CommercialLength.Seconds60,   "60" },
            { CommercialLength.Seconds90,   "90" },
            { CommercialLength.Seconds120,  "120" },
            { CommercialLength.Seconds150,  "150" },
            { CommercialLength.Seconds180,  "180" },
        };

        private static
        Dictionary<string, FollowersDurationPeriod> to_followers_duration_period = new Dictionary<string, FollowersDurationPeriod>
        {
            { "mo",         FollowersDurationPeriod.Months },
            { "month",      FollowersDurationPeriod.Months },
            { "months",     FollowersDurationPeriod.Months },
            { "w",          FollowersDurationPeriod.Weeks },
            { "week",       FollowersDurationPeriod.Weeks },
            { "weeks",      FollowersDurationPeriod.Weeks },
            { "d",          FollowersDurationPeriod.Days },
            { "day",        FollowersDurationPeriod.Days },
            { "days",       FollowersDurationPeriod.Days },
            { "h",          FollowersDurationPeriod.Hours },
            { "hour",       FollowersDurationPeriod.Hours },
            { "hours",      FollowersDurationPeriod.Hours },
            { "m",          FollowersDurationPeriod.Minutes },
            { "minute",     FollowersDurationPeriod.Minutes },
            { "minutes",    FollowersDurationPeriod.Minutes },
            { "s",          FollowersDurationPeriod.Seconds },
            { "second",     FollowersDurationPeriod.Seconds },
            { "seconds",    FollowersDurationPeriod.Seconds },
        };

        #endregion

        #region Methods

        /// <summary>
        /// Converts a string to a <see cref="BroadcasterLanguage"/> enum value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="BroadcasterLanguage"/> value if successful.
        /// Returns <see cref="BroadcasterLanguage.None"/> otherwise.
        /// </returns>
        public static BroadcasterLanguage
        ToBroadcasterLanguage(string str)
        {
            BroadcasterLanguage language = BroadcasterLanguage.None;
            if (str.IsNull())
            {
                return language;
            }

            to_broadcaster_language_cache.TryGetValue(str, out language);

            return language;
        }

        /// <summary>
        /// Converts a string to a <see cref="BadgeType"/> enum value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="BadgeType"/> value if successful.
        /// Returns <see cref="BadgeType.None"/> otherwise.
        /// </returns>
        public static BadgeType
        ToBadgeType(string str)
        {
            BadgeType badge = BadgeType.None;
            if (str.IsNull())
            {
                return badge;
            }

            to_badge_cache.TryGetValue(str, out badge);

            return badge;
        }

        /// <summary>
        /// Converts a string to an <see cref="UserType"/> enum value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="UserType"/> value if successful.
        /// Returns <see cref="UserType.None"/> otherwise.
        /// </returns>
        public static UserType
        ToUserType(string str)
        {
            UserType user_type = UserType.None;
            if (str.IsNull())
            {
                return user_type;
            }

            to_user_type_cache.TryGetValue(str, out user_type);

            return user_type;
        }

        /// <summary>
        /// Converts a string to an <see cref="UserNoticeType"/> enum value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="UserNoticeType"/> value if successful.
        /// Returns <see cref="UserNoticeType.None"/> otherwise.
        /// </returns>
        public static UserNoticeType
        ToUserNoticeType(string str)
        {
            UserNoticeType user_notice = UserNoticeType.None;
            if (str.IsNull())
            {
                return user_notice;
            }

            to_user_notice_type_cache.TryGetValue(str, out user_notice);

            return user_notice;
        }

        /// <summary>
        /// Converts a string to a <see cref="SubscriptionPlan"/> enum value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="SubscriptionPlan"/> value if successful.
        /// Returns <see cref="SubscriptionPlan.None"/> otherwise.
        /// </returns>
        public static SubscriptionPlan
        ToSubscriptionPlan(string str)
        {
            SubscriptionPlan plan = SubscriptionPlan.None;
            if (str.IsNull())
            {
                return plan;
            }

            to_subscription_plan_cache.TryGetValue(str, out plan);

            return plan;
        }

        /// <summary>
        /// Converts a string to a <see cref="RitualType"/> enum value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="RitualType"/> value if successful.
        /// Returns <see cref="RitualType.None"/> otherwise.
        /// </returns>
        public static RitualType
        ToRitualType(string str)
        {
            RitualType ritual = RitualType.None;
            if (str.IsNull())
            {
                return ritual;
            }

            to_ritual_type_cache.TryGetValue(str, out ritual);

            return ritual;
        }

        /// <summary>
        /// Converts a string to a <see cref="NoticeType"/> enum value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="NoticeType"/> value if successful.
        /// Returns <see cref="NoticeType.Other"/> otherwise.
        /// </returns>
        public static NoticeType
        ToNoticeType(string str)
        {
            NoticeType notice = NoticeType.Other;
            if (str.IsNull())
            {
                return notice;
            }

            to_notice_type_cache.TryGetValue(str, out notice);

            return notice;
        }

        /// <summary>
        /// Converts a <see cref="DisplayNameColor"/> enum value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding string value if successful.
        /// Returns string value using <code>ToString() otherwise.</code>.
        /// </returns>
        public static string
        FromDisplayNameColor(DisplayNameColor value)
        {
            if(!from_display_name_color_cache.TryGetValue(value, out string color))
            {
                color = value.ToString();
            }

            return color;
        }

        /// <summary>
        /// Converts a <see cref="ChatCommand"/> enum value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding string value if successful.
        /// Returns string value using <code>ToString() otherwise.</code>.
        /// </returns>
        public static string
        FromChatCommand(ChatCommand value)
        {
            if (!from_chat_command_cache.TryGetValue(value, out string command))
            {
                command = value.ToString();
            }

            return command;
        }

        /// <summary>
        /// Converts a string to a <see cref="ChatCommand"/> enum value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="ChatCommand"/> value if successful.
        /// Returns <see cref="ChatCommand.Other"/> otherwise.
        /// </returns>
        public static ChatCommand
        ToChatCommand(string str)
        {
            ChatCommand command = ChatCommand.Other;
            if (str.IsNull())
            {
                return command;
            }

            to_chat_command_cache.TryGetValue(str, out command);

            return command;
        }

        /// <summary>
        /// Converts a <see cref="CommercialLength"/> enum value to a string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>
        /// Returns the corresponding string value if successful.
        /// Returns string value using <code>ToString() otherwise.</code>.
        /// </returns>
        public static string
        FromCommercialLength(CommercialLength value)
        {
            if (!from_commercial_length_cache.TryGetValue(value, out string command))
            {
                command = ((int)value).ToString();
            }

            return command;
        }

        /// <summary>
        /// Converts a string to a <see cref="FollowersDurationPeriod"/> enum value.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>
        /// Returns the corresponding <see cref="FollowersDurationPeriod"/> value if successful.
        /// Returns <see cref="FollowersDurationPeriod.Seconds"/> otherwise.
        /// </returns>
        public static FollowersDurationPeriod
        ToFollowersDurationPeriod(string str)
        {
            FollowersDurationPeriod period = FollowersDurationPeriod.Seconds;
            if (str.IsNull())
            {
                return period;
            }

            to_followers_duration_period.TryGetValue(str, out period);

            return period;
        }

        #endregion                                                
    }
}
