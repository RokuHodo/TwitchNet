// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Models.Api.Streams
{
    public class
    HearthstoneHero
    {
        /// <summary>
        /// The type of the Hearthstone hero.
        /// </summary>
        [JsonProperty("type")]
        public string type      { get; protected set; }

        /// <summary>
        /// The name of the Hearthstone hero.
        /// </summary>
        [JsonProperty("name")]
        public string name      { get; protected set; }

        /// <summary>
        /// The class of the Hearthstone hero.
        /// </summary>
        [JsonProperty("class")]
        public string @class    { get; protected set; }
    }
}
