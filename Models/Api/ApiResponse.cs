﻿// standard namespaces
using System.Net;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Interfaces.Api;

// imported .dll's
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
        public string           status_error       { get; internal set; }

        /// <summary>
        /// The description of the status code returned.
        /// </summary>
        public string           status_description  { get; internal set; }

        /// <summary>
        /// The HTTP status code of the returned response.
        /// </summary>
        public HttpStatusCode   status_code         { get; internal set; }

        /// <summary>
        /// Contains the request limit, requests remaining, and when the rate limit resets.
        /// </summary>
        public RateLimit        rate_limit          { get; internal set; }

        public ApiResponse()
        {

        }

        public ApiResponse(IRestResponse response, RateLimit rate_limit, ApiError api_error)
        {
            status_code         = response.StatusCode;
            status_description  = response.StatusDescription;
            status_error        = api_error.message.IsValid() ? api_error.message : string.Empty;

            this.rate_limit     = rate_limit;
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

        public ApiResponse(IRestResponse<ApiData<type>> response, RateLimit rate_limit, ApiError api_error) : base(response, rate_limit, api_error)
        {
            result = response.Data;
        }
    }
}
