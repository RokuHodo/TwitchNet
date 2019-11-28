namespace
TwitchNet.Rest.OAuth.Token
{
    public class
    RevokeTokenParameters
    {
        [QueryParameter("token")]
        public string oauth_token { get; set; }
    }

    public class
    AppAccessTokenParameters
    {
        [QueryParameter("grant_type")]
        public GrantType grant_type { get; set; }

        [QueryParameter("scope", typeof(SpaceDelineatedQueryConverter))]
        public HelixScopes? scope { get; set; }
    }
}