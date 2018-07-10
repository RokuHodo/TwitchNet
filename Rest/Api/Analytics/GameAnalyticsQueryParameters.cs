// standard namespaces
using System;

// project namespaces
using TwitchNet.Rest.Api.Analytics;

namespace
TwitchNet.Rest.Api.Clips
{
    public class
    GameAnalyticsQueryParameters : HelixQueryParameters, IHelixQueryParameters
    {
        /// <summary>
        /// <para>The ID of the game to request a report for.</para>
        /// <para>If <see cref="game_id"/> is specified, this supercedes any cursor value.</para>
        /// </summary>
        [QueryParameter("game_id")]
        public string               game_id     { get; set; }

        /// <summary>
        /// <para>The start date/time for the requested report(s). If <see cref="started_at"/> is specified, <see cref="ended_at"/> must also be specified.</para>
        /// <para>If <see cref="started_at"/> is earlier than the default start date, the default start date is used.</para>
        /// </summary>
        [QueryParameter("started_at", typeof(RFC3339QueryFormatter))]
        public DateTime?            started_at  { get; set; }

        /// <summary>
        /// <para>The end date/time for the requested report(s). If <see cref="ended_at"/> is specified, <see cref="started_at"/> must also be specified.</para>
        /// <para>If <see cref="ended_at"/> is later than the default end date (1 - 2 days depending on availability), the default end date is used.</para>
        /// </summary>
        [QueryParameter("ended_at", typeof(RFC3339QueryFormatter))]
        public DateTime?            ended_at    { get; set; }

        /// <summary>
        /// <para>The type of analytics report to request.
        /// <para>If this is not specified, all report types are included for the authenticated user's games.</para>
        /// </summary>
        [QueryParameter("type")]
        public GameAnalyticsType?   type        { get; set; }

        /// <summary>
        /// Creates a new blank instance of the <see cref="GameAnalyticsQueryParameters"/> class.
        /// </summary>
        public GameAnalyticsQueryParameters()
        {

        }
    }
}
