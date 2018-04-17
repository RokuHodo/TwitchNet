﻿// project namespaces
using TwitchNet.Interfaces.Api;
using TwitchNet.Models.Api;
using TwitchNet.Models.Api.Clips;
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
        #region /clips

        /// <summary>
        /// Gets information about a clip.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Data<Clip>>
        GetClip(string client_id, ClipQueryParameters parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<Data<Clip>> clip = GetClipAsync(client_id, parameters, settings).Result;

            return clip;
        }

        #endregion

        #region /games

        /// <summary>
        /// Gets information about a list of games.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Data<Game>>
        GetGames(string client_id, GamesQueryParameters parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<Data<Game>> games = GetGamesAsync(client_id, parameters, settings).Result;

            return games;
        }

        #endregion

        #region /games/top

        /// <summary>
        /// Gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Game>>
        GetTopGamesPage(string client_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Game>> top_games = GetTopGamesPageAsync(client_id, settings).Result;

            return top_games;
        }

        /// <summary>
        /// Gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Game>>
        GetTopGamesPage(string client_id, TopGamesQueryParameters parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Game>> top_games = GetTopGamesPageAsync(client_id, parameters, settings).Result;

            return top_games;
        }

        /// <summary>
        /// Gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Game>>
        GetTopGames(string client_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Game>> top_games = GetTopGamesAsync(client_id, settings).Result;

            return top_games;
        }

        /// <summary>
        /// G6ets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Game>>
        GetTopGames(string client_id, TopGamesQueryParameters parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Game>> top_games = GetTopGamesAsync(client_id, parameters, settings).Result;

            return top_games;
        }

        #endregion

        #region /streams

        /// <summary>
        /// Gets a single page of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Stream>>
        GetStreamsPage(string client_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Stream>> streams = GetStreamsPageAsync(client_id, settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a single page of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Stream>>
        GetStreamsPage(string client_id, StreamsQueryParameters parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Stream>> streams = GetStreamsPageAsync(client_id, parameters, settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a complete list of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Stream>>
        GetStreams(string client_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Stream>> streams = GetStreamsAsync(client_id, settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a complete list of streams.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Stream>>
        GetStreams(string client_id, StreamsQueryParameters parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Stream>> streams = GetStreamsAsync(client_id, parameters, settings).Result;

            return streams;
        }        

        /// <summary>
        /// Checks to see if a user is streaming.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="user_id">The user to check if they are live.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<bool>
        IsStreamLive(string client_id, string user_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<bool> is_live = IsStreamLiveAsync(client_id, user_id, settings).Result;

            return is_live;
        }

        #endregion

        #region /streams/metadata

        /// <summary>
        /// Gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Metadata>>
        GetStreamsMetadataPage(string client_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Metadata>> metadata = GetStreamsMetadataPageAsync(client_id, settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Metadata>>
        GetStreamsMetadataPage(string client_id, StreamsQueryParameters parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Metadata>> metadata = GetStreamsMetadataPageAsync(client_id, parameters, settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Metadata>>
        GetStreamsMetadata(string client_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Metadata>> metadata = GetStreamsMetadataAsync(client_id, settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Metadata>>
        GetStreamsMetadata(string client_id, StreamsQueryParameters parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Metadata>> metadata = GetStreamsMetadataAsync(client_id, parameters, settings).Result;

            return metadata;
        }

        #endregion

        #region /users

        /// <summary>
        /// ets the information of one or more users by their id or login.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Data<User>>
        GetUsers(string client_id, UsersQueryParameters parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<Data<User>> users = GetUsersAsync(client_id, parameters, settings).Result;

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
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserRelationship(string client_id, string from_id, string to_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> relationship = GetUserRelationshipAsync(client_id, from_id, to_id, settings).Result;

            return relationship;
        }

        /// <summary>
        /// Checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<bool>
        IsUserFollowing(string client_id, string from_id, string to_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<bool> is_following = IsUserFollowingAsync(client_id, from_id, to_id, settings).Result;

            return is_following;
        }

        /// <summary>
        /// Gets a single page of a user's following list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowingPage(string client_id, string from_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> following = GetUserFollowingPageAsync(client_id, from_id, settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a single page of a user's following list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowingPage(string client_id, string from_id, FollowsQueryParameters parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> following = GetUserFollowingPageAsync(client_id, from_id, parameters, settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a user's following list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowing(string client_id, string from_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> following = GetUserFollowingAsync(client_id, from_id, settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a user's following list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowing(string client_id, string from_id, FollowsQueryParameters parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> following = GetUserFollowingAsync(client_id, from_id, parameters, settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowersPage(string client_id, string to_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> followers = GetUserFollowersPageAsync(client_id, to_id, settings).Result;

            return followers;
        }

        /// <summary>
        /// Gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowersPage(string client_id, string to_id, FollowsQueryParameters parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> followers = GetUserFollowersPageAsync(client_id, to_id, parameters, settings).Result;

            return followers;
        }

        /// <summary>
        /// Gets a user's followers list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowers(string client_id, string to_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> followers = GetUserFollowersAsync(client_id, to_id, settings).Result;

            return followers;
        }

        /// <summary>
        /// Gets a user's followers list.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowers(string client_id, string to_id, FollowsQueryParameters parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> followers = GetUserFollowersAsync(client_id, to_id, parameters, settings).Result;

            return followers;
        }

        #endregion

        #region /videos

        /// <summary>
        /// Gets a single page of information on one or more videos.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Video>>
        GetVideosPage(string client_id, VideosQueryParameters parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Video>> videos = GetVideosPageAsync(client_id, parameters, settings).Result;

            return videos;
        }

        /// <summary>
        /// Gets a complete list of information on one or more videos.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Video>>
        GetVideos(string client_id, VideosQueryParameters parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Video>> videos = GetVideosAsync(client_id, parameters, settings).Result;

            return videos;
        }

        #endregion
    }
}
