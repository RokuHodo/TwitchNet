// standard namespaces
using System.Threading.Tasks;

namespace
TwitchNet.Rest.Helix
{
    public static partial class
    HelixApi
    {
        #region /clips

        public static async Task<IHelixResponse<DataPage<Clip>>>
        GetClipsPageAsync(string client_id, ClipsParameters parameters, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<DataPage<Clip>> response = await Internal.GetClipsPageAsync(info, parameters);

            return response;
        }

        public static async Task<IHelixResponse<DataPage<Clip>>>
        GetClipsAsync(string client_id, ClipsParameters parameters, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<DataPage<Clip>> response = await Internal.GetClipsAsync(info, parameters);

            return response;
        }

        #endregion

        #region /games

        public static async Task<IHelixResponse<Data<Game>>>
        GetGamesAsync(string client_id, GamesParameters parameters, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<Data<Game>> response = await Internal.GetGamesAsync(info, parameters);

            return response;
        }

        #endregion

        #region /games/top

        public static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesPageAsync(string client_id, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            TopGamesParameters parameters = new TopGamesParameters();

            IHelixResponse<DataPage<Game>> response = await Internal.GetTopGamesPageAsync(info, parameters);

            return response;
        }

        public static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesPageAsync(string client_id, TopGamesParameters parameters, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<DataPage<Game>> top_games = await Internal.GetTopGamesPageAsync(info, parameters);

            return top_games;
        }

        public static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesAsync(string client_id, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            TopGamesParameters parameters = new TopGamesParameters();

            IHelixResponse<DataPage<Game>> response = await Internal.GetTopGamesAsync(info, parameters);

            return response;
        }

        public static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesAsync(string client_id, TopGamesParameters parameters, HelixRequestSettings settings = default)
        {
            HelixInfo request_info = new HelixInfo(settings);
            request_info.client_id = client_id;

            IHelixResponse<DataPage<Game>> response = await Internal.GetTopGamesAsync(request_info, parameters);

            return response;
        }

        #endregion

        #region /streams

        public static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsPageAsync(string client_id, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<DataPage<Stream>> response = await Internal.GetStreamsPageAsync(info, default);

            return response;
        }

        public static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsPageAsync(string client_id, StreamsParameters parameters, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<DataPage<Stream>> response = await Internal.GetStreamsPageAsync(info, parameters);

            return response;
        }

        public static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsAsync(string client_id, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<DataPage<Stream>> response = await Internal.GetStreamsAsync(info, default);

            return response;
        }

        public static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsAsync(string client_id, StreamsParameters parameters, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<DataPage<Stream>> response = await Internal.GetStreamsAsync(info, parameters);

            return response;
        }

        public static async Task<IHelixResponse<bool>>
        IsStreamLiveByUserIDAsync(string client_id, string user_id, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<bool> response = await Internal.IsStreamLiveByUserIDAsync(info, user_id);

            return response;
        }

        public static async Task<IHelixResponse<bool>>
        IsStreamLiveByUserLoginAsync(string client_id, string user_login, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<bool> response = await Internal.IsStreamLiveByUserLoginAsync(info, user_login);

            return response;
        }

        #endregion

        #region /streams/metadata

        public static async Task<IHelixResponse<DataPage<StreamMetadata>>>
        GetStreamsMetadataPageAsync(string client_id, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<DataPage<StreamMetadata>> response = await Internal.GetStreamsMetadataPageAsync(info, default);

            return response;
        }

        public static async Task<IHelixResponse<DataPage<StreamMetadata>>>
        GetStreamsMetadataPageAsync(string client_id, StreamsParameters parameters, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<DataPage<StreamMetadata>> response = await Internal.GetStreamsMetadataPageAsync(info, parameters);

            return response;
        }

        public static async Task<IHelixResponse<DataPage<StreamMetadata>>>
        GetStreamsMetadataAsync(string client_id, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<DataPage<StreamMetadata>> response = await Internal.GetStreamsMetadataAsync(info, default);

            return response;
        }

        public static async Task<IHelixResponse<DataPage<StreamMetadata>>>
        GetStreamsMetadataAsync(string client_id, StreamsParameters parameters, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<DataPage<StreamMetadata>> response = await Internal.GetStreamsMetadataAsync(info, parameters);

            return response;
        }

        #endregion

        #region /streams/tags

        public static async Task<IHelixResponse<Data<StreamTag>>>
        GetStreamsTagsAsync(string client_id, string broadcaster_id, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            StreamsTagsParameters parameters = new StreamsTagsParameters();
            parameters.broadcaster_id = broadcaster_id;

            IHelixResponse<Data<StreamTag>> response = await Internal.GetStreamsTagsAsync(info, parameters);

            return response;
        }

        public static async Task<IHelixResponse<Data<StreamTag>>>
        GetStreamsTagsAsync(string client_id, StreamsTagsParameters parameters, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<Data<StreamTag>> response = await Internal.GetStreamsTagsAsync(info, parameters);

            return response;
        }

        #endregion

        #region /tags/streams

        public static async Task<IHelixResponse<DataPage<StreamTag>>>
        GetStreamTagsPageAsync(string client_id, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<DataPage<StreamTag>> response = await Internal.GetStreamTagsPageAsync(info, default);

            return response;
        }

        public static async Task<IHelixResponse<DataPage<StreamTag>>>
        GetStreamTagsPageAsync(string client_id, StreamTagsParameters parameters, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<DataPage<StreamTag>> response = await Internal.GetStreamTagsPageAsync(info, parameters);

            return response;
        }

        public static async Task<IHelixResponse<DataPage<StreamTag>>>
        GetStreamTagsAsync(string client_id, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<DataPage<StreamTag>> response = await Internal.GetStreamTagsAsync(info, default);

            return response;
        }

        public static async Task<IHelixResponse<DataPage<StreamTag>>>
        GetStreamTagsAsync(string client_id, StreamTagsParameters parameters, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<DataPage<StreamTag>> response = await Internal.GetStreamTagsAsync(info, parameters);

            return response;
        }

        #endregion

        #region /users

        public static async Task<IHelixResponse<Data<User>>>
        GetUsersAsync(string client_id, UsersParameters parameters, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<Data<User>> response = await Internal.GetUsersAsync(info, parameters);

            return response;
        }

        #endregion

        #region /users/extensions

        public static async Task<IHelixResponse<ActiveExtensions>>
        GetUserActiveExtensionsAsync(string client_id, ActiveExtensionsParameters parameters, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<ActiveExtensions> response = await Internal.GetUserActiveExtensionsAsync(info, parameters);

            return response;
        }

        #endregion

        #region /users/follows

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingPageAsync(string client_id, string from_id, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            FollowsParameters parameters = new FollowsParameters();
            parameters.from_id = from_id;

            IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowingPageAsync(info, parameters);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingPageAsync(string client_id, FollowsParameters parameters, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowingPageAsync(info, parameters);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingAsync(string client_id, string from_id, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            FollowsParameters parameters = new FollowsParameters();
            parameters.from_id = from_id;

            IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowingAsync(info, parameters);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingAsync(string client_id, FollowsParameters parameters, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowingAsync(info, parameters);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersPageAsync(string client_id, string to_id, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            FollowsParameters parameters = new FollowsParameters();
            parameters.to_id = to_id;

            IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowersPageAsync(info, parameters);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersPageAsync(string client_id, FollowsParameters parameters, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowersPageAsync(info, parameters);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersAsync(string client_id, string to_id, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            FollowsParameters parameters = new FollowsParameters();
            parameters.to_id = to_id;

            IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowersAsync(info, parameters);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersAsync(string client_id, FollowsParameters parameters, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowersAsync(info, parameters);

            return response;
        }

        public static async Task<IHelixResponse<bool>>
        IsUserFollowingAsync(string client_id, string from_id, string to_id, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<bool> is_following = await Internal.IsUserFollowingAsync(info, from_id, to_id);

            return is_following;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowsRelationshipPageAsync(string client_id, FollowsParameters parameters, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowsRelationshipPageAsync(info, parameters);

            return response;
        }

        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowsRelationshipAsync(string client_id, FollowsParameters parameters, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<FollowsDataPage<Follow>> response = await Internal.GetUserFollowsRelationshipAsync(info, parameters);

            return response;
        }

        #endregion

        #region /videos

        public static async Task<IHelixResponse<DataPage<Video>>>
        GetVideosPageAsync(string client_id, VideosParameters parameters, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<DataPage<Video>> response = await Internal.GetVideosPageAsync(info, parameters);

            return response;
        }

        public static async Task<IHelixResponse<DataPage<Video>>>
        GetVideosAsync(string client_id, VideosParameters parameters, HelixRequestSettings settings = default)
        {
            HelixInfo info = new HelixInfo(settings);
            info.client_id = client_id;

            IHelixResponse<DataPage<Video>> response = await Internal.GetVideosAsync(info, parameters);

            return response;
        }

        #endregion
    }
}