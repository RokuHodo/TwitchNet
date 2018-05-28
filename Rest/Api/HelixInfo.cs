namespace
TwitchNet.Rest.Api
{
    public struct
    HelixInfo
    {
        /// <summary>
        /// <para>The token to authorize the request.</para>
        /// <para>Used only with the Helix API.</para>
        /// </summary>
        public string bearer_token;

        /// <summary>
        /// The Client-ID if both the Bearer token and Client Id are being provided.
        /// </summary>
        public string client_id;      
    }
}
