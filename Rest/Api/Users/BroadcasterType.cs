// imported namespaces
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace
TwitchNet.Rest.Api.Users
{
    // TODO: Add custom converter for the deserializer.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum
    BroadcasterType
    {
        /// <summary>
        /// The broadcaster is a normal user.
        /// </summary>
        Empty       = 0,

        /// <summary>
        /// The broadcaster is a Twitch partner.
        /// </summary>
        Partner     = 1,

        /// <summary>
        /// The broadcaster is a Twitch affiliate.
        /// </summary>
        Affiliate   = 2
    }
}
