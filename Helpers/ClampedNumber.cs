// standard namespaces
using System;

// project namespaces
using TwitchNet.Extensions;

namespace
TwitchNet.Helpers
{
    internal class
    ClampedNumber<type>
    where type : IComparable, IFormattable, IConvertible, IComparable<type>, IEquatable<type>
    {
        private type _value;
        private type _value_min;
        private type _value_default;

        internal type _value_max;

        public type value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value.Clamp(_value_min, _value_max);
            }
        }

        public ClampedNumber(type value_min, type value_max, type value_default)
        {
            _value_min      = value_min;
            _value_max      = value_max.ClampMin(_value_min);
            _value_default  = value_default.Clamp(_value_min, _value_max);
            _value          = _value_default;
        }
    }
}
