// standard namespaces
using System;
using System.Collections.Generic;
using System.Net;

// imported .dll's
using RestSharp;

namespace
TwitchNet.Rest.OAuth
{
    internal class
    OAuthResponse : IOAuthResponse
    {
        /// <summary>
        /// The description of the status code.
        /// </summary>
        public string                       status_description  { get; internal set; }

        /// <summary>
        /// The HTTP status code of the response.
        /// </summary>
        public HttpStatusCode               status_code         { get; internal set; }

        /// <summary>
        /// The response headers from the request.
        /// </summary>
        public Dictionary<string, string>   headers             { get; internal set; }

        /// <summary>
        /// The source of the error encountered while making the request.
        /// If more than one error was encountered, this represents the last error encountered.
        /// </summary>
        public RestErrorSource              exception_source    { get; internal set; }

        /// <summary>
        /// The details of the error if one occured.
        /// </summary>
        public IEnumerable<Exception>       exceptions          { get; internal set; }

        public OAuthResponse(RestInfo info)
        {
            /*
            status_description = info.response.StatusDescription;
            status_code = info.response.StatusCode;

            headers = new Dictionary<string, string>();
            foreach (Parameter header in info.response.Headers)
            {
                headers.Add(header.Name, header.Value.ToString());
            }

            exceptions = info.exceptions;
            exception_source = info.exception_source;
            */
        }

        public OAuthResponse(IOAuthResponse response)
        {
            status_description = response.status_description;
            status_code = response.status_code;

            headers = response.headers;

            exceptions = response.exceptions;
        }
    }

    internal class
    OAuthResponse<result_type> : OAuthResponse, IOAuthResponse<result_type>
    {
        public result_type result { get; internal set; }

        public OAuthResponse(RestInfo<result_type> info) : base(info)
        {
            result = info.response.Data;
        }
    }
}
