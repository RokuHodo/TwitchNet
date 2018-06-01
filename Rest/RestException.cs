// standard namespaces
using System;
using System.Net;
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Rest.Api;
using TwitchNet.Utilities;

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

        /// <summary>
        /// Sets the <see cref="SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="info"/> is null.</exception>
        public override void
        GetObjectData(SerializationInfo info, StreamingContext context)
        {
            ExceptionUtil.ThrowIfNull(info, nameof(info));

            info.AddValue("error_source", error_source, typeof(RestErrorSource));
            info.AddValue("status_code", status_code, typeof(HttpStatusCode));
            info.AddValue("status_error", status_error, typeof(string));
            info.AddValue("error_message", error_message, typeof(string));

            base.GetObjectData(info, context);
        }
    }
}
