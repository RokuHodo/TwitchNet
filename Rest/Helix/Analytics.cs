﻿// standard namespaces
using System;
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Helpers.Json;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Helix
{
    #region /analytics/extensions

    public class
    ExtensionAnalyticsParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// <para>The ID of an extension.</para>
        /// <para>
        /// If provided, the only CSV URL returned will be for that extension ID and the after cursor will be ignored.
        /// Otherwise, CSV URL's will be returned for all extensions associated with the authenticated user.
        /// </para>
        /// </summary>
        [QueryParameter("extension_id")]
        public virtual string extension_id { get; set; }

        /// <summary>
        /// <para>
        /// The latest date that a report will be returned, in UTC.
        /// The returned reports covers up through the entire day of this date.
        /// The value is automatically converted from its current time zone and the hours, minutes, and seconds are zeroed out.
        /// </para>
        /// <para>
        /// If provided, started_at must also be provided.
        /// If the end date is later than the default end date, the default end date is used.
        /// </para>
        /// <para>Default: 1-2 days before the request was issued depending on the report availability.</para>
        /// </summary>
        [QueryParameter("ended_at", typeof(RFC3339QueryConverter))]
        public virtual DateTime? ended_at { get; set; }

        /// <summary>
        /// <para>
        /// The earliest date that a report will be returned, in UTC.
        /// The value is automatically converted from its current time zone and the hours, minutes, and seconds are zeroed out.
        /// </para>
        /// <para>
        /// If provided, ended_at should also be provided.
        /// If the date is earlier than the default start date, the default start date is used which differs depending on the report type requested.
        /// </para>
        /// <para>Default: 90 days before the report was issued (OverviewV1), January 31, 2018 (OverviewV2).</para>
        /// </summary>
        [QueryParameter("started_at", typeof(RFC3339QueryConverter))]
        public virtual DateTime? started_at { get; set; }

        /// <summary>
        /// <para>The type of report to request.</para>
        /// <para>
        /// If provided, each report will only contain one CSV URL for the corresponding type.
        /// Otherwise, a paginated report for each avialable type will be returned for each extension.
        /// </para>
        /// </summary>
        [QueryParameter("type")]
        public virtual AnalyticsType? type { get; set; }
    }

    public class
    ExtensionAnalytics
    {
        /// <summary>
        /// The ID of the extension.
        /// </summary>
        [JsonProperty("extension_id")]
        public string extension_id { get; protected set; }

        /// <summary>
        /// The URL to the downloadable CSV file containing the analytic data.
        /// The URL is valid for 1 minute.
        /// </summary>
        [JsonProperty("URL")]
        public string url { get; protected set; }

        /// <summary>
        /// The analytic report type.
        /// </summary>
        [JsonProperty("type")]
        public AnalyticsType type { get; protected set; }

        /// <summary>
        /// The time period that the report covers.
        /// </summary>
        [JsonProperty("date_range")]
        public DateRange date_range { get; protected set; }
    }

    [JsonConverter(typeof(EnumConverter))]
    public enum
    AnalyticsType
    {
        /// <summary>
        /// The 1st analytic report type.
        /// </summary>
        [EnumMember(Value = "overview_v1")]
        OverviewV1 = 0,

        /// <summary>
        /// The 2nd analytic report type.
        /// </summary>
        [EnumMember(Value = "overview_v2")]
        OverviewV2
    }

    #endregion

    #region /analytics/games

    public class
    GameAnalyticsParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// <para>The ID of an game.</para>
        /// <para>
        /// If provided, the only CSV URL returned will be for that game ID and the after cursor will be ignored.
        /// Otherwise, CSV URL's will be returned for all games associated with the authenticated user.
        /// </para>
        /// </summary>
        [QueryParameter("game_id")]
        public virtual string game_id { get; set; }

        /// <summary>
        /// <para>
        /// The latest date that a report will be returned, in UTC.
        /// The returned reports covers up through the entire day of this date.
        /// The value is automatically converted from its current time zone and the hours, minutes, and seconds are zeroed out.
        /// </para>
        /// <para>
        /// If provided, started_at must also be provided.
        /// If the end date is later than the default end date, the default end date is used.
        /// </para>
        /// <para>Default: 1-2 days before the request was issued depending on the report availability.</para>
        /// </summary>
        [QueryParameter("ended_at", typeof(RFC3339QueryConverter))]
        public virtual DateTime? ended_at { get; set; }

        /// <summary>
        /// <para>
        /// The earliest date that a report will be returned, in UTC.
        /// The value is automatically converted from its current time zone and the hours, minutes, and seconds are zeroed out.
        /// </para>
        /// <para>
        /// If provided, ended_at should also be provided.
        /// If the start date is earlier than the default start date, the default start date is used which differs depending on the report type requested.
        /// </para>
        /// <para>Default: 90 days before the report was issued (OverviewV1), 365 days before the report was issued (OverviewV2).</para>
        /// </summary>
        [QueryParameter("started_at", typeof(RFC3339QueryConverter))]
        public virtual DateTime? started_at { get; set; }

        /// <summary>
        /// <para>The type of report to request.</para>
        /// <para>
        /// If provided, each report will only contain one CSV URL for the corresponding type.
        /// Otherwise, a paginated report for each avialable type will be returned for each game.
        /// </para>
        /// </summary>
        [QueryParameter("type")]
        public virtual AnalyticsType? type { get; set; }
    }

    public class
    GameAnalytics
    {
        /// <summary>
        /// The ID of the game.
        /// </summary>
        [JsonProperty("game_id")]
        public string game_id { get; protected set; }

        /// <summary>
        /// The URL to the downloadable CSV file containing the analytic data.
        /// the URL is valid for 1 minute.
        /// </summary>
        [JsonProperty("URL")]
        public string url { get; protected set; }

        /// <summary>
        /// The analytic report type.
        /// </summary>
        [JsonProperty("type")]
        public AnalyticsType type { get; protected set; }

        /// <summary>
        /// The time period that the report covers.
        /// </summary>
        [JsonProperty("date_range")]
        public DateRange date_range { get; protected set; }
    }

    #endregion
}