// standard namespaces
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Debug;
using TwitchNet.Enums.Debug;
using TwitchNet.Enums.Utilities;
using TwitchNet.Extensions;
using TwitchNet.Helpers.Json;
using TwitchNet.Models.Api;
using TwitchNet.Models.Paging;
using TwitchNet.Models.Utilities;

// imported .dll's
using RestSharp;

namespace TwitchNet.Utilities
{    
    internal static class RestRequestUtil
    {
        #region Execute methods

        /// <summary>
        /// Asynchronously executes a Twitch API rest request using both an oauth token and a client id for authenticrion.
        /// </summary>
        /// <typeparam name="return_type">
        /// The model <see cref="Type"/> to deserialize the result as.
        /// Restircted to a class.
        /// </typeparam>
        /// <param name="endpoint">The Twitch API endpoint url.</param>
        /// <param name="method">The HTTP method to use when making the request.</param>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="query_parameters">A set of parameters to customize the requests.</param>
        /// <returns>Returns an instance of the <see cref="ApiResponse{type}"/> model.</returns>
        public static async Task<ApiResponse<return_type>>
        ExecuteRequestAsync<return_type>(string endpoint, Method method, Authentication authentication, string oauth_token, string client_id, IList<QueryParameter> query_parameters)
        where return_type : class, new()
        {
            RestRequest request = Request(endpoint, method, authentication, oauth_token, client_id);
            request = PagingUtil.AddPaging(request, query_parameters);

            IRestResponse<ApiData<return_type>> rest_response = await ExecuteRequestAsync<ApiData<return_type>>(request);
            ApiResponse<return_type> api_response = new ApiResponse<return_type>(rest_response);

            return api_response;
        }

        /// <summary>
        /// Asynchronously executes a Twitch API rest request using both an oauth token and a client id for authenticrion.
        /// </summary>
        /// <typeparam name="return_type">
        /// The model <see cref="Type"/> to deserialize the result as.
        /// Restircted to a class.
        /// </typeparam>
        /// <param name="endpoint">The Twitch API endpoint url.</param>
        /// <param name="method">The HTTP method to use when making the request.</param>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="query_parameters">A set of parameters to customize the requests.</param>
        /// <returns>Returns an instance of the <see cref="ApiResponse{type}"/> model.</returns>
        public static async Task<ApiResponse<return_type>>
        ExecuteRequestAsync<return_type, query_parameters_type>(string endpoint, Method method, Authentication authentication, string oauth_token, string client_id, query_parameters_type query_parameters)
        where return_type           : class, new()
        where query_parameters_type : class, new()
        {
            RestRequest request = Request(endpoint, method, authentication, oauth_token, client_id);
            request = PagingUtil.AddPaging(request, query_parameters);

            IRestResponse<ApiData<return_type>> rest_response = await ExecuteRequestAsync<ApiData<return_type>>(request);
            ApiResponse<return_type> api_response = new ApiResponse<return_type>(rest_response);

            return api_response;
        }

        /// <summary>
        /// Asynchronously executes a paged Twitch API rest request using both an oauth token and a client id for authenticrion.
        /// </summary>
        /// <typeparam name="return_type">
        /// The model <see cref="Type"/> to deserialize the result as.
        /// Restircted to a class and must adhear to the <see cref="ITwitchPage{type}"/> interface.
        /// </typeparam>
        /// <typeparam name="data_type">
        /// The model <see cref="Type"/> of the <code>data</code> list in the payload.
        /// Restircted to a class.
        /// </typeparam>
        /// <typeparam name="query_parameters_type">
        /// The <see cref="Type"/> of the <paramref name="query_parameters"/>.
        /// Restircted to a class and must adhear to the <see cref="IQueryParametersPage"/> interface.
        /// </typeparam>
        /// <param name="endpoint">The Twitch API endpoint url.</param>
        /// <param name="method">The HTTP method to use when making the request.</param>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="query_parameters">A set of parameters to customize the requests.</param>
        /// <returns>Returns an instance of the <see cref="ApiResponsePage{type}"/> model.</returns>
        public static async Task<ApiResponsePage<data_type>>
        ExecuteRequestPageAsync<data_type>(string endpoint, Method method, Authentication authentication, string oauth_token, string client_id, QueryParametersPage query_parameters)
        where data_type : class, new()
        {
            RestRequest request = Request(endpoint, method, authentication, oauth_token, client_id);
            request = PagingUtil.AddPaging(request, query_parameters);

            IRestResponse<ApiDataPage<data_type>> rest_response = await ExecuteRequestAsync<ApiDataPage<data_type>>(request);
            ApiResponsePage<data_type> api_response = new ApiResponsePage<data_type>(rest_response);

            return api_response;
        }

