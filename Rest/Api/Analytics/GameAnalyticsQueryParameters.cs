using System;

using TwitchNet.Rest.Api.Analytics;

using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Clips
{
    public class
    GameAnalyticsQueryParameters : HelixQueryParameters, IHelixQueryParameters
    {
        [QueryParameter("game_id")]
        public string game_id { get; set; }

        [QueryParameter("started_at")]
        //[JsonConverter(typeof(RFC3339Converter))]
        public DateTime started_at { get; set; }

        [QueryParameter("ended_at")]
        //[JsonConverter(typeof(RFC3339Converter))]
        public DateTime ended_at { get; set; }

        [QueryParameter("type")]
        public GameAnalyticsType? type { get; set; }

        /// <summary>
        /// Creates a new blank instance of the <see cref="GameAnalyticsQueryParameters"/> class.
        /// </summary>
        public GameAnalyticsQueryParameters()
        {

        }
    }
}
