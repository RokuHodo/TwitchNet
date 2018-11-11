// standard namespaces
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;

// project namespaces
using TwitchNet.Extensions;

namespace
TwitchNet.Rest.Api
{
    public class
    HelixResponse : IHelixResponse
    {
        /// <summary>
        /// The description of the status code.
        /// </summary>
        public string               status_description  { get; protected set; }

        /// <summary>
        /// The HTTP status code of the response.
        /// </summary>
        public HttpStatusCode       status_code         { get; protected set; }

        /// <summary>
        /// The response headers.
        /// </summary>
        public HttpResponseHeaders  headers             { get; protected set; }

        /// <summary>
        /// The request limit, remaining requcests, and when the rate limit resets.
        /// </summary>
        public RateLimit            rate_limit          { get; protected set; }

        /// <summary>
        /// The error(s) that occurred, if any, in order of occurrence.
        /// </summary>
        public Exception            exception           { get; protected set; }

        public HelixResponse(RestResponse response)
        {
            if (!response.IsNull())
            {
                status_code         = response.status_code;
                status_description  = response.status_description;

                headers             = response.headers;

                exception           = response.exception;

                rate_limit          = new RateLimit(response.headers);
            }

        }

        public HelixResponse(Exception exception)
        {
            this.exception = exception;
        }

        public HelixResponse()
        {

        }

        public void
        SetInputError(ArgumentException exception, HelixRequestSettings settings)
        {
            this.exception = exception;

            if (settings.error_handling_inputs == ErrorHandling.Error)
            {
                throw exception;
            }
        }

        public void
        SetScopesError(MissingScopesException exception, HelixRequestSettings settings)
        {
            this.exception = exception;

            if (settings.error_handling_missing_scopes == ErrorHandling.Error)
            {
                throw exception;
            }
        }
    }

    public class
    HelixResponse<result_type> : HelixResponse, IHelixResponse<result_type>
    {
        /// <summary>
        /// The deserialized result form the Twitch API.
        /// </summary>
        public result_type result { get; protected set; }

        public HelixResponse(RestResponse<result_type> response) : base(response)
        {
            result = response.IsNull() ? default : response.data;
        }

        public HelixResponse(Exception exception) : base(exception)
        {

        }

        public HelixResponse() : base()
        {

        }
    }
}
