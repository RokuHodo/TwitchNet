// standard nsamespaces
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Helpers.Json;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Entitlements
{
    public class
    EntitlementsUploadParameters
    {
        /// <summary>
        /// Unique identifier of the manifest file to be uploaded. Must be 1-64 characters.
        /// </summary>
        [QueryParameter("manifest_id")]
        public virtual string manifest_id { get; set; }

        /// <summary>
        /// Determines the entitlement being dropped.
        /// </summary>
        [QueryParameter("type")]
        public virtual EntitlementType type { get; set; }
    }

    [JsonConverter(typeof(EnumConverter))]
    public enum
    EntitlementType
    {
        /// <summary>
        /// Bulk drops grant entitlement.
        /// </summary>
        [EnumMember(Value = "bulk_drops_grant")]
        BulkDropsGrant = 0
    }

    public class
    EntitlementUploadUrl
    {
        /// <summary>
        /// The URL where you will upload the manifest file.
        /// This is the URL of a pre-signed S3 bucket.
        /// Lease time: 15 minutes.
        /// </summary>
        [JsonProperty("url")]
        public string url { get; protected set; }
    }
}
