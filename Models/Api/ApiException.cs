// standard namespaces
using System;
using System.Net;

// project namespaces
using TwitchNet.Enums.Api;

// imported .dll's
using RestSharp;

namespace TwitchNet.Models.Api
{
    public class
    ApiException : Exception
    {
        /// <summary>
        /// The origin of the error.
        /// </summary>
        public ApiErrorSource   error_source     { get; protected set; }

        /// <summary>
        /// The HTTP status code.
        /// </summary>
        public HttpStatusCode   status_code      { get; protected set; }

        /// <summary>
        /// The error returned by the API.
        /// This typically coincides with traditional HTTP status code descriptions.
        /// </summary>
        public string           status_error     { get; protected set; }        

        /// <summary>
        /// Detailed information about the error.
        /// </summary>
        public string           error_message    { get; protected set; }

        /// <summary>
        /// Returns an empty api error.
        /// </summary>
        public static readonly ApiException None = new ApiException();

        /// <summary>
        /// Creates an instance of the <see cref="ApiException"/> error using the <see cref="IRestResponse"/>.
        /// </summary>
        /// <param name="response">The rest response.</param>
        /// <param name="source">The source of the error.</param>
        /// <param name="message">The error message. This is not the same as the normal exception message.</param>
        /// <param name="inner_exception">The inner exception encountered.</param>
        public ApiException(IRestResponse response, ApiErrorSource source, string message, Exception inner_exception) : base(message, inner_exception)
        {
            error_source     = source;

            status_code      = response.StatusCode;
            status_error     = response.StatusDescription;
            error_message    = message;
        }

        /// <summary>
        /// Creates an instance of the <see cref="ApiException"/> error using the <see cref="IRestResponse"/> and <see cref="ApiError"/>.
        /// </summary>
        /// <param name="response">The rest response.</param>
        /// <param name="source">The source of the error.</param>
        /// <param name="error">The error returned by the API.</param>
        public ApiException(IRestResponse response, ApiErrorSource source, ApiError error)
        {               
            error_source     = source;

            status_code      = response.StatusCode;
            status_error     = error.error;
            error_message    = error.message;
        }

        /// <summary>
        /// Creates a blank instance of the <see cref="ApiException"/> error.
        /// </summary>
        public ApiException()
        {
            error_source     = ApiErrorSource.None;

            status_code      = 0;
            status_error     = string.Empty;
            error_message    = string.Empty;
        }
    }
}
