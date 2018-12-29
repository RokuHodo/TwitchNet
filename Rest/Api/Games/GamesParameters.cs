// standard namespaces
using System.Collections.Generic;

namespace
TwitchNet.Rest.Api.Games
{
    public class
    GamesParameters
    {
        /// <summary>
        /// <para>A list of game ID's to query.</para>
        /// <para>
        /// A maximum of 100 total elements can be specified between ids and names.
        /// All elements that are null, empty, or only contain whitespace are filtered out and all duplicate elements are removed before calculating the final count.
        /// </para>
        /// </summary>
        [QueryParameter("id", typeof(SeparateQueryConverter))]
        public virtual List<string> ids { get; set; }

        /// <summary>
        /// <para>A list of game names to query.</para>
        /// <para>
        /// A maximum of 100 total elements can be specified between ids and names.
        /// All elements that are null, empty, or only contain whitespace are filtered out and all duplicate elements are removed before calculating the final count.
        /// </para>
        /// </summary>
        [QueryParameter("name", typeof(SeparateQueryConverter))]
        public virtual List<string> names { get; set; }

        public GamesParameters()
        {
            ids = new List<string>();

            names  = new List<string>();
        }
    }
}
