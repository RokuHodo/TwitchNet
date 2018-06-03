// project namespaces
using TwitchNet.Helpers.Json;

// standard namespaces
using System;

// imported namespaces
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Videos
{
    [Flags]
    [JsonConverter(typeof(EnumCacheConverter))]
    public enum
    VideoType
    {
        /// <summary>
        /// Returns uploaded videos.
        /// </summary>
        Upload     = 1 << 0,

        /// <summary>
        /// Returns archived videos.
        /// </summary>
        Archive     = 1 << 1,

        /// <summary>
        /// Returns highlighted videos.
        /// </summary>
        Highlight   = 1 << 2,

        /// <summary>
        /// Returns all videos.
        /// </summary>
        All         = Upload | Archive | Highlight,
    }
}
