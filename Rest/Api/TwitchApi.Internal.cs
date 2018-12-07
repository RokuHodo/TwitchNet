// standard namespaces
using System;
using System.Collections;
using System.Collections.Generic;
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
using TwitchNet.Utilities;

// imported .dll's
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace
TwitchNet.Rest.Api
{
    public static partial class
    TwitchApi
    {
        internal static class
        Internal
        {
            internal static readonly RestClient CLIENT_HELIX = GetHelixClient();

            internal static RestClient
            GetHelixClient()
            {
                RestClient client = new RestClient("https://api.twitch.tv/helix");

                return client;
            }

            internal static RestRequest
            GetBaseRequest(string endpoint, Method method, HelixInfo info)
            {
                RestRequest request = new RestRequest(endpoint, method);
                request.settings = info.settings;
                if (info.bearer_token.IsValid())
                {
                    request.AddHeader("Authorization", "Bearer " + info.bearer_token);
                }

                if (info.client_id.IsValid())
                {
                    request.AddHeader("Client-ID", info.client_id);
                }

                return request;
            }

            internal static bool
            ValidateRequiredBaseParameters(HelixInfo info, HelixResponse response)
            {
                if (info.required_scopes != 0)
                {
                    // Authentication is requirted, a Bearer token must be provided.
                    if (!info.bearer_token.IsValid())
                    {
                        // Bearer token has not been provided.
                        Scopes[] missing_scopes = EnumUtil.GetFlagValues<Scopes>(info.required_scopes);
                        MissingScopesException inner_exception = new MissingScopesException("One or more scopes are required for authentication.", missing_scopes);

                        response.SetInputError(new ArgumentException("Authenticaion was required and a bearer token was either not provided, null, empty, or contain only whitespace. See the inner exception for the required authentication.", nameof(info.bearer_token), inner_exception), info.settings);

                        return false;
                    }
                    else if(info.settings.available_scopes != Scopes.Other)
                    {
                        // Bearer token has been provided, available scopes have been specified.
                        Scopes[] available_scopes = EnumUtil.GetFlagValues<Scopes>(info.settings.available_scopes);
                        foreach (Scopes scope in available_scopes)
                        {
                            if ((scope & info.required_scopes) == scope)
                            {
                                info.required_scopes ^= scope;
                            }
                        }

                        if (info.required_scopes != 0)
                        {
                            Scopes[] missing_scopes = EnumUtil.GetFlagValues<Scopes>(info.required_scopes);
                            response.SetScopesError(new MissingScopesException("One or more scopes are missing from the specified OAuth token.", missing_scopes), info.settings);

                            return false;
                        }
                        else
                        {
                            // Authentication is requirted, all required scopes are included in available_scopes.

                            // I could just let this fall through, but return just for logical flow.
                            return true;
                        }
                    }
                    else
                    {
                        // Bearer token has been provided, no available scopes have been specified.
                        // If no available scopes were specified, it's not inherently an error.
                        // The user might not want to verify the scopes or didn't provide any.

                        // I could just let this fall through, but return just for logical flow.
                        return true;
                    }
                }
                else if (!info.bearer_token.IsValid() && !info.client_id.IsValid())
                {
                    // Authentication is not requirted, neither a bearer token or client ID was provided.
                    response.SetInputError(new ArgumentException("A bearer token or client ID must be provided."), info.settings);

                    return false;
                }

                // Authentication is not requirted, either a bearer token and/or a client ID was provided.
                return true;
            }

            public static async Task<RestResponse<data_type>>
            HandleResponse<data_type>(RestResponse<data_type> response)
            {
                if (response.exception.IsNull())
                {
                    return response;
                }

                if (response.exception is StatusException)
                {
                    // Yep, this is the only difference between the native handler and this one.
                    response.exception = new HelixException("An error was returned by Twitch after executing the requets.", response.content, response.exception);

                    int code = (int)response.status_code;
                    switch (response.request.settings.status_error[code].handling)
                    {
                        case StatusHandling.Error:
                        {
                            throw response.exception;
                        };

                        case StatusHandling.Retry:
                        {
                            StatusCodeSetting status_setting = response.request.settings.status_error[code];

                            ++status_setting.retry_count;
                            if (status_setting.retry_count > status_setting.retry_limit && status_setting.retry_limit != -1)
                            {
                                response.exception = new RetryLimitReachedException("The retry limit " + status_setting.retry_limit + " has been reached for status code " + code + ".", status_setting.retry_limit, response.exception);

                                return await HandleResponse(response);
                            }

                            RateLimit rate_limit = new RateLimit(response.headers);
                            TimeSpan difference = rate_limit.reset_time - DateTime.Now;
                            if (rate_limit.remaining == 0 && difference.TotalMilliseconds > 0)
                            {
                                await Task.Delay(difference);
                            }

                            // Clone the message to a new instance because the same instance can't be sent twice.
                            response.request.CloneMessage();
                            response = await CLIENT_HELIX.ExecuteAsync<data_type>(response.request, HandleResponse);
                        };
                        break;

                        case StatusHandling.Return:
                        default:
                        {
                            return response;
                        }
                    }
                }
                else
                {
                    ErrorHandling handling;
                    if (response.exception is InvalidOperationException)
                    {
                        handling = response.request.settings.invalid_operation_handling;

                    }
                    else if (response.exception is HttpRequestException)
                    {
                        handling = response.request.settings.request_handling;
                    }
                    else
                    {
                        int code = (int)response.status_code;
                        handling = response.request.settings.status_error[code].handling_rety_limit_reached;
                    }

                    switch (handling)
                    {
                        case ErrorHandling.Error:
                        {
                            throw response.exception;
                        }

                        case ErrorHandling.Return:
                        default:
                        {
                            return response;
                        }
                    }
                }

                return response;
            }

            #region To Reimplement

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

            #endregion

            #region /users

            /// <summary>
            /// <para>Asynchronously gets the information about one or more users.</para>
            /// <para>
            /// Optional scope: <see cref="Scopes.UserReadEmail"/>.
            /// If provided, the user's email is included in the response.
            /// </para>
            /// </summary>
            /// <param name="info">The information used to assemble and execute the request.</param>
            /// <param name="parameters">
            /// A set of rest parameters specific to this request.
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
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<User>>>
            GetUsersAsync(HelixInfo info, UsersParameters parameters)
            {
                HelixResponse<Data<User>> response = new HelixResponse<Data<User>>();

                if (!ValidateRequiredBaseParameters(info, response))
                {
                    return response;
                }

                int total_query_parameters = 0;
                if (!parameters.IsNull())
                {
                    parameters.ids.RemoveInvalidAndDuplicateValues();
                    parameters.logins.RemoveInvalidAndDuplicateValues();

                    total_query_parameters = parameters.ids.Count + parameters.logins.Count;

                    if (total_query_parameters > 100)
                    {
                        response.SetInputError(new ArgumentException("A maximum of 100 total user logins and/or user IDs can be specified at one time.", nameof(parameters)), info.settings);

                        return response;
                    }
                }

                if (!info.bearer_token.IsValid() && total_query_parameters == 0)
                {
                    // We know a client ID has been specified if we got this far.
                    // But these conditions must still be true to have a valid request.
                    if (parameters.IsNull())
                    {
                        response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);
                    }
                    else
                    {
                        response.SetInputError(new ArgumentException("A bearer token must be specified if parameters is null, or all specified ids and logins are null, empty, or only contain whitespace.", nameof(info.bearer_token)), info.settings);
                    }

                    return response;
                }

                RestRequest request = GetBaseRequest("users", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<Data<User>> _response = await CLIENT_HELIX.ExecuteAsync<Data<User>>(request, HandleResponse);
                response = new HelixResponse<Data<User>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously sets the description of a user specified by the bearer token.</para>
            /// <para>Required scope: <see cref="Scopes.UserEdit"/>.</para>
            /// </summary>
            /// <param name="info">The information used to assemble and execute the request.</param>
            /// <param name="description">The text to set the user's description to.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains information about the user with the updated description.
            /// </returns>
            /// <exception cref="ArgumentException">Thrown if both bearer token and client ID, or the description are null, empty, or contains only whitespace.</exception>
            /// <exception cref="MissingScopesException">Thrown if the bearer token does not include the <see cref="Scopes.UserEdit"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<User>>>
            SetUserDescriptionAsync(HelixInfo info, string description)
            {
                DescriptionParameters parameters = new DescriptionParameters();
                parameters.description = description;

                IHelixResponse<Data<User>> response = await SetUserDescriptionAsync(info, parameters);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously sets the description of a user specified by the bearer token.</para>
            /// <para>Required scope: <see cref="Scopes.UserEdit"/>.</para>
            /// </summary>
            /// <param name="info">The information used to assemble and execute the request.</param>
            /// <param name="parameters">A set of rest parameters specific to this request.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains information about the user with the updated description.
            /// </returns>
            /// <exception cref="ArgumentException">Thrown if the bearer token or description is null, empty, or contains only whitespace.</exception>
            /// <exception cref="MissingScopesException">Thrown if the bearer token does not include the <see cref="Scopes.UserEdit"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<User>>>
            SetUserDescriptionAsync(HelixInfo info, DescriptionParameters parameters)
            {
                HelixResponse<Data<User>> response = new HelixResponse<Data<User>>();

                // TODO: Change MissingScopesException to handle 403 - Forbidden as well, e.g., "Missing user:edit scope"?
                info.required_scopes = Scopes.UserEdit;
                if (!ValidateRequiredBaseParameters(info, response))
                {
                    return response;
                }        

                if (parameters.IsNull())
                {
                    parameters = new DescriptionParameters();
                }

                if (!parameters.description.IsValid())
                {
                    // There's the possibility that it's null or white space, sanitize it.
                    parameters.description = string.Empty;
                }

                RestRequest request = GetBaseRequest("users", Method.PUT, info);
                request.AddParameters(parameters);

                RestResponse<Data<User>> _response = await CLIENT_HELIX.ExecuteAsync<Data<User>>(request, HandleResponse);
                response = new HelixResponse<Data<User>>(_response);

                return response;
            }

            #endregion

            #region /users/extensions

            /// <summary>
            /// <para>Asynchronously gets a list of active extensions a user has installed.</para>
            /// <para>Optional scopes: <see cref="Scopes.UserReadBroadcast"/> or <see cref="Scopes.UserEditBroadcast"/>.</para>
            /// </summary>
            /// <param name="info">The information used to assemble and execute the request, and while handling the response and any errors that may have occurred.</param>
            /// <param name="parameters">
            /// A set of rest parameters specific to this request.
            /// If no user ID is specified, the user is implicityly specified from the bearer token.
            /// </param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains active extensions the user has instlled.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null when no valid bearer token is specified.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if both bearer token and client ID are null, empty, or contains only whitespace.
            /// Thrown if the specified user ID is null, empty, or only contains whitespace.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<ActiveExtensionsData>>
            GetUserActiveExtensionsAsync(HelixInfo info, ActiveExtensionsParameters parameters)
            {
                HelixResponse<ActiveExtensionsData> response = new HelixResponse<ActiveExtensionsData>();

                if (!ValidateRequiredBaseParameters(info, response))
                {
                    return response;
                }

                if (!info.bearer_token.IsValid() && parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters), "Parameters must be specified if no bearer token is specified."), info.settings);

                    return response;
                }

                // Always do this check regardless if a bearer token was provided.
                // If the user provided parameters, they did it for a reason.
                if (!parameters.IsNull() && !parameters.user_id.IsValid())
                {
                    response.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(parameters.user_id)), info.settings);

                    return response;
                }

                RestRequest request = GetBaseRequest("users/extensions", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<ActiveExtensionsData> _response = await CLIENT_HELIX.ExecuteAsync<ActiveExtensionsData>(request, HandleResponse);
                response = new HelixResponse<ActiveExtensionsData>(_response);

                return response;
            }

            /// <summary>
            /// <para>
            /// Asynchronously updates the installed extensions for a user specified by the bearer token.
            /// The activation state, extension ID, verison number, or {x,y} coordinates (components only) can be updated.
            /// </para>
            /// <para>If a given extension is updated under multiple extension types, the last write wins.
            /// There is no guarantee of write order.
            /// </para>
            /// <para>Required scope: <see cref="Scopes.UserEditBroadcast"/>.</para>
            /// </summary>
            /// <exception cref="ArgumentNullException">Thrown if parameters or the parameters' data or all extension types' are null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if both bearer token is null, empty, or contains only whitespace.
            /// Thrown if each extension slot for each extension type is null.
            /// </exception>
            /// <exception cref="MissingScopesException">Thrown if the available scopes, if specified, does not include the <see cref="Scopes.UserEdit"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<ActiveExtensionsData>>
            UpdateUserExtensionsAsync(HelixInfo info, UpdateExtensionsParameters parameters)
            {
                HelixResponse<ActiveExtensionsData> response = new HelixResponse<ActiveExtensionsData>();

                info.required_scopes = Scopes.UserEditBroadcast;
                if (!ValidateRequiredBaseParameters(info, response))
                {
                    return response;
                }

                // Everything below here is valid going by the API and effectively behaves the same as performing a 'GET'.
                // It shouldn't be allowed. Don't allow any of this.
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (parameters.extensions.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters.extensions)), info.settings);

                    return response;
                }

                if (parameters.extensions.data.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters.extensions.data)), info.settings);

                    return response;
                }

                parameters.extensions.data.panel = ValidateExtensionSlots(parameters.extensions.data.panel, response, info.settings);
                if (!response.exception.IsNull())
                {
                    return response;
                }

                parameters.extensions.data.overlay = ValidateExtensionSlots(parameters.extensions.data.overlay, response, info.settings);
                if (!response.exception.IsNull())
                {
                    return response;
                }

                parameters.extensions.data.component = ValidateExtensionSlots(parameters.extensions.data.component, response, info.settings);
                if (!response.exception.IsNull())
                {
                    return response;
                }

                if (!parameters.extensions.data.component.IsValid() && !parameters.extensions.data.panel.IsValid() && !parameters.extensions.data.overlay.IsValid())
                {
                    response.SetInputError(new ArgumentException("At least one extension type must be updated."), info.settings);

                    return response;
                }

                // TODO: Check and see if any duplicate extension ID's were provided. Only allow unique ID's to guarantee requests.

                RestRequest request = GetBaseRequest("users/extensions", Method.PUT, info);
                request.AddParameters(parameters);

                RestResponse<ActiveExtensionsData> _response = await CLIENT_HELIX.ExecuteAsync<ActiveExtensionsData>(request, HandleResponse);
                response = new HelixResponse<ActiveExtensionsData>(_response);

                return response;
            }

            private static Dictionary<string, ActiveExtension>
            ValidateExtensionSlots(Dictionary<string, ActiveExtension> extensions, HelixResponse<ActiveExtensionsData> response, HelixRequestSettings settings, bool is_component = false)
            {
                extensions = extensions.RemoveNullValues();

                if (!extensions.IsValid())
                {
                    // other types still might have valid extensions
                    return extensions;
                }

                foreach (KeyValuePair<string, ActiveExtension> pair in extensions)
                {
                    if (!pair.Value.id.IsValid())
                    {
                        response.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(pair.Value.id)), settings);

                        return extensions;
                    }

                    if (!pair.Value.name.IsValid())
                    {
                        response.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(pair.Value.name)), settings);

                        return extensions;
                    }

                    if (!pair.Value.version.IsValid())
                    {
                        response.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(pair.Value.version)), settings);

                        return extensions;
                    }

                    // TODO: Check these regardless of the extension type? If they were specified, it was probably done intentionally.
                    if (is_component)
                    {
                        if(pair.Value.x < 0 || pair.Value.x > 8000)
                        {
                            response.SetInputError(new ArgumentOutOfRangeException(nameof(pair.Value.x), pair.Value.x, "The x coordinate must be between 0 and 8000, inclusive."), settings);

                            return extensions;
                        }

                        if (pair.Value.y < 0 || pair.Value.x > 5000)
                        {
                            response.SetInputError(new ArgumentOutOfRangeException(nameof(pair.Value.y), pair.Value.y, "The y coordinate must be between 0 and 8000, inclusive."), settings);

                            return extensions;
                        }
                    }
                }

                return extensions;
            }

            #endregion

            #region To Reimpliment 2

            /*
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
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
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
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
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
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
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
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
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
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
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
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
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

                info.request = info.request.AddParameters(parameters);
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
            /// <exception cref="StatusException">Thrown if an error was returned by Twitch after executing the request.</exception>
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

                info.request = info.request.AddParameters(parameters);
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

            #endregion
        }
    }
}