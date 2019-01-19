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
            */

            #region /games

            public static async Task<IHelixResponse<Data<Game>>>
            GetGamesAsync(string bearer_token, GamesParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;

                IHelixResponse<Data<Game>> response = await Internal.GetGamesAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<Data<Game>>>
            GetGamesAsync(string bearer_token, string client_id, GamesParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;
                info.client_id      = client_id;

                IHelixResponse<Data<Game>> response = await Internal.GetGamesAsync(info, parameters);

                return response;
            }

            #endregion

            #region /games/top

            public static async Task<IHelixResponse<DataPage<Game>>>
            GetTopGamesPageAsync(string bearer_token, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;

                TopGamesParameters parameters = new TopGamesParameters();

                IHelixResponse<DataPage<Game>> response = await Internal.GetTopGamesPageAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<Game>>>
            GetTopGamesPageAsync(string bearer_token, TopGamesParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;

                IHelixResponse<DataPage<Game>> response = await Internal.GetTopGamesPageAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<Game>>>
            GetTopGamesPageAsync(string bearer_token, string client_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;
                info.client_id      = client_id;

                TopGamesParameters parameters = new TopGamesParameters();

                IHelixResponse<DataPage<Game>> response = await Internal.GetTopGamesPageAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<Game>>>
            GetTopGamesPageAsync(string bearer_token, string client_id, TopGamesParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;
                info.client_id      = client_id;

                IHelixResponse<DataPage<Game>> top_games = await Internal.GetTopGamesPageAsync(info, parameters);

                return top_games;
            }

            public static async Task<IHelixResponse<DataPage<Game>>>
            GetTopGamesAsync(string bearer_token, HelixRequestSettings settings = default)
            {
                HelixInfo request_info = new HelixInfo(settings);
                request_info.bearer_token   = bearer_token;

                TopGamesParameters parameters = new TopGamesParameters();

                IHelixResponse<DataPage<Game>> response = await Internal.GetTopGamesAsync(request_info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<Game>>>
            GetTopGamesAsync(string bearer_token, TopGamesParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;

                IHelixResponse<DataPage<Game>> response = await Internal.GetTopGamesAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<Game>>>
            GetTopGamesAsync(string bearer_token, string client_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;
                info.client_id      = client_id;

                TopGamesParameters parameters = new TopGamesParameters();

                IHelixResponse<DataPage<Game>> response = await Internal.GetTopGamesAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<Game>>>
            GetTopGamesAsync(string bearer_token, string client_id, TopGamesParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo request_info = new HelixInfo(settings);
                request_info.bearer_token   = bearer_token;
                request_info.client_id      = client_id;

                IHelixResponse<DataPage<Game>> response = await Internal.GetTopGamesAsync(request_info, parameters);

                return response;
            }

            #endregion

            #region /streams

            public static async Task<IHelixResponse<DataPage<Stream>>>
            GetStreamsPageAsync(string bearer_token, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;

                IHelixResponse<DataPage<Stream>> response = await Internal.GetStreamsPageAsync(info, default);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<Stream>>>
            GetStreamsPageAsync(string bearer_token, StreamsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;

                IHelixResponse<DataPage<Stream>> response = await Internal.GetStreamsPageAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<Stream>>>
            GetStreamsPageAsync(string bearer_token, string client_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;
                info.client_id      = client_id;

                IHelixResponse<DataPage<Stream>> response = await Internal.GetStreamsPageAsync(info, default);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<Stream>>>
            GetStreamsPageAsync(string bearer_token,string client_id, StreamsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;
                info.client_id      = client_id;

                IHelixResponse<DataPage<Stream>> response = await Internal.GetStreamsPageAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<Stream>>>
            GetStreamsAsync(string bearer_token, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<DataPage<Stream>> response = await Internal.GetStreamsAsync(info, default);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<Stream>>>
            GetStreamsAsync(string bearer_token, StreamsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<DataPage<Stream>> response = await Internal.GetStreamsAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<Stream>>>
            GetStreamsAsync(string bearer_token, string client_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<DataPage<Stream>> response = await Internal.GetStreamsAsync(info, default);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<Stream>>>
            GetStreamsAsync(string bearer_token, string client_id, StreamsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<DataPage<Stream>> response = await Internal.GetStreamsAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<bool>>
            IsStreamLiveByUserIDAsync(string bearer_token, string user_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<bool> is_live = await Internal.IsStreamLiveByUserIDAsync(info, user_id);

                return is_live;
            }

            public static async Task<IHelixResponse<bool>>
            IsStreamLiveByUserIDAsync(string bearer_token, string client_id, string user_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<bool> response = await Internal.IsStreamLiveByUserIDAsync(info, user_id);

                return response;
            }

            public static async Task<IHelixResponse<bool>>
            IsStreamLiveByUserLoginAsync(string bearer_token, string user_login, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<bool> is_live = await Internal.IsStreamLiveByUserLoginAsync(info, user_login);

                return is_live;
            }

            public static async Task<IHelixResponse<bool>>
            IsStreamLiveByUserLoginAsync(string bearer_token, string client_id, string user_login, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<bool> response = await Internal.IsStreamLiveByUserLoginAsync(info, user_login);

                return response;
            }

            #endregion

            #region /streams/metadata

            public static async Task<IHelixResponse<DataPage<StreamMetadata>>>
            GetStreamsMetadataPageAsync(string bearer_token, HelixRequestSettings settings = default)
            {
                HelixInfo request_info = new HelixInfo(settings);
                request_info.bearer_token   = bearer_token;

                IHelixResponse<DataPage<StreamMetadata>> response = await Internal.GetStreamsMetadataPageAsync(request_info, default);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<StreamMetadata>>>
            GetStreamsMetadataPageAsync(string bearer_token, StreamsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo request_info = new HelixInfo(settings);
                request_info.bearer_token   = bearer_token;

                IHelixResponse<DataPage<StreamMetadata>> response = await Internal.GetStreamsMetadataPageAsync(request_info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<StreamMetadata>>>
            GetStreamsMetadataPageAsync(string bearer_token, string client_id, HelixRequestSettings settings = default)
            {
                HelixInfo request_info = new HelixInfo(settings);
                request_info.bearer_token   = bearer_token;
                request_info.client_id      = client_id;

                IHelixResponse<DataPage<StreamMetadata>> response = await Internal.GetStreamsMetadataPageAsync(request_info, default);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<StreamMetadata>>>
            GetStreamsMetadataPageAsync(string bearer_token, string client_id, StreamsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo request_info = new HelixInfo(settings);
                request_info.bearer_token   = bearer_token;
                request_info.client_id      = client_id;

                IHelixResponse<DataPage<StreamMetadata>> response = await Internal.GetStreamsMetadataPageAsync(request_info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<StreamMetadata>>>
            GetStreamsMetadataAsync(string bearer_token, HelixRequestSettings settings = default)
            {
                HelixInfo request_info = new HelixInfo(settings);
                request_info.bearer_token   = bearer_token;

                IHelixResponse<DataPage<StreamMetadata>> response = await Internal.GetStreamsMetadataAsync(request_info, default);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<StreamMetadata>>>
            GetStreamsMetadataAsync(string bearer_token, StreamsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo request_info = new HelixInfo(settings);
                request_info.bearer_token   = bearer_token;

                IHelixResponse<DataPage<StreamMetadata>> response = await Internal.GetStreamsMetadataAsync(request_info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<StreamMetadata>>>
            GetStreamsMetadataAsync(string bearer_token, string client_id, HelixRequestSettings settings = default)
            {
                HelixInfo request_info = new HelixInfo(settings);
                request_info.bearer_token   = bearer_token;
                request_info.client_id      = client_id;

                IHelixResponse<DataPage<StreamMetadata>> response = await Internal.GetStreamsMetadataAsync(request_info, default);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<StreamMetadata>>>
            GetStreamsMetadataAsync(string bearer_token, string client_id, StreamsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo request_info = new HelixInfo(settings);
                request_info.bearer_token   = bearer_token;
                request_info.client_id      = client_id;

                IHelixResponse<DataPage<StreamMetadata>> response = await Internal.GetStreamsMetadataAsync(request_info, parameters);

                return response;
            }

            #endregion

            #region /users

            public static async Task<IHelixResponse<Data<User>>>
            GetUserAsync(string bearer_token, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<Data<User>> response = await Internal.GetUsersAsync(info, default);

                return response;
            }

            public static async Task<IHelixResponse<Data<User>>>
            GetUserAsync(string bearer_token, string client_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<Data<User>> response = await Internal.GetUsersAsync(info, default);

                return response;
            }

            public static async Task<IHelixResponse<Data<User>>>
            GetUsersAsync(string bearer_token, UsersParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);                
                info.bearer_token = bearer_token;

                IHelixResponse<Data<User>> response = await Internal.GetUsersAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<Data<User>>>
            GetUsersAsync(string bearer_token, string client_id, UsersParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);                
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<Data<User>> response = await Internal.GetUsersAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<Data<User>>>
            SetUserDescriptionAsync(string bearer_token, string description, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);                
                info.bearer_token = bearer_token;

                IHelixResponse<Data<User>> response = await Internal.SetUserDescriptionAsync(info, description);

                return response;
            }

            public static async Task<IHelixResponse<Data<User>>>
            SetUserDescriptionAsync(string bearer_token, string client_id, string description, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);                
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<Data<User>> response = await Internal.SetUserDescriptionAsync(info, description);

                return response;
            }

            public static async Task<IHelixResponse<Data<User>>>
            SetUserDescriptionAsync(string bearer_token, DescriptionParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);                
                info.bearer_token = bearer_token;

                IHelixResponse<Data<User>> response = await Internal.SetUserDescriptionAsync(info, parameters);

                return response;
            }

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

            public static async Task<IHelixResponse<ActiveExtensions>>
            GetUserActiveExtensionsAsync(string bearer_token, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<ActiveExtensions> response = await Internal.GetUserActiveExtensionsAsync(info, default);

                return response;
            }

            public static async Task<IHelixResponse<ActiveExtensions>>
            GetUserActiveExtensionsAsync(string bearer_token, ActiveExtensionsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<ActiveExtensions> response = await Internal.GetUserActiveExtensionsAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<ActiveExtensions>>
            GetUserActiveExtensionsAsync(string bearer_token, string client_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<ActiveExtensions> response = await Internal.GetUserActiveExtensionsAsync(info, default);

                return response;
            }

            public static async Task<IHelixResponse<ActiveExtensions>>
            GetUserActiveExtensionsAsync(string bearer_token, string client_id, ActiveExtensionsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<ActiveExtensions> response = await Internal.GetUserActiveExtensionsAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<ActiveExtensions>>
            UpdateUserActiveExtensionsAsync(string bearer_token, UpdateExtensionsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<ActiveExtensions> response = await Internal.UpdateUserActiveExtensionsAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<ActiveExtensions>>
            UpdateUserActiveExtensionsAsync(string bearer_token, string client_id, UpdateExtensionsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<ActiveExtensions> response = await Internal.UpdateUserActiveExtensionsAsync(info, parameters);

                return response;
            }

            #endregion

            #region /users/extensions/list

            public static async Task<IHelixResponse<Data<Extension>>>
            GetUserExtensionsAsync(string bearer_token, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<Data<Extension>> response = await Internal.GetUserExtensionsAsync(info);

                return response;
            }

            public static async Task<IHelixResponse<Data<Extension>>>
            GetUserExtensionsAsync(string bearer_token, string client_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;
                info.client_id      = client_id;

                IHelixResponse<Data<Extension>> response = await Internal.GetUserExtensionsAsync(info);

                return response;
            }

            #endregion

            #region /users/follows

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingPageAsync(string bearer_token, string from_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                FollowsParameters parameters = new FollowsParameters();
                parameters.from_id = from_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowingPageAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingPageAsync(string bearer_token, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowingPageAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingPageAsync(string bearer_token, string client_id, string from_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                FollowsParameters parameters = new FollowsParameters();
                parameters.from_id = from_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowingPageAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingPageAsync(string bearer_token, string client_id, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowingPageAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingAsync(string bearer_token, string from_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                FollowsParameters parameters = new FollowsParameters();
                parameters.from_id = from_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowingAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingAsync(string bearer_token, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowingAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingAsync(string bearer_token, string client_id, string from_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                FollowsParameters parameters = new FollowsParameters();
                parameters.from_id = from_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowingAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingAsync(string bearer_token, string client_id, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowingAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersPageAsync(string bearer_token, string to_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                FollowsParameters parameters = new FollowsParameters();
                parameters.to_id = to_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowersPageAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersPageAsync(string bearer_token, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowersPageAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersPageAsync(string bearer_token, string client_id, string to_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                FollowsParameters parameters = new FollowsParameters();
                parameters.to_id = to_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowersPageAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersPageAsync(string bearer_token, string client_id, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowersPageAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersAsync(string bearer_token, string to_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                FollowsParameters parameters = new FollowsParameters();
                parameters.to_id = to_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowersAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersAsync(string bearer_token, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowersAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersAsync(string bearer_token, string client_id, string to_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                FollowsParameters parameters = new FollowsParameters();
                parameters.to_id = to_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowersAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersAsync(string bearer_token, string client_id, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowersAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<bool>>
            IsUserFollowingAsync(string bearer_token, string from_id, string to_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<bool> is_following = await Internal.IsUserFollowingAsync(info, from_id, to_id);

                return is_following;
            }

            public static async Task<IHelixResponse<bool>>
            IsUserFollowingAsync(string bearer_token, string client_id, string from_id, string to_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<bool> is_following = await Internal.IsUserFollowingAsync(info, from_id, to_id);

                return is_following;
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserRelationshipPageAsync(string bearer_token, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserRelationshipPageAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserRelationshipPageAsync(string bearer_token, string client_id, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserRelationshipPageAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserRelationshipAsync(string bearer_token, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserRelationshipAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserRelationshipAsync(string bearer_token, string client_id, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserRelationshipAsync(info, parameters);

                return response;
            }

            #endregion

            #region /videos

            public static async Task<IHelixResponse<DataPage<Video>>>
            GetVideosPageAsync(string bearer_token, VideosParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;

                IHelixResponse<DataPage<Video>> response = await Internal.GetVideosPageAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<Video>>>
            GetVideosPageAsync(string bearer_token, string client_id, VideosParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;
                info.client_id      = client_id;

                IHelixResponse<DataPage<Video>> response = await Internal.GetVideosPageAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<Video>>>
            GetVideosAsync(string bearer_token, VideosParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;

                IHelixResponse<DataPage<Video>> response = await Internal.GetVideosAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<Video>>>
            GetVideosAsync(string bearer_token, string client_id, VideosParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;
                info.client_id      = client_id;

                IHelixResponse<DataPage<Video>> response = await Internal.GetVideosAsync(info, parameters);

                return response;
            }

            #endregion
        }
    }
}