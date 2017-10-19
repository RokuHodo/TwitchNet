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
    TwitchResponse : ITwitchResponse
    {
        #region Public properties

        /// <summary>
        /// The error message returned with the response by Twitch.
        /// This is only valid when an error is encountered.
        /// </summary>
        public string           error_message       { get; internal set; }

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
        /// Creates a new instance of <see cref="TwitchResponse"/> with all properties to be filled out manually.
        /// </summary>
        public TwitchResponse()
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="TwitchResponse"/> and inherits all properties from the <see cref="IRestResponse{type}"/>.
        /// </summary>
        /// <param name="response"></param>
        public TwitchResponse(IRestResponse response)
        {
            status_code         = response.StatusCode;
            status_description  = response.StatusDescription;

            error_message       = JsonConvert.DeserializeObject<TwitchError>(response.Content).message;

            rate_limit          = new RateLimit(response);
        }

        #endregion       
    }

    internal class
    TwitchResponse<type> : TwitchResponse, ITwitchResponse<type>
    {
        #region Public properties

        /// <summary>
        /// Contains the deserialized result from the Twitch API.
        /// </summary>
        public type             result              { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="TwitchResponse{type}"/> with all properties to be filled out manually.
        /// </summary>
        public TwitchResponse()
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="TwitchResponse{type}"/> and inherits all properties from the <see cref="IRestResponse{type}"/>.
        /// </summary>
        /// <param name="response"></param>
        public TwitchResponse(IRestResponse<type> response) : base(response)
        {
            result = response.Data;
        }

        public TwitchResponse(ITwitchResponse response)
        {
            status_code = response.status_code;
            status_description = response.status_description;

            error_message = response.error_message;

            rate_limit = response.rate_limit;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clones the base properties from a <see cref="TwitchResponse"/> instance to a <see cref="TwitchResponse{type}"/> instance.
        /// </summary>
        /// <param name="response"></param>
        public void
        CloneBaseProperties(ITwitchResponse response)
        {
            status_code = response.status_code;
            status_description = response.status_description;

            error_message = response.error_message;

            rate_limit = response.rate_limit;
        }

        #endregion
    }
}
