// standard namespaces
using System.Runtime.Serialization;

// imported namespaces
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace
TwitchNet.Rest.Api.Videos
{
    [JsonConverter(typeof(StringEnumConverter))]
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
        Day     = 1,

        /// <summary>
        /// Returns videos that were created this week.
        /// </summary>
        [EnumMember(Value = "week")]
        Week    = 2,

        /// <summary>
        /// Returns videos that were created this month.
        /// </summary>
        [EnumMember(Value = "month")]
        Month   = 3,
    }
}
