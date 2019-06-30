// standard namespaces
using System;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Helix
{
    public class
    FollowsParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// A user's id.
        /// The request returns information about users who are being followed by the this user.
        /// </summary>
        [QueryParameter("from_id")]
        public virtual string from_id   { get; set; }

        /// <summary>
        /// A user's id.
        /// The request returns information about users who are following this user.
        /// </summary>
        [QueryParameter("to_id")]
        public virtual string to_id     { get; set; }

        public FollowsParameters()
        {

        }

        public FollowsParameters(string from_id, string to_id)
        {
            this.from_id = from_id;
            this.to_id = to_id;
        }        
    }

    public class
        FollowsDataPage<data_type> : DataPage<data_type>
    {
        /// <summary>
        /// When providing only 'from_id', 'total' represents the number of users a user is following.
        /// When providing only 'to_id', 'total' represents the total number of followers the user has.
        /// When providing both 'from_id' and 'to_id', 'total' is either '1' or '0' depending if 'from_id' is following 'to_id'.
        /// </summary>
        [JsonProperty("total")]
        public int total { get; protected set; }
    }

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
        public string from_id { get; protected set; }

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
        public string to_id { get; protected set; }

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
