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
        
        /// <summary>
        /// Creates an instance of the <see cref="RequestInfo"/> struct.
        /// </summary>
        /// <param name="endpoint">The API endpoint.</param>
        /// <param name="method">The HTTP request method.</param>
        public RequestInfo(string endpoint, Method method)
        {
            this.endpoint   = endpoint;
            this.method     = method;

            bearer_token    = string.Empty;
            oauth_token     = string.Empty;
            client_id       = string.Empty;
        }
    }
}
