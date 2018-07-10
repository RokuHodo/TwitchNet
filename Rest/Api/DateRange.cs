// standard namespaces
using System;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api
{
    public class
    DateRange
    {
        /// <summary>
        /// The start date/time period for the returned data.
        /// </summary>
        [JsonProperty("started_at")]
        public DateTime started_at { get; protected set; }

        /// <summary>
        /// The end date/time period for the returned data.
        /// </summary>
        [JsonProperty("ended_at")]
        public DateTime ended_at  { get; protected set; }
    }
}
