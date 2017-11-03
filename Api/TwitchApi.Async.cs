﻿// standard namespaces
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
    TwitchApi
    {
        #region Streams

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Stream>>
        GetStreamsPageAsync(string client_id, StreamsQueryParameters query_parameters = null)
        {
            IApiResponsePage<Stream> streams = await TwitchApiInternal.GetStreamsPageAsync(Authentication.Client_ID, string.Empty, client_id, query_parameters);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Stream>>
        GetStreamsAsync(string client_id, StreamsQueryParameters query_parameters = null)
        {
            IApiResponse<Stream> streams = await TwitchApiInternal.GetStreamsAsync(Authentication.Client_ID, string.Empty, client_id, query_parameters);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Metadata>>
        GetStreamsMetadataPageAsync(string client_id, StreamsQueryParameters query_parameters = null)
        {
            IApiResponsePage<Metadata> metadata = await TwitchApiInternal.GetStreamsMetadataPageAsync(Authentication.Client_ID, string.Empty, client_id, query_parameters);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Metadata>>
        GetStreamsMetadataAsync(string client_id, StreamsQueryParameters query_parameters = null)
        {
            IApiResponse<Metadata> metadata = await TwitchApiInternal.GetStreamsMetadataAsync(Authentication.Client_ID, string.Empty, client_id, query_parameters);

            return metadata;
        }

        #endregion

        #region Users

        /// <summary>
        /// Asynchronously gets the information of one or more users by their id or login.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">The users to look up either by id or by login.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<User>>
        GetUsersAsync(string client_id, IList<QueryParameter> query_parameters)
        {
            IApiResponse<User> users = await TwitchApiInternal.GetUsersAsync(Authentication.Client_ID, string.Empty, client_id, query_parameters);

            return users;
        }

        /// <summary>
        /// Asynchronously gets the information of one or more users by their id.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="ids">The id(s) of the user(s).</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<User>>
        GetUsersByIdAsync(string client_id, IList<string> ids)
        {
            IApiResponse<User> users = await TwitchApiInternal.GetUsersAsync(Authentication.Client_ID, string.Empty, client_id, "id", ids);

            return users;
        }

        /// <summary>
        /// Asynchronously gets the information of one or more users by their login.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="logins">The name(s) of the user(s).</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<User>>
        GetUsersByLoginAsync(string client_id, IList<string> logins)
        {
            IApiResponse<User> users = await TwitchApiInternal.GetUsersAsync(Authentication.Client_ID, string.Empty, client_id, "login", logins);

            return users;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Follow>>
        GetUserRelationshipAsync(string client_id, string from_id, string to_id)
        {
            IApiResponsePage<Follow> relationship = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Client_ID, string.Empty, client_id, from_id, to_id);

            return relationship;
        }

        /// <summary>
        /// Asynchronously checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponseValue<bool>>
        IsUserFollowingAsync(string client_id, string from_id, string to_id)
        {
            IApiResponseValue<bool> is_following = await TwitchApiInternal.IsUserFollowingAsync(Authentication.Client_ID, string.Empty, client_id, from_id, to_id);

            return is_following;
        }

        /// <summary>
        /// Asynchronously gets a single page of a user's following list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Follow>>
        GetUserFollowingPageAsync(string client_id, string from_id, FollowsQueryParameters query_parameters = null)
        {
            IApiResponsePage<Follow> following = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Client_ID, string.Empty, client_id, from_id, string.Empty, query_parameters);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a user's following list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Follow>>
        GetUserFollowingAsync(string client_id, string from_id)
        {
            IApiResponse<Follow> following = await TwitchApiInternal.GetUserRelationshipAsync(Authentication.Client_ID, string.Empty, client_id, from_id, string.Empty);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Follow>>
        GetUserFollowersPageAsync(string client_id, string to_id, FollowsQueryParameters query_parameters = null)
        {
            IApiResponsePage<Follow> followers = await TwitchApiInternal.GetUserRelationshipPageAsync(Authentication.Client_ID, string.Empty, client_id, string.Empty, to_id, query_parameters);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a user's followers list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Follow>>
        GetUserFollowersAsync(string client_id, string to_id)
        {
            IApiResponse<Follow> followers = await TwitchApiInternal.GetUserRelationshipAsync(Authentication.Client_ID, string.Empty, client_id, string.Empty, to_id);

            return followers;
        }

        #endregion

    }
}
