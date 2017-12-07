// project namespaces
using TwitchNet.Interfaces.Api;
using TwitchNet.Models.Api;
using TwitchNet.Models.Api.Games;
using TwitchNet.Models.Api.Streams;
using TwitchNet.Models.Api.Users;
using TwitchNet.Models.Api.Videos;

namespace
TwitchNet.Api
{
    public static partial class
    TwitchApi
    {
        #region /games

        /// <summary>
        /// Gets information about a list of games.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Game>
        GetGames(string client_id, GamesQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Game> games = GetGamesAsync(client_id, query_parameters, api_request_settings).Result;

            return games;
        }

        #endregion

        #region /streams

        /// <summary>
        /// Gets a single page of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static IApiResponsePage<Stream>
        GetStreamsPage(string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Stream> streams = GetStreamsPageAsync(client_id, api_request_settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a single page of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static IApiResponsePage<Stream>
        GetStreamsPage(string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Stream> streams = GetStreamsPageAsync(client_id, query_parameters, api_request_settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a complete list of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Stream>
        GetStreams(string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Stream> streams = GetStreamsAsync(client_id, api_request_settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a complete list of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Stream>
        GetStreams(string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Stream> streams = GetStreamsAsync(client_id, query_parameters, api_request_settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static IApiResponsePage<Metadata>
        GetStreamsMetadataPage(string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Metadata> metadata = GetStreamsMetadataPageAsync(client_id, api_request_settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static IApiResponsePage<Metadata>
        GetStreamsMetadataPage(string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Metadata> metadata = GetStreamsMetadataPageAsync(client_id, query_parameters, api_request_settings).Result;

            return metadata;
        }

        /// <summary>
        /// Checks to see if a user is streaming.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="user_id">The user to check if they are live.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponseValue{type}"/> interface.</returns>
        public static IApiResponseValue<bool>
        IsLive(string client_id, string user_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponseValue<bool> is_live = IsLiveAsync(client_id, user_id, api_request_settings).Result;

            return is_live;
        }

        #endregion

        #region /streams/metadata

        /// <summary>
        /// Gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Metadata>
        GetStreamsMetadata(string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Metadata> metadata = GetStreamsMetadataAsync(client_id, api_request_settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Metadata>
        GetStreamsMetadata(string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Metadata> metadata = GetStreamsMetadataAsync(client_id, query_parameters, api_request_settings).Result;

            return metadata;
        }

        #endregion

        #region /users

        /// <summary>
        /// ets the information of one or more users by their id or login.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<User>
        GetUsers(string client_id, UsersQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<User> users = GetUsersAsync(client_id, query_parameters, api_request_settings).Result;

            return users;
        }

        #endregion

        #region /users/follows

        /// <summary>
        /// Gets the relationship between two users.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static IApiResponsePage<Follow>
        GetUserRelationship(string client_id, string from_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> relationship = GetUserRelationshipAsync(client_id, from_id, to_id, api_request_settings).Result;

            return relationship;
        }

        /// <summary>
        /// Checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponseValue<bool>
        IsUserFollowing(string client_id, string from_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponseValue<bool> is_following = IsUserFollowingAsync(client_id, from_id, to_id, api_request_settings).Result;

            return is_following;
        }

        /// <summary>
        /// Gets a single page of a user's following list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponsePage<Follow>
        GetUserFollowingPage(string client_id, string from_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> following = GetUserFollowingPageAsync(client_id, from_id, api_request_settings).Result;

            return following;
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
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponsePage<Follow>
        GetUserFollowingPage(string client_id, string from_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> following = GetUserFollowingPageAsync(client_id, from_id, query_parameters, api_request_settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a user's following list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Follow>
        GetUserFollowing(string client_id, string from_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Follow> following = GetUserFollowingAsync(client_id, from_id,  api_request_settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponsePage<Follow>
        GetUserFollowersPage(string client_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> followers = GetUserFollowersPageAsync(client_id, to_id, api_request_settings).Result;

            return followers;
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
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponsePage<Follow>
        GetUserFollowersPage(string client_id, string to_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> followers = GetUserFollowersPageAsync(client_id, to_id, query_parameters, api_request_settings).Result;

            return followers;
        }

        /// <summary>
        /// Gets a user's followers list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Follow>
        GetUserFollowers(string client_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Follow> followers = GetUserFollowersAsync(client_id, to_id, api_request_settings).Result;

            return followers;
        }

        #endregion

        #region /videos

        /// <summary>
        /// Gets a single page of information on one or more videos.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static IApiResponsePage<Video>
        GetVideosPage(string client_id, VideosQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Video> videos = GetVideosPageAsync(client_id, query_parameters, api_request_settings).Result;

            return videos;
        }

        /// <summary>
        /// Gets a complete list of information on one or more videos.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Video>
        GetVideos(string client_id, VideosQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Video> videos = GetVideosAsync(client_id, query_parameters, api_request_settings).Result;

            return videos;
        }

        #endregion
    }
}
