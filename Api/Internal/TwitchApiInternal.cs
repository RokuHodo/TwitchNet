﻿// standard namespaces
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Enums.Api;
using TwitchNet.Extensions;
using TwitchNet.Helpers.Json;
using TwitchNet.Helpers.Paging.Users;
using TwitchNet.Models.Api.Users;
using TwitchNet.Utilities;

// imported .dll's
using RestSharp;

namespace TwitchNet.Api.Internal
{
    internal static partial class TwitchApiInternal
    {
        #region Users

        /// <summary>
        /// Asynchronously gets a user's information by their id or login name.
        /// Optional Scope: 'user:read:email'
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="query_name">How to get the user's information, by 'id' or by 'login'.</param>
        /// <param name="lookup">The id(s) or login(s) to get the information for.</param>
        /// <returns></returns>
        internal static async Task<Users> GetUsersAsync(Authentication authentication, string token, string query_name, params string[] lookup)
        {
            RestRequest request = Request("users", Method.GET, authentication, token);
            foreach (string element in lookup)
            {
                request.AddQueryParameter(query_name, element);
            }

            RestClient client = Client();
            IRestResponse<Users> response = await client.ExecuteTaskAsync<Users>(request);

            return response.Data;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users, or a single page of the following/follower lists of one user.
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="to_id">Optional. The user to compare to. Used to get a user's follower list.</param>
        /// <param name="from_id">Optional. The user to compare from. Used to get the following list of a user.</param>
        /// <param name="parameters">Optional. A set of parameters to customize the requests. The 'to_id' and 'from_id' properties in the parameters are ignored if specified.</param>
        /// <returns></returns>
        internal static async Task<Follows> GetUserRelationshipPageAsync(Authentication authentication, string token, string to_id, string from_id, FollowsParameters parameters = null)
        {
            RestRequest request = Request("users/follows", Method.GET, authentication, token);

            if (parameters.isNull())
            {
                parameters = new FollowsParameters();
            }
            parameters.to_id = to_id;
            parameters.from_id = from_id;

            PagingUtil.AddPaging(request, parameters);

            RestClient client = Client();
            IRestResponse<Follows> response = await client.ExecuteTaskAsync<Follows>(request);

            return response.Data;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users, or the complete following/follower lists of one user.
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="to_id">Optional. The user to compare to. Used to get a user's follower list.</param>
        /// <param name="from_id">Optional. The user to compare from. Used to get the following list of a user.</param>
        /// <param name="parameters">Optional. A set of parameters to customize the requests. The 'to_id' and 'from_id' properties in the parameters are ignored if specified.</param>
        /// <returns></returns>
        internal static async Task<List<Follow>> GetUserRelationshipAsync(Authentication authentication, string token, string to_id, string from_id, FollowsParameters parameters = null)
        {
            List<Follow> follows = await PagingUtil.GetAllPagesAsync<Follow, Follows, FollowsParameters>(GetUserRelationshipPageAsync, authentication, token, to_id, from_id, parameters);

            return follows;
        }

        /// <summary>
        /// Asynchronously sets the description of a user specified by the OAuth token.
        /// Required Scope: 'user:read'
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="user_oauth_token">The user's OAuth token used to determine which description to update.</param>
        /// <param name="description">The new description to set.</param>
        /// <returns></returns>
        internal static async Task<bool> SetUserDescriptionAsync(Authentication authentication, string user_oauth_token, string supplementary_token, string description)
        {
            RestRequest request = Request("users", Method.PUT, authentication, user_oauth_token, supplementary_token);
            request.AddQueryParameter("description", description);

            RestClient client = Client();
            IRestResponse<Users> response = await client.ExecuteTaskAsync<Users>(request);

            return response.StatusCode == HttpStatusCode.OK;
        }

        #endregion

        #region Rest Request

        /// <summary>
        /// Creates a new instance of a <see cref="RestRequest"/> which holds the information to request.
        /// </summary>        
        /// <param name="endpoint">The endpoint URL.</param>
        /// <param name="method">The HTTP method used when making the request.</param>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request when only either is provided. If both are being provided, this is assumed to be the OAuth token.</param>
        /// <param name="supplementary_token">The Client Id if both the OAuth token and Client Id are being provided.</param>
        /// <returns></returns>
        private static RestRequest Request(string endpoint, Method method, Authentication authentication, string token, string supplementary_token = "")
        {
            RestRequest request = new RestRequest(endpoint, method);
            switch (authentication)
            {
                case Authentication.Authorization:
                {
                    request.AddHeader("Authorization", "Bearer " + token);
                }
                break;

                case Authentication.Client_ID:
                {
                    request.AddHeader("Client-ID", token);
                }
                break;

                case Authentication.Both:
                {
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.AddHeader("Client-ID", supplementary_token);
                }
                break;
            }

            return request;
        }

        /// <summary>
        /// Creates a new instance of a <see cref="RestClient"/> to execute the rest request to the Twitch API.
        /// </summary>
        /// <returns></returns>
        private static RestClient Client()
        {
            RestClient client = new RestClient("https://api.twitch.tv/helix");
            client.AddHandler("application/json", new JsonDeserializer());
            client.AddHandler("application/xml", new JsonDeserializer());

            return client;
        }

		#endregion
    }
}