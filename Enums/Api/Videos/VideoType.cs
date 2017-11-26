// standard namespaces
using System;
using System.Runtime.Serialization;

// imported namespaces
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace
TwitchNet.Enums.Api.Videos
{
    [Flags]
    [JsonConverter(typeof(StringEnumConverter))]
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
        /// Returns highlighted videos .
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
