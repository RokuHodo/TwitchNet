// standard namespaces
using System;
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Helpers.Json;

// imported namespaces
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Streams
{
    [Flags]
    [JsonConverter(typeof(EnumConverter))]
    public enum
    StreamType
    {
        /// <summary>
        /// The stream is not live or a vodcast.
        /// </summary>
        [EnumMember(Value = "")]
        Other       = 0,

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
        /// Specifies to return all stream types and is only applicable when providing query parameters.
        /// </summary>
        [EnumMember(Value = "all")]
        All         = Live | Vodcast
    }
}
