// standard namespaces
using System;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Users
{
    public class
    Follow
    {
        /// <summary>
        /// A user's id.
        /// Returned information about users who are being followed by the this user.
        /// </summary>
        [JsonProperty("from_id")]
        public string   from_id     { get; protected set; }

        /// <summary>
        /// A user's id.
        /// Returned information about users who are following this user.
        /// </summary>
        [JsonProperty("to_id")]
        public string   to_id       { get; protected set; }

        /// <summary>
        /// The date that the user followed.
        /// </summary>
        [JsonProperty("followed_at")]
        public DateTime followed_at { get; protected set; }
    }
}
