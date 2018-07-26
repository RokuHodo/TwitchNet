// standad namespaces
using System;
using System.Collections.Generic;
using System.Net;

namespace
TwitchNet.Rest.OAuth
{
    public interface
    IOAuthResponse
    {
        /// <summary>
        /// The description of the status code.
        /// </summary>
        string                      status_description  { get; }

        /// <summary>
        /// The HTTP status code of the response.
        /// </summary>
        HttpStatusCode              status_code         { get; }

        /// <summary>
        /// The response headers from the request.
        /// </summary>
        Dictionary<string, string>  headers             { get; }

        /// <summary>
        /// The source of the error encountered while making the request.
        /// If more than one error was encountered, this represents the last error encountered.
        /// </summary>
        RestErrorSource             exception_source    { get; }

        /// <summary>
        /// The exception(s) that were encountered.
        /// </summary>
        IEnumerable<Exception>      exceptions          { get; }
    }

    public interface
    IOAuthResponse<result_type> : IOAuthResponse
    {
        /// <summary>
        /// The deserialized content.
        /// </summary>
        result_type                 result              { get; }
    }
}
