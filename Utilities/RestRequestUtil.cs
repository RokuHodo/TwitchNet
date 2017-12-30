// standard namespaces
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Debug;
using TwitchNet.Enums;
using TwitchNet.Enums.Debug;
using TwitchNet.Enums.Api;
using TwitchNet.Extensions;
using TwitchNet.Helpers.Json;
using TwitchNet.Interfaces.Api;
using TwitchNet.Models.Api;

// imported .dll's
using RestSharp;

namespace 
TwitchNet.Utilities
{
    // TODO: (RestRequestUtil) Make the base URL customizable to accomodate for any future API revisions that may use different URL's.

    internal static class
    RestRequestUtil
    {
        #region Execute methods

        /// <summary>
        /// Asynchronously executes a rest request.
        /// </summary>
        /// <typeparam name="result_type">The type of the <see cref="IRestResponse{T}.Data"/> object.</typeparam>
        /// <param name="endpoint">The endpoint URL.</param>
        /// <param name="method">The HTTP request method.</param>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A list of individual query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns an instance of the <see cref="ApiResponse{result_type}"/> class.</returns>
        public static async Task<ApiResponse<result_type>>
        ExecuteRequestAsync<result_type>(string endpoint, Method method, string bearer_token, string client_id, IList<QueryParameter> query_parameters, ApiRequestSettings api_request_settings)
        {
            if (api_request_settings.IsNull())
            {
                api_request_settings = new ApiRequestSettings();
            }

            RestRequest request = Request(endpoint, method, bearer_token, client_id, api_request_settings);
            request = PagingUtil.AddPaging(request, query_parameters);

            ApiResponseBundle<result_type> response_bundle = await ExecuteRequestAsync<result_type>(request, api_request_settings);            
            ApiResponse<result_type> api_response = new ApiResponse<result_type>(response_bundle);

            return api_response;
        }

        /// <summary>
        /// Asynchronously executes a rest request.
        /// </summary>
        /// <typeparam name="result_type">The type of the <see cref="IRestResponse{T}.Data"/> object.</typeparam>
        /// <typeparam name="query_parameters_type">
        /// The type of the <paramref name="query_parameters"/> object.
        /// Restricted to a class.
        /// </typeparam>
        /// <param name="endpoint">The endpoint URL.</param>
        /// <param name="method">The HTTP request method.</param>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns an instance of the <see cref="ApiResponse{result_type}"/> class.</returns>
        public static async Task<ApiResponse<result_type>>
        ExecuteRequestAsync<result_type, query_parameters_type>(string endpoint, Method method, string bearer_token, string client_id, query_parameters_type query_parameters, ApiRequestSettings api_request_settings)
        where query_parameters_type : class, new()
        {
            if (api_request_settings.IsNull())
            {
                api_request_settings = new ApiRequestSettings();
            }

            RestRequest request = Request(endpoint, method, bearer_token, client_id, api_request_settings);
            request = PagingUtil.AddPaging(request, query_parameters);

            ApiResponseBundle<result_type> response_bundle = await ExecuteRequestAsync<result_type>(request, api_request_settings);
            ApiResponse<result_type> api_response = new ApiResponse<result_type>(response_bundle);

            return api_response;
        }

        /// <summary>
        /// Asynchronously executes a rest request.
        /// </summary>
        /// <typeparam name="result_type">The type of the <see cref="IRestResponse{T}.Data"/> object.</typeparam>
        /// <param name="endpoint">The endpoint URL.</param>
        /// <param name="method">The HTTP request method.</param>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns an instance of the <see cref="ApiResponse{result_type}"/> class.</returns>
        public static async Task<ApiResponse<result_type>>
        ExecuteRequestAsync<result_type>(string endpoint, Method method, string bearer_token, string client_id, QueryParametersPage query_parameters, ApiRequestSettings api_request_settings)
        {
            if (api_request_settings.IsNull())
            {
                api_request_settings = new ApiRequestSettings();
            }

            RestRequest request = Request(endpoint, method, bearer_token, client_id, api_request_settings);
            request = PagingUtil.AddPaging(request, query_parameters);

            ApiResponseBundle<result_type> response_bundle = await ExecuteRequestAsync<result_type>(request, api_request_settings);
            ApiResponse<result_type> api_response = new ApiResponse<result_type>(response_bundle);

            return api_response;
        }

        /// <summary>
        /// Asynchronously executes a multi-page rest request.
        /// </summary>
        /// <typeparam name="data_type">
        /// The generic type passed through to the <see cref="Data{data_type}"/> class.
        /// This is the object type that the <code>data[{data_type}]</code> structure from Twitch will deserialize into.
        /// </typeparam>
        /// <typeparam name="result_type">The type of the <see cref="IRestResponse{T}.Data"/> object.</typeparam>
        /// <param name="endpoint">The endpoint URL.</param>
        /// <param name="method">The HTTP request method.</param>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns an instance of the <see cref="ApiResponse{result_type}"/> class.</returns>
        public static async Task<ApiResponse<result_type>>
        ExecuteRequestPagesAsync<data_type, result_type>(string endpoint, Method method, string bearer_token, string client_id, QueryParametersPage query_parameters, ApiRequestSettings api_request_settings)
        where result_type : DataPage<data_type>, IDataPage<data_type>, new()
        {
            ApiResponse<result_type> api_response = new ApiResponse<result_type>();
            List<data_type> data = new List<data_type>();

            if (query_parameters.IsNullOrDefault())
            {
                query_parameters = new QueryParametersPage();
            }

            if (api_request_settings.IsNullOrDefault())
            {
                api_request_settings = new ApiRequestSettings();
            }

            bool requesting = true;
            do
            {
                ApiResponse<result_type> api_page_response = await ExecuteRequestAsync<result_type>(endpoint, method, bearer_token, client_id, query_parameters, api_request_settings);
                if (api_page_response.result.data.IsValid())
                {
                    data.AddRange(api_page_response.result.data);
                }

                api_response = api_page_response;
                api_response.result.data = data;

                requesting = api_page_response.result.data.IsValid() && api_page_response.result.pagination.cursor.IsValid();
                if (requesting)
                {
                    query_parameters.after = api_page_response.result.pagination.cursor;
                }
            }
            while (requesting);

            // reset after in case the same set of query parameters are used for more than one request
            query_parameters.after = string.Empty;

            return api_response;
        }

        /// <summary>
        /// Asynchronously executes a rest request.
        /// </summary>
        /// <typeparam name="result_type">The type of the <see cref="IRestResponse{T}.Data"/> object.</typeparam>
        /// <param name="rest_request">The rest request to execute.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>An instance of the <see cref="ApiResponseBundle{result_type}"/> class.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static async Task<ApiResponseBundle<result_type>>
        ExecuteRequestAsync<result_type>(IRestRequest rest_request, ApiRequestSettings api_request_settings)
        {
            RestClient client = Client();
            IRestResponse<result_type> rest_response = await client.ExecuteTaskAsync<result_type>(rest_request);

            ApiResponse api_response = new ApiResponse(rest_response);
            api_response.exception = rest_response.ErrorException;

            if(api_request_settings.internal_error_handling == ErrorHandling.Error && !rest_response.ErrorException.IsNullOrDefault())
            {
                Log.Error(Log.FormatColumns(nameof(rest_response.ErrorException), rest_response.ErrorException.Message));

                throw rest_response.ErrorException;
            }

            ApiResponseBundle<result_type> response_bundle = await HandleStatus(rest_response, api_response, api_request_settings);

            return response_bundle;
        }

        #endregion

        #region Status handling

        /// <summary>
        /// Handles the status code returned with the rest response.
        /// </summary>
        /// <typeparam name="result_type">The type of the <see cref="IRestResponse{T}.Data"/> object.</typeparam>
        /// <param name="rest_response">The rest response.</param>
        /// <param name="api_response">The base response assembled from the <see cref="IRestResponse{T}"/> object.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>An instance of the <see cref="ApiResponseBundle{result_type}"/> class.</returns>
        private static async Task<ApiResponseBundle<result_type>>
        HandleStatus<result_type>(IRestResponse<result_type> rest_response, ApiResponse api_response, ApiRequestSettings api_request_settings)
        {
            ushort status_code = (ushort)api_response.status_code;
            string status_prefix = "'" + status_code + "' " + api_response.status_description + ".";

            Exception exception = null;
            api_response.exception = exception;

            // no error, return the valid response
            if (!api_response.status_error.IsValid() && rest_response.IsSuccessful)
            {
                Log.PrintLine(status_prefix + ", Requests remaining: " + api_response.rate_limit.remaining);

                return new ApiResponseBundle<result_type>(rest_response, api_response);
            }

            // small hack to use the deault handler '000' since it isn't a real status code
            if (!api_request_settings._status_handlers_settings.ContainsKey(status_code))
            {
                status_code = 000;
            }

            string exception_message = status_prefix;
            if (api_response.status_error_message.IsValid())
            {
                exception_message += " " + api_response.status_error_message + ".";
            }

            switch (api_request_settings._status_handlers_settings[status_code].handling)
            {
                case StatusHandling.Error:
                {                    
                    Log.Error(exception_message);

                    exception = new Exception(exception_message);
                    api_response.exception = exception;

                    throw exception;
                }

                case StatusHandling.Retry:
                {
                    lock (api_request_settings)
                    {
                        ++api_request_settings._status_handlers_settings[status_code].retry_count;
                        if(api_request_settings._status_handlers_settings[status_code].retry_count > api_request_settings._status_handlers_settings[status_code].retry_limit.value && api_request_settings._status_handlers_settings[status_code].retry_limit.value != -1)
                        {
                            Log.Warning(TimeStamp.TimeLong, status_prefix + " receieved from Twitch. Retry limit " + api_request_settings._status_handlers_settings[status_code].retry_limit.value + " exceeded. Cancelling request.");
                            api_request_settings._status_handlers_settings[status_code].retry_count = 0;

                            exception_message += " Retry limit exceeded.";
                            exception = new Exception(exception_message);
                            api_response.exception = exception;

                            throw exception;
                        }
                    }

                    // this should only ever be a concern when '429 - Too Many Requests' is receieved
                    TimeSpan time = api_response.rate_limit.reset - DateTime.Now;
                    if (api_response.rate_limit.remaining == 0 && time.TotalMilliseconds > 0)
                    {
                        Log.Warning(TimeStamp.TimeLong, "Request rate limit reached. Waiting " + time.TotalMilliseconds + "ms to execute the request again.");
                        await Task.Delay(time);
                        Log.Warning(TimeStamp.TimeLong, "Resuming request.");
                    }

                    ApiResponseBundle<result_type> response_bundle = await ExecuteRequestAsync<result_type>(rest_response.Request, api_request_settings);
                    rest_response = response_bundle.rest_response;
                    api_response = response_bundle.api_response;
                }
                break;

                case StatusHandling.Ignore:
                default:
                {
                    exception = new Exception(exception_message);
                    api_response.exception = exception;
                }
                break;
            }            

            return new ApiResponseBundle<result_type>(rest_response, api_response);
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Creates a new instance of a <see cref="RestRequest"/> which holds the information to request.
        /// </summary>        
        /// <param name="endpoint">The endpoint URL.</param>
        /// <param name="method">The HTTP method used when making the request.</param>
        /// <param name="bearer_token">The Bearer token or Client Id to authorize the request when only either is provided. If both are being provided, this is assumed to be the Bearer token.</param>
        /// <param name="client_id">The Client Id if both the Bearer token and Client Id are being provided.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns in instance of the <see cref="RestRequest"/> with the added bearer_token, client id, or both.</returns>
        /// <exception cref="ArgumentException">Thrown if both the <paramref name="bearer_token"/> and <paramref name="client_id"/> are invalid.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static RestRequest
        Request(string endpoint, Method method, string bearer_token, string client_id, ApiRequestSettings api_request_settings)
        {
            if(api_request_settings.input_hanlding == InputHandling.Error && !bearer_token.IsValid() && !client_id.IsValid())
            {
                throw new ArgumentException("A valid " + nameof(bearer_token) + " or " + nameof(client_id) + " must be provided.");
            }

            RestRequest request = new RestRequest(endpoint, method);
            if (bearer_token.IsValid())
            {
                request.AddHeader("Authorization", "Bearer " + bearer_token);
            }
            if (client_id.IsValid())
            {
                request.AddHeader("Client-ID", client_id);
            }

            return request;
        }

        /// <summary>
        /// Creates a new instance of a <see cref="RestClient"/> to execute the rest request to the Twitch API.
        /// </summary>
        /// <returns>Returns an instance of the <see cref="RestClient"/> configured to make requests to the Twitch API.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static RestClient
        Client()
        {
            RestClient client = new RestClient("https://api.twitch.tv/helix");
            client.AddHandler("application/json", new JsonDeserializer());
            client.AddHandler("application/xml", new JsonDeserializer());

            return client;
        }

        #endregion
    }
}