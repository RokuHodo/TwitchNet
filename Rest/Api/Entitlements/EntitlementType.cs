// standard namespaces
using System.Runtime.Serialization;

// imported namespaces
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace
TwitchNet.Rest.Api.Entitlements
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum
    EntitlementType
    {
        /// <summary>
        /// Bulk drops grant entitlement.
        /// </summary>
        [EnumMember(Value = "bulk_drops_grant")]
        Bulk_Drops_Grant = 0
    }
}
