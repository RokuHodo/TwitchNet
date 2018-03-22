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
            { "",                           NoticeType.None },
            { "already_banned",             NoticeType.AlreadyBanned },
            { "already_emote_only_off",     NoticeType.AlreadyEmoteOnlyOff },
            { "already_emote_only_on",      NoticeType.AlreadyEmoteOnlyOn },
            { "already_r9k_off",            NoticeType.AlreadyR9kOff },
            { "already_r9k_on",             NoticeType.AlreadyR9kOn },
            { "already_subs_off",           NoticeType.AlreadySubsOff },
            { "already_subs_on",            NoticeType.AlreadySubsOn },
            { "bad_host_hosting",           NoticeType.BadHostHosting },
            { "ban_success",                NoticeType.BanSuccess },
            { "bad_unban_no_ban",           NoticeType.BadUnbanNoBan },
            { "emote_only_off",             NoticeType.EmoteOnlyOff },
            { "emote_only_on",              NoticeType.EmoteOnlyOn },
            { "host_off",                   NoticeType.HostOff },
            { "host_on",                    NoticeType.HostOn },
            { "hosts_remaining",            NoticeType.HostsRemaining },
            { "msg_channel_suspended",      NoticeType.MsgChannelSuspended },
            { "msg_room_not_found",         NoticeType.MsgRoomNotFound },
            { "no_permission",              NoticeType.NoPermission },
            { "r9k_off",                    NoticeType.R9kOff },
            { "r9k_on",                     NoticeType.R9kOn },
            { "slow_off",                   NoticeType.SlowOff },
            { "slow_on",                    NoticeType.SlowOn },
            { "subs_off",                   NoticeType.SubsOff },
            { "subs_on",                    NoticeType.SubsOn },
            { "timeout_success",            NoticeType.TimeoutSuccess },
            { "unban_success",              NoticeType.UnbanSuccess },
            { "unrecognized_cmd",           NoticeType.UnrecognizedCmd },
            { "unsupported_chatrooms_cmd",  NoticeType.UnsupportedChatRoomsCmd }
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
        ToBadge(string str)
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
        /// Returns <see cref="NoticeType.None"/> otherwise.
        /// </returns>
        public static NoticeType
        ToNoticeType(string str)
        {
            NoticeType notice = NoticeType.None;
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

        #endregion                                                
    }
}
