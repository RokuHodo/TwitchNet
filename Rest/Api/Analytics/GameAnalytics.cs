// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Analytics
{
    public class
    GameAnalytics
    {
        /// <summary>
        /// The ID of the game to get the report for.
        /// </summary>
        [JsonProperty("game_id")]
        public string               game_id     { get; protected set; }        

        /// <summary>
        /// The URL to the downloadable CSV file containing the analytic data.
        /// Valid for 1 minute.
        /// </summary>
        [JsonProperty("URL")]
        public string               url         { get; protected set; }

        /// <summary>
        /// The type of report.
        /// </summary>
        [JsonProperty("type")]
        public GameAnalyticsType    type        { get; protected set; }

        /// <summary>
        /// The date/time range that the report was taken.
        /// </summary>
        [JsonProperty("date_range")]
        public DateRange            date_range  { get; protected set; }
    }
}
