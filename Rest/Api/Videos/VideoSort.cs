// imported namespaces
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace
TwitchNet.Rest.Api.Videos
{
    // TODO: Add custom converter for the deserializer.
    [JsonConverter(typeof(StringEnumConverter))]
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
