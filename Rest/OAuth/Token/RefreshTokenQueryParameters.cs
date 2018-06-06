// standard namespaces


namespace TwitchNet.Rest.OAuth.Token
{
    public class
    RefreshTokenQueryParameters
    {
        /// <summary>
        /// Your app's client secret.
        /// </summary>
        [QueryParameter("client_secret")]
        public string client_secret { get; set; }

        /// <summary>
        /// The entire set or sub-set of scopes assigned to the original token grant.
        /// </summary>
        [QueryParameter("scope", QueryParameterType.ListSpaceSeparated)]
        public Scopes scopes        { get; set; }

        /// <summary>
        /// Creates a new blank instance of the <see cref="RefreshTokenQueryParameters"/> class.
        /// </summary>
        public RefreshTokenQueryParameters()
        {

        }
    }
}
