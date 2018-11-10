// standard namespaces
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;

namespace
TwitchNet.Rest.Api
{
    public interface
    IHelixResponse
    {
        /// <summary>
        /// The description of the status code.
        /// </summary>
        string              status_description  { get; }

        /// <summary>
        /// The HTTP status code of the response.
        /// </summary>
        HttpStatusCode      status_code         { get; }

        /// <summary>
        /// The response headers.
        /// </summary>
        HttpResponseHeaders headers             { get; }

        /// <summary>
        /// The request limit, remaining requcests, and when the rate limit resets.
        /// </summary>
        RateLimit           rate_limit          { get; }

        /// <summary>
        /// The error(s) that occurred, if any, in order of occurrence.
        /// </summary>
        Exception           exception           { get; }
    }

    public interface
    IHelixResponse<result_type> : IHelixResponse
    {
        /// <summary>
        /// The deserialized result form the Twitch API.
        /// </summary>
        result_type result                      { get; }
    }
}
