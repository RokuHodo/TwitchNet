// project namespaces
using TwitchNet.Helpers.Json;

// imported namespaces
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Entitlements
{
    // TODO: Add custom converter for the deserializer.
    [JsonConverter(typeof(EnumCacheConverter))]
    public enum
    EntitlementType
    {
        /// <summary>
        /// Bulk drops grant entitlement.
        /// </summary>
        BulkDropsGrant = 0
    }
}
