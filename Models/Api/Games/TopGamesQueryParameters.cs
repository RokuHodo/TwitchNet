﻿// project namespaces
using TwitchNet.Interfaces.Api;

namespace
TwitchNet.Models.Api.Games
{
    public class
    TopGamesQueryParameters : QueryParametersPage, IQueryParameters
    {
        /// <summary>
        /// The cursor that tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [QueryParameter("before")]
        public string before { get; set; }

        /// <summary>
        /// Creates a new blank instance of the <see cref="TopGamesQueryParameters"/> class.
        /// </summary>
        public TopGamesQueryParameters()
        {

        }
    }
}
