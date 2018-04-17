// standard namespaces
using System;
using System.Collections.Generic;
using System.Net;

// project namespaces
using TwitchNet.Enums.Api;
using TwitchNet.Interfaces.Api;

// imported .dll's
using Newtonsoft.Json;

using RestSharp;

namespace
TwitchNet.Models.Api
{
    internal class
    ApiResponse : IApiResponse
    {
        /// <summary>
        /// The description of the status code.
        /// </summary>
        public string                       status_description  { get; internal set; }

        /// <summary>
        /// The HTTP status code of the response.
        /// </summary>
        public HttpStatusCode               status_code         { get; internal set; }

        /// <summary>
        /// The response headers from the request.
        /// </summary>
        public Dictionary<string, string>   headers             { get; internal set; }

        /// <summary>
        /// The request limit, remaining requests, and when the rate limit resets.
        /// </summary>
        public RateLimit                    rate_limit          { get; internal set; }

        public ResponseError                error               { get; internal set; }        

        public ApiResponse(IRestResponse response)
        {
            error = new ResponseError(response); 

            status_description = response.StatusDescription;
            status_code = response.StatusCode;

            headers = new Dictionary<string, string>();
            foreach(Parameter header in response.Headers)
            {
                headers.Add(header.Name, header.Value.ToString());
            }

            rate_limit = new RateLimit(headers);
        }

        public ApiResponse()
        {

        }
    }

    internal class
    ApiResponse<result_type> : ApiResponse, IApiResponse<result_type>
    {
        /// <summary>
        /// Contains the deserialized result from the Twitch API.
        /// </summary>
        public result_type result { get; internal set; }

        public ApiResponse(IApiResponse response)
        {
            status_description = response.status_description;
            status_code = response.status_code;

            headers = response.headers;

            rate_limit = response.rate_limit;

            error = response.error;

            result = default(result_type);
        }

        public ApiResponse(IRestResponse<result_type> api_response) : base(api_response)
        {
            result = api_response.Data;
        }        

        public ApiResponse()
        {

        }
    }
}
