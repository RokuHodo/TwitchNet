// standad namespaces
using System;
using System.Collections.Generic;
using System.Net;

// project namepspaces
using TwitchNet.Models.Api;

namespace
TwitchNet.Interfaces.Api
{
    public interface
    IApiResponse
    {

        /// <summary>
        /// The status error returned by Twitch when a request is unsuccessful.
        /// This typically coincides with traditional HTTP status codes.
        /// </summary>
        string                      status_error            { get; }

        /// <summary>
        /// The status error message returned by Twitch when a request is unsuccessful.
        /// This can be blank even if a status error is returned by twitch.
        /// </summary>
        string                      status_error_message    { get; }

        /// <summary>
        /// The description of the status code.
        /// </summary>
        string                      status_description      { get; }

        /// <summary>
        /// The HTTP status code of the response.
        /// </summary>
        HttpStatusCode              status_code             { get; }

        /// <summary>
        /// The response headers from the request.
        /// </summary>
        Dictionary<string, string>  headers                 { get; }

        /// <summary>
        /// The request limit, remaining requests, and when the rate limit resets.
        /// </summary>
        RateLimit                   rate_limit              { get; }

        /// <summary>
        /// The exception that was encountered while making the request, if any.
        /// </summary>
        Exception                   exception               { get; }
    }

    public interface
    IApiResponse<result_type> : IApiResponse
    {
        /// <summary>
        /// Contains the deserialized result from the Twitch API.
        /// </summary>
        result_type                 result                  { get; }
    }
}
