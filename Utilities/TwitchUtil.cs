// standard namespaces
using System;
using System.Text.RegularExpressions;

namespace TwitchNet.Utilities
{
    public static class
    TwitchUtil
    {
        private static Regex user_nick_regex = new Regex("^[a-z][a-z0-9_]{2,24}$");

        //"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$"  -   Aternative that matches #000 as well as #000000
        private static Regex html_hex_color_regex = new Regex("^#([A-Fa-f0-9]{6})$");

        private static Regex video_length_regex = new Regex("^(?:(?:(?<hours>\\d{1,2})h)?(?<minutes>\\d{1,2})m)?(?<seconds>\\d{1,2})s$");

        public static bool
        IsValidNick(string nick)
        {
            bool result = user_nick_regex.IsMatch(nick);

            return result;
        }

        public static bool
        IsValidHtmlColor(string html_color)
        {
            bool result = html_hex_color_regex.IsMatch(html_color);

            return result;
        }

        public static bool
        IsValidVideoLength(string length)
        {
            bool result = video_length_regex.IsMatch(length);

            return result;
        }

        public static bool
        TryGetVideoLength(string value, out TimeSpan length)
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
