// project namespaces
using TwitchNet.Helpers.Json;

// imported namespaces
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Users
{
    [JsonConverter(typeof(EnumCacheConverter))]
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
