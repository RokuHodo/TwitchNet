// project namespaces
using TwitchNet.Extensions;

namespace
TwitchNet.Models.Api
{
    public class
    QueryParameter
    {
        #region Fields

        private string _name;
        private string _name_default = string.Empty;

        private string _value;
        private string _value_default = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// The string value to be added as a query parameter.
        /// </summary>
        public string name
        {
            get
            {
                return _name.IsValid() ? _name : _name_default;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// The string value to be added as a query parameter.
        /// </summary>
        public string value
        {
            get
            {
                return _value.IsValid() ? _value : _value_default;
            }
            set
            {
                _value = value;
            }
        }

        #endregion

        #region Constructors

        public QueryParameter()
        {

        }

        public QueryParameter(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        #endregion
    }
}
