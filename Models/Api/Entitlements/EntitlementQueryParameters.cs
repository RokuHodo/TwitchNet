// project namespaces
using TwitchNet.Enums.Api.Entitlements;

namespace
TwitchNet.Models.Api.Entitlements
{
    public class
    EntitlementQueryParameters
    {
        /// <summary>
        /// Unique identifier of the manifest file to be uploaded. Must be 1-64 characters.
        /// </summary>
        [QueryParameter("manifest_id")]
        public string           manifest_id { get; set; }

        /// <summary>
        /// Determines the entitlement being dropped.
        /// </summary>
        [QueryParameter("type")]
        public EntitlementType? type        { get; set; }

        /// <summary>
        /// Creates a new blank instance of the <see cref="EntitlementQueryParameters"/> class.
        /// </summary>
        public EntitlementQueryParameters()
        {

        }
    }
}
