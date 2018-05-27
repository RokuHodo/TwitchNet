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
        public static readonly RateLimit None = new RateLimit();

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
        public RateLimit(IList<Parameter> headers)
        {
            Dictionary<string, string> _headers = new Dictionary<string, string>();
            foreach(Parameter header in headers)
            {
                if (_headers.ContainsKey(header.Name))
                {
                    continue;
                }

                _headers.Add(header.Name, header.Value.ToString());
            }

            limit = 0;
            if (_headers.ContainsKey("Ratelimit-Limit"))
            {
                limit = Convert.ToUInt16(_headers["Ratelimit-Limit"]);
            }

            remaining = 0;
            if (_headers.ContainsKey("Ratelimit-Remaining"))
            {
                remaining = Convert.ToUInt16(_headers["Ratelimit-Remaining"]);
            }

            reset_time = DateTime.MinValue;
            if (_headers.ContainsKey("Ratelimit-Reset"))
            {
                long reset_double = Convert.ToInt64(_headers["Ratelimit-Reset"]);
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
