// standard namespaces
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Enums.Api;
using TwitchNet.Extensions;
using TwitchNet.Interfaces.Api;
using TwitchNet.Models.Api;
using TwitchNet.Models.Api.Clips;
using TwitchNet.Models.Api.Entitlements;
using TwitchNet.Models.Api.Games;
using TwitchNet.Models.Api.Streams;
using TwitchNet.Models.Api.Users;
using TwitchNet.Models.Api.Videos;
using TwitchNet.Utilities;

// imported .dll's
using RestSharp;

namespace
TwitchNet.Api
{
    internal static class
    TwitchApiInternal
    {
        #region Fields

        private static readonly ClientInfo client_info = ClientInfo.DefaultHelix;

        #endregion

        // TODO: /analytics/games

        // TODO: /bits/leaderboard

        #region /clips

        /// <summary>
        /// <para>Asynchronously creates a clip.</para>
        /// <para>Required Scope: 'clips:edit'.</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<Data<CreatedClip>>>
        CreateClipAsync(string bearer_token, string client_id, ClipCreationQueryParameters query_parameters, ApiRequestSettings settings = null)
        {
            if (settings.IsNull())
            {
                settings = new ApiRequestSettings();
            }            

            if (settings.input_hanlding == InputHandling.Error)
            {
                ExceptionUtil.ThrowIfNullOrDefault(query_parameters, nameof(query_parameters));
                ExceptionUtil.ThrowIfInvalid(query_parameters.broadcaster_id, nameof(query_parameters.broadcaster_id));
            }

            RequestInfo request_info    = new RequestInfo();
            request_info.endpoint       = "clips";
            request_info.method         = Method.POST;
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IApiResponse<Data<CreatedClip>> clip_creation = await RestRequestUtil.ExecuteAsync<Data<CreatedClip>, ClipCreationQueryParameters>(client_info, request_info, query_parameters, settings);

            return clip_creation;
        }

        /// <summary>
        /// Asynchronously gets information about a clip.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<Data<Clip>>>
        GetClipAsync(string bearer_token, string client_id, ClipQueryParameters query_parameters, ApiRequestSettings settings)
        {
            if (settings.IsNull())
            {
                settings = new ApiRequestSettings();
            }

            if (settings.input_hanlding == InputHandling.Error)
            {
                ExceptionUtil.ThrowIfNullOrDefault(query_parameters, nameof(query_parameters));
                ExceptionUtil.ThrowIfInvalid(query_parameters.id, nameof(query_parameters.id));
            }

            RequestInfo request_info    = new RequestInfo();
            request_info.endpoint       = "clips";
            request_info.method         = Method.GET;
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IApiResponse<Data<Clip>> clip = await RestRequestUtil.ExecuteAsync<Data<Clip>, ClipQueryParameters>(client_info, request_info, query_parameters, settings);

            return clip;
        }

        #endregion

        #region /entitlements/upload

        /// <summary>
        /// Asynchronously creates a URL where you can upload a manifest file and notify users that they have an entitlement.
        /// </summary>
        /// <param name="app_access_token">The application access token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<Data<Url>>>
        CreateEntitlementGrantsUploadUrlAsync(string app_access_token, string client_id, EntitlementQueryParameters query_parameters, ApiRequestSettings settings)
        {
            if (settings.IsNull())
            {
                settings = new ApiRequestSettings();
            }

            if (settings.input_hanlding == InputHandling.Error)
            {
                ExceptionUtil.ThrowIfNullOrDefault(query_parameters, nameof(query_parameters));
                ExceptionUtil.ThrowIfInvalid(query_parameters.manifest_id, nameof(query_parameters.manifest_id));
                if (!query_parameters.manifest_id.Length.IsInRange(1, 64))
                {
                    throw new ArgumentOutOfRangeException(nameof(query_parameters.manifest_id), query_parameters.manifest_id, nameof(query_parameters.manifest_id) + " must be between 1 and 64 characters, inclusive.");
                }
                ExceptionUtil.ThrowIfNull(query_parameters.type, nameof(query_parameters.type));
            }

            RequestInfo request_info    = new RequestInfo();
            request_info.endpoint       = "entitlements/upload";
            request_info.method         = Method.POST;
            request_info.bearer_token   = app_access_token;
            request_info.client_id      = client_id;

            IApiResponse<Data<Url>> url = await RestRequestUtil.ExecuteAsync<Data<Url>, EntitlementQueryParameters>(client_info, request_info, query_parameters, settings);

            return url;
        }

        #endregion

        #region /games

        /// <summary>
        /// Asynchronously information about a list of games.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<Data<Game>>>
        GetGamesAsync(string bearer_token, string client_id, GamesQueryParameters query_parameters, ApiRequestSettings settings)
        {
            if (settings.IsNull())
            {
                settings = new ApiRequestSettings();
            }

            if (settings.input_hanlding == InputHandling.Error)
            {
                ExceptionUtil.ThrowIfNullOrDefault(query_parameters, nameof(query_parameters));
                if(!query_parameters.ids.IsValid() && !query_parameters.names.IsValid())
                {
                    throw new ArgumentException("At least one valid game name or ID must be provided.");
                }
            }

            RequestInfo request_info    = new RequestInfo();
            request_info.endpoint       = "games";
            request_info.method         = Method.GET;
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IApiResponse<Data<Game>> games = await RestRequestUtil.ExecuteAsync<Data<Game>, GamesQueryParameters>(client_info, request_info, query_parameters, settings);

            return games;
        }

        #endregion

        #region /games/tpp

        /// <summary>
        /// Asynchronously gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<DataPage<Game>>>
        GetTopGamesPageAsync(string bearer_token, string client_id, TopGamesQueryParameters query_parameters, ApiRequestSettings settings)
        {
            RequestInfo request_info    = new RequestInfo();
            request_info.endpoint       = "games/top";
            request_info.method         = Method.GET;
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IApiResponse<DataPage<Game>> top_games = await RestRequestUtil.ExecuteAsync<DataPage<Game>>(client_info, request_info, query_parameters, settings);

            return top_games;
        }

        /// <summary>
        /// Asynchronously gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<DataPage<Game>>>
        GetTopGamesAsync(string bearer_token, string client_id, TopGamesQueryParameters query_parameters, ApiRequestSettings settings)
        {
            RequestInfo request_info    = new RequestInfo();
            request_info.endpoint       = "games/top";
            request_info.method         = Method.GET;
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IApiResponse<DataPage<Game>> top_games = await RestRequestUtil.ExecutePagesAsync<Game, DataPage<Game>>(client_info, request_info, query_parameters, settings);

            return top_games;
        }

        #endregion

        #region /streams

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<DataPage<Stream>>>
        GetStreamsPageAsync(string bearer_token, string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings settings)
        {
            RequestInfo request_info    = new RequestInfo();
            request_info.endpoint       = "streams";
            request_info.method         = Method.GET;
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IApiResponse<DataPage<Stream>> streams = await RestRequestUtil.ExecuteAsync<DataPage<Stream>>(client_info, request_info, query_parameters, settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<DataPage<Stream>>>
        GetStreamsAsync(string bearer_token, string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings settings)
        {
            RequestInfo request_info    = new RequestInfo();
            request_info.endpoint       = "streams";
            request_info.method         = Method.GET;
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IApiResponse<DataPage<Stream>> streams = await RestRequestUtil.ExecutePagesAsync<Stream, DataPage<Stream>>(client_info, request_info, query_parameters, settings);

            return streams;
        }

        /// <summary>
        /// Asynchronously checks to see if a user is streaming.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="user_id">The user to check if they are live.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<bool>>
        IsStreamLiveAsync(string bearer_token, string client_id, string user_id, ApiRequestSettings settings)
        {
            StreamsQueryParameters query_parameters = new StreamsQueryParameters()
            {
                user_ids = new List<string>()
                {
                    user_id
                }
            };

            IApiResponse<DataPage<Stream>> streams = await GetStreamsPageAsync(bearer_token, client_id, query_parameters, settings);

            ApiResponse<bool> is_live = new ApiResponse<bool>(streams);
            is_live.result = streams.result.data.IsValid();

            return is_live;
        }

        #endregion

        #region /streams/metadata

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<DataPage<Metadata>>>
        GetStreamsMetadataPageAsync(string bearer_token, string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings settings)
        {
            RequestInfo request_info    = new RequestInfo();
            request_info.endpoint       = "streams/metadata";
            request_info.method         = Method.GET;
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IApiResponse<DataPage<Metadata>> metadata = await RestRequestUtil.ExecuteAsync<DataPage<Metadata>>(client_info, request_info, query_parameters, settings);

            return metadata;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<DataPage<Metadata>>>
        GetStreamsMetadataAsync(string bearer_token, string client_id, StreamsQueryParameters query_parameters, ApiRequestSettings settings)
        {
            RequestInfo request_info    = new RequestInfo();
            request_info.endpoint       = "streams/metadata";
            request_info.method         = Method.GET;
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IApiResponse<DataPage<Metadata>> metadata = await RestRequestUtil.ExecutePagesAsync<Metadata, DataPage<Metadata>>(client_info, request_info, query_parameters, settings);

            return metadata;
        }

        #endregion

        #region /users

        /// <summary>
        /// <para>
        /// Asynchronously gets the information of one or more users by their id or login.
        /// If no <paramref name="query_parameters"/> are specified, the user is looked up by the token provided if it is an Bearer token.
        /// </para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>/// <param name="query_parameters">The users to look up either by id or by login.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<Data<User>>>
        GetUsersAsync(string bearer_token, string client_id, UsersQueryParameters query_parameters, ApiRequestSettings settings)
        {
            RequestInfo request_info    = new RequestInfo();
            request_info.endpoint       = "users";
            request_info.method         = Method.GET;
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IApiResponse<Data<User>> users = await RestRequestUtil.ExecuteAsync<Data<User>, UsersQueryParameters>(client_info, request_info, query_parameters, settings);

            return users;
        }

        /// <summary>
        /// <para>Asynchronously sets the description of a user specified by the Bearer token.</para>
        /// <para>Required Scope: 'user:edit'</para>
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="description">The new description to set.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<Data<User>>>
        SetUserDescriptionAsync(string bearer_token, string client_id, string description, ApiRequestSettings settings)
        {
            QueryParameter[] query_parameters = new QueryParameter[]
            {
                new QueryParameter
                {
                    name = "description",
                    value = description
                },
            };

            RequestInfo request_info    = new RequestInfo();
            request_info.endpoint       = "users";
            request_info.method         = Method.PUT;
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IApiResponse<Data<User>> result = await RestRequestUtil.ExecutetAsync<Data<User>>(client_info, request_info, query_parameters, settings);

            return result;
        }

        #endregion

        #region /users/follows

        /// <summary>
        /// Asynchronously gets the relationship between two users, or a single page of the following/follower lists of one user.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="query_parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="query_parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserRelationshipPageAsync(string bearer_token, string client_id, string from_id, string to_id, FollowsQueryParameters query_parameters, ApiRequestSettings settings)
        {
            if (settings.IsNull())
            {
                settings = new ApiRequestSettings();
            }

            if (settings.input_hanlding == InputHandling.Error)
            {
                if (!from_id.IsValid() && !to_id.IsValid())
                {
                    throw new ArgumentException("At least one " + nameof(from_id) + " or " + nameof(to_id) + " must be provided");
                }
            }

            if (query_parameters.IsNull())
            {
                query_parameters = new FollowsQueryParameters();
            }
            query_parameters.from_id = from_id;
            query_parameters.to_id = to_id;

            RequestInfo request_info    = new RequestInfo();
            request_info.endpoint       = "users/follows";
            request_info.method         = Method.GET;
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IApiResponse<FollowsDataPage<Follow>> follows = await RestRequestUtil.ExecuteAsync<FollowsDataPage<Follow>>(client_info, request_info, query_parameters, settings);

            return follows;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users, or the complete following/follower lists of one user.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="from_id">The user to compare from. Used to get the following list of a user.</param>
        /// <param name="to_id">The user to compare to. Used to get a user's follower list.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request. The 'to_id' and 'from_id' properties in the parameters are ignored if specified.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<FollowsDataPage<Follow>>>
        GetUserRelationshipAsync(string bearer_token, string client_id, string from_id, string to_id, FollowsQueryParameters query_parameters, ApiRequestSettings settings)
        {
            if (settings.IsNull())
            {
                settings = new ApiRequestSettings();
            }

            if (settings.input_hanlding == InputHandling.Error)
            {
                if(!from_id.IsValid() && !to_id.IsValid())
                {
                    throw new ArgumentException("At least one " + nameof(from_id) + " or " + nameof(to_id) + " must be provided");
                }
            }

            if (query_parameters.IsNull())
            {
                query_parameters = new FollowsQueryParameters();
            }            
            query_parameters.from_id = from_id;
            query_parameters.to_id = to_id;

            RequestInfo request_info    = new RequestInfo();
            request_info.endpoint       = "users/follows";
            request_info.method         = Method.GET;
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IApiResponse<FollowsDataPage<Follow>> follows = await RestRequestUtil.ExecutePagesAsync<Follow, FollowsDataPage<Follow>>(client_info, request_info, query_parameters, settings);

            return follows;
        }

        /// <summary>
        /// Asynchronously checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="bearer_token">The Bearer token to authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request and to authorize the request if no Bearer token was provided.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<bool>>
        IsUserFollowingAsync(string bearer_token, string client_id, string from_id, string to_id, ApiRequestSettings settings)
        {
            if (settings.IsNull())
            {
                settings = new ApiRequestSettings();
            }

            if(settings.input_hanlding == InputHandling.Error)
            {
                ExceptionUtil.ThrowIfInvalid(from_id, nameof(from_id));
                ExceptionUtil.ThrowIfInvalid(to_id, nameof(to_id));
            }

            IApiResponse<FollowsDataPage<Follow>> relationship = await GetUserRelationshipPageAsync(bearer_token, client_id, from_id, to_id, default(FollowsQueryParameters), settings);

            ApiResponse<bool> is_following = new ApiResponse<bool>(relationship);
            is_following.result = relationship.result.data.IsValid();

            return is_following;
        }

        #endregion        

        #region /videos

        /// <summary>
        /// Asynchronously gets information on one or more videos.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<DataPage<Video>>>
        GetVideosPageAsync(string bearer_token, string client_id, VideosQueryParameters query_parameters, ApiRequestSettings settings)
        {
            if (settings.IsNull())
            {
                settings = new ApiRequestSettings();
            }

            if (settings.input_hanlding == InputHandling.Error) 
            {
                ExceptionUtil.ThrowIfNull(query_parameters, nameof(query_parameters));
                if((query_parameters.ids.IsValid() && (query_parameters.user_id.IsValid() || query_parameters.game_id.IsValid())) ||
                   (query_parameters.user_id.IsValid() && query_parameters.game_id.IsValid()))
                {
                    throw new ArgumentException("Only one or more video ID's, one user ID, or one game ID can be provided.");
                }
            }

            RequestInfo request_info    = new RequestInfo();
            request_info.endpoint       = "videos";
            request_info.method         = Method.GET;
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IApiResponse<DataPage<Video>> videos = await RestRequestUtil.ExecuteAsync<DataPage<Video>>(client_info, request_info, query_parameters, settings);

            return videos;
        }

        /// <summary>
        /// Asynchronously gets a complete list of information on one or more videos.
        /// </summary>
        /// <param name="bearer_token">The Bearer token used to determine whose description to update and authorize the request.</param>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="query_parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IApiResponse{type}"/> interface.</returns>
        internal static async Task<IApiResponse<DataPage<Video>>>
        GetVideosAsync(string bearer_token, string client_id, VideosQueryParameters query_parameters, ApiRequestSettings settings)
        {
            if (settings.IsNull())
            {
                settings = new ApiRequestSettings();
            }

            if (settings.input_hanlding == InputHandling.Error)
            {
                ExceptionUtil.ThrowIfNull(query_parameters, nameof(query_parameters));
                if ((query_parameters.ids.IsValid() && (query_parameters.user_id.IsValid() || query_parameters.game_id.IsValid())) ||
                   (query_parameters.user_id.IsValid() && query_parameters.game_id.IsValid()))
                {
                    throw new ArgumentException("Only one or more video ID's, one user ID, or one game ID can be provided.");
                }
            }

            RequestInfo request_info    = new RequestInfo();
            request_info.endpoint       = "videos";
            request_info.method         = Method.GET;
            request_info.bearer_token   = bearer_token;
            request_info.client_id      = client_id;

            IApiResponse<DataPage<Video>> videos = await RestRequestUtil.ExecutePagesAsync<Video, DataPage<Video>>(client_info, request_info, query_parameters, settings);

            return videos;
        }

        #endregion
    }
}