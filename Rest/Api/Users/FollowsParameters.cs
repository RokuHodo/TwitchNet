namespace
TwitchNet.Rest.Api.Users
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
}
