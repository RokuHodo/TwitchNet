// project namespaces
using TwitchNet.Enums.Api.Entitlements;

namespace
TwitchNet.Models.Api.Entitlements
{
    public class
    EntitlementQueryParameters
    {
        private string              _manifest_id;

        private EntitlementType?    _type;

        /// <summary>
        /// Unique identifier of the manifest file to be uploaded. Must be 1-64 characters.
        /// </summary>
        [QueryParameter("manifest_id")]
        public string manifest_id
        {
            get
            {
                return _manifest_id;
            }
            set
            {
                _manifest_id = value;
            }
        }

        /// <summary>
        /// Determines the entitlement being dropped.
        /// </summary>
        [QueryParameter("type")]
        public EntitlementType? type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        public EntitlementQueryParameters()
        {

        }
    }
}
