namespace
TwitchNet.Rest.Api.Users
{
    public class
    ActiveExtensionsQueryParameters
    {
        /// <summary>
        /// The ID of the user whose installed active extensions will be returned.
        /// </summary>
        [QueryParameter("user_id")]
        public string user_id { get; set; }

        /// <summary>
        /// Creates a new blank instance of the <see cref="ActiveExtensionsQueryParameters"/> class.
        /// </summary>
        public ActiveExtensionsQueryParameters()
        {

        }
    }
}
