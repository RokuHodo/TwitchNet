// project namespaces

namespace
TwitchNet.Rest.Api.Entitlements
{
    public class
    EntitlementParameters
    {
        /// <summary>
        /// Unique identifier of the manifest file to be uploaded. Must be 1-64 characters.
        /// </summary>
        [QueryParameter("manifest_id")]
        public virtual string           manifest_id { get; set; }

        /// <summary>
        /// Determines the entitlement being dropped.
        /// </summary>
        [QueryParameter("type")]
        public virtual EntitlementType type         { get; set; }
    }
}
