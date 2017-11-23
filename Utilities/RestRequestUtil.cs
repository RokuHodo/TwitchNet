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
        /// <param name="request">The rest request to execute.</param>
        /// <returns>Returns an instance of the <see cref="ApiResponse{type}"/> model.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static async Task<(IRestResponse<return_type> rest_response, RateLimit rate_limit, ApiError api_error)>
        ExecuteRequestAsync<return_type>(IRestRequest request, ApiRequestSettings settings)
        where return_type : class, new()
        {
            if (settings.IsNullOrDefault())
            {
                settings = new ApiRequestSettings();
            }            

            RestClient client = Client();
            IRestResponse<return_type> rest_response = await client.ExecuteTaskAsync<return_type>(request);

            RateLimit rate_limit = new RateLimit(rest_response);

            ApiError api_error = JsonConvert.DeserializeObject<ApiError>(rest_response.Content);

            // TODO: ExecuteRequestAsync - Implement more customizable settings for each request that the user can tweak
            switch ((ushort)rest_response.StatusCode) 
            {
                case 429:
                {
                    rest_response = await StatusHandler_TooManyRequest(request, rest_response, rate_limit, settings);
                }
                break;

                case 500:
                {
                    rest_response = await StatusHandler_IntenralServerError(request, rest_response, rate_limit, settings);
                }
                break;

                default:
                {
                    ExceptionUtil.ThrowIf(api_error.message.IsValid(), api_error.status + ": " + api_error.error + " - " + api_error.message);
                }
                break;
            }

            return (rest_response, rate_limit, api_error);
        }

        #endregion

        #region Status handlers

        private static async Task<IRestResponse<return_type>>
        StatusHandler_TooManyRequest<return_type>(IRestRequest request, IRestResponse<return_type> response, RateLimit rate_limit, ApiRequestSettings settings)
        where return_type : class, new()
        {
            switch (settings.too_many_request_handling)
            {
                case TooManyRequestHandling.Error:
                {
                    throw new Exception("Status Code: " + response.StatusCode + " - " + response.StatusDescription);
                }

                case TooManyRequestHandling.Wait:
                {
                    lock (settings)
                    {
                        ++settings._too_many_request_retry_count;
                        if(settings._too_many_request_retry_count > settings.too_many_request_retry_limit)
                        {
                            Log.Warning(TimeStamp.TimeLong, "Status '429' receieved from Twitch. Wait limit " + settings.too_many_request_retry_limit + " reached. Cancelling request.");
                            settings._too_many_request_retry_count = 0;

                            break;
                        }
                    }

                    TimeSpan time = rate_limit.reset - DateTime.Now;
                    if (time.TotalMilliseconds > 0)
                    {
                        Log.Warning(TimeStamp.TimeLong, "Status '429' receieved from Twitch. Waiting " + time.TotalMilliseconds + "ms to execute request again.");
                        await Task.Delay(time);
                    }

                    Log.Warning(TimeStamp.TimeLong, "Resuming request.");
                    (IRestResponse<return_type> rest_response, RateLimit rate_limit, ApiError api_error) rest_result = await ExecuteRequestAsync<return_type>(request, settings);
                    response = rest_result.rest_response;
                }
                break;

                case TooManyRequestHandling.Ignore:
                default:
                {

                }
                break;
            }

            return response;
        }

        private static async Task<IRestResponse<return_type>>
        StatusHandler_IntenralServerError<return_type>(IRestRequest request, IRestResponse<return_type> response, RateLimit rate_limit, ApiRequestSettings settings)
        where return_type : class, new()
        {
            switch (settings.internal_server_error_handling)
            {
                case InternalServerErrorHandling.Error:
                {
                    throw new Exception("Status Code: " + response.StatusCode + " - " + response.StatusDescription);
                }

                case InternalServerErrorHandling.Retry:
                {
                    lock (settings)
                    {
                        ++settings._internal_server_error_retry_count;
                        if (settings._internal_server_error_retry_count > settings.internal_server_error_retry_limit && settings.internal_server_error_retry_limit != -1)
                        {
                            Log.Warning(TimeStamp.TimeLong, "Status '500' receieved from Twitch. Retry limit " + settings.internal_server_error_retry_limit + " reached. Cancelling request.");
                            settings._internal_server_error_retry_count = 0;

                            break;
                        }
                    }

                    Log.Warning(TimeStamp.TimeLong, "Status '500' receieved from Twitch. Retrying, count = " + settings._internal_server_error_retry_count);
                    (IRestResponse<return_type> rest_response, RateLimit rate_limit, ApiError api_error) rest_result = await ExecuteRequestAsync<return_type>(request, settings);
                    response = rest_result.rest_response;
                }
                break;

                case InternalServerErrorHandling.Ignore:
                default:
                {

                }
                break;
            }

            return response;
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