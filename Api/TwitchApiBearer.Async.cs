// standard namespaces
using System.Threading.Tasks;

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
        #region Fields

        private static readonly string              client_id_default               = string.Empty;

        private const           ApiRequestSettings  api_request_settings_default    = default(ApiRequestSettings);

        #endregion

        #region /clips

        /// <summary>
        /// <para>Asynchronously creates a clip.</para>
        /// <para>Required Scope: 'clips:edit'.</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Data<CreatedClip>>>
        CreateClipAsync(string bearer_token, ClipCreationQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Data<CreatedClip>> clip = await CreateClipAsync(bearer_token, client_id_default, query_parameters, api_request_settings);

            return clip;
        }

        /// <summary>
        /// <para>Asynchronously creates a clip.</para>
        /// <para>Required Scope: 'clips:edit'.</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Data<CreatedClip>>>
        CreateClipAsync(string bearer_token, string client_id, ClipCreationQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Data<CreatedClip>> clip = await TwitchApiInternal.CreateClipAsync(bearer_token, client_id, query_parameters, api_request_settings);

            return clip;
        }

        /// <summary>
        /// Asynchronously gets information about a clip.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Data<Clip>>>
        GetClipAsync(string bearer_token, ClipQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Data<Clip>> clip = await GetClipAsync(bearer_token, client_id_default, query_parameters, api_request_settings);

            return clip;
        }

        /// <summary>
        /// Asynchronously gets information about a clip.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Data<Clip>>>
        GetClipAsync(string bearer_token, string client_id, ClipQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Data<Clip>> clip = await TwitchApiInternal.GetClipAsync(bearer_token, client_id, query_parameters, api_request_settings);

            return clip;
        }

        #endregion

        #region /entitlements/uploads

        /// <summary>
        /// <para>Asynchronously creates a URL where you can upload a manifest file and notify users that they have an entitlement.</para>
        /// <para>Required Authorization: Application Access Token</para>
        /// </summary>
        /// <param name="application_token">The application access token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Data<Url>>>
        CreateEntitlementGrantsUploadUrlAsync(string application_token, EntitlementQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Data<Url>> url = await CreateEntitlementGrantsUploadUrlAsync(application_token, client_id_default, query_parameters, api_request_settings);

            return url;
        }

        /// <summary>
        /// <para>Asynchronously creates a URL where you can upload a manifest file and notify users that they have an entitlement.</para>
        /// <para>Required Authorization: Application Access Token</para>
        /// </summary>
        /// <param name="application_token">The application access token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Data<Url>>>
        CreateEntitlementGrantsUploadUrlAsync(string application_token, string client_id, EntitlementQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Data<Url>> url = await TwitchApiInternal.CreateEntitlementGrantsUploadUrlAsync(application_token, client_id, query_parameters, api_request_settings);

            return url;
        }

        #endregion

        #region /games

        /// <summary>
        /// Asynchronously gets information about a list of games.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Data<Game>>>
        GetGamesAsync(string bearer_token, GamesQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Data<Game>> games = await GetGamesAsync(bearer_token, client_id_default, query_parameters, api_request_settings);

            return games;
        }

        /// <summary>
        /// Asynchronously gets information about a list of games.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Data<Game>>>
        GetGamesAsync(string bearer_token, string client_id, GamesQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Data<Game>> games = await TwitchApiInternal.GetGamesAsync(bearer_token, client_id, query_parameters, api_request_settings);

            return games;
        }

        #endregion

        #region /games/top

        /// <summary>
        /// Asynchronously gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Game>>>
        GetTopGamesPageAsync(string bearer_token, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Game>> top_games = await GetTopGamesPageAsync(bearer_token, default(TopGamesQueryParameters), api_request_settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Game>>>
        GetTopGamesPageAsync(string bearer_token, TopGamesQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Game>> top_games = await GetTopGamesPageAsync(bearer_token, client_id_default, query_parameters, api_request_settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Game>>>
        GetTopGamesPageAsync(string bearer_token, string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Game>> top_games = await GetTopGamesPageAsync(bearer_token, client_id, default(TopGamesQueryParameters), api_request_settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Game>>>
        GetTopGamesPageAsync(string bearer_token, string client_id, TopGamesQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Game>> top_games = await TwitchApiInternal.GetTopGamesPageAsync(bearer_token, client_id, query_parameters, api_request_settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Game>>>
        GetTopGamesAsync(string bearer_token, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Game>> top_games = await GetTopGamesAsync(bearer_token, default(TopGamesQueryParameters), api_request_settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Game>>>
        GetTopGamesAsync(string bearer_token, TopGamesQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Game>> top_games = await GetTopGamesAsync(bearer_token, client_id_default, query_parameters, api_request_settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Game>>>
        GetTopGamesAsync(string bearer_token, string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Game>> top_games = await GetTopGamesAsync(bearer_token, client_id, default(TopGamesQueryParameters), api_request_settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Game>>>
        GetTopGamesAsync(string bearer_token, string client_id, TopGamesQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Game>> top_games = await TwitchApiInternal.GetTopGamesAsync(bearer_token, client_id, query_parameters, api_request_settings);

            return top_games;
        }

        #endregion

        #region /streams

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Stream>>>
        GetStreamsPageAsync(string bearer_token, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Stream>> streams = await GetStreamsPageAsync(bearer_token, default(StreamsQueryParameters), api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Stream>>>
        GetStreamsPageAsync(string bearer_token, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Stream>> streams = await GetStreamsPageAsync(bearer_token, client_id_default, query_parameters, api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Stream>>>
        GetStreamsPageAsync(string bearer_token, string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Stream>> streams = await GetStreamsPageAsync(bearer_token, client_id, default(StreamsQueryParameters), api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Stream>>>
        GetStreamsPageAsync(string bearer_token,string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Stream>> streams = await TwitchApiInternal.GetStreamsPageAsync(bearer_token, client_id, query_parameters, api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Stream>>>
        GetStreamsAsync(string bearer_token, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Stream>> streams = await GetStreamsAsync(bearer_token, default(StreamsQueryParameters), api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Stream>>>
        GetStreamsAsync(string bearer_token, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Stream>> streams = await GetStreamsAsync(bearer_token, client_id_default, query_parameters, api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Stream>>>
        GetStreamsAsync(string bearer_token, string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Stream>> streams = await GetStreamsAsync(bearer_token, client_id, default(StreamsQueryParameters), api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Stream>>>
        GetStreamsAsync(string bearer_token, string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Stream>> streams = await TwitchApiInternal.GetStreamsAsync(bearer_token, client_id, query_parameters, api_request_settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously checks to see if a user is streaming.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="user_id">The user to check if they are live.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<bool>>
        IsStreamLiveAsync(string bearer_token, string user_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<bool> is_live = await IsstreamLiveAsync(bearer_token, string.Empty, user_id, api_request_settings);

            return is_live;
        }

        /// <summary>
        /// Asynchronously checks to see if a user is streaming.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="user_id">The user to check if they are live.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<bool>>
        IsstreamLiveAsync(string bearer_token, string client_id, string user_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<bool> is_live = await TwitchApiInternal.IsStreamLiveAsync(bearer_token, client_id, user_id, api_request_settings);

            return is_live;
        }

        #endregion

        #region /streams/metadata

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Metadata>>>
        GetStreamsMetadataPageAsync(string bearer_token, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Metadata>> metadata = await GetStreamsMetadataPageAsync(bearer_token, default(StreamsQueryParameters), api_request_settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Metadata>>>
        GetStreamsMetadataPageAsync(string bearer_token, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Metadata>> metadata = await GetStreamsMetadataPageAsync(bearer_token, client_id_default, query_parameters, api_request_settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Metadata>>>
        GetStreamsMetadataPageAsync(string bearer_token, string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Metadata>> metadata = await GetStreamsMetadataPageAsync(bearer_token, client_id, default(StreamsQueryParameters), api_request_settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Metadata>>>
        GetStreamsMetadataPageAsync(string bearer_token, string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Metadata>> metadata = await TwitchApiInternal.GetStreamsMetadataPageAsync(bearer_token, client_id, query_parameters, api_request_settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Metadata>>>
        GetStreamsMetadataAsync(string bearer_token, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Metadata>> metadata = await GetStreamsMetadataAsync(bearer_token, default(StreamsQueryParameters), api_request_settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Metadata>>>
        GetStreamsMetadataAsync(string bearer_token, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Metadata>> metadata = await GetStreamsMetadataAsync(bearer_token, client_id_default, query_parameters, api_request_settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Metadata>>>
        GetStreamsMetadataAsync(string bearer_token, string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Metadata>> metadata = await GetStreamsMetadataAsync(bearer_token, client_id, default(StreamsQueryParameters), api_request_settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Metadata>>>
        GetStreamsMetadataAsync(string bearer_token, string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Metadata>> metadata = await TwitchApiInternal.GetStreamsMetadataAsync(bearer_token, client_id, query_parameters, api_request_settings);

            return metadata;
        }

        #endregion

        #region /users

        /// <summary>
        /// <para>Asynchronously gets the information of the user looked up by the provided Bearer token.</para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Data<User>>>
        GetUserAsync(string bearer_token, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Data<User>> users = await GetUserAsync(bearer_token, client_id_default, api_request_settings);

            return users;
        }

        /// <summary>
        /// <para>Asynchronously gets the information of the user looked up by the provided Bearer token.</para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Data<User>>>
        GetUserAsync(string bearer_token, string client_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Data<User>> users = await GetUsersAsync(bearer_token, client_id, null, api_request_settings);

            return users;
        }

        /// <summary>
        /// <para>
        /// Asynchronously gets the information of one or more users by their id or login.
        /// If no <paramref name="ids"/> are specified, the user is looked up by if the Bearer token provided.
        /// </para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Data<User>>>
        GetUsersAsync(string bearer_token, UsersQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Data<User>> users = await GetUsersAsync(bearer_token, client_id_default, query_parameters, api_request_settings);

            return users;
        }

        /// <summary>
        /// <para>
        /// Asynchronously gets the information of one or more users by their id or login.
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
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Data<User>>>
        GetUsersAsync(string bearer_token, string client_id, UsersQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Data<User>> users = await TwitchApiInternal.GetUsersAsync(bearer_token, client_id, query_parameters, api_request_settings);

            return users;
        }

        /// <summary>
        /// <para>Asynchronously sets the description of a user specified by the Bearer token provided.</para>
        /// <para>Required Scope: 'user:edit'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="description">The new description to set.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Data<User>>>
        SetUserDescriptionAsync(string bearer_token, string description, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Data<User>> success = await SetUserDescriptionAsync(bearer_token, client_id_default, description, api_request_settings);

            return success;
        }

        /// <summary>
        /// <para>Asynchronously sets the description of a user specified by the Bearer token provided.</para>
        /// <para>Required Scope: 'user:edit'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="description">The new description to set.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<Data<User>>>
        SetUserDescriptionAsync(string bearer_token, string client_id, string description, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<Data<User>> result = await TwitchApiInternal.SetUserDescriptionAsync(bearer_token, client_id, description, api_request_settings);

            return result;
        }

        #endregion

        #region /users/follows

        /// <summary>
        /// Asynchronously gets the relationship between two users.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserRelationshipAsync(string bearer_token, string from_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> relationship = await GetUserRelationshipAsync(bearer_token, client_id_default, from_id, to_id, api_request_settings);

            return relationship;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserRelationshipAsync(string bearer_token, string client_id, string from_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> relationship = await TwitchApiInternal.GetUserRelationshipPageAsync(bearer_token, client_id, from_id, to_id, default(FollowsQueryParameters), api_request_settings);

            return relationship;
        }

        /// <summary>
        /// Asynchronously checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<bool>>
        IsUserFollowingAsync(string bearer_token, string from_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<bool> is_following = await IsUserFollowingAsync(bearer_token, client_id_default, from_id, to_id, api_request_settings);

            return is_following;
        }

        /// <summary>
        /// Asynchronously checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<bool>>
        IsUserFollowingAsync(string bearer_token, string client_id, string from_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<bool> is_following = await TwitchApiInternal.IsUserFollowingAsync(bearer_token, client_id, from_id, to_id, api_request_settings);

            return is_following;
        }

        /// <summary>
        /// Asynchronously gets a single page of a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserFollowingPageAsync(string bearer_token, string from_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> following = await GetUserFollowingPageAsync(bearer_token, from_id, default(FollowsQueryParameters), api_request_settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a single page of a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserFollowingPageAsync(string bearer_token, string from_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> following = await GetUserFollowingPageAsync(bearer_token, client_id_default, from_id, query_parameters, api_request_settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a single page of a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserFollowingPageAsync(string bearer_token, string client_id, string from_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> following = await GetUserFollowingPageAsync(bearer_token, client_id, from_id, default(FollowsQueryParameters), api_request_settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a single page of a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserFollowingPageAsync(string bearer_token, string client_id, string from_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> following = await TwitchApiInternal.GetUserRelationshipPageAsync(bearer_token, client_id, from_id, string.Empty, query_parameters, api_request_settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserFollowingAsync(string bearer_token, string from_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> following = await GetUserFollowingAsync(bearer_token, from_id, default(FollowsQueryParameters), api_request_settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserFollowingAsync(string bearer_token, string from_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> following = await GetUserFollowingAsync(bearer_token, client_id_default, from_id, query_parameters, api_request_settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserFollowingAsync(string bearer_token, string client_id, string from_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> following = await GetUserFollowingAsync(bearer_token, client_id, from_id, default(FollowsQueryParameters), api_request_settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserFollowingAsync(string bearer_token, string client_id, string from_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> following = await TwitchApiInternal.GetUserRelationshipAsync(bearer_token, client_id, from_id, string.Empty, query_parameters, api_request_settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserFollowersPageAsync(string bearer_token, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> followers = await GetUserFollowersPageAsync(bearer_token, to_id, default(FollowsQueryParameters), api_request_settings);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserFollowersPageAsync(string bearer_token, string to_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> followers = await GetUserFollowersPageAsync(bearer_token, client_id_default, to_id, query_parameters);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserFollowersPageAsync(string bearer_token, string client_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> followers = await GetUserFollowersPageAsync(bearer_token, client_id, to_id, default(FollowsQueryParameters), api_request_settings);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserFollowersPageAsync(string bearer_token, string client_id, string to_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> followers = await TwitchApiInternal.GetUserRelationshipPageAsync(bearer_token, client_id, string.Empty, to_id, query_parameters, api_request_settings);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserFollowersAsync(string bearer_token, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> followers = await GetUserFollowersAsync(bearer_token, to_id, default(FollowsQueryParameters), api_request_settings);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserFollowersAsync(string bearer_token, string to_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> followers = await GetUserFollowersAsync(bearer_token, client_id_default, to_id, query_parameters, api_request_settings);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserFollowersAsync(string bearer_token, string client_id, string to_id, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> followers = await GetUserFollowersAsync(bearer_token, client_id, to_id, default(FollowsQueryParameters), api_request_settings);

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
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserFollowersAsync(string bearer_token, string client_id, string to_id, FollowsQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<FollowsDataPage<Follow>> followers = await TwitchApiInternal.GetUserRelationshipAsync(bearer_token, client_id, string.Empty, to_id, query_parameters, api_request_settings);

            return followers;
        }

        #endregion        

        #region /videos

        /// <summary>
        /// Asynchronously gets a single page of information on one or more videos.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Video>>>
        GetVideosPageAsync(string bearer_token, VideosQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Video>> videos = await GetVideosPageAsync(bearer_token, client_id_default, query_parameters, api_request_settings);

            return videos;
        }

        /// <summary>
        /// Asynchronously gets a single page of information on one or more videos.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Video>>>
        GetVideosPageAsync(string bearer_token, string client_id, VideosQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Video>> videos = await TwitchApiInternal.GetVideosPageAsync(bearer_token, client_id, query_parameters, api_request_settings);

            return videos;
        }

        /// <summary>
        /// Asynchronously gets a complete list of information on one or more videos.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Video>>>
        GetVideosAsync(string bearer_token, VideosQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Video>> videos = await GetVideosAsync(bearer_token, client_id_default, query_parameters, api_request_settings);

            return videos;
        }

        /// <summary>
        /// Asynchronously gets a complete list of information on one or more videos.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="api_request_settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        public static async Task<IApiResponse<DataPage<Video>>>
        GetVideosAsync(string bearer_token, string client_id, VideosQueryParameters query_parameters, ApiRequestSettings api_request_settings = api_request_settings_default)
        {
            IApiResponse<DataPage<Video>> videos = await TwitchApiInternal.GetVideosAsync(bearer_token, client_id, query_parameters, api_request_settings);

            return videos;
        }

        #endregion
    }
}