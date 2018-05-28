// standard namespaces
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Rest.Api.Clips;
using TwitchNet.Rest.Api.Games;
using TwitchNet.Rest.Api.Streams;
using TwitchNet.Rest.Api.Users;
using TwitchNet.Rest.Api.Videos;

namespace
TwitchNet.Rest.Api
{
    public static partial class
    TwitchApi
    {
        #region /clips

        /// <summary>
        /// Asynchronously gets information about a clip.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<Clip>>>
        GetClipAsync(string client_id, ClipQueryParameters parameters, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id  = client_id;

            IHelixResponse<Data<Clip>> clip = await TwitchApiInternal.GetClipAsync(request_info, parameters, settings);

            return clip;
        }

        #endregion

        #region /games

        /// <summary>
        /// Asynchronously gets information about a list of games.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<Game>>>
        GetGamesAsync(string client_id, GamesQueryParameters parameters, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<Data<Game>> games = await TwitchApiInternal.GetGamesAsync(request_info, parameters, settings);

            return games;
        }

        #endregion

        #region /games/top

        /// <summary>
        /// Asynchronously gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesPageAsync(string client_id, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<DataPage<Game>> top_games = await TwitchApiInternal.GetTopGamesPageAsync(request_info, default(TopGamesQueryParameters), settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesPageAsync(string client_id, TopGamesQueryParameters parameters, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<DataPage<Game>> top_games = await TwitchApiInternal.GetTopGamesPageAsync(request_info, parameters, settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesAsync(string client_id, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<DataPage<Game>> top_games = await TwitchApiInternal.GetTopGamesAsync(request_info, default(TopGamesQueryParameters), settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesAsync(string client_id, TopGamesQueryParameters parameters, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<DataPage<Game>> top_games = await TwitchApiInternal.GetTopGamesAsync(request_info, parameters, settings);

            return top_games;
        }

        #endregion

        #region /streams

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsPageAsync(string client_id, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<DataPage<Stream>> streams = await TwitchApiInternal.GetStreamsPageAsync(request_info, default(StreamsQueryParameters), settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsPageAsync(string client_id, StreamsQueryParameters parameters, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<DataPage<Stream>> streams = await TwitchApiInternal.GetStreamsPageAsync(request_info, parameters, settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsAsync(string client_id, RestSettings settings = default(RestSettings))
        {
            IHelixResponse<DataPage<Stream>> streams = await GetStreamsAsync(client_id, default(StreamsQueryParameters), settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsAsync(string client_id, StreamsQueryParameters parameters, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<DataPage<Stream>> streams = await TwitchApiInternal.GetStreamsAsync(request_info, parameters, settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously checks to see if a user is streaming.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="user_id">The user to check if they are live.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<bool>>
        IsStreamLiveAsync(string client_id, string user_id, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<bool> is_live = await TwitchApiInternal.IsStreamLiveAsync(request_info, user_id, settings);

            return is_live;
        }

        #endregion

        #region /streams/metadata

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Metadata>>>
        GetStreamsMetadataPageAsync(string client_id, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<DataPage<Metadata>> metadata = await TwitchApiInternal.GetStreamsMetadataPageAsync(request_info, default(StreamsQueryParameters), settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Metadata>>>
        GetStreamsMetadataPageAsync(string client_id, StreamsQueryParameters parameters, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<DataPage<Metadata>> metadata = await TwitchApiInternal.GetStreamsMetadataPageAsync(request_info, parameters, settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Metadata>>>
        GetStreamsMetadataAsync(string client_id, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<DataPage<Metadata>> metadata = await TwitchApiInternal.GetStreamsMetadataAsync(request_info, default(StreamsQueryParameters), settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Metadata>>>
        GetStreamsMetadataAsync(string client_id, StreamsQueryParameters parameters, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<DataPage<Metadata>> metadata = await TwitchApiInternal.GetStreamsMetadataAsync(request_info, parameters, settings);

            return metadata;
        }

        #endregion

        #region /users

        /// <summary>
        /// Asynchronously gets the information of one or more users by their id or login.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<User>>>
        GetUsersAsync(string client_id, UsersQueryParameters parameters, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<Data<User>> users = await TwitchApiInternal.GetUsersAsync(request_info, parameters, settings);

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
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserRelationshipAsync(string client_id, string from_id, string to_id, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> relationship = await TwitchApiInternal.GetUserRelationshipPageAsync(request_info, from_id, to_id, default(FollowsQueryParameters), settings);

            return relationship;
        }        

        /// <summary>
        /// Asynchronously gets a single page of a user's following list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingPageAsync(string client_id, string from_id, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> following = await TwitchApiInternal.GetUserRelationshipPageAsync(request_info, from_id, string.Empty, default(FollowsQueryParameters), settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a single page of a user's following list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingPageAsync(string client_id, string from_id, FollowsQueryParameters parameters, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> following = await TwitchApiInternal.GetUserRelationshipPageAsync(request_info, from_id, string.Empty, parameters, settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a user's following list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingAsync(string client_id, string from_id, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> following = await TwitchApiInternal.GetUserRelationshipAsync(request_info, from_id, string.Empty, default(FollowsQueryParameters), settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a user's following list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingAsync(string client_id, string from_id, FollowsQueryParameters parameters, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> following = await TwitchApiInternal.GetUserRelationshipAsync(request_info, from_id, string.Empty, parameters, settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersPageAsync(string client_id, string to_id, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> followers = await TwitchApiInternal.GetUserRelationshipPageAsync(request_info, string.Empty, to_id, default(FollowsQueryParameters), settings);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersPageAsync(string client_id, string to_id, FollowsQueryParameters parameters, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> followers = await TwitchApiInternal.GetUserRelationshipPageAsync(request_info, string.Empty, to_id, parameters, settings);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a user's followers list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersAsync(string client_id, string to_id, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> followers = await TwitchApiInternal.GetUserRelationshipAsync(request_info, string.Empty, to_id, default(FollowsQueryParameters), settings);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a user's followers list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersAsync(string client_id, string to_id, FollowsQueryParameters parameters, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> followers = await TwitchApiInternal.GetUserRelationshipAsync(request_info, string.Empty, to_id, parameters, settings);

            return followers;
        }

        /// <summary>
        /// Asynchronously checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<bool>>
        IsUserFollowingAsync(string client_id, string from_id, string to_id, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<bool> is_following = await TwitchApiInternal.IsUserFollowingAsync(request_info, from_id, to_id, settings);

            return is_following;
        }

        #endregion

        #region /videos

        /// <summary>
        /// Asynchronously gets a single page of information on one or more videos.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Video>>>
        GetVideosPageAsync(string client_id, VideosQueryParameters parameters, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<DataPage<Video>> videos = await TwitchApiInternal.GetVideosPageAsync(request_info, parameters, settings);

            return videos;
        }

        /// <summary>
        /// Asynchronously gets a complete list of information on one or more videos.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Video>>>
        GetVideosAsync(string client_id, VideosQueryParameters parameters, RestSettings settings = default(RestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.client_id = client_id;

            IHelixResponse<DataPage<Video>> videos = await TwitchApiInternal.GetVideosAsync(request_info, parameters, settings);

            return videos;
        }

        #endregion
    }
}
