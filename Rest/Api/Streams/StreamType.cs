// standard namespaces
using System;

// project namespaces
using TwitchNet.Helpers.Json;

// imported namespaces
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Streams
{
    [Flags]
    [JsonConverter(typeof(EnumCacheConverter))]
    public enum
    StreamType
    {
        /// <summary>
        /// The stream is not live or a vodcast.
        /// </summary>
        Other       = 0,

        /// <summary>
        /// The stream is live.
        /// </summary>
        Live        = 1 << 0,

        /// <summary>
        /// The stream is a rebroadcast of a past stream.
        /// </summary>
        Vodcast     = 1 << 1,

        /// <summary>
        /// Specifies to return all stream types and is only applicable when providing query parameters.
        /// </summary>
        All         = Live | Vodcast
    }
}
