// standard nsamespaces
using System.Collections.Generic;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Games
{
    public class
    GamesParameters
    {
        /// <summary>
        /// <para>A list of game ID's to query.</para>
        /// <para>
        /// A maximum of 100 total elements can be specified between ids and names.
        /// All elements that are null, empty, or contain only whitespace are filtered out and all duplicate elements are removed before calculating the final count.
        /// </para>
        /// </summary>
        [QueryParameter("id", typeof(SeparateQueryConverter))]
        public virtual List<string> ids { get; set; }

        /// <summary>
        /// <para>A list of game names to query.</para>
        /// <para>
        /// A maximum of 100 total elements can be specified between ids and names.
        /// All elements that are null, empty, or contain only whitespace are filtered out and all duplicate elements are removed before calculating the final count.
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
        /// The template URL for the game’s box art.
        /// The {width} amd {height} parameters should be replaced with the desired values before navigating to the url.
        /// </summary>
        [JsonProperty("box_art_url")]
        public string box_art_url { get; protected set; }
    }
}
