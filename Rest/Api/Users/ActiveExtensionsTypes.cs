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
        /// <para>Contains data for panel extensions.</para>
        /// <para>Valid keys: 1, 2, 3.</para>
        /// </summary>
        [JsonProperty("panel")]
        public Dictionary<string, ActiveExtension> panel { get; set; }

        /// <summary>
        /// <para>Contains data for overlay extensions.</para>
        /// <para>Valid keys: 1.</para>
        /// </summary>
        [JsonProperty("overlay")]
        public Dictionary<string, ActiveExtension> overlay { get; set; }

        /// <summary>
        /// <para>Contains data for component extensions.</para>
        /// <para>Valid keys: 1, 2.</para>
        /// </summary>
        [JsonProperty("component")]
        public Dictionary<string, ActiveExtension> component { get; set; }

        public
        ActiveExtensionsTypes()
        {   
            component   = new Dictionary<string, ActiveExtension>();
            panel       = new Dictionary<string, ActiveExtension>();
            overlay     = new Dictionary<string, ActiveExtension>();
        }
    }
}
