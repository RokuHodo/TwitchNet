// standard namespaces
using System;
using System.Text.RegularExpressions;

// project namespaces
using TwitchNet.Enums.Clients.Irc.Twitch;
using TwitchNet.Extensions;

namespace TwitchNet.Utilities
{    
    public static class
    TwitchUtil
    {
        private static readonly uint FOLLOWERS_DURATION_MAX_MONTHS  = 3;
        private static readonly uint FOLLOWERS_DURATION_MAX_WEEKS   = 12;
        private static readonly uint FOLLOWERS_DURATION_MAX_DAYS    = 90;
        private static readonly uint FOLLOWERS_DURATION_MAX_HOURS   = 2_160;
        private static readonly uint FOLLOWERS_DURATION_MAX_MINUTES = 129_600;
        private static readonly uint FOLLOWERS_DURATION_MAX_SECONDS = 7_776_000;

        private static Regex user_nick_regex                        = new Regex("^[a-z][a-z0-9_]{2,24}$");        
        private static Regex video_length_regex                     = new Regex("^(?:(?:(?<hours>\\d{1,2})h)?(?<minutes>\\d{1,2})m)?(?<seconds>\\d{1,2})s$");
        private static Regex followers_duration_regex               = new Regex("^(?:(?<duration>\\d+)\\s*(?<period>mo|months?|w|weeks?|d|days?|h|hours?|m|mninutes?|s|seconds?)\\s*)+$");

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

            int months  = 0;
            int weeks   = 0;
            int days    = 0;
            int hours   = 0;
            int minutes = 0;
            int seconds = 0;

            CaptureCollection caputure_duratrions   = match.Groups["duration"].Captures;
            CaptureCollection caputure_periods      = match.Groups["period"].Captures;
            for (int index = 0; index < caputure_duratrions.Count; ++index)
            {
                // If this fails, the number could not fit into a uint and would never be a valid duration
                if(!Int32.TryParse(caputure_duratrions[index].Value, out int period_duration))
                {
                    return result;
                }

                switch (EnumCacheUtil.ToFollowersDurationPeriod(caputure_periods[index].Value))
                {
                    case FollowersDurationPeriod.Months:
                    {
                        if(period_duration > FOLLOWERS_DURATION_MAX_MONTHS || months + period_duration > FOLLOWERS_DURATION_MAX_MONTHS)
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

            // 1 minute ................................................... = 60 seconds
            // 1 hour   ............................... = 60 minutes     .. = 3,600 seconds
            // 1 day    ............... = 24 hours  ... = 1,440 minutes  .. = 86,400 seconds
            // 1 week   ... = 7 days  . = 168 hours ... = 10,080 minutes .. = 604,800 seconds
            // 1 month  ... = 30 days . = 720 hours ... = 43,200 minutes .. = 2,592,000 seconds
            int total_seconds = seconds + (minutes * 60) + (hours * 3_600) + (days * 86_400) + (weeks * 604_800) + (months * 2_592_000);
            if (total_seconds > FOLLOWERS_DURATION_MAX_SECONDS)
            {
                return result;
            }

            result = true;
            duration = new TimeSpan((months * 30) + (weeks * 7) + days, hours, minutes, seconds);

            return result;
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
    }
}