// standard namespaces

// project namespaces
using TwitchNet.Interfaces.Api;

// imported .dll's

using RestSharp;

namespace
TwitchNet.Models.Api
{
    internal class
    HelixResponse : OAuthResponse, IHelixResponse
    {
        /// <summary>
        /// The request limit, remaining requests, and when the rate limit resets.
        /// </summary>
        public RateLimit rate_limit { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="HelixResponse"/> class.
        /// </summary>
        /// <param name="response">The rest response.</param>
        public HelixResponse(IRestResponse response, RestException exception, RateLimit rate_limit) : base(response, exception)
        {
            this.rate_limit = rate_limit;
        }
        
        public HelixResponse(IHelixResponse response) : base(response)
        {
            rate_limit = response.rate_limit;
        }
    }

    internal class
    HelixResponse<result_type> : HelixResponse, IHelixResponse<result_type>
    {
        public result_type result { get; internal set; }

        /// <summary>
        /// Creates a new instance of the <see cref="ApiResponse{result_type}"/> class.
        /// </summary>
        /// <param name="response">The rest response.</param>
        public HelixResponse(IRestResponse<result_type> response, RestException exception, RateLimit rate_limit) : base(response, exception, rate_limit)
        {
            result = response.Data;
        }

        public HelixResponse(IHelixResponse response, result_type result) : base(response)
        {
            this.result = result;
        }
    }
}
