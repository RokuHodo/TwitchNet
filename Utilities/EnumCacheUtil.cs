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
            RitualType type = RitualType.None;

            if (!str.IsValid())
            {
                return type;
            }

            switch (str)
            {
                case "new_chatter":
                {
                    type = RitualType.NewChatter;
                }
                break;
            }

            return type;
        }
    }
}
