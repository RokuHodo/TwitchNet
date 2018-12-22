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
    VideoType
    {
        /// <summary>
        /// Returns uploaded videos.
        /// </summary>
        [EnumMember(Value = "upload")]
        Upload,

        /// <summary>
        /// Returns archived videos.
        /// </summary>
        [EnumMember(Value = "archive")]
        Archive,

        /// <summary>
        /// Returns highlighted videos.
        /// </summary>
        [EnumMember(Value = "highlight")]
        Highlight,

        /// <summary>
        /// Returns all videos.
        /// </summary>
        [EnumMember(Value = "all")]
        All
    }
}
