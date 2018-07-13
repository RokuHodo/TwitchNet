namespace
TwitchNet.Rest.Api.Users
{
    public class
    ActiveExtensionsParameters
    {
        /// <summary>
        /// The ID of the user whose installed active extensions will be returned.
        /// </summary>
        [QueryParameter("user_id")]
        public virtual string user_id { get; set; }
    }
}
