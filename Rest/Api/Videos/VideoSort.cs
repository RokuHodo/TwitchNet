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
    VideoSort
    {
        /// <summary>
        /// Returns videos sorted by time, newest to oldest.
        /// </summary>
        [EnumMember(Value = "time")]
        Time     = 0,

        /// <summary>
        /// Returns videos sorted by which ones are trending.
        /// </summary>
        [EnumMember(Value = "trending")]
        Trending,

        /// <summary>
        /// Returns videos sorted by view count, highest to lowest.
        /// </summary>
        [EnumMember(Value = "views")]
        Views
    }
}
