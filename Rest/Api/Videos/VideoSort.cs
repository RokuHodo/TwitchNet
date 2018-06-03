// project namespaces
using TwitchNet.Helpers.Json;

// imported namespaces
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Videos
{
    [JsonConverter(typeof(EnumCacheConverter))]
    public enum
    VideoSort
    {
        /// <summary>
        /// Returns videos sorted by time.
        /// </summary>
        Time     = 0,

        /// <summary>
        /// Returns videos sorted by which ones are trending.
        /// </summary>
        Trending     = 1,

        /// <summary>
        /// Returns videos sorted by view count, highest to lowest.
        /// </summary>
        Views    = 2,
    }
}
