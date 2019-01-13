// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Users
{
    public class
    ActiveExtensions
    {
        /// <summary>
        /// Contains the active extension data for each extension type.
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
