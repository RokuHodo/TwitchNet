// standard namespaces
using System;

// imported namespaces
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace
TwitchNet.Rest.Api.Videos
{
    // TODO: Add custom converter for the deserializer.
    [Flags]
    [JsonConverter(typeof(StringEnumConverter))]
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
