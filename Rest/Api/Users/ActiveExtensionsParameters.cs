namespace
TwitchNet.Rest.Api.Users
{
    public class
    ActiveExtensionsParameters
    {
        // TODO: Make overloads for methods that use this parameter class that takes a user_id directly.

        /// <summary>
        /// The ID of the user to query.
        /// </summary>
        [QueryParameter("user_id")]
        public virtual string user_id { get; set; }
    }
}
