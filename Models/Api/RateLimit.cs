// standard namespaces
using System;
using System.Collections.Generic;

// project namespaces
using TwitchNet.Extensions;

// imported .dll's
using RestSharp;

namespace
TwitchNet.Models.Api
{
    public class
    RateLimit
    {
        /// <summary>
        /// The number of requests you can use for the rate-limit window (60 seconds).
        /// </summary>
        public ushort   limit       { get; protected set; }

        /// <summary>
        /// The number of requests left to use for the rate-limit window.
        /// </summary>
        public ushort   remaining   { get; protected set; }

        /// <summary>A 
        /// When the rate-limit window will reset.
        /// </summary>
        public DateTime reset       { get; protected set; }

        public RateLimit()
        {

        }

        public RateLimit(Dictionary<string, string> headers)
        {
            if (headers.ContainsKey("Ratelimit-Limit"))
            {
                limit = Convert.ToUInt16(headers["Ratelimit-Limit"]);
            }

            if (headers.ContainsKey("Ratelimit-Remaining"))
            {
                remaining = Convert.ToUInt16(headers["Ratelimit-Remaining"]);
            }

            if (headers.ContainsKey("Ratelimit-Reset"))
            {
                long reset_double = Convert.ToInt64(headers["Ratelimit-Reset"]);
                reset = reset_double.FromUnixEpochSeconds();
            }
        }
    }
}
