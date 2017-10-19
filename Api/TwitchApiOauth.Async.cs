// standard namespaces
using System.Collections.Generic;
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Api.Internal;
using TwitchNet.Enums.Utilities;
using TwitchNet.Interfaces.Api;
using TwitchNet.Models.Api.Streams;
using TwitchNet.Models.Api.Users;
using TwitchNet.Models.Paging;
using TwitchNet.Models.Paging.Streams;
using TwitchNet.Models.Paging.Users;

namespace TwitchNet.Api
{
    public static partial class
    TwitchApiOauth
    {
        #region Users

        /// <summary>
        /// <para>
        /// Asynchronously gets the information of one or more users by their id or login.
        /// If no <paramref name="query_parameters"/> are specified, the user is looked up by the OAuth token provided.
        /// </para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="query_parameters">The users to look up either by id or by login.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<Users>>
        GetUsersAsync(string oauth_token, params QueryParameter[] query_parameters)
        {
            ITwitchResponse<Users> users = await TwitchApiInternal.GetUsersAsync(Authentication.Authorization, oauth_token, query_parameters);

            return users;
        }

        /// <summary>
        /// <para>
        /// Asynchronously gets the information of one or more users by their id.
        /// If no <paramref name="ids"/> are specified, the user is looked up by if the OAuth token provided.
        /// </para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="ids">The id(s) of the user(s).</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<Users>>
        GetUsersByIdAsync(string oauth_token, params string[] ids)
        {
            ITwitchResponse<Users> users = await TwitchApiInternal.GetUsersAsync(Authentication.Authorization, oauth_token, "id", ids);

            return users;
        }

        /// <summary>
        /// <para>
        /// Asynchronously gets the information of one or more users by their login.
        /// If no <paramref name="logins"/> are specified, the user is looked up by if the OAuth token provided.
        /// </para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="logins">The name(s) of the user(s).</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<Users>>
        GetUsersByLoginAsync(string oauth_token, params string[] logins)
        {
            ITwitchResponse<Users> users = await TwitchApiInternal.GetUsersAsync(Authentication.Authorization, oauth_token, "login", logins);

            return users;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<Follows>>
        GetUserRelationshipAsync(string oauth_token, string from_id, string to_id)
        {
            ITwitchResponse<Follows> relationship = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Authorization, oauth_token, from_id, to_id);

            return relationship;
        }

        /// <summary>
        /// Asynchronously checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<bool>>
        IsUserFollowingAsync(string oauth_token, string from_id, string to_id)
        {
            ITwitchResponse<bool> is_following = await TwitchApiInternal.IsUserFollowingAsync(Authentication.Authorization, oauth_token, from_id, to_id);

            return is_following;
        }

        /// <summary>
        /// Asynchronously gets a single page of a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="query_parameters">
        /// A set of parameters to customize the requests.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<Follows>>
        GetUserFollowingPageAsync(string oauth_token, string from_id, FollowsQueryParameters query_parameters = null)
        {
            ITwitchResponse<Follows> following = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Authorization, oauth_token, from_id, string.Empty, query_parameters);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<IList<Follow>>>
        GetUserFollowingAsync(string oauth_token, string from_id)
        {
            ITwitchResponse<IList<Follow>> following = await TwitchApiInternal.GetUserRelationshipAsync(Authentication.Authorization, oauth_token, from_id, string.Empty);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="query_parameters">
        /// A set of parameters to customize the requests.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<Follows>>
        GetUserFollowersPageAsync(string oauth_token, string to_id, FollowsQueryParameters query_parameters = null)
        {
            ITwitchResponse<Follows> followers = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Authorization, oauth_token, string.Empty, to_id, query_parameters);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<IList<Follow>>>
        GetUserFollowersAsync(string oauth_token, string to_id)
        {
            ITwitchResponse<IList<Follow>> followers = await TwitchApiInternal.GetUserRelationshipAsync(Authentication.Authorization, oauth_token, string.Empty, to_id);

            return followers;
        }

        /// <summary>
        /// Asynchronously sets the description of a user specified by the OAuth token provided.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="description">The new description to set.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<bool>>
        SetUserDescriptionAsync(string oauth_token, string description)
        {
            ITwitchResponse<bool> success = await TwitchApiInternal.SetUserDescriptionAsync(Authentication.Authorization, oauth_token, string.Empty, description);

            return success;
        }

        /// <summary>
        /// Asynchronously sets the description of a user specified by the OAuth token provided.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update.</param>
        /// <param name="client_id">The Client Id of the application to authorize the request.</param>
        /// <param name="description">The new description to set.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<bool>>
        SetUserDescriptionAsync(string oauth_token, string client_id, string description)
        {
            ITwitchResponse<bool> success = await TwitchApiInternal.SetUserDescriptionAsync(Authentication.Both, oauth_token, client_id, description);

            return success;
        }

        #endregion

        #region Streams

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="query_parameters">A set of parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<Streams>>
        GetStreamsPageAsync(string oauth_token, StreamsQueryParameters query_parameters = null)
        {
            ITwitchResponse<Streams> streams = await TwitchApiInternal.GetStreamsPageAsync(Authentication.Authorization, oauth_token, query_parameters);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="query_parameters">Optional. A set of parameters to customize the requests.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<IList<Stream>>>
        GetStreamsAsync(string oauth_token, StreamsQueryParameters query_parameters = null)
        {
            ITwitchResponse<IList<Stream>> streams = await TwitchApiInternal.GetStreamsAsync(Authentication.Authorization, oauth_token, query_parameters);

            return streams;
        }

        #endregion
    }
}
