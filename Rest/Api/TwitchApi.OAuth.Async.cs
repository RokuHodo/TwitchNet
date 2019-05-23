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
            #region /analytics/extensions

            public static async Task<IHelixResponse<Data<ExtensionAnalytics>>>
            GetExtensionAnalyticsAsync(string bearer_token, RequestSettings settings = default(RequestSettings))
            {
                HelixInfo request_info = new HelixInfo();
                request_info.bearer_token = bearer_token;

                IHelixResponse<Data<ExtensionAnalytics>> analytics = await TwitchApiInternal.GetExtensionAnalyticsAsync(request_info, default(ExtensionAnalyticsParameters), settings);

                return analytics;
            }

            public static async Task<IHelixResponse<Data<ExtensionAnalytics>>>
            GetExtensionAnalyticsAsync(string bearer_token, ExtensionAnalyticsParameters parameters, RequestSettings settings = default(RequestSettings))
            {
                HelixInfo request_info = new HelixInfo();
                request_info.bearer_token = bearer_token;

                IHelixResponse<Data<ExtensionAnalytics>> analytics = await TwitchApiInternal.GetExtensionAnalyticsAsync(request_info, parameters, settings);

                return analytics;
            }

            public static async Task<IHelixResponse<Data<ExtensionAnalytics>>>
            GetExtensionAnalyticsAsync(string bearer_token, string client_id, RequestSettings settings = default(RequestSettings))
            {
                HelixInfo request_info = new HelixInfo();
                request_info.bearer_token   = bearer_token;
                request_info.client_id      = client_id;

                IHelixResponse<Data<ExtensionAnalytics>> analytics = await TwitchApiInternal.GetExtensionAnalyticsAsync(request_info, default(ExtensionAnalyticsParameters), settings);

                return analytics;
            }

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

            #region /analytics/games

            public static async Task<IHelixResponse<Data<GameAnalytics>>>
            GetGameAnalyticsPageAsync(string bearer_token, RequestSettings settings = null)
            {
                HelixInfo request_info = new HelixInfo();
                request_info.bearer_token = bearer_token;

                IHelixResponse<Data<GameAnalytics>> response = await TwitchApiInternal.GetGameAnalyticsPageAsync(request_info, default(GameAnalyticsParameters), settings);

                return response;
            }

            public static async Task<IHelixResponse<Data<GameAnalytics>>>
            GetGameAnalyticsPageAsync(string bearer_token, GameAnalyticsParameters parameters, RequestSettings settings = null)
            {
                HelixInfo request_info = new HelixInfo();
                request_info.bearer_token = bearer_token;

                IHelixResponse<Data<GameAnalytics>> response = await TwitchApiInternal.GetGameAnalyticsPageAsync(request_info, parameters, settings);

                return response;
            }

            public static async Task<IHelixResponse<Data<GameAnalytics>>>
            GetGameAnalyticsPageAsync(string bearer_token, string client_id, RequestSettings settings = null)
            {
                HelixInfo request_info = new HelixInfo();
                request_info.bearer_token   = bearer_token;
                request_info.client_id      = client_id;

                IHelixResponse<Data<GameAnalytics>> response = await TwitchApiInternal.GetGameAnalyticsPageAsync(request_info, default(GameAnalyticsParameters), settings);

                return response;
            }

            public static async Task<IHelixResponse<Data<GameAnalytics>>>
            GetGameAnalyticsPageAsync(string bearer_token, string client_id, GameAnalyticsParameters parameters, RequestSettings settings = null)
            {
                HelixInfo request_info = new HelixInfo();
                request_info.bearer_token   = bearer_token;
                request_info.client_id      = client_id;

                IHelixResponse<Data<GameAnalytics>> response = await TwitchApiInternal.GetGameAnalyticsPageAsync(request_info, parameters, settings);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<GameAnalytics>>>
            GetGameAnalyticsAsync(string bearer_token, RequestSettings settings = null)
            {
                HelixInfo request_info = new HelixInfo();
                request_info.bearer_token = bearer_token;

                IHelixResponse<DataPage<GameAnalytics>> response = await TwitchApiInternal.GetGameAnalyticsAsync(request_info, default(GameAnalyticsParameters), settings);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<GameAnalytics>>>
            GetGameAnalyticsAsync(string bearer_token, GameAnalyticsParameters parameters, RequestSettings settings = null)
            {
                HelixInfo request_info = new HelixInfo();
                request_info.bearer_token = bearer_token;

                IHelixResponse<DataPage<GameAnalytics>> response = await TwitchApiInternal.GetGameAnalyticsAsync(request_info, parameters, settings);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<GameAnalytics>>>
            GetGameAnalyticsAsync(string bearer_token, string client_id, RequestSettings settings = null)
            {
                HelixInfo request_info = new HelixInfo();
                request_info.bearer_token = bearer_token;
                request_info.client_id = client_id;

                IHelixResponse<DataPage<GameAnalytics>> response = await TwitchApiInternal.GetGameAnalyticsAsync(request_info, default(GameAnalyticsParameters), settings);

                return response;
            }

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
            */

            #region /bits/leaderboard

            public static async Task<IHelixResponse<BitsLeaderboardData<BitsUser>>>
            GetBitsLeaderboardAsync(string bearer_token, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<BitsLeaderboardData<BitsUser>> response = await Internal.GetBitsLeaderboardAsync(info, default);

                return response;
            }

            public static async Task<IHelixResponse<BitsLeaderboardData<BitsUser>>>
            GetBitsLeaderboardAsync(string bearer_token, BitsLeaderboardParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<BitsLeaderboardData<BitsUser>> response = await Internal.GetBitsLeaderboardAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<BitsLeaderboardData<BitsUser>>>
            GetBitsLeaderboardAsync(string bearer_token, string client_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<BitsLeaderboardData<BitsUser>> response = await Internal.GetBitsLeaderboardAsync(info, default);

                return response;
            }

            public static async Task<IHelixResponse<BitsLeaderboardData<BitsUser>>>
            GetBitsLeaderboardAsync(string bearer_token, string client_id, BitsLeaderboardParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;
                info.client_id      = client_id;

                IHelixResponse<BitsLeaderboardData<BitsUser>> response = await Internal.GetBitsLeaderboardAsync(info, parameters);

                return response;
            }

            #endregion

            #region /clips

            public static async Task<IHelixResponse<Data<CreatedClip>>>
            CreateClipAsync(string bearer_token, ClipCreationParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<Data<CreatedClip>> response = await Internal.CreateClipAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<Data<CreatedClip>>>
            CreateClipAsync(string bearer_token, string client_id, ClipCreationParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;
                info.client_id      = client_id;

                IHelixResponse<Data<CreatedClip>> response = await Internal.CreateClipAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<Clip>>>
            GetClipsPageAsync(string bearer_token, ClipParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;

                IHelixResponse<DataPage<Clip>> response = await Internal.GetClipsPageAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<Clip>>>
            GetClipsPageAsync(string bearer_token, string client_id, ClipParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id = client_id;

                IHelixResponse<DataPage<Clip>> response = await Internal.GetClipsPageAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<Clip>>>
            GetClipsAsync(string bearer_token, ClipParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;

                IHelixResponse<DataPage<Clip>> response = await Internal.GetClipsAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<Clip>>>
            GetClipsAsync(string bearer_token, string client_id, ClipParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token = bearer_token;
                info.client_id  = client_id;

                IHelixResponse<DataPage<Clip>> response = await Internal.GetClipsAsync(info, parameters);

                return response;
            }

            #endregion

            #region /entitlements/upload

            public static async Task<IHelixResponse<Data<EntitlementUploadUrl>>>
            CreateEntitlementGrantsUploadUrlAsync(string app_access_token, EntitlementsUploadParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = app_access_token;

                IHelixResponse<Data<EntitlementUploadUrl>> response = await Internal.CreateEntitlementGrantsUploadUrlAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<Data<EntitlementUploadUrl>>>
            CreateEntitlementGrantsUploadUrlAsync(string app_access_token, string client_id, EntitlementsUploadParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = app_access_token;
                info.client_id      = client_id;

                IHelixResponse<Data<EntitlementUploadUrl>> response = await Internal.CreateEntitlementGrantsUploadUrlAsync(info, parameters);

                return response;
            }

            #endregion

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
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;

                IHelixResponse<DataPage<StreamMetadata>> response = await Internal.GetStreamsMetadataPageAsync(info, default);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<StreamMetadata>>>
            GetStreamsMetadataPageAsync(string bearer_token, StreamsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;

                IHelixResponse<DataPage<StreamMetadata>> response = await Internal.GetStreamsMetadataPageAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<StreamMetadata>>>
            GetStreamsMetadataPageAsync(string bearer_token, string client_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;
                info.client_id      = client_id;

                IHelixResponse<DataPage<StreamMetadata>> response = await Internal.GetStreamsMetadataPageAsync(info, default);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<StreamMetadata>>>
            GetStreamsMetadataPageAsync(string bearer_token, string client_id, StreamsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;
                info.client_id      = client_id;

                IHelixResponse<DataPage<StreamMetadata>> response = await Internal.GetStreamsMetadataPageAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<StreamMetadata>>>
            GetStreamsMetadataAsync(string bearer_token, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;

                IHelixResponse<DataPage<StreamMetadata>> response = await Internal.GetStreamsMetadataAsync(info, default);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<StreamMetadata>>>
            GetStreamsMetadataAsync(string bearer_token, StreamsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;

                IHelixResponse<DataPage<StreamMetadata>> response = await Internal.GetStreamsMetadataAsync(info, parameters);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<StreamMetadata>>>
            GetStreamsMetadataAsync(string bearer_token, string client_id, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;
                info.client_id      = client_id;

                IHelixResponse<DataPage<StreamMetadata>> response = await Internal.GetStreamsMetadataAsync(info, default);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<StreamMetadata>>>
            GetStreamsMetadataAsync(string bearer_token, string client_id, StreamsParameters parameters, HelixRequestSettings settings = default)
            {
                HelixInfo info = new HelixInfo(settings);
                info.bearer_token   = bearer_token;
                info.client_id      = client_id;

                IHelixResponse<DataPage<StreamMetadata>> response = await Internal.GetStreamsMetadataAsync(info, parameters);

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