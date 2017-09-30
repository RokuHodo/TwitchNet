﻿// standard namespaces
using System.Collections.Generic;
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
        /// Asynchronously get gets a user's information by their id or login name.
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="query_name">How to get the user's information, by 'id' or by 'login'.</param>
        /// <param name="lookup">The id(s) or login(s) to get the information for.</param>
        /// <returns></returns>
        internal static async Task<Users> GetUsersAsync(Authentication authentication, string token, string query_name, params string[] lookup)
        {
            RestRequest request = Request(authentication, token, "users", Method.GET);
            foreach (string element in lookup)
            {
                request.AddQueryParameter(query_name, element);
            }

            RestClient client = Client();
            IRestResponse<Users> response = await client.ExecuteTaskAsync<Users>(request);

            return response.Data;
        }

        /// <summary>
        /// Asynchronously get's the relationship between two users, or a single page of the following/follower lists of one user.
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="to_id">Optional. The user to compare to. Used to get a user's follower list.</param>
        /// <param name="from_id">Optional. The user to compare from. Used to get the following list of a user.</param>
        /// <param name="parameters">Optional. A set of parameters to customize the requests. The 'to_id' and 'from_id' properties in the parameters are ignored if specified.</param>
        /// <returns></returns>
        internal static async Task<Follows> GetUserRelationshipPageAsync(Authentication authentication, string token, string to_id, string from_id, FollowsParameters parameters = null)
        {
            RestRequest request = Request(authentication, token, "users/follows", Method.GET);

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
        /// Asynchronously get's the relationship between two users, or the complete following/follower lists of one user.
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

        #endregion

        #region Rest Request

        /// <summary>
        /// Creates a new instance of a <see cref="RestRequest"/> which holds the information to request.
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="endpoint">The endpoint URL.</param>
        /// <param name="method">The HTTP method used when making the request.</param>
        /// <returns></returns>
        private static RestRequest Request(Authentication authentication, string token, string endpoint, Method method)
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
