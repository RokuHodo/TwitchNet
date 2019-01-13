//standard namespaces
using System.Collections.Generic;

namespace
TwitchNet.Rest.Api.Streams
{
    public class
    StreamsParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// <para>A list of communities to query.</para>
        /// <para>
        /// A maximum of 100 total elements can be specified.
        /// All elements that are null, empty, or contain only whitespace are filtered out and all duplicate elements are removed before calculating the final count.
        /// </para>
        /// </summary>
        [QueryParameter("community_id", typeof(SeparateQueryConverter))]
        public virtual List<string>     community_ids   { get; set; }

        /// <summary>
        /// <para>A list of game ID's to query.</para>
        /// <para>
        /// A maximum of 100 total elements can be specified.
        /// All elements that are null, empty, or contain only whitespace are filtered out and all duplicate elements are removed before calculating the final count.
        /// </para>
        /// </summary>
        [QueryParameter("game_id", typeof(SeparateQueryConverter))]
        public virtual List<string>     game_ids        { get; set; }

        /// <summary>
        /// The language of the stream.
        /// This is the language selected at the home page, not the language found in the Twitch dashboard.
        /// Bitfield enum.
        /// </summary>
        [QueryParameter("language", typeof(SeparateQueryConverter))]
        public virtual StreamLanguage?  language        { get; set; }

        /// <summary>
        /// <para>A list of user ID's to query.</para>
        /// <para>
        /// A maximum of 100 total elements can be specified.
        /// All elements that are null, empty, or contain only whitespace are filtered out and all duplicate elements are removed before calculating the final count.
        /// </para>
        /// </summary>
        [QueryParameter("user_id", typeof(SeparateQueryConverter))]
        public virtual List<string>     user_ids        { get; set; }

        /// <summary>
        /// <para>A list of user login names to query.</para>
        /// <para>
        /// A maximum of 100 total elements can be specified.
        /// All elements that are null, empty, or contain only whitespace are filtered out and all duplicate elements are removed before calculating the final count.
        /// </para>
        /// </summary>
        [QueryParameter("user_login", typeof(SeparateQueryConverter))]
        public virtual List<string>     user_logins     { get; set; }

        /// <summary>
        /// The cursor that tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [QueryParameter("before")]
        public string                   before          { get; set; }

        public StreamsParameters()
        {
            community_ids   = new List<string>();
            game_ids        = new List<string>();
            user_ids        = new List<string>();
            user_logins     = new List<string>();
        }
    }
}
