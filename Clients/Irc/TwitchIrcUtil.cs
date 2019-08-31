// standard namespaces
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

// project namespaces
using TwitchNet.Utilities;
using TwitchNet.Extensions;

namespace
TwitchNet.Clients.Irc
{
    internal static class
    TwitchIrcUtil
    {
        private static readonly uint FOLLOWERS_DURATION_MAX_MONTHS = 3;
        private static readonly uint FOLLOWERS_DURATION_MAX_WEEKS = 12;
        private static readonly uint FOLLOWERS_DURATION_MAX_DAYS = 90;
        private static readonly uint FOLLOWERS_DURATION_MAX_HOURS = 2_160;
        private static readonly uint FOLLOWERS_DURATION_MAX_MINUTES = 129_600;
        private static readonly uint FOLLOWERS_DURATION_MAX_SECONDS = 7_776_000;

        private static Regex user_nick_regex = new Regex("^[a-z][a-z0-9_]{2,24}$");
        private static Regex video_length_regex = new Regex("^(?:(?:(?<hours>\\d{1,2})h)?(?<minutes>\\d{1,2})m)?(?<seconds>\\d{1,2})s$");
        private static Regex followers_duration_regex = new Regex("^(?:(?<duration>\\d+)\\s*(?<period>mo|months?|w|weeks?|d|days?|h|hours?|m|mninutes?|s|seconds?)\\s*)+$");

        public static readonly string ACTION_PREFIX = "\u0001ACTION";
        public static readonly string ACTION_SUFFIX = "\u0001";

        public const string REGEX_PATTERN_UUID = "[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}";

        public static MessageSource
        GetMessageSource(string channel)
        {
            if (channel.IsNull())
            {
                return MessageSource.Other;
            }

            return channel.TextBefore(':') == "#chatrooms" ? MessageSource.ChatRoom : MessageSource.StreamChat;
        }

        public static Tuple<string, string>
        ParseChatRoomChannel(string channel)
        {
            string user_id = channel.TextBetween(':', ':');
            string uuid = string.Empty;

            int index = channel.LastIndexOf(':');
            if (index != -1)
            {
                uuid = channel.TextAfter(':', index);
            }

            return Tuple.Create(user_id, uuid);
        }        

        /// <summary>
        /// Checks to see if the followers duration length is between 0 seconds and 90 days (3 months).
        /// </summary>
        /// <param name="duration">The duration to check.</param>
        /// <returns>
        /// Returns true if the duration is between 0 seconds and 90 days (3months), inclusive.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        IsValidFollowersDurationLength(string duration)
        {
            bool result = TryConvertToFollowerDuratrion(duration, out TimeSpan _duration);

            return result;
        }

        /// <summary>
        /// Checks to see if the followers duration length is between 0 seconds and 90 days (3 months).
        /// </summary>
        /// <param name="duration">The duration to check.</param>
        /// <returns>
        /// Returns true if the duration is between 0 seconds and 90 days (3months), inclusive.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        IsValidFollowersDurationLength(TimeSpan duration)
        {
            bool result = duration.TotalSeconds.IsInRange(0, FOLLOWERS_DURATION_MAX_SECONDS);

            return result;
        }

        /// <summary>
        /// Checks to see if the string matches the followers duration format.
        /// </summary>
        /// <param name="duration">The string to check.</param>
        /// <returns>
        /// Returns true if the string matches the followers duration format.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        IsValidFollowersDurationFormat(string duration)
        {
            bool result = duration.IsValid() && followers_duration_regex.IsMatch(duration);

            return result;
        }

        /// <summary>
        /// Tries to match the string to the followers duration format.
        /// </summary>
        /// <param name="duration">The string to check.</param>
        /// <param name="match">
        /// <para>The regex match if the string matched the followers duration format.</para>
        /// <para>Set to <see cref="Match.Empty"/> if no match was found.</para>
        /// </param>
        /// <returns>
        /// Returns true if the string matched the allowed followers duration format.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        TryGetFollowersDurationMatch(string duration, out Match match)
        {
            match = duration.IsValid() ? followers_duration_regex.Match(duration) : Match.Empty;

            return match.Success;
        }

        /// <summary>
        /// Tries to convert a string to a valid followers duration.
        /// </summary>
        /// <param name="str">The string to convert./</param>
        /// <param name="duration">
        /// <para>The equivalent <see cref="TimeSpan"/> duration if the conversion was sucessful.</para>
        /// <para>Set to <see cref="TimeSpan.Zero"/> otherwise.</para>
        /// </param>
        /// <returns>
        /// Returns true if the string was able to converted to an equivalent <see cref="TimeSpan"/> duration.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        TryConvertToFollowerDuratrion(string str, out TimeSpan duration)
        {
            bool result = false;

            duration = TimeSpan.Zero;
            if (!TryGetFollowersDurationMatch(str, out Match match))
            {
                return result;
            }

            int months = 0;
            int weeks = 0;
            int days = 0;
            int hours = 0;
            int minutes = 0;
            int seconds = 0;

            CaptureCollection caputure_duratrions = match.Groups["duration"].Captures;
            CaptureCollection caputure_periods = match.Groups["period"].Captures;
            for (int index = 0; index < caputure_duratrions.Count; ++index)
            {
                // If this fails, the number could not fit into a uint and would never be a valid duration
                if (!Int32.TryParse(caputure_duratrions[index].Value, out int period_duration))
                {
                    return result;
                }

                switch (GetFollowersDurationPeriod(caputure_periods[index].Value))
                {
                    case FollowersDurationPeriod.Months:
                        {
                            if (period_duration > FOLLOWERS_DURATION_MAX_MONTHS || months + period_duration > FOLLOWERS_DURATION_MAX_MONTHS)
                            {
                                return result;
                            }

                            months += period_duration;
                        }
                        break;

                    case FollowersDurationPeriod.Weeks:
                        {
                            if (period_duration > FOLLOWERS_DURATION_MAX_WEEKS || weeks + period_duration > FOLLOWERS_DURATION_MAX_WEEKS)
                            {
                                return result;
                            }

                            weeks += period_duration;
                        }
                        break;

                    case FollowersDurationPeriod.Days:
                        {
                            if (period_duration > FOLLOWERS_DURATION_MAX_DAYS || days + period_duration > FOLLOWERS_DURATION_MAX_DAYS)
                            {
                                return result;
                            }

                            days += period_duration;
                        }
                        break;

                    case FollowersDurationPeriod.Hours:
                        {
                            if (period_duration > FOLLOWERS_DURATION_MAX_HOURS || hours + period_duration > FOLLOWERS_DURATION_MAX_HOURS)
                            {
                                return result;
                            }

                            hours += period_duration;
                        }
                        break;

                    case FollowersDurationPeriod.Minutes:
                        {
                            if (period_duration > FOLLOWERS_DURATION_MAX_MINUTES || minutes + period_duration > FOLLOWERS_DURATION_MAX_MINUTES)
                            {
                                return result;
                            }

                            minutes += period_duration;
                        }
                        break;

                    case FollowersDurationPeriod.Seconds:
                        {
                            if (period_duration > FOLLOWERS_DURATION_MAX_SECONDS || seconds + period_duration > FOLLOWERS_DURATION_MAX_SECONDS)
                            {
                                return result;
                            }

                            seconds += period_duration;
                        }
                        break;
                }
            }

            // 1 minute .................................................. = 60        seconds
            // 1 hour   .............................. = 60     minutes .. = 3,600     seconds
            // 1 day    ............... = 24  hours .. = 1,440  minutes .. = 86,400    seconds
            // 1 week   .. = 7  days .. = 168 hours .. = 10,080 minutes .. = 604,800   seconds
            // 1 month  .. = 30 days .. = 720 hours .. = 43,200 minutes .. = 2,592,000 seconds
            int total_seconds = seconds + (minutes * 60) + (hours * 3_600) + (days * 86_400) + (weeks * 604_800) + (months * 2_592_000);
            if (total_seconds > FOLLOWERS_DURATION_MAX_SECONDS)
            {
                return result;
            }

            result = true;
            duration = new TimeSpan((months * 30) + (weeks * 7) + days, hours, minutes, seconds);

            return result;

            FollowersDurationPeriod
            GetFollowersDurationPeriod(string period)
            {
                switch (period)
                {
                    case "mo":
                    case "month":
                    case "months":
                        {
                            return FollowersDurationPeriod.Months;
                        }

                    case "w":
                    case "week":
                    case "weeks":
                        {
                            return FollowersDurationPeriod.Weeks;
                        }

                    case "d":
                    case "day":
                    case "days":
                        {
                            return FollowersDurationPeriod.Days;
                        }

                    case "h":
                    case "hour":
                    case "hours":
                        {
                            return FollowersDurationPeriod.Hours;
                        }

                    case "m":
                    case "minute":
                    case "minutes":
                        {
                            return FollowersDurationPeriod.Minutes;
                        }

                    case "s":
                    case "second":
                    case "seconds":
                        {
                            return FollowersDurationPeriod.Seconds;
                        }
                }

                // This will never be reached.
                // This is just to make sure it compiles.
                return FollowersDurationPeriod.Seconds;
            }
        }

        public enum
        FollowersDurationPeriod
        {
            /// <summary>
            /// Unsupported duration period.
            /// </summary>
            [EnumMember(Value = "")]
            Other = 0,

            /// <summary>
            /// s, second, seconds
            /// </summary>
            [EnumMember(Value = "seconds")]
            Seconds,

            /// <summary>
            /// m, minute, minutes
            /// </summary>
            [EnumMember(Value = "minutes")]
            Minutes,

            /// <summary>
            /// h, hour, hours
            /// </summary>
            [EnumMember(Value = "hours")]
            Hours,

            /// <summary>
            /// d, day, days
            /// </summary>
            [EnumMember(Value = "days")]
            Days,

            /// <summary>
            /// w, week, weeks
            /// </summary>
            [EnumMember(Value = "weeks")]
            Weeks,

            /// <summary>
            /// m, month, months
            /// </summary>
            [EnumMember(Value = "months")]
            Months,
        }

        /// <summary>
        /// Checks to see if a user's Twitch nick is valid.
        /// </summary>
        /// <param name="nick">
        /// <para>The nick to check.</para>
        /// <para>The length must be between 2 and 24 and only contain alpha-numeric characters.</para>
        /// </param>
        /// <returns>
        /// Returns true if the nick length is between 2 and 24 and only contains alpha-numeric characers.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        IsValidNick(string nick)
        {
            bool result = user_nick_regex.IsMatch(nick);

            return result;
        }

        /// <summary>
        /// Checks to see if the video length is in the proper format, HHH:MM:SS.
        /// </summary>
        /// <param name="length">The video length to check.</param>
        /// <returns>
        /// Returns true is the video length matches the format HH:MM:SS.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        IsValidVideoLength(string length)
        {
            bool result = video_length_regex.IsMatch(length);

            return result;
        }

        /// <summary>
        /// Tries to convert a video length to equivalent <see cref="TimeSpan"/> vlaue.
        /// </summary>
        /// <param name="value">the string to convert.</param>
        /// <param name="length">
        /// <para>The equivalent <see cref="TimeSpan"/> length of the conversion was successful.</para>
        /// <para>Set to <see cref="TimeSpan.Zero"/> otherwise.</para>
        /// </param>
        /// <returns>
        /// Returns true if the string was able to converted to an equivalent <see cref="TimeSpan"/> length.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        TryConvertToVideoLength(string value, out TimeSpan length)
        {
            bool success = false;

            length = TimeSpan.Zero;

            Match match = video_length_regex.Match(value);
            if (match.Success)
            {
                Int32.TryParse(match.Groups["hours"].Value, out int hours);
                Int32.TryParse(match.Groups["minutes"].Value, out int minutes);
                Int32.TryParse(match.Groups["seconds"].Value, out int seconds);

                length = new TimeSpan(hours, minutes, seconds);

                success = true;
            }

            return success;
        }

        internal class
        Tags
        {
            /// <summary>
            /// Converts a tag to an equivalent <see cref="String"/> value.
            /// </summary>
            /// <param name="tags">The IRC message tags.</param>
            /// <param name="key">The tag to convert.</param>
            /// <returns>
            /// Returns the equivalent <see cref="string"/> value if the tag was able to be converted.
            /// Returns <see cref="string.Empty"/> otherwise.
            /// </returns>
            public static string
            ToString(in IrcMessage message, string key)
            {
                if (!IsTagValid(message, key))
                {
                    return string.Empty;
                }

                return message.tags[key];
            }

            /// <summary>
            /// Converts a tag to an equivalent <see cref="UInt16"/> value.
            /// </summary>
            /// <param name="tags">The IRC message tags.</param>
            /// <param name="key">The tag to convert.</param>
            /// <returns>
            /// Returns the equivalent <see cref="UInt16"/> value if the tag was able to be converted.
            /// Returns 0 otherwise.
            /// </returns>
            public static ushort
            ToUInt16(in IrcMessage message, string key)
            {
                if (!IsTagValid(message, key))
                {
                    return 0;
                }

                UInt16.TryParse(message.tags[key], out ushort value);

                return value;
            }

            /// <summary>
            /// Converts a tag to an equivalent <see cref="UInt32"/> value.
            /// </summary>
            /// <param name="tags">The IRC message tags.</param>
            /// <param name="key">The tag to convert.</param>
            /// <returns>
            /// Returns the equivalent <see cref="UInt32"/> value if the tag was able to be converted.
            /// Returns 0 otherwise.
            /// </returns>
            public static uint
            ToUInt32(in IrcMessage message, string key)
            {
                if (!IsTagValid(message, key))
                {
                    return 0;
                }

                UInt32.TryParse(message.tags[key], out uint value);

                return value;
            }

            /// <summary>
            /// Converts a tag to an equivalent <see cref="TimeSpan"/> value.
            /// </summary>
            /// <param name="tags">The IRC message tags.</param>
            /// <param name="key">The tag to convert.</param>
            /// <returns>
            /// Returns the equivalent <see cref="TimeSpan"/> value if the tag was able to be converted.
            /// Returns <see cref="TimeSpan.Zero"/> otherwise.
            /// </returns>
            public static TimeSpan
            ToTimeSpanFromSeconds(in IrcMessage message, string key)
            {
                int seconds = ToInt32(message, key);

                TimeSpan time = seconds == 0 ? TimeSpan.Zero : new TimeSpan(0, 0, seconds);

                return time;
            }

            /// <summary>
            /// Converts a tag to an equivalent <see cref="Int32"/> value.
            /// </summary>
            /// <param name="tags">The IRC message tags.</param>
            /// <param name="key">The tag to convert.</param>
            /// <returns>
            /// Returns the equivalent <see cref="Int32"/> value if the tag was able to be converted.
            /// Returns -1 otherwise.
            /// </returns>
            public static int
            ToInt32(in IrcMessage message, string key)
            {
                if (!IsTagValid(message, key))
                {
                    return -1;
                }

                Int32.TryParse(message.tags[key], out int value);

                return value;
            }

            /// <summary>
            /// Converts a tag to an equivalent <see cref="Boolean"/> value.
            /// </summary>
            /// <param name="tags">The IRC message tags.</param>
            /// <param name="key">The tag to convert.</param>
            /// <returns>
            /// Returns the equivalent <see cref="Boolean"/> value if the tag was able to be converted.
            /// Returns false otherwise.
            /// </returns>
            public static bool
            ToBool(in IrcMessage message, string key)
            {
                if (!IsTagValid(message, key))
                {
                    return default;
                }

                if (!Byte.TryParse(message.tags[key], out byte _value))
                {
                    return default;
                }

                return Convert.ToBoolean(_value);
            }

            /// <summary>
            /// Converts a tag to an equivalent <see cref="UserType"/> value.
            /// </summary>
            /// <param name="tags">The IRC message tags.</param>
            /// <param name="key">The tag to convert.</param>
            /// <returns>
            /// Returns the equivalent <see cref="UserType"/> value if the tag was able to be converted.
            /// Returns <see cref="UserType.None"/> otherwise.
            /// </returns>
            public static UserType
            ToUserType(in IrcMessage message, string key)
            {
                if (!IsTagValid(message, key))
                {
                    return UserType.None;
                }

                return EnumUtil.Parse<UserType>(message.tags[key]);
            }

            /// <summary>
            /// Converts a tag to an equivalent <see cref="NoticeType"/> value.
            /// </summary>
            /// <param name="tags">The IRC message tags.</param>
            /// <param name="key">The tag to convert.</param>
            /// <returns>
            /// Returns the equivalent <see cref="NoticeType"/> value if the tag was able to be converted.
            /// Returns <see cref="NoticeType.Other"/> otherwise.
            /// </returns>
            public static NoticeType
            ToNoticeType(in IrcMessage message, string key)
            {
                if (!IsTagValid(message, key))
                {
                    return NoticeType.Other;
                }

                EnumUtil.TryParse(message.tags[key], out NoticeType notice);

                return notice;
            }

            /// <summary>
            /// Converts a tag to an equivalent <see cref="UserNoticeType"/> value.
            /// </summary>
            /// <param name="tags">The IRC message tags.</param>
            /// <param name="key">The tag to convert.</param>
            /// <returns>
            /// Returns the equivalent <see cref="UserNoticeType"/> value if the tag was able to be converted.
            /// Returns <see cref="UserNoticeType.Other"/> otherwise.
            /// </returns>
            public static UserNoticeType
            ToUserNoticeType(in IrcMessage message, string key)
            {
                if (!IsTagValid(message, key))
                {
                    return UserNoticeType.Other;
                }

                EnumUtil.TryParse(message.tags[key], out UserNoticeType user_notice);

                return user_notice;
            }

            /// <summary>
            /// Converts a tag to an equivalent <see cref="SubscriptionTier"/> value.
            /// </summary>
            /// <param name="tags">The IRC message tags.</param>
            /// <param name="key">The tag to convert.</param>
            /// <returns>
            /// Returns the equivalent <see cref="SubscriptionTier"/> value if the tag was able to be converted.
            /// Returns <see cref="SubscriptionTier.Other"/> otherwise.
            /// </returns>
            public static SubscriptionTier
            ToSubscriptionPlan(in IrcMessage message, string key)
            {
                if (!IsTagValid(message, key))
                {
                    return SubscriptionTier.Other;
                }

                EnumUtil.TryParse(message.tags[key], out SubscriptionTier plan);

                return plan;
            }

            /// <summary>
            /// Converts a tag to an equivalent <see cref="RitualType"/> value.
            /// </summary>
            /// <param name="tags">The IRC message tags.</param>
            /// <param name="key">The tag to convert.</param>
            /// <returns>
            /// Returns the equivalent <see cref="RitualType"/> value if the tag was able to be converted.
            /// Returns <see cref="RitualType.Other"/> otherwise.
            /// </returns>
            public static RitualType
            ToRitualType(in IrcMessage message, string key)
            {
                if (!IsTagValid(message, key))
                {
                    return RitualType.Other;
                }

                EnumUtil.TryParse(message.tags[key], out RitualType type);

                return type;
            }

            /// <summary>
            /// Converts a tag to an equivalent <see cref="BroadcasterLanguage"/> value.
            /// </summary>
            /// <param name="tags">The IRC message tags.</param>
            /// <param name="key">The tag to convert.</param>
            /// <returns>
            /// Returns the equivalent <see cref="BroadcasterLanguage"/> value if the tag was able to be converted.
            /// Returns <see cref="BroadcasterLanguage.None"/> otherwise.
            /// </returns>
            public static BroadcasterLanguage
            ToBroadcasterLanguage(in IrcMessage message, string key)
            {
                if (!IsTagValid(message, key))
                {
                    return BroadcasterLanguage.None;
                }

                EnumUtil.TryParse(message.tags[key], out BroadcasterLanguage language);

                return language;
            }

            /// <summary>
            /// Converts a tag to an equivalent <see cref="Color"/> value.
            /// </summary>
            /// <param name="tags">The IRC message tags.</param>
            /// <param name="key">The tag to convert.</param>
            /// <returns>
            /// Returns the equivalent <see cref="Color"/> value if the tag was able to be converted.
            /// Returns <see cref="Color.Empty"/> otherwise.
            /// </returns>
            public static Color
            FromtHtml(in IrcMessage message, string key)
            {
                if (!IsTagValid(message, key))
                {
                    return Color.Empty;
                }

                if (!message.tags[key].IsValidHtmlColor())
                {
                    return Color.Empty;
                }

                return ColorTranslator.FromHtml(message.tags[key]);
            }

            /// <summary>
            /// Converts a tag Unix Epoch time to an equivalent <see cref="DateTime"/> value.
            /// </summary>
            /// <param name="tags">The IRC message tags.</param>
            /// <param name="key">The tag to convert.</param>
            /// <returns>
            /// Returns the equivalent <see cref="Color"/> value if the tag was able to be converted.
            /// Returns <see cref="Color.Empty"/> otherwise.
            /// </returns>
            public static DateTime
            FromUnixEpochMilliseconds(in IrcMessage message, string key)
            {
                if (!IsTagValid(message, key))
                {
                    return DateTime.MinValue;
                }

                if (!Int64.TryParse(message.tags[key], out long time_epoch))
                {
                    return DateTime.MinValue;
                }

                return time_epoch.FromUnixEpochMilliseconds();
            }

            /// <summary>
            /// Converts a tag to an equivalent array of <see cref="Badge"/> values.
            /// </summary>
            /// <param name="tags">The IRC message tags.</param>
            /// <param name="key">The tag to convert.</param>
            /// <returns>
            /// Returns the equivalent array of <see cref="Badge"/> values if the tag was able to be converted.
            /// Returns an empty <see cref="Badge"/> array otherwise.
            /// </returns>
            public static Badge[]
            ToBadges(in IrcMessage message, string key)
            {
                if (!IsTagValid(message, key))
                {
                    return new Badge[0];
                }

                string[] badge_pairs = message.tags[key].Split(',');
                if (badge_pairs.Length == 0)
                {
                    return new Badge[0];
                }

                List<Badge> badges = new List<Badge>();
                foreach (string pair in badge_pairs)
                {
                    Badge badge = new Badge(pair);
                    badges.Add(badge);
                }

                return badges.ToArray();
            }

            public static BadgeInfo[]
            ToBadgeInfo(in IrcMessage message, string key)
            {
                if (!IsTagValid(message, key))
                {
                    return new BadgeInfo[0];
                }

                string[] badge_pairs = message.tags[key].Split(',');
                if (!badge_pairs.IsValid())
                {
                    return new BadgeInfo[0];
                }

                List<BadgeInfo> badge_info = new List<BadgeInfo>();
                foreach (string pair in badge_pairs)
                {
                    BadgeInfo badge = new BadgeInfo(pair);
                    badge_info.Add(badge);
                }

                return badge_info.ToArray();
            }

            /// <summary>
            /// Converts a tag to an equivalent array of <see cref="Emote"/> values.
            /// </summary>
            /// <param name="tags">The IRC message tags.</param>
            /// <param name="key">The tag to convert.</param>
            /// <returns>
            /// Returns the equivalent array of <see cref="Emote"/> values if the tag was able to be converted.
            /// Returns an empty <see cref="Emote"/> array otherwise.
            /// </returns>
            public static Emote[]
            ToEmotes(in IrcMessage message, string key)
            {
                if (!IsTagValid(message, key))
                {
                    return new Emote[0];
                }

                string[] pairs = message.tags[key].Split('/');
                if (pairs.Length == 0)
                {
                    return new Emote[0];
                }

                List<Emote> emotes = new List<Emote>();
                foreach (string pair in pairs)
                {
                    Emote emote = new Emote(pair);
                    emotes.Add(emote);
                }

                return emotes.ToArray();
            }

            /// <summary>
            /// Converts a tag to an equivalent array of <see cref="String"/> values.
            /// </summary>
            /// <param name="tags">The IRC message tags.</param>
            /// <param name="key">The tag to convert.</param>
            /// <returns>
            /// Returns the equivalent array of <see cref="String"/> values if the tag was able to be converted.
            /// Returns an empty <see cref="String"/> array otherwise.
            /// </returns>
            public static string[]
            ToStringArray(in IrcMessage message, string key, char separator)
            {
                if (!IsTagValid(message, key))
                {
                    return new string[0];
                }

                return message.tags[key].Split(separator);
            }

            /// <summary>
            /// Checks to see if the specified tag was included in the attached tags.
            /// </summary>
            /// <param name="tags">The IRC message tags.</param>
            /// <param name="key">The tag to check.</param>
            /// <returns>
            /// Returns true if tags were attached in the IRC message and if the specified tag was included in the attached tags.
            /// Returns false otherwise.
            /// </returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool
            IsTagValid(in IrcMessage message, string key)
            {
                bool valid = true;

                if (message.IsNullOrDefault())
                {
                    return false;
                }

                if (!key.IsValid() || !message.tags.IsValid())
                {
                    return false;
                }

                if (!message.tags.ContainsKey(key) || !message.tags[key].IsValid())
                {
                    return false;
                }

                return valid;
            }
        }
    }
}
