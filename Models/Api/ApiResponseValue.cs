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

        public ApiResponseValue()
        {

        }

        public ApiResponseValue(IApiResponse api_response, type value) : base(api_response)
        {
            result = value;
        }
    }
}
