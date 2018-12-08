// standard namespaces
using System.Collections.Generic;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Users
{
    public class
    ActiveExtensions
    {
        /// <summary>
        /// Contains data for the extensions a user has active.
        /// </summary>
        [JsonProperty("data")]
        public ActiveExtensionsData data { get; set; }

        public
        ActiveExtensions()
        {
            data = new ActiveExtensionsData();            
        }
    }
}
