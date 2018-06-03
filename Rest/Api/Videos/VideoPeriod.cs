// project namespaces
using TwitchNet.Helpers.Json;

// imported namespaces
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Videos
{
    [JsonConverter(typeof(EnumCacheConverter))]
    public enum
    VideoPeriod
    {
        /// <summary>
        /// Returns all videos, regardless of when they were created.
        /// </summary>
        All     = 0,

        /// <summary>
        /// Returns videos that were created today.
        /// </summary>
        Day     = 1,

        /// <summary>
        /// Returns videos that were created this week.
        /// </summary>
        Week    = 2,

        /// <summary>
        /// Returns videos that were created this month.
        /// </summary>
        Month   = 3,
    }
}
