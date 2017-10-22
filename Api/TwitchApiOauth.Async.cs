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
        /// <para>Asynchronously gets the information of the user looked up by the provided OAuth token.</para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<UserPage>>
        GetUserAsync(string oauth_token)
        {
            ITwitchResponse<UserPage> users = await GetUsersAsync(oauth_token, null);

            return users;
        }

        /// <summary>
        /// <para>Asynchronously gets the information of the user looked up by the provided OAuth token.</para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<UserPage>>
        GetUserAsync(string oauth_token, string client_id)
        {
            ITwitchResponse<UserPage> users = await GetUsersAsync(oauth_token, client_id, null);

            return users;
        }

        /// <summary>
        /// <para>
        /// Asynchronously gets the information of one or more users by their id or login.
        /// If no <paramref name="ids"/> are specified, the user is looked up by if the OAuth token provided.
        /// </para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="query_parameters">The users to look up either by id or by login.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<UserPage>>
        GetUsersAsync(string oauth_token, IList<QueryParameter> query_parameters)
        {
            ITwitchResponse<UserPage> users = await TwitchApiInternal.GetUsersAsync(Authentication.Authorization, oauth_token, string.Empty, query_parameters);

            return users;
        }

        /// <summary>
        /// <para>
        /// Asynchronously gets the information of one or more users by their id or login.
        /// If no <paramref name="ids"/> are specified, the user is looked up by if the OAuth token provided.
        /// </para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">The users to look up either by id or by login.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<UserPage>>
        GetUsersAsync(string oauth_token, string client_id, IList<QueryParameter> query_parameters)
        {
            ITwitchResponse<UserPage> users = await TwitchApiInternal.GetUsersAsync(Authentication.Both, oauth_token, client_id, query_parameters);

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
        public static async Task<ITwitchResponse<UserPage>>
        GetUsersByIdAsync(string oauth_token, IList<string> ids)
        {
            ITwitchResponse<UserPage> users = await TwitchApiInternal.GetUsersAsync(Authentication.Authorization, oauth_token, string.Empty, "id", ids);

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
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="ids">The id(s) of the user(s).</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<UserPage>>
        GetUsersByIdAsync(string oauth_token, string client_id, IList<string> ids)
        {
            ITwitchResponse<UserPage> users = await TwitchApiInternal.GetUsersAsync(Authentication.Both, oauth_token, client_id, "id", ids);

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
        public static async Task<ITwitchResponse<UserPage>>
        GetUsersByLoginAsync(string oauth_token, IList<string> logins)
        {
            ITwitchResponse<UserPage> users = await TwitchApiInternal.GetUsersAsync(Authentication.Authorization, oauth_token, string.Empty, "login", logins);

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
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="logins">The name(s) of the user(s).</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<UserPage>>
        GetUsersByLoginAsync(string oauth_token, string client_id, IList<string> logins)
        {
            ITwitchResponse<UserPage> users = await TwitchApiInternal.GetUsersAsync(Authentication.Both, oauth_token, client_id, "login", logins);

            return users;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<FollowPage>>
        GetUserRelationshipAsync(string oauth_token, string from_id, string to_id)
        {
            ITwitchResponse<FollowPage> relationship = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Authorization, oauth_token, string.Empty, from_id, to_id);

            return relationship;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<FollowPage>>
        GetUserRelationshipAsync(string oauth_token, string client_id, string from_id, string to_id)
        {
            ITwitchResponse<FollowPage> relationship = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Both, oauth_token, client_id, from_id, to_id);

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
            ITwitchResponse<bool> is_following = await TwitchApiInternal.IsUserFollowingAsync(Authentication.Authorization, oauth_token, string.Empty, from_id, to_id);

            return is_following;
        }

        /// <summary>
        /// Asynchronously checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<bool>>
        IsUserFollowingAsync(string oauth_token, string client_id, string from_id, string to_id)
        {
            ITwitchResponse<bool> is_following = await TwitchApiInternal.IsUserFollowingAsync(Authentication.Both, oauth_token, client_id, from_id, to_id);

            return is_following;
        }

        /// <summary>
        /// Asynchronously gets a single page of a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<FollowPage>>
        GetUserFollowingPageAsync(string oauth_token, string from_id, FollowsQueryParameters query_parameters = null)
        {
            ITwitchResponse<FollowPage> following = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Authorization, oauth_token, string.Empty, from_id, string.Empty, query_parameters);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a single page of a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<FollowPage>>
        GetUserFollowingPageAsync(string oauth_token, string client_id, string from_id, FollowsQueryParameters query_parameters = null)
        {
            ITwitchResponse<FollowPage> following = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Both, oauth_token, client_id, from_id, string.Empty, query_parameters);

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
            ITwitchResponse<IList<Follow>> following = await TwitchApiInternal.GetUserRelationshipAsync(Authentication.Authorization, oauth_token, string.Empty, from_id, string.Empty);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<IList<Follow>>>
        GetUserFollowingAsync(string oauth_token, string client_id, string from_id)
        {
            ITwitchResponse<IList<Follow>> following = await TwitchApiInternal.GetUserRelationshipAsync(Authentication.Both, oauth_token, client_id, from_id, string.Empty);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<FollowPage>>
        GetUserFollowersPageAsync(string oauth_token, string to_id, FollowsQueryParameters query_parameters = null)
        {
            ITwitchResponse<FollowPage> followers = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Authorization, oauth_token, string.Empty, string.Empty, to_id, query_parameters);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<FollowPage>>
        GetUserFollowersPageAsync(string oauth_token, string client_id, string to_id, FollowsQueryParameters query_parameters = null)
        {
            ITwitchResponse<FollowPage> followers = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Both, oauth_token, client_id, string.Empty, to_id, query_parameters);

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
            ITwitchResponse<IList<Follow>> followers = await TwitchApiInternal.GetUserRelationshipAsync(Authentication.Authorization, oauth_token, string.Empty, string.Empty, to_id);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<IList<Follow>>>
        GetUserFollowersAsync(string oauth_token, string client_id, string to_id)
        {
            ITwitchResponse<IList<Follow>> followers = await TwitchApiInternal.GetUserRelationshipAsync(Authentication.Both, oauth_token, client_id, string.Empty, to_id);

            return followers;
        }

        /// <summary>
        /// <para>synchronously sets the description of a user specified by the OAuth token provided.</para>
        /// <para>Required Scope: 'user:edit'</para>
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
        /// <para>synchronously sets the description of a user specified by the OAuth token provided.</para>
        /// <para>Required Scope: 'user:edit'</para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
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
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<StreamPage>>
        GetStreamsPageAsync(string oauth_token, StreamsQueryParameters query_parameters = null)
        {
            ITwitchResponse<StreamPage> streams = await TwitchApiInternal.GetStreamsPageAsync(Authentication.Authorization, oauth_token, string.Empty, query_parameters);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<StreamPage>>
        GetStreamsPageAsync(string oauth_token,string client_id, StreamsQueryParameters query_parameters = null)
        {
            ITwitchResponse<StreamPage> streams = await TwitchApiInternal.GetStreamsPageAsync(Authentication.Both, oauth_token, client_id, query_parameters);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<IList<Stream>>>
        GetStreamsAsync(string oauth_token, StreamsQueryParameters query_parameters = null)
        {
            ITwitchResponse<IList<Stream>> streams = await TwitchApiInternal.GetStreamsAsync(Authentication.Authorization, oauth_token, string.Empty, query_parameters);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<IList<Stream>>>
        GetStreamsAsync(string oauth_token, string client_id, StreamsQueryParameters query_parameters = null)
        {
            ITwitchResponse<IList<Stream>> streams = await TwitchApiInternal.GetStreamsAsync(Authentication.Both, oauth_token, client_id, query_parameters);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<MetadataPage>>
        GetStreamsMetadataPageAsync(string oauth_token, StreamsQueryParameters query_parameters = null)
        {
            ITwitchResponse<MetadataPage> metadata = await TwitchApiInternal.GetStreamsMetadataPageAsync(Authentication.Authorization, oauth_token, string.Empty, query_parameters);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<MetadataPage>>
        GetStreamsMetadataPageAsync(string oauth_token, string client_id, StreamsQueryParameters query_parameters = null)
        {
            ITwitchResponse<MetadataPage> metadata = await TwitchApiInternal.GetStreamsMetadataPageAsync(Authentication.Both, oauth_token, client_id, query_parameters);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<IList<Metadata>>>
        GetStreamsMetadataAsync(string oauth_token, StreamsQueryParameters query_parameters = null)
        {
            ITwitchResponse<IList<Metadata>> metadata = await TwitchApiInternal.GetStreamsMetadataAsync(Authentication.Authorization, oauth_token, string.Empty, query_parameters);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static async Task<ITwitchResponse<IList<Metadata>>>
        GetStreamsMetadataAsync(string oauth_token, string client_id, StreamsQueryParameters query_parameters = null)
        {
            ITwitchResponse<IList<Metadata>> metadata = await TwitchApiInternal.GetStreamsMetadataAsync(Authentication.Both, oauth_token, client_id, query_parameters);

            return metadata;
        }

        #endregion
    }
}
