// standard namespaces
using System.Collections.Generic;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Helix
{
    #region /tags

    public class
    StreamTagsParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// A list of stream tag ID's, up to 100.
        /// All other parameters are ignored if tag ID's are provited.
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
        /// The stream tag ID.
        /// </summary>
        [JsonProperty("tag_id")]
        public string tag_id { get; protected set; }

        /// <summary>
        /// Whether or not this is a stream tag automatically added by Twitch.
        /// If set to true, this stream tag cannot be manually added or removed.
        /// </summary>
        [JsonProperty("is_auto")]
        public bool is_auto { get; protected set; }

        // TODO: StreamTag - Generate a list of each language and swap out the string key for an enum.
        /// <summary>
        /// Localized stream tag names.
        /// </summary>
        [JsonProperty("localization_names")]
        public Dictionary<string, string> localization_names { get; protected set; }

        /// <summary>
        /// Localized stream tag descriptions.
        /// </summary>
        [JsonProperty("localization_descriptions")]
        public Dictionary<string, string> localization_descriptions { get; protected set; }
    }

    #endregion
}
