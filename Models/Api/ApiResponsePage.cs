// project namespaces
using TwitchNet.Interfaces.Api;

// imported .dll's
using RestSharp;

namespace
TwitchNet.Models.Api
{
    internal class
    ApiResponsePage<type> : ApiResponse, IApiResponsePage<type>
    where type : class, new()
    {
        /// <summary>
        /// Contains the deserialized result from the Twitch API.
        /// </summary>
        public ApiDataPage<type> result { get; internal set; }

        public ApiResponsePage()
        {

        }

        public ApiResponsePage((IRestResponse<ApiDataPage<type>> rest_response, RateLimit rate_limit, ApiError api_error) rest_result) : base(rest_result)
        {
            result = rest_result.rest_response.Data;
        }
    }
}
