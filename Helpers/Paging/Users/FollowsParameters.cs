using TwitchNet.Extensions;

namespace TwitchNet.Helpers.Paging.Users
{
    public class FollowsParameters
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

        [PagingProperty("first")]
        public ushort first
        {
            get
            {
                return _first.isDefault() ? _first_default : _first;
            }
            set
            {
                _first = value.Clamp(_first_min, _first_max, _first_default);
            }
        }

        [PagingProperty("after")]
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

        [PagingProperty("before")]
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

        [PagingProperty("from_id")]
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

        [PagingProperty("to_id")]
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

        public FollowsParameters()
        {

        }

        #endregion
    }
}
