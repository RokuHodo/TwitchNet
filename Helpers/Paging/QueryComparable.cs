// standard namespaces
using System;

// project namespaces
using TwitchNet.Extensions;

namespace TwitchNet.Helpers.Paging
{
    internal class
    QueryComparable<type>
    where type : IComparable<type>
    {
        private type _value;
        private type _value_min;
        private type _value_max;
        private type _value_default;

        /// <summary>
        /// The comparable value to be added as a query parameter.
        /// </summary>
        public type value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value.Clamp(_value_min, _value_max, _value_default);
            }
        }

        public QueryComparable(type value_default)
        {
            _value_min = value_default;
            _value_max = value_default;
            _value_default = value_default;
            _value = _value_default;
        }

        public QueryComparable(type value_min, type value_max, type value_default)
        {
            _value_min      = value_min;
            _value_max      = value_max.ClampMin(_value_min);
            _value_default  = value_default.Clamp(_value_min, _value_max);
            _value = _value_default;
        }
    }
}
