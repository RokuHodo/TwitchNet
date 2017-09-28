// standard namespaces
using System.Collections.Generic;

// imported .dll's
using Newtonsoft.Json;

namespace TwitchNet.Models.Api.Users
{
    public class Follows
    {
        [JsonProperty("data")]
        public List<Follower>   data        { get; protected set; }

        [JsonProperty("pagination")]
        public Pagination       pagination  { get; protected set; }
    }
}
