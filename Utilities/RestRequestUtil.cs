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
using TwitchNet.Models.Api;

using RestSharp;

namespace 
TwitchNet.Utilities
{    
    internal static class
    RestRequestUtil
    {
        #region Execute methods

        /// <summary>
        /// Asynchronously executes a Twitch API rest request using both an Bearer token and a client id for authenticrion.
        /// </summary>
        /// <typeparam name="data_type">
        /// The <see cref="Type"/> of the JSON data object.
        /// Restircted to a class.
        /// </typeparam>
        /// <param name="endpoint">The Twitch API endpoint url.</param>
        /// <param name="method">The HTTP method to use when making the request.</param>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="query_parameters">A set of parameters to customize the requests.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns an instance of the <see cref="ApiResponse{type}"/> model.</returns>
        public static async Task<ApiResponse<data_type>>
        ExecuteRequestAsync<data_type>(string endpoint, Method method, string bearer_token, string client_id, IList<QueryParameter> query_parameters, ApiRequestSettings api_request_settings)
        where data_type : class, new()
        {
            if (api_request_settings.IsNull())
            {
                api_request_settings = new ApiRequestSettings();
            }

            RestRequest request = Request(endpoint, method, bearer_token, client_id, api_request_settings);
            request = PagingUtil.AddPaging(request, query_parameters);

            (IRestResponse<ApiData<data_type>> rest_response, ApiResponse api_response) rest_result = await ExecuteRequestAsync<ApiData<data_type>>(request, api_request_settings);            
            ApiResponse<data_type> api_response = new ApiResponse<data_type>(rest_result);

            return api_response;
        }

        /// <summary>
        /// Asynchronously executes a Twitch API rest request using both an Bearer token and a client id for authenticrion.
        /// </summary>
        /// <typeparam name="data_type">
        /// The <see cref="Type"/> of the JSON data object.
        /// Restircted to a class.
        /// </typeparam>
        /// <typeparam name="query_parameters_type">
        /// The model <see cref="Type"/> of the query parameters.
        /// Restircted to a class.
        /// </typeparam>
        /// <param name="endpoint">The Twitch API endpoint url.</param>
        /// <param name="method">The HTTP method to use when making the request.</param>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="query_parameters">A set of parameters to customize the requests.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns an instance of the <see cref="ApiResponse{type}"/> model.</returns>
        public static async Task<ApiResponse<data_type>>
        ExecuteRequestAsync<data_type, query_parameters_type>(string endpoint, Method method, string bearer_token, string client_id, query_parameters_type query_parameters, ApiRequestSettings api_request_settings)
        where data_type             : class, new()
        where query_parameters_type : class, new()
        {
            if (api_request_settings.IsNull())
            {
                api_request_settings = new ApiRequestSettings();
            }

            RestRequest request = Request(endpoint, method, bearer_token, client_id, api_request_settings);
            request = PagingUtil.AddPaging(request, query_parameters);

            (IRestResponse<ApiData<data_type>> rest_response, ApiResponse api_response) rest_result = await ExecuteRequestAsync<ApiData<data_type>>(request, api_request_settings);
            ApiResponse<data_type> api_response = new ApiResponse<data_type>(rest_result);

            return api_response;
        }

        /// <summary>
        /// Asynchronously executes a paged Twitch API rest request using both an Bearer token and a client id for authenticrion.
        /// </summary>
        /// <typeparam name="data_type">
        /// The <see cref="Type"/> of the JSON data object.
        /// Restircted to a class.
        /// </typeparam>
        /// <param name="endpoint">The Twitch API endpoint url.</param>
        /// <param name="method">The HTTP method to use when making the request.</param>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="query_parameters">A set of parameters to customize the requests.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns an instance of the <see cref="ApiResponsePage{type}"/> model.</returns>
        public static async Task<ApiResponsePage<data_type>>
        ExecuteRequestPageAsync<data_type>(string endpoint, Method method, string bearer_token, string client_id, QueryParametersPage query_parameters, ApiRequestSettings api_request_settings)
        where data_type : class, new()
        {
            if (api_request_settings.IsNull())
            {
                api_request_settings = new ApiRequestSettings();
            }

            RestRequest request = Request(endpoint, method, bearer_token, client_id, api_request_settings);
            request = PagingUtil.AddPaging(request, query_parameters);

            (IRestResponse<ApiDataPage<data_type>> rest_response, ApiResponse api_response) rest_result = await ExecuteRequestAsync<ApiDataPage<data_type>>(request, api_request_settings);
            ApiResponsePage<data_type> api_response = new ApiResponsePage<data_type>(rest_result);

            return api_response;
        }

        /// <summary>
        /// Asynchronously executes all paged Twitch API rest requests for a particular endpoint using both an Bearer token and a client id for authenticrion.
        /// </summary>
        /// <typeparam name="return_type">
        /// The model <see cref="Type"/> to deserialize the result as.
        /// Restircted to a class.
        /// </typeparam>
        /// <param name="endpoint">The Twitch API endpoint url.</param>
        /// <param name="method">The HTTP method to use when making the request.</param>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="query_parameters">A set of parameters to customize the requests.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns an instance of the <see cref="ApiResponse{type}"/> model.</returns>
        public static async Task<ApiResponse<return_type>>
        ExecuteRequestAllPagesAsync<return_type>(string endpoint, Method method, string bearer_token, string client_id, QueryParametersPage query_parameters, ApiRequestSettings api_request_settings)
        where return_type : class, new()
        {
            ApiResponse<return_type> api_response = new ApiResponse<return_type>();
            List<return_type> data = new List<return_type>();

            if (api_request_settings.IsNull())
            {
                api_request_settings = new ApiRequestSettings();
            }

            if (query_parameters.IsNullOrDefault())
            {
                query_parameters = new QueryParametersPage();
            }
            query_parameters.first = 100;

            bool requesting = true;
            do
            {
                ApiResponsePage<return_type> _api_response = await ExecuteRequestPageAsync<return_type>(endpoint, method, bearer_token, client_id, query_parameters, api_request_settings);
                data.AddRange(_api_response.result.data);

                requesting = _api_response.result.data.IsValid() && _api_response.result.pagination.cursor.IsValid();
                if (requesting)
                {
                    query_parameters.after = _api_response.result.pagination.cursor;
                }
                else
                {
                    api_response = new ApiResponse<return_type>()
                    {
                        status_error        = _api_response.status_error,
                        status_description  = _api_response.status_description,
                        status_code         = _api_response.status_code,

                        rate_limit          = _api_response.rate_limit,

                        result              = new ApiData<return_type>()
                        {
                            data            = data
                        }
                    };
                }
            }
            while (requesting);

            // reset after in case the same set of query parameters are used for more than one request
            query_parameters.after = string.Empty;

            return api_response;
        }

        /// <summary>
        /// Asynchronously executes a Twitch API rest request.
        /// </summary>
        /// <typeparam name="data_type">
        /// The model <see cref="Type"/> to deserialize the result as.
        /// Restircted to a class.
        /// </typeparam>
        /// <param name="rest_request">The rest request to execute.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns a <see cref="Tuple{T1, T2}"/> that contains the <see cref="IRestResponse{T}"/> and <see cref="ApiResponse"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static async Task<(IRestResponse<data_type>, ApiResponse)>
        ExecuteRequestAsync<data_type>(IRestRequest rest_request, ApiRequestSettings api_request_settings)
        where data_type : class, new()
        {
            IRestResponse<data_type> rest_response = new RestResponse<data_type>();
            ApiResponse api_response = new ApiResponse();

            try
            {
                RestClient client = Client();
                rest_response = await client.ExecuteTaskAsync<data_type>(rest_request);
                if(api_request_settings.internal_error_handling == ErrorHandling.Error && !rest_response.ErrorException.IsNullOrDefault())
                {
                    Log.Error(Log.FormatColumns(nameof(rest_response.ErrorException), rest_response.ErrorException.Message));

                    throw rest_response.ErrorException;
                }

                api_response = new ApiResponse(rest_response);

                (IRestResponse<data_type> rest_response, ApiResponse api_response) rest_result = await HandleStatus(rest_response, api_response, api_request_settings);
                rest_response = rest_result.rest_response;
                api_response = rest_result.api_response;
            }
            catch(Exception exception)
            {
                if(api_request_settings.internal_error_handling == ErrorHandling.Error)
                {
                    Log.Error("Intenral exception: " + exception.Message);

                    throw exception;
                }
            }

            return (rest_response, api_response);
        }

        #endregion

        #region Status handling

        /// <summary>
        /// Handles the status code returned witht the rest response.
        /// </summary>
        /// <typeparam name="data_type">
        /// The model <see cref="Type"/> to deserialize the result as when retrying.
        /// Restircted to a class.
        /// </typeparam>
        /// <param name="rest_response">The rest client to execute the reuest.</param>
        /// <param name="rate_limit">Contains the request limit, requests remaining, and when the rate limit resets.</param>
        /// <param name="api_error">The API error that is returned with a bad request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns a <see cref="Tuple{T1, T2}"/> that contains the <see cref="IRestResponse{T}"/> and <see cref="ApiResponse"/>.</returns>
        /// <exception cref="Exception">Thrown when an API error is returned by Twitch or when an internal <see cref="Exception"/> is encountered.</exception>
        private static async Task<(IRestResponse<data_type>, ApiResponse)>
        HandleStatus<data_type>(IRestResponse<data_type> rest_response, ApiResponse api_response, ApiRequestSettings api_request_settings)
        where data_type : class, new()
        {
            ushort status = (ushort)api_response.status_code;
            string status_prefix = "'" + status + " " + api_response.status_description + "'";

            // no error, return the valid response
            if (!api_response.status_error.IsValid() || rest_response.IsSuccessful)
            {
                Log.PrintLine(status_prefix + ", Requests remaining: " + api_response.rate_limit.remaining);

                return (rest_response, api_response);
            }

            // small hack to use the deault handler '000' since it isn't a real status code
            if (!api_request_settings._status_handlers_settings.ContainsKey(status))
            {
                status = 000;
            }

            // error handling happens here 
            switch (api_request_settings._status_handlers_settings[status].handling)
            {
                case StatusHandling.Error:
                {
                    Log.Error("API Error: " + status_prefix + ": " + api_response.status_error);

                    throw new Exception(status_prefix + ": "+ api_response.status_error);
                }

                case StatusHandling.Retry:
                {
                    lock (api_request_settings)
                    {
                        ++api_request_settings._status_handlers_settings[status].retry_count;
                        if(api_request_settings._status_handlers_settings[status].retry_count > api_request_settings._status_handlers_settings[status].retry_limit.value && api_request_settings._status_handlers_settings[status].retry_limit.value != -1)
                        {
                            Log.Warning(TimeStamp.TimeLong, status_prefix + " receieved from Twitch. Retry limit " + api_request_settings._status_handlers_settings[status].retry_limit.value + " reached. Cancelling request.");
                            api_request_settings._status_handlers_settings[status].retry_count = 0;

                            break;
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

                    (IRestResponse<data_type> rest_response, ApiResponse api_response) rest_result = await ExecuteRequestAsync<data_type>(rest_response.Request, api_request_settings);
                    rest_response = rest_result.rest_response;
                    api_response = rest_result.api_response;
                    }
                break;

                case StatusHandling.Ignore:
                default:
                {

                }
                break;
            }

            return (rest_response, api_response);
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