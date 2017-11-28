﻿// standard namespaces
using System.Collections.Generic;
using System.Net;

// project namespaces
using TwitchNet.Extensions;
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
        /// The error message returned with the response by Twitch.
        /// This is only valid when an error is encountered.
        /// </summary>
        public string                       status_error        { get; internal set; }

        /// <summary>
        /// The description of the status code returned.
        /// </summary>
        public string                       status_description  { get; internal set; }

        /// <summary>
        /// The HTTP status code of the returned response.
        /// </summary>
        public HttpStatusCode               status_code         { get; internal set; }

        /// <summary>
        /// The response headers from the requet.
        /// </summary>
        public Dictionary<string, string>   headers             { get; internal set; }

        /// <summary>
        /// Contains the request limit, requests remaining, and when the rate limit resets.
        /// </summary>
        public RateLimit                    rate_limit          { get; internal set; }

        public ApiResponse()
        {

        }

        public ApiResponse(IRestResponse rest_response)
        {
            status_code         = rest_response.StatusCode;
            status_description  = rest_response.StatusDescription;
            status_error        = JsonConvert.DeserializeObject<ApiError>(rest_response.Content).message ?? string.Empty;

            headers = new Dictionary<string, string>();
            foreach(Parameter header in rest_response.Headers)
            {
                headers.Add(header.Name, header.Value.ToString());
            }

            rate_limit          = new RateLimit(headers);
        }

        public ApiResponse(IApiResponse api_response)
        {
            status_code         = api_response.status_code;
            status_description  = api_response.status_description;
            status_error        = api_response.status_error;

            headers             = api_response.headers;

            rate_limit          = api_response.rate_limit;
        }
    }

    internal class
    ApiResponse<type> : ApiResponse, IApiResponse<type>
    where type : class, new()
    {
        /// <summary>
        /// Contains the deserialized result from the Twitch API.
        /// </summary>
        public ApiData<type> result { get; internal set; }

        public ApiResponse()
        {

        }

        public ApiResponse((IRestResponse<ApiData<type>> rest_response, IApiResponse api_response) rest_result) : base(rest_result.api_response)
        {
            result = rest_result.rest_response.Data;
        }
    }
}
