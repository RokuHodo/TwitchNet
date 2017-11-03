// standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Extensions;

namespace TwitchNet.Helpers
{
    internal class
    ClampedList<type>
    {
        private int         _range_start            = 0;
        private int         _range_count            = 100;
        private int         _range_count_default    = 100;

        private List<type>  _values                 = new List<type>();
        private List<type>  _values_default         = new List<type>();

        public List<type> values
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

        public ClampedList()
        {

        }

        public ClampedList(int capactity)
        {
            _range_count = capactity.ClampMin(_range_start, _range_count_default);
            _range_count_default = _range_count;
        }
    }
}
