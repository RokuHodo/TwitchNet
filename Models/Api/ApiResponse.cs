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
    ApiResponse : IApiResponse
    {
        #region Public properties

        /// <summary>
        /// The error message returned with the response by Twitch.
        /// This is only valid when an error is encountered.
        /// </summary>
        public string           status_error       { get; internal set; }

        /// <summary>
        /// The description of the status code returned.
        /// </summary>
        public string           status_description  { get; internal set; }

        /// <summary>
        /// The HTTP status code of the returned response.
        /// </summary>
        public HttpStatusCode   status_code         { get; internal set; }

        /// <summary>
        /// Contains the request limit, requests remaining, and when the rate limit resets.
        /// </summary>
        public RateLimit        rate_limit          { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="ApiResponse"/> with all properties to be filled out manually.
        /// </summary>
        public ApiResponse()
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="ApiResponse"/> and inherits all properties from the <see cref="IRestResponse{type}"/>.
        /// </summary>
        /// <param name="response"></param>
        public ApiResponse(IRestResponse response)
        {
            status_code         = response.StatusCode;
            status_description  = response.StatusDescription;

            status_error        = JsonConvert.DeserializeObject<ApiError>(response.Content).message ?? string.Empty;

            rate_limit          = new RateLimit(response);
        }

        #endregion       
    }

    internal class
    ApiResponse<type> : ApiResponse, IApiResponse<type>
    where type : class, new()
    {
        #region Public properties

        /// <summary>
        /// Contains the deserialized result from the Twitch API.
        /// </summary>
        public ApiData<type> result { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="ApiResponse{type}"/> with all properties to be filled out manually.
        /// </summary>
        public ApiResponse()
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="ApiResponse{type}"/> and inherits all properties from the <see cref="IRestResponse{type}"/>.
        /// </summary>
        /// <param name="response"></param>
        public ApiResponse(IRestResponse<ApiData<type>> response) : base(response)
        {
            result = response.Data;
        }

        public ApiResponse(IApiResponse response)
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
