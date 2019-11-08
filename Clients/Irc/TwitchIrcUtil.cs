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

            // 1 minute = 60        seconds
            // 1 hour   = 3,600     seconds
            // 1 day    = 86,400    seconds
            // 1 week   = 604,800   seconds
            // 1 month  = 2,592,000 seconds
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
            return !nick.IsNull() && user_nick_regex.IsMatch(nick);
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
            ToString(in IrcTags tags, string key)
            {
                if (!tags.TryGetValue(key, out string value))
                {
                    return string.Empty;
                }

                return value;
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
            ToUInt16(in IrcTags tags, string key)
            {
                if (!tags.TryGetValue(key, out string value))
                {
                    return 0;
                }

                UInt16.TryParse(value, out ushort _value);

                return _value;
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
            ToUInt32(in IrcTags tags, string key)
            {
                if (!tags.TryGetValue(key, out string value))
                {
                    return 0;
                }

                UInt32.TryParse(value, out uint _value);

                return _value;
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
            ToTimeSpanFromSeconds(in IrcTags tags, string key)
            {
                int seconds = ToInt32(tags, key);

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
            ToInt32(in IrcTags tags, string key)
            {
                if (!tags.TryGetValue(key, out string value))
                {
                    return -1;
                }

                Int32.TryParse(value, out int _value);

                return _value;
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
            ToBool(in IrcTags tags, string key)
            {
                if (!tags.TryGetValue(key, out string value))
                {
                    return default;
                }

                if (!Byte.TryParse(value, out byte _value))
                {
                    return default;
                }

                return Convert.ToBoolean(_value);
            }

            /// <summary>
            /// Converts a tag to an equivalent enum value.
            /// </summary>
            /// <param name="tags">The IRC message tags.</param>
            /// <param name="key">The tag to convert.</param>
            /// <returns>
            /// Returns the equivalent enum value if the tag was able to be converted.
            /// Returns the enum's default value otherwise.
            /// </returns>
            public static enum_type
            ToEnum<enum_type>(in IrcTags tags, string key)
            where enum_type : struct
            {
                if (!tags.TryGetValue(key, out string value))
                {
                    return default;
                }

                if (!EnumUtil.TryParse(value, out enum_type _value))
                {
                    return default;
                }

                return _value;
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
            FromtHtml(in IrcTags tags, string key)
            {
                if (!tags.TryGetValue(key, out string value))
                {
                    return Color.Empty;
                }

                if (!value.IsValidHtmlColor())
                {
                    return Color.Empty;
                }

                return ColorTranslator.FromHtml(value);
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
            FromUnixEpochMilliseconds(in IrcTags tags, string key)
            {
                if (!tags.TryGetValue(key, out string value))
                {
                    return DateTime.MinValue;
                }

                if (!Int64.TryParse(value, out long _value))
                {
                    return DateTime.MinValue;
                }

                return _value.FromUnixEpochMilliseconds();
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
            ToBadges(in IrcTags tags, string key)
            {
                if (!tags.TryGetValue(key, out string value))
                {
                    return new Badge[0];
                }

                string[] pairs = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (pairs.Length == 0)
                {
                    return new Badge[0];
                }

                List<Badge> badges = new List<Badge>();
                foreach (string pair in pairs)
                {
                    Badge badge = new Badge(pair);
                    badges.Add(badge);
                }

                return badges.ToArray();
            }

            /// <summary>
            /// Converts a tag to an equivalent array of <see cref="BadgeInfo"/> values.
            /// </summary>
            /// <param name="tags">The IRC message tags.</param>
            /// <param name="key">The tag to convert.</param>
            /// <returns>
            /// Returns the equivalent array of <see cref="BadgeInfo"/> values if the tag was able to be converted.
            /// Returns an empty <see cref="BadgeInfo"/> array otherwise.
            /// </returns>
            public static BadgeInfo[]
            ToBadgeInfo(in IrcTags tags, string key)
            {
                if (!tags.TryGetValue(key, out string value))
                {
                    return new BadgeInfo[0];
                }

                string[] pairs = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (!pairs.IsValid())
                {
                    return new BadgeInfo[0];
                }

                List<BadgeInfo> badge_info = new List<BadgeInfo>();
                foreach (string pair in pairs)
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
            ToEmotes(in IrcTags tags, string key)
            {
                if (!tags.TryGetValue(key, out string value))
                {
                    return new Emote[0];
                }

                string[] pairs = value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
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
            ToStringArray(in IrcTags tags, string key, char separator)
            {
                if (!tags.TryGetValue(key, out string value))
                {
                    return new string[0];
                }

                return value.Split(separator);
            }
        }
    }
}
