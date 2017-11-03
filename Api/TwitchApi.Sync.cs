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
    TwitchApi
    {
        #region Streams

        /// <summary>
        /// Gets a single page of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponsePage<Stream>
        GetStreamsPage(string client_id, StreamsQueryParameters query_parameters = null)
        {
            IApiResponsePage<Stream> streams = GetStreamsPageAsync(client_id, query_parameters).Result;

            return streams;
        }

        /// <summary>
        /// Gets a complete list of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Stream>
        GetStreams(string client_id, StreamsQueryParameters query_parameters = null)
        {
            IApiResponse<Stream> streams = GetStreamsAsync(client_id, query_parameters).Result;

            return streams;
        }

        /// <summary>
        /// Gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponsePage<Metadata>
        GetStreamsMetadataPage(string client_id, StreamsQueryParameters query_parameters = null)
        {
            IApiResponsePage<Metadata> metadata = GetStreamsMetadataPageAsync(client_id, query_parameters).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Metadata>
        GetStreamsMetadata(string client_id, StreamsQueryParameters query_parameters = null)
        {
            IApiResponse<Metadata> metadata = GetStreamsMetadataAsync(client_id, query_parameters).Result;

            return metadata;
        }

        #endregion

        #region Users

        /// <summary>
        /// Gets the information of one or more users by their id or login.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">The users to look up either by id or by login.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<User>
        GetUsers(string client_id, IList<QueryParameter> query_parameters)
        {
            IApiResponse<User> users = GetUsersAsync(client_id, query_parameters).Result;

            return users;
        }

        /// <summary>
        /// Gets the information of one or more users by their id.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="ids">The id(s) of the user(s).</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<User>
        GetUsersById(string client_id, IList<string> ids)
        {
            IApiResponse<User> users = GetUsersByIdAsync(client_id, ids).Result;

            return users;
        }

        /// <summary>
        /// Gets the information of one or more users by their login.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="logins">The name(s) of the user(s).</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<User>
        GetUsersByLogin(string client_id, IList<string> logins)
        {
            IApiResponse<User> users = GetUsersByLoginAsync(client_id, logins).Result;

            return users;
        }

        /// <summary>
        /// Gets the relationship between two users.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponsePage<Follow>
        GetUserRelationship(string client_id, string from_id, string to_id)
        {
            IApiResponsePage<Follow> relationship = GetUserRelationshipAsync(client_id, from_id, to_id).Result;

            return relationship;
        }

        /// <summary>
        /// Checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponseValue<bool>
        IsUserFollowing(string client_id, string from_id, string to_id)
        {
            IApiResponseValue<bool> is_following = IsUserFollowingAsync(client_id, from_id, to_id).Result;

            return is_following;
        }

        /// <summary>
        /// Gets a single page of a user's following list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponsePage<Follow>
        GetUserFollowingPage(string client_id, string from_id, FollowsQueryParameters query_parameters = null)
        {
            IApiResponsePage<Follow> following = GetUserFollowingPageAsync(client_id, from_id, query_parameters).Result;

            return following;
        }

        /// <summary>
        /// Gets a user's following list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Follow>
        GetUserFollowing(string client_id, string from_id)
        {
            IApiResponse<Follow> following = GetUserFollowingAsync(client_id, from_id).Result;

            return following;
        }

        /// <summary>
        /// Gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponsePage<Follow>
        GetUserFollowersPage(string client_id, string to_id, FollowsQueryParameters query_parameters = null)
        {
            IApiResponsePage<Follow> followers = GetUserFollowersPageAsync(client_id, to_id, query_parameters).Result;

            return followers;
        }

        /// <summary>
        /// Gets a user's followers list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Follow>
        GetUserFollowers(string client_id, string to_id)
        {
            IApiResponse<Follow> followers = GetUserFollowersAsync(client_id, to_id).Result;

            return followers;
        }

        #endregion

    }
}