        /// <summary>
        /// Asynchronously executes all paged Twitch API rest requests for a particular endpoint using both an oauth token and a client id for authenticrion.
        /// </summary>
        /// <typeparam name="return_type">
        /// The model <see cref="Type"/> to deserialize the result as.
        /// Restircted to a class.
        /// </typeparam>
        /// <typeparam name="page_type">
        /// The model <see cref="Type"/> to deserialize each paged result as.
        /// Restircted to a class and must adhear to the <see cref="ITwitchPage{type}"/> interface.
        /// </typeparam>
        /// <typeparam name="query_parameters_type">
        /// The <see cref="Type"/> of the <paramref name="query_parameters"/>.
        /// Restircted to a class and must adhear to the <see cref="IQueryParametersPage"/> interface.
        /// </typeparam>
        /// <param name="endpoint">The Twitch API endpoint url.</param>
        /// <param name="method">The HTTP method to use when making the request.</param>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="query_parameters">A set of parameters to customize the requests.</param>
        /// <returns>Returns an instance of the <see cref="ApiResponse{type}"/> model.</returns>
        public static async Task<ApiResponse<return_type>>
        ExecuteRequestAllPagesAsync<return_type>(string endpoint, Method method, Authentication authentication, string oauth_token, string client_id, QueryParametersPage query_parameters)
        where return_type : class, new()
        {
            if (query_parameters.IsNull())
            {
                query_parameters = new QueryParametersPage();
            }

            ApiResponse<return_type> twitch_response = new ApiResponse<return_type>();
            twitch_response.result = new ApiData<return_type>();
            twitch_response.result.data = new List<return_type>();

            bool requesting = true;
            do
            {
                // request the page and add each element to the result
                // TODO: ExecuteRequestAsync - Need to handle '429' TooManyRequestHandling.Ignore here for multi-page responses
                ApiResponsePage<return_type> response = await ExecuteRequestPageAsync<return_type>(endpoint, method, authentication, oauth_token, client_id, query_parameters);
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
        private static async Task<IRestResponse<return_type>>
        ExecuteRequestAsync<return_type>(IRestRequest request, TwitchRequestSettings settings = null)
        where return_type : class, new()
        {
            if (settings.IsNull())
            {
                settings = new TwitchRequestSettings();
            }

            RestClient client = Client();
            IRestResponse<return_type> response = await client.ExecuteTaskAsync<return_type>(request);

            // TODO: ExecuteRequestAsync - Implemenet customizable settings for each request that the user can tweak
            // TODO: ExecuteRequestAsync - Handle any settings here since this is where all execute paths lead to

            RateLimit rate_limit = new RateLimit(response);

            // TODO: ExecuteRequestAsync - Handle these in some sort of settings class or something
            switch ((ushort)response.StatusCode)
            {
                case 429:
                {
                    // TODO: ExecuteRequestAsync - Find a much mroe graceful way to handle handling cases
                    if (settings.too_many_request_handling == TooManyRequestHandling.Error)
                    {
                        throw new Exception("Status Code: " + response.StatusCode + " - " + response.StatusDescription);
                    }
                    else if(settings.too_many_request_handling == TooManyRequestHandling.Wait)
                    {
                        TimeSpan time = rate_limit.reset - DateTime.Now;
                        if (time.TotalMilliseconds < 0)
                        {
                            Log.Warning(TimeStamp.TimeLong, "Status '429' receieved from Twitch. Time to reset, " + time.TotalMilliseconds + "ms, was negative.");
                        }
                        else
                        {
                            Log.Warning(TimeStamp.TimeLong, "Status '429' receieved from Twitch. Waiting " + time.TotalMilliseconds + "ms to execute request again.");
                            await Task.Delay(time);
                        }

                        Log.Warning(TimeStamp.TimeLong, "Resuming request.");
                        response = await ExecuteRequestAsync<return_type>(request);
                    }
                    else
                    {
                        return response;
                    }
                }
                break;

                default:
                {
                    Log.PrintLine(((ushort)response.StatusCode).ToString());
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
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token_primiary">The OAuth token or Client Id to authorize the request when only either is provided. If both are being provided, this is assumed to be the OAuth token.</param>
        /// <param name="token_supplementary">The Client Id if both the OAuth token and Client Id are being provided.</param>
        /// <returns>Returns in instance of the <see cref="RestRequest"/> with the added oauth_token, client id, or both.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static RestRequest
        Request(string endpoint, Method method, Authentication authentication, string token_primiary, string token_supplementary = "")
        {
            RestRequest request = new RestRequest(endpoint, method);
            switch (authentication)
            {
                case Authentication.Authorization:
                    {
                        request.AddHeader("Authorization", "Bearer " + token_primiary);
                    }
                    break;

                case Authentication.Client_ID:
                    {
                        request.AddHeader("Client-ID", token_primiary);
                    }
                    break;

                case Authentication.Both:
                    {
                        request.AddHeader("Authorization", "Bearer " + token_primiary);
                        request.AddHeader("Client-ID", token_supplementary);
                    }
                    break;
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