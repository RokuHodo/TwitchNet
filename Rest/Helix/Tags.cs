// standard namespaces
using System.Collections.Generic;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Helix
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

    public class
    StreamTag
    {
        /// <summary>
        /// The tag ID.
        /// </summary>
        [JsonProperty("tag_id")]
        public string tag_id { get; protected set; }

        /// <summary>
        /// Whether or not this is a tag automatically added by Twitch.
        /// If set to true, this tag cannot be manually added or removed.
        /// </summary>
        [JsonProperty("is_auto")]
        public bool is_auto { get; protected set; }

        // TODO: /tags/streams - Generate a list of each language and swap out the string key for an enum.
        /// <summary>
        /// Localized tag names.
        /// </summary>
        [JsonProperty("localization_names")]
        public Dictionary<string, string> localization_names { get; protected set; }

        /// <summary>
        /// Localized tag descriptions.
        /// </summary>
        [JsonProperty("localization_descriptions")]
        public Dictionary<string, string> localization_descriptions { get; protected set; }
    }
}
