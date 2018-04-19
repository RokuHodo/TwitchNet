//imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Models.Api.Users
{
    public class
    FollowsData<data_type> : Data<data_type>
    {
        /// <summary>
        /// When providing only 'from_id', 'total' represents the number of users a user is following.
        /// When providing only 'to_id', 'total' represents the total number of followers the user has.
        /// When providing both 'from_id' and 'to_id', 'total' is either '1' or '0' depending if 'from_id' is following 'to_id'.
        /// </summary>
        [JsonProperty("total")]
        public string total { get; protected set; }
    }
}
