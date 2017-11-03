// standard namespaces
using System.Collections.Generic;
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Api.Internal;
using TwitchNet.Enums.Utilities;
using TwitchNet.Interfaces.Api;
using TwitchNet.Models.Api.Entitlement;
using TwitchNet.Models.Api.Streams;
using TwitchNet.Models.Api.Users;
using TwitchNet.Models.Paging;
using TwitchNet.Models.Paging.Entitlement;
using TwitchNet.Models.Paging.Streams;
using TwitchNet.Models.Paging.Users;

namespace TwitchNet.Api
{
    public static partial class
    TwitchApiOauth
    {
        #region Entitlements

        /// <summary>
        /// Asynchronously creates a URL where you can upload a manifest file and notify users that they have an entitlement.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Url>>
        CreateEntitlementGrantsUploadUrlAsync(string oauth_token, EntitlementQueryParameters query_parameters)
        {
            IApiResponse<Url> streams = await TwitchApiInternal.CreateEntitlementGrantsUploadUrlAsync(Authentication.Authorization, oauth_token, string.Empty, query_parameters);

            return streams;
        }

        /// <summary>
        /// Asynchronously creates a URL where you can upload a manifest file and notify users that they have an entitlement.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Url>>
        CreateEntitlementGrantsUploadUrlAsync(string oauth_token, string client_id, EntitlementQueryParameters query_parameters)
        {
            IApiResponse<Url> streams = await TwitchApiInternal.CreateEntitlementGrantsUploadUrlAsync(Authentication.Both, oauth_token, client_id, query_parameters);

            return streams;
        }

        #endregion

        #region Streams

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Stream>>
        GetStreamsPageAsync(string oauth_token, StreamsQueryParameters query_parameters = null)
        {
            IApiResponsePage<Stream> streams = await TwitchApiInternal.GetStreamsPageAsync(Authentication.Authorization, oauth_token, string.Empty, query_parameters);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Stream>>
        GetStreamsPageAsync(string oauth_token,string client_id, StreamsQueryParameters query_parameters = null)
        {
            IApiResponsePage<Stream> streams = await TwitchApiInternal.GetStreamsPageAsync(Authentication.Both, oauth_token, client_id, query_parameters);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Stream>>
        GetStreamsAsync(string oauth_token, StreamsQueryParameters query_parameters = null)
        {
            IApiResponse<Stream> streams = await TwitchApiInternal.GetStreamsAsync(Authentication.Authorization, oauth_token, string.Empty, query_parameters);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Stream>>
        GetStreamsAsync(string oauth_token, string client_id, StreamsQueryParameters query_parameters = null)
        {
            IApiResponse<Stream> streams = await TwitchApiInternal.GetStreamsAsync(Authentication.Both, oauth_token, client_id, query_parameters);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Metadata>>
        GetStreamsMetadataPageAsync(string oauth_token, StreamsQueryParameters query_parameters = null)
        {
            IApiResponsePage<Metadata> metadata = await TwitchApiInternal.GetStreamsMetadataPageAsync(Authentication.Authorization, oauth_token, string.Empty, query_parameters);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Metadata>>
        GetStreamsMetadataPageAsync(string oauth_token, string client_id, StreamsQueryParameters query_parameters = null)
        {
            IApiResponsePage<Metadata> metadata = await TwitchApiInternal.GetStreamsMetadataPageAsync(Authentication.Both, oauth_token, client_id, query_parameters);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Metadata>>
        GetStreamsMetadataAsync(string oauth_token, StreamsQueryParameters query_parameters = null)
        {
            IApiResponse<Metadata> metadata = await TwitchApiInternal.GetStreamsMetadataAsync(Authentication.Authorization, oauth_token, string.Empty, query_parameters);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Metadata>>
        GetStreamsMetadataAsync(string oauth_token, string client_id, StreamsQueryParameters query_parameters = null)
        {
            IApiResponse<Metadata> metadata = await TwitchApiInternal.GetStreamsMetadataAsync(Authentication.Both, oauth_token, client_id, query_parameters);

            return metadata;
        }

        #endregion

        #region Users

        /// <summary>
        /// <para>Asynchronously gets the information of the user looked up by the provided OAuth token.</para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<User>>
        GetUserAsync(string oauth_token)
        {
            IApiResponse<User> users = await GetUsersAsync(oauth_token, null);

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
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<User>>
        GetUserAsync(string oauth_token, string client_id)
        {
            IApiResponse<User> users = await GetUsersAsync(oauth_token, client_id, null);

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
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<User>>
        GetUsersAsync(string oauth_token, IList<QueryParameter> query_parameters)
        {
            IApiResponse<User> users = await TwitchApiInternal.GetUsersAsync(Authentication.Authorization, oauth_token, string.Empty, query_parameters);

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
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<User>>
        GetUsersAsync(string oauth_token, string client_id, IList<QueryParameter> query_parameters)
        {
            IApiResponse<User> users = await TwitchApiInternal.GetUsersAsync(Authentication.Both, oauth_token, client_id, query_parameters);

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
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<User>>
        GetUsersByIdAsync(string oauth_token, IList<string> ids)
        {
            IApiResponse<User> users = await TwitchApiInternal.GetUsersAsync(Authentication.Authorization, oauth_token, string.Empty, "id", ids);

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
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<User>>
        GetUsersByIdAsync(string oauth_token, string client_id, IList<string> ids)
        {
            IApiResponse<User> users = await TwitchApiInternal.GetUsersAsync(Authentication.Both, oauth_token, client_id, "id", ids);

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
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<User>>
        GetUsersByLoginAsync(string oauth_token, IList<string> logins)
        {
            IApiResponse<User> users = await TwitchApiInternal.GetUsersAsync(Authentication.Authorization, oauth_token, string.Empty, "login", logins);

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
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<User>>
        GetUsersByLoginAsync(string oauth_token, string client_id, IList<string> logins)
        {
            IApiResponse<User> users = await TwitchApiInternal.GetUsersAsync(Authentication.Both, oauth_token, client_id, "login", logins);

            return users;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Follow>>
        GetUserRelationshipAsync(string oauth_token, string from_id, string to_id)
        {
            IApiResponsePage<Follow> relationship = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Authorization, oauth_token, string.Empty, from_id, to_id);

            return relationship;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Follow>>
        GetUserRelationshipAsync(string oauth_token, string client_id, string from_id, string to_id)
        {
            IApiResponsePage<Follow> relationship = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Both, oauth_token, client_id, from_id, to_id);

            return relationship;
        }

        /// <summary>
        /// Asynchronously checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponseValue<bool>>
        IsUserFollowingAsync(string oauth_token, string from_id, string to_id)
        {
            IApiResponseValue<bool> is_following = await TwitchApiInternal.IsUserFollowingAsync(Authentication.Authorization, oauth_token, string.Empty, from_id, to_id);

            return is_following;
        }

        /// <summary>
        /// Asynchronously checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponseValue<bool>>
        IsUserFollowingAsync(string oauth_token, string client_id, string from_id, string to_id)
        {
            IApiResponseValue<bool> is_following = await TwitchApiInternal.IsUserFollowingAsync(Authentication.Both, oauth_token, client_id, from_id, to_id);

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
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Follow>>
        GetUserFollowingPageAsync(string oauth_token, string from_id, FollowsQueryParameters query_parameters = null)
        {
            IApiResponsePage<Follow> following = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Authorization, oauth_token, string.Empty, from_id, string.Empty, query_parameters);

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
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Follow>>
        GetUserFollowingPageAsync(string oauth_token, string client_id, string from_id, FollowsQueryParameters query_parameters = null)
        {
            IApiResponsePage<Follow> following = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Both, oauth_token, client_id, from_id, string.Empty, query_parameters);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Follow>>
        GetUserFollowingAsync(string oauth_token, string from_id)
        {
            IApiResponse<Follow> following = await TwitchApiInternal.GetUserRelationshipAsync(Authentication.Authorization, oauth_token, string.Empty, from_id, string.Empty);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Follow>>
        GetUserFollowingAsync(string oauth_token, string client_id, string from_id)
        {
            IApiResponse<Follow> following = await TwitchApiInternal.GetUserRelationshipAsync(Authentication.Both, oauth_token, client_id, from_id, string.Empty);

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
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Follow>>
        GetUserFollowersPageAsync(string oauth_token, string to_id, FollowsQueryParameters query_parameters = null)
        {
            IApiResponsePage<Follow> followers = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Authorization, oauth_token, string.Empty, string.Empty, to_id, query_parameters);

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
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Follow>>
        GetUserFollowersPageAsync(string oauth_token, string client_id, string to_id, FollowsQueryParameters query_parameters = null)
        {
            IApiResponsePage<Follow> followers = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Both, oauth_token, client_id, string.Empty, to_id, query_parameters);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Follow>>
        GetUserFollowersAsync(string oauth_token, string to_id)
        {
            IApiResponse<Follow> followers = await TwitchApiInternal.GetUserRelationshipAsync(Authentication.Authorization, oauth_token, string.Empty, string.Empty, to_id);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Follow>>
        GetUserFollowersAsync(string oauth_token, string client_id, string to_id)
        {
            IApiResponse<Follow> followers = await TwitchApiInternal.GetUserRelationshipAsync(Authentication.Both, oauth_token, client_id, string.Empty, to_id);

            return followers;
        }

        /// <summary>
        /// <para>synchronously sets the description of a user specified by the OAuth token provided.</para>
        /// <para>Required Scope: 'user:edit'</para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="description">The new description to set.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponseValue<bool>>
        SetUserDescriptionAsync(string oauth_token, string description)
        {
            IApiResponseValue<bool> success = await TwitchApiInternal.SetUserDescriptionAsync(Authentication.Authorization, oauth_token, string.Empty, description);

            return success;
        }

        /// <summary>
        /// <para>synchronously sets the description of a user specified by the OAuth token provided.</para>
        /// <para>Required Scope: 'user:edit'</para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="description">The new description to set.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponseValue<bool>>
        SetUserDescriptionAsync(string oauth_token, string client_id, string description)
        {
            IApiResponseValue<bool> success = await TwitchApiInternal.SetUserDescriptionAsync(Authentication.Both, oauth_token, client_id, description);

            return success;
        }

        #endregion

    }
}
