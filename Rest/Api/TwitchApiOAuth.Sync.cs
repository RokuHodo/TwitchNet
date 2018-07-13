// project namespaces
using TwitchNet.Rest.Api.Analytics;
using TwitchNet.Rest.Api.Bits;
using TwitchNet.Rest.Api.Clips;
using TwitchNet.Rest.Api.Entitlements;
using TwitchNet.Rest.Api.Games;
using TwitchNet.Rest.Api.Streams;
using TwitchNet.Rest.Api.Users;
using TwitchNet.Rest.Api.Videos;

namespace
TwitchNet.Rest.Api
{
    public static partial class
    TwitchApiBearer
    {
        #region /analytics/extensions

        /// <summary>
        /// <para>Gets analytic urls for one or more devloper extensions.</para>
        /// <para>Required Scope: 'analytics:read:extensions'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<ExtensionAnalytics>>
        GetExtensionAnalytics(string bearer_token, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<Data<ExtensionAnalytics>> analytics = GetExtensionAnalyticsAsync(bearer_token, settings).Result;

            return analytics;
        }

        /// <summary>
        /// <para>Gets analytic urls for one or more devloper extensions.</para>
        /// <para>Required Scope: 'analytics:read:extensions'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<ExtensionAnalytics>>
        GetExtensionAnalytics(string bearer_token, ExtensionAnalyticsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<Data<ExtensionAnalytics>> analytics = GetExtensionAnalyticsAsync(bearer_token, parameters, settings).Result;

            return analytics;
        }

        /// <summary>
        /// <para>Gets analytic urls for one or more devloper extensions.</para>
        /// <para>Required Scope: 'analytics:read:extensions'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<ExtensionAnalytics>>
        GetExtensionAnalytics(string bearer_token, string client_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<Data<ExtensionAnalytics>> analytics = GetExtensionAnalyticsAsync(bearer_token, client_id, settings).Result;

            return analytics;
        }

        /// <summary>
        /// <para>Gets analytic urls for one or more devloper extensions.</para>
        /// <para>Required Scope: 'analytics:read:extensions'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<ExtensionAnalytics>>
        GetExtensionAnalytics(string bearer_token, string client_id, ExtensionAnalyticsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<Data<ExtensionAnalytics>> analytics = GetExtensionAnalyticsAsync(bearer_token, client_id, parameters, settings).Result;

            return analytics;
        }

        #endregion

        #region /analytics/games

        /// <summary>
        /// <para>Gets a single page of analytic urls for one or more devloper games.</para>
        /// <para>Required Scope: 'analytics:read:games'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<GameAnalytics>>
        GetGameAnalyticsPage(string bearer_token, RequestSettings settings = null)
        {
            IHelixResponse<Data<GameAnalytics>> respose = GetGameAnalyticsPageAsync(bearer_token, settings).Result;

            return respose;
        }

        /// <summary>
        /// <para>Gets a single page of analytic urls for one or more devloper games.</para>
        /// <para>Required Scope: 'analytics:read:games'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<GameAnalytics>>
        GetGameAnalyticsPage(string bearer_token, GameAnalyticsParameters parameters, RequestSettings settings = null)
        {
            IHelixResponse<Data<GameAnalytics>> respose = GetGameAnalyticsPageAsync(bearer_token, parameters, settings).Result;

            return respose;
        }

        /// <summary>
        /// <para>Gets a single page of analytic urls for one or more devloper games.</para>
        /// <para>Required Scope: 'analytics:read:games'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<GameAnalytics>>
        GetGameAnalyticsPage(string bearer_token, string client_id, RequestSettings settings = null)
        {
            IHelixResponse<Data<GameAnalytics>> respose = GetGameAnalyticsPageAsync(bearer_token, client_id, settings).Result;

            return respose;
        }

        /// <summary>
        /// <para>Gets a single page of analytic urls for one or more devloper games.</para>
        /// <para>Required Scope: 'analytics:read:games'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<GameAnalytics>>
        GetGameAnalyticsPage(string bearer_token, string client_id, GameAnalyticsParameters parameters, RequestSettings settings = null)
        {
            IHelixResponse<Data<GameAnalytics>> respose = GetGameAnalyticsPageAsync(bearer_token, client_id, parameters, settings).Result;

            return respose;
        }

        /// <summary>
        /// <para>Gets a complete list of analytic urls for one or more devloper games.</para>
        /// <para>Required Scope: 'analytics:read:games'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<GameAnalytics>>
        GetGameAnalytics(string bearer_token, RequestSettings settings = null)
        {
            IHelixResponse<DataPage<GameAnalytics>> respose = GetGameAnalyticsAsync(bearer_token, settings).Result;

            return respose;
        }

        /// <summary>
        /// <para>Gets a complete list of analytic urls for one or more devloper games.</para>
        /// <para>Required Scope: 'analytics:read:games'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<GameAnalytics>>
        GetGameAnalytics(string bearer_token, GameAnalyticsParameters parameters, RequestSettings settings = null)
        {
            IHelixResponse<DataPage<GameAnalytics>> respose = GetGameAnalyticsAsync(bearer_token, parameters, settings).Result;

            return respose;
        }

        /// <summary>
        /// <para>Gets a complete list of analytic urls for one or more devloper games.</para>
        /// <para>Required Scope: 'analytics:read:games'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<GameAnalytics>>
        GetGameAnalytics(string bearer_token, string client_id, RequestSettings settings = null)
        {
            IHelixResponse<DataPage<GameAnalytics>> respose = GetGameAnalyticsAsync(bearer_token, client_id, settings).Result;

            return respose;
        }

        /// <summary>
        /// <para>Gets a complete list of analytic urls for one or more devloper games.</para>
        /// <para>Required Scope: 'analytics:read:games'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<GameAnalytics>>
        GetGameAnalytics(string bearer_token, string client_id, GameAnalyticsParameters parameters, RequestSettings settings = null)
        {
            IHelixResponse<DataPage<GameAnalytics>> respose = GetGameAnalyticsAsync(bearer_token, client_id, parameters, settings).Result;

            return respose;
        }

        #endregion

        #region bits/leaderboard

        /// <summary>
        /// <para>Gets a ranked list of bits leaderboard information for an authorized broadcaster.</para>
        /// <para>Required Scope: 'bits:read'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<BitsLeaderboardData<BitsUser>>
        GetBitsLeaderboard(string bearer_token, RequestSettings settings = null)
        {
            IHelixResponse<BitsLeaderboardData<BitsUser>> respose = GetBitsLeaderboardAsync(bearer_token, settings).Result;

            return respose;
        }

        /// <summary>
        /// <para>Gets a ranked list of bits leaderboard information for an authorized broadcaster.</para>
        /// <para>Required Scope: 'bits:read'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<BitsLeaderboardData<BitsUser>>
        GetBitsLeaderboard(string bearer_token, BitsLeaderboardParameters parameters, RequestSettings settings = null)
        {
            IHelixResponse<BitsLeaderboardData<BitsUser>> respose = GetBitsLeaderboardAsync(bearer_token, parameters, settings).Result;

            return respose;
        }

        /// <summary>
        /// <para>Gets a ranked list of bits leaderboard information for an authorized broadcaster.</para>
        /// <para>Required Scope: 'bits:read'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<BitsLeaderboardData<BitsUser>>
        GetBitsLeaderboard(string bearer_token, string client_id, RequestSettings settings = null)
        {
            IHelixResponse<BitsLeaderboardData<BitsUser>> respose = GetBitsLeaderboardAsync(bearer_token, client_id, settings).Result;

            return respose;
        }

        /// <summary>
        /// <para>Gets a ranked list of bits leaderboard information for an authorized broadcaster.</para>
        /// <para>Required Scope: 'bits:read'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<BitsLeaderboardData<BitsUser>>
        GetBitsLeaderboard(string bearer_token, string client_id, BitsLeaderboardParameters parameters, RequestSettings settings = null)
        {
            IHelixResponse<BitsLeaderboardData<BitsUser>> respose = GetBitsLeaderboardAsync(bearer_token, client_id, parameters, settings).Result;

            return respose;
        }

        #endregion

        #region /clips

        /// <summary>
        /// <para>Creates a clip.</para>
        /// <para>Required Scope: 'clips:edit'.</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<CreatedClip>>
        CreateClip(string bearer_token, ClipCreationParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<Data<CreatedClip>> clip = CreateClipAsync(bearer_token, parameters, settings).Result;

            return clip;
        }

        /// <summary>
        /// <para>Creates a clip.</para>
        /// <para>Required Scope: 'clips:edit'.</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<CreatedClip>>
        CreateClip(string bearer_token, string client_id, ClipCreationParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<Data<CreatedClip>> clip = CreateClipAsync(bearer_token, client_id, parameters, settings).Result;

            return clip;
        }

        /// <summary>
        /// Gets information about a clip.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<Clip>>
        GetClip(string bearer_token, ClipParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<Data<Clip>> clip = GetClipAsync(bearer_token, parameters, settings).Result;

            return clip;
        }

        /// <summary>
        /// Gets information about a clip.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<Clip>>
        GetClip(string bearer_token, string client_id, ClipParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<Data<Clip>> clip = GetClipAsync(bearer_token, client_id, parameters, settings).Result;

            return clip;
        }

        #endregion

        #region /entitlements/upload

        /// <summary>
        /// <para>Creates a URL where you can upload a manifest file and notify users that they have an entitlement.</para>
        /// <para>Required Authorization: Application Access Token</para>
        /// </summary>
        /// <param name="application_token">The application access token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<Url>>
        CreateEntitlementGrantsUploadUrl(string application_token, EntitlementParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<Data<Url>> url = CreateEntitlementGrantsUploadUrlAsync(application_token, parameters, settings).Result;

            return url;
        }

        /// <summary>
        /// <para>Creates a URL where you can upload a manifest file and notify users that they have an entitlement.</para>
        /// <para>Required Authorization: Application Access Token</para>
        /// </summary>
        /// <param name="application_token">The application access token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<Url>>
        CreateEntitlementGrantsUploadUrl(string application_token, string client_id, EntitlementParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<Data<Url>> url = CreateEntitlementGrantsUploadUrlAsync(application_token, client_id, parameters, settings).Result;

            return url;
        }

        #endregion

        #region /games

        /// <summary>
        /// Gets information about a list of games.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<Game>>
        GetGames(string bearer_token, GamesParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<Data<Game>> games = GetGamesAsync(bearer_token, parameters, settings).Result;

            return games;
        }

        /// <summary>
        /// Gets information about a list of games.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<Game>>
        GetGames(string bearer_token, string client_id, GamesParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<Data<Game>> games = GetGamesAsync(bearer_token, client_id, parameters, settings).Result;

            return games;
        }

        #endregion

        #region /games/top

        /// <summary>
        /// Gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Game>>
        GetTopGamesPage(string bearer_token, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Game>> top_games = GetTopGamesPageAsync(bearer_token, settings).Result;

            return top_games;
        }

        /// <summary>
        /// Gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Game>>
        GetTopGamesPage(string bearer_token, TopGamesParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Game>> top_games = GetTopGamesPageAsync(bearer_token, parameters, settings).Result;

            return top_games;
        }

        /// <summary>
        /// Gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Game>>
        GetTopGamesPage(string bearer_token, string client_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Game>> top_games = GetTopGamesPageAsync(bearer_token, client_id, settings).Result;

            return top_games;
        }

        /// <summary>
        /// Gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Game>>
        GetTopGamesPage(string bearer_token, string client_id, TopGamesParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Game>> top_games = GetTopGamesPageAsync(bearer_token, client_id, parameters, settings).Result;

            return top_games;
        }

        /// <summary>
        /// Gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Game>>
        GetTopGames(string bearer_token, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Game>> top_games = GetTopGamesAsync(bearer_token, settings).Result;

            return top_games;
        }

        /// <summary>
        /// Gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Game>>
        GetTopGames(string bearer_token, TopGamesParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Game>> top_games = GetTopGamesAsync(bearer_token, parameters, settings).Result;

            return top_games;
        }

        /// <summary>
        /// Gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Game>>
        GetTopGames(string bearer_token, string client_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Game>> top_games = GetTopGamesAsync(bearer_token, client_id, settings).Result;

            return top_games;
        }

        /// <summary>
        /// Gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Game>>
        GetTopGames(string bearer_token, string client_id, TopGamesParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Game>> top_games = GetTopGamesAsync(bearer_token, client_id, parameters, settings).Result;

            return top_games;
        }

        #endregion

        #region /streams

        /// <summary>
        /// Gets a single page of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Stream>>
        GetStreamsPage(string bearer_token, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Stream>> streams = GetStreamsPageAsync(bearer_token, settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a single page of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Stream>>
        GetStreamsPage(string bearer_token, StreamsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Stream>> streams = GetStreamsPageAsync(bearer_token, parameters, settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a single page of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Stream>>
        GetStreamsPage(string bearer_token, string client_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Stream>> streams = GetStreamsPageAsync(bearer_token, client_id, settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a single page of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Stream>>
        GetStreamsPage(string bearer_token, string client_id, StreamsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Stream>> streams = GetStreamsPageAsync(bearer_token, client_id, parameters, settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a complete list of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Stream>>
        GetStreams(string bearer_token, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Stream>> streams = GetStreamsAsync(bearer_token, settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a complete list of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Stream>>
        GetStreams(string bearer_token, StreamsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Stream>> streams = GetStreamsAsync(bearer_token, parameters, settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a complete list of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Stream>>
        GetStreams(string bearer_token, string client_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Stream>> streams = GetStreamsAsync(bearer_token, client_id, settings).Result;

            return streams;
        }

        /// <summary>
        /// Gets a complete list of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Stream>>
        GetStreams(string bearer_token, string client_id, StreamsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Stream>> streams = GetStreamsAsync(bearer_token, client_id, parameters, settings).Result;

            return streams;
        }        

        /// <summary>
        /// Checks to see if a user is streaming.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="user_id">The user to check if they are live.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<bool>
        IsStreamLive(string bearer_token, string user_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<bool> is_live = IsStreamLiveAsync(bearer_token, user_id, settings).Result;

            return is_live;
        }

        /// <summary>
        /// Checks to see if a user is streaming.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="user_id">The user to check if they are live.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<bool>
        IsStreamLive(string bearer_token, string client_id, string user_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<bool> is_live = IsstreamLiveAsync(bearer_token, client_id, user_id, settings).Result;

            return is_live;
        }

        #endregion

        #region /streams/metadata

        /// <summary>
        /// Gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Metadata>>
        GetStreamsMetadataPage(string bearer_token, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Metadata>> metadata = GetStreamsMetadataPageAsync(bearer_token, settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Metadata>>
        GetStreamsMetadataPage(string bearer_token, StreamsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Metadata>> metadata = GetStreamsMetadataPageAsync(bearer_token, parameters, settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Metadata>>
        GetStreamsMetadataPage(string bearer_token, string client_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Metadata>> metadata = GetStreamsMetadataPageAsync(bearer_token, client_id, settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Metadata>>
        GetStreamsMetadataPage(string bearer_token, string client_id, StreamsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Metadata>> metadata = GetStreamsMetadataPageAsync(bearer_token, client_id, parameters, settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Metadata>>
        GetStreamsMetadata(string bearer_token, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Metadata>> metadata = GetStreamsMetadataAsync(bearer_token, settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Metadata>>
        GetStreamsMetadata(string bearer_token, StreamsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Metadata>> metadata = GetStreamsMetadataAsync(bearer_token, parameters, settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Metadata>>
        GetStreamsMetadata(string bearer_token, string client_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Metadata>> metadata = GetStreamsMetadataAsync(bearer_token, client_id, settings).Result;

            return metadata;
        }

        /// <summary>
        /// Gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Metadata>>
        GetStreamsMetadata(string bearer_token, string client_id, StreamsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Metadata>> metadata = GetStreamsMetadataAsync(bearer_token, client_id, parameters, settings).Result;

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
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<User>>
        GetUser(string bearer_token, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<Data<User>> users = GetUserAsync(bearer_token, settings).Result;

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
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<User>>
        GetUser(string bearer_token, string client_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<Data<User>> users = GetUserAsync(bearer_token, client_id, settings).Result;

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
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<User>>
        GetUsers(string bearer_token, UsersParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<Data<User>> users = GetUsersAsync(bearer_token, parameters, settings).Result;

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
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<User>>
        GetUsers(string bearer_token, string client_id, UsersParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<Data<User>> users = GetUsersAsync(bearer_token, client_id, parameters, settings).Result;

            return users;
        }

        /// <summary>
        /// <para>Sets the description of a user specified by the Bearer token provided.</para>
        /// <para>Required Scope: 'user:edit'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="description">The new description to set.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<User>>
        SetUserDescription(string bearer_token, string description, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<Data<User>> success = SetUserDescriptionAsync(bearer_token, description, settings).Result;

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
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<User>>
        SetUserDescription(string bearer_token, string client_id, string description, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<Data<User>> success = SetUserDescriptionAsync(bearer_token, client_id, description, settings).Result;

            return success;
        }

        #endregion

        #region /users/extensions

        /// <summary>
        /// <para>
        /// Gets a list of active extensions installed by a user.
        /// The user is identified either by user ID or by the provided Bearer token.
        /// </para>
        /// <para>Optional Scope: <see cref="Scopes.UserReadBroadcast"/> or <see cref="Scopes.UserEditBroadcast"/>.</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<ActiveExtensionsData>
        GetUserActiveExtensions(string bearer_token, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<ActiveExtensionsData> response = GetUserActiveExtensionsAsync(bearer_token, settings).Result;

            return response;
        }

        /// <summary>
        /// <para>
        /// Asynchronously gets a list of active extensions installed by a user.
        /// The user is identified either by user ID or by the provided Bearer token.
        /// </para>
        /// <para>Optional Scope: <see cref="Scopes.UserReadBroadcast"/> or <see cref="Scopes.UserEditBroadcast"/>.</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<ActiveExtensionsData>
        GetUserActiveExtensions(string bearer_token, string client_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<ActiveExtensionsData> response = GetUserActiveExtensionsAsync(bearer_token, client_id, settings).Result;

            return response;
        }

        /// <summary>
        /// <para>
        /// Gets a list of active extensions installed by a user.
        /// The user is identified either by user ID or by the provided Bearer token.
        /// </para>
        /// <para>Optional Scope: <see cref="Scopes.UserReadBroadcast"/> or <see cref="Scopes.UserEditBroadcast"/>.</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<ActiveExtensionsData>
        GetUserActiveExtensions(string bearer_token, ActiveExtensionsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<ActiveExtensionsData> response = GetUserActiveExtensionsAsync(bearer_token, parameters, settings).Result;

            return response;
        }

        /// <summary>
        /// <para>
        /// Gets a list of active extensions installed by a user.
        /// The user is identified either by user ID or by the provided Bearer token.
        /// </para>
        /// <para>Optional Scope: <see cref="Scopes.UserReadBroadcast"/> or <see cref="Scopes.UserEditBroadcast"/>.</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<ActiveExtensionsData>
        GetUserActiveExtensions(string bearer_token, string client_id, ActiveExtensionsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<ActiveExtensionsData> response = GetUserActiveExtensionsAsync(bearer_token, client_id, parameters, settings).Result;

            return response;
        }

        /// <summary>
        /// <para>
        /// Updates the active extensions for a user identified by a user ID or by the provided Bearer token.
        /// The activation state, extension ID, verison number, or x/y coordinates (components only) can be updated.
        /// </para>
        /// <para>Required Scope: <see cref="Scopes.UserEditBroadcast"/>.</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<ActiveExtensionsData>
        UpdateUserExtensions(string bearer_token, UpdateExtensionsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<ActiveExtensionsData> response = UpdateUserExtensionsAsync(bearer_token, parameters, settings).Result;

            return response;
        }

        /// <summary>
        /// <para>
        /// Updates the active extensions for a user identified by a user ID or by the provided Bearer token.
        /// The activation state, extension ID, verison number, or x/y coordinates (components only) can be updated.
        /// </para>
        /// <para>Required Scope: <see cref="Scopes.UserEditBroadcast"/>.</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<ActiveExtensionsData>
        UpdateUserExtensions(string bearer_token, string client_id, UpdateExtensionsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<ActiveExtensionsData> response = UpdateUserExtensionsAsync(bearer_token, client_id, parameters, settings).Result;

            return response;
        }

        #endregion

        #region /users/extensions/list

        /// <summary>
        /// <para>Gets a list of all extensions a user has installed, active or inactive.</para>
        /// <para>Required Scope: 'user:read:broadcast'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<Extension>>
        GetUserExtensions(string bearer_token, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<Data<Extension>> response = GetUserExtensionsAsync(bearer_token, settings).Result;

            return response;
        }

        /// <summary>
        /// <para>Asynchronously gets a list of all extensions a user has installed, active or inactive.</para>
        /// <para>Required Scope: 'user:read:broadcast'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<Data<Extension>>
        GetUserExtensions(string bearer_token, string client_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<Data<Extension>> response = GetUserExtensionsAsync(bearer_token, client_id, settings).Result;

            return response;
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
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<FollowsDataPage<Follow>>
        GetUserRelationship(string bearer_token, string from_id, string to_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<FollowsDataPage<Follow>> relationship = GetUserRelationshipAsync(bearer_token, from_id, to_id, settings).Result;

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
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<FollowsDataPage<Follow>>
        GetUserRelationship(string bearer_token, string client_id, string from_id, string to_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<FollowsDataPage<Follow>> relationship = GetUserRelationshipAsync(bearer_token, client_id, from_id, to_id, settings).Result;

            return relationship;
        }

        /// <summary>
        /// Checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<bool>
        IsUserFollowing(string bearer_token, string from_id, string to_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<bool> is_following = IsUserFollowingAsync(bearer_token, from_id, to_id, settings).Result;

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
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<bool>
        IsUserFollowing(string bearer_token, string client_id, string from_id, string to_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<bool> is_following = IsUserFollowingAsync(bearer_token, client_id, from_id, to_id, settings).Result;

            return is_following;
        }

        /// <summary>
        /// Gets a single page of a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<FollowsDataPage<Follow>>
        GetUserFollowingPage(string bearer_token, string from_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<FollowsDataPage<Follow>> following = GetUserFollowingPageAsync(bearer_token, from_id, settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a single page of a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see
        public static IHelixResponse<FollowsDataPage<Follow>>
        GetUserFollowingPage(string bearer_token, string from_id, FollowsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<FollowsDataPage<Follow>> following = GetUserFollowingPageAsync(bearer_token, from_id, parameters, settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a single page of a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<FollowsDataPage<Follow>>
        GetUserFollowingPage(string bearer_token, string client_id, string from_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<FollowsDataPage<Follow>> following = GetUserFollowingPageAsync(bearer_token, client_id, from_id, settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a single page of a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<FollowsDataPage<Follow>>
        GetUserFollowingPage(string bearer_token, string client_id, string from_id, FollowsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<FollowsDataPage<Follow>> following = GetUserFollowingPageAsync(bearer_token, client_id, from_id, parameters, settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<FollowsDataPage<Follow>>
        GetUserFollowing(string bearer_token, string from_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<FollowsDataPage<Follow>> following = GetUserFollowingAsync(bearer_token, from_id, settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<FollowsDataPage<Follow>>
        GetUserFollowing(string bearer_token, string from_id, FollowsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<FollowsDataPage<Follow>> following = GetUserFollowingAsync(bearer_token, from_id, parameters, settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<FollowsDataPage<Follow>>
        GetUserFollowing(string bearer_token, string client_id, string from_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<FollowsDataPage<Follow>> following = GetUserFollowingAsync(bearer_token, client_id, from_id, settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<FollowsDataPage<Follow>>
        GetUserFollowing(string bearer_token, string client_id, string from_id, FollowsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<FollowsDataPage<Follow>> following = GetUserFollowingAsync(bearer_token, client_id, from_id, parameters, settings).Result;

            return following;
        }

        /// <summary>
        /// Gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<FollowsDataPage<Follow>>
        GetUserFollowersPage(string bearer_token, string to_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<FollowsDataPage<Follow>> followers = GetUserFollowersPageAsync(bearer_token, to_id, settings).Result;

            return followers;
        }

        /// <summary>
        /// Gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<FollowsDataPage<Follow>>
        GetUserFollowersPage(string bearer_token, string to_id, FollowsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<FollowsDataPage<Follow>> followers = GetUserFollowersPageAsync(bearer_token, to_id, parameters).Result;

            return followers;
        }

        /// <summary>
        /// Gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<FollowsDataPage<Follow>>
        GetUserFollowersPage(string bearer_token, string client_id, string to_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<FollowsDataPage<Follow>> followers = GetUserFollowersPageAsync(bearer_token, client_id, to_id, settings).Result;
            return followers;
        }

        /// <summary>
        /// Gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<FollowsDataPage<Follow>>
        GetUserFollowersPage(string bearer_token, string client_id, string to_id, FollowsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<FollowsDataPage<Follow>> followers = GetUserFollowersPageAsync(bearer_token, client_id, to_id, parameters, settings).Result;
            return followers;
        }

        /// <summary>
        /// Gets a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<FollowsDataPage<Follow>>
        GetUserFollowers(string bearer_token, string to_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<FollowsDataPage<Follow>> followers = GetUserFollowersAsync(bearer_token, to_id, settings).Result;

            return followers;
        }

        /// <summary>
        /// Gets a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<FollowsDataPage<Follow>>
        GetUserFollowers(string bearer_token, string to_id, FollowsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<FollowsDataPage<Follow>> followers = GetUserFollowersAsync(bearer_token, to_id, parameters, settings).Result;

            return followers;
        }

        /// <summary>
        /// Gets a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<FollowsDataPage<Follow>>
        GetUserFollowers(string bearer_token, string client_id, string to_id, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<FollowsDataPage<Follow>> followers = GetUserFollowersAsync(bearer_token, client_id, to_id, settings).Result;

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<FollowsDataPage<Follow>>
        GetUserFollowers(string bearer_token, string client_id, string to_id, FollowsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<FollowsDataPage<Follow>> followers = GetUserFollowersAsync(bearer_token, client_id, to_id, parameters, settings).Result;

            return followers;
        }

        #endregion

        #region /videos

        /// <summary>
        /// Gets a single page of information on one or more videos.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Video>>
        GetVideosPage(string bearer_token, VideosParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Video>> videos = GetVideosPageAsync(bearer_token, parameters, settings).Result;

            return videos;
        }

        /// <summary>
        /// Gets a single page of information on one or more videos.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Video>>
        GetVideosPage(string bearer_token, string client_id, VideosParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Video>> videos = GetVideosPageAsync(bearer_token, client_id, parameters, settings).Result;

            return videos;
        }

        /// <summary>
        /// Gets a complete list of information on one or more videos.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Video>>
        GetVideos(string bearer_token, VideosParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Video>> videos = GetVideosAsync(bearer_token, parameters, settings).Result;

            return videos;
        }

        /// <summary>
        /// Gets a complete list of information on one or more videos.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static IHelixResponse<DataPage<Video>>
        GetVideos(string bearer_token, string client_id, VideosParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            IHelixResponse<DataPage<Video>> videos = GetVideosAsync(bearer_token, client_id, parameters, settings).Result;

            return videos;
        }

        #endregion
    }
}
