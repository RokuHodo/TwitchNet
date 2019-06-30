// standard namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;

// project namespaces
using TwitchNet.Extensions;

// impolrted .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Helix
{
    #region Shared Public Data Structures      

    public class
    DateRange
    {
        /// <summary>
        /// The start date/time period for the returned data.
        /// </summary>
        [JsonProperty("started_at")]
        public DateTime started_at { get; protected set; }

        /// <summary>
        /// The end date/time period for the returned data.
        /// </summary>
        [JsonProperty("ended_at")]
        public DateTime ended_at { get; protected set; }
    }

    public interface
    IPagingParameters
    {
        string after { get; }
    }

    public class
    PagingParameters : IPagingParameters
    {
        /// <summary>
        /// <para>Maximum number of objects to return.</para>
        /// <para>
        /// Min:        1,
        /// Max:        100,
        /// Default:    20.
        /// The value is clamped between the minimum and the maximum values.
        /// </para>
        /// </summary>
        [QueryParameter("first")]
        public int? first { get; set; }

        /// <summary>
        /// The cursor that tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [QueryParameter("after")]
        public string after { get; set; }
    }

    public class
    Pagination
    {
        /// <summary>
        /// A string used to tell the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [JsonProperty("cursor")]
        public string cursor { get; internal set; }
    }

    public interface
    IData<data_type>
    {
        /// <summary>
        /// Contains the response data.
        /// </summary>
        [JsonProperty("data")]
        IList<data_type> data { get; }
    }

    public class
    Data<data_type> : IData<data_type>
    {
        /// <summary>
        /// Contains the response data.
        /// </summary>
        [JsonProperty("data")]
        public IList<data_type> data { get; internal set; }
    }

    public interface
    IDataPage<data_type> : IData<data_type>
    {
        /// <summary>
        /// A string used to tell the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [JsonProperty("pagination")]
        Pagination pagination { get; }
    }

    public class
    DataPage<data_type> : Data<data_type>, IDataPage<data_type>
    {
        /// <summary>
        /// Contains information used when makling multi-page requests.
        /// </summary>
        [JsonProperty("pagination")]
        public Pagination pagination { get; internal set; }
    }

    public interface
    IHelixResponse
    {
        /// <summary>
        /// The description of the status code.
        /// </summary>
        string status_description { get; }

        /// <summary>
        /// The HTTP status code of the response.
        /// </summary>
        HttpStatusCode status_code { get; }

        /// <summary>
        /// The response headers.
        /// </summary>
        HttpResponseHeaders headers { get; }

        /// <summary>
        /// The request limit, remaining requcests, and when the rate limit resets.
        /// </summary>
        RateLimit rate_limit { get; }

        /// <summary>
        /// The error(s) that occurred, if any, in order of occurrence.
        /// </summary>
        Exception exception { get; }
    }

    public class
    HelixResponse : IHelixResponse
    {
        /// <summary>
        /// The description of the status code.
        /// </summary>
        public string status_description { get; protected set; }

        /// <summary>
        /// The HTTP status code of the response.
        /// </summary>
        public HttpStatusCode status_code { get; protected set; }

        /// <summary>
        /// The response headers.
        /// </summary>
        public HttpResponseHeaders headers { get; protected set; }

        /// <summary>
        /// The request limit, remaining requcests, and when the rate limit resets.
        /// </summary>
        public RateLimit rate_limit { get; protected set; }

        /// <summary>
        /// The error(s) that occurred, if any, in order of occurrence.
        /// </summary>
        public Exception exception { get; protected set; }

        public HelixResponse(IHelixResponse response)
        {
            status_code = response.status_code;
            status_description = response.status_description;

            headers = response.headers;

            exception = response.exception;

            rate_limit = response.rate_limit;
        }

        public HelixResponse(RestResponse response)
        {
            if (!response.IsNull())
            {
                status_code = response.status_code;
                status_description = response.status_description;

                headers = response.headers;

                exception = response.exception;

                rate_limit = new RateLimit(response.headers);
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
        SetInputError(Exception exception, HelixRequestSettings settings)
        {
            this.exception = exception;

            if (settings.error_handling_inputs == ErrorHandling.Error)
            {
                throw exception;
            }
        }

        public void
        SetScopesError(AvailableScopesException exception, HelixRequestSettings settings)
        {
            this.exception = exception;

            if (settings.error_handling_missing_scopes == ErrorHandling.Error)
            {
                throw exception;
            }
        }
    }

    public interface
    IHelixResponse<result_type> : IHelixResponse
    {
        /// <summary>
        /// The deserialized result form the Twitch API.
        /// </summary>
        result_type result { get; }
    }

    public class
    HelixResponse<result_type> : HelixResponse, IHelixResponse<result_type>
    {
        /// <summary>
        /// The deserialized result form the Twitch API.
        /// </summary>
        public result_type result { get; protected set; }

        public HelixResponse(IHelixResponse response, result_type result) : base(response)
        {
            this.result = result;
        }

        public HelixResponse(RestResponse<result_type> response) : base(response)
        {
            result = response.IsNull() ? default : response.data;
        }

        public HelixResponse(Exception exception) : base(exception)
        {

        }

        public HelixResponse()
        {

        }
    }

    public class
    RateLimit
    {
        /// <summary>
        /// The number of requests you can use for the rate-limit window (60 seconds).
        /// </summary>
        public ushort limit { get; protected set; }

        /// <summary>
        /// The number of requests left to use for the rate-limit window.
        /// </summary>
        public ushort remaining { get; protected set; }

        /// <summary>A 
        /// When the rate-limit window will reset.
        /// </summary>
        public DateTime reset_time { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="RateLimit"/> class.
        /// </summary>
        /// <param name="headers">The headers from the <see cref="IRestResponse"/>.</param>
        public RateLimit(HttpResponseHeaders headers)
        {
            limit = 0;
            remaining = 0;
            reset_time = DateTime.MinValue;

            if (headers.IsNull())
            {
                return;
            }

            if (headers.TryGetValues("Ratelimit-Limit", out IEnumerable<string> _limit))
            {
                limit = Convert.ToUInt16(_limit.ElementAt(0));
            }

            if (headers.TryGetValues("Ratelimit-Remaining", out IEnumerable<string> _remaining))
            {
                remaining = Convert.ToUInt16(_remaining.ElementAt(0));
            }

            if (headers.TryGetValues("Ratelimit-Reset", out IEnumerable<string> _reset_time))
            {
                long reset_double = Convert.ToInt64(_reset_time.ElementAt(0));
                reset_time = reset_double.FromUnixEpochSeconds();
            }
        }

        public RateLimit()
        {
            limit = 0;
            remaining = 0;

            reset_time = DateTime.MinValue;
        }
    }

    #endregion

    #region Shared Internal Data Structures

    internal class
    HelixInfo
    {
        public string client_id;

        public string bearer_token;

        public HelixRequestSettings settings;

        public Scopes required_scopes;

        public HelixInfo(HelixRequestSettings settings)
        {
            this.settings = settings ?? new HelixRequestSettings();
        }
    }

    #endregion

    #region Settings

    public class
    HelixRequestSettings : RequestSettings
    {
        /// <summary>
        /// <para>How to handle errors experienced while executing the request.</para>
        /// <para>Default: <see cref="ErrorHandling.Error"/>.</para>
        /// </summary>
        public ErrorHandling error_handling_inputs { get; set; }

        /// <summary>
        /// <para>
        /// How to handle OAuth tokens with missing scopes needed to authenticate the request.
        /// This setting is only valid when available scopes are specified.
        /// </para>
        /// <para>Default: <see cref="ErrorHandling.Error"/>.</para>
        /// </summary>
        public ErrorHandling error_handling_missing_scopes { get; set; }

        /// <summary>
        /// <para>
        /// The scopes that were requested when creating an OAuth token.
        /// The scopes contained within an OAuth token can be obtained by using <see cref="OAuth2.ValidateToken(string, HelixRequestSettings)"/>.
        /// </para>
        /// <para>
        /// If specified when making an authenticated request, the available scopes will be used to check if the required scope(s) to make the request are present.
        /// If not specified when making an authenticated request, the reuqest will be made normally without perfomring preemptive checks.
        /// If specified when making a request that does not require authentication, the available scopes are ignored.
        /// </para>
        /// </summary>
        public Scopes available_scopes { get; set; }

        public HelixRequestSettings()
        {
            // Copy paste from Reset() to avoid duplicate instanciation.
            error_handling_inputs = ErrorHandling.Error;
            error_handling_missing_scopes = ErrorHandling.Error;

            available_scopes = Scopes.Other;
        }

        /// <summary>
        /// Sets the request settings back to their default values.
        /// </summary>
        public override void
        Reset()
        {
            error_handling_inputs = ErrorHandling.Error;
            error_handling_missing_scopes = ErrorHandling.Error;

            available_scopes = Scopes.Other;

            base.Reset();
        }
    }

    #endregion

    #region Errors

    public class
    HelixException : Exception
    {
        /// <summary>
        /// The HTTP status code of the returned response.
        /// </summary>
        public int status_code { get; protected set; }

        /// <summary>
        /// The error associated with the status code, i.e., the status description.
        /// </summary>
        public string helix_error { get; protected set; }

        /// <summary>
        /// The descriptive error message that gives more detailed information on the type of error.
        /// </summary>
        public string helix_message { get; protected set; }

        public HelixException(string message, string content, Exception inner_exception) : base(message, inner_exception)
        {
            HelixError _error = JsonConvert.DeserializeObject<HelixError>(content);

            status_code = _error.status;
            helix_error = _error.error;
            helix_message = _error.message;
        }
    }

    public class
    HelixError
    {
        /// <summary>
        /// The error associated with the status code, i.e., the status description.
        /// </summary>
        [JsonProperty("error")]
        public string error { get; protected set; }

        /// <summary>
        /// The HTTP status code of the returned response.
        /// </summary>
        [JsonProperty("status")]
        public int status { get; protected set; }

        /// <summary>
        /// The descriptive error message that gives more detailed information on the type of error.
        /// </summary>
        [JsonProperty("message")]
        public string message { get; protected set; }
    }

    #endregion
}