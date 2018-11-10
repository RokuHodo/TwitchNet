// standard namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

// project namespaces
using TwitchNet.Extensions;

namespace
TwitchNet.Rest
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
        public RateLimit(HttpResponseHeaders headers)
        {
            limit = 0;
            if (headers.TryGetValues("Ratelimit-Limit", out IEnumerable<string> _limit))
            {
                limit = Convert.ToUInt16(_limit.ElementAt(0));
            }

            remaining = 0;
            if (headers.TryGetValues("Ratelimit-Remaining", out IEnumerable<string> _remaining))
            {
                remaining = Convert.ToUInt16(_remaining.ElementAt(0));
            }

            reset_time = DateTime.MinValue;
            if (headers.TryGetValues("Ratelimit-Reset", out IEnumerable<string> _reset_time))
            {
                long reset_double = Convert.ToInt64(_reset_time.ElementAt(0));
                reset_time = reset_double.FromUnixEpochSeconds();
            }
        }

        public RateLimit()
        {
            limit = 0;
            remaining = 0;

            reset_time = DateTime.MinValue;
        }
    }
}
