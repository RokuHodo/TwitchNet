using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Users
{
    public class
    ActiveExtensions
    {
        /// <summary>
        /// The active extension data to update.
        /// </summary>
        [JsonProperty("data")]
        public ActiveExtensionsData data { get; set; }
    }
}
