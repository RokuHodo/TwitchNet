// project namespaces
using TwitchNet.Interfaces.Api;
using TwitchNet.Models.Api;
using TwitchNet.Models.Api.Clips;
using TwitchNet.Models.Api.Entitlements;
using TwitchNet.Models.Api.Games;
using TwitchNet.Models.Api.Streams;
using TwitchNet.Models.Api.Users;
using TwitchNet.Models.Api.Videos;

namespace
TwitchNet.Api
{
    public static partial class
    TwitchApiBearer
    {
        #region /clips

        /// <summary>
        /// <para>Creates a clip.</para>
        /// <para>Required Scope: 'clips:edit'.</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Data<CreatedClip>>
        CreateClip(string bearer_token, ClipCreationQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<Data<CreatedClip>> clip = CreateClipAsync(bearer_token, query_parameters, settings).Result;

            return clip;
        }

        /// <summary>
        /// <para>Creates a clip.</para>
        /// <para>Required Scope: 'clips:edit'.</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Data<CreatedClip>>
        CreateClip(string bearer_token, string client_id, ClipCreationQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<Data<CreatedClip>> clip = CreateClipAsync(bearer_token, client_id, query_parameters, settings).Result;

            return clip;
        }

        /// <summary>
        /// Gets information about a clip.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Data<Clip>>
        GetClip(string bearer_token, ClipQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<Data<Clip>> clip = GetClipAsync(bearer_token, query_parameters, settings).Result;

            return clip;
        }

        /// <summary>
        /// Gets information about a clip.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Data<Clip>>
        GetClip(string bearer_token, string client_id, ClipQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<Data<Clip>> clip = GetClipAsync(bearer_token, client_id, query_parameters, settings).Result;

            return clip;
        }

        #endregion

        #region /entitlements/upload

        /// <summary>
        /// <para>Creates a URL where you can upload a manifest file and notify users that they have an entitlement.</para>
        /// <para>Required Authorization: Application Access Token</para>
        /// </summary>
        /// <param name="application_token">The application access token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Data<Url>>
        CreateEntitlementGrantsUploadUrl(string application_token, EntitlementQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<Data<Url>> url = CreateEntitlementGrantsUploadUrlAsync(application_token, query_parameters, settings).Result;

            return url;
        }

        /// <summary>
        /// <para>Creates a URL where you can upload a manifest file and notify users that they have an entitlement.</para>
        /// <para>Required Authorization: Application Access Token</para>
        /// </summary>
        /// <param name="application_token">The application access token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Data<Url>>
        CreateEntitlementGrantsUploadUrl(string application_token, string client_id, EntitlementQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<Data<Url>> url = CreateEntitlementGrantsUploadUrlAsync(application_token, client_id, query_parameters, settings).Result;

            return url;
        }

        #endregion

        #region /games

        /// <summary>
        /// Gets information about a list of games.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Data<Game>>
        GetGames(string bearer_token, GamesQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<Data<Game>> games = GetGamesAsync(bearer_token, query_parameters, settings).Result;

            return games;
        }

        /// <summary>
        /// Gets information about a list of games.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Data<Game>>
        GetGames(string bearer_token, string client_id, GamesQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<Data<Game>> games = GetGamesAsync(bearer_token, client_id, query_parameters, settings).Result;

            return games;
        }

        #endregion

        #region /games/top

        /// <summary>
        /// Gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Game>>
        GetTopGamesPage(string bearer_token, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Game>> top_games = GetTopGamesPageAsync(bearer_token, settings).Result;

            return top_games;
        }

        /// <summary>
        /// Gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Game>>
        GetTopGamesPage(string bearer_token, TopGamesQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Game>> top_games = GetTopGamesPageAsync(bearer_token, query_parameters, settings).Result;

            return top_games;
        }

        /// <summary>
        /// Gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Game>>
        GetTopGamesPage(string bearer_token, string client_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Game>> top_games = GetTopGamesPageAsync(bearer_token, client_id, settings).Result;

            return top_games;
        }

        /// <summary>
        /// Gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Game>>
        GetTopGamesPage(string bearer_token, string client_id, TopGamesQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Game>> top_games = GetTopGamesPageAsync(bearer_token, client_id, query_parameters, settings).Result;

            return top_games;
        }

        /// <summary>
        /// Gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Game>>
        GetTopGames(string bearer_token, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Game>> top_games = GetTopGamesAsync(bearer_token, settings).Result;

            return top_games;
        }

        /// <summary>
        /// Gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Game>>
        GetTopGames(string bearer_token, TopGamesQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Game>> top_games = GetTopGamesAsync(bearer_token, query_parameters, settings).Result;

            return top_games;
        }

        /// <summary>
        /// Gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Game>>
        GetTopGames(string bearer_token, string client_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Game>> top_games = GetTopGamesAsync(bearer_token, client_id, settings).Result;

            return top_games;
        }

        /// <summary>
        /// Gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Game>>
        GetTopGames(string bearer_token, string client_id, TopGamesQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Game>> top_games = GetTopGamesAsync(bearer_token, client_id, query_parameters, settings).Result;

            return top_games;
        }

        #endregion

        #region /streams

        /// <summary>
        /// Gets a single page of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Stream>>
        GetStreamsPage(string bearer_token, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Stream>> streams = GetStreamsPageAsync(bearer_token, settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a single page of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Stream>>
        GetStreamsPage(string bearer_token, StreamsQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Stream>> streams = GetStreamsPageAsync(bearer_token, query_parameters, settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a single page of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Stream>>
        GetStreamsPage(string bearer_token, string client_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Stream>> streams = GetStreamsPageAsync(bearer_token, client_id, settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a single page of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Stream>>
        GetStreamsPage(string bearer_token, string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Stream>> streams = GetStreamsPageAsync(bearer_token, client_id, query_parameters, settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a complete list of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Stream>>
        GetStreams(string bearer_token, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Stream>> streams = GetStreamsAsync(bearer_token, settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a complete list of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Stream>>
        GetStreams(string bearer_token, StreamsQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Stream>> streams = GetStreamsAsync(bearer_token, query_parameters, settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a complete list of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Stream>>
        GetStreams(string bearer_token, string client_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Stream>> streams = GetStreamsAsync(bearer_token, client_id, settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a complete list of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Stream>>
        GetStreams(string bearer_token, string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Stream>> streams = GetStreamsAsync(bearer_token, client_id, query_parameters, settings).Result;

            return streams;
        }        

        /// <summary>
        /// Checks to see if a user is streaming.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="user_id">The user to check if they are live.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<bool>
        IsStreamLive(string bearer_token, string user_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<bool> is_live = IsStreamLiveAsync(bearer_token, user_id, settings).Result;

            return is_live;
        }

        /// <summary>
        /// Checks to see if a user is streaming.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="user_id">The user to check if they are live.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<bool>
        IsStreamLive(string bearer_token, string client_id, string user_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<bool> is_live = IsstreamLiveAsync(bearer_token, client_id, user_id, settings).Result;

            return is_live;
        }

        #endregion

        #region /streams/metadata

        /// <summary>
        /// Gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Metadata>>
        GetStreamsMetadataPage(string bearer_token, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Metadata>> metadata = GetStreamsMetadataPageAsync(bearer_token, settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Metadata>>
        GetStreamsMetadataPage(string bearer_token, StreamsQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Metadata>> metadata = GetStreamsMetadataPageAsync(bearer_token, query_parameters, settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Metadata>>
        GetStreamsMetadataPage(string bearer_token, string client_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Metadata>> metadata = GetStreamsMetadataPageAsync(bearer_token, client_id, settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Metadata>>
        GetStreamsMetadataPage(string bearer_token, string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Metadata>> metadata = GetStreamsMetadataPageAsync(bearer_token, client_id, query_parameters, settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Metadata>>
        GetStreamsMetadata(string bearer_token, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Metadata>> metadata = GetStreamsMetadataAsync(bearer_token, settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Metadata>>
        GetStreamsMetadata(string bearer_token, StreamsQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Metadata>> metadata = GetStreamsMetadataAsync(bearer_token, query_parameters, settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Metadata>>
        GetStreamsMetadata(string bearer_token, string client_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Metadata>> metadata = GetStreamsMetadataAsync(bearer_token, client_id, settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Metadata>>
        GetStreamsMetadata(string bearer_token, string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Metadata>> metadata = GetStreamsMetadataAsync(bearer_token, client_id, query_parameters, settings).Result;

            return metadata;
        }

        #endregion

        #region /users

        /// <summary>
        /// <para>Gets the information of the user looked up by the provided Bearer token.</para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Data<User>>
        GetUser(string bearer_token, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<Data<User>> users = GetUserAsync(bearer_token, settings).Result;

            return users;
        }

        /// <summary>
        /// <para>Gets the information of the user looked up by the provided Bearer token.</para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Data<User>>
        GetUser(string bearer_token, string client_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<Data<User>> users = GetUserAsync(bearer_token, client_id, settings).Result;

            return users;
        }

        /// <summary>
        /// <para>
        /// Gets the information of one or more users by their id or login.
        /// If no <paramref name="ids"/> are specified, the user is looked up by if the Bearer token provided.
        /// </para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Data<User>>
        GetUsers(string bearer_token, UsersQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<Data<User>> users = GetUsersAsync(bearer_token, query_parameters, settings).Result;

            return users;
        }

        /// <summary>
        /// <para>
        /// Gets the information of one or more users by their id or login.
        /// If no <paramref name="ids"/> are specified, the user is looked up by if the Bearer token provided.
        /// </para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Data<User>>
        GetUsers(string bearer_token, string client_id, UsersQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<Data<User>> users = GetUsersAsync(bearer_token, client_id, query_parameters, settings).Result;

            return users;
        }

        /// <summary>
        /// <para>Sets the description of a user specified by the Bearer token provided.</para>
        /// <para>Required Scope: 'user:edit'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="description">The new description to set.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Data<User>>
        SetUserDescription(string bearer_token, string description, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<Data<User>> success = SetUserDescriptionAsync(bearer_token, description, settings).Result;

            return success;
        }

        /// <summary>
        /// <para>Sets the description of a user specified by the Bearer token provided.</para>
        /// <para>Required Scope: 'user:edit'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="description">The new description to set.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<Data<User>>
        SetUserDescription(string bearer_token, string client_id, string description, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<Data<User>> success = SetUserDescriptionAsync(bearer_token, client_id, description, settings).Result;

            return success;
        }

        #endregion

        #region /users/follows

        /// <summary>
        /// Gets the relationship between two users.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserRelationship(string bearer_token, string from_id, string to_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> relationship = GetUserRelationshipAsync(bearer_token, from_id, to_id, settings).Result;

            return relationship;
        }

        /// <summary>
        /// Gets the relationship between two users.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserRelationship(string bearer_token, string client_id, string from_id, string to_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> relationship = GetUserRelationshipAsync(bearer_token, client_id, from_id, to_id, settings).Result;

            return relationship;
        }

        /// <summary>
        /// Checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<bool>
        IsUserFollowing(string bearer_token, string from_id, string to_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<bool> is_following = IsUserFollowingAsync(bearer_token, from_id, to_id, settings).Result;

            return is_following;
        }

        /// <summary>
        /// Checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<bool>
        IsUserFollowing(string bearer_token, string client_id, string from_id, string to_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<bool> is_following = IsUserFollowingAsync(bearer_token, client_id, from_id, to_id, settings).Result;

            return is_following;
        }

        /// <summary>
        /// Gets a single page of a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowingPage(string bearer_token, string from_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> following = GetUserFollowingPageAsync(bearer_token, from_id, settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a single page of a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowingPage(string bearer_token, string from_id, FollowsQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> following = GetUserFollowingPageAsync(bearer_token, from_id, query_parameters, settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a single page of a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowingPage(string bearer_token, string client_id, string from_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> following = GetUserFollowingPageAsync(bearer_token, client_id, from_id, settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a single page of a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowingPage(string bearer_token, string client_id, string from_id, FollowsQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> following = GetUserFollowingPageAsync(bearer_token, client_id, from_id, query_parameters, settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowing(string bearer_token, string from_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> following = GetUserFollowingAsync(bearer_token, from_id, settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowing(string bearer_token, string from_id, FollowsQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> following = GetUserFollowingAsync(bearer_token, from_id, query_parameters, settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowing(string bearer_token, string client_id, string from_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> following = GetUserFollowingAsync(bearer_token, client_id, from_id, settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowing(string bearer_token, string client_id, string from_id, FollowsQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> following = GetUserFollowingAsync(bearer_token, client_id, from_id, query_parameters, settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowersPage(string bearer_token, string to_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> followers = GetUserFollowersPageAsync(bearer_token, to_id, settings).Result;

            return followers;
        }

        /// <summary>
        /// Gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowersPage(string bearer_token, string to_id, FollowsQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> followers = GetUserFollowersPageAsync(bearer_token, to_id, query_parameters).Result;

            return followers;
        }

        /// <summary>
        /// Gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowersPage(string bearer_token, string client_id, string to_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> followers = GetUserFollowersPageAsync(bearer_token, client_id, to_id, settings).Result;
            return followers;
        }

        /// <summary>
        /// Gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowersPage(string bearer_token, string client_id, string to_id, FollowsQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> followers = GetUserFollowersPageAsync(bearer_token, client_id, to_id, query_parameters, settings).Result;
            return followers;
        }

        /// <summary>
        /// Gets a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowers(string bearer_token, string to_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> followers = GetUserFollowersAsync(bearer_token, to_id, settings).Result;

            return followers;
        }

        /// <summary>
        /// Gets a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowers(string bearer_token, string to_id, FollowsQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> followers = GetUserFollowersAsync(bearer_token, to_id, query_parameters, settings).Result;

            return followers;
        }

        /// <summary>
        /// Gets a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowers(string bearer_token, string client_id, string to_id, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> followers = GetUserFollowersAsync(bearer_token, client_id, to_id, settings).Result;

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<FollowsDataPage<Follow>>
        GetUserFollowers(string bearer_token, string client_id, string to_id, FollowsQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> followers = GetUserFollowersAsync(bearer_token, client_id, to_id, query_parameters, settings).Result;

            return followers;
        }

        #endregion

        #region /videos

        /// <summary>
        /// Gets a single page of information on one or more videos.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Video>>
        GetVideosPage(string bearer_token, VideosQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Video>> videos = GetVideosPageAsync(bearer_token, query_parameters, settings).Result;

            return videos;
        }

        /// <summary>
        /// Gets a single page of information on one or more videos.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Video>>
        GetVideosPage(string bearer_token, string client_id, VideosQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Video>> videos = GetVideosPageAsync(bearer_token, client_id, query_parameters, settings).Result;

            return videos;
        }

        /// <summary>
        /// Gets a complete list of information on one or more videos.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Video>>
        GetVideos(string bearer_token, VideosQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Video>> videos = GetVideosAsync(bearer_token, query_parameters, settings).Result;

            return videos;
        }

        /// <summary>
        /// Gets a complete list of information on one or more videos.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static IApiResponse<DataPage<Video>>
        GetVideos(string bearer_token, string client_id, VideosQueryParameters query_parameters, ApiRequestSettings settings = settings_default)
        {
            IApiResponse<DataPage<Video>> videos = GetVideosAsync(bearer_token, client_id, query_parameters, settings).Result;

            return videos;
        }

        #endregion
    }
}
