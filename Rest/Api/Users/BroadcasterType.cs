// standard namespaces
using System.Runtime.Serialization;

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
        [EnumMember(Value = "")]
        Empty       = 0,

        /// <summary>
        /// The broadcaster is a Twitch partner.
        /// </summary>
        [EnumMember(Value = "partner")]
        Partner     = 1,

        /// <summary>
        /// The broadcaster is a Twitch affiliate.
        /// </summary>
        [EnumMember(Value = "affiliate")]
        Affiliate   = 2
    }
}
