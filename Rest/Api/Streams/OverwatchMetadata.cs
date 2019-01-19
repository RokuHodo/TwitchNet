// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Streams
{
    public class
    OverwatchMetadata
    {
        /// <summary>
        /// Overwatch metadata about the broadcaster.
        /// </summary>
        [JsonProperty("broadcaster")]
        public Player<OverwatchHero> broadcaster { get; protected set; }        
    }
}
