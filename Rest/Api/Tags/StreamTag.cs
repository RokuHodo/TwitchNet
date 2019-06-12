// standard namespaces
using System.Collections.Generic;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Tags
{
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
        public string is_auto { get; protected set; }

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
