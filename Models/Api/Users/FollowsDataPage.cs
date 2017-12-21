//imported .dll's
using Newtonsoft.Json;

namespace TwitchNet.Models.Api.Users
{
    public class
    FollowsDataPage<data_type> : DataPage<data_type>
    {
        /// <summary>
        /// How many followers a user has or how many users a user is following, depending on what is being requested.
        /// </summary>
        [JsonProperty("total")]
        public string total { get; protected set; }
    }
}
