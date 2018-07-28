// standard namespaces
using System;
using System.Collections;
using System.Collections.Generic;
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
using TwitchNet.Utilities;

// imported .dll's
using RestSharp;

namespace
TwitchNet.Rest.Api
{
    internal static class
    TwitchApiInternal
    {
        /*
        #region /analytics/extensions

        /// <summary>
        /// <para>Asynchronously gets analytic urls for one or more devloper extensions.</para>
        /// <para>Required Scope: <see cref="Scopes.AnalyticsReadExtensions"/>.</para>
        /// </summary>
        public static async Task<IHelixResponse<Data<ExtensionAnalytics>>>
        GetExtensionAnalyticsAsync(RestInfo<Data<ExtensionAnalytics>> info, ExtensionAnalyticsParameters parameters)
        {
            IHelixResponse<Data<ExtensionAnalytics>> response = default;

            info.required_scopes = Scopes.AnalyticsReadExtensions;
            info = RestUtil.CreateHelixRequest("analytics/extensions", Method.GET, info);

            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<Data<ExtensionAnalytics>>(info);

                return response;
            }

            info.request = info.request.AddPaging(parameters);
            info = await RestUtil.ExecuteAsync(info);

            response = new HelixResponse<Data<ExtensionAnalytics>>(info);

            return response;
        }

        #endregion

        #region /analytics/games

        /// <summary>
        /// <para>Asynchronously gets a single page of analytic urls for one or more devloper games.</para>
        /// <para>Required Scope: <see cref="Scopes.AnalyticsReadGames"/>.</para>
        /// </summary>
        public static async Task<IHelixResponse<Data<GameAnalytics>>>
        GetGameAnalyticsPageAsync(RestInfo<Data<GameAnalytics>> info, GameAnalyticsParameters parameters)
        {
            IHelixResponse<Data<GameAnalytics>> response = default;

            if ((!parameters.started_at.IsNull() && parameters.ended_at.IsNull()) || (!parameters.ended_at.IsNull() && parameters.started_at.IsNull()))
            {
                info.SetInputError(new ArgumentException(nameof(parameters.started_at).WrapQuotes() + " and " + nameof(parameters.ended_at).WrapQuotes() + " must be specified together."));

                response = new HelixResponse<Data<GameAnalytics>>(info);

                return response;
            }

            info.required_scopes = Scopes.AnalyticsReadGames;
            info = RestUtil.CreateHelixRequest("analytics/games", Method.GET, info);

            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<Data<GameAnalytics>>(info);

                return response;
            }

            info.request = info.request.AddPaging(parameters);
            info = await RestUtil.ExecuteAsync(info);

            response = new HelixResponse<Data<GameAnalytics>>(info);

            return response;
        }

        /// <summary>
        /// <para>Asynchronously gets a complete list of analytic urls for one or more devloper games.</para>
        /// <para>Required Scope: <see cref="Scopes.AnalyticsReadGames"/>.</para>
        /// </summary>
        public static async Task<IHelixResponse<DataPage<GameAnalytics>>>
        GetGameAnalyticsAsync(RestInfo<DataPage<GameAnalytics>> info, GameAnalyticsParameters parameters)
        {
            IHelixResponse<DataPage<GameAnalytics>> response = default;

            if ((!parameters.started_at.IsNull() && parameters.ended_at.IsNull()) || (!parameters.ended_at.IsNull() && parameters.started_at.IsNull()))
            {
                info.SetInputError(new ArgumentException(nameof(parameters.started_at).WrapQuotes() + " and " + nameof(parameters.ended_at).WrapQuotes() + " must be specified together."));

                response = new HelixResponse<DataPage<GameAnalytics>>(info);

                return response;
            }            

            info.required_scopes = Scopes.AnalyticsReadGames;
            info = RestUtil.CreateHelixRequest("analytics/games", Method.GET, info);

            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<DataPage<GameAnalytics>>(info);

                return response;
            }

            // Make sure the user doesn't accidentally break shit.
            if (parameters.IsNull())
            {
                parameters = new GameAnalyticsParameters();
            }
            parameters.game_id = string.Empty;

            info.request = info.request.AddPaging(parameters);
            info = await RestUtil.TraceExecuteAsync<GameAnalytics, DataPage<GameAnalytics>>(info, parameters);

            response = new HelixResponse<DataPage<GameAnalytics>>(info);

            return response;
        }

        #endregion

        #region bits/leaderboard

        /// <summary>
        /// <para>Asynchronously gets a ranked list of bits leaderboard information for an authorized broadcaster.</para>
        /// <para>Required Scope: <see cref="Scopes.BitsRead"/>.</para>
        /// </summary>
        public static async Task<IHelixResponse<BitsLeaderboardData<BitsUser>>>
        GetBitsLeaderboardAsync(RestInfo<BitsLeaderboardData<BitsUser>> info, BitsLeaderboardParameters parameters)
        {
            IHelixResponse<BitsLeaderboardData<BitsUser>> response = default;

            info.required_scopes = Scopes.BitsRead;
            info = RestUtil.CreateHelixRequest("bits/leaderboard", Method.GET, info);

            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<BitsLeaderboardData<BitsUser>>(info);

                return response;
            }

            if (!parameters.IsNull() && parameters.period == BitsLeaderboardPeriod.All)
            {
                parameters.started_at = null;
            }

            info.request = info.request.AddPaging(parameters);
            info = await RestUtil.ExecuteAsync(info);

            response = new HelixResponse<BitsLeaderboardData<BitsUser>>(info);

            return response;
        }

        #endregion

        #region /clips

        /// <summary>
        /// <para>Asynchronously creates a clip.</para>
        /// <para>Required Scope: <see cref="Scopes.ClipsEdit"/>.</para>
        /// </summary>
        public static async Task<IHelixResponse<Data<CreatedClip>>>
        CreateClipAsync(RestInfo<Data<CreatedClip>> info, ClipCreationParameters parameters)
        {
            IHelixResponse<Data<CreatedClip>> response = default;

            if (parameters.IsNull())
            {
                info.SetInputError(new ArgumentNullException(nameof(parameters)));

                response = new HelixResponse<Data<CreatedClip>>(info);

                return response;
            }

            if (!parameters.broadcaster_id.IsValid())
            {
                info.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(parameters.broadcaster_id)));

                response = new HelixResponse<Data<CreatedClip>>(info);

                return response;
            }

            info.required_scopes = Scopes.ClipsEdit;
            info = RestUtil.CreateHelixRequest("clips", Method.POST, info);

            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<Data<CreatedClip>>(info);

                return response;
            }

            info.request = info.request.AddPaging(parameters);
            info = await RestUtil.ExecuteAsync(info);

            response = new HelixResponse<Data<CreatedClip>>(info);

            return response;
        }

        /// <summary>
        /// Asynchronously gets information about a clip.
        /// </summary>
        public static async Task<IHelixResponse<Data<Clip>>>
        GetClipAsync(RestInfo<Data<Clip>> info, ClipParameters parameters)
        {
            IHelixResponse<Data<Clip>> response = default;

            if (parameters.IsNull())
            {
                info.SetInputError(new ArgumentNullException(nameof(parameters)));

                response = new HelixResponse<Data<Clip>>(info);

                return response;
            }

            if (!parameters.id.IsValid())
            {
                info.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(parameters.id)));

                response = new HelixResponse<Data<Clip>>(info);

                return response;
            }

            info = RestUtil.CreateHelixRequest("clips", Method.GET, info);

            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<Data<Clip>>(info);

                return response;
            }

            info.request = info.request.AddPaging(parameters);
            info = await RestUtil.ExecuteAsync(info);

            response = new HelixResponse<Data<Clip>>(info);

            return response;
        }

        #endregion

        #region /entitlements/upload

        /// <summary>
        /// <para>Asynchronously creates a URL where you can upload a manifest file and notify users that they have an entitlement.</para>
        /// <para>Required Authorization: App Access Token.</para>
        /// </summary>
        public static async Task<IHelixResponse<Data<Url>>>
        CreateEntitlementGrantsUploadUrlAsync(RestInfo<Data<Url>> info, EntitlementParameters parameters)
        {
            IHelixResponse<Data<Url>> response = default;

            if (parameters.IsNull())
            {
                info.SetInputError(new ArgumentNullException(nameof(parameters)));

                response = new HelixResponse<Data<Url>>(info);

                return response;
            }

            // Check for this separately here in case the user calls the overloaded function that passes the app access token and the client ID
            // and only the client ID is only valid, for some reason.
            if (!info.bearer_token.IsValid())
            {
                info.SetInputError(new ArgumentException("An app access token must be specified.", nameof(info.bearer_token)));

                response = new HelixResponse<Data<Url>>(info);

                return response;
            }

            if (!parameters.manifest_id.IsValid())
            {
                info.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(parameters.manifest_id)));

                response = new HelixResponse<Data<Url>>(info);

                return response;
            }

            if (!parameters.manifest_id.Length.IsInRange(1, 64))
            {
                info.SetInputError(new ArgumentException("The string must be between 1 and 64 characters long.", nameof(parameters.manifest_id)));

                response = new HelixResponse<Data<Url>>(info);

                return response;
            }

            info = RestUtil.CreateHelixRequest("entitlements/upload", Method.POST, info);

            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<Data<Url>>(info);

                return response;
            }

            info.request = info.request.AddPaging(parameters);
            info = await RestUtil.ExecuteAsync(info);

            response = new HelixResponse<Data<Url>>(info);

            return response;
        }

        #endregion

        #region /games

        /// <summary>
        /// Asynchronously information about a list of games.
        /// </summary>
        public static async Task<IHelixResponse<Data<Game>>>
        GetGamesAsync(RestInfo<Data<Game>> info, GamesParameters parameters)
        {
            IHelixResponse<Data<Game>> response = default;

            if (parameters.IsNull())
            {
                info.SetInputError(new ArgumentNullException(nameof(parameters)));

                response = new HelixResponse<Data<Game>>(info);

                return response;
            }

            if (!parameters.ids.IsValid() && !parameters.names.IsValid())
            {
                info.SetInputError(new ArgumentException("At least one game name or game ID must be provided."));

                response = new HelixResponse<Data<Game>>(info);

                return response;
            }

            info = RestUtil.CreateHelixRequest("games", Method.GET, info);

            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<Data<Game>>(info);

                return response;
            }

            info.request = info.request.AddPaging(parameters);
            info = await RestUtil.ExecuteAsync(info);

            response = new HelixResponse<Data<Game>>(info);

            return response;
        }

        #endregion

        #region /games/tpp

        /// <summary>
        /// Asynchronously gets a single page of top games, most popular first.
        /// </summary>
        public static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesPageAsync(RestInfo<DataPage<Game>> info, TopGamesParameters parameters)
        {
            IHelixResponse<DataPage<Game>> response = default;

            info = RestUtil.CreateHelixRequest("games/top", Method.GET, info);

            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<DataPage<Game>>(info);

                return response;
            }

            info.request = info.request.AddPaging(parameters);
            info = await RestUtil.ExecuteAsync(info);

            response = new HelixResponse<DataPage<Game>>(info);

            return response;
        }

        /// <summary>
        /// Asynchronously gets a complete list of top games, most popular first.
        /// </summary>
        public static async Task<IHelixResponse<DataPage<Game>>>
        GetTopGamesAsync(RestInfo<DataPage<Game>> info, TopGamesParameters parameters)
        {
            IHelixResponse<DataPage<Game>> response = default;

            info = RestUtil.CreateHelixRequest("games/top", Method.GET, info);

            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<DataPage<Game>>(info);

                return response;
            }

            info.request = info.request.AddPaging(parameters);
            info = await RestUtil.TraceExecuteAsync<Game, DataPage<Game>>(info, parameters);

            response = new HelixResponse<DataPage<Game>>(info);

            return response;
        }

        #endregion

        #region /streams

        /// <summary>
        /// Asynchronously gets a single page of streams.
        /// </summary>
        public static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsPageAsync(RestInfo<DataPage<Stream>> info, StreamsParameters parameters)
        {
            IHelixResponse<DataPage<Stream>> response = default;

            info = RestUtil.CreateHelixRequest("streams", Method.GET, info);

            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<DataPage<Stream>>(info);

                return response;
            }

            info.request = info.request.AddPaging(parameters);
            info = await RestUtil.ExecuteAsync(info);

            response = new HelixResponse<DataPage<Stream>>(info);

            return response;
        }

        /// <summary>
        /// Asynchronously gets a complete list of streams.
        /// </summary>
        public static async Task<IHelixResponse<DataPage<Stream>>>
        GetStreamsAsync(RestInfo<DataPage<Stream>> info, StreamsParameters parameters)
        {
            IHelixResponse<DataPage<Stream>> response = default;

            info = RestUtil.CreateHelixRequest("streams", Method.GET, info);

            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<DataPage<Stream>>(info);

                return response;
            }

            info.request = info.request.AddPaging(parameters);
            info = await RestUtil.TraceExecuteAsync<Stream, DataPage<Stream>>(info, parameters);

            response = new HelixResponse<DataPage<Stream>>(info);

            return response;
        }

        /// <summary>
        /// Asynchronously checks to see if a user is streaming.
        /// </summary>
        public static async Task<IHelixResponse<bool>>
        IsStreamLiveAsync(RestInfo<DataPage<Stream>> info, string user_id)
        {
            IHelixResponse<DataPage<Stream>> response = default;
            IHelixResponse<bool> is_live = default;

            if (!user_id.IsValid())
            {
                info.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(user_id)));

                response = new HelixResponse<DataPage<Stream>>(info);
                is_live = new HelixResponse<bool>(info, false);

                return is_live;
            }

            StreamsParameters parameters = new StreamsParameters()
            {
                user_ids = new List<string>()
                {
                    user_id
                }
            };

            // Info *should* get populated with valid results after the stream is requested.
            // TODO: Make sure this is the case.
            response = await GetStreamsPageAsync(info, parameters);
            is_live = new HelixResponse<bool>(info, response.result.data.IsValid());

            return is_live;
        }

        #endregion

        #region /streams/metadata

        /// <summary>
        /// Asynchronously gets a single page of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        public static async Task<IHelixResponse<DataPage<Metadata>>>
        GetStreamsMetadataPageAsync(RestInfo<DataPage<Metadata>> info, StreamsParameters parameters)
        {
            IHelixResponse<DataPage<Metadata>> response = default;

            info = RestUtil.CreateHelixRequest("streams/metadata", Method.GET, info);

            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<DataPage<Metadata>>(info);

                return response;
            }

            info.request = info.request.AddPaging(parameters);
            info = await RestUtil.ExecuteAsync(info);

            response = new HelixResponse<DataPage<Metadata>>(info);

            return response;
        }

        /// <summary>
        /// Asynchronously gets a complete list of metadata about streams playing either Overwatch or Hearthstone.
        /// </summary>
        public static async Task<IHelixResponse<DataPage<Metadata>>>
        GetStreamsMetadataAsync(RestInfo<DataPage<Metadata>> info, StreamsParameters parameters)
        {
            IHelixResponse<DataPage<Metadata>> response = default;

            info = RestUtil.CreateHelixRequest("streams/metadata", Method.GET, info);

            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<DataPage<Metadata>>(info);

                return response;
            }

            info.request = info.request.AddPaging(parameters);
            info = await RestUtil.TraceExecuteAsync<Metadata, DataPage<Metadata>>(info, parameters);

            response = new HelixResponse<DataPage<Metadata>>(info);

            return response;
        }

        #endregion
            */
        #region /users

        /// <summary>
        /// <para>Asynchronously gets the information about one or more users.</para>
        /// <para>
        /// Optional scope: <see cref="Scopes.UserReadEmail"/>.
        /// If provided, the user's email is included in the response.
        /// </para>
        /// </summary>
        /// <param name="info">The information used to assemble and execute the request, and while handling the response and any errors that may have occurred.</param>
        /// <param name="parameters">
        /// A set of rest parameters to add to the request.
        /// If not specified, the user is looked up by the specified bearer token.
        /// </param>
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
        /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
        /// <exception cref="RestException">Thrown if an error was returned by Twitch after executing the request.</exception>
        /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
        public static async Task<IHelixResponse<Data<User>>>
        GetUsersAsync(RestInfo<Data<User>> info, UsersParameters parameters)
        {
            IHelixResponse<Data<User>> response = default;

            // Build the request up here because it simplifies error checking down below.
            info = RestUtil.CreateHelixRequest("users", Method.GET, info);
            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<Data<User>>(info.exception_source, info.exceptions);

                return response;
            }

            int total_query_parameters = 0;
            if (!parameters.IsNull())
            {
                parameters.ids.Sanitize();
                parameters.logins.Sanitize();

                total_query_parameters = parameters.ids.Count + parameters.logins.Count;

                if (total_query_parameters > 100)
                {
                    info.SetInputError(new ArgumentException("A maximum of 100 total user logins and/or user IDs can be specified at one time.", nameof(parameters)));

                    response = new HelixResponse<Data<User>>(info.exception_source, info.exceptions);

                    return response;
                }
            }

            if(!info.bearer_token.IsValid() && total_query_parameters == 0)
            {
                // We know a client ID has been specified if we got this far.
                // But these conditions must still be true to have a valid request.
                if (parameters.IsNull())
                {
                    info.SetInputError(new ArgumentNullException(nameof(parameters)));
                }
                else
                {
                    info.SetInputError(new ArgumentException("A bearer token must be specified if parameters is null, or all specified ids and logins are null, empty, or only contain whitespace.", nameof(info.bearer_token)));
                }

                response = new HelixResponse<Data<User>>(info.exception_source, info.exceptions);

                return response;
            }

            info.request = info.request.AddPaging(parameters);
            info = await RestUtil.ExecuteAsync(info);

            response = new HelixResponse<Data<User>>(info.response, info.rate_limit, info.exception_source, info.exceptions);

            return response;
        }

        /// <summary>
        /// <para>Asynchronously sets the description of a user from the specified bearer token.</para>
        /// <para>Required scope: <see cref="Scopes.UserEdit"/>.</para>
        /// </summary>
        /// <param name="info">The information used to assemble and execute the request, and while handling the response and any errors that may have occurred.</param>
        /// <param name="description">The text to set the user's description to..</param>
        /// <returns>
        /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
        /// <see cref="IHelixResponse{result_type}.result"/> contains information about the user with the updated description.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if both bearer token and client ID, or the description are null, empty, or contains only whitespace.
        /// Thrown if the description is null, empty, or contains only whitespace.
        /// </exception>
        /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
        /// <exception cref="MissingScopesException">Thrown if the bearer token does not include the <see cref="Scopes.UserEdit"/> scope.</exception>
        /// <exception cref="RestException">Thrown if an error was returned by Twitch after executing the request.</exception>
        /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
        public static async Task<IHelixResponse<Data<User>>>
        SetUserDescriptionAsync(RestInfo<Data<User>> info, string description)
        {
            DescriptionParameters parameters = new DescriptionParameters();
            parameters.description = description;

            IHelixResponse<Data<User>> response = await SetUserDescriptionAsync(info, parameters);

            return response;
        }

        /// <summary>
        /// <para>Asynchronously sets the description of a user from the specified bearer token.</para>
        /// <para>Required scope: <see cref="Scopes.UserEdit"/>.</para>
        /// </summary>
        /// <param name="info">The information used to assemble and execute the request, and while handling the response and any errors that may have occurred.</param>
        /// <param name="parameters">A set of rest parameters to add to the request.</param>
        /// <returns>
        /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
        /// <see cref="IHelixResponse{result_type}.result"/> contains information about the user with the updated description.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the bearer token or description is null, empty, or contains only whitespace.</exception>
        /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
        /// <exception cref="MissingScopesException">Thrown if the available scopes, if specified, does not include the <see cref="Scopes.UserEdit"/> scope.</exception>
        /// <exception cref="RestException">Thrown if an error was returned by Twitch after executing the request.</exception>
        /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
        public static async Task<IHelixResponse<Data<User>>>
        SetUserDescriptionAsync(RestInfo<Data<User>> info, DescriptionParameters parameters)
        {
            IHelixResponse<Data<User>> response = default;

            if (!info.bearer_token.IsValid())
            {
                info.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(info.bearer_token)));

                response = new HelixResponse<Data<User>>(info.exception_source, info.exceptions);

                return response;
            }

            if (parameters.IsNull())
            {
                info.SetInputError(new ArgumentNullException(nameof(parameters)));

                response = new HelixResponse<Data<User>>(info.exception_source, info.exceptions);

                return response;
            }

            if (!parameters.description.IsValid())
            {
                info.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(parameters.description)));

                response = new HelixResponse<Data<User>>(info.exception_source, info.exceptions);

                return response;
            }

            // TODO: Change MissingScopesException to handle 403 - Forbidden as well, e.g., "Missing user:edit scope"?
            info.required_scopes = Scopes.UserEdit;
            info = RestUtil.CreateHelixRequest("users", Method.PUT, info);
            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<Data<User>>(info.exception_source, info.exceptions);

                return response;
            }

            info.request = info.request.AddPaging(parameters);
            info = await RestUtil.ExecuteAsync(info);

            response = new HelixResponse<Data<User>>(info.response, info.rate_limit, info.exception_source, info.exceptions);

            return response;
        }

        #endregion
        /*
        #region /users/extensions

        /// <summary>
        /// <para>
        /// Asynchronously gets a list of active extensions installed by a user.
        /// The user is identified either by user ID or by the provided Bearer token.
        /// </para>
        /// <para>Optional Scope: <see cref="Scopes.UserReadBroadcast"/> or <see cref="Scopes.UserEditBroadcast"/>.</para>
        /// </summary>
        public static async Task<IHelixResponse<ActiveExtensionsData>>
        GetUserActiveExtensionsAsync(RestInfo<ActiveExtensionsData> info, ActiveExtensionsParameters parameters)
        {
            IHelixResponse<ActiveExtensionsData> response = default;

            // The parameters are optional only when a bearer token is provided.
            if (!info.bearer_token.IsValid())
            {
                if (parameters.IsNull())
                {
                    info.SetInputError(new ArgumentNullException(nameof(parameters), "Parameters must be specified if no bearer token is specified."));

                    response = new HelixResponse<ActiveExtensionsData>(info);

                    return response;
                }

                if (!parameters.user_id.IsValid())
                {
                    info.SetInputError(new ArgumentException("A valid user ID must be specified if no bearer token is specified. The value cannot be null, empty, or contain only whitespace.", nameof(parameters.user_id)));

                    response = new HelixResponse<ActiveExtensionsData>(info);

                    return response;
                }
            }

            info = RestUtil.CreateHelixRequest("users/extensions", Method.GET, info);

            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<ActiveExtensionsData>(info);

                return response;
            }

            info.request = info.request.AddPaging(parameters);
            info = await RestUtil.ExecuteAsync(info);

            response = new HelixResponse<ActiveExtensionsData>(info);

            return response;
        }

        /// <summary>
        /// <para>
        /// Asynchronously updates the active extensions for a user identified by a user ID or by the provided Bearer token.
        /// The activation state, extension ID, verison number, or x/y coordinates (components only) can be updated.
        /// </para>
        /// <para>Required Scope: <see cref="Scopes.UserEditBroadcast"/>.</para>
        /// </summary>
        public static async Task<IHelixResponse<ActiveExtensionsData>>
        UpdateUserExtensionsAsync(RestInfo<ActiveExtensionsData> info, UpdateExtensionsParameters parameters)
        {
            IHelixResponse<ActiveExtensionsData> response = default;

            if (parameters.IsNull())
            {
                info.SetInputError(new ArgumentNullException(nameof(parameters)));

                response = new HelixResponse<ActiveExtensionsData>(info);

                return response;
            }

            if (parameters.data.IsNull())
            {
                info.SetInputError(new ArgumentNullException(nameof(parameters.data)));

                response = new HelixResponse<ActiveExtensionsData>(info);

                return response;
            }

            info.required_scopes = Scopes.UserEditBroadcast;
            info = RestUtil.CreateHelixRequest("users/extensions", Method.PUT, info);

            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<ActiveExtensionsData>(info);

                return response;
            }

            info.request = info.request.AddPaging(parameters);
            info = await RestUtil.ExecuteAsync(info);

            response = new HelixResponse<ActiveExtensionsData>(info);

            return response;
        }

        #endregion

        #region /users/extensions/list

        /// <summary>
        /// <para>Asynchronously gets a list of all extensions a user has installed, active or inactive.</para>
        /// <para>Required Scope: <see cref="Scopes.UserReadBroadcast"/></para>
        /// </summary>
        public static async Task<IHelixResponse<Data<Extension>>>
        GetUserExtensionsAsync(RestInfo<Data<Extension>> info)
        {
            IHelixResponse<Data<Extension>> response = default;

            info.required_scopes = Scopes.UserReadBroadcast;
            info = RestUtil.CreateHelixRequest("users/extensions/list", Method.GET, info);

            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<Data<Extension>>(info);

                return response;
            }

            info = await RestUtil.ExecuteAsync(info);

            response = new HelixResponse<Data<Extension>>(info);

            return response;
        }

        #endregion
        */
        #region /users/follows        

        /// <summary>
        /// Asynchronously gets a single page of user's following list.
        /// </summary>
        /// <param name="info">The information used to assemble and execute the request, and while handling the response and any errors that may have occurred.</param>
        /// <param name="from_id">The user ID to get the following list for.</param>
        /// <param name="parameters">
        /// A set of rest parameters to add to the request.
        /// If specified, from_id and to_id are ignored.
        /// </param>
        /// <returns>
        /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
        /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of user's following list.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if both bearer token and client ID are null, empty, or contains only whitespace.
        /// Thrown if <paramref name="from_id"/> is null, empty, or contains only whitespace.
        /// </exception>
        /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
        /// <exception cref="RestException">Thrown if an error was returned by Twitch after executing the request.</exception>
        /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingPageAsync(RestInfo<FollowsDataPage<Follow>> info, string from_id, FollowsParameters parameters)
        {
            IHelixResponse<FollowsDataPage<Follow>> response = default;

            if (!from_id.IsValid())
            {
                info.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(from_id)));

                response = new HelixResponse<FollowsDataPage<Follow>>(info.exception_source, info.exceptions);

                return response;
            }

            if (parameters.IsNull())
            {
                parameters = new FollowsParameters(from_id, string.Empty);
            }
            else
            {
                parameters.from_id = from_id;
                parameters.to_id = string.Empty;
            }

            response = await GetUserRelationshipPageAsync(info, parameters);

            return response;
        }

        /// <summary>
        /// Asynchronously gets a user's complete following list.
        /// </summary>
        /// <param name="info">The information used to assemble and execute the request, and while handling the response and any errors that may have occurred.</param>
        /// <param name="from_id">The user ID to get the following list for.</param>
        /// <param name="parameters">
        /// A set of rest parameters to add to the request.
        /// If specified, from_id and to_id are ignored.
        /// </param>
        /// <returns>
        /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
        /// <see cref="IHelixResponse{result_type}.result"/> contains the user's complete following list.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if both bearer token and client ID are null, empty, or contains only whitespace.
        /// Thrown if <paramref name="from_id"/> is null, empty, or contains only whitespace.
        /// </exception>
        /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
        /// <exception cref="RestException">Thrown if an error was returned by Twitch after executing the request.</exception>
        /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowingAsync(RestInfo<FollowsDataPage<Follow>> info, string from_id, FollowsParameters parameters)
        {
            IHelixResponse<FollowsDataPage<Follow>> response = default;

            if (!from_id.IsValid())
            {
                info.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(from_id)));

                response = new HelixResponse<FollowsDataPage<Follow>>(info.exception_source, info.exceptions);

                return response;
            }

            if (parameters.IsNull())
            {
                parameters = new FollowsParameters(from_id, string.Empty);
            }
            else
            {
                parameters.from_id = from_id;
                parameters.to_id = string.Empty;
            }

            response = await GetUserRelationshipAsync(info, parameters);

            return response;
        }

        /// <summary>
        /// Asynchronously gets a single page of a user's followers list.
        /// </summary>
        /// <param name="info">The information used to assemble and execute the request, and while handling the response and any errors that may have occurred.</param>
        /// <param name="to_id">The user ID to get the follower list for.</param>
        /// <param name="parameters">
        /// A set of rest parameters to add to the request.
        /// If specified, from_id and to_id are ignored.
        /// </param>
        /// <returns>
        /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
        /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of a user's followers list.
        /// </returns>        
        /// <exception cref="ArgumentException">
        /// Thrown if both bearer token and client ID are null, empty, or contains only whitespace.
        /// Thrown if <paramref name="to_id"/> is null, empty, or contains only whitespace.
        /// </exception>
        /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
        /// <exception cref="RestException">Thrown if an error was returned by Twitch after executing the request.</exception>
        /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersPageAsync(RestInfo<FollowsDataPage<Follow>> info, string to_id, FollowsParameters parameters)
        {
            IHelixResponse<FollowsDataPage<Follow>> response = default;

            if (!to_id.IsValid())
            {
                info.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(to_id)));

                response = new HelixResponse<FollowsDataPage<Follow>>(info.exception_source, info.exceptions);

                return response;
            }

            if (parameters.IsNull())
            {
                parameters = new FollowsParameters(string.Empty, to_id);
            }
            else
            {
                parameters.from_id = string.Empty;
                parameters.to_id = to_id;
            }

            response = await GetUserRelationshipPageAsync(info, parameters);

            return response;
        }

        /// <summary>
        /// Asynchronously gets a user's complete follower list.
        /// </summary>
        /// <param name="info">The information used to assemble and execute the request, and while handling the response and any errors that may have occurred.</param>
        /// <param name="to_id">The user ID to get the follower list for.</param>
        /// <param name="parameters">
        /// A set of rest parameters to add to the request.
        /// If specified, from_id and to_id are ignored.
        /// </param>
        /// <returns>
        /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
        /// <see cref="IHelixResponse{result_type}.result"/> contains the user's complete follower list.
        /// </returns>        
        /// <exception cref="ArgumentException">
        /// Thrown if both bearer token and client ID are null, empty, or contains only whitespace.
        /// Thrown if <paramref name="to_id"/> is null, empty, or contains only whitespace.
        /// </exception>
        /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
        /// <exception cref="RestException">Thrown if an error was returned by Twitch after executing the request.</exception>
        /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserFollowersAsync(RestInfo<FollowsDataPage<Follow>> info, string to_id, FollowsParameters parameters)
        {
            IHelixResponse<FollowsDataPage<Follow>> response = default;

            if (!to_id.IsValid())
            {
                info.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(to_id)));

                response = new HelixResponse<FollowsDataPage<Follow>>(info.exception_source, info.exceptions);

                return response;
            }

            if (parameters.IsNull())
            {
                parameters = new FollowsParameters(string.Empty, to_id);
            }
            else
            {
                parameters.from_id = string.Empty;
                parameters.to_id = to_id;
            }

            response = await GetUserRelationshipAsync(info, parameters);

            return response;
        }

        /// <summary>
        /// Asynchronously checks to see if a user is following another user.
        /// </summary>
        /// <param name="info">The information used to assemble and execute the request, and while handling the response and any errors that may have occurred.</param>
        /// <param name="from_id">The user ID to check if they are following another user.</param>
        /// <param name="to_id">The user ID to check if another user is following them.</param>
        /// <returns>
        /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
        /// <see cref="IHelixResponse{result_type}.result"/> is set true if from_id is following to_id, otherwise false.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if both bearer token and client ID are null, empty, or contains only whitespace.
        /// Thrown if either from_id and to_id are null, empty, or contains only whitespace.
        /// </exception>
        /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
        /// <exception cref="RestException">Thrown if an error was returned by Twitch after executing the request.</exception>
        /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
        public static async Task<IHelixResponse<bool>>
        IsUserFollowingAsync(RestInfo<FollowsDataPage<Follow>> info, string from_id, string to_id)
        {
            IHelixResponse<FollowsDataPage<Follow>> _response = default;
            IHelixResponse<bool> response = default;

            if (!to_id.IsValid() || !from_id.IsValid())
            {
                info.SetInputError(new ArgumentException("Both from_id and to_id must be specified and cannot be null, empty, or contain only whitespace."));

                _response = new HelixResponse<FollowsDataPage<Follow>>(info.exception_source, info.exceptions);
                response = new HelixResponse<bool>(_response, false);

                return response;
            }

            FollowsParameters parameters = new FollowsParameters();
            parameters.from_id = from_id;
            parameters.to_id = to_id;

            _response = await GetUserRelationshipPageAsync(info, parameters);
            response = new HelixResponse<bool>(_response, _response.result.data.IsValid());

            return response;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users, or a single page of a user's following/follower list.
        /// </summary>
        /// <param name="info">The information used to assemble and execute the request, and while handling the response and any errors that may have occurred.</param>
        /// <param name="parameters">
        /// <para>A set of rest parameters to add to the request.</para>
        /// <para>A from_id or to_id must be specified.</para>
        /// </param>
        /// <returns>
        /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
        /// <see cref="IHelixResponse{result_type}.result"/> contains the user relationship page, or a single page of the following/follower list of one user.
        /// </returns> 
        /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown if both bearer token and client ID are null, empty, or contains only whitespace.
        /// Thrown if both from_id and to_id are null, empty, or contains only whitespace.
        /// </exception>
        /// <exception cref="Exception">Thrown if an error occurred in an external assembly while assembling or executing a request, or while deserializing a response.</exception>
        /// <exception cref="RestException">Thrown if an error was returned by Twitch after executing the request.</exception>
        /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserRelationshipPageAsync(RestInfo<FollowsDataPage<Follow>> info, FollowsParameters parameters)
        {
            IHelixResponse<FollowsDataPage<Follow>> response = default;

            if (parameters.IsNull())
            {
                info.SetInputError(new ArgumentNullException(nameof(parameters)));

                response = new HelixResponse<FollowsDataPage<Follow>>(info.exception_source, info.exceptions);

                return response;
            }

            if (!parameters.to_id.IsValid() && !parameters.from_id.IsValid())
            {
                info.SetInputError(new ArgumentException("At least either from_id or to_id must be specified and cannot be null, empty, or contain only whitespace."));

                response = new HelixResponse<FollowsDataPage<Follow>>(info.exception_source, info.exceptions);

                return response;
            }

            info = RestUtil.CreateHelixRequest("users/follows", Method.GET, info);
            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<FollowsDataPage<Follow>>(info.exception_source, info.exceptions);

                return response;
            }

            info.request = info.request.AddPaging(parameters);
            info = await RestUtil.ExecuteAsync(info);

            response = new HelixResponse<FollowsDataPage<Follow>>(info.response, info.rate_limit, info.exception_source, info.exceptions);

            return response;
        }

        /// <summary>
        /// Asynchronously gets the relationship between two users, or a user's complete following/follower list.
        /// </summary>
        /// <param name="info">The information used to assemble and execute the request, and while handling the response and any errors that may have occurred.</param>
        /// <param name="parameters">
        /// <para>A set of rest parameters to add to the request.</para>
        /// <para>A from_id or to_id must be specified.</para>
        /// </param>
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
        /// <exception cref="RestException">Thrown if an error was returned by Twitch after executing the request.</exception>
        /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
        public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
        GetUserRelationshipAsync(RestInfo<FollowsDataPage<Follow>> info, FollowsParameters parameters)
        {
            IHelixResponse<FollowsDataPage<Follow>> response = default;

            if (parameters.IsNull())
            {
                info.SetInputError(new ArgumentNullException(nameof(parameters)));

                response = new HelixResponse<FollowsDataPage<Follow>>(info.exception_source, info.exceptions);

                return response;
            }

            if (!parameters.to_id.IsValid() && !parameters.from_id.IsValid())
            {
                info.SetInputError(new ArgumentException("At least either from_id or to_id must be specified and cannot be null, empty, or contain only whitespace."));

                response = new HelixResponse<FollowsDataPage<Follow>>(info.exception_source, info.exceptions);

                return response;
            }

            info = RestUtil.CreateHelixRequest("users/follows", Method.GET, info);
            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<FollowsDataPage<Follow>>(info.exception_source, info.exceptions);

                return response;
            }

            info.request = info.request.AddPaging(parameters);
            info = await RestUtil.TraceExecuteAsync<Follow, FollowsDataPage<Follow>>(info, parameters);

            response = new HelixResponse<FollowsDataPage<Follow>>(info.response, info.rate_limit, info.exception_source, info.exceptions);

            return response;
        }

        #endregion
        /*
        #region /videos

        /// <summary>
        /// Asynchronously gets information on one or more videos.
        /// </summary>
        public static async Task<IHelixResponse<DataPage<Video>>>
        GetVideosPageAsync(RestInfo<DataPage<Video>> info, VideosParameters parameters, RequestSettings settings)
        {
            IHelixResponse<DataPage<Video>> response = default;

            if (parameters.IsNull())
            {
                info.SetInputError(new ArgumentNullException(nameof(parameters)));

                response = new HelixResponse<DataPage<Video>>(info);

                return response;
            }

            if((parameters.ids.IsValid() && (parameters.user_id.IsValid() ||parameters.game_id.IsValid())) ||
               (parameters.user_id.IsValid() && parameters.game_id.IsValid()))
            {
                info.SetInputError(new ArgumentException("Only one or more video ID's, one user ID, or one game ID can be provided."));

                response = new HelixResponse<DataPage<Video>>(info);

                return response;
            }

            info = RestUtil.CreateHelixRequest("videos", Method.GET, info);

            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<DataPage<Video>>(info);

                return response;
            }
            
            info.request = info.request.AddPaging(parameters);
            info = await RestUtil.ExecuteAsync(info);

            response = new HelixResponse<DataPage<Video>>(info);

            return response;
        }

        /// <summary>
        /// Asynchronously gets a complete list of information on one or more videos.
        /// </summary>
        public static async Task<IHelixResponse<DataPage<Video>>>
        GetVideosAsync(RestInfo<DataPage<Video>> info, VideosParameters parameters, RequestSettings settings)
        {
            IHelixResponse<DataPage<Video>> response = default;

            if (parameters.IsNull())
            {
                info.SetInputError(new ArgumentNullException(nameof(parameters)));

                response = new HelixResponse<DataPage<Video>>(info);

                return response;
            }

            if ((parameters.ids.IsValid() && (parameters.user_id.IsValid() || parameters.game_id.IsValid())) ||
               (parameters.user_id.IsValid() && parameters.game_id.IsValid()))
            {
                info.SetInputError(new ArgumentException("Only one or more video ID's, one user ID, or one game ID can be provided."));

                response = new HelixResponse<DataPage<Video>>(info);

                return response;
            }

            info = RestUtil.CreateHelixRequest("videos", Method.GET, info);

            if (info.exception_source != RestErrorSource.None)
            {
                response = new HelixResponse<DataPage<Video>>(info);

                return response;
            }

            info.request = info.request.AddPaging(parameters);
            info = await RestUtil.TraceExecuteAsync<Video, DataPage<Video>>(info, parameters);

            response = new HelixResponse<DataPage<Video>>(info);

            return response;
        }

        #endregion
        */
    }
}