// standard namespaces
using System.Collections.Generic;
using System.Net;

// project namespaces
using TwitchNet.Interfaces.Api;

// imported .dll's
using RestSharp;

namespace
TwitchNet.Models.Api
{
    internal class
    OAuthResponse : IOAuthResponse
    {
        /// <summary>
        /// The description of the status code.
        /// </summary>
        public string                       status_description  { get; protected set; }

        /// <summary>
        /// The HTTP status code of the response.
        /// </summary>
        public HttpStatusCode               status_code         { get; protected set; }

        /// <summary>
        /// The response headers from the request.
        /// </summary>
        public Dictionary<string, string>   headers             { get; protected set; }

        /// <summary>
        /// The details of the error if one occured.
        /// </summary>
        public RestException                exception           { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="HelixResponse"/> class.
        /// </summary>
        /// <param name="response">The rest response.</param>
        public OAuthResponse(IRestResponse response, RestException exception)
        {
            status_description = response.StatusDescription;
            status_code = response.StatusCode;

            headers = new Dictionary<string, string>();
            foreach (Parameter header in response.Headers)
            {
                headers.Add(header.Name, header.Value.ToString());
            }

            this.exception = exception;
        }

        public OAuthResponse(IOAuthResponse response)
        {
            status_description = response.status_description;
            status_code = response.status_code;

            headers = response.headers;

            exception = response.exception;
        }
    }

    internal class
    OAuthResponse<result_type> : OAuthResponse, IOAuthResponse<result_type>
    {
        public result_type result { get; protected set; }

        public OAuthResponse(IRestResponse<result_type> response, RestException exception) : base(response, exception)
        {
            result = response.Data;
        }

        public OAuthResponse(IOAuthResponse response, result_type result) : base(response)
        {
            this.result = result;
        }
    }
}
