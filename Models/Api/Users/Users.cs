// standard namespaces
using System.Collections.Generic;

// imported .dll's
using Newtonsoft.Json;

namespace TwitchNet.Models.Api.Users
{
    public class Users
    {
        /// <summary>
        /// Contains the retured paged user data.
        /// </summary>
        [JsonProperty("data")]
        public List<User> data { get; protected set; }
    }
}
