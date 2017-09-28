// standard namespaces
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

		internal static async Task<Follows> GetUserFollowsPageAsync(Authentication authentication, string token, FollowsParameters parameters)
        {
            RestRequest request = Request(authentication, token, "users/follows", Method.GET);
            PagingUtil.AddPaging(request, parameters);

            RestClient client = Client();
            IRestResponse<Follows> response = await client.ExecuteTaskAsync<Follows>(request);

            return response.Data;
        }

        internal static async Task<List<Follower>> GetUserFollowsAsync(Authentication authentication, string token, FollowsParameters parameters)
        {
            List<Follower> follows = await PagingUtil.GetAllPagesAsync<Follower, Follows, FollowsParameters>(GetUserFollowsPageAsync, authentication, token, parameters);

            return follows;
        }

        internal static async Task<Follows> GetUserRelationshipPageAsync(Authentication authentication, string token, string to_id, string from_id, FollowsParameters parameters = null)
        {
            if (parameters.isNull())
            {
                parameters = new FollowsParameters();
            }
            parameters.to_id = to_id;
            parameters.from_id = from_id;

            Follows relationship = await GetUserFollowsPageAsync(authentication, token, parameters);

            return relationship;
        }

        internal static async Task<List<Follower>> GetUserRelationshipAsync(Authentication authentication, string token, string to_id, string from_id, FollowsParameters parameters = null)
        {
            if (parameters.isNull())
            {
                parameters = new FollowsParameters();
            }
            parameters.to_id = to_id;
            parameters.from_id = from_id;

            List<Follower> relationship = await GetUserFollowsAsync(authentication, token, parameters);

            return relationship;
        }

        #endregion

        #region Rest Request

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
