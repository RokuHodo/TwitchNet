// standard namespaces
using System;
using System.Collections.Generic;

// project namespaces
using TwitchNet.Extensions;

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
        public DateTime reset_time  { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="RateLimit"/> class.
        /// </summary>
        /// <param name="headers">The headers from the <see cref="IRestResponse"/>.</param>
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
                reset_time = reset_double.FromUnixEpochSeconds();
            }
        }
    }
}
