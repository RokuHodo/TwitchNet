// standad namespaces
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
        /// The description of the status code.
        /// </summary>
        string                      status_description   { get; }

        /// <summary>
        /// The HTTP status code of the response.
        /// </summary>
        HttpStatusCode              status_code          { get; }

        /// <summary>
        /// The response headers from the request.
        /// </summary>
        Dictionary<string, string>  headers             { get; }

        /// <summary>
        /// The request limit, remaining requests, and when the rate limit resets.
        /// </summary>
        RateLimit                   rate_limit           { get; }

        /// <summary>
        /// The details of the error if one occured.
        /// </summary>
        ApiException                exception           { get; }
    }

    public interface
    IApiResponse<result_type> : IApiResponse
    {
        /// <summary>
        /// Contains the deserialized result from the Twitch API.
        /// </summary>
        result_type                 Result              { get; }
    }
}
