// imported .dll's
using RestSharp;

namespace TwitchNet.Models.Api
{
    public struct
    RequestInfo
    {
        /// <summary>
        /// The API endpoint.
        /// </summary>
        public string endpoint;

        /// <summary>
        /// The HTTP rest request method.
        /// </summary>
        public Method method;

        /// <summary>
        /// <para>The token to authorize the request.</para>
        /// <para>Used only with the Helix API.</para>
        /// </summary>
        public string bearer_token;

        /// <summary>
        /// <para>The token to authorize the request.</para>
        /// <para>Used only with the Kraken API.</para>
        /// </summary>
        public string oauth_token;

        /// <summary>
        /// The Client-ID if both the Bearer token and Client Id are being provided.
        /// </summary>
        public string client_id;        
    }
}
