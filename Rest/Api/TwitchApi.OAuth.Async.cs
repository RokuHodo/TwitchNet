// standard namespaces
using System;
using System.Net.Http;
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
    TwitchApi
    {
        public static class
        OAuth
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
                */
            #region /users

            /// <summary>
            /// <para>Asynchronously gets the information about a user from the specified bearer token.</para>
            /// <para>
            /// Optional scope: <see cref="Scopes.UserReadEmail"/>.
            /// If provided, the user's email is included in the response.
            /// </para>
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the information about the requested user.
            /// </returns>
            /// <exception cref="ArgumentException">Thrown if the bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<User>>>
            GetUserAsync(string bearer_token, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<Data<User>> response = await Internal.GetUsersAsync(info, default);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously gets the information about a user from the specified bearer token.</para>
            /// <para>
            /// Optional scope: <see cref="Scopes.UserReadEmail"/>.
            /// If provided, the user's email is included in the response.
            /// </para>
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="client_id">The application ID to identify the source of the request.</param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the information about the requested user.
            /// </returns>
            /// <exception cref="ArgumentException">Thrown if the bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<User>>>
            GetUserAsync(string bearer_token, string client_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<Data<User>> response = await Internal.GetUsersAsync(info, default);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously gets the information about one or more users.</para>
            /// <para>
            /// Optional scope: <see cref="Scopes.UserReadEmail"/>.
            /// If provided, the user's email is included in the response.
            /// </para>
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="parameters">
            /// A set of rest parameters specific to this request.
            /// If not specified, the user is looked up by the specified bearer token.
            /// </param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the information about each requested user.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null when no valid bearer token specified.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if the bearer token is null, empty, or contains only whitespace.
            /// Thrown if all specified user logins and user ID's are null, empty, or only contains whitespace when no valid bearer token is specified.
            /// Thrown if more than 100 total user logins and/or user IDs are specified.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<User>>>
            GetUsersAsync(string bearer_token, UsersParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);                
                info.bearer_token = bearer_token;

                IHelixResponse<Data<User>> response = await Internal.GetUsersAsync(info, parameters);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously gets the information about one or more users.</para>
            /// <para>
            /// Optional scope: <see cref="Scopes.UserReadEmail"/>.
            /// If provided, the user's email is included in the response.
            /// </para>
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="client_id">The application ID to identify the source of the request.</param>
            /// <param name="parameters">
            /// A set of rest parameters specific to this request.
            /// If not specified, the user is looked up by the specified bearer token.
            /// </param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the information about each requested user.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null when no valid bearer token specified.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if both bearer token and client ID are null, empty, or contains only whitespace.
            /// Thrown if all specified user logins and user ID's are null, empty, or only contains whitespace when no valid bearer token is specified.
            /// Thrown if more than 100 total user logins and/or user IDs are specified.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<User>>>
            GetUsersAsync(string bearer_token, string client_id, UsersParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);                
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<Data<User>> response = await Internal.GetUsersAsync(info, parameters);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously sets the description of a user specified by the bearer token.</para>
            /// <para>Required scope: <see cref="Scopes.UserEdit"/>.</para>
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="client_id">The application ID to identify the source of the request.</param>
            /// <param name="description">The text to set the user's description to.</param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains information about the user with the updated description.
            /// </returns>
            /// <exception cref="ArgumentException">Thrown if the bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="MissingScopesException">Thrown if the available scopes
            /// does not include the <see cref="Scopes.UserEdit"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<User>>>
            SetUserDescriptionAsync(string bearer_token, string description, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);                
                info.bearer_token = bearer_token;

                IHelixResponse<Data<User>> response = await Internal.SetUserDescriptionAsync(info, description);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously sets the description of a user specified by the bearer token.</para>
            /// <para>Required scope: <see cref="Scopes.UserEdit"/>.</para>
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="client_id">The application ID to identify the source of the request.</param>
            /// <param name="description">The text to set the user's description to.</param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains information about the user with the updated description.
            /// </returns>
            /// <exception cref="ArgumentException">Thrown if the bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="MissingScopesException">Thrown if the available scopes, if specified, does not include the <see cref="Scopes.UserEdit"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<User>>>
            SetUserDescriptionAsync(string bearer_token, string client_id, string description, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);                
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<Data<User>> response = await Internal.SetUserDescriptionAsync(info, description);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously sets the description of a user specified by the bearer token.</para>
            /// <para>Required scope: <see cref="Scopes.UserEdit"/>.</para>
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="parameters">A set of rest parameters specific to this request.</param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains information about the user with the updated description.
            /// </returns>
            /// <exception cref="ArgumentException">Thrown if the bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="MissingScopesException">Thrown if the available scopes, if specified, does not include the <see cref="Scopes.UserEdit"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<User>>>
            SetUserDescriptionAsync(string bearer_token, DescriptionParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);                
                info.bearer_token = bearer_token;

                IHelixResponse<Data<User>> response = await Internal.SetUserDescriptionAsync(info, parameters);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously sets the description of a user specified by the bearer token.</para>
            /// <para>Required scope: <see cref="Scopes.UserEdit"/>.</para>
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="client_id">The application ID to identify the source of the request.</param>
            /// <param name="parameters">A set of rest parameters specific to this request.</param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains information about the user with the updated description.
            /// </returns>
            /// <exception cref="ArgumentException">Thrown if the bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="MissingScopesException">Thrown if the available scopes, if specified, does not include the <see cref="Scopes.UserEdit"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<User>>>
            SetUserDescriptionAsync(string bearer_token, string client_id, DescriptionParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);                
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<Data<User>> response = await Internal.SetUserDescriptionAsync(info, parameters);

                return response;
            }

            #endregion

            #region /users/extensions

            /// <summary>
            /// <para>Asynchronously gets a list of active extensions a user has installed.</para>
            /// <para>Optional scopes: <see cref="Scopes.UserReadBroadcast"/> or <see cref="Scopes.UserEditBroadcast"/>.</para>
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains active extensions the user has instlled.
            /// </returns>
            /// <exception cref="ArgumentException">Thrown if both bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<ActiveExtensions>>
            GetUserActiveExtensionsAsync(string bearer_token, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<ActiveExtensions> response = await Internal.GetUserActiveExtensionsAsync(info, default);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously gets a list of active extensions a user has installed.</para>
            /// <para>Optional scopes: <see cref="Scopes.UserReadBroadcast"/> or <see cref="Scopes.UserEditBroadcast"/>.</para>
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="parameters">
            /// A set of rest parameters specific to this request.
            /// If no user ID is specified, the user is implicityly specified from the bearer token.
            /// </param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains active extensions the user has instlled.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// Thrown if both bearer token is null, empty, or contains only whitespace.
            /// Thrown if the specified user ID is null, empty, or only contains whitespace.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<ActiveExtensions>>
            GetUserActiveExtensionsAsync(string bearer_token, ActiveExtensionsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<ActiveExtensions> response = await Internal.GetUserActiveExtensionsAsync(info, parameters);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously gets a list of active extensions a user has installed.</para>
            /// <para>Optional scopes: <see cref="Scopes.UserReadBroadcast"/> or <see cref="Scopes.UserEditBroadcast"/>.</para>
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="client_id">The application ID to identify the source of the request.</param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains active extensions the user has instlled.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// Thrown if both bearer token and client ID are null, empty, or contains only whitespace.
            /// Thrown if both bearer token is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<ActiveExtensions>>
            GetUserActiveExtensionsAsync(string bearer_token, string client_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<ActiveExtensions> response = await Internal.GetUserActiveExtensionsAsync(info, default);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously gets a list of active extensions a user has installed.</para>
            /// <para>Optional scopes: <see cref="Scopes.UserReadBroadcast"/> or <see cref="Scopes.UserEditBroadcast"/>.</para>
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="client_id">The application ID to identify the source of the request.</param>
            /// <param name="parameters">
            /// A set of rest parameters specific to this request.
            /// If no user ID is specified, the user is implicityly specified from the bearer token.
            /// </param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains active extensions the user has instlled.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null when no valid bearer token is specified.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if both bearer token and client ID are null, empty, or contains only whitespace.
            /// Thrown if both bearer token is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<ActiveExtensions>>
            GetUserActiveExtensionsAsync(string bearer_token, string client_id, ActiveExtensionsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<ActiveExtensions> response = await Internal.GetUserActiveExtensionsAsync(info, parameters);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously updates the installed extensions for a user specified by the bearer token.</para>
            /// <para>Required scope: <see cref="Scopes.UserEditBroadcast"/>.</para>
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="parameters">
            /// <para>A set of rest parameters specific to this request.</para>
            /// <para>
            /// Any extensions specified outside of the valid extension slots for each type are ignored.
            /// The valid extension slots for each type are specified under each <see cref="ActiveExtensionsData"/> member.
            /// The (x, y) corrdinates are applicable only to component extensions.
            /// </para>
            /// </param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains active extensions the user has instlled after the changes have been applied.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if the <see cref="UpdateExtensionsParameters"/>, <see cref="UpdateExtensionsParameters.extensions"/>, or <see cref="ActiveExtensions.data"/> are null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if both bearer token is null, empty, or contains only whitespace.
            /// Thrown if each extension slot for each extension type is empty or null.
            /// Thrown if the name, ID, or version for each specified active extension is null, empty, or only contains whitespace.
            /// </exception>
            /// <exception cref="ArgumentOutOfRangeException">Thrown if the the either (x, y) coordinate for a component extension exceeds the range (0, 0) to (8000, 5000).</exception>
            /// <exception cref="DuplicateExtensionException">Thrown if an extension ID is set in more then one valid slot across all extension types.</exception>
            /// <exception cref="MissingScopesException">Thrown if the available scopes, if specified, does not include the <see cref="Scopes.UserEdit"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<ActiveExtensions>>
            UpdateUserExtensionsAsync(string bearer_token, UpdateExtensionsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<ActiveExtensions> response = await Internal.UpdateUserExtensionsAsync(info, parameters);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously updates the installed extensions for a user specified by the bearer token.</para>
            /// <para>Required scope: <see cref="Scopes.UserEditBroadcast"/>.</para>
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="client_id">The application ID to identify the source of the request.</param>
            /// <param name="parameters">
            /// <para>A set of rest parameters specific to this request.</para>
            /// <para>
            /// Any extensions specified outside of the valid extension slots for each type are ignored.
            /// The valid extension slots for each type are specified under each <see cref="ActiveExtensionsData"/> member.
            /// The (x, y) corrdinates are applicable only to component extensions.
            /// </para>
            /// </param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains active extensions the user has instlled after the changes have been applied.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if the <see cref="UpdateExtensionsParameters"/>, <see cref="UpdateExtensionsParameters.extensions"/>, or <see cref="ActiveExtensions.data"/> are null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if both bearer token is null, empty, or contains only whitespace.
            /// Thrown if each extension slot for each extension type is empty or null.
            /// Thrown if the name, ID, or version for each specified active extension is null, empty, or only contains whitespace.
            /// </exception>
            /// <exception cref="ArgumentOutOfRangeException">Thrown if the the either (x, y) coordinate for a component extension exceeds the range (0, 0) to (8000, 5000).</exception>
            /// <exception cref="DuplicateExtensionException">Thrown if an extension ID is set in more then one valid slot across all extension types.</exception>
            /// <exception cref="MissingScopesException">Thrown if the available scopes, if specified, does not include the <see cref="Scopes.UserEdit"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<ActiveExtensions>>
            UpdateUserExtensionsAsync(string bearer_token, string client_id, UpdateExtensionsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<ActiveExtensions> response = await Internal.UpdateUserExtensionsAsync(info, parameters);

                return response;
            }

            #endregion

            /*
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
            /*
            #region /users/follows

            /// <summary>
            /// Asynchronously gets a single page of user's following list.
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="from_id">The user ID to get the following list for.</param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of user's following list.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// Thrown if the bearer token is null, empty, or contains only whitespace.
            /// Thrown if <paramref name="from_id"/> is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingPageAsync(string bearer_token, string from_id, HelixRequestSettings settings = default)
            {
                RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
                info.bearer_token = bearer_token;

                IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowingPageAsync(info, from_id, default);

                return response;
            }

            /// <summary>
            /// Asynchronously gets a single page of user's following list.
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="from_id">The user ID to get the following list for.</param>
            /// <param name="parameters">
            /// A set of rest parameters to add to the request.
            /// If specified, from_id and to_id are ignored.
            /// </param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of user's following list.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// Thrown if the bearer token is null, empty, or contains only whitespace.
            /// Thrown if <paramref name="from_id"/> is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingPageAsync(string bearer_token, string from_id, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
                info.bearer_token = bearer_token;

                IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowingPageAsync(info, from_id, parameters);

                return response;
            }

            /// <summary>
            /// Asynchronously gets a single page of user's following list.
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="client_id">The application ID to identify the source of the request.</param>
            /// <param name="from_id">The user ID to get the following list for.</param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of user's following list.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// Thrown if both bearer token and client ID are null, empty, or contains only whitespace.
            /// Thrown if <paramref name="from_id"/> is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingPageAsync(string bearer_token, string client_id, string from_id, HelixRequestSettings settings = default)
            {
                RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowingPageAsync(info, from_id, default);

                return response;
            }

            /// <summary>
            /// Asynchronously gets a single page of user's following list.
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="client_id">The application ID to identify the source of the request.</param>
            /// <param name="from_id">The user ID to get the following list for.</param>
            /// <param name="parameters">
            /// A set of rest parameters to add to the request.
            /// If specified, from_id and to_id are ignored.
            /// </param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of user's following list.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// Thrown if both bearer token and client ID are null, empty, or contains only whitespace.
            /// Thrown if <paramref name="from_id"/> is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingPageAsync(string bearer_token, string client_id, string from_id, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowingPageAsync(info, from_id, parameters);

                return response;
            }

            /// <summary>
            /// Asynchronously gets a user's complete following list.
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="from_id">The user ID to get the following list for.</param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the user's complete following list.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// Thrown if the bearer token is null, empty, or contains only whitespace.
            /// Thrown if <paramref name="from_id"/> is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingAsync(string bearer_token, string from_id, HelixRequestSettings settings = default)
            {
                RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
                info.bearer_token = bearer_token;

                IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowingAsync(info, from_id, default);

                return response;
            }

            /// <summary>
            /// Asynchronously gets a user's complete following list.
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="from_id">The user ID to get the following list for.</param>
            /// <param name="parameters">
            /// A set of rest parameters to add to the request.
            /// If specified, from_id and to_id are ignored.
            /// </param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the user's complete following list.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// Thrown if the bearer token is null, empty, or contains only whitespace.
            /// Thrown if <paramref name="from_id"/> is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingAsync(string bearer_token, string from_id, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
                info.bearer_token = bearer_token;

                IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowingAsync(info, from_id, parameters);

                return response;
            }

            /// <summary>
            /// Asynchronously gets a user's complete following list.
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="client_id">The application ID to identify the source of the request.</param>
            /// <param name="from_id">The user ID to get the following list for.</param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the user's complete following list.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// Thrown if both bearer token and client ID are null, empty, or contains only whitespace.
            /// Thrown if <paramref name="from_id"/> is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingAsync(string bearer_token, string client_id, string from_id, HelixRequestSettings settings = default)
            {
                RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowingAsync(info, from_id, default);

                return response;
            }

            /// <summary>
            /// Asynchronously gets a user's complete following list.
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="client_id">The application ID to identify the source of the request.</param>
            /// <param name="from_id">The user ID to get the following list for.</param>
            /// <param name="parameters">
            /// A set of rest parameters to add to the request.
            /// If specified, from_id and to_id are ignored.
            /// </param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the user's complete following list.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// Thrown if both bearer token and client ID are null, empty, or contains only whitespace.
            /// Thrown if <paramref name="from_id"/> is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingAsync(string bearer_token, string client_id, string from_id, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowingAsync(info, from_id, parameters);

                return response;
            }

            /// <summary>
            /// Asynchronously gets a single page of a user's followers list.
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="to_id">The user ID to get the follower list for.</param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of a user's followers list.
            /// </returns> 
            /// <exception cref="ArgumentException">
            /// Thrown if the bearer token is null, empty, or contains only whitespace.
            /// Thrown if <paramref name="to_id"/> is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersPageAsync(string bearer_token, string to_id, HelixRequestSettings settings = default)
            {
                RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
                info.bearer_token = bearer_token;

                IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowersPageAsync(info, to_id, default);

                return response;
            }

            /// <summary>
            /// Asynchronously gets a single page of a user's followers list.
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="to_id">The user ID to get the follower list for.</param>
            /// <param name="parameters">
            /// A set of rest parameters to add to the request.
            /// If specified, from_id and to_id are ignored.
            /// </param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of a user's followers list.
            /// </returns> 
            /// <exception cref="ArgumentException">
            /// Thrown if the bearer token is null, empty, or contains only whitespace.
            /// Thrown if <paramref name="to_id"/> is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersPageAsync(string bearer_token, string to_id, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
                info.bearer_token = bearer_token;

                IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowersPageAsync(info, to_id, parameters);

                return response;
            }

            /// <summary>
            /// Asynchronously gets a single page of a user's followers list.
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="client_id">The application ID to identify the source of the request.</param>
            /// <param name="to_id">The user ID to get the follower list for.</param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of a user's followers list.
            /// </returns> 
            /// <exception cref="ArgumentException">
            /// Thrown if both bearer token and client ID are null, empty, or contains only whitespace.
            /// Thrown if <paramref name="to_id"/> is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersPageAsync(string bearer_token, string client_id, string to_id, HelixRequestSettings settings = default)
            {
                RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowersPageAsync(info, to_id, default);

                return response;
            }

            /// <summary>
            /// Asynchronously gets a single page of a user's followers list.
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="client_id">The application ID to identify the source of the request.</param>
            /// <param name="to_id">The user ID to get the follower list for.</param>
            /// <param name="parameters">
            /// A set of rest parameters to add to the request.
            /// If specified, from_id and to_id are ignored.
            /// </param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of a user's followers list.
            /// </returns> 
            /// <exception cref="ArgumentException">
            /// Thrown if both bearer token and client ID are null, empty, or contains only whitespace.
            /// Thrown if <paramref name="to_id"/> is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersPageAsync(string bearer_token, string client_id, string to_id, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowersPageAsync(info, to_id, parameters);

                return response;
            }

            /// <summary>
            /// Asynchronously gets a user's complete follower list.
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="to_id">The user ID to get the follower list for.</param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the user's complete follower list.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// Thrown if the bearer token is null, empty, or contains only whitespace.
            /// Thrown if <paramref name="to_id"/> is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersAsync(string bearer_token, string to_id, HelixRequestSettings settings = default)
            {
                RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
                info.bearer_token = bearer_token;

                IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowersAsync(info, to_id, default);

                return response;
            }

            /// <summary>
            /// Asynchronously gets a user's complete follower list.
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="to_id">The user ID to get the follower list for.</param>
            /// <param name="parameters">
            /// A set of rest parameters to add to the request.
            /// If specified, from_id and to_id are ignored.
            /// </param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the user's complete follower list.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// Thrown if the bearer token is null, empty, or contains only whitespace.
            /// Thrown if <paramref name="to_id"/> is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersAsync(string bearer_token, string to_id, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
                info.bearer_token = bearer_token;

                IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowersAsync(info, to_id, parameters);

                return response;
            }

            /// <summary>
            /// Asynchronously gets a user's complete follower list.
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="client_id">The application ID to identify the source of the request.</param>
            /// <param name="to_id">The user ID to get the follower list for.</param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the user's complete follower list.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// Thrown if both bearer token and client ID are null, empty, or contains only whitespace.
            /// Thrown if <paramref name="to_id"/> is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersAsync(string bearer_token, string client_id, string to_id, HelixRequestSettings settings = default)
            {
                RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowersAsync(info, to_id, default);

                return response;
            }

            /// <summary>
            /// Asynchronously gets a user's complete follower list.
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="client_id">The application ID to identify the source of the request.</param>
            /// <param name="to_id">The user ID to get the follower list for.</param>
            /// <param name="parameters">
            /// A set of rest parameters to add to the request.
            /// If specified, from_id and to_id are ignored.
            /// </param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the user's complete follower list.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// Thrown if both bearer token and client ID are null, empty, or contains only whitespace.
            /// Thrown if <paramref name="to_id"/> is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersAsync(string bearer_token, string client_id, string to_id, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserFollowersAsync(info, to_id, parameters);

                return response;
            }

            /// <summary>
            /// Asynchronously checks to see if a user is following another user.
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="from_id">The user ID to check if they are following another user.</param>
            /// <param name="to_id">The user ID to check if another user is following them.</param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> is set true if from_id is following to_id, otherwise false.
            /// </returns>        
            /// <exception cref="ArgumentException">
            /// Thrown if the bearer token is null, empty, or contains only whitespace.
            /// Thrown if either from_id and to_id are null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            public static async Task<IHelixResponse<bool>>
            IsUserFollowingAsync(string bearer_token, string from_id, string to_id, HelixRequestSettings settings = default)
            {
                RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
                info.bearer_token = bearer_token;

                IHelixResponse<bool> is_following = await TwitchApiInternal.IsUserFollowingAsync(info, from_id, to_id);

                return is_following;
            }

            /// <summary>
            /// Asynchronously checks to see if a user is following another user.
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="client_id">The application ID to identify the source of the request.</param>
            /// <param name="from_id">The user ID to check if they are following another user.</param>
            /// <param name="to_id">The user ID to check if another user is following them.</param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> is set true if from_id is following to_id, otherwise false.
            /// </returns>        
            /// <exception cref="ArgumentException">
            /// Thrown if both bearer token and client ID are null, empty, or contains only whitespace.
            /// Thrown if either from_id and to_id are null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            public static async Task<IHelixResponse<bool>>
            IsUserFollowingAsync(string bearer_token, string client_id, string from_id, string to_id, HelixRequestSettings settings = default)
            {
                RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<bool> is_following = await TwitchApiInternal.IsUserFollowingAsync(info, from_id, to_id);

                return is_following;
            }

            /// <summary>
            /// Asynchronously gets the relationship between two users, or a single page of a user's following/follower list.
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="parameters">
            /// <para>A set of rest parameters to add to the request.</para>
            /// <para>A from_id or to_id must be specified.</para>
            /// </param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the user relationship page, or a single page of the following/follower list of one user.
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if the bearer token is null, empty, or contains only whitespace.
            /// Thrown if both from_id and to_id are null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserRelationshipPageAsync(string bearer_token, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
                info.bearer_token = bearer_token;

                IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserRelationshipPageAsync(info, parameters);

                return response;
            }

            /// <summary>
            /// Asynchronously gets the relationship between two users, or a single page of a user's following/follower list.
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="client_id">The application ID to identify the source of the request.</param>
            /// <param name="parameters">
            /// <para>A set of rest parameters to add to the request.</para>
            /// <para>A from_id or to_id must be specified.</para>
            /// </param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the user relationship page, or a single page of the following/follower list of one user.
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if both bearer token and client ID are null, empty, or contains only whitespace.
            /// Thrown if both from_id and to_id are null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserRelationshipPageAsync(string bearer_token, string client_id, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserRelationshipPageAsync(info, parameters);

                return response;
            }

            /// <summary>
            /// Asynchronously gets the relationship between two users, or a user's complete following/follower list.
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="parameters">
            /// <para>A set of rest parameters to add to the request.</para>
            /// <para>A from_id or to_id must be specified.</para>
            /// </param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the user relationship, or the complete following/follower list of one user.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if the bearer token is null, empty, or contains only whitespace.
            /// Thrown if both from_id and to_id are null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserRelationshipAsync(string bearer_token, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
                info.bearer_token = bearer_token;

                IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserRelationshipAsync(info, parameters);

                return response;
            }

            /// <summary>
            /// Asynchronously gets the relationship between two users, or a user's complete following/follower list.
            /// </summary>
            /// <param name="bearer_token">A user access OAuth token.</param>
            /// <param name="client_id">The application ID to identify the source of the request.</param>
            /// <param name="parameters">
            /// <para>A set of rest parameters to add to the request.</para>
            /// <para>A from_id or to_id must be specified.</para>
            /// </param>
            /// <param name="settings">Settings to customize how the inputs, request, and response are handled.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the user relationship, or the complete following/follower list of one user.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if both bearer token and client ID are null, empty, or contains only whitespace.
            /// Thrown if both from_id and to_id are null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserRelationshipAsync(string bearer_token, string client_id, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                RestInfo<FollowsDataPage<Follow>> info = new RestInfo<FollowsDataPage<Follow>>(RestClients.Helix, settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await TwitchApiInternal.GetUserRelationshipAsync(info, parameters);

                return response;
            }

            #endregion
            */
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
}