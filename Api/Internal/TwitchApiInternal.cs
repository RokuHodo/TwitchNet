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
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="query_parameters">The users to look up either by id or by login.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        internal static async Task<ITwitchResponse<UserPage>>
        GetUsersAsync(Authentication authentication, string oauth_token, string client_id, IList<QueryParameter> query_parameters)
        {
            ITwitchResponse<UserPage> users = await RestRequestUtil.ExecuteRequestAsync<UserPage>("users", Method.GET, authentication, oauth_token, client_id, query_parameters);

            return users;
        }

        /// <summary>
        /// <para>
        /// Asynchronously gets the information of one or more users by their id or login.
        /// If no <paramref name="lookup"/>s are specified, the user is looked up by the token provided if it is an OAuth token.
        /// </para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="query_name">How to get the user's information, by 'id' or by 'login'.</param>
        /// <param name="lookup">The id(s) or login(s) to get the information for.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        internal static async Task<ITwitchResponse<UserPage>>
        GetUsersAsync(Authentication authentication, string oauth_token, string client_id, string query_name, IList<string> lookup)
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

            ITwitchResponse<UserPage> users = await GetUsersAsync(authentication, oauth_token, client_id, query_parameters.ToArray());

            return users;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users, or a single page of the following/follower lists of one user.
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        internal static async Task<ITwitchResponse<FollowPage>>
        GetUserRelationshipPageAsync(Authentication authentication, string oauth_token, string client_id, string from_id, string to_id, FollowsQueryParameters query_parameters = null)
        {
            if (query_parameters.IsNull())
            {
                query_parameters = new FollowsQueryParameters();
            }
            query_parameters.from_id = from_id;
            query_parameters.to_id = to_id;

            ITwitchResponse<FollowPage> follows = await RestRequestUtil.ExecuteRequestPageAsync<FollowPage, Follow, FollowsQueryParameters>("users/follows", Method.GET, authentication, oauth_token, client_id, query_parameters);

            return follows;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users, or the complete following/follower lists of one user.
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="from_id">The user to compare from. Used to get the following list of a user.</param>
        /// <param name="to_id">The user to compare to. Used to get a user's follower list.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request. The 'to_id' and 'from_id' properties in the parameters are ignored if specified.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        internal static async Task<ITwitchResponse<IList<Follow>>>
        GetUserRelationshipAsync(Authentication authentication, string oauth_token, string client_id, string from_id, string to_id, FollowsQueryParameters query_parameters = null)
        {
            if (query_parameters.IsNull())
            {
                query_parameters = new FollowsQueryParameters();
            }            
            query_parameters.from_id = from_id;
            query_parameters.to_id = to_id;
            query_parameters.first = 100;

            ITwitchResponse<IList<Follow>> follows = await RestRequestUtil.ExecuteRequestAllPagesAsync<Follow, FollowPage, FollowsQueryParameters>("users/follows", Method.GET, authentication, oauth_token, client_id, query_parameters);

            return follows;
        }

        /// <summary>
        /// Asynchronously checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        internal static async Task<ITwitchResponse<bool>>
        IsUserFollowingAsync(Authentication authentication, string oauth_token, string client_id, string from_id, string to_id)
        {
            ITwitchResponse<FollowPage> relationship = await GetUserRelationshipPageAsync(authentication, oauth_token, client_id, from_id, to_id);

            TwitchResponse<bool> is_following = new TwitchResponse<bool>(relationship);
            is_following.result = relationship.result.data.IsValid();

            return is_following;
        }

        /// <summary>
        /// <para>Asynchronously sets the description of a user specified by the OAuth token.</para>
        /// <para>Required Scope: 'user:read'</para>
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="description">The new description to set.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        internal static async Task<ITwitchResponse<bool>>
        SetUserDescriptionAsync(Authentication authentication, string oauth_token, string client_id, string description)
        {
            QueryParameter[] query_parameters = new QueryParameter[]
            {
                new QueryParameter
                {
                    name = "description",
                    value = description
                },
            };

            ITwitchResponse<UserPage> users = await RestRequestUtil.ExecuteRequestAsync<UserPage>("users", Method.PUT, authentication, oauth_token, client_id, query_parameters);

            // TODO: SetUserDescriptionAsync - Test to see if this is a valid check to see if the description was actually updated.
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
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        internal static async Task<ITwitchResponse<StreamPage>>
        GetStreamsPageAsync(Authentication authentication, string oauth_token, string client_id, StreamsQueryParameters query_parameters = null)
        {
            ITwitchResponse<StreamPage> streams = await RestRequestUtil.ExecuteRequestPageAsync<StreamPage, Stream, StreamsQueryParameters>("streams", Method.GET, authentication, oauth_token, client_id, query_parameters);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        internal static async Task<ITwitchResponse<IList<Stream>>>
        GetStreamsAsync(Authentication authentication, string oauth_token, string client_id, StreamsQueryParameters query_parameters = null)
        {
            if (query_parameters.IsNull())
            {
                query_parameters = new StreamsQueryParameters();
            }
            query_parameters.first = 100;

            ITwitchResponse<IList<Stream>> streams = await RestRequestUtil.ExecuteRequestAllPagesAsync<Stream, StreamPage, StreamsQueryParameters>("streams", Method.GET, authentication, oauth_token, client_id, query_parameters);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        internal static async Task<ITwitchResponse<MetadataPage>>
        GetStreamsMetadataPageAsync(Authentication authentication, string oauth_token, string client_id, StreamsQueryParameters query_parameters = null)
        {
            ITwitchResponse<MetadataPage> metadata = await RestRequestUtil.ExecuteRequestPageAsync<MetadataPage, Metadata, StreamsQueryParameters>("streams/metadata", Method.GET, authentication, oauth_token, client_id, query_parameters);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        internal static async Task<ITwitchResponse<IList<Metadata>>>
        GetStreamsMetadataAsync(Authentication authentication, string oauth_token, string client_id, StreamsQueryParameters query_parameters = null)
        {
            ITwitchResponse<IList<Metadata>> metadata = await RestRequestUtil.ExecuteRequestAllPagesAsync<Metadata, MetadataPage, StreamsQueryParameters>("streams/metadata", Method.GET, authentication, oauth_token, client_id, query_parameters);

            return metadata;
        }

        #endregion
    }
}
