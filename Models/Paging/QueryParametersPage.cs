using TwitchNet.Interfaces.Models.Paging;

namespace TwitchNet.Models.Paging
{
    public class
    QueryParametersPage : IQueryParametersPage
    {
        private QueryComparable<ushort> _first = new QueryComparable<ushort>(1, 100, 20);

        private QueryParameter _after = new QueryParameter();

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
                return _after.value;
            }
            set
            {
                _after.value = value;
            }
        }
    }
}
