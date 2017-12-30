// imported .dll's
using RestSharp;

namespace
TwitchNet.Models.Api
{
    internal class
    ApiResponseBundle<result_type>
    {
        internal ApiResponse                api_response    { get; set; }

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
