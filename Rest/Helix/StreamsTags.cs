// standard namspaces
using System.Collections.Generic;

namespace
TwitchNet.Rest.Helix
{
    public class
    StreamsTagsParameters
    {
        /// <summary>
        /// The user ID of a broadcaster.
        /// </summary>
        [QueryParameter("broadcaster_id")]
        public virtual string broadcaster_id { get; set; }
    }

    public class
    SetStreamsTagsParameters
    {
        /// <summary>
        /// A user ID to update the stream tags for.
        /// </summary>
        [QueryParameter("broadcaster_id")]
        public virtual string broadcaster_id { get; set; }

        /// <summary>
        /// <para>A list of tag ID's, up to 5.</para>
        /// <para>
        /// Automatic tags cannot be added or removed.
        /// All elements that are null, empty, or contain only whitespace are filtered out and all duplicate elements are removed before calculating the final count.
        /// </para>
        /// </summary>
        [Body("tag_ids")]
        public virtual List<string> tag_ids { get; set; }

        public SetStreamsTagsParameters()
        {
            tag_ids = new List<string>();
        }
    }
}