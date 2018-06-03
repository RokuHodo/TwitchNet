// imported namespaces
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace
TwitchNet.Rest.Api.Videos
{
    // TODO: Add custom converter for the deserializer.
    [JsonConverter(typeof(StringEnumConverter))]
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
