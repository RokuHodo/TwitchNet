﻿// project namespaces
using System;

namespace
TwitchNet.Extensions
{
    internal static class
    DateTimeExtensions
    {
        /// <summary>
        /// <para>Converts a unix epoch time stamp to a local <see cref="DateTime"/> value.</para>
        /// <para>This assumes the epoch time stanp is in seconds.</para>
        /// </summary>
        /// <param name="time">The unix time stamp to convert in seconds.</param>
        /// <returns>Returns a <seealso cref="DateTime"/> equivalent of a unix timestamp converted to local time.</returns>
        public static DateTime
        FromUnixEpochSeconds(this long time)
        {
            DateTime unix_epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime date_time = unix_epoch.AddSeconds(time).ToLocalTime();

            return date_time;
        }

        /// <summary>
        /// <para>Converts a unix epoch time stamp to a local <see cref="DateTime"/> value.</para>
        /// <para>This assumes the epoch time stanp is in seconds.</para>
        /// </summary>
        /// <param name="time">The unix time stamp to convert in milliseconds.</param>
        /// <returns>Returns a <seealso cref="DateTime"/> equivalent of a unix timestamp converted to local time.</returns>
        public static DateTime
        FromUnixEpochMilliseconds(this long time)
        {
            DateTime unix_epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime date_time = unix_epoch.AddMilliseconds(time).ToLocalTime();

            return date_time;
        }
    }
}
