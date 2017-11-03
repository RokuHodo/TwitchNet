// standard namespaces
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Interfaces.Api;
using TwitchNet.Models.Api;
using TwitchNet.Models.Api.Games;
using TwitchNet.Models.Api.Streams;
using TwitchNet.Models.Api.Users;
using TwitchNet.Models.Api.Videos;

namespace TwitchNet.Api
{
    public static partial class
    TwitchApi
    {
        #region Fields

        private static readonly string              oauth_token_default             = string.Empty;

        private const           ApiRequestSettings  api_request_settings_default    = default(ApiRequestSettings);

        #endregion

        #region Games

        /// <summary>
        /// Asynchronously gets information about a list of games.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Game>>
        GetGamesAsync(string client_id, GamesQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Game> games = await TwitchApiInternal.GetGamesAsync(oauth_token_default, client_id, query_parameters, api_request_settings);

            return games;
        }

        #endregion

        #region Streams

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Stream>>
        GetStreamsPageAsync(string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Stream> streams = await GetStreamsPageAsync(client_id, default(StreamsQueryParameters), api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Stream>>
        GetStreamsPageAsync(string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Stream> streams = await TwitchApiInternal.GetStreamsPageAsync(oauth_token_default, client_id, query_parameters, api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Stream>>
        GetStreamsAsync(string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Stream> streams = await GetStreamsAsync(client_id, default(StreamsQueryParameters), api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Stream>>
        GetStreamsAsync(string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Stream> streams = await TwitchApiInternal.GetStreamsAsync(oauth_token_default, client_id, query_parameters, api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Metadata>>
        GetStreamsMetadataPageAsync(string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Metadata> metadata = await GetStreamsMetadataPageAsync(client_id, default(StreamsQueryParameters), api_request_settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Metadata>>
        GetStreamsMetadataPageAsync(string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Metadata> metadata = await TwitchApiInternal.GetStreamsMetadataPageAsync(oauth_token_default, client_id, query_parameters, api_request_settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Metadata>>
        GetStreamsMetadataAsync(string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Metadata> metadata = await GetStreamsMetadataAsync(client_id, default(StreamsQueryParameters), api_request_settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Metadata>>
        GetStreamsMetadataAsync(string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Metadata> metadata = await TwitchApiInternal.GetStreamsMetadataAsync(oauth_token_default, client_id, query_parameters, api_request_settings);

            return metadata;
        }

        #endregion

        #region Users

        /// <summary>
        /// Asynchronously gets the information of one or more users by their id or login.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<User>>
        GetUsersAsync(string client_id, UsersQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<User> users = await TwitchApiInternal.GetUsersAsync(oauth_token_default, client_id, query_parameters, api_request_settings);

            return users;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Follow>>
        GetUserRelationshipAsync(string client_id, string from_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> relationship = await TwitchApiInternal.GetUserRelationshipPageAsync(oauth_token_default, client_id, from_id, to_id, default(FollowsQueryParameters), api_request_settings);

            return relationship;
        }

        /// <summary>
        /// Asynchronously checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponseValue<bool>>
        IsUserFollowingAsync(string client_id, string from_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponseValue<bool> is_following = await TwitchApiInternal.IsUserFollowingAsync(oauth_token_default, client_id, from_id, to_id, api_request_settings);

            return is_following;
        }

        /// <summary>
        /// Asynchronously gets a single page of a user's following list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Follow>>
        GetUserFollowingPageAsync(string client_id, string from_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> following = await GetUserFollowingPageAsync(client_id, from_id, default(FollowsQueryParameters), api_request_settings);

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
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Follow>>
        GetUserFollowingPageAsync(string client_id, string from_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> following = await TwitchApiInternal.GetUserRelationshipPageAsync(oauth_token_default, client_id, from_id, string.Empty, query_parameters, api_request_settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a user's following list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Follow>>
        GetUserFollowingAsync(string client_id, string from_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Follow> following = await TwitchApiInternal.GetUserRelationshipAsync(oauth_token_default, client_id, from_id, string.Empty, default(FollowsQueryParameters), api_request_settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Follow>>
        GetUserFollowersPageAsync(string client_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> followers = await GetUserFollowersPageAsync(client_id, to_id, default(FollowsQueryParameters), api_request_settings);

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
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Follow>>
        GetUserFollowersPageAsync(string client_id, string to_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Follow> followers = await TwitchApiInternal.GetUserRelationshipPageAsync(oauth_token_default, client_id, string.Empty, to_id, query_parameters, api_request_settings);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a user's followers list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Follow>>
        GetUserFollowersAsync(string client_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Follow> followers = await TwitchApiInternal.GetUserRelationshipAsync(oauth_token_default, client_id, string.Empty, to_id, default(FollowsQueryParameters), api_request_settings);

            return followers;
        }

        #endregion

        #region Videos

        /// <summary>
        /// Asynchronously gets a single page of information on one or more videos.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponsePage{type}"/> interface.</returns>
        public static async Task<IApiResponsePage<Video>>
        GetVideosPageAsync(string client_id, VideosQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponsePage<Video> videos = await TwitchApiInternal.GetVideosPageAsync(oauth_token_default, client_id, query_parameters, api_request_settings);

            return videos;
        }

        /// <summary>
        /// Asynchronously gets a complete list of information on one or more videos.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">A set up customizable settings to handle diferent status codes.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Video>>
        GetVideosAsync(string client_id, VideosQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Video> videos = await TwitchApiInternal.GetVideosAsync(oauth_token_default, client_id, query_parameters, api_request_settings);

            return videos;
        }

        #endregion
    }
}
