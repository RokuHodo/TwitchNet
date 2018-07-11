// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Users
{
    public class
    ActiveExtensionsData
    {
        /// <summary>
        /// Contains data for the extensions a user has active.
        /// </summary>
        [JsonProperty("data")]
        public ActiveExtensionsTypes data { get; protected set; }
    }
}
