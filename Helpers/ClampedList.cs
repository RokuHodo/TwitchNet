// standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Extensions;

namespace
TwitchNet.Helpers
{
    public class
    ClampedList<type>
    {
        private uint        _capactity              = 100;

        private List<type>  _values                 = new List<type>();

        /// <summary>
        /// The list values.
        /// Clamped between 0 and 100 elements.
        /// </summary>
        public List<type> values
        {
            get
            {
                return _values;
            }
            set
            {
                if(_values.Count < _capactity)
                {
                    _values = value;
                }
            }
        }

        public ClampedList()
        {
            _values = new List<type>((int)_capactity);
        }

        public ClampedList(uint capactity)
        {
            _capactity = capactity;
            _values = new List<type>((int)_capactity);
        }
    }
}
