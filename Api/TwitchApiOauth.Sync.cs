// standard namespaces
using System.Collections.Generic;

// project namespaces
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
        /// <para>Gets the information of the user looked up by the provided OAuth token.</para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<UserPage>
        GetUser(string oauth_token)
        {
            ITwitchResponse<UserPage> users = GetUserAsync(oauth_token).Result;

            return users;
        }

        /// <summary>
        /// <para>Gets the information of the user looked up by the provided OAuth token.</para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<UserPage>
        GetUser(string oauth_token, string client_id)
        {
            ITwitchResponse<UserPage> users = GetUserAsync(oauth_token, client_id).Result;

            return users;
        }

        /// <summary>
        /// <para>
        /// Gets the information of one or more users by their id or login.
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
        public static ITwitchResponse<UserPage>
        GetUsers(string oauth_token, IList<QueryParameter> query_parameters)
        {
            ITwitchResponse<UserPage> users = GetUsersAsync(oauth_token, query_parameters).Result;

            return users;
        }

        /// <summary>
        /// <para>
        /// Gets the information of one or more users by their id or login.
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
        public static ITwitchResponse<UserPage>
        GetUsers(string oauth_token, string client_id, IList<QueryParameter> query_parameters)
        {
            ITwitchResponse<UserPage> users = GetUsersAsync(oauth_token, client_id, query_parameters).Result;

            return users;
        }

        /// <summary>
        /// <para>
        /// Gets the information of one or more users by their id.
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
        public static ITwitchResponse<UserPage>
        GetUsersById(string oauth_token, IList<string> ids)
        {
            ITwitchResponse<UserPage> users = GetUsersByIdAsync(oauth_token, ids).Result;

            return users;
        }

        /// <summary>
        /// <para>
        /// Gets the information of one or more users by their id.
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
        public static ITwitchResponse<UserPage>
        GetUsersById(string oauth_token, string client_id, IList<string> ids)
        {
            ITwitchResponse<UserPage> users = GetUsersByIdAsync(oauth_token, client_id, ids).Result;

            return users;
        }

        /// <summary>
        /// <para>
        /// Gets the information of one or more users by their login.
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
        public static ITwitchResponse<UserPage>
        GetUsersByLogin(string oauth_token, IList<string> logins)
        {
            ITwitchResponse<UserPage> users = GetUsersByLoginAsync(oauth_token, logins).Result;

            return users;
        }

        /// <summary>
        /// <para>
        /// Gets the information of one or more users by their login.
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
        public static ITwitchResponse<UserPage>
        GetUsersByLogin(string oauth_token, string client_id, IList<string> logins)
        {
            ITwitchResponse<UserPage> users = GetUsersByLoginAsync(oauth_token, client_id, logins).Result;

            return users;
        }

        /// <summary>
        /// Gets the relationship between two users.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<FollowPage>
        GetUserRelationship(string oauth_token, string from_id, string to_id)
        {
            ITwitchResponse<FollowPage> relationship = GetUserRelationshipAsync(oauth_token, from_id, to_id).Result;

            return relationship;
        }

        /// <summary>
        /// Gets the relationship between two users.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<FollowPage>
        GetUserRelationship(string oauth_token, string client_id, string from_id, string to_id)
        {
            ITwitchResponse<FollowPage> relationship = GetUserRelationshipAsync(oauth_token, client_id, from_id, to_id).Result;

            return relationship;
        }

        /// <summary>
        /// Checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<bool>
        IsUserFollowing(string oauth_token, string from_id, string to_id)
        {
            ITwitchResponse<bool> is_following = IsUserFollowingAsync(oauth_token, from_id, to_id).Result;

            return is_following;
        }

        /// <summary>
        /// Checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<bool>
        IsUserFollowing(string oauth_token, string client_id, string from_id, string to_id)
        {
            ITwitchResponse<bool> is_following = IsUserFollowingAsync(oauth_token, client_id, from_id, to_id).Result;

            return is_following;
        }

        /// <summary>
        /// Gets a single page of a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<FollowPage>
        GetUserFollowingPage(string oauth_token, string from_id, FollowsQueryParameters query_parameters = null)
        {
            ITwitchResponse<FollowPage> following = GetUserFollowingPageAsync(oauth_token, from_id, query_parameters).Result;

            return following;
        }

        /// <summary>
        /// Gets a single page of a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<FollowPage>
        GetUserFollowingPage(string oauth_token, string client_id, string from_id, FollowsQueryParameters query_parameters = null)
        {
            ITwitchResponse<FollowPage> following = GetUserFollowingPageAsync(oauth_token, client_id, from_id, query_parameters).Result;

            return following;
        }

        /// <summary>
        /// Gets a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<IList<Follow>>
        GetUserFollowing(string oauth_token, string from_id)
        {
            ITwitchResponse<IList<Follow>> following = GetUserFollowingAsync(oauth_token, from_id).Result;

            return following;
        }

        /// <summary>
        /// Gets a user's following list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<IList<Follow>>
        GetUserFollowing(string oauth_token, string client_id, string from_id)
        {
            ITwitchResponse<IList<Follow>> following = GetUserFollowingAsync(oauth_token, client_id, from_id).Result;

            return following;
        }

        /// <summary>
        /// Gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<FollowPage>
        GetUserFollowersPage(string oauth_token, string to_id, FollowsQueryParameters query_parameters = null)
        {
            ITwitchResponse<FollowPage> followers = GetUserFollowersPageAsync(oauth_token, to_id, query_parameters).Result;

            return followers;
        }

        /// <summary>
        /// Gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<FollowPage>
        GetUserFollowersPage(string oauth_token, string client_id, string to_id, FollowsQueryParameters query_parameters = null)
        {
            ITwitchResponse<FollowPage> followers = GetUserFollowersPageAsync(oauth_token, client_id, to_id, query_parameters).Result;

            return followers;
        }

        /// <summary>
        /// Gets a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<IList<Follow>>
        GetUserFollowers(string oauth_token, string to_id)
        {
            ITwitchResponse<IList<Follow>> followers = GetUserFollowersAsync(oauth_token, to_id).Result;

            return followers;
        }

        /// <summary>
        /// Gets a user's followers list.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<IList<Follow>>
        GetUserFollowers(string oauth_token, string client_id, string to_id)
        {
            ITwitchResponse<IList<Follow>> followers = GetUserFollowersAsync(oauth_token, client_id, to_id).Result;

            return followers;
        }

        /// <summary>
        /// <para>Sets the description of a user specified by the OAuth token provided.</para>
        /// <para>Required Scope: 'user:edit'</para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="description">The new description to set.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<bool>
        SetUserDescription(string oauth_token, string description)
        {
            ITwitchResponse<bool> success = SetUserDescriptionAsync(oauth_token, description).Result;

            return success;
        }

        /// <summary>
        /// <para>Sets the description of a user specified by the OAuth token provided.</para>
        /// <para>Required Scope: 'user:edit'</para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="description">The new description to set.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<bool>
        SetUserDescription(string oauth_token, string client_id, string description)
        {
            ITwitchResponse<bool> success = SetUserDescriptionAsync(oauth_token, client_id, description).Result;

            return success;
        }

        #endregion

        #region Streams

        /// <summary>
        /// Gets a single page of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<StreamPage>
        GetStreamsPage(string oauth_token, StreamsQueryParameters query_parameters = null)
        {
            ITwitchResponse<StreamPage> streams = GetStreamsPageAsync(oauth_token, query_parameters).Result;

            return streams;
        }

        /// <summary>
        /// Gets a single page of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<StreamPage>
        GetStreamsPage(string oauth_token, string client_id, StreamsQueryParameters query_parameters = null)
        {
            ITwitchResponse<StreamPage> streams = GetStreamsPageAsync(oauth_token, client_id, query_parameters).Result;

            return streams;
        }

        /// <summary>
        /// Gets a complete list of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<IList<Stream>>
        GetStreams(string oauth_token, StreamsQueryParameters query_parameters = null)
        {
            ITwitchResponse<IList<Stream>> streams = GetStreamsAsync(oauth_token, query_parameters).Result;

            return streams;
        }

        /// <summary>
        /// Gets a complete list of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<IList<Stream>>
        GetStreams(string oauth_token, string client_id, StreamsQueryParameters query_parameters = null)
        {
            ITwitchResponse<IList<Stream>> streams = GetStreamsAsync(oauth_token, client_id, query_parameters).Result;

            return streams;
        }

        /// <summary>
        /// Gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<MetadataPage>
        GetStreamsMetadataPage(string oauth_token, StreamsQueryParameters query_parameters = null)
        {
            ITwitchResponse<MetadataPage> metadata = GetStreamsMetadataPageAsync(oauth_token, query_parameters).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<MetadataPage>
        GetStreamsMetadataPage(string oauth_token, string client_id, StreamsQueryParameters query_parameters = null)
        {
            ITwitchResponse<MetadataPage> metadata = GetStreamsMetadataPageAsync(oauth_token, client_id, query_parameters).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<IList<Metadata>>
        GetStreamsMetadata(string oauth_token, StreamsQueryParameters query_parameters = null)
        {
            ITwitchResponse<IList<Metadata>> metadata = GetStreamsMetadataAsync(oauth_token, query_parameters).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="ITwitchResponse{type}"/> interface.</returns>
        public static ITwitchResponse<IList<Metadata>>
        GetStreamsMetadat(string oauth_token, string client_id, StreamsQueryParameters query_parameters = null)
        {
            ITwitchResponse<IList<Metadata>> metadata = GetStreamsMetadataAsync(oauth_token, client_id, query_parameters).Result;

            return metadata;
        }

        #endregion
    }
}
