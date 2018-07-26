// standard namespaces
using System;
using System.Collections.Generic;
using System.Net;

namespace
TwitchNet.Rest.Api
{
    public interface
    IHelixResponse
    {
        /// <summary>
        /// The description of the status code.
        /// </summary>
        string                      status_description { get; }

        /// <summary>
        /// The HTTP status code of the response.
        /// </summary>
        HttpStatusCode              status_code { get; }

        /// <summary>
        /// The response headers.
        /// </summary>
        Dictionary<string, string>  headers { get; }

        /// <summary>
        /// The request limit, remaining requests, and when the rate limit resets.
        /// </summary>
        RateLimit                   rate_limit  { get; }

        /// <summary>
        /// The source of the error encountered while making the request.
        /// If more than one error was encountered, this represents the last error encountered.
        /// </summary>
        RestErrorSource             exception_source { get; }

        /// <summary>
        /// The error(s) that occurred, if any.
        /// </summary>
        IEnumerable<Exception>      exceptions { get; }
    }

    public interface
    IHelixResponse<result_type> : IHelixResponse
    {
        /// <summary>
        /// The deserialized result form the Twitch API.
        /// </summary>
        result_type result      { get; }
    }
}
