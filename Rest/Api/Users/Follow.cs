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
        /// <para>The ID of the following user.</para>
        /// <para>
        /// This value will be constant when a user's following list is returned.
        /// This value will vary when a user's follower list is returned.
        /// </para>
        /// </summary>
        [JsonProperty("from_id")]
        public string   from_id     { get; protected set; }

        /// <summary>
        /// <para>The login name of the following user.</para>
        /// <para>
        /// This value will be constant when a user's following list is returned.
        /// This value will vary when a user's follower list is returned.
        /// </para>
        /// </summary>
        [JsonProperty("from_name")]
        public string from_name { get; protected set; }

        /// <summary>
        /// <para>The ID of the followed user.</para>
        /// <para>
        /// This value will vary when a user's following list is returned.
        /// This value will be constant when a user's follower list is returned.
        /// </para>
        /// </summary>
        [JsonProperty("to_id")]
        public string   to_id       { get; protected set; }

        /// <summary>
        /// <para>The login name of the followed user.</para>
        /// <para>
        /// This value will vary when a user's following list is returned.
        /// This value will be constant when a user's follower list is returned.
        /// </para>
        /// </summary>
        [JsonProperty("to_name")]
        public string to_name { get; protected set; }

        /// <summary>
        /// The date that the from_id user followed the to_id user.
        /// </summary>
        [JsonProperty("followed_at")]
        public DateTime followed_at { get; protected set; }
    }
}
