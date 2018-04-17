// project namespaces
using TwitchNet.Interfaces.Api;

namespace
TwitchNet.Models.Api.Games
{
    public class
    TopGamesQueryParameters : QueryParameters, IQueryParameters
    {
        private string _before;

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

        public TopGamesQueryParameters()
        {

        }
    }
}
