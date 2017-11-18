﻿// standard namespaces
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Interfaces.Api;
using TwitchNet.Models.Api;
using TwitchNet.Models.Api.Entitlements;
using TwitchNet.Models.Api.Games;
using TwitchNet.Models.Api.Streams;
using TwitchNet.Models.Api.Users;
using TwitchNet.Models.Api.Videos;
using TwitchNet.Utilities;

// imported .dll's
using RestSharp;

namespace TwitchNet.Api
{
    //TODO: Implement Exceptions wherever a user can wrongly pass through data
    internal static partial class
    TwitchApiInternal
    {
        #region Entitlements

        /// <summary>
        /// Asynchronously creates a URL where you can upload a manifest file and notify users that they have an entitlement.
        /// </summary>
        /// <param name="application_token">The application access token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<Url>>
        CreateEntitlementGrantsUploadUrlAsync(string application_token, string client_id, EntitlementQueryParameters query_parameters, ApiRequestSettings api_request_settings)
        {
            IApiResponse<Url> url = await RestRequestUtil.ExecuteRequestAsync<Url, EntitlementQueryParameters>("entitlements/upload", Method.POST, application_token, client_id, query_parameters, api_request_settings);

            return url;
        }

        #endregion

        #region Games

        /// <summary>
        /// Asynchronously information about a list of games.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<Game>>
        GetGamesAsync(string oauth_token, string client_id, GamesQueryParameters query_parameters, ApiRequestSettings api_request_settings)
        {
            IApiResponse<Game> games = await RestRequestUtil.ExecuteRequestAsync<Game, GamesQueryParameters>("games", Method.GET, oauth_token, client_id, query_parameters, api_request_settings);

            return games;
        }

        #endregion

        #region Streams

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponsePage<Stream>>
        GetStreamsPageAsync(string oauth_token, string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings)
        {
            IApiResponsePage<Stream> streams = await RestRequestUtil.ExecuteRequestPageAsync<Stream>("streams", Method.GET, oauth_token, client_id, query_parameters, api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<Stream>>
        GetStreamsAsync(string oauth_token, string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings)
        {
            IApiResponse<Stream> streams = await RestRequestUtil.ExecuteRequestAllPagesAsync<Stream>("streams", Method.GET, oauth_token, client_id, query_parameters, api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponsePage<Metadata>>
        GetStreamsMetadataPageAsync(string oauth_token, string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings)
        {
            IApiResponsePage<Metadata> metadata = await RestRequestUtil.ExecuteRequestPageAsync<Metadata>("streams/metadata", Method.GET, oauth_token, client_id, query_parameters, api_request_settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<Metadata>>
        GetStreamsMetadataAsync(string oauth_token, string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings)
        {
            IApiResponse<Metadata> metadata = await RestRequestUtil.ExecuteRequestAllPagesAsync<Metadata>("streams/metadata", Method.GET, oauth_token, client_id, query_parameters, api_request_settings);

            return metadata;
        }

        #endregion

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
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>/// <param name="query_parameters">The users to look up either by id or by login.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<User>>
        GetUsersAsync(string oauth_token, string client_id, UsersQueryParameters query_parameters, ApiRequestSettings api_request_settings)
        {
            IApiResponse<User> users = await RestRequestUtil.ExecuteRequestAsync<User, UsersQueryParameters>("users", Method.GET, oauth_token, client_id, query_parameters, api_request_settings);

            return users;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users, or a single page of the following/follower lists of one user.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponsePage<Follow>>
        GetUserRelationshipPageAsync(string oauth_token, string client_id, string from_id, string to_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings)
        {
            if (query_parameters.IsNull())
            {
                query_parameters = new FollowsQueryParameters();
            }
            query_parameters.from_id = from_id;
            query_parameters.to_id = to_id;

            IApiResponsePage<Follow> follows = await RestRequestUtil.ExecuteRequestPageAsync<Follow>("users/follows", Method.GET, oauth_token, client_id, query_parameters, api_request_settings);

            return follows;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users, or the complete following/follower lists of one user.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="from_id">The user to compare from. Used to get the following list of a user.</param>
        /// <param name="to_id">The user to compare to. Used to get a user's follower list.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request. The 'to_id' and 'from_id' properties in the parameters are ignored if specified.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<Follow>>
        GetUserRelationshipAsync(string oauth_token, string client_id, string from_id, string to_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings)
        {
            if (query_parameters.IsNull())
            {
                query_parameters = new FollowsQueryParameters();
            }            
            query_parameters.from_id = from_id;
            query_parameters.to_id = to_id;

            IApiResponse<Follow> follows = await RestRequestUtil.ExecuteRequestAllPagesAsync<Follow>("users/follows", Method.GET, oauth_token, client_id, query_parameters, api_request_settings);

            return follows;
        }

        /// <summary>
        /// Asynchronously checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponseValue<bool>>
        IsUserFollowingAsync(string oauth_token, string client_id, string from_id, string to_id, ApiRequestSettings api_request_settings)
        {
            IApiResponsePage<Follow> relationship = await GetUserRelationshipPageAsync(oauth_token, client_id, from_id, to_id, default(FollowsQueryParameters), api_request_settings);

            ApiResponseValue<bool> is_following = new ApiResponseValue<bool>(relationship, relationship.result.data.IsValid());

            return is_following;
        }

        /// <summary>
        /// <para>Asynchronously sets the description of a user specified by the OAuth token.</para>
        /// <para>Required Scope: 'user:read'</para>
        /// </summary>
        /// <param name="oauth_token">The OAuth token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no OAuth token was provided.</param>
        /// <param name="description">The new description to set.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponseValue<bool>>
        SetUserDescriptionAsync(string oauth_token, string client_id, string description, ApiRequestSettings api_request_settings)
        {
            QueryParameter[] query_parameters = new QueryParameter[]
            {
                new QueryParameter
                {
                    name = "description",
                    value = description
                },
            };

            IApiResponse<User> users = await RestRequestUtil.ExecuteRequestAsync<User>("users", Method.PUT, oauth_token, client_id, query_parameters, api_request_settings);

            // TODO: SetUserDescriptionAsync - Test to see if this is a valid check to see if the description was actually updated.
            ApiResponseValue<bool> response = new ApiResponseValue<bool>(users, users.result.data.IsValid());

            return response;
        }

        #endregion

        #region Videos

        /// <summary>
        /// Asynchronously gets information on one or more videos.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        internal static async Task<IApiResponsePage<Video>>
        GetVideosPageAsync(string oauth_token, string client_id, VideosQueryParameters query_parameters, ApiRequestSettings api_request_settings)
        {
            IApiResponsePage<Video> videos = await RestRequestUtil.ExecuteRequestPageAsync<Video>("videos", Method.GET, oauth_token, client_id, query_parameters, api_request_settings);

            return videos;
        }

        /// <summary>
        /// Asynchronously gets a complete list of information on one or more videos.
        /// </summary>
        /// <param name="oauth_token">The OAuth token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<Video>>
        GetVideosAsync(string oauth_token, string client_id, VideosQueryParameters query_parameters, ApiRequestSettings api_request_settings)
        {
            IApiResponse<Video> videos = await RestRequestUtil.ExecuteRequestAllPagesAsync<Video>("videos", Method.GET, oauth_token, client_id, query_parameters, api_request_settings);

            return videos;
        }

        #endregion
    }
}