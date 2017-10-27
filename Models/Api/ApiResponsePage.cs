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
    ApiResponsePage<type> : ApiResponse, IApiResponsePage<type>
    where type : class, new()
    {
        #region Public properties

        /// <summary>
        /// Contains the deserialized result from the Twitch API.
        /// </summary>
        public ApiDataPage<type> result { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="ApiResponse{type}"/> with all properties to be filled out manually.
        /// </summary>
        public ApiResponsePage()
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="ApiResponse{type}"/> and inherits all properties from the <see cref="IRestResponse{type}"/>.
        /// </summary>
        /// <param name="response"></param>
        public ApiResponsePage(IRestResponse<ApiDataPage<type>> response) : base(response)
        {
            result = response.Data;
        }

        public ApiResponsePage(IApiResponse response)
        {
            status_code = response.status_code;
            status_description = response.status_description;

            status_error = response.status_error;

            rate_limit = response.rate_limit;
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
