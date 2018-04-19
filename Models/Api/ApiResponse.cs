// standard namespaces
using System.Collections.Generic;
using System.Net;

// project namespaces
using TwitchNet.Enums.Api;
using TwitchNet.Extensions;
using TwitchNet.Interfaces.Api;

// imported .dll's
using Newtonsoft.Json;

using RestSharp;

namespace
TwitchNet.Models.Api
{
    internal class
    ApiResponse : IApiResponse
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
        /// The response headers from the request.
        /// </summary>
        public Dictionary<string, string>   headers             { get; protected set; }

        /// <summary>
        /// The request limit, remaining requests, and when the rate limit resets.
        /// </summary>
        public RateLimit                    rate_limit          { get; protected set; }

        /// <summary>
        /// The details of the error if one occured.
        /// </summary>
        public ApiException                 exception           { get; protected set; }        

        /// <summary>
        /// Creates a new instance of the <see cref="ApiResponse"/> class.
        /// </summary>
        /// <param name="response">The rest response.</param>
        public ApiResponse(IRestResponse response)
        {
            ApiError error = JsonConvert.DeserializeObject<ApiError>(response.Content);
            if (!response.ErrorException.IsNull())
            {
                exception = new ApiException(response, ApiErrorSource.Internal, "Error encountered by RestSharp while making the request. See the inner exception for more detials.", response.ErrorException);
            }
            else if(error.error.IsValid())
            {
                exception = new ApiException(response, ApiErrorSource.Api, error);
            }
            else
            {
                exception = ApiException.None;
            }

            status_description   = response.StatusDescription;
            status_code          = response.StatusCode;

            headers = new Dictionary<string, string>();
            foreach(Parameter header in response.Headers)
            {
                headers.Add(header.Name, header.Value.ToString());
            }

            rate_limit = new RateLimit(headers);
        }

        /// <summary>
        /// Creates a new blank instance of the <see cref="RestResponse"/> class.
        /// </summary>
        public ApiResponse()
        {

        }
    }

    internal class
    ApiResponse<result_type> : ApiResponse, IApiResponse<result_type>
    {
        /// <summary>
        /// Contains the deserialized result from the Twitch API.
        /// </summary>
        public result_type Result { get; internal set; }

        public ApiResponse(IApiResponse response)
        {
            exception           = response.exception;

            status_description  = response.status_description;
            status_code         = response.status_code;

            headers             = response.headers;

            rate_limit          = response.rate_limit;

            Result              = default(result_type);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ApiResponse{result_type}"/> class.
        /// </summary>
        /// <param name="response">The rest response.</param>
        public ApiResponse(IRestResponse<result_type> response) : base(response)
        {
            Result = response.Data;
        }        

        /// <summary>
        /// Creates a new blank instance of a <see cref="ApiResponse{result_type}"/> class.
        /// </summary>
        public ApiResponse()
        {

        }
    }
}
