// standard namespaces
using System.Runtime.Serialization;

// imported namespaces
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TwitchNet.Enums.Api.Users
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BroadcasterType
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
