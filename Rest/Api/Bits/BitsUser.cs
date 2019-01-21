// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Bits
{
    public class
    BitsUser
    {
        /// <summary>
        /// The ID of the user.
        /// </summary>
        [JsonProperty("user_id")]
        public string   user_id { get; protected set; }

        /// <summary>
        /// The login of the user.
        /// </summary>
        [JsonProperty("user_name")]
        public string user_name { get; protected set; }

        /// <summary>
        /// The rank of the user in the bits leaderboard.
        /// </summary>
        [JsonProperty("rank")]
        public uint     rank    { get; protected set; }

        /// <summary>
        /// How many bits the user has cheered.
        /// </summary>
        [JsonProperty("score")]
        public uint     score   { get; protected set; }
    }
}
