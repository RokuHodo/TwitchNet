// standard nsamespaces
using System.Collections.Generic;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Helix
{
    #region /games

    public class
    GamesParameters
    {
        /// <summary>
        /// <para>A list of game ID's to query.</para>
        /// <para>
        /// A maximum of 100 total elements can be specified between ids and names.
        /// </para>
        /// </summary>
        [QueryParameter("id", typeof(SeparateQueryConverter))]
        public virtual List<string> ids { get; set; }

        /// <summary>
        /// <para>A list of game names to query.</para>
        /// <para>
        /// A maximum of 100 total elements can be specified between ids and names.
        /// </para>
        /// </summary>
        [QueryParameter("name", typeof(SeparateQueryConverter))]
        public virtual List<string> names { get; set; }

        public GamesParameters()
        {
            ids = new List<string>();

            names = new List<string>();
        }
    }

    public class
    Game
    {
        /// <summary>
        /// The game id.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; protected set; }

        /// <summary>
        /// The game name.
        /// </summary>
        [JsonProperty("name")]
        public string name { get; protected set; }

        /// <summary>
        /// The URL templatefor the game’s box art.
        /// The {width} amd {height} parameters should be replaced with the desired values before navigating to the url.
        /// </summary>
        [JsonProperty("box_art_url")]
        public string box_art_url { get; protected set; }
    }

    #endregion

    #region /games/top

    public class
    TopGamesParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// The cursor that tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [QueryParameter("before")]
        public virtual string before { get; set; }
    }

    #endregion
}
