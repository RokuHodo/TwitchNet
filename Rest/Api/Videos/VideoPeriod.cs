// standard namespaces
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Helpers.Json;

// imported namespaces
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Videos
{
    [JsonConverter(typeof(EnumConverter))]
    public enum
    VideoPeriod
    {
        /// <summary>
        /// Returns all videos, regardless of when they were created.
        /// </summary>
        [EnumMember(Value = "all")]
        All     = 0,

        /// <summary>
        /// Returns videos that were created today.
        /// </summary>
        [EnumMember(Value = "day")]
        Day,

        /// <summary>
        /// Returns videos that were created this week.
        /// </summary>
        [EnumMember(Value = "week")]
        Week,

        /// <summary>
        /// Returns videos that were created this month.
        /// </summary>
        [EnumMember(Value = "month")]
        Month
    }
}
