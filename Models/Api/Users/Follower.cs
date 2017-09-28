// standard namespaces
using System;

// imported .dll's
using Newtonsoft.Json;

namespace TwitchNet.Models.Api.Users
{
    public class Follower
    {
        [JsonProperty("from_id")]
        public string   from_id     { get; protected set; }

        [JsonProperty("to_id")]
        public string   to_id       { get; protected set; }

        [JsonProperty("followed_at")]
        public DateTime followed_at { get; protected set; }
    }
}
