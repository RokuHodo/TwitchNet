// project namespaces
using TwitchNet.Interfaces.Models.Paging;

namespace TwitchNet.Models.Paging.Users
{
    public class
    FollowsQueryParameters : QueryParametersPage, IQueryParametersPage
    {
        #region Fields

        private string _before;
        private string _from_id;
        private string _to_id;

        #endregion

        #region Properties

        /// <summary>
        /// The cursor that tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [QueryParameter("before", false)]
        public string before
        {
            get
            {
                return _before;
            }
            set
            {
                _before = value;
            }
        }

        /// <summary>
        /// A user's id.
        /// The request returns information about users who are being followed by the this user
        /// </summary>
        [QueryParameter("from_id")]
        public string from_id
        {
            get
            {
                return _from_id;
            }
            set
            {
                _from_id = value;
            }
        }

        /// <summary>
        /// A user's id.
        /// The request returns information about users who are following this user
        /// </summary>
        [QueryParameter("to_id")]
        public string to_id
        {
            get
            {
                return _to_id;
            }
            set
            {
                _to_id = value;
            }
        }

        #endregion

        #region Contstructor

        public FollowsQueryParameters()
        {

        }

        #endregion
    }
}
