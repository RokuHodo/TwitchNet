// standard namespaces
using System;
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Helpers.Json;

// imported namespaces
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Videos
{
    [Flags]
    [JsonConverter(typeof(EnumConverter))]
    public enum
    VideoType
    {
        /// <summary>
        /// Returns uploaded videos.
        /// </summary>
        [EnumMember(Value = "upload")]
        Upload     = 1 << 0,

        /// <summary>
        /// Returns archived videos.
        /// </summary>
        [EnumMember(Value = "archive")]
        Archive     = 1 << 1,

        /// <summary>
        /// Returns highlighted videos.
        /// </summary>
        [EnumMember(Value = "highlight")]
        Highlight   = 1 << 2,

        /// <summary>
        /// Returns all videos.
        /// </summary>
        [EnumMember(Value = "all")]
        All         = Upload | Archive | Highlight,
    }
}
