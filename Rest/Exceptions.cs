// standard namespaces
using System;
using System.Net;

// project namespaces
using TwitchNet.Extensions;

namespace TwitchNet.Rest
{
    public class
    StatusException : Exception
    {
        /// <summary>
        /// The error associated with the status code, i.e., the status description.
        /// </summary>
        public string status_description { get; protected set; }

        /// <summary>
        /// The HTTP status code of the returned response.
        /// </summary>
        public HttpStatusCode status_code { get; protected set; }

        /// <summary>
        /// The descriptive error message that gives more detailed information on the type of error.
        /// </summary>
        public string error_message { get; protected set; }

        public StatusException()
        {

        }

        public StatusException(string message, Exception inner_exception = default) : base(message, inner_exception)
        {

        }

        public StatusException(string message, HttpStatusCode status_code, string status_description, Exception inner_exception = default) : base(message, inner_exception)
        {
            this.status_code = status_code;
            this.status_description = status_description;
        }

        public StatusException(string message, HttpStatusCode status_code, string status_description, string error_message, Exception inner_exception = default) : base(message, inner_exception)
        {
            this.status_code = status_code;
            this.status_description = status_description;
            this.error_message = error_message;
        }
    }

    public class
    MissingScopesException : Exception
    {
        public Scopes[] missing_scopes { get; private set; }

        public MissingScopesException()
        {

        }

        public MissingScopesException(string message) : base(message)
        {

        }

        public MissingScopesException(Scopes[] scopes)
        {
            missing_scopes = scopes.IsValid() ? scopes : new Scopes[0];
        }

        public MissingScopesException(string message, Scopes[] scopes) : base(message)
        {
            missing_scopes = scopes.IsValid() ? scopes : new Scopes[0];
        }
    }

    public class
    RetryLimitReachedException : Exception
    {
        public int retry_limit { get; protected set; }

        public RetryLimitReachedException()
        {

        }

        public RetryLimitReachedException(string message, Exception inner_exception = default) : base(message, inner_exception)
        {

        }

        public RetryLimitReachedException(int retry_limit)

        {
            this.retry_limit = retry_limit;
        }

        public RetryLimitReachedException(string message, int retry_limit, Exception inner_exception = default) : base(message, inner_exception)
        {
            this.retry_limit = retry_limit;
        }
    }
}
