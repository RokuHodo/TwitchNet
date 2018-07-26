﻿// standard namespaces
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Extensions;
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
        /*
        // TODO: Test once I actually get around to making an extension.
        #region /analytics/extensions

        /// <summary>
        /// <para>Asynchronously gets analytic urls for one or more devloper extensions.</para>
        /// <para>Required Scope: 'analytics:read:extensions'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<ExtensionAnalytics>>>
        GetExtensionAnalyticsAsync(string bearer_token, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token = bearer_token;

            IHelixResponse<Data<ExtensionAnalytics>> analytics = await TwitchApiInternal.GetExtensionAnalyticsAsync(request_info, default(ExtensionAnalyticsParameters), settings);

            return analytics;
        }

        /// <summary>
        /// <para>Asynchronously gets analytic urls for one or more devloper extensions.</para>
        /// <para>Required Scope: 'analytics:read:extensions'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<ExtensionAnalytics>>>
        GetExtensionAnalyticsAsync(string bearer_token, ExtensionAnalyticsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token = bearer_token;

            IHelixResponse<Data<ExtensionAnalytics>> analytics = await TwitchApiInternal.GetExtensionAnalyticsAsync(request_info, parameters, settings);

            return analytics;
        }

        /// <summary>
        /// <para>Asynchronously gets analytic urls for one or more devloper extensions.</para>
        /// <para>Required Scope: 'analytics:read:extensions'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<ExtensionAnalytics>>>
        GetExtensionAnalyticsAsync(string bearer_token, string client_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<Data<ExtensionAnalytics>> analytics = await TwitchApiInternal.GetExtensionAnalyticsAsync(request_info, default(ExtensionAnalyticsParameters), settings);

            return analytics;
        }

        /// <summary>
        /// <para>Asynchronously gets analytic urls for one or more devloper extensions.</para>
        /// <para>Required Scope: 'analytics:read:extensions'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<ExtensionAnalytics>>>
        GetExtensionAnalyticsAsync(string bearer_token, string client_id, ExtensionAnalyticsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<Data<ExtensionAnalytics>> analytics = await TwitchApiInternal.GetExtensionAnalyticsAsync(request_info, parameters, settings);

            return analytics;
        }

        #endregion

        // TODO: Test if I actually get around to making a game.. or ask someone who has :)
        #region /analytics/games

        /// <summary>
        /// <para>Asynchronously gets a single page of analytic urls for one or more devloper games.</para>
        /// <para>Required Scope: 'analytics:read:games'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<GameAnalytics>>>
        GetGameAnalyticsPageAsync(string bearer_token, RequestSettings settings = null)
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token = bearer_token;

            IHelixResponse<Data<GameAnalytics>> response = await TwitchApiInternal.GetGameAnalyticsPageAsync(request_info, default(GameAnalyticsParameters), settings);

            return response;
        }

        /// <summary>
        /// <para>Asynchronously gets a single page of analytic urls for one or more devloper games.</para>
        /// <para>Required Scope: 'analytics:read:games'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<GameAnalytics>>>
        GetGameAnalyticsPageAsync(string bearer_token, GameAnalyticsParameters parameters, RequestSettings settings = null)
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token = bearer_token;

            IHelixResponse<Data<GameAnalytics>> response = await TwitchApiInternal.GetGameAnalyticsPageAsync(request_info, parameters, settings);

            return response;
        }

        /// <summary>
        /// <para>Asynchronously gets a single page of analytic urls for one or more devloper games.</para>
        /// <para>Required Scope: 'analytics:read:games'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<GameAnalytics>>>
        GetGameAnalyticsPageAsync(string bearer_token, string client_id, RequestSettings settings = null)
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<Data<GameAnalytics>> response = await TwitchApiInternal.GetGameAnalyticsPageAsync(request_info, default(GameAnalyticsParameters), settings);

            return response;
        }

        /// <summary>
        /// <para>Asynchronously gets a single page of analytic urls for one or more devloper games.</para>
        /// <para>Required Scope: 'analytics:read:games'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<GameAnalytics>>>
        GetGameAnalyticsPageAsync(string bearer_token, string client_id, GameAnalyticsParameters parameters, RequestSettings settings = null)
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<Data<GameAnalytics>> response = await TwitchApiInternal.GetGameAnalyticsPageAsync(request_info, parameters, settings);

            return response;
        }

        /// <summary>
        /// <para>Asynchronously gets a complete list of analytic urls for one or more devloper games.</para>
        /// <para>Required Scope: 'analytics:read:games'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<GameAnalytics>>>
        GetGameAnalyticsAsync(string bearer_token, RequestSettings settings = null)
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token = bearer_token;

            IHelixResponse<DataPage<GameAnalytics>> response = await TwitchApiInternal.GetGameAnalyticsAsync(request_info, default(GameAnalyticsParameters), settings);

            return response;
        }

        /// <summary>
        /// <para>Asynchronously gets a complete list of analytic urls for one or more devloper games.</para>
        /// <para>Required Scope: 'analytics:read:games'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<GameAnalytics>>>
        GetGameAnalyticsAsync(string bearer_token, GameAnalyticsParameters parameters, RequestSettings settings = null)
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token = bearer_token;

            IHelixResponse<DataPage<GameAnalytics>> response = await TwitchApiInternal.GetGameAnalyticsAsync(request_info, parameters, settings);

            return response;
        }

        /// <summary>
        /// <para>Asynchronously gets a complete list of analytic urls for one or more devloper games.</para>
        /// <para>Required Scope: 'analytics:read:games'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<GameAnalytics>>>
        GetGameAnalyticsAsync(string bearer_token, string client_id, RequestSettings settings = null)
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token = bearer_token;
            request_info.client_id = client_id;

            IHelixResponse<DataPage<GameAnalytics>> response = await TwitchApiInternal.GetGameAnalyticsAsync(request_info, default(GameAnalyticsParameters), settings);

            return response;
        }

        /// <summary>
        /// <para>Asynchronously gets a complete list of analytic urls for one or more devloper games.</para>
        /// <para>Required Scope: 'analytics:read:games'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<GameAnalytics>>>
        GetGameAnalyticsAsync(string bearer_token, string client_id, GameAnalyticsParameters parameters, RequestSettings settings = null)
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token = bearer_token;
            request_info.client_id = client_id;

            IHelixResponse<DataPage<GameAnalytics>> response = await TwitchApiInternal.GetGameAnalyticsAsync(request_info, parameters, settings);

            return response;
        }

        #endregion

        #region bits/leaderboard

        /// <summary>
        /// <para>Asynchronously gets a ranked list of bits leaderboard information for an authorized broadcaster.</para>
        /// <para>Required Scope: 'bits:read'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<BitsLeaderboardData<BitsUser>>>
        GetBitsLeaderboardAsync(string bearer_token, RequestSettings settings = null)
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token = bearer_token;

            IHelixResponse<BitsLeaderboardData<BitsUser>> response = await TwitchApiInternal.GetBitsLeaderboardAsync(request_info, default(BitsLeaderboardParameters), settings);

            return response;
        }

        /// <summary>
        /// <para>Asynchronously gets a ranked list of bits leaderboard information for an authorized broadcaster.</para>
        /// <para>Required Scope: 'bits:read'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<BitsLeaderboardData<BitsUser>>>
        GetBitsLeaderboardAsync(string bearer_token, BitsLeaderboardParameters parameters, RequestSettings settings = null)
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token = bearer_token;

            IHelixResponse<BitsLeaderboardData<BitsUser>> response = await TwitchApiInternal.GetBitsLeaderboardAsync(request_info, parameters, settings);

            return response;
        }

        /// <summary>
        /// <para>Asynchronously gets a ranked list of bits leaderboard information for an authorized broadcaster.</para>
        /// <para>Required Scope: 'bits:read'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<BitsLeaderboardData<BitsUser>>>
        GetBitsLeaderboardAsync(string bearer_token, string client_id, RequestSettings settings = null)
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token = bearer_token;
            request_info.client_id = client_id;

            IHelixResponse<BitsLeaderboardData<BitsUser>> response = await TwitchApiInternal.GetBitsLeaderboardAsync(request_info, default(BitsLeaderboardParameters), settings);

            return response;
        }

        /// <summary>
        /// <para>Asynchronously gets a ranked list of bits leaderboard information for an authorized broadcaster.</para>
        /// <para>Required Scope: 'bits:read'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<BitsLeaderboardData<BitsUser>>>
        GetBitsLeaderboardAsync(string bearer_token, string client_id, BitsLeaderboardParameters parameters, RequestSettings settings = null)
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<BitsLeaderboardData<BitsUser>> response = await TwitchApiInternal.GetBitsLeaderboardAsync(request_info, parameters, settings);

            return response;
        }

        #endregion

        #region /clips

        /// <summary>
        /// <para>Asynchronously creates a clip.</para>
        /// <para>Required Scope: 'clips:edit'.</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<CreatedClip>>>
        CreateClipAsync(string bearer_token, ClipCreationParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token = bearer_token;

            IHelixResponse<Data<CreatedClip>> clip = await TwitchApiInternal.CreateClipAsync(request_info, parameters, settings);

            return clip;
        }

        /// <summary>
        /// <para>Asynchronously creates a clip.</para>
        /// <para>Required Scope: 'clips:edit'.</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<CreatedClip>>>
        CreateClipAsync(string bearer_token, string client_id, ClipCreationParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<Data<CreatedClip>> clip = await TwitchApiInternal.CreateClipAsync(request_info, parameters, settings);

            return clip;
        }

        /// <summary>
        /// Asynchronously gets information about a clip.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<Clip>>>
        GetClipAsync(string bearer_token, ClipParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<Data<Clip>> clip = await TwitchApiInternal.GetClipAsync(request_info, parameters, settings);

            return clip;
        }

        /// <summary>
        /// Asynchronously gets information about a clip.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<Clip>>>
        GetClipAsync(string bearer_token, string client_id, ClipParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<Data<Clip>> clip = await TwitchApiInternal.GetClipAsync(request_info, parameters, settings);

            return clip;
        }

        #endregion

        #region /entitlements/uploads

        /// <summary>
        /// <para>Asynchronously creates a URL where you can upload a manifest file and notify users that they have an entitlement.</para>
        /// <para>Required Authorization: Application Access Token</para>
        /// </summary>
        /// <param name="application_token">The application access token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<Url>>>
        CreateEntitlementGrantsUploadUrlAsync(string application_token, EntitlementParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = application_token;

            IHelixResponse<Data<Url>> url = await TwitchApiInternal.CreateEntitlementGrantsUploadUrlAsync(request_info, parameters, settings);

            return url;
        }

        /// <summary>
        /// <para>Asynchronously creates a URL where you can upload a manifest file and notify users that they have an entitlement.</para>
        /// <para>Required Authorization: Application Access Token</para>
        /// </summary>
        /// <param name="application_token">The application access token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<Url>>>
        CreateEntitlementGrantsUploadUrlAsync(string application_token, string client_id, EntitlementParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = application_token;
            request_info.client_id      = client_id;

            IHelixResponse<Data<Url>> url = await TwitchApiInternal.CreateEntitlementGrantsUploadUrlAsync(request_info, parameters, settings);

            return url;
        }

        #endregion

        #region /games

        /// <summary>
        /// Asynchronously gets information about a list of games.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<Game>>>
        GetGamesAsync(string bearer_token, GamesParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<Data<Game>> games = await TwitchApiInternal.GetGamesAsync(request_info, parameters, settings);

            return games;
        }

        /// <summary>
        /// Asynchronously gets information about a list of games.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<Game>>>
        GetGamesAsync(string bearer_token, string client_id, GamesParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<Data<Game>> games = await TwitchApiInternal.GetGamesAsync(request_info, parameters, settings);

            return games;
        }

        #endregion

        #region /games/top

        /// <summary>
        /// Asynchronously gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesPageAsync(string bearer_token, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<DataPage<Game>> top_games = await TwitchApiInternal.GetTopGamesPageAsync(request_info, default(TopGamesParameters), settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesPageAsync(string bearer_token, TopGamesParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<DataPage<Game>> top_games = await TwitchApiInternal.GetTopGamesPageAsync(request_info, parameters, settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesPageAsync(string bearer_token, string client_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<DataPage<Game>> top_games = await TwitchApiInternal.GetTopGamesPageAsync(request_info, default(TopGamesParameters), settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesPageAsync(string bearer_token, string client_id, TopGamesParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<DataPage<Game>> top_games = await TwitchApiInternal.GetTopGamesPageAsync(request_info, parameters, settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesAsync(string bearer_token, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<DataPage<Game>> top_games = await TwitchApiInternal.GetTopGamesAsync(request_info, default(TopGamesParameters), settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesAsync(string bearer_token, TopGamesParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<DataPage<Game>> top_games = await TwitchApiInternal.GetTopGamesAsync(request_info, parameters, settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesAsync(string bearer_token, string client_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<DataPage<Game>> top_games = await TwitchApiInternal.GetTopGamesAsync(request_info, default(TopGamesParameters), settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesAsync(string bearer_token, string client_id, TopGamesParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<DataPage<Game>> top_games = await TwitchApiInternal.GetTopGamesAsync(request_info, parameters, settings);

            return top_games;
        }

        #endregion

        #region /streams

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsPageAsync(string bearer_token, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<DataPage<Stream>> streams = await TwitchApiInternal.GetStreamsPageAsync(request_info, default(StreamsParameters), settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsPageAsync(string bearer_token, StreamsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<DataPage<Stream>> streams = await TwitchApiInternal.GetStreamsPageAsync(request_info, parameters, settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsPageAsync(string bearer_token, string client_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<DataPage<Stream>> streams = await TwitchApiInternal.GetStreamsPageAsync(request_info, default(StreamsParameters), settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsPageAsync(string bearer_token,string client_id, StreamsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<DataPage<Stream>> streams = await TwitchApiInternal.GetStreamsPageAsync(request_info, parameters, settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsAsync(string bearer_token, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<DataPage<Stream>> streams = await TwitchApiInternal.GetStreamsAsync(request_info, default(StreamsParameters), settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsAsync(string bearer_token, StreamsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<DataPage<Stream>> streams = await TwitchApiInternal.GetStreamsAsync(request_info, parameters, settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsAsync(string bearer_token, string client_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<DataPage<Stream>> streams = await TwitchApiInternal.GetStreamsAsync(request_info, default(StreamsParameters), settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsAsync(string bearer_token, string client_id, StreamsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<DataPage<Stream>> streams = await TwitchApiInternal.GetStreamsAsync(request_info, parameters, settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously checks to see if a user is streaming.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="user_id">The user to check if they are live.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<bool>>
        IsStreamLiveAsync(string bearer_token, string user_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<bool> is_live = await TwitchApiInternal.IsStreamLiveAsync(request_info, user_id, settings);

            return is_live;
        }

        /// <summary>
        /// Asynchronously checks to see if a user is streaming.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="user_id">The user to check if they are live.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<bool>>
        IsstreamLiveAsync(string bearer_token, string client_id, string user_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<bool> is_live = await TwitchApiInternal.IsStreamLiveAsync(request_info, user_id, settings);

            return is_live;
        }

        #endregion

        #region /streams/metadata

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Metadata>>>
        GetStreamsMetadataPageAsync(string bearer_token, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<DataPage<Metadata>> metadata = await TwitchApiInternal.GetStreamsMetadataPageAsync(request_info, default(StreamsParameters), settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Metadata>>>
        GetStreamsMetadataPageAsync(string bearer_token, StreamsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<DataPage<Metadata>> metadata = await TwitchApiInternal.GetStreamsMetadataPageAsync(request_info, parameters, settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Metadata>>>
        GetStreamsMetadataPageAsync(string bearer_token, string client_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<DataPage<Metadata>> metadata = await TwitchApiInternal.GetStreamsMetadataPageAsync(request_info, default(StreamsParameters), settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Metadata>>>
        GetStreamsMetadataPageAsync(string bearer_token, string client_id, StreamsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<DataPage<Metadata>> metadata = await TwitchApiInternal.GetStreamsMetadataPageAsync(request_info, parameters, settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Metadata>>>
        GetStreamsMetadataAsync(string bearer_token, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<DataPage<Metadata>> metadata = await TwitchApiInternal.GetStreamsMetadataAsync(request_info, default(StreamsParameters), settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Metadata>>>
        GetStreamsMetadataAsync(string bearer_token, StreamsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<DataPage<Metadata>> metadata = await TwitchApiInternal.GetStreamsMetadataAsync(request_info, parameters, settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Metadata>>>
        GetStreamsMetadataAsync(string bearer_token, string client_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<DataPage<Metadata>> metadata = await TwitchApiInternal.GetStreamsMetadataAsync(request_info, default(StreamsParameters), settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Metadata>>>
        GetStreamsMetadataAsync(string bearer_token, string client_id, StreamsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<DataPage<Metadata>> metadata = await TwitchApiInternal.GetStreamsMetadataAsync(request_info, parameters, settings);

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
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<User>>>
        GetUserAsync(string bearer_token, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<Data<User>> users = await TwitchApiInternal.GetUsersAsync(request_info, default(UsersParameters), settings);

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
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<User>>>
        GetUserAsync(string bearer_token, string client_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<Data<User>> users = await TwitchApiInternal.GetUsersAsync(request_info, default(UsersParameters), settings);

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
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<User>>>
        GetUsersAsync(string bearer_token, UsersParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<Data<User>> users = await TwitchApiInternal.GetUsersAsync(request_info, parameters, settings);

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
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<User>>>
        GetUsersAsync(string bearer_token, string client_id, UsersParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<Data<User>> users = await TwitchApiInternal.GetUsersAsync(request_info, parameters, settings);

            return users;
        }

        /// <summary>
        /// <para>Asynchronously sets the description of a user specified by the Bearer token provided.</para>
        /// <para>Required Scope: 'user:edit'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="description">The new description to set.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<User>>>
        SetUserDescriptionAsync(string bearer_token, string description, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<Data<User>> success = await TwitchApiInternal.SetUserDescriptionAsync(request_info, description, settings);

            return success;
        }

        /// <summary>
        /// <para>Asynchronously sets the description of a user specified by the Bearer token provided.</para>
        /// <para>Required Scope: 'user:edit'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="description">The new description to set.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<User>>>
        SetUserDescriptionAsync(string bearer_token, string client_id, string description, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<Data<User>> result = await TwitchApiInternal.SetUserDescriptionAsync(request_info, description, settings);

            return result;
        }

        #endregion

        #region /users/extensions

        /// <summary>
        /// <para>
        /// Asynchronously gets a list of active extensions installed by a user.
        /// The user is identified either by user ID or by the provided Bearer token.
        /// </para>
        /// <para>Optional Scope: <see cref="Scopes.UserReadBroadcast"/> or <see cref="Scopes.UserEditBroadcast"/>.</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<ActiveExtensionsData>>
        GetUserActiveExtensionsAsync(string bearer_token, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token = bearer_token;

            IHelixResponse<ActiveExtensionsData> response = await TwitchApiInternal.GetUserActiveExtensionsAsync(request_info, default(ActiveExtensionsParameters), settings);

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
        public static async Task<IHelixResponse<ActiveExtensionsData>>
        GetUserActiveExtensionsAsync(string bearer_token, string client_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token = bearer_token;
            request_info.client_id = client_id;

            IHelixResponse<ActiveExtensionsData> response = await TwitchApiInternal.GetUserActiveExtensionsAsync(request_info, default(ActiveExtensionsParameters), settings);

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
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<ActiveExtensionsData>>
        GetUserActiveExtensionsAsync(string bearer_token, ActiveExtensionsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token = bearer_token;

            IHelixResponse<ActiveExtensionsData> response = await TwitchApiInternal.GetUserActiveExtensionsAsync(request_info, parameters, settings);

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
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<ActiveExtensionsData>>
        GetUserActiveExtensionsAsync(string bearer_token, string client_id, ActiveExtensionsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<ActiveExtensionsData> response = await TwitchApiInternal.GetUserActiveExtensionsAsync(request_info, parameters, settings);

            return response;
        }

        /// <summary>
        /// <para>
        /// Asynchronously updates the active extensions for a user identified by a user ID or by the provided Bearer token.
        /// The activation state, extension ID, verison number, or x/y coordinates (components only) can be updated.
        /// </para>
        /// <para>Required Scope: <see cref="Scopes.UserEditBroadcast"/>.</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<ActiveExtensionsData>>
        UpdateUserExtensionsAsync(string bearer_token, UpdateExtensionsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token = bearer_token;

            IHelixResponse<ActiveExtensionsData> response = await TwitchApiInternal.UpdateUserExtensionsAsync(request_info, parameters, settings);

            return response;
        }

        /// <summary>
        /// <para>
        /// Asynchronously updates the active extensions for a user identified by a user ID or by the provided Bearer token.
        /// The activation state, extension ID, verison number, or x/y coordinates (components only) can be updated.
        /// </para>
        /// <para>Required Scope: <see cref="Scopes.UserEditBroadcast"/>.</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<ActiveExtensionsData>>
        UpdateUserExtensionsAsync(string bearer_token, string client_id, UpdateExtensionsParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<ActiveExtensionsData> response = await TwitchApiInternal.UpdateUserExtensionsAsync(request_info, parameters, settings);

            return response;
        }

        #endregion

        #region /users/extensions/list

        /// <summary>
        /// <para>Asynchronously gets a list of all extensions a user has installed, active or inactive.</para>
        /// <para>Required Scope: 'user:read:broadcast'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<Extension>>>
        GetUserExtensionsAsync(string bearer_token, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo helix_info = new HelixInfo();
            helix_info.bearer_token = bearer_token;

            IHelixResponse<Data<Extension>> response = await TwitchApiInternal.GetUserExtensionsAsync(helix_info, settings);

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
        public static async Task<IHelixResponse<Data<Extension>>>
        GetUserExtensionsAsync(string bearer_token, string client_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<Data<Extension>> response = await TwitchApiInternal.GetUserExtensionsAsync(request_info, settings);

            return response;
        }

        #endregion
        */
        #region /users/follows

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingPageAsync(string bearer_token, string from_id, RequestSettings settings = default)
        {
            RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
            info.bearer_token = bearer_token;

            IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowingPageAsync(info, from_id, default);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingPageAsync(string bearer_token, string from_id, FollowsParameters parameters, RequestSettings settings = default)
        {
            RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
            info.bearer_token = bearer_token;

            IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowingPageAsync(info, from_id, parameters);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingPageAsync(string bearer_token, string client_id, string from_id, RequestSettings settings = default)
        {
            RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
            info.bearer_token = bearer_token;
            info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowingPageAsync(info, from_id, default);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingPageAsync(string bearer_token, string client_id, string from_id, FollowsParameters parameters, RequestSettings settings = default)
        {
            RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
            info.bearer_token = bearer_token;
            info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowingPageAsync(info, from_id, parameters);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingAsync(string bearer_token, string from_id, RequestSettings settings = default)
        {
            RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
            info.bearer_token = bearer_token;

            IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowingAsync(info, from_id, default);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingAsync(string bearer_token, string from_id, FollowsParameters parameters, RequestSettings settings = default)
        {
            RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
            info.bearer_token = bearer_token;

            IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowingAsync(info, from_id, parameters);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingAsync(string bearer_token, string client_id, string from_id, RequestSettings settings = default)
        {
            RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
            info.bearer_token = bearer_token;
            info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowingAsync(info, from_id, default);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingAsync(string bearer_token, string client_id, string from_id, FollowsParameters parameters, RequestSettings settings = default)
        {
            RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
            info.bearer_token = bearer_token;
            info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowingAsync(info, from_id, parameters);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersPageAsync(string bearer_token, string to_id, RequestSettings settings = default)
        {
            RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
            info.bearer_token = bearer_token;

            IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowersPageAsync(info, to_id, default);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersPageAsync(string bearer_token, string to_id, FollowsParameters parameters, RequestSettings settings = default)
        {
            RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
            info.bearer_token = bearer_token;

            IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowersPageAsync(info, to_id, parameters);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersPageAsync(string bearer_token, string client_id, string to_id, RequestSettings settings = default)
        {
            RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
            info.bearer_token = bearer_token;
            info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowersPageAsync(info, to_id, default);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersPageAsync(string bearer_token, string client_id, string to_id, FollowsParameters parameters, RequestSettings settings = default)
        {
            RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
            info.bearer_token = bearer_token;
            info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowersPageAsync(info, to_id, parameters);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersAsync(string bearer_token, string to_id, RequestSettings settings = default)
        {
            RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
            info.bearer_token = bearer_token;

            IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowersAsync(info, to_id, default);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersAsync(string bearer_token, string to_id, FollowsParameters parameters, RequestSettings settings = default)
        {
            RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
            info.bearer_token = bearer_token;

            IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowersAsync(info, to_id, parameters);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersAsync(string bearer_token, string client_id, string to_id, RequestSettings settings = default)
        {
            RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
            info.bearer_token = bearer_token;
            info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowersAsync(info, to_id, default);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersAsync(string bearer_token, string client_id, string to_id, FollowsParameters parameters, RequestSettings settings = default)
        {
            RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
            info.bearer_token = bearer_token;
            info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowersAsync(info, to_id, parameters);

            return response;
        }

        public static async Task<IHelixResponse<bool>>
        IsUserFollowingAsync(string bearer_token, string from_id, string to_id, RequestSettings settings = default)
        {
            RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
            info.bearer_token = bearer_token;

            IHelixResponse<bool> is_following = await TwitchApiInternal.IsUserFollowingAsync(info, from_id, to_id);

            return is_following;
        }

        public static async Task<IHelixResponse<bool>>
        IsUserFollowingAsync(string bearer_token, string client_id, string from_id, string to_id, RequestSettings settings = default)
        {
            RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
            info.bearer_token = bearer_token;
            info.client_id = client_id;

            IHelixResponse<bool> is_following = await TwitchApiInternal.IsUserFollowingAsync(info, from_id, to_id);

            return is_following;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserRelationshipPageAsync(string bearer_token, FollowsParameters parameters, RequestSettings settings = default)
        {
            RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
            info.bearer_token = bearer_token;

            IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserRelationshipPageAsync(info, parameters);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserRelationshipPageAsync(string bearer_token, string client_id, FollowsParameters parameters, RequestSettings settings = default)
        {
            RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
            info.bearer_token = bearer_token;
            info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserRelationshipPageAsync(info, parameters);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserRelationshipAsync(string bearer_token, FollowsParameters parameters, RequestSettings settings = default)
        {
            RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
            info.bearer_token = bearer_token;

            IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserRelationshipAsync(info, parameters);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserRelationshipAsync(string bearer_token, string client_id, FollowsParameters parameters, RequestSettings settings = default)
        {
            RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
            info.bearer_token = bearer_token;
            info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserRelationshipAsync(info, parameters);

            return response;
        }

        #endregion
        /*
        #region /videos

        /// <summary>
        /// Asynchronously gets a single page of information on one or more videos.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Video>>>
        GetVideosPageAsync(string bearer_token, VideosParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<DataPage<Video>> videos = await TwitchApiInternal.GetVideosPageAsync(request_info, parameters, settings);

            return videos;
        }

        /// <summary>
        /// Asynchronously gets a single page of information on one or more videos.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Video>>>
        GetVideosPageAsync(string bearer_token, string client_id, VideosParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<DataPage<Video>> videos = await TwitchApiInternal.GetVideosPageAsync(request_info, parameters, settings);

            return videos;
        }

        /// <summary>
        /// Asynchronously gets a complete list of information on one or more videos.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Video>>>
        GetVideosAsync(string bearer_token, VideosParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<DataPage<Video>> videos = await TwitchApiInternal.GetVideosAsync(request_info, parameters, settings);

            return videos;
        }

        /// <summary>
        /// Asynchronously gets a complete list of information on one or more videos.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Video>>>
        GetVideosAsync(string bearer_token, string client_id, VideosParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<DataPage<Video>> videos = await TwitchApiInternal.GetVideosAsync(request_info, parameters, settings);

            return videos;
        }

        #endregion
        */
    }
}