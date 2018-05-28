// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Streams
{
    public class
    Metadata
    {
        /// <summary>
        /// The id of the user who is streaming.
        /// </summary>
        [JsonProperty("user_id")]
        public string       user_id     { get; protected set; }

        /// <summary>
        /// The id of the game being played.
        /// </summary>
        [JsonProperty("game_id")]
        public string       game_id     { get; protected set; }

        /// <summary>
        /// The Overwatch metadata of the current stream, if that is the game being played.
        /// </summary>
        [JsonProperty("overwatch")]
        public Overwatch    overwatch   { get; protected set; }

        /// <summary>
        /// The Hearthstone metadata of the current stream, if that is the game being played.
        /// </summary>
        [JsonProperty("hearthstone")]
        public Hearthstone  hearthstone { get; protected set; }
    }
}
