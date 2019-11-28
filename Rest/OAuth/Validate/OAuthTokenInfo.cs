
using Newtonsoft.Json;

namespace TwitchNet.Rest.OAuth.Validate
{
    public class
    OAuthTokenInfo
    {
        [JsonProperty("client_id")]
        public string   client_id   { get; protected set; }

        [JsonProperty("login")]
        public string   login       { get; protected set; }

        [JsonProperty("user_id")]
        public string   user_id     { get; protected set; }

        [JsonProperty("scopes")]
        public HelixScopes[] scopes      { get; protected set; }
    }
}
