// project namespaces
using TwitchNet.Extensions;

namespace TwitchNet.Helpers.Paging.Users
{
    public class FollowsQueryParameters
    {
        #region Fields

        private ushort _first;
        private ushort _first_min       = 1;
        private ushort _first_max       = 100;
        private ushort _first_default   = 20;

        private string _after;
        private string _after_default   = string.Empty;

        private string _before;
        private string _before_default  = string.Empty;

        private string _from_id;
        private string _from_id_default = string.Empty;

        private string _to_id;
        private string _to_id_default   = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Maximum number of objects to return.
        /// Minimum: 1;
        /// Maximum: 100.
        /// Default: 20.
        /// </summary>
        [QueryParameter("first")]
        public ushort first
        {
            get
            {
                return _first.IsDefault() ? _first_default : _first;
            }
            set
            {
                _first = value.Clamp(_first_min, _first_max, _first_default);
            }
        }

        /// <summary>
        /// The cursor that tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [QueryParameter("after")]
        public string after
        {
            get
            {
                return _after ?? _after_default;
            }
            set
            {
                _after = value;
            }
        }

        /// <summary>
        /// The cursor that tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [QueryParameter("before")]
        public string before
        {
            get
            {
                return _before ?? _before_default;
            }
            set
            {
                _before = value;
            }
        }

        /// <summary>
        /// A user's id.
        /// The request returns information about users who are being followed by the this user
        /// </summary>
        [QueryParameter("from_id")]
        public string from_id
        {
            get
            {
                return _from_id ?? _from_id_default;
            }
            set
            {
                _from_id = value;
            }
        }

        /// <summary>
        /// A user's id.
        /// The request returns information about users who are following this user
        /// </summary>
        [QueryParameter("to_id")]
        public string to_id
        {
            get
            {
                return _to_id ?? _to_id_default;
            }
            set
            {
                _to_id = value;
            }
        }

        #endregion

        #region Contstructor

        public FollowsQueryParameters()
        {

        }

        #endregion
    }
}
