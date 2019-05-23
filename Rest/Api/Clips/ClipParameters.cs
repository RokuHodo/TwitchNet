using System;
using System.Collections.Generic;

namespace
TwitchNet.Rest.Api.Clips
{
    public class
    ClipParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// The cursor that tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [QueryParameter("before")]
        public virtual string before { get; set; }

        /// <summary>
        /// <para>The user ID of a broadcaster.</para>
        /// <para>Only one or more clip ID, one broadcaster ID, or one game ID can be provided with each request.</para>
        /// </summary>
        [QueryParameter("broadcaster_id")]
        public virtual string broadcaster_id { get; set; }

        /// <summary>
        /// <para>The ID of a game.</para>
        /// <para>Only one or more clip ID, one broadcaster ID, or one game ID can be provided with each request.</para>
        /// </summary>
        [QueryParameter("game_id")]
        public virtual string game_id { get; set; }

        /// <summary>
        /// <para>
        /// A list of clip ID's, up to 100.
        /// All elements that are null, empty, or contain only whitespace are filtered out and all duplicate elements are removed before calculating the final count.
        /// All other optional parameters are ignored if video ID's are provited.
        /// </para>
        /// <para>Only one or more clip ID, one broadcaster ID, or one game ID can be provided with each request.</para>
        /// </summary>
        [QueryParameter("id", typeof(SeparateQueryConverter))]
        public virtual List<string> ids { get; set; }

        /// <summary>
        /// <para>
        /// The latest date/time that a clip will be returned.
        /// The resolved seconds are ignored.
        /// </para>
        /// <para>
        /// If specified, started_at should also be provided.
        /// If no started_at is specified, the time period is ignored.
        /// </para>
        /// </summary>
        [QueryParameter("ended_at", typeof(RFC3339QueryConverter))]
        public virtual DateTime? ended_at { get; set; }

        /// <summary>
        /// <para>
        /// The earliest date/time that a clip will be returned.
        /// The resolved seconds are ignored.
        /// </para>
        /// <para>
        /// If specified, ended_at should also be provided.
        /// If no ended_at is specified, the time period is ignored.
        /// </para>
        /// </summary>
        [QueryParameter("started_at", typeof(RFC3339QueryConverter))]
        public virtual DateTime? started_at { get; set; }

        public ClipParameters()
        {
            ids = new List<string>();
        }
    }
}
