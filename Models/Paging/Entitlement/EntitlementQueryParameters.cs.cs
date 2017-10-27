//standard namespaces
using System;

using TwitchNet.Extensions;

// project namespaces
using TwitchNet.Enums.Api.Entitlement;

namespace TwitchNet.Models.Paging.Entitlement
{
    //TODO: Test to see if these paging parameters function properly
    public class
    EntitlementQueryParameters
    {
        #region Fields

        private QueryParameter _manifest_id = new QueryParameter();

        private QueryEnum<EntitlementType> _type = new QueryEnum<EntitlementType>();

        #endregion

        #region Properties

        /// <summary>
        /// Unique identifier of the manifest file to be uploaded. Must be 1-64 characters.
        /// </summary>
        [QueryParameter("manifest_id")]
        public string manifest_id
        {
            get
            {
                return _manifest_id.value;
            }
            set
            {
                if(!value.Length.IsInRange(1, 64))
                {
                    throw new ArgumentOutOfRangeException(nameof(manifest_id), value, nameof(manifest_id) + " must be between 1 and 64 characters, inclusive.");
                }

                _manifest_id.value = value;
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
                return _type.value;
            }
            set
            {
                _type.value = value;
            }
        }

        #endregion

        #region Contstructor

        public EntitlementQueryParameters()
        {

        }

        #endregion
    }
}
