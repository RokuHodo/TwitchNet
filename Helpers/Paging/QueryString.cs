// project namespaces
using TwitchNet.Extensions;

namespace TwitchNet.Helpers.Paging
{
    internal class QueryString
    {
        private string _value;
        private string _value_default = string.Empty;

        /// <summary>
        /// The string value to be added as a query parameter.
        /// </summary>
        public string value
        {
            get
            {
                return _value.isValid() ? _value : _value_default;
            }
            set
            {
                _value = value;
            }
        }

        public QueryString()
        {

        }

        public QueryString(string value)
        {
            this.value = value;
        }
    }
}
