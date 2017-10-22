// imported .dll's
using Newtonsoft.Json;

namespace TwitchNet.Models.Api.Streams
{
    public class
    Overwatch
    {
        /// <summary>
        /// Overwatch metadata about the broadcaster.
        /// </summary>
        [JsonProperty("broadcaster")]
        public Player<OverwatchHero> broadcaster { get; protected set; }        
    }
}
