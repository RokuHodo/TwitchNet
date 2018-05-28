// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Games
{
    public class
    Game
    {
        /// <summary>
        /// The game id.
        /// </summary>
        [JsonProperty("id")]
        public string id            { get ; protected set; }

        /// <summary>
        /// The game name.
        /// </summary>
        [JsonProperty("name")]
        public string name          { get; protected set; }

        /// <summary>
        /// The template URL for the game’s box art.
        /// The {width} amd {height} parameters should be replaced with the desired values before navigating to the url.
        /// </summary>
        [JsonProperty("box_art_url")]
        public string box_art_url   { get; protected set; }
    }
}
