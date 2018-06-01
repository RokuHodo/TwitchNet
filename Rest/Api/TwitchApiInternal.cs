// standard namespaces
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Rest.Api.Clips;
using TwitchNet.Rest.Api.Entitlements;
using TwitchNet.Rest.Api.Games;
using TwitchNet.Rest.Api.Streams;
using TwitchNet.Rest.Api.Users;
using TwitchNet.Rest.Api.Videos;
using TwitchNet.Utilities;

// imported .dll's
using RestSharp;

namespace
TwitchNet.Rest.Api
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
        /// <param name="helix_info">The information needed to make the rest request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        internal static async Task<IHelixResponse<Data<CreatedClip>>>
        CreateClipAsync(HelixInfo helix_info, ClipCreationQueryParameters parameters, RequestSettings settings = null)
        {
            if (settings.IsNull())
            {
                settings = RequestSettings.Default;
            }            

            if (settings.input_hanlding == InputHandling.Error)
            {
                ExceptionUtil.ThrowIfNull(parameters, nameof(parameters));
                ExceptionUtil.ThrowIfInvalid(parameters.broadcaster_id, nameof(parameters.broadcaster_id));
            }

            RestRequest request = RestUtil.CretaeHelixRequest("clips", Method.POST, helix_info, settings);
            request = request.AddPaging(parameters);

            Tuple<IRestResponse<Data<CreatedClip>>, RestException, RateLimit> tuple = await RestUtil.ExecuteAsync<Data<CreatedClip>>(client_info, request, settings);

            IHelixResponse<Data<CreatedClip>> respose = new HelixResponse<Data<CreatedClip>>(tuple.Item1, tuple.Item2, tuple.Item3);

            return respose;
        }

        /// <summary>
        /// Asynchronously gets information about a clip.
        /// </summary>
        /// <param name="helix_info">The information needed to make the rest request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        internal static async Task<IHelixResponse<Data<Clip>>>
        GetClipAsync(HelixInfo helix_info, ClipQueryParameters parameters, RequestSettings settings)
        {
            if (settings.IsNull())
            {
                settings = RequestSettings.Default;
            }

            if (settings.input_hanlding == InputHandling.Error)
            {
                ExceptionUtil.ThrowIfNull(parameters, nameof(parameters));
                ExceptionUtil.ThrowIfInvalid(parameters.id, nameof(parameters.id));
            }

            RestRequest request = RestUtil.CretaeHelixRequest("clips", Method.GET, helix_info, settings);
            request = request.AddPaging(parameters);

            Tuple<IRestResponse<Data<Clip>>, RestException, RateLimit> tuple = await RestUtil.ExecuteAsync<Data<Clip>>(client_info, request, settings);

            IHelixResponse<Data<Clip>> response = new HelixResponse<Data<Clip>>(tuple.Item1, tuple.Item2, tuple.Item3);

            return response;
        }

        #endregion

        #region /entitlements/upload

        /// <summary>
        /// Asynchronously creates a URL where you can upload a manifest file and notify users that they have an entitlement.
        /// </summary>
        /// <param name="helix_info">The information needed to make the rest request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        internal static async Task<IHelixResponse<Data<Url>>>
        CreateEntitlementGrantsUploadUrlAsync(HelixInfo helix_info, EntitlementQueryParameters parameters, RequestSettings settings)
        {
            if (settings.IsNull())
            {
                settings = RequestSettings.Default;
            }

            if (settings.input_hanlding == InputHandling.Error)
            {
                ExceptionUtil.ThrowIfNull(parameters, nameof(parameters));
                ExceptionUtil.ThrowIfInvalid(parameters.manifest_id, nameof(parameters.manifest_id));
                ExceptionUtil.ThrowIfOutOfRange(nameof(parameters.manifest_id), parameters.manifest_id.Length, 1, 64);
                ExceptionUtil.ThrowIfNull(parameters.type, nameof(parameters.type));
            }

            RestRequest request = RestUtil.CretaeHelixRequest("entitlements/upload", Method.POST, helix_info, settings);
            request = request.AddPaging(parameters);

            Tuple<IRestResponse<Data<Url>>, RestException, RateLimit> tuple = await RestUtil.ExecuteAsync<Data<Url>>(client_info, request, settings);

            IHelixResponse<Data<Url>> response = new HelixResponse<Data<Url>>(tuple.Item1, tuple.Item2, tuple.Item3);

            return response;
        }

        #endregion

        #region /games

        /// <summary>
        /// Asynchronously information about a list of games.
        /// </summary>
        /// <param name="helix_info">The information needed to make the rest request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        internal static async Task<IHelixResponse<Data<Game>>>
        GetGamesAsync(HelixInfo helix_info, GamesQueryParameters parameters, RequestSettings settings)
        {
            if (settings.IsNull())
            {
                settings = RequestSettings.Default;
            }

            if (settings.input_hanlding == InputHandling.Error)
            {
                ExceptionUtil.ThrowIfNull(parameters, nameof(parameters));
                if(!parameters.ids.IsValid() && !parameters.names.IsValid())
                {
                    throw new ArgumentException("At least one valid game name or ID must be provided.");
                }
            }

            RestRequest request = RestUtil.CretaeHelixRequest("games", Method.GET, helix_info, settings);
            request = request.AddPaging(parameters);

            Tuple<IRestResponse<Data<Game>>, RestException, RateLimit> tuple = await RestUtil.ExecuteAsync<Data<Game>>(client_info, request, settings);

            IHelixResponse<Data<Game>> response = new HelixResponse<Data<Game>>(tuple.Item1, tuple.Item2, tuple.Item3);

            return response;
        }

        #endregion

        #region /games/tpp

        /// <summary>
        /// Asynchronously gets a single page of top games, most popular first.
        /// </summary>
        /// <param name="helix_info">The information needed to make the rest request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        internal static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesPageAsync(HelixInfo helix_info, TopGamesQueryParameters parameters, RequestSettings settings)
        {
            RestRequest request = RestUtil.CretaeHelixRequest("games/top", Method.GET, helix_info, settings);
            request = request.AddPaging(parameters);

            Tuple<IRestResponse<DataPage<Game>>, RestException, RateLimit> tuple = await RestUtil.ExecuteAsync<DataPage<Game>>(client_info, request, settings);

            IHelixResponse<DataPage<Game>> response = new HelixResponse<DataPage<Game>>(tuple.Item1, tuple.Item2, tuple.Item3);

            return response;
        }

        /// <summary>
        /// Asynchronously gets a complete list of top games, most popular first.
        /// </summary>
        /// <param name="helix_info">The information needed to make the rest request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        internal static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesAsync(HelixInfo helix_info, TopGamesQueryParameters parameters, RequestSettings settings)
        {
            RestRequest request = RestUtil.CretaeHelixRequest("games/top", Method.GET, helix_info, settings);
            request = request.AddPaging(parameters);

            Tuple<IRestResponse<DataPage<Game>>, RestException, RateLimit> tuple = await RestUtil.TraceExecuteAsync<Game, DataPage<Game>>(client_info, request, parameters, settings);

            IHelixResponse<DataPage<Game>> response = new HelixResponse<DataPage<Game>>(tuple.Item1, tuple.Item2, tuple.Item3);

            return response;
        }

        #endregion

        #region /streams

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        /// <param name="helix_info">The information needed to make the rest request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        internal static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsPageAsync(HelixInfo helix_info, StreamsQueryParameters parameters, RequestSettings settings)
        {
            RestRequest request = RestUtil.CretaeHelixRequest("streams", Method.GET, helix_info, settings);
            request = request.AddPaging(parameters);

            Tuple<IRestResponse<DataPage<Stream>>, RestException, RateLimit> tuple = await RestUtil.ExecuteAsync<DataPage<Stream>>(client_info, request, settings);

            IHelixResponse<DataPage<Stream>> response = new HelixResponse<DataPage<Stream>>(tuple.Item1, tuple.Item2, tuple.Item3);

            return response;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        /// <param name="helix_info">The information needed to make the rest request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        internal static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsAsync(HelixInfo helix_info, StreamsQueryParameters parameters, RequestSettings settings)
        {
            RestRequest request = RestUtil.CretaeHelixRequest("streams", Method.GET, helix_info, settings);
            request = request.AddPaging(parameters);

            Tuple<IRestResponse<DataPage<Stream>>, RestException, RateLimit> tuple = await RestUtil.TraceExecuteAsync<Stream, DataPage<Stream>>(client_info, request, parameters, settings);

            IHelixResponse<DataPage<Stream>> response = new HelixResponse<DataPage<Stream>>(tuple.Item1, tuple.Item2, tuple.Item3);

            return response;
        }

        /// <summary>
        /// Asynchronously checks to see if a user is streaming.
        /// </summary>
        /// <param name="helix_info">The information needed to make the rest request.</param>
        /// <param name="user_id">The user to check if they are live.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        internal static async Task<IHelixResponse<bool>>
        IsStreamLiveAsync(HelixInfo helix_info, string user_id, RequestSettings settings)
        {
            if (settings.IsNull())
            {
                settings = RequestSettings.Default;
            }

            if(settings.input_hanlding == InputHandling.Error)
            {
                ExceptionUtil.ThrowIfInvalid(user_id, nameof(user_id));
            }

            StreamsQueryParameters parameters = new StreamsQueryParameters()
            {
                user_ids = new List<string>()
                {
                    user_id
                }
            };

            IHelixResponse<DataPage<Stream>> response = await GetStreamsPageAsync(helix_info, parameters, settings);
            IHelixResponse<bool> is_live = new HelixResponse<bool>(response, response.result.data.IsValid());

            return is_live;
        }

        #endregion

        #region /streams/metadata

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="helix_info">The information needed to make the rest request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        internal static async Task<IHelixResponse<DataPage<Metadata>>>
        GetStreamsMetadataPageAsync(HelixInfo helix_info, StreamsQueryParameters parameters, RequestSettings settings)
        {
            RestRequest request = RestUtil.CretaeHelixRequest("streams/metadata", Method.GET, helix_info, settings);
            request = request.AddPaging(parameters);

            Tuple<IRestResponse<DataPage<Metadata>>, RestException, RateLimit> tuple = await RestUtil.ExecuteAsync<DataPage<Metadata>>(client_info, request, settings);

            IHelixResponse<DataPage<Metadata>> response = new HelixResponse<DataPage<Metadata>>(tuple.Item1, tuple.Item2, tuple.Item3);

            return response;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        /// <param name="helix_info">The information needed to make the rest request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        internal static async Task<IHelixResponse<DataPage<Metadata>>>
        GetStreamsMetadataAsync(HelixInfo helix_info, StreamsQueryParameters parameters, RequestSettings settings)
        {
            RestRequest request = RestUtil.CretaeHelixRequest("streams/metadata", Method.GET, helix_info, settings);
            request = request.AddPaging(parameters);

            Tuple<IRestResponse<DataPage<Metadata>>, RestException, RateLimit> tuple = await RestUtil.TraceExecuteAsync<Metadata, DataPage<Metadata>>(client_info, request, parameters, settings);

            IHelixResponse<DataPage<Metadata>> response = new HelixResponse<DataPage<Metadata>>(tuple.Item1, tuple.Item2, tuple.Item3);

            return response;
        }

        #endregion

        #region /users

        /// <summary>
        /// <para>
        /// Asynchronously gets the information of one or more users by their id or login.
        /// If no <paramref name="parameters"/> are specified, the user is looked up by the token provided if it is an Bearer token.
        /// </para>
        /// <para>
        /// Optional Scope: 'user:read:email'.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="helix_info">The information needed to make the rest request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>/// <param name="parameters">The users to look up either by id or by login.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        internal static async Task<IHelixResponse<Data<User>>>
        GetUsersAsync(HelixInfo helix_info, UsersQueryParameters parameters, RequestSettings settings)
        {
            RestRequest request = RestUtil.CretaeHelixRequest("users", Method.GET, helix_info, settings);
            request = request.AddPaging(parameters);

            Tuple<IRestResponse<Data<User>>, RestException, RateLimit> tuple = await RestUtil.ExecuteAsync<Data<User>>(client_info, request, settings);

            IHelixResponse<Data<User>> response = new HelixResponse<Data<User>>(tuple.Item1, tuple.Item2, tuple.Item3);

            return response;
        }

        /// <summary>
        /// <para>Asynchronously sets the description of a user specified by the Bearer token.</para>
        /// <para>Required Scope: 'user:edit'</para>
        /// </summary>
        /// <param name="helix_info">The information needed to make the rest request.</param>
        /// <param name="description">The new description to set.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        internal static async Task<IHelixResponse<Data<User>>>
        SetUserDescriptionAsync(HelixInfo helix_info, string description, RequestSettings settings)
        {
            RestRequest request = RestUtil.CretaeHelixRequest("users", Method.PUT, helix_info, settings);
            string value = description.IsValid() ? description : string.Empty;
            request.AddQueryParameter("description", value);

            Tuple<IRestResponse<Data<User>>, RestException, RateLimit> tuple = await RestUtil.ExecuteAsync<Data<User>>(client_info, request, settings);

            IHelixResponse<Data<User>> response = new HelixResponse<Data<User>>(tuple.Item1, tuple.Item2, tuple.Item3);

            return response;
        }

        #endregion

        #region /users/follows

        /// <summary>
        /// Asynchronously gets the relationship between two users, or a single page of the following/follower lists of one user.
        /// </summary>
        /// <param name="helix_info">The information needed to make the rest request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="parameters">
        /// A set of query parameters to customize the request.
        /// The <code>from_id</code> and <code>to_id</code> properties in the <paramref name="parameters"/> are ignored if specified.
        /// </param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        internal static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserRelationshipPageAsync(HelixInfo helix_info, string from_id, string to_id, FollowsQueryParameters parameters, RequestSettings settings)
        {
            if (settings.IsNull())
            {
                settings = new RequestSettings();
            }

            if (settings.input_hanlding == InputHandling.Error)
            {
                if (!from_id.IsValid() && !to_id.IsValid())
                {
                    throw new ArgumentException("At least one " + nameof(from_id) + " or " + nameof(to_id) + " must be provided");
                }
            }

            if (parameters.IsNull())
            {
                parameters = new FollowsQueryParameters();
            }
            parameters.from_id = from_id;
            parameters.to_id = to_id;

            RestRequest request = RestUtil.CretaeHelixRequest("users/follows", Method.GET, helix_info, settings);
            request = request.AddPaging(parameters);

            Tuple<IRestResponse<FollowsDataPage<Follow>>, RestException, RateLimit> tuple = await RestUtil.ExecuteAsync<FollowsDataPage<Follow>>(client_info, request, settings);

            IHelixResponse<FollowsDataPage<Follow>> response = new HelixResponse<FollowsDataPage<Follow>>(tuple.Item1, tuple.Item2, tuple.Item3);

            return response;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users, or the complete following/follower lists of one user.
        /// </summary>
        /// <param name="helix_info">The information needed to make the rest request.</param>
        /// <param name="from_id">The user to compare from. Used to get the following list of a user.</param>
        /// <param name="to_id">The user to compare to. Used to get a user's follower list.</param>
        /// <param name="parameters">A set of query parameters to customize the request. The 'to_id' and 'from_id' properties in the parameters are ignored if specified.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        internal static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserRelationshipAsync(HelixInfo helix_info, string from_id, string to_id, FollowsQueryParameters parameters, RequestSettings settings)
        {
            if (settings.IsNull())
            {
                settings = RequestSettings.Default;
            }

            if (settings.input_hanlding == InputHandling.Error)
            {
                if(!from_id.IsValid() && !to_id.IsValid())
                {
                    throw new ArgumentException("At least one " + nameof(from_id) + " or " + nameof(to_id) + " must be provided");
                }
            }

            if (parameters.IsNull())
            {
                parameters = new FollowsQueryParameters();
            }            
            parameters.from_id = from_id;
            parameters.to_id = to_id;

            RestRequest request = RestUtil.CretaeHelixRequest("users/follows", Method.GET, helix_info, settings);
            request = request.AddPaging(parameters);

            Tuple<IRestResponse<FollowsDataPage<Follow>>, RestException, RateLimit> tuple = await RestUtil.TraceExecuteAsync<Follow, FollowsDataPage<Follow>>(client_info, request, parameters, settings);

            IHelixResponse<FollowsDataPage<Follow>> response = new HelixResponse<FollowsDataPage<Follow>>(tuple.Item1, tuple.Item2, tuple.Item3);

            return response;
        }

        /// <summary>
        /// Asynchronously checks to see if <paramref name="from_id"/> is following <paramref name="to_id"/>.
        /// </summary>
        /// <param name="helix_info">The information needed to make the rest request.</param>
        /// <param name="from_id">The user to compare from.</param>
        /// <param name="to_id">The user to compare to.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        internal static async Task<IHelixResponse<bool>>
        IsUserFollowingAsync(HelixInfo helix_info, string from_id, string to_id, RequestSettings settings)
        {
            if (settings.IsNull())
            {
                settings = RequestSettings.Default;
            }

            if(settings.input_hanlding == InputHandling.Error)
            {
                ExceptionUtil.ThrowIfInvalid(from_id, nameof(from_id));
                ExceptionUtil.ThrowIfInvalid(to_id, nameof(to_id));
            }

            IHelixResponse<FollowsDataPage<Follow>> response = await GetUserRelationshipPageAsync(helix_info, from_id, to_id, default(FollowsQueryParameters), settings);
            IHelixResponse<bool> is_following = new HelixResponse<bool>(response, response.result.data.IsValid());

            return is_following;
        }

        #endregion        

        #region /videos

        /// <summary>
        /// Asynchronously gets information on one or more videos.
        /// </summary>
        /// <param name="helix_info">The information needed to make the rest request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        internal static async Task<IHelixResponse<DataPage<Video>>>
        GetVideosPageAsync(HelixInfo helix_info, VideosQueryParameters parameters, RequestSettings settings)
        {
            if (settings.IsNull())
            {
                settings = RequestSettings.Default;
            }

            if (settings.input_hanlding == InputHandling.Error) 
            {
                ExceptionUtil.ThrowIfNull(parameters, nameof(parameters));
                if((parameters.ids.IsValid() && (parameters.user_id.IsValid() || parameters.game_id.IsValid())) ||
                   (parameters.user_id.IsValid() && parameters.game_id.IsValid()))
                {
                    throw new ArgumentException("Only one or more video ID's, one user ID, or one game ID can be provided.");
                }
            }

            RestRequest request = RestUtil.CretaeHelixRequest("videos", Method.GET, helix_info, settings);
            request = request.AddPaging(parameters);

            Tuple<IRestResponse<DataPage<Video>>, RestException, RateLimit> tuple = await RestUtil.ExecuteAsync<DataPage<Video>>(client_info, request, settings);

            IHelixResponse<DataPage<Video>> response = new HelixResponse<DataPage<Video>>(tuple.Item1, tuple.Item2, tuple.Item3);

            return response;
        }

        /// <summary>
        /// Asynchronously gets a complete list of information on one or more videos.
        /// </summary>
        /// <param name="helix_info">The information needed to make the rest request.</param>
        /// <param name="parameters">A set of query parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the API request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IHelixResponse{type}"/> interface.</returns>
        internal static async Task<IHelixResponse<DataPage<Video>>>
        GetVideosAsync(HelixInfo helix_info, VideosQueryParameters parameters, RequestSettings settings)
        {
            if (settings.IsNull())
            {
                settings = RequestSettings.Default;
            }

            if (settings.input_hanlding == InputHandling.Error)
            {
                ExceptionUtil.ThrowIfNull(parameters, nameof(parameters));
                if ((parameters.ids.IsValid() && (parameters.user_id.IsValid() || parameters.game_id.IsValid())) ||
                   (parameters.user_id.IsValid() && parameters.game_id.IsValid()))
                {
                    throw new ArgumentException("Only one or more video ID's, one user ID, or one game ID can be provided.");
                }
            }

            RestRequest request = RestUtil.CretaeHelixRequest("videos", Method.GET, helix_info, settings);
            request = request.AddPaging(parameters);

            Tuple<IRestResponse<DataPage<Video>>, RestException, RateLimit> tuple = await RestUtil.TraceExecuteAsync<Video, DataPage<Video>>(client_info, request, parameters, settings);

            IHelixResponse<DataPage<Video>> response = new HelixResponse<DataPage<Video>>(tuple.Item1, tuple.Item2, tuple.Item3);

            return response;
        }

        #endregion
    }
}