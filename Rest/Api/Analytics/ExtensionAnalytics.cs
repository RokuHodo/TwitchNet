// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Analytics
{
    public class
    ExtensionAnalytics
    {
        /// <summary>
        /// The ID of the extension.
        /// </summary>
        [JsonProperty("extension_id")]
        public string extension_id  { get; protected set; }

        /// <summary>
        /// The URL to the downloadable CSV file containing the analytic data.
        /// Valid for 1 minute.
        /// </summary>
        [JsonProperty("URL")]
        public string url           { get; protected set; }

        /// <summary>
        /// The analytic report type.
        /// </summary>
        [JsonProperty("type")]
        public AnalyticsType type { get; protected set; }

        /// <summary>
        /// The time period that the analytic reports cover.
        /// </summary>
        [JsonProperty("date_range")]
        public DateRange date_range { get; protected set; }
    }
}
