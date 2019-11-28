// standard namespaces
using System.Collections.Generic;

// project namespaces
using Newtonsoft.Json;

namespace
TwitchNet.Rest.OAuth.Token
{
    public class
    AppAccessTokenData
    {
        [JsonProperty("access_token")]
        public string access_token { get; internal set; }

        [JsonProperty("refresh_token")]
        public string refresh_token { get; internal set; }

        [JsonProperty("expires_in")]
        public int expires_in { get; internal set; }

        [JsonProperty("scope")]
        public IList<HelixScopes> scope { get; internal set; }

        // TODO: Enum?
        [JsonProperty("token_type")]
        public string token_type { get; internal set; }
    }
}
