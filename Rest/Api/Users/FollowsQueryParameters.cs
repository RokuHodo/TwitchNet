namespace
TwitchNet.Rest.Api.Users
{
    public class
    FollowsQueryParameters : HelixQueryParameters, IHelixQueryParameters
    {
        /// <summary>
        /// A user's id.
        /// The request returns information about users who are being followed by the this user.
        /// </summary>
        [QueryParameter("from_id")]
        public string from_id   { get; set; }

        /// <summary>
        /// A user's id.
        /// The request returns information about users who are following this user.
        /// </summary>
        [QueryParameter("to_id")]
        public string to_id     { get; set; }

        /// <summary>
        /// Creates a new blank instance of the <see cref="FollowsQueryParameters"/> class.
        /// </summary>
        public FollowsQueryParameters()
        {

        }
    }
}
