// standard namespaces
using System;
using System.Runtime.Serialization;

// imported namespaces
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace
TwitchNet.Enums.Api.Streams
{
    [Flags]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum
    StreamType
    {
        /// <summary>
        /// The stream is not live, a vodcast, or a playlist.
        /// </summary>
        [EnumMember(Value = "")]
        Empty       = 0,

        /// <summary>
        /// The stream is live.
        /// </summary>
        [EnumMember(Value = "live")]
        Live        = 1 << 0,

        /// <summary>
        /// The stream is a rebroadcast of a past stream.
        /// </summary>
        [EnumMember(Value = "vodcast")]
        Vodcast     = 1 << 1,

        /// <summary>
        /// The stream is collection of videos or vodcasts.
        /// </summary>
        [EnumMember(Value = "playlist")]
        Playlist    = 1 << 2,

        /// <summary>
        /// Specifies to return all stream types and is only applicable when specifying query parameters.
        /// </summary>
        [EnumMember(Value = "all")]
        All         = Live | Vodcast | Playlist
    }
}
