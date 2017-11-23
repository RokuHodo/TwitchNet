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
        #region Properties

        /// <summary>
        /// Contains the deserialized result from the Twitch API.
        /// </summary>
        public ApiDataPage<type> result { get; internal set; }

        #endregion

        #region Constructors

        public ApiResponsePage()
        {

        }

        public ApiResponsePage(IRestResponse<ApiDataPage<type>> response, RateLimit rate_limit, ApiError api_error) : base(response, rate_limit, api_error)
        {
            result = response.Data;
        }

        #endregion
    }
}
