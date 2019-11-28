namespace
TwitchNet.Rest.OAuth.Token
{
    public class
    RefreshTokenParameters
    {
        /// <summary>
        /// Your app's client secret.
        /// </summary>
        [QueryParameter("client_secret")]
        public virtual string client_secret { get; set; }

        /// <summary>
        /// The entire set or sub-set of scopes assigned to the original token grant.
        /// </summary>
        [QueryParameter("scope", typeof(SpaceDelineatedQueryConverter))]
        public virtual HelixScopes scopes        { get; set; }
    }
}
