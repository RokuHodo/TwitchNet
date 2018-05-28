// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Streams
{
    public class
    Player<hero_type>
    {
        /// <summary>
        /// The metadata about the hero selected by the player.
        /// </summary>
        [JsonProperty("hero")]
        public hero_type hero { get; protected set; }
    }
}
