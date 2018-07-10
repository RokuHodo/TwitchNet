// standard namespaces
using System.Threading.Tasks;

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

            IHelixResponse<Data<ExtensionAnalytics>> analytics = await TwitchApiInternal.GetExtensionAnalyticsAsync(request_info, default(ExtensionAnalyticsQueryParameters), settings);

            return analytics;
        }

        /// <summary>
        /// <para>Asynchronously gets analytic urls for one or more devloper extensions.</para>
        /// <para>Required Scope: 'analytics:read:extensions'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<ExtensionAnalytics>>>
        GetExtensionAnalyticsAsync(string bearer_token, ExtensionAnalyticsQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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

            IHelixResponse<Data<ExtensionAnalytics>> analytics = await TwitchApiInternal.GetExtensionAnalyticsAsync(request_info, default(ExtensionAnalyticsQueryParameters), settings);

            return analytics;
        }

        /// <summary>
        /// <para>Asynchronously gets analytic urls for one or more devloper extensions.</para>
        /// <para>Required Scope: 'analytics:read:extensions'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<ExtensionAnalytics>>>
        GetExtensionAnalyticsAsync(string bearer_token, string client_id, ExtensionAnalyticsQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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

            IHelixResponse<Data<GameAnalytics>> respose = await TwitchApiInternal.GetGameAnalyticsPageAsync(request_info, default(GameAnalyticsQueryParameters), settings);

            return respose;
        }

        /// <summary>
        /// <para>Asynchronously gets a single page of analytic urls for one or more devloper games.</para>
        /// <para>Required Scope: 'analytics:read:games'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<GameAnalytics>>>
        GetGameAnalyticsPageAsync(string bearer_token, GameAnalyticsQueryParameters parameters, RequestSettings settings = null)
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token = bearer_token;

            IHelixResponse<Data<GameAnalytics>> respose = await TwitchApiInternal.GetGameAnalyticsPageAsync(request_info, parameters, settings);

            return respose;
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

            IHelixResponse<Data<GameAnalytics>> respose = await TwitchApiInternal.GetGameAnalyticsPageAsync(request_info, default(GameAnalyticsQueryParameters), settings);

            return respose;
        }

        /// <summary>
        /// <para>Asynchronously gets a single page of analytic urls for one or more devloper games.</para>
        /// <para>Required Scope: 'analytics:read:games'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<GameAnalytics>>>
        GetGameAnalyticsPageAsync(string bearer_token, string client_id, GameAnalyticsQueryParameters parameters, RequestSettings settings = null)
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<Data<GameAnalytics>> respose = await TwitchApiInternal.GetGameAnalyticsPageAsync(request_info, parameters, settings);

            return respose;
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

            IHelixResponse<DataPage<GameAnalytics>> respose = await TwitchApiInternal.GetGameAnalyticsAsync(request_info, default(GameAnalyticsQueryParameters), settings);

            return respose;
        }

        /// <summary>
        /// <para>Asynchronously gets a complete list of analytic urls for one or more devloper games.</para>
        /// <para>Required Scope: 'analytics:read:games'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<GameAnalytics>>>
        GetGameAnalyticsAsync(string bearer_token, GameAnalyticsQueryParameters parameters, RequestSettings settings = null)
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token = bearer_token;

            IHelixResponse<DataPage<GameAnalytics>> respose = await TwitchApiInternal.GetGameAnalyticsAsync(request_info, parameters, settings);

            return respose;
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

            IHelixResponse<DataPage<GameAnalytics>> respose = await TwitchApiInternal.GetGameAnalyticsAsync(request_info, default(GameAnalyticsQueryParameters), settings);

            return respose;
        }

        /// <summary>
        /// <para>Asynchronously gets a complete list of analytic urls for one or more devloper games.</para>
        /// <para>Required Scope: 'analytics:read:games'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<GameAnalytics>>>
        GetGameAnalyticsAsync(string bearer_token, string client_id, GameAnalyticsQueryParameters parameters, RequestSettings settings = null)
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token = bearer_token;
            request_info.client_id = client_id;

            IHelixResponse<DataPage<GameAnalytics>> respose = await TwitchApiInternal.GetGameAnalyticsAsync(request_info, parameters, settings);

            return respose;
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

            IHelixResponse<BitsLeaderboardData<BitsUser>> respose = await TwitchApiInternal.GetBitsLeaderboardAsync(request_info, default(BitsLeaderboardQueryParameters), settings);

            return respose;
        }

        /// <summary>
        /// <para>Asynchronously gets a ranked list of bits leaderboard information for an authorized broadcaster.</para>
        /// <para>Required Scope: 'bits:read'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<BitsLeaderboardData<BitsUser>>>
        GetBitsLeaderboardAsync(string bearer_token, BitsLeaderboardQueryParameters parameters, RequestSettings settings = null)
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token = bearer_token;

            IHelixResponse<BitsLeaderboardData<BitsUser>> respose = await TwitchApiInternal.GetBitsLeaderboardAsync(request_info, parameters, settings);

            return respose;
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

            IHelixResponse<BitsLeaderboardData<BitsUser>> respose = await TwitchApiInternal.GetBitsLeaderboardAsync(request_info, default(BitsLeaderboardQueryParameters), settings);

            return respose;
        }

        /// <summary>
        /// <para>Asynchronously gets a ranked list of bits leaderboard information for an authorized broadcaster.</para>
        /// <para>Required Scope: 'bits:read'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<BitsLeaderboardData<BitsUser>>>
        GetBitsLeaderboardAsync(string bearer_token, string client_id, BitsLeaderboardQueryParameters parameters, RequestSettings settings = null)
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<BitsLeaderboardData<BitsUser>> respose = await TwitchApiInternal.GetBitsLeaderboardAsync(request_info, parameters, settings);

            return respose;
        }

        #endregion

        #region /clips

        /// <summary>
        /// <para>Asynchronously creates a clip.</para>
        /// <para>Required Scope: 'clips:edit'.</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<CreatedClip>>>
        CreateClipAsync(string bearer_token, ClipCreationQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<CreatedClip>>>
        CreateClipAsync(string bearer_token, string client_id, ClipCreationQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<Clip>>>
        GetClipAsync(string bearer_token, ClipQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<Clip>>>
        GetClipAsync(string bearer_token, string client_id, ClipQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<Url>>>
        CreateEntitlementGrantsUploadUrlAsync(string application_token, EntitlementQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<Url>>>
        CreateEntitlementGrantsUploadUrlAsync(string application_token, string client_id, EntitlementQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<Game>>>
        GetGamesAsync(string bearer_token, GamesQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<Game>>>
        GetGamesAsync(string bearer_token, string client_id, GamesQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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

            IHelixResponse<DataPage<Game>> top_games = await TwitchApiInternal.GetTopGamesPageAsync(request_info, default(TopGamesQueryParameters), settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesPageAsync(string bearer_token, TopGamesQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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

            IHelixResponse<DataPage<Game>> top_games = await TwitchApiInternal.GetTopGamesPageAsync(request_info, default(TopGamesQueryParameters), settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesPageAsync(string bearer_token, string client_id, TopGamesQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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

            IHelixResponse<DataPage<Game>> top_games = await TwitchApiInternal.GetTopGamesAsync(request_info, default(TopGamesQueryParameters), settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesAsync(string bearer_token, TopGamesQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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

            IHelixResponse<DataPage<Game>> top_games = await TwitchApiInternal.GetTopGamesAsync(request_info, default(TopGamesQueryParameters), settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesAsync(string bearer_token, string client_id, TopGamesQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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

            IHelixResponse<DataPage<Stream>> streams = await TwitchApiInternal.GetStreamsPageAsync(request_info, default(StreamsQueryParameters), settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsPageAsync(string bearer_token, StreamsQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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

            IHelixResponse<DataPage<Stream>> streams = await TwitchApiInternal.GetStreamsPageAsync(request_info, default(StreamsQueryParameters), settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsPageAsync(string bearer_token,string client_id, StreamsQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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

            IHelixResponse<DataPage<Stream>> streams = await TwitchApiInternal.GetStreamsAsync(request_info, default(StreamsQueryParameters), settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsAsync(string bearer_token, StreamsQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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

            IHelixResponse<DataPage<Stream>> streams = await TwitchApiInternal.GetStreamsAsync(request_info, default(StreamsQueryParameters), settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsAsync(string bearer_token, string client_id, StreamsQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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

            IHelixResponse<DataPage<Metadata>> metadata = await TwitchApiInternal.GetStreamsMetadataPageAsync(request_info, default(StreamsQueryParameters), settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Metadata>>>
        GetStreamsMetadataPageAsync(string bearer_token, StreamsQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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

            IHelixResponse<DataPage<Metadata>> metadata = await TwitchApiInternal.GetStreamsMetadataPageAsync(request_info, default(StreamsQueryParameters), settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Metadata>>>
        GetStreamsMetadataPageAsync(string bearer_token, string client_id, StreamsQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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

            IHelixResponse<DataPage<Metadata>> metadata = await TwitchApiInternal.GetStreamsMetadataAsync(request_info, default(StreamsQueryParameters), settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Metadata>>>
        GetStreamsMetadataAsync(string bearer_token, StreamsQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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

            IHelixResponse<DataPage<Metadata>> metadata = await TwitchApiInternal.GetStreamsMetadataAsync(request_info, default(StreamsQueryParameters), settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Metadata>>>
        GetStreamsMetadataAsync(string bearer_token, string client_id, StreamsQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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

            IHelixResponse<Data<User>> users = await TwitchApiInternal.GetUsersAsync(request_info, default(UsersQueryParameters), settings);

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

            IHelixResponse<Data<User>> users = await TwitchApiInternal.GetUsersAsync(request_info, default(UsersQueryParameters), settings);

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
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<User>>>
        GetUsersAsync(string bearer_token, UsersQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<Data<User>>>
        GetUsersAsync(string bearer_token, string client_id, UsersQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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

        #region /users/follows

        /// <summary>
        /// Asynchronously gets the relationship between two users.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserRelationshipAsync(string bearer_token, string from_id, string to_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<FollowsDataPage<Follow>> relationship = await TwitchApiInternal.GetUserRelationshipPageAsync(request_info, from_id, to_id, default(FollowsQueryParameters), settings);

            return relationship;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserRelationshipAsync(string bearer_token, string client_id, string from_id, string to_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<FollowsDataPage<Follow>> relationship = await TwitchApiInternal.GetUserRelationshipPageAsync(request_info, from_id, to_id, default(FollowsQueryParameters), settings);

            return relationship;
        }        

        /// <summary>
        /// Asynchronously gets a single page of a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingPageAsync(string bearer_token, string from_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<FollowsDataPage<Follow>> following = await TwitchApiInternal.GetUserRelationshipPageAsync(request_info, from_id, string.Empty, default(FollowsQueryParameters), settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a single page of a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingPageAsync(string bearer_token, string from_id, FollowsQueryParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<FollowsDataPage<Follow>> following = await TwitchApiInternal.GetUserRelationshipPageAsync(request_info, from_id, string.Empty, parameters, settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a single page of a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingPageAsync(string bearer_token, string client_id, string from_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<FollowsDataPage<Follow>> following = await TwitchApiInternal.GetUserRelationshipPageAsync(request_info, from_id, string.Empty, default(FollowsQueryParameters), settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a single page of a user's following list.
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
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingPageAsync(string bearer_token, string client_id, string from_id, FollowsQueryParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<FollowsDataPage<Follow>> following = await TwitchApiInternal.GetUserRelationshipPageAsync(request_info, from_id, string.Empty, parameters, settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingAsync(string bearer_token, string from_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<FollowsDataPage<Follow>> following = await TwitchApiInternal.GetUserRelationshipAsync(request_info, from_id, string.Empty, default(FollowsQueryParameters), settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingAsync(string bearer_token, string from_id, FollowsQueryParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token = bearer_token;

            IHelixResponse<FollowsDataPage<Follow>> following = await TwitchApiInternal.GetUserRelationshipAsync(request_info, from_id, string.Empty, parameters, settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a user's following list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to get the following list from.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingAsync(string bearer_token, string client_id, string from_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<FollowsDataPage<Follow>> following = await TwitchApiInternal.GetUserRelationshipAsync(request_info, from_id, string.Empty, default(FollowsQueryParameters), settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a user's following list.
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
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingAsync(string bearer_token, string client_id, string from_id, FollowsQueryParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<FollowsDataPage<Follow>> following = await TwitchApiInternal.GetUserRelationshipAsync(request_info, from_id, string.Empty, parameters, settings);

            return following;
        }

        /// <summary>
        /// Asynchronously gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersPageAsync(string bearer_token, string to_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<FollowsDataPage<Follow>> followers = await TwitchApiInternal.GetUserRelationshipPageAsync(request_info, string.Empty, to_id, default(FollowsQueryParameters), settings);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersPageAsync(string bearer_token, string to_id, FollowsQueryParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<FollowsDataPage<Follow>> followers = await TwitchApiInternal.GetUserRelationshipPageAsync(request_info, string.Empty, to_id, parameters, settings);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a single paged of a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersPageAsync(string bearer_token, string client_id, string to_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<FollowsDataPage<Follow>> followers = await TwitchApiInternal.GetUserRelationshipPageAsync(request_info, string.Empty, to_id, default(FollowsQueryParameters), settings);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a single paged of a user's followers list.
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
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersPageAsync(string bearer_token, string client_id, string to_id, FollowsQueryParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<FollowsDataPage<Follow>> followers = await TwitchApiInternal.GetUserRelationshipPageAsync(request_info, string.Empty, to_id, parameters, settings);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersAsync(string bearer_token, string to_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<FollowsDataPage<Follow>> followers = await TwitchApiInternal.GetUserRelationshipAsync(request_info, string.Empty, to_id, default(FollowsQueryParameters), settings);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersAsync(string bearer_token, string to_id, FollowsQueryParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<FollowsDataPage<Follow>> followers = await TwitchApiInternal.GetUserRelationshipAsync(request_info, string.Empty, to_id, parameters, settings);

            return followers;
        }

        /// <summary>
        /// Asynchronously gets a user's followers list.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="to_id">The user to get the followers for.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersAsync(string bearer_token, string client_id, string to_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<FollowsDataPage<Follow>> followers = await TwitchApiInternal.GetUserRelationshipAsync(request_info, string.Empty, to_id, default(FollowsQueryParameters), settings);

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
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersAsync(string bearer_token, string client_id, string to_id, FollowsQueryParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<FollowsDataPage<Follow>> followers = await TwitchApiInternal.GetUserRelationshipAsync(request_info, string.Empty, to_id, parameters, settings);

            return followers;
        }

        /// <summary>
        /// Asynchronously checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<bool>>
        IsUserFollowingAsync(string bearer_token, string from_id, string to_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;

            IHelixResponse<bool> is_following = await TwitchApiInternal.IsUserFollowingAsync(request_info, from_id, to_id, settings);

            return is_following;
        }

        /// <summary>
        /// Asynchronously checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<bool>>
        IsUserFollowingAsync(string bearer_token, string client_id, string from_id, string to_id, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<bool> is_following = await TwitchApiInternal.IsUserFollowingAsync(request_info, from_id, to_id, settings);

            return is_following;
        }

        #endregion        

        #region /videos

        /// <summary>
        /// Asynchronously gets a single page of information on one or more videos.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Video>>>
        GetVideosPageAsync(string bearer_token, VideosQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Video>>>
        GetVideosPageAsync(string bearer_token, string client_id, VideosQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Video>>>
        GetVideosAsync(string bearer_token, VideosQueryParameters parameters, RequestSettings settings = default(RequestSettings))
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
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        public static async Task<IHelixResponse<DataPage<Video>>>
        GetVideosAsync(string bearer_token, string client_id, VideosQueryParameters parameters, RequestSettings settings = default(RequestSettings))
        {
            HelixInfo request_info = new HelixInfo();
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IHelixResponse<DataPage<Video>> videos = await TwitchApiInternal.GetVideosAsync(request_info, parameters, settings);

            return videos;
        }

        #endregion
    }
}