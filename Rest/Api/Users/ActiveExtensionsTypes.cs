// standard namespaces
using System.Collections.Generic;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Users
{
    public class
    ActiveExtensionsTypes
    {
        /// <summary>
        /// Contains data for panel extensions.
        /// </summary>
        [JsonProperty("panel")]
        public Dictionary<string, ActiveExtension> panel { get; set; }

        /// <summary>
        /// Contains data for overlay extensions.
        /// </summary>
        [JsonProperty("overlay")]
        public Dictionary<string, ActiveExtension> overlay { get; set; }

        /// <summary>
        /// Contains data for component extensions.
        /// </summary>
        [JsonProperty("component")]
        public Dictionary<string, ActiveExtension> component { get; set; }

        /// <summary>
        /// Contains data for mobile extensions.
        /// </summary>
        [JsonProperty("mobile")]
        public Dictionary<string, ActiveExtension> mobile { get; set; }
    }
}
