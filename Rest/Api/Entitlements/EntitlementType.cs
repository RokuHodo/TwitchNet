// imported namespaces
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace
TwitchNet.Rest.Api.Entitlements
{
    // TODO: Add custom converter for the deserializer.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum
    EntitlementType
    {
        /// <summary>
        /// Bulk drops grant entitlement.
        /// </summary>
        BulkDropsGrant = 0
    }
}
