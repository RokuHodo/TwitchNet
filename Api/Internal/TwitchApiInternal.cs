// standard namespaces
using System.Collections.Generic;
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Enums.Utilities;
using TwitchNet.Extensions;
using TwitchNet.Helpers.Paging;
using TwitchNet.Helpers.Paging.Streams;
using TwitchNet.Helpers.Paging.Users;
using TwitchNet.Interfaces.Api;
using TwitchNet.Models.Api;
using TwitchNet.Models.Api.Streams;
using TwitchNet.Models.Api.Users;
using TwitchNet.Utilities;

// imported .dll's
using RestSharp;

namespace TwitchNet.Api.Internal
{
    //TODO: Implement Exceptions wherever a user can wrongly pass through data
    //TODO: Re-test all end points to make sure nothing is broken after all RestRequestUtil changes are implemented
    internal static partial class
    TwitchApiInternal
    {
        #region Users

        /// <summary>
        /// <para>Asynchronously gets a user's information by their id or login name.</para>
        /// <para>Optional Scope: 'user:read:email'</para>
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="query_name">How to get the user's information, by 'id' or by 'login'.</param>
        /// <param name="lookup">The id(s) or login(s) to get the information for.</param>
        /// <returns></returns>
        internal static async Task<ITwitchResponse<Users>>
        GetUsersAsync(Authentication authentication, string token, string query_name, params string[] lookup)
        {
            List<QueryParameter> query_parameters = new List<QueryParameter>();

            foreach (string query_value in lookup)
            {
                QueryParameter query_parameter = new QueryParameter(query_name, query_value);
                query_parameters.Add(new QueryParameter()
                {
                    name = query_name,
                    value = query_value
                });
            }

            ITwitchResponse<Users> users = await RestRequestUtil.ExecuteRequestAsync<Users>("users", Method.GET, authentication, token, query_parameters.ToArray());

            return users;
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
        internal static async Task<ITwitchResponse<Follows>>
        GetUserRelationshipPageAsync(Authentication authentication, string token, string to_id, string from_id, FollowsQueryParameters parameters = null)
        {
            if (parameters.IsNull())
            {
                parameters = new FollowsQueryParameters();
            }
            parameters.to_id = to_id;
            parameters.from_id = from_id;

            ITwitchResponse<Follows> follows = await RestRequestUtil.ExecuteRequestPageAsync<Follows, Follow, FollowsQueryParameters>("users/follows", Method.GET, authentication, token, parameters);

            return follows;
        }

        internal static async Task<ITwitchResponse<bool>>
        IsUserFollowingAsync(Authentication authentication, string token, string to_id, string from_id)
        {
            ITwitchResponse<Follows> relationship = await GetUserRelationshipPageAsync(authentication, token, to_id, from_id);

            TwitchResponse<bool> is_following = new TwitchResponse<bool>(relationship);
            is_following.result = relationship.result.data.IsValid();

            return is_following;
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
        internal static async Task<ITwitchResponse<IList<Follow>>>
        GetUserRelationshipAsync(Authentication authentication, string token, string to_id, string from_id, FollowsQueryParameters parameters = null)
        {
            if (parameters.IsNull())
            {
                parameters = new FollowsQueryParameters();
            }            
            parameters.to_id = to_id;
            parameters.from_id = from_id;
            parameters.first = 100;

            ITwitchResponse<IList<Follow>> follows = await RestRequestUtil.ExecuteRequestAllPagesAsync<Follow, Follows, FollowsQueryParameters>("users/follows", Method.GET, authentication, token, parameters);

            return follows;
        }

        /// <summary>
        /// <para>Asynchronously sets the description of a user specified by the OAuth token.</para>
        /// <para>Required Scope: 'user:read'</para>
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="user_oauth_token">The user's OAuth token used to determine which description to update.</param>
        /// <param name="description">The new description to set.</param>
        /// <returns></returns>
        internal static async Task<ITwitchResponse<bool>>
        SetUserDescriptionAsync(Authentication authentication, string user_oauth_token, string client_id, string description)
        {
            QueryParameter query_parameter = new QueryParameter("description", description);

            ITwitchResponse<Users> users = await RestRequestUtil.ExecuteRequestAsync<Users>("users", Method.PUT, user_oauth_token, client_id, query_parameter);

            TwitchResponse<bool> response = new TwitchResponse<bool>(users);
            response.result = users.result.data.IsValid();

            return response;
        }

        #endregion

        #region Streams

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="parameters">Optional. A set of parameters to customize the requests.</param>
        /// <returns></returns>
        internal static async Task<ITwitchResponse<Streams>>
        GetStreamsPageAsync(Authentication authentication, string token, StreamsQueryParameters parameters = null)
        {
            ITwitchResponse<Streams> streams = await RestRequestUtil.ExecuteRequestPageAsync<Streams, Stream, StreamsQueryParameters>("streams", Method.GET, authentication, token, parameters);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="parameters">Optional. A set of parameters to customize the requests.</param>
        /// <returns></returns>
        internal static async Task<ITwitchResponse<IList<Stream>>>
        GetStreamsAsync(Authentication authentication, string token, StreamsQueryParameters parameters = null)
        {
            if (parameters.IsNull())
            {
                parameters = new StreamsQueryParameters();
            }
            parameters.first = 100;

            ITwitchResponse<IList<Stream>> streams = await RestRequestUtil.ExecuteRequestAllPagesAsync<Stream, Streams, StreamsQueryParameters>("streams", Method.GET, authentication, token, parameters);

            return streams;
        }

        #endregion

        #region Rest Request

        

		#endregion
    }
}
