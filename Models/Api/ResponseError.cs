// standard namespaces
using System;
using System.Net;

// project namespaces
using TwitchNet.Enums.Api;
using TwitchNet.Extensions;

// imported .dll's
using Newtonsoft.Json;

using RestSharp;

namespace TwitchNet.Models.Api
{
    public class
    ResponseError
    {
        /// <summary>
        /// The origin of the error.
        /// </summary>
        public ResponseErrorSource  source      { get; internal set; }

        /// <summary>
        /// The HTTP status code.
        /// </summary>
        public HttpStatusCode       status_code { get; internal set; }

        /// <summary>
        /// The error returned by the API.
        /// This typically coincides with traditional HTTP status code descriptions.
        /// </summary>
        public string               error       { get; internal set; }        

        /// <summary>
        /// Detailed information about the error.
        /// </summary>
        public string               message     { get; internal set; }

        /// <summary>
        /// The exception that may have been encountered.
        /// </summary>
        public Exception            exception   { get; internal set; }

        /// <summary>
        /// Returns an empty error.
        /// </summary>
        public static readonly ResponseError Empty = new ResponseError();

        /// <summary>
        /// Creates a new instance of the <see cref="ResponseError"/> class and populates the members if an error is detected.
        /// </summary>
        /// <param name="response">The rest response.</param>
        public ResponseError(IRestResponse response)
        {
            ApiError api_error = JsonConvert.DeserializeObject<ApiError>(response.Content);

            if (!response.ErrorException.IsNull())
            {
                source      = ResponseErrorSource.Internal;

                status_code = response.StatusCode;
                error       = response.StatusDescription;
                message     = "An error was encountered while executing the request from within RestSharp. For more detials, see the attached exception.";

                exception   = response.ErrorException;
            }
            else if (api_error.error.IsValid())
            {
                source      = ResponseErrorSource.Api;

                status_code = response.StatusCode;
                error       = api_error.error;
                message     = api_error.message;

                exception   = new Exception((int)status_code + ": " + error);
            }
            else
            {
                EmptyError();
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ResponseError"/> class.
        /// </summary>
        public ResponseError()
        {
            EmptyError();
        }

        /// <summary>
        /// Sets the members to the equivalent of an empty error.
        /// </summary>
        private void
        EmptyError()
        {
            source      = ResponseErrorSource.None;

            status_code = 0;
            error       = string.Empty;
            message     = string.Empty;

            exception   = null;
        }
    }
}
