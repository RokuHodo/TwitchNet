namespace
TwitchNet.Rest.OAuth.Token
{
    public class
    AppAccessTokenParameters
    {
        [QueryParameter("grant_type")]
        public GrantType grant_type { get; set; }

        [QueryParameter("scope", typeof(SpaceDelineatedQueryConverter))]
        public Scopes? scope { get; set; }
    }
}