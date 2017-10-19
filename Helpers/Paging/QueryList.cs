// standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Extensions;

namespace TwitchNet.Helpers.Paging
{
    internal class
    QueryList
    {
        private ushort          _range_start            = 0;
        private ushort          _range_count            = 100;
        private ushort          _range_count__default   = 100;

        private List<string>    _values                 = new List<string>();
        private List<string>    _values_default         = new List<string>();

        /// <summary>
        /// A list of all elements to be added as query parameters.
        /// </summary>
        public List<string> values
        {
            get
            {
                int count = _range_count > _values.Count ? _values.Count : _range_count;
                return _values.IsValid() ? _values.GetRange(_range_start, count) : _values_default;
            }
            set
            {
                _values = value;
            }
        }

        public QueryList()
        {

        }

        public QueryList(ushort capactity)
        {
            _range_count = capactity.ClampMin(_range_start, _range_count__default);
        }
    }
}
