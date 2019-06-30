// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Helix
{
    public class
    StreamMetadata
    {
        /// <summary>
        /// The id of the user who is streaming.
        /// </summary>
        [JsonProperty("user_id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// The login of the user who is streaming.
        /// </summary>
        [JsonProperty("user_name")]
        public string user_name { get; protected set; }

        /// <summary>
        /// The id of the game being played.
        /// </summary>
        [JsonProperty("game_id")]
        public string game_id { get; protected set; }

        /// <summary>
        /// The Overwatch metadata of the current stream, if that is the game being played.
        /// </summary>
        [JsonProperty("overwatch")]
        public OverwatchMetadata overwatch { get; protected set; }

        /// <summary>
        /// The Hearthstone metadata of the current stream, if that is the game being played.
        /// </summary>
        [JsonProperty("hearthstone")]
        public HearthstoneMetadata hearthstone { get; protected set; }
    }

    public class
    OverwatchMetadata
    {
        /// <summary>
        /// Overwatch metadata about the broadcaster.
        /// </summary>
        [JsonProperty("broadcaster")]
        public Player<OverwatchHero> broadcaster { get; protected set; }
    }

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

    public class
    HearthstoneMetadata
    {
        /// <summary>
        /// Hearthstone metadata about the broadcaster.
        /// </summary>
        [JsonProperty("broadcaster")]
        public Player<HearthstoneHero> broadcaster { get; protected set; }

        /// <summary>
        /// Hearthstone metadata about the opponent.
        /// </summary>
        [JsonProperty("opponent")]
        public Player<HearthstoneHero> opponent { get; protected set; }
    }

    public class
    HearthstoneHero
    {
        /// <summary>
        /// The type of the Hearthstone hero.
        /// </summary>
        [JsonProperty("type")]
        public string type { get; protected set; }

        /// <summary>
        /// The name of the Hearthstone hero.
        /// </summary>
        [JsonProperty("name")]
        public string name { get; protected set; }

        /// <summary>
        /// The class of the Hearthstone hero.
        /// </summary>
        [JsonProperty("class")]
        public string @class { get; protected set; }
    }

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