// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Streams
{
    public class
    StreamMetadata
    {
        /// <summary>
        /// The id of the user who is streaming.
        /// </summary>
        [JsonProperty("user_id")]
        public string               user_id     { get; protected set; }

        /// <summary>
        /// The login of the user who is streaming.
        /// </summary>
        [JsonProperty("user_name")]
        public string               user_name   { get; protected set; }

        /// <summary>
        /// The id of the game being played.
        /// </summary>
        [JsonProperty("game_id")]
        public string               game_id     { get; protected set; }

        /// <summary>
        /// The Overwatch metadata of the current stream, if that is the game being played.
        /// </summary>
        [JsonProperty("overwatch")]
        public OverwatchMetadata    overwatch   { get; protected set; }

        /// <summary>
        /// The Hearthstone metadata of the current stream, if that is the game being played.
        /// </summary>
        [JsonProperty("hearthstone")]
        public HearthstoneMetadata  hearthstone { get; protected set; }
    }
}
