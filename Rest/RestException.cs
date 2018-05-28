// standard namespaces
using System;
using System.Net;

// project namespaces
using TwitchNet.Rest.Api;

// imported .dll's
using RestSharp;

namespace
TwitchNet.Rest
{
    [Serializable]
    public class
    RestException : Exception
    {
        /// <summary>
        /// The origin of the error.
        /// </summary>
        public RestErrorSource  error_source     { get; protected set; }

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
        public static readonly RestException None = new RestException();

        /// <summary>
        /// Creates an instance of the <see cref="RestException"/> error using the <see cref="IRestResponse"/>.
        /// </summary>
        /// <param name="response">The rest response.</param>
        /// <param name="source">The source of the error.</param>
        /// <param name="message">The error message. This is not the same as the normal exception message.</param>
        /// <param name="inner_exception">The inner exception encountered.</param>
        public RestException(IRestResponse response, RestErrorSource source, string message, Exception inner_exception) : base(message, inner_exception)
        {
            error_source     = source;

            status_code      = response.StatusCode;
            status_error     = response.StatusDescription;
            error_message    = message;
        }

        /// <summary>
        /// Creates an instance of the <see cref="RestException"/> error using the <see cref="IRestResponse"/> and <see cref="RestError"/>.
        /// </summary>
        /// <param name="response">The rest response.</param>
        /// <param name="source">The source of the error.</param>
        /// <param name="error">The error returned by the API.</param>
        public RestException(IRestResponse response, RestErrorSource source, RestError error)
        {               
            error_source     = source;

            status_code      = response.StatusCode;
            status_error     = error.error;
            error_message    = error.message;
        }

        /// <summary>
        /// Creates a blank instance of the <see cref="RestException"/> error.
        /// </summary>
        public RestException()
        {
            error_source     = RestErrorSource.None;

            status_code      = 0;

            status_error     = string.Empty;
            error_message    = string.Empty;
        }
    }
}
