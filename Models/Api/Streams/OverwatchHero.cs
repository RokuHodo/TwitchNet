// imported .dll's
using Newtonsoft.Json;

namespace TwitchNet.Models.Api.Streams
{
    public class
    OverwatchHero
    {
        /// <summary>
        /// The role of the Overwatch hero.
        /// </summary>
        [JsonProperty("role")]
        public string role      { get; protected set; }

        /// <summary>
        /// The name of the Overwatch hero.
        /// </summary>
        [JsonProperty("name")]
        public string name      { get; protected set; }

        /// <summary>
        /// The ability being used by the broadcaster.
        /// </summary>
        [JsonProperty("ability")]
        public string ability   { get; protected set; }
    }
}
