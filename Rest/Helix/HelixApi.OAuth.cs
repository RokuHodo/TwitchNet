// standard namespaces
using System.Threading.Tasks;

namespace
TwitchNet.Rest.Helix
{
    public static partial class
    HelixApi
    {
        public static class
        OAuth
        {
            #region /analytics/extensions - DONE

            public static async Task<IHelixResponse<DataPage<ExtensionAnalytics>>>
            GetExtensionAnalyticsPageAsync(HelixAuthorization authorization, HelixRequestSettings settings = default)
            {
                return await Internal.GetExtensionAnalyticsAsync(RequestedPages.Single, authorization, default, settings);
            }

            public static async Task<IHelixResponse<DataPage<ExtensionAnalytics>>>
            GetExtensionAnalyticsPageAsync(HelixAuthorization authorization, ExtensionAnalyticsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetExtensionAnalyticsAsync(RequestedPages.Single, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<DataPage<ExtensionAnalytics>>>
            GetExtensionAnalyticsAsync(HelixAuthorization authorization, HelixRequestSettings settings = default)
            {
                return await Internal.GetExtensionAnalyticsAsync(RequestedPages.All, authorization, default, settings);
            }

            public static async Task<IHelixResponse<DataPage<ExtensionAnalytics>>>
            GetExtensionAnalyticsAsync(HelixAuthorization authorization, ExtensionAnalyticsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetExtensionAnalyticsAsync(RequestedPages.All, authorization, parameters, settings);
            }

            #endregion

            #region /analytics/games - DONE

            public static async Task<IHelixResponse<DataPage<GameAnalytics>>>
            GetGameAnalyticsPageAsync(HelixAuthorization authorization, HelixRequestSettings settings = default)
            {
                return await Internal.GetGameAnalyticsAsync(RequestedPages.Single, authorization, default, settings);
            }

            public static async Task<IHelixResponse<DataPage<GameAnalytics>>>
            GetGameAnalyticsPageAsync(HelixAuthorization authorization, GameAnalyticsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetGameAnalyticsAsync(RequestedPages.Single, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<DataPage<GameAnalytics>>>
            GetGameAnalyticsAsync(HelixAuthorization authorization, HelixRequestSettings settings = default)
            {
                return await Internal.GetGameAnalyticsAsync(RequestedPages.All, authorization, default, settings);
            }

            public static async Task<IHelixResponse<DataPage<GameAnalytics>>>
            GetGameAnalyticsAsync(HelixAuthorization authorization, GameAnalyticsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetGameAnalyticsAsync(RequestedPages.All, authorization, parameters, settings);
            }

            #endregion

            #region /bits/leaderboard - DONE

            public static async Task<IHelixResponse<BitsLeaderboardData<BitsUser>>>
            GetBitsLeaderboardAsync(HelixAuthorization authorization, HelixRequestSettings settings = default)
            {
                return await Internal.GetBitsLeaderboardAsync(authorization, default, settings);
            }

            public static async Task<IHelixResponse<BitsLeaderboardData<BitsUser>>>
            GetBitsLeaderboardAsync(HelixAuthorization authorization, BitsLeaderboardParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetBitsLeaderboardAsync(authorization, parameters, settings);
            }

            #endregion

            #region /clips - DONE

            public static async Task<IHelixResponse<Data<CreatedClip>>>
            CreateClipAsync(HelixAuthorization authorization, CreateClipParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.CreateClipAsync(authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<DataPage<Clip>>>
            GetClipsPageAsync(HelixAuthorization authorization, ClipsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetClipsAsync(RequestedPages.Single, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<DataPage<Clip>>>
            GetClipsAsync(HelixAuthorization authorization, ClipsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetClipsAsync(RequestedPages.All, authorization, parameters, settings);
            }

            #endregion

            #region /entitlements/codes

            public static async Task<IHelixResponse<Data<CodeStatus>>>
            GetEntitlementCodeStatusAsync(HelixAuthorization authorization, EntitlementsCodeParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetEntitlementCodeStatusAsync(authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<Data<CodeStatus>>>
            RedeemEntitlementCodeStatusAsync(HelixAuthorization authorization, EntitlementsCodeParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.RedeemEntitlementCodeStatusAsync(authorization, parameters, settings);
            }

            #endregion

            #region /entitlements/upload

            public static async Task<IHelixResponse<Data<EntitlementUploadUrl>>>
            CreateEntitlementGrantsUploadUrlAsync(HelixAuthorization authorization, EntitlementsUploadParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.CreateEntitlementGrantsUploadUrlAsync(authorization, parameters, settings);
            }

            #endregion

            #region /extensions/transactions

            public static async Task<IHelixResponse<DataPage<ExtensionTransaction>>>
            GetExtensionTransactionsPageAsync(HelixAuthorization authorization, ExtensionTransactionsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetExtensionTransactionsAsync(RequestedPages.Single, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<DataPage<ExtensionTransaction>>>
            GetExtensionTransactionsAsync(HelixAuthorization authorization, ExtensionTransactionsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetExtensionTransactionsAsync(RequestedPages.All, authorization, parameters, settings);
            }

            #endregion

            #region /games

            public static async Task<IHelixResponse<Data<Game>>>
            GetGamesAsync(HelixAuthorization authorization, GamesParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetGamesAsync(authorization, parameters, settings);
            }

            #endregion

            #region /games/top

            public static async Task<IHelixResponse<DataPage<Game>>>
            GetTopGamesPageAsync(HelixAuthorization authorization, HelixRequestSettings settings = default)
            {
                return await Internal.GetTopGamesAsync(RequestedPages.Single, authorization, default, settings);
            }

            public static async Task<IHelixResponse<DataPage<Game>>>
            GetTopGamesPageAsync(HelixAuthorization authorization, TopGamesParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetTopGamesAsync(RequestedPages.Single, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<DataPage<Game>>>
            GetTopGamesAsync(HelixAuthorization authorization, HelixRequestSettings settings = default)
            {
                return await Internal.GetTopGamesAsync(RequestedPages.All, authorization, default, settings);
            }

            public static async Task<IHelixResponse<DataPage<Game>>>
            GetTopGamesAsync(HelixAuthorization authorization, TopGamesParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetTopGamesAsync(RequestedPages.All, authorization, parameters, settings);
            }

            #endregion

            #region /moderation/banned

            public static async Task<IHelixResponse<DataPage<BannedUser>>>
            GetBannedUsersPageAsync(HelixAuthorization authorization, BannedUsersParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetBannedUsersAsync(RequestedPages.Single, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<DataPage<BannedUser>>>
            GetBannedUsersAsync(HelixAuthorization authorization, BannedUsersParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetBannedUsersAsync(RequestedPages.All, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<bool>>
            IsUserBannedAsync(HelixAuthorization authorization, string broadcaster_id, string user_id, HelixRequestSettings settings = default)
            {
                return await Internal.IsUserBannedAsync(authorization, broadcaster_id, user_id, settings);
            }

            #endregion

            #region /moderation/banned/events

            public static async Task<IHelixResponse<DataPage<BannedEvent>>>
            GetBannedEventsPageAsync(HelixAuthorization authorization, BannedEventsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetBannedEventsAsync(RequestedPages.Single, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<DataPage<BannedEvent>>>
            GetBannedEventsAsync(HelixAuthorization authorization, BannedEventsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetBannedEventsAsync(RequestedPages.All, authorization, parameters, settings);
            }

            #endregion

            #region /moderation/enforcements/status

            public static async Task<IHelixResponse<Data<AutoModMessageStatus>>>
            CheckAutoModMessageStatus(HelixAuthorization authorization, AutoModMessageStatusParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.CheckAutoModMessageStatus(authorization, parameters, settings);
            }

            #endregion

            #region /moderation/moderators

            public static async Task<IHelixResponse<DataPage<Moderator>>>
            GetModeratorsPageAsync(HelixAuthorization authorization, ModeratorsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetModeratorsAsync(RequestedPages.Single, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<DataPage<Moderator>>>
            GetModeratorsAsync(HelixAuthorization authorization, ModeratorsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetModeratorsAsync(RequestedPages.All, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<bool>>
            IsUserModeratorAsync(HelixAuthorization authorization, string broadcaster_id, string user_id, HelixRequestSettings settings = default)
            {
                return await Internal.IsUserModeratorAsync(authorization, broadcaster_id, user_id, settings);
            }

            #endregion

            #region /moderation/moderators/events

            public static async Task<IHelixResponse<DataPage<ModeratorEvent>>>
            GetModeratorEventsPageAsync(HelixAuthorization authorization, ModeratorEventsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetModeratorEventsAsync(RequestedPages.Single, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<DataPage<ModeratorEvent>>>
            GetModeratorEventsAsync(HelixAuthorization authorization, ModeratorEventsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetModeratorEventsAsync(RequestedPages.All, authorization, parameters, settings);
            }

            #endregion

            #region /streams

            public static async Task<IHelixResponse<DataPage<Stream>>>
            GetStreamsPageAsync(HelixAuthorization authorization, HelixRequestSettings settings = default)
            {
                return await Internal.GetStreamsAsync(RequestedPages.Single, authorization, default, settings);
            }

            public static async Task<IHelixResponse<DataPage<Stream>>>
            GetStreamsPageAsync(HelixAuthorization authorization, StreamsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetStreamsAsync(RequestedPages.Single, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<DataPage<Stream>>>
            GetStreamsAsync(HelixAuthorization authorization, HelixRequestSettings settings = default)
            {
                return await Internal.GetStreamsAsync(RequestedPages.All, authorization, default, settings);
            }

            public static async Task<IHelixResponse<DataPage<Stream>>>
            GetStreamsAsync(HelixAuthorization authorization, StreamsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetStreamsAsync(RequestedPages.All, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<bool>>
            IsStreamLiveAsync_UserID(HelixAuthorization authorization, string user_id, HelixRequestSettings settings = default)
            {
                return await Internal.IsStreamLiveAsync_UserID(authorization, user_id, settings);
            }

            public static async Task<IHelixResponse<bool>>
            IsStreamLiveAsync_Login(HelixAuthorization authorization, string user_login, HelixRequestSettings settings = default)
            {
                return await Internal.IsStreamLiveAsync_Login(authorization, user_login, settings);
            }

            #endregion

            #region /streams/markers

            public static async Task<IHelixResponse<DataPage<CreatedStreamMarker>>>
            CreateStreamMarkerAsync(HelixAuthorization authorization, CreateStreamMarkerParameters parameters,  HelixRequestSettings settings = default)
            {
                return await Internal.CreateStreamMarkerAsync(authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<DataPage<StreamMarkers>>>
            GetStreamMarkersPageAsync(HelixAuthorization authorization, StreamMarkersParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetStreamMarkersAsync(RequestedPages.Single, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<DataPage<StreamMarkers>>>
            GetStreamMarkersAsync(HelixAuthorization authorization, StreamMarkersParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetStreamMarkersAsync(RequestedPages.All, authorization, parameters, settings);
            }

            #endregion

            #region /streams/tags

            public static async Task<IHelixResponse<Data<StreamTag>>>
            GetStreamsTagsAsync(HelixAuthorization authorization, string broadcaster_id, HelixRequestSettings settings = default)
            {
                StreamsTagsParameters parameters = new StreamsTagsParameters();
                parameters.broadcaster_id = broadcaster_id;

                return await Internal.GetSetStreamTagsAsync(authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<Data<StreamTag>>>
            GetStreamsTagsAsync(HelixAuthorization authorization, StreamsTagsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetSetStreamTagsAsync(authorization, parameters, settings);
            }

            public static async Task<IHelixResponse>
            SetStreamsTagsAsync(HelixAuthorization authorization, SetStreamsTagsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.SetStreamTagsAsync(authorization, parameters, settings);
            }

            public static async Task<IHelixResponse>
            RemoveStreamsTagsAsync(HelixAuthorization authorization, string broadcaster_id, HelixRequestSettings settings = default)
            {
                SetStreamsTagsParameters parameters = new SetStreamsTagsParameters();
                parameters.broadcaster_id = broadcaster_id;

                return await Internal.RemoveStreamTagsAsync(authorization, parameters, settings);
            }

            public static async Task<IHelixResponse>
            RemoveStreamsTagsAsync(HelixAuthorization authorization, SetStreamsTagsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.RemoveStreamTagsAsync(authorization, parameters, settings);
            }

            #endregion

            #region /subscriptions

            public static async Task<IHelixResponse<DataPage<Subscription>>>
            GetBroadcasterSubscribersPageAsync(HelixAuthorization authorization, SubscriptionParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetBroadcasterSubscribersAsync(RequestedPages.Single, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<DataPage<Subscription>>>
            GetBroadcasterSubscribersAsync(HelixAuthorization authorization, SubscriptionParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetBroadcasterSubscribersAsync(RequestedPages.All, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<Data<Subscription>>>
            GetSubscriptionRelationshipAsync(HelixAuthorization authorization, SubscriptionRelationshipParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetSubscriptionRelationshipAsync(authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<bool>>
            IsUserSubscribedAsync(HelixAuthorization authorization, string broadcaster_id, string user_id, HelixRequestSettings settings = default)
            {
                return await Internal.IsUserSubscribedAsync(authorization, broadcaster_id, user_id, settings);
            }

            #endregion

            #region /subscriptions/events

            public static async Task<IHelixResponse<DataPage<SubscriptionEvent>>>
            GetSubscriptionEventsPageAsync(HelixAuthorization authorization, SubscriptionEventsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetSubscriptionEventsAsync(RequestedPages.Single, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<DataPage<SubscriptionEvent>>>
            GetSubscriptionEventsAsync(HelixAuthorization authorization, SubscriptionEventsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetSubscriptionEventsAsync(RequestedPages.All, authorization, parameters, settings);
            }

            #endregion

            #region /tags/streams

            public static async Task<IHelixResponse<DataPage<StreamTag>>>
            GetStreamTagsPageAsync(HelixAuthorization authorization, HelixRequestSettings settings = default)
            {
                return await Internal.GetStreamTagsAsync(RequestedPages.Single, authorization, default, settings);
            }

            public static async Task<IHelixResponse<DataPage<StreamTag>>>
            GetStreamTagsPageAsync(HelixAuthorization authorization, StreamTagsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetStreamTagsAsync(RequestedPages.Single, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<DataPage<StreamTag>>>
            GetStreamTagsAsync(HelixAuthorization authorization, HelixRequestSettings settings = default)
            {
                return await Internal.GetStreamTagsAsync(RequestedPages.All, authorization, default, settings);
            }

            public static async Task<IHelixResponse<DataPage<StreamTag>>>
            GetStreamTagsAsync(HelixAuthorization authorization, StreamTagsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetStreamTagsAsync(RequestedPages.All, authorization, parameters, settings);
            }

            #endregion

            #region /users

            public static async Task<IHelixResponse<Data<User>>>
            GetUserAsync(HelixAuthorization authorization, HelixRequestSettings settings = default)
            {
                return await Internal.GetUsersAsync(authorization, default, settings);
            }

            public static async Task<IHelixResponse<Data<User>>>
            GetUsersAsync(HelixAuthorization authorization, UsersParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetUsersAsync(authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<Data<User>>>
            SetUserDescriptionAsync(HelixAuthorization authorization, string description, HelixRequestSettings settings = default)
            {
                return await Internal.SetUserDescriptionAsync(authorization, description, settings);
            }

            public static async Task<IHelixResponse<Data<User>>>
            SetUserDescriptionAsync(HelixAuthorization authorization, DescriptionParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.SetUserDescriptionAsync(authorization, parameters, settings);
            }

            #endregion

            #region /users/extensions

            public static async Task<IHelixResponse<ActiveExtensions>>
            GetUserActiveExtensionsAsync(HelixAuthorization authorization, HelixRequestSettings settings = default)
            {
                return await Internal.GetUserActiveExtensionsAsync(authorization, default, settings);
            }

            public static async Task<IHelixResponse<ActiveExtensions>>
            GetUserActiveExtensionsAsync(HelixAuthorization authorization, ActiveExtensionsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetUserActiveExtensionsAsync(authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<ActiveExtensions>>
            UpdateUserActiveExtensionsAsync(HelixAuthorization authorization, UpdateExtensionsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.UpdateUserActiveExtensionsAsync(authorization, parameters, settings);
            }

            #endregion

            #region /users/extensions/list

            public static async Task<IHelixResponse<Data<Extension>>>
            GetUserExtensionsAsync(HelixAuthorization authorization, HelixRequestSettings settings = default)
            {
                return await Internal.GetUserExtensionsAsync(authorization, settings);
            }

            #endregion

            #region /users/follows

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingPageAsync(HelixAuthorization authorization, string from_id, HelixRequestSettings settings = default)
            {
                FollowsParameters parameters = new FollowsParameters();
                parameters.from_id = from_id;

                return await Internal.GetUserFollowingAsync(RequestedPages.Single, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingPageAsync(HelixAuthorization authorization, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetUserFollowingAsync(RequestedPages.Single, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingAsync(HelixAuthorization authorization, string from_id, HelixRequestSettings settings = default)
            {
                FollowsParameters parameters = new FollowsParameters();
                parameters.from_id = from_id;

                return await Internal.GetUserFollowingAsync(RequestedPages.All, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingAsync(HelixAuthorization authorization, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetUserFollowingAsync(RequestedPages.All, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersPageAsync(HelixAuthorization authorization, string to_id, HelixRequestSettings settings = default)
            {
                FollowsParameters parameters = new FollowsParameters();
                parameters.to_id = to_id;

                return await Internal.GetUserFollowersAsync(RequestedPages.Single, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersPageAsync(HelixAuthorization authorization, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetUserFollowersAsync(RequestedPages.Single, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersAsync(HelixAuthorization authorization, string to_id, HelixRequestSettings settings = default)
            {
                FollowsParameters parameters = new FollowsParameters();
                parameters.to_id = to_id;

                return await Internal.GetUserFollowersAsync(RequestedPages.All, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersAsync(HelixAuthorization authorization, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetUserFollowersAsync(RequestedPages.All, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<bool>>
            IsUserFollowingAsync(HelixAuthorization authorization, string from_id, string to_id, HelixRequestSettings settings = default)
            {
                return await Internal.IsUserFollowingAsync(authorization, from_id, to_id, settings);
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowsRelationshipPageAsync(HelixAuthorization authorization, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetUserFollowsRelationshipAsync(RequestedPages.Single, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowsRelationshipAsync(HelixAuthorization authorization, FollowsParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetUserFollowsRelationshipAsync(RequestedPages.All, authorization, parameters, settings);
            }

            #endregion

            #region /videos

            public static async Task<IHelixResponse<DataPage<Video>>>
            GetVideosPageAsync(HelixAuthorization authorization, VideosParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetVideosAsync(RequestedPages.Single, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<DataPage<Video>>>
            GetVideosAsync(HelixAuthorization authorization, VideosParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetVideosAsync(RequestedPages.All, authorization, parameters, settings);
            }

            #endregion

            #region /webhooks/subscriptions

            public static async Task<IHelixResponse<WebhookDataPage<WebhookSubscription>>>
            GetWebhookSubscriptionsPageAsync(HelixAuthorization authorization, HelixRequestSettings settings = default)
            {
                return await Internal.GetWebhookSubscriptionsAsync(RequestedPages.Single, authorization, default, settings);
            }

            public static async Task<IHelixResponse<WebhookDataPage<WebhookSubscription>>>
            GetWebhookSubscriptionsPageAsync(HelixAuthorization authorization, PagingParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetWebhookSubscriptionsAsync(RequestedPages.Single, authorization, parameters, settings);
            }

            public static async Task<IHelixResponse<WebhookDataPage<WebhookSubscription>>>
            GetWebhookSubscriptionsAsync(HelixAuthorization authorization, HelixRequestSettings settings = default)
            {
                return await Internal.GetWebhookSubscriptionsAsync(RequestedPages.All, authorization, default, settings);
            }

            public static async Task<IHelixResponse<WebhookDataPage<WebhookSubscription>>>
            GetWebhookSubscriptionsAsync(HelixAuthorization authorization, PagingParameters parameters, HelixRequestSettings settings = default)
            {
                return await Internal.GetWebhookSubscriptionsAsync(RequestedPages.All, authorization, parameters, settings);
            }

            #endregion
        }
    }
}