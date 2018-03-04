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
        /// <summary>
        /// Converts a string to a Broadcaster Language enum value.
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

            if (!str.IsValid())
            {
                return language;
            }

            switch (str)
            {
                case "en":
                {
                    language = BroadcasterLanguage.En;
                }
                break;

                case "da":
                {
                    language = BroadcasterLanguage.Da;
                }
                break;

                case "de":
                {
                    language = BroadcasterLanguage.De;
                }
                break;

                case "es":
                {
                    language = BroadcasterLanguage.Es;
                }
                break;

                case "fr":
                {
                    language = BroadcasterLanguage.Fr;
                }
                break;

                case "it":
                {
                    language = BroadcasterLanguage.It;
                }
                break;

                case "hu":
                {
                    language = BroadcasterLanguage.Hu;
                }
                break;

                case "nl":
                {
                    language = BroadcasterLanguage.Nl;
                }
                break;

                case "no":
                {
                    language = BroadcasterLanguage.No;
                }
                break;

                case "pl":
                {
                    language = BroadcasterLanguage.Pl;
                }
                break;

                case "pt":
                {
                    language = BroadcasterLanguage.Pt;
                }
                break;

                case "sk":
                {
                    language = BroadcasterLanguage.Sk;
                }
                break;

                case "fi":
                {
                    language = BroadcasterLanguage.Fi;
                }
                break;

                case "sv":
                {
                    language = BroadcasterLanguage.Sv;
                }
                break;

                case "vi":
                {
                    language = BroadcasterLanguage.Vi;
                }
                break;

                case "tr":
                {
                    language = BroadcasterLanguage.Tr;
                }
                break;

                case "cs":
                {
                    language = BroadcasterLanguage.Cs;
                }
                break;

                case "el":
                {
                    language = BroadcasterLanguage.El;
                }
                break;

                case "bg":
                {
                    language = BroadcasterLanguage.Bg;
                }
                break;

                case "ru":
                {
                    language = BroadcasterLanguage.Ru;
                }
                break;

                case "ar":
                {
                    language = BroadcasterLanguage.Ar;
                }
                break;

                case "th":
                {
                    language = BroadcasterLanguage.Th;
                }
                break;

                case "zh":
                {
                    language = BroadcasterLanguage.Zh;
                }
                break;

                case "zh-hk":
                {
                    language = BroadcasterLanguage.Zh_Hk;
                }
                break;

                case "ja":
                {
                    language = BroadcasterLanguage.Ja;
                }
                break;

                case "ko":
                {
                    language = BroadcasterLanguage.Ko;
                }
                break;

                case "asl":
                {
                    language = BroadcasterLanguage.Asl;
                }
                break;

                case "other":
                {
                    language = BroadcasterLanguage.Other;
                }
                break;

                case "":
                default:
                {
                    language = BroadcasterLanguage.None;
                }
                break;
            }

            return language;
        }

        /// <summary>
        /// Converts a string to a Badge enum value.
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

            if (!str.IsValid())
            {
                return badge;
            }

            switch (str)
            {
                case "admin":
                {
                    badge = BadgeType.Admin;
                }
                break;

                case "bits":
                {
                    badge = BadgeType.Bits;
                }
                break;

                case "broadcaster":
                {
                    badge = BadgeType.Broadcaster;
                }
                break;

                case "global_mod":
                {
                    badge = BadgeType.GlobalMod;
                }
                break;

                case "moderator":
                {
                    badge = BadgeType.Moderator;
                }
                break;

                case "subscriber":
                {
                    badge = BadgeType.Subscriber;
                }
                break;

                case "staff":
                {
                    badge = BadgeType.Staff;
                }
                break;

                case "premium":
                {
                    badge = BadgeType.Premium;
                }
                break;

                case "turbo":
                {
                    badge = BadgeType.Turbo;
                }
                break;

                case "":
                default:
                {
                    badge = BadgeType.None;
                }
                break;
            }

            return badge;
        }

        /// <summary>
        /// Converts a string to a User Type enum value.
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

            if (!str.IsValid())
            {
                return user_type;
            }

            switch (str)
            {
                case "mod":
                {
                    user_type = UserType.Mod;
                }
                break;

                case "global_mod":
                {
                    user_type = UserType.GlobalMod;
                }
                break;

                case "admin":
                {
                    user_type = UserType.Admin;
                }
                break;

                case "staff":
                {
                    user_type = UserType.Staff;
                }
                break;

                case "":
                default:
                {
                    user_type = UserType.None;
                }
                break;
            }

            return user_type;
        }

        /// <summary>
        /// Converts a string to a user notice type enum value.
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

            if (!str.IsValid())
            {
                return user_notice;
            }

            switch (str)
            {
                case "sub":
                {
                    user_notice = UserNoticeType.Sub;
                }
                break;

                case "resub":
                {
                    user_notice = UserNoticeType.Resub;
                }
                break;

                case "raid":
                {
                    user_notice = UserNoticeType.Raid;
                }
                break;

                case "ritual":
                {
                    user_notice = UserNoticeType.Ritual;
                }
                break;

                case "":
                default:
                {
                    user_notice = UserNoticeType.None;
                }
                break;
            }

            return user_notice;
        }

        public static SubscriptionPlan
        ToSubscriptionPlan(string str)
        {
            SubscriptionPlan plan = SubscriptionPlan.None;

            if (!str.IsValid())
            {
                return plan;
            }

            switch (str)
            {
                case "Prime":
                {
                    plan = SubscriptionPlan.Prime;
                }
                break;

                case "1000":
                {
                    plan = SubscriptionPlan.Tier1;
                }
                break;

                case "2000":
                {
                    plan = SubscriptionPlan.Tier2;
                }
                break;

                case "3000":
                {
                    plan = SubscriptionPlan.Tier3;
                }
                break;
            }

            return plan;
        }

        public static RitualType
        ToRitualType(string str)
        {
            RitualType ritual_type = RitualType.None;

            if (!str.IsValid())
            {
                return ritual_type;
            }

            switch (str)
            {
                case "new_chatter":
                {
                    ritual_type = RitualType.NewChatter;
                }
                break;
            }

            return ritual_type;
        }

        public static NoticeType
        ToNoticeType(string str)
        {
            NoticeType notice_type = NoticeType.None;

            if (!str.IsValid())
            {
                return notice_type;
            }

            switch (str)
            {
                case "already_banned":
                {
                    notice_type = NoticeType.AlreadyBanned;
                }
                break;

                case "already_emote_only_off":
                {
                    notice_type = NoticeType.AlreadyEmoteOnlyOff;
                }
                break;

                case "already_emote_only_on":
                {
                    notice_type = NoticeType.AlreadyEmoteOnlyOn;
                }
                break;

                case "already_r9k_off":
                {
                    notice_type = NoticeType.AlreadyR9kOff;
                }
                break;

                case "already_r9k_on":
                {
                    notice_type = NoticeType.AlreadyR9kOn;
                }
                break;

                case "already_subs_off":
                {
                    notice_type = NoticeType.AlreadySubsOff;
                }
                break;

                case "already_subs_on":
                {
                    notice_type = NoticeType.AlreadySubsOn;
                }
                break;

                case "bad_host_hosting":
                {
                    notice_type = NoticeType.BadHostHosting;
                }
                break;

                case "bad_unban_no_ban":
                {
                    notice_type = NoticeType.BadUnbanNoBan;
                }
                break;

                case "ban_success":
                {
                    notice_type = NoticeType.BanSuccess;
                }
                break;

                case "emote_only_off":
                {
                    notice_type = NoticeType.EmoteOnlyOff;
                }
                break;

                case "emote_only_on":
                {
                    notice_type = NoticeType.EmoteOnlyOn;
                }
                break;

                case "host_off":
                {
                    notice_type = NoticeType.HostOff;
                }
                break;

                case "host_on":
                {
                    notice_type = NoticeType.HostOn;
                }
                break;

                case "hosts_remaining":
                {
                    notice_type = NoticeType.HostsRemaining;
                }
                break;

                case "msg_channel_suspended":
                {
                    notice_type = NoticeType.MsgChannelSuspended;
                }
                break;

                case "r9k_off":
                {
                    notice_type = NoticeType.R9kOff;
                }
                break;

                case "r9k_on":
                {
                    notice_type = NoticeType.R9kOn;
                }
                break;

                case "slow_off":
                {
                    notice_type = NoticeType.SlowOff;
                }
                break;

                case "slow_on":
                {
                    notice_type = NoticeType.SlowOn;
                }
                break;

                case "subs_off":
                {
                    notice_type = NoticeType.SubsOff;
                }
                break;

                case "subs_on":
                {
                    notice_type = NoticeType.SubsOn;
                }
                break;

                case "timeout_success":
                {
                    notice_type = NoticeType.TimeoutSuccess;
                }
                break;

                case "unban_success":
                {
                    notice_type = NoticeType.UnbanSuccess;
                }
                break;

                case "unrecognized_cmd":
                {
                    notice_type = NoticeType.UnrecognizedCmd;
                }
                break;
            }

            return notice_type;
        }
    }
}
