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
    VideoSort
    {
        /// <summary>
        /// Returns videos sorted by time.
        /// </summary>
        [EnumMember(Value = "time")]
        Time     = 0,

        /// <summary>
        /// Returns videos sorted by which ones are trending.
        /// </summary>
        [EnumMember(Value = "trending")]
        Trending     = 1,

        /// <summary>
        /// Returns videos sorted by view count, highest to lowest.
        /// </summary>
        [EnumMember(Value = "views")]
        Views    = 2,
    }
}
