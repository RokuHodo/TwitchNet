// standard namespaces
using System.Net;

// project namespaces
using TwitchNet.Interfaces.Api;

// imported .dll's
using Newtonsoft.Json;

using RestSharp;

namespace TwitchNet.Models.Api
{
    internal class
    ApiResponseValue<type> : ApiResponse, IApiResponseValue<type>
    {
        #region Public properties

        /// <summary>
        /// Contains the deserialized result from the Twitch API.
        /// </summary>
        public type result { get; internal set; }

        #endregion

        #region Constructors


        public ApiResponseValue(IApiResponse response)
        {
            status_description  = response.status_description;
            status_error        = response.status_error;
            status_code         = response.status_code;

            rate_limit          = response.rate_limit;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ApiResponse{type}"/> with all properties to be filled out manually.
        /// </summary>
        public ApiResponseValue(IApiResponse response, type value) : this(response)
        {
            result = value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clones the base properties from a <see cref="ApiResponse"/> instance to a <see cref="ApiResponse{type}"/> instance.
        /// </summary>
        /// <param name="response"></param>
        public void
        CloneBaseProperties(IApiResponse response)
        {
            status_code = response.status_code;
            status_description = response.status_description;

            status_error = response.status_error;

            rate_limit = response.rate_limit;
        }

        #endregion
    }
}
