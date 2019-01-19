// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Streams
{
    public class
    HearthstoneMetadata
    {
        /// <summary>
        /// Hearthstone metadata about the broadcaster.
        /// </summary>
        [JsonProperty("broadcaster")]
        public Player<HearthstoneHero> broadcaster  { get; protected set; }

        /// <summary>
        /// Hearthstone metadata about the opponent.
        /// </summary>
        [JsonProperty("opponent")]
        public Player<HearthstoneHero> opponent     { get; protected set; }
    }
}
