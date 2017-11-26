// project namespaces
using TwitchNet.Helpers;
using TwitchNet.Interfaces.Api;

namespace
TwitchNet.Models.Api
{
    public class
    QueryParametersPage : IQueryParametersPage
    {
        private ClampedNumber<ushort>   _first  = new ClampedNumber<ushort>(1, 100, 20);

        private string                  _after  = string.Empty;

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
                return _first.value;
            }
            set
            {
                _first.value = value;
            }
        }

        /// <summary>
        /// The cursor that tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [QueryParameter("after", false)]
        public string after
        {
            get
            {
                return _after;
            }
            set
            {
                _after = value;
            }
        }
    }
}
