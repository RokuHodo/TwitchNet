// standard namespaces
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Enums.Utilities;
using TwitchNet.Extensions;
using TwitchNet.Helpers.Json;
using TwitchNet.Helpers.Paging;
using TwitchNet.Interfaces.Helpers.Paging;
using TwitchNet.Models.Api;

// imported .dll's
using RestSharp;

namespace TwitchNet.Utilities
{    
    internal static class RestRequestUtil
    {
        #region Public execute

        /// <summary>
        /// Asynchronously executes a Twitch API rest request using either an oauth token or a client id for authenticrion.
        /// </summary>
        /// <typeparam name="return_type">
        /// The model <see cref="Type"/> to deserialize the result as.
        /// Restircted to a class.
        /// </typeparam>
        /// <param name="endpoint">The Twitch API endpoint url.</param>
        /// <param name="method">The HTTP method to use when making the request.</param>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="query_parameters">A set of parameters to customize the requests.</param>
        /// <returns>
        /// Returns an instance of the <see cref="TwitchResponse{type}"/> model.
        /// </returns>
        public static async Task<TwitchResponse<return_type>>
        ExecuteRequestAsync<return_type>(string endpoint, Method method, Authentication authentication, string token, params QueryParameter[] query_parameters)
        where return_type : class, new()
        {
            RestRequest request = Request(endpoint, method, authentication, token);

            TwitchResponse<return_type> response = await ExecuteRequestAsync<return_type>(request, query_parameters);

            return response;
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
        /// <param name="oauth_token">The OAuth token to be used in the request.</param>
        /// <param name="client_id">The Client Id of the application to be used in the request.</param>
        /// <param name="query_parameters">A set of parameters to customize the requests.</param>
        /// <returns>
        /// Returns an instance of the <see cref="TwitchResponse{type}"/> model.
        /// </returns>
        public static async Task<TwitchResponse<return_type>>
        ExecuteRequestAsync<return_type>(string endpoint, Method method, string oauth_token, string client_id, params QueryParameter[] query_parameters)
        where return_type : class, new()
        {
            RestRequest request = Request(endpoint, method, Authentication.Both, oauth_token, client_id);

            TwitchResponse<return_type> response = await ExecuteRequestAsync<return_type>(request, query_parameters);

            return response;
        }

        /// <summary>
        /// Asynchronously executes a paged Twitch API rest request using either an oauth token or a client id for authenticrion.
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
        /// Restircted to a class and must adhear to the <see cref="ITwitchQueryParameters"/> interface.
        /// </typeparam>
        /// <param name="endpoint">The Twitch API endpoint url.</param>
        /// <param name="method">The HTTP method to use when making the request.</param>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="query_parameters">A set of parameters to customize the requests.</param>
        /// <returns>
        /// Returns an instance of the <see cref="TwitchResponse{type}"/> model.
        /// </returns>
        public static async Task<TwitchResponse<return_type>>
        ExecuteRequestPageAsync<return_type, data_type, query_parameters_type>(string endpoint, Method method, Authentication authentication, string token, query_parameters_type query_parameters)
        where return_type           : class, ITwitchPage<data_type>, new()
        where data_type             : class, new()
        where query_parameters_type : class, ITwitchQueryParameters, new()
        {
            RestRequest request = Request(endpoint, method, authentication, token);

            TwitchResponse<return_type> response = await ExecuteRequestPageAsync<return_type, data_type, query_parameters_type>(request, query_parameters);

            return response;
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
        /// Restircted to a class and must adhear to the <see cref="ITwitchQueryParameters"/> interface.
        /// </typeparam>
        /// <param name="endpoint">The Twitch API endpoint url.</param>
        /// <param name="method">The HTTP method to use when making the request.</param>
        /// <param name="oauth_token">The OAuth token to be used in the request.</param>
        /// <param name="client_id">The Client Id of the application to be used in the request.</param>
        /// <param name="query_parameters">A set of parameters to customize the requests.</param>
        /// <returns>
        /// Returns an instance of the <see cref="TwitchResponse{type}"/> model.
        /// </returns>
        public static async Task<TwitchResponse<return_type>>
        ExecuteRequestPageAsync<return_type, data_type, query_parameters_type>(string endpoint, Method method, string oauth_token, string client_id, query_parameters_type query_parameters)
        where return_type           : class, ITwitchPage<data_type>, new()
        where data_type             : class, new()
        where query_parameters_type : class, ITwitchQueryParameters, new()
        {
            RestRequest request = Request(endpoint, method, Authentication.Both, oauth_token, client_id);

            TwitchResponse<return_type> response = await ExecuteRequestPageAsync<return_type, data_type, query_parameters_type>(request, query_parameters);

            return response;
        }

        /// <summary>
        /// Asynchronously executes all paged Twitch API rest requests for a particular endpoint using either an oauth token or a client id for authenticrion.
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
        /// Restircted to a class and must adhear to the <see cref="ITwitchQueryParameters"/> interface.
        /// </typeparam>
        /// <param name="endpoint">The Twitch API endpoint url.</param>
        /// <param name="method">The HTTP method to use when making the request.</param>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="query_parameters">A set of parameters to customize the requests.</param>
        /// <returns>
        /// Returns an instance of the <see cref="TwitchResponse{type}"/> model.
        /// </returns>
        public static async Task<TwitchResponse<IList<return_type>>>
        ExecuteRequestAllPagesAsync<return_type, page_type, query_parameters_type>(string endpoint, Method method, Authentication authentication, string token, query_parameters_type query_parameters)
        where return_type           : class, new()
        where page_type             : class, ITwitchPage<return_type>, new()
        where query_parameters_type : class, ITwitchQueryParameters, new()
        {
            RestRequest request = Request(endpoint, method, authentication, token);

            TwitchResponse<IList<return_type>> twitch_response = await ExecuteRequestPagesAllAsync<return_type, page_type, query_parameters_type>(request, query_parameters);

            return twitch_response;
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
        /// Restircted to a class and must adhear to the <see cref="ITwitchQueryParameters"/> interface.
        /// </typeparam>
        /// <param name="endpoint">The Twitch API endpoint url.</param>
        /// <param name="method">The HTTP method to use when making the request.</param>
        /// <param name="oauth_token">The OAuth token to be used in the request.</param>
        /// <param name="client_id">The Client Id of the application to be used in the request.</param>
        /// <param name="query_parameters">A set of parameters to customize the requests.</param>
        /// <returns>
        /// Returns an instance of the <see cref="TwitchResponse{type}"/> model.
        /// </returns>
        public static async Task<TwitchResponse<IList<return_type>>>
        ExecuteRequestAllPagesAsync<return_type, page_type, query_parameters_type>(string endpoint, Method method, string oauth_token, string client_id, query_parameters_type query_parameters)
        where return_type           : class, new()
        where page_type             : class, ITwitchPage<return_type>, new()
        where query_parameters_type : class, ITwitchQueryParameters, new()
        {
            RestRequest request = Request(endpoint, method, Authentication.Both, oauth_token, client_id);

            TwitchResponse<IList<return_type>> twitch_response = await ExecuteRequestPagesAllAsync<return_type, page_type, query_parameters_type>(request, query_parameters);

            return twitch_response;
        }

        #endregion

        #region Private execute

        /// <summary>
        /// Asynchronously executes a Twitch API rest request.
        /// </summary>
        /// <typeparam name="return_type">
        /// The model <see cref="Type"/> to deserialize the result as.
        /// Restircted to a class.
        /// </typeparam>
        /// <param name="request">The rest request to execute.</param>
        /// <returns>
        /// Returns an instance of the <see cref="TwitchResponse{type}"/> model.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static async Task<TwitchResponse<return_type>>
        ExecuteRequestAsync<return_type>(IRestRequest request)
        where return_type : class, new()
        {
            RestClient client = Client();
            IRestResponse<return_type> response = await client.ExecuteTaskAsync<return_type>(request);

            // TODO: ExecuteRequestAsync - Implemenet customizable settings for each request that the user can tweak
            // TODO: ExecuteRequestAsync - Handle any settings here since this is where all execute paths lead to
            TwitchResponse<return_type> twitch_response = new TwitchResponse<return_type>(response);

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
        /// <param name="query_parameters">A set of parameters to customize the requests.</param>
        /// <returns>
        /// Returns an instance of the <see cref="TwitchResponse{type}"/> model.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static async Task<TwitchResponse<return_type>>
        ExecuteRequestAsync<return_type>(IRestRequest request, params QueryParameter[] query_parameters)
        where return_type : class, new()
        {
            request = PagingUtil.AddPaging(request, query_parameters);

            TwitchResponse<return_type> twitch_response = await ExecuteRequestAsync<return_type>(request);

            return twitch_response;
        }

        /// <summary>
        /// Asynchronously executes a paged Twitch API rest request.
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
        /// Restircted to a class and must adhear to the <see cref="ITwitchQueryParameters"/> interface.
        /// </typeparam>
        /// <param name="request">The rest request to execute.</param>
        /// <param name="query_parameters">A set of parameters to customize the requests.</param>
        /// <returns>
        /// Returns an instance of the <see cref="TwitchResponse{type}"/> model.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static async Task<TwitchResponse<return_type>>
        ExecuteRequestPageAsync<return_type, data_type, query_parameters_type>(IRestRequest request, query_parameters_type query_parameters)
        where return_type           : class, ITwitchPage<data_type>, new()
        where data_type             : class, new()
        where query_parameters_type : class, ITwitchQueryParameters, new()
        {
            request = PagingUtil.AddPaging(request, query_parameters);

            TwitchResponse<return_type> twitch_response = await ExecuteRequestAsync<return_type>(request);

            return twitch_response;
        }

        /// <summary>
        /// Asynchronously executes a all paged Twitch API rest requests.
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
        /// Restircted to a class and must adhear to the <see cref="ITwitchQueryParameters"/> interface.
        /// </typeparam>
        /// <param name="request">The rest request to execute.</param>
        /// <param name="query_parameters">A set of parameters to customize the requests.</param>
        /// <returns>
        /// Returns an instance of the <see cref="TwitchResponse{type}"/> model.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static async Task<TwitchResponse<IList<return_type>>>
        ExecuteRequestPagesAllAsync<return_type, page_type, query_parameters_type>(IRestRequest request, query_parameters_type query_parameters)
        where return_type           : class, new()
        where page_type             : class, ITwitchPage<return_type>, new()
        where query_parameters_type : class, ITwitchQueryParameters, new()
        {
            if (query_parameters.IsNull())
            {
                query_parameters = new query_parameters_type();
            }

            TwitchResponse<IList<return_type>> twitch_response = new TwitchResponse<IList<return_type>>();
            twitch_response.result = new List<return_type>();

            bool requesting = true;
            do
            {
                // request the page and add each element to the result
                TwitchResponse<page_type> response = await ExecuteRequestPageAsync<page_type, return_type, query_parameters_type>(request, query_parameters);
                foreach (return_type element in response.result.data)
                {
                    twitch_response.result.Add(element);
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

            return twitch_response;
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
        /// <returns></returns>
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
        /// <returns></returns>
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