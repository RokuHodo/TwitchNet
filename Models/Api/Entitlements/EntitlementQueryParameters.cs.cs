//standard namespaces
using System;

// project namespaces
using TwitchNet.Extensions;

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
                if(!value.Length.IsInRange(1, 64))
                {
                    throw new ArgumentOutOfRangeException(nameof(manifest_id), value, nameof(manifest_id) + " must be between 1 and 64 characters, inclusive.");
                }

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
