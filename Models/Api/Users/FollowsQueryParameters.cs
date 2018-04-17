// project namespaces
using TwitchNet.Interfaces.Api;

namespace
TwitchNet.Models.Api.Users
{
    public class
    FollowsQueryParameters : QueryParameters, IQueryParameters
    {
        #region Fields

        private string _from_id;
        private string _to_id;

        #endregion

        #region Properties

        /// <summary>
        /// A user's id.
        /// The request returns information about users who are being followed by the this user.
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
        /// The request returns information about users who are following this user.
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

        #region Contstructors

        public FollowsQueryParameters()
        {

        }

        #endregion
    }
}
