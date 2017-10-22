// project namespaces
using System;

namespace TwitchNet.Extensions
{
    internal static class
    DateTimeExtensions
    {
        /// <summary>
        /// Converts a unix epoch time stamp to a local <see cref="DateTime"/> value.
        /// </summary>
        /// <param name="time">The unix time stamp to convert.</param>
        /// <returns>Returns a <seealso cref="DateTime"/> equivalent of a unix timestamp converted to local time..</returns>
        public static DateTime
        ToDateTimeFromUnixEpoch(this double time)
        {
            DateTime unix_epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime date_time = unix_epoch.AddSeconds(time).ToLocalTime();

            return date_time;
        }
    }
}
