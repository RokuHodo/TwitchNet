// standard namespaces
using System.Collections.Generic;
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Enums.Utilities;
using TwitchNet.Extensions;
using TwitchNet.Interfaces.Api;
using TwitchNet.Models.Api;
using TwitchNet.Models.Api.Streams;
using TwitchNet.Models.Api.Users;
using TwitchNet.Models.Paging;
using TwitchNet.Models.Paging.Streams;
using TwitchNet.Models.Paging.Users;
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
        /// <para>
        /// Asynchronously gets the information of one or more users by their id or login.
        /// If no <paramref name="query_parameters"/> are specified, the user is looked up by the token provided if it is an OAuth token.
        /// </para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="query_parameters">The users to look up either by id or by login.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        internal static async Task<ITwitchResponse<Users>>
        GetUsersAsync(Authentication authentication, string token, params QueryParameter[] query_parameters)
        {
            ITwitchResponse<Users> users = await RestRequestUtil.ExecuteRequestAsync<Users>("users", Method.GET, authentication, token, query_parameters);

            return users;
        }

        /// <summary>
        /// <para>
        /// Asynchronously gets the information of one or more users by their id or login.
        /// If no <paramref name="query_parameters"/> are specified, the user is looked up by the token provided if it is an OAuth token.
        /// </para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="query_name">How to get the user's information, by 'id' or by 'login'.</param>
        /// <param name="lookup">The id(s) or login(s) to get the information for.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        internal static async Task<ITwitchResponse<Users>>
        GetUsersAsync(Authentication authentication, string token, string query_name, params string[] lookup)
        {
            List<QueryParameter> query_parameters = new List<QueryParameter>();
            foreach (string query_value in lookup)
            {
                query_parameters.Add(new QueryParameter()
                {
                    name = query_name,
                    value = query_value
                });
            }

            ITwitchResponse<Users> users = await GetUsersAsync(authentication, token, query_parameters.ToArray());

            return users;
        }

        /// <summary>
        /// Asynchronously checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        internal static async Task<ITwitchResponse<bool>>
        IsUserFollowingAsync(Authentication authentication, string token, string from_id, string to_id)
        {
            ITwitchResponse<Follows> relationship = await GetUserRelationshipPageAsync(authentication, token, from_id, to_id);

            TwitchResponse<bool> is_following = new TwitchResponse<bool>(relationship);
            is_following.result = relationship.result.data.IsValid();

            return is_following;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users, or a single page of the following/follower lists of one user.
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="query_parameters">
        /// A set of parameters to customize the requests.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        internal static async Task<ITwitchResponse<Follows>>
        GetUserRelationshipPageAsync(Authentication authentication, string token, string from_id, string to_id, FollowsQueryParameters query_parameters = null)
        {
            if (query_parameters.IsNull())
            {
                query_parameters = new FollowsQueryParameters();
            }
            query_parameters.from_id = from_id;
            query_parameters.to_id = to_id;

            ITwitchResponse<Follows> follows = await RestRequestUtil.ExecuteRequestPageAsync<Follows, Follow, FollowsQueryParameters>("users/follows", Method.GET, authentication, token, query_parameters);

            return follows;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users, or the complete following/follower lists of one user.
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="to_id">Optional. The user to compare to. Used to get a user's follower list.</param>
        /// <param name="from_id">Optional. The user to compare from. Used to get the following list of a user.</param>
        /// <param name="parameters">Optional. A set of parameters to customize the requests. The 'to_id' and 'from_id' properties in the parameters are ignored if specified.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        internal static async Task<ITwitchResponse<IList<Follow>>>
        GetUserRelationshipAsync(Authentication authentication, string token, string from_id, string to_id, FollowsQueryParameters parameters = null)
        {
            if (parameters.IsNull())
            {
                parameters = new FollowsQueryParameters();
            }            
            parameters.from_id = from_id;
            parameters.to_id = to_id;
            parameters.first = 100;

            ITwitchResponse<IList<Follow>> follows = await RestRequestUtil.ExecuteRequestAllPagesAsync<Follow, Follows, FollowsQueryParameters>("users/follows", Method.GET, authentication, token, parameters);

            return follows;
        }

        /// <summary>
        /// <para>Asynchronously sets the description of a user specified by the OAuth token.</para>
        /// <para>Required Scope: 'user:read'</para>
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request if no <paramref name="client_id"/> is provided.</param>
        /// <param name="client_id">The Client Id of the application to validate the request.</param>
        /// <param name="description">The new description to set.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        internal static async Task<ITwitchResponse<bool>>
        SetUserDescriptionAsync(Authentication authentication, string oauth_token, string client_id, string description)
        {
            QueryParameter query_parameter = new QueryParameter("description", description);

            // TODO: SetUserDescriptionAsync - Test to see if providing an oauth token and a client id is even worth doing.
            // Providing an client id and an oauth token created using the same client id does nothing. The client if is essnetially ignored in the request.
            // What about providing a client id and an oauth token that was made using a different client id?
            ITwitchResponse<Users> users = await RestRequestUtil.ExecuteRequestAsync<Users>("users", Method.PUT, oauth_token, client_id, query_parameter);

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
        /// <param name="parameters">A set of parameters to customize the requests.</param>
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
        /// <param name="parameters">A set of parameters to customize the requests.</param>
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
