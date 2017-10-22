// standard namespaces
using System;

namespace TwitchNet.Models.Paging
{
    internal class
    QueryEnum<type>
    where type : struct, IConvertible
    {
        private type? _value;
        private type? _value_default;

        /// <summary>
        /// The enum value to be added as a query parameter.
        /// </summary>
        public type? value
        {
            get
            {
                return _value ?? _value_default;
            }
            set
            {
                _value = value;
            }
        }

        public QueryEnum(type? value_default = null)
        {
            _value_default = value_default;
            _value = _value_default;
        }
    }
}
