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
    AvailableScopesException : Exception
    {
        public Scopes[] missing_scopes { get; private set; }

        public AvailableScopesException(Scopes[] scopes)
        {
            missing_scopes = scopes.IsValid() ? scopes : new Scopes[0];
        }

        public AvailableScopesException(string message, Scopes[] scopes) : base(message)
        {
            missing_scopes = scopes.IsValid() ? scopes : new Scopes[0];
        }
    }

    public class
    RestParameterException : Exception
    {
        public string parameter_name { get; protected set; }

        public HttpParameterType parameter_type { get; protected set; }

        public object parameter_value { get; protected set; }

        public RestParameterException() : base()
        {

        }

        public RestParameterException(HttpParameterType type) : base()
        {
            parameter_type = type;
        }

        public RestParameterException(string message, HttpParameterType type) : base(message)
        {
            parameter_type = type;
        }

        public RestParameterException(string message, HttpParameterType type, Exception inner_exception) : base(message, inner_exception)
        {
            parameter_type = type;
        }

        public RestParameterException(string message, string name, HttpParameterType type) : base(message)
        {
            parameter_name = name;
            parameter_type = type;
        }

        public RestParameterException(string message, string name, HttpParameterType type, Exception inner_exception) : base(message, inner_exception)
        {
            parameter_name = name;
            parameter_type = type;
        }

        public RestParameterException(string message, string name, object value, HttpParameterType type) : base(message)
        {
            parameter_name = name;
            parameter_type = type;
            parameter_value = value;
        }

        public RestParameterException(string message, string name, object value, HttpParameterType type, Exception inner_exception) : base(message, inner_exception)
        {
            parameter_name = name;
            parameter_type = type;
            parameter_value = value;
        }
    }    

    public class
    QueryParameterException : RestParameterException
    {
        public QueryParameterException() : base(HttpParameterType.Query)
        {

        }

        public QueryParameterException(string message) : base(message, HttpParameterType.Query)
        {

        }

        public QueryParameterException(string message, Exception inner_exception) : base(message, HttpParameterType.Query, inner_exception)
        {

        }

        public QueryParameterException(string message, string name) : base(message, name, HttpParameterType.Query)
        {

        }

        public QueryParameterException(string message, string name, Exception inner_exception) : base(message, name, HttpParameterType.Query, inner_exception)
        {

        }

        public QueryParameterException(string message, string name, object value) : base(message, name, value, HttpParameterType.Query)
        {

        }

        public QueryParameterException(string message, string name, object value, Exception inner_exception) : base(message, name, value, HttpParameterType.Query, inner_exception)
        {

        }
    }

    public class
    BodyParameterException : RestParameterException
    {
        public BodyParameterException() : base(HttpParameterType.Body)
        {

        }

        public BodyParameterException(string message) : base(message, HttpParameterType.Body)
        {

        }

        public BodyParameterException(string message, Exception inner_exception) : base(message, HttpParameterType.Body, inner_exception)
        {

        }

        public BodyParameterException(string message, string name) : base(message, name, HttpParameterType.Body)
        {

        }

        public BodyParameterException(string message, string name, Exception inner_exception) : base(message, name, HttpParameterType.Body, inner_exception)
        {

        }

        public BodyParameterException(string message, string name, object value) : base(message, name, value, HttpParameterType.Body)
        {

        }

        public BodyParameterException(string message, string name, object value, Exception inner_exception) : base(message, name, value, HttpParameterType.Body, inner_exception)
        {

        }
    }

    public class
    HeaderParameterException : RestParameterException
    {
        public HeaderParameterException() : base(HttpParameterType.Header)
        {

        }

        public HeaderParameterException(string message) : base(message, HttpParameterType.Header)
        {

        }

        public HeaderParameterException(string message, Exception inner_exception) : base(message, HttpParameterType.Header, inner_exception)
        {

        }

        public HeaderParameterException(string message, string name) : base(message, name, HttpParameterType.Header)
        {

        }

        public HeaderParameterException(string message, string name, Exception inner_exception) : base(message, name, HttpParameterType.Header, inner_exception)
        {

        }

        public HeaderParameterException(string message, string name, object value) : base(message, name, value, HttpParameterType.Header)
        {

        }

        public HeaderParameterException(string message, string name, object value, Exception inner_exception) : base(message, name, value, HttpParameterType.Header, inner_exception)
        {

        }
    }

    public class
    RestParameterCountException : Exception
    {
        public string parameter_name { get; protected set; }

        public HttpParameterType parameter_type { get; protected set; }

        public int maximum_count { get; protected set; }

        public int count { get; protected set; }

        public RestParameterCountException(string message, int maximum, int count, HttpParameterType type) : base(message)
        {
            maximum_count = maximum;
            this.count = count;

            parameter_type = type;
        }

        public RestParameterCountException(string message, int maximum, int count, HttpParameterType type, Exception inner_exception) : base(message, inner_exception)
        {
            maximum_count = maximum;
            this.count = count;

            parameter_type = type;
        }

        public RestParameterCountException(string message, string name, int maximum, int count, HttpParameterType type) : base(message)
        {
            maximum_count = maximum;
            this.count = count;

            parameter_name = name;
            parameter_type = type;
        }

        public RestParameterCountException(string message, string name, int maximum, int count, HttpParameterType type, Exception inner_exception) : base(message, inner_exception)
        {
            maximum_count = maximum;
            this.count = count;

            parameter_name = name;
            parameter_type = type;
        }
    }

    public class
    QueryParameterCountException : RestParameterCountException
    {
        public QueryParameterCountException(string message, int maximum, int count) : base(message, maximum, count, HttpParameterType.Query)
        {

        }

        public QueryParameterCountException(string message, int maximum, int count, Exception inner_exception) : base(message, maximum, count, HttpParameterType.Query, inner_exception)
        {

        }

        public QueryParameterCountException(string message, string name, int maximum, int count) : base(message, name, maximum, count, HttpParameterType.Query)
        {

        }

        public QueryParameterCountException(string message, string name, int maximum, int count, Exception inner_exception) : base(message, name, maximum, count, HttpParameterType.Query, inner_exception)
        {

        }
    }

    public class
    BodyParameterCountException : RestParameterCountException
    {
        public BodyParameterCountException(string message, int maximum, int count) : base(message, maximum, count, HttpParameterType.Body)
        {

        }

        public BodyParameterCountException(string message, int maximum, int count, Exception inner_exception) : base(message, maximum, count, HttpParameterType.Body, inner_exception)
        {

        }

        public BodyParameterCountException(string message, string name, int maximum, int count) : base(message, name, maximum, count, HttpParameterType.Body)
        {

        }

        public BodyParameterCountException(string message, string name, int maximum, int count, Exception inner_exception) : base(message, name, maximum, count, HttpParameterType.Body, inner_exception)
        {

        }
    }

    public class
    RetryLimitReachedException : Exception
    {
        public int retry_limit { get; protected set; }

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