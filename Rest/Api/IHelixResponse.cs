﻿// project namepspaces
using TwitchNet.Rest.OAuth;

namespace
TwitchNet.Rest.Api
{
    public interface
    IHelixResponse : IOAuthResponse
    {
        /// <summary>
        /// The request limit, remaining requests, and when the rate limit resets.
        /// </summary>
        RateLimit   rate_limit  { get; }
    }

    public interface
    IHelixResponse<result_type> : IHelixResponse
    {
        /// <summary>
        /// Contains the deserialized result from the Twitch API.
        /// </summary>
        result_type result      { get; }
    }
}