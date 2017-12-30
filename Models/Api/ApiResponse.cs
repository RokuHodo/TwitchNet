﻿// standard namespaces
using System;
using System.Collections.Generic;
using System.Net;

// project namespaces
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
        /// The status error returned by Twitch when a request is unsuccessful.
        /// This typically coincides with traditional HTTP status codes.
        /// </summary>
        public string                       status_error        { get; internal set; }

        /// <summary>
        /// The status error message returned by Twitch when a request is unsuccessful.
        /// This can be blank even if a status error is returned by twitch.
        /// </summary>
        public string                       status_error_message { get; internal set; }

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

        /// <summary>
        /// The exception that was encountered or thrown while making the request, if any.
        /// </summary>
        public Exception                    exception           { get; internal set; }

        public ApiResponse()
        {

        }

        public ApiResponse(IRestResponse rest_response)
        {           
            ApiError api_error      = JsonConvert.DeserializeObject<ApiError>(rest_response.Content);
            status_error            = api_error.error ?? string.Empty;
            status_error_message    = api_error.message ?? string.Empty;

            status_description = rest_response.StatusDescription;
            status_code = rest_response.StatusCode;

            headers = new Dictionary<string, string>();
            foreach(Parameter header in rest_response.Headers)
            {
                headers.Add(header.Name, header.Value.ToString());
            }

            rate_limit = new RateLimit(headers);
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
    ApiResponse<result_type> : ApiResponse, IApiResponse<result_type>
    {
        /// <summary>
        /// Contains the deserialized result from the Twitch API.
        /// </summary>
        public result_type result { get; internal set; }

        public ApiResponse()
        {

        }

        public ApiResponse(IApiResponse api_response) : base(api_response)
        {

        }

        public ApiResponse(IApiResponse<result_type> api_response) : base(api_response)
        {
            result = api_response.result;
        }

        public ApiResponse(ApiResponseBundle<result_type> response_bundle) : base(response_bundle.api_response)
        {
            result = response_bundle.rest_response.Data;
        }
    }
}
