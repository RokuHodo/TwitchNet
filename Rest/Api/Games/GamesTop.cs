namespace
TwitchNet.Rest.Api.Games
{
    public class
    TopGamesParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// The cursor that tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [QueryParameter("before")]
        public virtual string before { get; set; }
    }
}
