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
        /// The earliest date/time that a report will be returned in UTC.
        /// </summary>
        [JsonProperty("started_at")]
        public DateTime started_at { get; protected set; }

        /// <summary>
        /// The latest  date/time that a report will be returned in UTC.
        /// </summary>
        [JsonProperty("ended_at")]
        public DateTime ended_at { get; protected set; }
    }
}
