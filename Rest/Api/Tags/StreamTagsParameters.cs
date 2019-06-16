// standard namespaces
using System.Collections.Generic;

namespace
TwitchNet.Rest.Api.Tags
{
    public class
    StreamTagsParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// <para>A list of tag ID's, up to 100. All other parameters are ignored if tag ID's are provited.</para>
        /// <para>All elements that are null, empty, or contain only whitespace are filtered out and all duplicate elements are removed before calculating the final count.</para>
        /// </summary>
        [QueryParameter("tag_id", typeof(SeparateQueryConverter))]
        public virtual List<string> tag_ids { get; set; }

        public StreamTagsParameters()
        {
            tag_ids = new List<string>();
        }
    }
}
