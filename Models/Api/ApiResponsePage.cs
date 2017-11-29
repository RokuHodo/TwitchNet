// project namespaces
using TwitchNet.Interfaces.Api;

// imported .dll's
using RestSharp;

namespace
TwitchNet.Models.Api
{
    internal class
    ApiResponsePage<data_type> : ApiResponse, IApiResponsePage<data_type>
    where data_type : class, new()
    {
        /// <summary>
        /// Contains the deserialized result from the Twitch API.
        /// </summary>
        public ApiDataPage<data_type> result { get; internal set; }

        public ApiResponsePage()
        {

        }

        public ApiResponsePage((IRestResponse<ApiDataPage<data_type>> rest_response, IApiResponse api_response) rest_result) : base(rest_result.api_response)
        {
            result = rest_result.rest_response.Data;
        }
    }
}
