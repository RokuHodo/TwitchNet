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

        public RestParameterException() : base()
        {

        }

        public RestParameterException(HttpParameterType type) : base()
        {
            parameter_type = type;
        }

        public RestParameterException(HttpParameterType type, string message) : base(message)
        {
            parameter_type = type;
        }

        public RestParameterException(HttpParameterType type, string message, Exception inner_exception) : base(message, inner_exception)
        {
            parameter_type = type;
        }

        public RestParameterException(string name, HttpParameterType type) : base()
        {
            parameter_name = name;
            parameter_type = type;
        }

        public RestParameterException(string name, HttpParameterType type, string message) : base(message)
        {
            parameter_name = name;
            parameter_type = type;
        }

        public RestParameterException(string name, HttpParameterType type, string message, Exception inner_exception) : base(message, inner_exception)
        {
            parameter_name = name;
            parameter_type = type;
        }
    }

    public class
    RestParameterValueException : RestParameterException
    {
        public object parameter_value { get; protected set; }

        public RestParameterValueException() : base()
        {

        }

        public RestParameterValueException(HttpParameterType type, string message) : base(type, message)
        {

        }

        public RestParameterValueException(HttpParameterType type, object value, string message) : base(type, message)
        {
            parameter_value = value;
        }

        public RestParameterValueException(string name, HttpParameterType type, string message) : base(name, type, message)
        {

        }

        public RestParameterValueException(string name, HttpParameterType type, object value) : base(name, type)
        {
            parameter_value = value;
        }

        public RestParameterValueException(string name, HttpParameterType type, object value, string message) : base(name, type, message)
        {
            parameter_value = value;
        }

        public RestParameterValueException(string name, HttpParameterType type, object value, string message, Exception inner_exception) : base(name, type, message, inner_exception)
        {
            parameter_value = value;
        }
    }

    public class
    RestParameterCountException : RestParameterException
    {
        public int maximum { get; protected set; }

        public int count { get; protected set; }

        public RestParameterCountException(HttpParameterType type, int maximum, int count, string message) : base(type, message)
        {
            this.maximum = maximum;
            this.count = count;
        }

        public RestParameterCountException(string name, HttpParameterType type, int maximum, int count) : base(name, type)
        {
            this.maximum = maximum;
            this.count = count;
        }

        public RestParameterCountException(string name, HttpParameterType type, int maximum, int count, string message) : base(name, type, message)
        {
            this.maximum = maximum;
            this.count = count;
        }
    }

    public class
    QueryParameterException : RestParameterException
    {
        public QueryParameterException() : base(HttpParameterType.Query)
        {

        }

        public QueryParameterException(string message) : base(HttpParameterType.Query, message)
        {

        }

        public QueryParameterException(string message, Exception inner_exception) : base(HttpParameterType.Query, message, inner_exception)
        {

        }

        public QueryParameterException(string name, string message) : base(name, HttpParameterType.Query, message)
        {

        }

        public QueryParameterException(string name, string message, Exception inner_exception) : base(name, HttpParameterType.Query, message, inner_exception)
        {

        }
    }

    public class
    HeaderParameterException : RestParameterException
    {
        public HeaderParameterException() : base(HttpParameterType.Query)
        {

        }

        public HeaderParameterException(string message) : base(HttpParameterType.Header, message)
        {

        }

        public HeaderParameterException(string message, Exception inner_exception) : base(HttpParameterType.Header, message, inner_exception)
        {

        }

        public HeaderParameterException(string name, string message) : base(name, HttpParameterType.Header, message)
        {

        }

        public HeaderParameterException(string name, string message, Exception inner_exception) : base(name, HttpParameterType.Header, message, inner_exception)
        {

        }
    }

    public class
    BodyParameterException : RestParameterException
    {
        public BodyParameterException() : base(HttpParameterType.Body)
        {

        }

        public BodyParameterException(string message) : base(HttpParameterType.Body, message)
        {

        }

        public BodyParameterException(string message, Exception inner_exception) : base(HttpParameterType.Body, message, inner_exception)
        {

        }

        public BodyParameterException(string name, string message) : base(name, HttpParameterType.Body, message)
        {

        }

        public BodyParameterException(string name, string message, Exception inner_exception) : base(name, HttpParameterType.Body, message, inner_exception)
        {

        }
    }

    public class
    QueryParameterValueException : RestParameterValueException
    {
        public QueryParameterValueException() : base()
        {

        }

        public QueryParameterValueException(string name, string message) : base(name, HttpParameterType.Query, message)
        {

        }

        public QueryParameterValueException(string name, object value) : base(name, HttpParameterType.Query, value)
        {

        }

        public QueryParameterValueException(string name, object value, string message) : base(name, HttpParameterType.Query, value, message)
        {

        }

        public QueryParameterValueException(string name, object value, string message, Exception inner_exception) : base(name, HttpParameterType.Query, value, message, inner_exception)
        {

        }
    }

    public class
    BodyParameterValueException : RestParameterValueException
    {
        public BodyParameterValueException() : base()
        {

        }

        public BodyParameterValueException(string name, string message) : base(name, HttpParameterType.Body, message)
        {

        }

        public BodyParameterValueException(string name, object value) : base(name, HttpParameterType.Body, value)
        {

        }

        public BodyParameterValueException(string name, object value, string message) : base(name, HttpParameterType.Body, value, message)
        {

        }

        public BodyParameterValueException(string name, object value, string message, Exception inner_exception) : base(name, HttpParameterType.Body, value, message, inner_exception)
        {

        }
    }

    public class
    QueryParameterCountException : RestParameterCountException
    {
        public QueryParameterCountException(int maximum, int count, string message) : base(HttpParameterType.Query, maximum, count, message)
        {

        }

        public QueryParameterCountException(string name, int maximum, int count) : base(name, HttpParameterType.Query, maximum, count)
        {

        }

        public QueryParameterCountException(string name, int maximum, int count, string message) : base(name, HttpParameterType.Query, maximum, count, message)
        {

        }
    }

    public class
    BodyParameterCountException : RestParameterCountException
    {
        public BodyParameterCountException(int maximum, int count, string message) : base(HttpParameterType.Body, maximum, count, message)
        {

        }

        public BodyParameterCountException(string name, int maximum, int count) : base(name, HttpParameterType.Body, maximum, count)
        {

        }

        public BodyParameterCountException(string name, int maximum, int count, string message) : base(name, HttpParameterType.Body, maximum, count, message)
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