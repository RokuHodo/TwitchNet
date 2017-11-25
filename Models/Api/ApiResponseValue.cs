// project namespaces
using TwitchNet.Interfaces.Api;

namespace
TwitchNet.Models.Api
{
    internal class
    ApiResponseValue<type> : ApiResponse, IApiResponseValue<type>
    {
        /// <summary>
        /// Contains the deserialized result from the Twitch API.
        /// </summary>
        public type result { get; internal set; }

        public ApiResponseValue(IApiResponse response)
        {
            status_description  = response.status_description;
            status_error        = response.status_error;
            status_code         = response.status_code;

            rate_limit          = response.rate_limit;
        }

        public ApiResponseValue(IApiResponse response, type value) : this(response)
        {
            result = value;
        }
    }
}
