using System;

namespace
TwitchNet.Rest.Api.Analytics
{
    public class
    ExtensionAnalyticsParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// <para>The ID of an extension.</para>
        /// <para>
        /// If specified, the only CSV URL returned will be for that extension ID and the after cursor if provided will be ignored.
        /// Otherwise, CSV URL's will be returned for all extensions associated with the authenticated user.
        /// </para>
        /// </summary>
        [QueryParameter("extension_id")]
        public virtual string extension_id { get; set; }

        /// <summary>
        /// <para>
        /// The latest date/time that a report will be returned in UTC.
        /// The resolved hours, minutes, and seconds are zeroed out.
        /// If the date/time is not in UTC, it will be automatically converted from its current time zone.
        /// </para>
        /// <para>
        /// If specified, started_at must also be provided.
        /// If the end date/time is later than the default end date, the default end date is used.
        /// </para>
        /// <para>Default: 1-2 days before the request was issued depending on the report availability.</para>
        /// </summary>
        [QueryParameter("ended_at", typeof(RFC3339QueryConverter))]
        public virtual DateTime? ended_at { get; set; }

        /// <summary>
        /// <para>
        /// The earliest date/time that a report will be returned in UTC.
        /// The resolved hours, minutes, and seconds are zeroed out.
        /// If the date/time is not in UTC, it will be automatically converted from its current time zone.
        /// </para>
        /// <para>
        /// If specified, ended_at should also be provided.
        /// If the start date/time is earlier than the default start date, the default start date is used.
        /// The default date differs depending on the report type requested.
        /// </para>
        /// <para>Default: 90 days before the report was issued (OverviewV1), January 31, 2018 (OverviewV2).</para>
        /// </summary>
        [QueryParameter("started_at", typeof(RFC3339QueryConverter))]
        public virtual DateTime? started_at { get; set; }

        /// <summary>
        /// <para>The type of report to be issued.</para>
        /// <para>
        /// If specified, each report will only contain one CSV URL for the corresponding type.
        /// Otherwise, a paginated report for each avialable type will be returned for each extension.
        /// </para>
        /// </summary>
        [QueryParameter("type")]
        public virtual AnalyticsType? type { get; set; }
    }
}
