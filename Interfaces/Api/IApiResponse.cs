// standad namespaces
using System.Net;

// project namepspaces
using TwitchNet.Models.Api;

namespace TwitchNet.Interfaces.Api
{
    public interface
    IApiResponse
    {

        /// <summary>
        /// The error message returned with the response by Twitch.
        /// This is only valid when an error is encountered.
        /// </summary>
        string          status_error       { get; }

        /// <summary>
        /// The description of the status code returned.
        /// </summary>
        string          status_description  { get; }

        /// <summary>
        /// The HTTP status code of the returned response.
        /// </summary>
        HttpStatusCode  status_code         { get; }

        /// <summary>
        /// Contains the request limit, requests remaining, and when the rate limit resets.
        /// </summary>
        RateLimit       rate_limit          { get; }
    }

    public interface
    IApiResponse<type> : IApiResponse
    where type : class, new()
    {
        /// <summary>
        /// Contains the deserialized result from the Twitch API.
        /// </summary>
        ApiData<type> result { get; }
    }
}
