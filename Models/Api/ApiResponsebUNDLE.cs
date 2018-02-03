// imported .dll's
using RestSharp;

namespace
TwitchNet.Models.Api
{
    internal class
    ApiResponseBundle<result_type>
    {
        /// <summary>
        /// The base TwitchApi response.
        /// </summary>
        internal ApiResponse                api_response    { get; set; }

        /// <summary>
        /// The response from the Rest Request.
        /// </summary>
        internal IRestResponse<result_type> rest_response   { get; set; }

        internal
        ApiResponseBundle()
        {

        }

        internal
        ApiResponseBundle(IRestResponse<result_type> _rest_response, ApiResponse _api_response)
        {
            rest_response = _rest_response;

            api_response = _api_response;
        }
    }
}
