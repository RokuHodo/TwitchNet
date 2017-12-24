// standard namespaces
using System.Threading.Tasks;

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
        #region Fields

        private static readonly string              bearer_token_default             = string.Empty;

        private const           ApiRequestSettings  api_request_settings_default    = default(ApiRequestSettings);

        #endregion

        #region /games

        /// <summary>
        /// Asynchronously gets information about a list of games.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Data<Game>>>
        GetGamesAsync(string client_id, GamesQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Data<Game>> games = await TwitchApiInternal.GetGamesAsync(bearer_token_default, client_id, query_parameters, api_request_settings);

            return games;
        }

        #endregion

        #region /games/top

        /// <summary>
        /// Asynchronously gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Game>>>
        GetTopGamesPageAsync(string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Game>> top_games = await GetTopGamesPageAsync(client_id, default(TopGamesQueryParameters), api_request_settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Game>>>
        GetTopGamesPageAsync(string client_id, TopGamesQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Game>> top_games = await TwitchApiInternal.GetTopGamesPageAsync(bearer_token_default, client_id, query_parameters, api_request_settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Data<Game>>>
        GetTopGamesAsync(string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Data<Game>> top_games = await TwitchApiInternal.GetTopGamesAsync(bearer_token_default, client_id, api_request_settings);

            return top_games;
        }

        #endregion

        #region /streams

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Stream>>>
        GetStreamsPageAsync(string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Stream>> streams = await GetStreamsPageAsync(client_id, default(StreamsQueryParameters), api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Stream>>>
        GetStreamsPageAsync(string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Stream>> streams = await TwitchApiInternal.GetStreamsPageAsync(bearer_token_default, client_id, query_parameters, api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Data<Stream>>>
        GetStreamsAsync(string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Data<Stream>> streams = await GetStreamsAsync(client_id, default(StreamsQueryParameters), api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Data<Stream>>>
        GetStreamsAsync(string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Data<Stream>> streams = await TwitchApiInternal.GetStreamsAsync(bearer_token_default, client_id, query_parameters, api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously checks to see if a user is streaming.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="user_id">The user to check if they are live.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<bool>>
        IsStreamLiveAsync(string client_id, string user_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<bool> is_live = await TwitchApiInternal.IsStreamLiveAsync(string.Empty, client_id, user_id, api_request_settings);

            return is_live;
        }

        #endregion

        #region /streams/metadata

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Metadata>>>
        GetStreamsMetadataPageAsync(string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Metadata>> metadata = await GetStreamsMetadataPageAsync(client_id, default(StreamsQueryParameters), api_request_settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Metadata>>>
        GetStreamsMetadataPageAsync(string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Metadata>> metadata = await TwitchApiInternal.GetStreamsMetadataPageAsync(bearer_token_default, client_id, query_parameters, api_request_settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Data<Metadata>>>
        GetStreamsMetadataAsync(string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Data<Metadata>> metadata = await GetStreamsMetadataAsync(client_id, default(StreamsQueryParameters), api_request_settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Data<Metadata>>>
        GetStreamsMetadataAsync(string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Data<Metadata>> metadata = await TwitchApiInternal.GetStreamsMetadataAsync(bearer_token_default, client_id, query_parameters, api_request_settings);

            return metadata;
        }

        #endregion

        #region /users

        /// <summary>
        /// Asynchronously gets the information of one or more users by their id or login.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Data<User>>>
        GetUsersAsync(string client_id, UsersQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Data<User>> users = await TwitchApiInternal.GetUsersAsync(bearer_token_default, client_id, query_parameters, api_request_settings);

            return users;
        }

        #endregion

        #region /users/follows

        /// <summary>
        /// Asynchronously gets the relationship between two users.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserRelationshipAsync(string client_id, string from_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> relationship = await TwitchApiInternal.GetUserRelationshipPageAsync(bearer_token_default, client_id, from_id, to_id, default(FollowsQueryParameters), api_request_settings);

            return relationship;
        }

        /// <summary>
        /// Asynchronously checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<bool>>
        IsUserFollowingAsync(string client_id, string from_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<bool> is_following = await TwitchApiInternal.IsUserFollowingAsync(bearer_token_default, client_id, from_id, to_id, api_request_settings);

            return is_following;
        }

        /// <summary>
        /// Asynchronously gets a single page of a user's following list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserFollowingPageAsync(string client_id, string from_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> following = await GetUserFollowingPageAsync(client_id, from_id, default(FollowsQueryParameters), api_request_settings);

            return following;
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
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserFollowingPageAsync(string client_id, string from_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> following = await TwitchApiInternal.GetUserRelationshipPageAsync(bearer_token_default, client_id, from_id, string.Empty, query_parameters, api_request_settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a user's following list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsData<Follow>>>
        GetUserFollowingAsync(string client_id, string from_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsData<Follow>> following = await TwitchApiInternal.GetUserRelationshipAsync(bearer_token_default, client_id, from_id, string.Empty, default(FollowsQueryParameters), api_request_settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserFollowersPageAsync(string client_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> followers = await GetUserFollowersPageAsync(client_id, to_id, default(FollowsQueryParameters), api_request_settings);

            return followers;
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
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserFollowersPageAsync(string client_id, string to_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> followers = await TwitchApiInternal.GetUserRelationshipPageAsync(bearer_token_default, client_id, string.Empty, to_id, query_parameters, api_request_settings);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a user's followers list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsData<Follow>>>
        GetUserFollowersAsync(string client_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsData<Follow>> followers = await TwitchApiInternal.GetUserRelationshipAsync(bearer_token_default, client_id, string.Empty, to_id, default(FollowsQueryParameters), api_request_settings);

            return followers;
        }

        #endregion

        #region /videos

        /// <summary>
        /// Asynchronously gets a single page of information on one or more videos.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Video>>>
        GetVideosPageAsync(string client_id, VideosQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Video>> videos = await TwitchApiInternal.GetVideosPageAsync(bearer_token_default, client_id, query_parameters, api_request_settings);

            return videos;
        }

        /// <summary>
        /// Asynchronously gets a complete list of information on one or more videos.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Data<Video>>>
        GetVideosAsync(string client_id, VideosQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Data<Video>> videos = await TwitchApiInternal.GetVideosAsync(bearer_token_default, client_id, query_parameters, api_request_settings);

            return videos;
        }

        #endregion
    }
}
