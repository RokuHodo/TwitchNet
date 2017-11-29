// standard namespaces
using System.Collections.Generic;

// imported .dll's
using RestSharp;

namespace
TwitchNet.Models.Api
{
    public class
    Headers
    {
        /// <summary>
        /// The response headers from the HTTP requets.
        /// </summary>
        public Dictionary<string, string> headers { get; protected set; }

        public Headers(IRestResponse rest_response)
        {
            foreach (Parameter header in rest_response.Headers)
            {
                headers.Add(header.Name, header.Value.ToString());
            }
        }
    }
}
