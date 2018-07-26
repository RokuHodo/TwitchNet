// standard namespaces
using System;
using System.Collections.Generic;
using System.Net;

// project namespaces
using TwitchNet.Extensions;

// imported .dll's
using RestSharp;

namespace
TwitchNet.Rest.Api
{
    public class
    HelixResponse : IHelixResponse
    {
        /// <summary>
        /// The description of the status code.
        /// </summary>
        public string                       status_description  { get; protected set; }

        /// <summary>
        /// The HTTP status code of the response.
        /// </summary>
        public HttpStatusCode               status_code         { get; protected set; }

        /// <summary>
        /// The response headers.
        /// </summary>
        public Dictionary<string, string>   headers             { get; protected set; }

        /// <summary>
        /// The request limit, remaining requests, and when the rate limit resets.
        /// </summary>
        public RateLimit                    rate_limit          { get; protected set; }

        /// <summary>
        /// The source of the error encountered while making the request.
        /// If more than one error was encountered, this represents the last error encountered.
        /// </summary>
        public RestErrorSource              exception_source    { get; protected set; }

        /// <summary>
        /// The error(s) that occurred, if any, in order of occurrence.
        /// </summary>
        public IEnumerable<Exception>       exceptions          { get; protected set; }

        public HelixResponse(IHelixResponse response)
        {
            status_description  = response.status_description;
            status_code         = response.status_code;

            headers             = response.headers;

            rate_limit          = response.rate_limit;

            exception_source    = response.exception_source;
            exceptions          = response.exceptions;
        }

        public HelixResponse(IRestResponse response, RateLimit rate_limit, RestErrorSource exception_source, IEnumerable<Exception> exceptions)
        {
            if (!response.IsNull())
            {
                status_description  = response.StatusDescription;
                status_code         = response.StatusCode;

                headers             = new Dictionary<string, string>();
                foreach (Parameter header in response.Headers)
                {
                    headers.Add(header.Name, header.Value.ToString());
                }
            }

            this.rate_limit         = rate_limit;

            this.exception_source   = exception_source;
            this.exceptions         = exceptions;
        }

        public HelixResponse(RestErrorSource exception_source, IEnumerable<Exception> exceptions)
        {
            status_description      = string.Empty;
            status_code             = 0;
            headers                 = null;

            rate_limit              = RateLimit.None;

            this.exception_source   = exception_source;
            this.exceptions         = exceptions;
        }
    }

    public class
    HelixResponse<result_type> : HelixResponse, IHelixResponse<result_type>
    {
        /// <summary>
        /// The deserialized result form the Twitch API.
        /// </summary>
        public result_type result { get; protected set; }

        public HelixResponse(IHelixResponse response, result_type value) : base(response)
        {
            result = value;
        }

        public HelixResponse(IRestResponse<result_type> response, RateLimit rate_limit, RestErrorSource exception_source, IEnumerable<Exception> exceptions) : base(response, rate_limit, exception_source, exceptions)
        {
            result = response.IsNull() ? default : response.Data;
        }

        public HelixResponse(RestErrorSource exception_source, IEnumerable<Exception> exceptions) : base(exception_source, exceptions)
        {
            result = default;
        }
    }
}
