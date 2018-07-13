namespace
TwitchNet.Rest.Api.Users
{
    public class
    FollowsParameters : HelixQueryParameters, IHelixQueryParameters
    {
        /// <summary>
        /// A user's id.
        /// The request returns information about users who are being followed by the this user.
        /// </summary>
        [QueryParameter("from_id", typeof(int))]
        public virtual string from_id   { get; set; }

        /// <summary>
        /// A user's id.
        /// The request returns information about users who are following this user.
        /// </summary>
        [QueryParameter("to_id")]
        public virtual string to_id     { get; set; }
    }
}
