﻿namespace
TwitchNet.Clients.Irc.Twitch
{
    public enum
    FollowersDurationPeriod
    {
        /// <summary>
        /// Unsupported duration period.
        /// </summary>
        Other = 0,

        /// <summary>
        /// s, second, seconds
        /// </summary>
        Seconds,

        /// <summary>
        /// m, minute, minutes
        /// </summary>
        Minutes,

        /// <summary>
        /// h, hour, hours
        /// </summary>
        Hours,

        /// <summary>
        /// d, day, days
        /// </summary>
        Days,

        /// <summary>
        /// w, week, weeks
        /// </summary>
        Weeks,

        /// <summary>
        /// m, month, months
        /// </summary>
        Months,
    }
}
