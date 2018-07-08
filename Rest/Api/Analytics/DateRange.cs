// standard namespaces
using System;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Analytics
{
    public class
    DateRange
    {
        /// <summary>
        /// The report start date/time.
        /// </summary>
        [JsonProperty("started_at")]
        public DateTime started_at { get; protected set; }

        /// <summary>
        /// The report end date/time.
        /// </summary>
        [JsonProperty("ended_at")]
        public DateTime ended_at  { get; protected set; }
    }
}
