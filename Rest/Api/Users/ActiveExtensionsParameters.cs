namespace
TwitchNet.Rest.Api.Users
{
    public class
    ActiveExtensionsParameters
    {
        /// <summary>
        /// The ID of the user to query.
        /// </summary>
        [QueryParameter("user_id")]
        public virtual string user_id { get; set; }
    }
}
