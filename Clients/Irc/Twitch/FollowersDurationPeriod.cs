// standard namespaces
using System.Runtime.Serialization;

namespace
TwitchNet.Clients.Irc.Twitch
{
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
}
