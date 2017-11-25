// standard namespaces
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Debug;
using TwitchNet.Enums.Debug;
using TwitchNet.Enums.Api;
using TwitchNet.Extensions;
using TwitchNet.Helpers.Json;
using TwitchNet.Models.Api;

// imported .dll's
using Newtonsoft.Json;

using RestSharp;

namespace 
TwitchNet.Utilities
{    
    internal static class RestRequestUtil
    {
        #region Execute methods

        /// <summary>
        /// Asynchronously executes a Twitch API rest request using both an Bearer token and a client id for authenticrion.
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
        /// <returns>Returns an instance of the <see cref="ApiResponse{type}"/> model.</returns>
        public static async Task<ApiResponse<return_type>>
        ExecuteRequestAsync<return_type>(string endpoint, Method method, string bearer_token, string client_id, IList<QueryParameter> query_parameters, ApiRequestSettings settings)
        where return_type : class, new()
        {
            RestRequest request = Request(endpoint, method, bearer_token, client_id);
            request = PagingUtil.AddPaging(request, query_parameters);

            (IRestResponse<ApiData<return_type>> rest_response, RateLimit rate_limit, ApiError api_error) rest_result = await ExecuteRequestAsync<ApiData<return_type>>(request, settings);            
            ApiResponse<return_type> api_response = new ApiResponse<return_type>(rest_result.rest_response, rest_result.rate_limit, rest_result.api_error);

            return api_response;
        }

        /// <summary>
        /// Asynchronously executes a Twitch API rest request using both an Bearer token and a client id for authenticrion.
        /// </summary>
        /// <typeparam name="return_type">
        /// The model <see cref="Type"/> to deserialize the result as.
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
        /// <returns>Returns an instance of the <see cref="ApiResponse{type}"/> model.</returns>
        public static async Task<ApiResponse<return_type>>
        ExecuteRequestAsync<return_type, query_parameters_type>(string endpoint, Method method, string bearer_token, string client_id, query_parameters_type query_parameters, ApiRequestSettings settings)
        where return_type           : class, new()
        where query_parameters_type : class, new()
        {
            RestRequest request = Request(endpoint, method, bearer_token, client_id);
            request = PagingUtil.AddPaging(request, query_parameters);

            (IRestResponse<ApiData<return_type>> rest_response, RateLimit rate_limit, ApiError api_error) rest_result = await ExecuteRequestAsync<ApiData<return_type>>(request, settings);
            ApiResponse<return_type> api_response = new ApiResponse<return_type>(rest_result.rest_response, rest_result.rate_limit, rest_result.api_error);

            return api_response;
        }

        /// <summary>
        /// Asynchronously executes a paged Twitch API rest request using both an Bearer token and a client id for authenticrion.
        /// </summary>
        /// <typeparam name="data_type">
        /// The model <see cref="Type"/> of the <code>data</code> list in the payload.
        /// Restircted to a class.
        /// </typeparam>
        /// <param name="endpoint">The Twitch API endpoint url.</param>
        /// <param name="method">The HTTP method to use when making the request.</param>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="query_parameters">A set of parameters to customize the requests.</param>
        /// <returns>Returns an instance of the <see cref="ApiResponsePage{type}"/> model.</returns>
        public static async Task<ApiResponsePage<data_type>>
        ExecuteRequestPageAsync<data_type>(string endpoint, Method method, string bearer_token, string client_id, QueryParametersPage query_parameters, ApiRequestSettings settings)
        where data_type : class, new()
        {
            RestRequest request = Request(endpoint, method, bearer_token, client_id);
            request = PagingUtil.AddPaging(request, query_parameters);

            (IRestResponse<ApiDataPage<data_type>> rest_response, RateLimit rate_limit, ApiError api_error) rest_result = await ExecuteRequestAsync<ApiDataPage<data_type>>(request, settings);
            ApiResponsePage<data_type> api_response = new ApiResponsePage<data_type>(rest_result.rest_response, rest_result.rate_limit, rest_result.api_error);

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
        /// <returns>Returns an instance of the <see cref="ApiResponse{type}"/> model.</returns>
        public static async Task<ApiResponse<return_type>>
        ExecuteRequestAllPagesAsync<return_type>(string endpoint, Method method, string bearer_token, string client_id, QueryParametersPage query_parameters, ApiRequestSettings settings)
        where return_type : class, new()
        {
            if (query_parameters.IsNullOrDefault())
            {
                query_parameters = new QueryParametersPage();
            }
            query_parameters.first = 100;

            ApiResponse<return_type> twitch_response = new ApiResponse<return_type>();
            twitch_response.result = new ApiData<return_type>();
            twitch_response.result.data = new List<return_type>();

            bool requesting = true;
            do
            {
                // request the page and add each element to the result
                // TODO: ExecuteRequestAsync - Need to handle '429' TooManyRequestHandling.Ignore here for multi-page responses
                ApiResponsePage<return_type> response = await ExecuteRequestPageAsync<return_type>(endpoint, method, bearer_token, client_id, query_parameters, settings);
                foreach (return_type element in response.result.data)
                {
                    twitch_response.result.data.Add(element);
                }

                // check to see if there is a new page to request
                requesting = response.result.data.IsValid() && response.result.pagination.cursor.IsValid();
                if (requesting)
                {
                    query_parameters.after = response.result.pagination.cursor;
                }
                else
                {
                    twitch_response.CloneBaseProperties(response);
                }
            }
            while (requesting);

            // reset after in case the same set of query parameters are used for more than one request
            query_parameters.after = string.Empty;

            return twitch_response;
        }

        /// <summary>
        /// Asynchronously executes a Twitch API rest request.
        /// </summary>
        /// <typeparam name="return_type">
        /// The model <see cref="Type"/> to deserialize the result as.
        /// Restircted to a class.
        /// </typeparam>
        /// <param name="rest_request">The rest request to execute.</param>
        /// <returns>Returns an instance of the <see cref="ApiResponse{type}"/> model.</returns>
        /// <exception cref="Exception">Thrown if an error is returned by Twitch after making the request.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static async Task<(IRestResponse<return_type> rest_response, RateLimit rate_limit, ApiError api_error)>
        ExecuteRequestAsync<return_type>(IRestRequest rest_request, ApiRequestSettings settings)
        where return_type : class, new()
        {
            if (settings.IsNullOrDefault())
            {
                settings = new ApiRequestSettings();
            }            

            RestClient client = Client();
            IRestResponse<return_type> rest_response = await client.ExecuteTaskAsync<return_type>(rest_request);

            RateLimit rate_limit = new RateLimit(rest_response);
            ApiError api_error = JsonConvert.DeserializeObject<ApiError>(rest_response.Content);

            rest_response = await HandleStatus(rest_response, rate_limit, api_error, settings);

            return (rest_response, rate_limit, api_error);
        }

        #endregion

        #region Status handler methods

        /// <summary>
        /// Handles the status code returned witht the rest response.
        /// </summary>
        /// <typeparam name="return_type">
        /// The model <see cref="Type"/> to deserialize the result as when retrying.
        /// Restircted to a class.
        /// </typeparam>
        /// <param name="rest_request">The rest request to execute.</param>
        /// <param name="rest_response">The rest client to execute the reuest.</param>
        /// <param name="rate_limit">Contains the request limit, requests remaining, and when the rate limit resets.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns the <see cref="IRestResponse{T}"/>.</returns>
        private static async Task<IRestResponse<return_type>>
        HandleStatus<return_type>(IRestResponse<return_type> rest_response, RateLimit rate_limit, ApiError api_error, ApiRequestSettings api_request_settings)
        where return_type : class, new()
        {
            ushort status = (ushort)rest_response.StatusCode;
            string status_prefix = "'" + status + " " + rest_response.StatusDescription + "'";

            // no error, return the valid response
            if (!api_error.error.IsValid() || rest_response.IsSuccessful)
            {
                Log.PrintLine(ConsoleColor.Green, status_prefix);

                return rest_response;
            }

            // small hack to use the deault handler '000' since it isn't a real status code
            if (!api_request_settings.status_handlers_settings.ContainsKey(status))
            {
                status = 000;
            }

            // error handling happens here 
            switch (api_request_settings.status_handlers_settings[status].handling)
            {
                case StatusHandling.Error:
                {
                    throw new Exception(status_prefix + ": "+ api_error.message);
                }

                case StatusHandling.Retry:
                {
                    lock (api_request_settings)
                    {
                        ++api_request_settings.status_handlers_settings[status].retry_count;
                        if(api_request_settings.status_handlers_settings[status].retry_count > api_request_settings.status_handlers_settings[status].retry_limit.value && api_request_settings.status_handlers_settings[status].retry_limit.value != -1)
                        {
                            Log.Warning(TimeStamp.TimeLong, status_prefix + " receieved from Twitch. Retry limit " + api_request_settings.status_handlers_settings[status].retry_limit.value + " reached. Cancelling request.");
                            api_request_settings.status_handlers_settings[status].retry_count = 0;

                            break;
                        }
                    }

                    // this should only ever be a concern when '429 - Too Many Requests' is receieved
                    TimeSpan time = rate_limit.reset - DateTime.Now;
                    if (time.TotalMilliseconds > 0)
                    {
                        Log.Warning(TimeStamp.TimeLong, "Request rate limit reached. Waiting " + time.TotalMilliseconds + "ms to execute request again.");
                        await Task.Delay(time);
                    }
                    Log.Warning(TimeStamp.TimeLong, "Resuming request.");

                    (IRestResponse<return_type> rest_response, RateLimit rate_limit, ApiError api_error) result = await ExecuteRequestAsync<return_type>(rest_response.Request, api_request_settings);
                    rest_response   = result.rest_response;
                    rate_limit      = result.rate_limit;
                    api_error       = result.api_error;
                }
                break;

                case StatusHandling.Ignore:
                default:
                {

                }
                break;
            }

            return rest_response;
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
        /// <returns>Returns in instance of the <see cref="RestRequest"/> with the added bearer_token, client id, or both.</returns>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="endpoint"/> is invalid.</exception>
        /// <exception cref="Exception">Thrown if both the <paramref name="bearer_token"/> and <paramref name="client_id"/> are invalid.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static RestRequest
        Request(string endpoint, Method method, string bearer_token, string client_id)
        {
            ExceptionUtil.ThrowIfInvalid(endpoint, nameof(endpoint));
            ExceptionUtil.ThrowIf(!bearer_token.IsValid() && !client_id.IsValid(), "A valid " + nameof(bearer_token) + " or token or " + nameof(client_id) + "must be provided.");

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