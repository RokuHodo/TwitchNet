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

            #region Helpers

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
            ValidateAuthorizationParameters(HelixInfo info, HelixResponse response)
            {
                if (info.required_scopes != 0)
                {
                    // Authentication is requirted, a Bearer token must be provided.
                    if (!info.bearer_token.IsValid())
                    {
                        // Bearer token has not been provided.
                        Scopes[] missing_scopes = EnumUtil.GetFlagValues<Scopes>(info.required_scopes);
                        AvailableScopesException inner_exception = new AvailableScopesException("One or more scopes are required for authentication.", missing_scopes);

                        response.SetInputError(new ArgumentException("A Bearer token was required and either not provided or was null, empty, or contained only whitespace. See the inner exception for the required authentication.", nameof(info.bearer_token), inner_exception), info.settings);

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
                            response.SetScopesError(new AvailableScopesException("One or more scopes are missing from the provided Bearer token.", missing_scopes), info.settings);

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
                    response.SetInputError(new ArgumentException("A Bearer token or Client ID must be provided to authenticate the request."), info.settings);

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

            #endregion

            // TODO: Reimplement /analytics/extensions
            #region /analytics/extensions
            /*
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
            */
            #endregion

            // TODO: Reimplement /analytics/games
            #region /analytics/games
            /*
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
            */
            #endregion

            // TODO: Reimplement /bits/leaderboard
            #region /bits/leaderboard
            /*
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
            */
            #endregion

            // TODO: Reimplement /clips
            #region /clips
            /*
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
            */
            #endregion

            // TODO: Implement /entitlements/codes

            // TODO: Reimplement /entitlements/upload
            #region /entitlements/upload
            /*
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
            */
            #endregion

            // TODO: Implement /streams/markers

            #region /games

            /// <summary>
            /// Asynchronously gets a list of games.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the list of games.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.
            /// Thrown if all provided game ID's and game names are null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="ParameterCountException">
            /// Thrown if no game ID's or game names are provided.            
            /// Thrown if more than 100 total game ID's and/or game names are provided.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<Game>>>
            GetGamesAsync(HelixInfo info, GamesParameters parameters)
            {
                HelixResponse<Data<Game>> response = new HelixResponse<Data<Game>>();
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                int count = parameters.ids.Count + parameters.names.Count;
                if (count == 0)
                {
                    response.SetInputError(new ParameterCountException("At least one game ID or game name must be provided.", 100, count), info.settings);

                    return response;
                }

                parameters.ids      = parameters.ids.RemoveInvalidAndDuplicateValues();
                parameters.names    = parameters.names.RemoveInvalidAndDuplicateValues();

                count = parameters.ids.Count + parameters.names.Count;
                if (count == 0)
                {
                    response.SetInputError(new ArgumentException("All provided game ID's and/or game names were null, empty, or contained only whitespace."), info.settings);

                    return response;
                }
                else if (count > 100)
                {
                    response.SetInputError(new ParameterCountException("A maximum of 100 total game ID's and/or names can be specified at one time.", 100, count), info.settings);

                    return response;
                }

                RestRequest request = GetBaseRequest("games", Method.GET, info);
                request.AddParameters(parameters);
                
                RestResponse<Data<Game>> _response = await CLIENT_HELIX.ExecuteAsync<Data<Game>>(request, HandleResponse);
                response = new HelixResponse<Data<Game>>(_response);

                return response;
            }

            #endregion

            #region /games/top

            /// <summary>
            /// Asynchronously gets a single page of top games, most popular first.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of top videos.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.
            /// Thrown if after and before are provided.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<Game>>>
            GetTopGamesPageAsync(HelixInfo info, TopGamesParameters parameters)
            {
                HelixResponse<DataPage<Game>> response = new HelixResponse<DataPage<Game>>();
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                if (!parameters.IsNull())
                {
                    parameters.first    = parameters.first.Clamp(1, 100);
                    parameters.after    = parameters.after.NullIfInvalid();
                    parameters.before   = parameters.before.NullIfInvalid();

                    if (parameters.after.IsValid() && parameters.before.IsValid())
                    {
                        response.SetInputError(new ArgumentException("Only one pagination direction can be specified. Provide either 'after' or 'before', not both."), info.settings);

                        return response;
                    }                    
                }                

                RestRequest request = GetBaseRequest("games/top", Method.GET, info);
                request.AddParameters(parameters);

                // TODO: /games/top - Sanitize the list based on the game ID and return a distinct list.
                RestResponse<DataPage<Game>> _response = await CLIENT_HELIX.ExecuteAsync<DataPage<Game>>(request, HandleResponse);
                response = new HelixResponse<DataPage<Game>>(_response);

                return response;
            }

            /// <summary>
            /// Asynchronously gets a complete list of top games, most popular first.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the complete list of top videos.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.
            /// Thrown if after and before are provided.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<Game>>>
            GetTopGamesAsync(HelixInfo info, TopGamesParameters parameters)
            {
                HelixResponse<DataPage<Game>> response = new HelixResponse<DataPage<Game>>();
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                string direction = "after";
                if (!parameters.IsNull())
                {

                    parameters.after = parameters.after.NullIfInvalid();
                    parameters.before = parameters.before.NullIfInvalid();
                    if (parameters.after.IsValid() && parameters.before.IsValid())
                    {
                        response.SetInputError(new ArgumentException("Only one pagination direction can be specified. Provide either 'after' or 'before', not both."), info.settings);

                        return response;
                    }                    

                    // TODO: /games/top - Disallow 'before' until it works properly?
                    if (parameters.before.IsValid())
                    {
                        direction = "before";
                    }

                    parameters.first = parameters.first.Clamp(1, 100);
                }

                RestRequest request = GetBaseRequest("games/top", Method.GET, info);
                request.AddParameters(parameters);

                // TODO: /games/top - Sanitize the list based on the game ID and return a distinct list.
                RestResponse<DataPage<Game>> _response = await CLIENT_HELIX.TraceExecuteAsync<Game, DataPage<Game>>(request, direction, HandleResponse);
                response = new HelixResponse<DataPage<Game>>(_response);

                return response;
            }

            #endregion

            #region /streams

            /// <summary>
            /// Asynchronously gets a single page of streams.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of streams.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.
            /// Thrown if after and before are provided.
            /// </exception>
            /// <exception cref="ParameterCountException">
            /// Thrown if more than 100 total community ID's are provided.
            /// Thrown if more than 100 total game ID's are provided.
            /// Thrown if more than 100 total user ID's are provided.
            /// Thrown if more than 100 total user logins are provided.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<Stream>>>
            GetStreamsPageAsync(HelixInfo info, StreamsParameters parameters)
            {
                HelixResponse<DataPage<Stream>> response = new HelixResponse<DataPage<Stream>>();
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                if (!parameters.IsNull())
                {
                    parameters.after = parameters.after.NullIfInvalid();
                    parameters.before = parameters.before.NullIfInvalid();
                    if (parameters.after.IsValid() && parameters.before.IsValid())
                    {
                        response.SetInputError(new ArgumentException("Only one pagination direction can be specified. Provide either 'after' or 'before', not both."), info.settings);

                        return response;
                    }

                    parameters.community_ids = parameters.community_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.community_ids.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total community ID's can be specified at one time.", nameof(parameters.community_ids), 100, parameters.community_ids.Count), info.settings);

                        return response;
                    }

                    parameters.game_ids = parameters.game_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.game_ids.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total game ID's can be specified at one time.", nameof(parameters.game_ids), 100, parameters.game_ids.Count), info.settings);

                        return response;
                    }

                    parameters.user_ids = parameters.user_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.user_ids.Count > 100)
                    {                        
                        response.SetInputError(new ParameterCountException("A maximum of 100 total user ID's can be specified at one time.", nameof(parameters.user_ids), 100, parameters.user_ids.Count), info.settings);

                        return response;
                    }

                    parameters.user_logins = parameters.user_logins.RemoveInvalidAndDuplicateValues();
                    if (parameters.user_logins.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total user logins can be specified at one time.", nameof(parameters.user_logins), 100, parameters.user_logins.Count), info.settings);

                        return response;
                    }                    

                    parameters.first = parameters.first.Clamp(1, 100);
                }

                RestRequest request = GetBaseRequest("streams", Method.GET, info);
                request.AddParameters(parameters);

                // TODO: /streams - Sanitize the list based on the stream ID and return a distinct list.
                RestResponse<DataPage<Stream>> _response = await CLIENT_HELIX.ExecuteAsync<DataPage<Stream>>(request, HandleResponse);
                response = new HelixResponse<DataPage<Stream>>(_response);

                return response;
            }

            /// <summary>
            /// Asynchronously gets a complete list of streams.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the complete list of streams.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.
            /// Thrown if after and before are provided.
            /// </exception>
            /// <exception cref="ParameterCountException">
            /// Thrown if more than 100 total community ID's are provided.
            /// Thrown if more than 100 total game ID's are provided.
            /// Thrown if more than 100 total user ID's are provided.
            /// Thrown if more than 100 total user logins are provided.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<Stream>>>
            GetStreamsAsync(HelixInfo info, StreamsParameters parameters)
            {
                HelixResponse<DataPage<Stream>> response = new HelixResponse<DataPage<Stream>>();
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                string direction = "after";
                if (!parameters.IsNull())
                {
                    parameters.after = parameters.after.NullIfInvalid();
                    parameters.before = parameters.before.NullIfInvalid();
                    if (parameters.after.IsValid() && parameters.before.IsValid())
                    {
                        response.SetInputError(new ArgumentException("Only one pagination direction can be specified. Provide either 'after' or 'before', not both."), info.settings);

                        return response;
                    }

                    parameters.community_ids = parameters.community_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.community_ids.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total community ID's can be specified at one time.", nameof(parameters.community_ids), 100, parameters.community_ids.Count), info.settings);

                        return response;
                    }

                    parameters.game_ids = parameters.game_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.game_ids.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total game ID's can be specified at one time.", nameof(parameters.game_ids), 100, parameters.game_ids.Count), info.settings);

                        return response;
                    }

                    parameters.user_ids = parameters.user_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.user_ids.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total user ID's can be specified at one time.", nameof(parameters.user_ids), 100, parameters.user_ids.Count), info.settings);

                        return response;
                    }

                    parameters.user_logins = parameters.user_logins.RemoveInvalidAndDuplicateValues();
                    if (parameters.user_logins.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total user logins can be specified at one time.", nameof(parameters.user_logins), 100, parameters.user_logins.Count), info.settings);

                        return response;
                    }

                    // TODO: /streams - Disallow 'before' until it works properly?
                    if (parameters.before.IsValid())
                    {
                        direction = "before";
                    }

                    parameters.first = parameters.first.Clamp(1, 100);
                }

                RestRequest request = GetBaseRequest("streams", Method.GET, info);
                request.AddParameters(parameters);

                // TODO: /streams - Sanitize the list based on the stream ID and return a distinct list.
                RestResponse<DataPage<Stream>> _response = await CLIENT_HELIX.TraceExecuteAsync<Stream, DataPage<Stream>>(request, direction, HandleResponse);
                response = new HelixResponse<DataPage<Stream>>(_response);

                return response;
            }

            /// <summary>
            /// Asynchronously checks to see if a user is streaming.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="user_id">The ID of the user to check.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> is set to true is the user is streaming, otherwise false.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.
            /// Thrown if the user ID is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<bool>>
            IsStreamLiveByUserIDAsync(HelixInfo info, string user_id)
            {
                HelixResponse<bool> response = new HelixResponse<bool>();
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                if (!user_id.IsValid())
                {
                    response.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(user_id)), info.settings);

                    return response;
                }

                StreamsParameters parameters = new StreamsParameters();
                parameters.user_ids.Add(user_id);

                IHelixResponse<DataPage<Stream>> _response = await GetStreamsPageAsync(info, parameters);

                bool result = _response.exception.IsNull() ? _response.result.data.IsValid() : false;
                response = new HelixResponse<bool>(_response, result);

                return response;
            }

            /// <summary>
            /// Asynchronously checks to see if a user is streaming.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="user_login">The login of the user to check.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> is set to true is the user is streaming, otherwise false.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.
            /// Thrown if the user login is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<bool>>
            IsStreamLiveByUserLoginAsync(HelixInfo info, string user_login)
            {
                HelixResponse<bool> response = new HelixResponse<bool>();
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                if (!user_login.IsValid())
                {
                    response.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(user_login)), info.settings);

                    return response;
                }

                StreamsParameters parameters = new StreamsParameters();
                parameters.user_logins.Add(user_login);

                IHelixResponse<DataPage<Stream>> _response = await GetStreamsPageAsync(info, parameters);

                bool result = _response.exception.IsNull() ? _response.result.data.IsValid() : false;
                response = new HelixResponse<bool>(_response, result);

                return response;
            }

            #endregion

            // TODO: Implement /streams/markers

            // TODO: Reimplement /streams/metadata
            #region /streams/metadata
            /*
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
            */
            #endregion

            #region /users

            /// <summary>
            /// <para>Asynchronously gets a list of users.</para>
            /// <para>
            /// Optional scope: <see cref="Scopes.UserReadEmail"/>.
            /// If a Bearer token is provided provided, the email of the associated user is included in the response.
            /// </para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">
            /// A set of rest parameters.
            /// If parameters are not provided, the user is looked up by the specified bearer token.            
            /// </param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the list of users.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null and no Bearer token is provided.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.
            /// Thrown if all provided user ID's and user logins are null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="ParameterCountException">
            /// Thrown if no user ID's or user logins are provided.            
            /// Thrown if more than 100 total user ID's and/or user logins are provided.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<User>>>
            GetUsersAsync(HelixInfo info, UsersParameters parameters)
            {
                HelixResponse<Data<User>> response = new HelixResponse<Data<User>>();
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                int count = 0;
                if (!parameters.IsNull())
                {
                    // If the user provided parameters, they did it for a reason.
                    count = parameters.ids.Count + parameters.logins.Count;
                    if (count == 0)
                    {
                        response.SetInputError(new ParameterCountException("At least one user ID or user login must be provided.", 100, count), info.settings);

                        return response;
                    }

                    parameters.ids = parameters.ids.RemoveInvalidAndDuplicateValues();
                    parameters.logins = parameters.logins.RemoveInvalidAndDuplicateValues();

                    count = parameters.ids.Count + parameters.logins.Count;
                    if (count == 0)
                    {
                        response.SetInputError(new ArgumentException("All provided user ID's and/or user logins were null, empty, or contained only whitespace."), info.settings);

                        return response;
                    }
                    else if (count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total user ID's and/or user logins can be specified at one time.", 100, count), info.settings);

                        return response;
                    }
                }
                else if (!info.bearer_token.IsValid())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters), "Parameters cannot be null when no Bearer token is provided."), info.settings);

                    return response;
                }

                RestRequest request = GetBaseRequest("users", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<Data<User>> _response = await CLIENT_HELIX.ExecuteAsync<Data<User>>(request, HandleResponse);
                response = new HelixResponse<Data<User>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously sets the description of a user. The user is specified by the bearer token.</para>
            /// <para>Required scope: <see cref="Scopes.UserEdit"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="description">The text to set the user's description to.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains updated user information.
            /// </returns>
            /// <exception cref="ArgumentException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.UserEdit"/> scope.</exception>
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
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains updated user information.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="ArgumentException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.UserEdit"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<User>>>
            SetUserDescriptionAsync(HelixInfo info, DescriptionParameters parameters)
            {
                info.required_scopes = Scopes.UserEdit;

                HelixResponse<Data<User>> response = new HelixResponse<Data<User>>();
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }        

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!parameters.description.IsValid())
                {
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
            /// <param name="info">The information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">
            /// A set of rest parameters specific to this request.
            /// If no user ID is specified, the user is implicityly specified from the bearer token.
            /// </param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains active extensions the user has instlled.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null and no Bearer token is provided.</exception>            
            /// <exception cref="ArgumentException">
            /// Thrown if bearer_token and client_id are null, empty, or contains only whitespace.
            /// Thrown if the user_id parameter, if specified, is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<ActiveExtensions>>
            GetUserActiveExtensionsAsync(HelixInfo info, ActiveExtensionsParameters parameters)
            {
                HelixResponse<ActiveExtensions> response = new HelixResponse<ActiveExtensions>();

                if (!ValidateAuthorizationParameters(info, response))
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

                RestResponse<ActiveExtensions> _response = await CLIENT_HELIX.ExecuteAsync<ActiveExtensions>(request, HandleResponse);
                response = new HelixResponse<ActiveExtensions>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously updates the installed extensions for a user specified by the bearer token.</para>
            /// <para>Required scope: <see cref="Scopes.UserEditBroadcast"/>.</para>
            /// </summary>
            /// <param name="info">The information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters specific to this request.<para>
            /// Any extensions specified outside of the valid extension slots for each type are ignored.
            /// The valid extension slots for each type are specified under each <see cref="ActiveExtensionsData"/> member.
            /// The (x, y) corrdinates are applicable only to component extensions.
            /// </para>
            /// </param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains active extensions the user has instlled after the changes have been applied.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if the <see cref="UpdateExtensionsParameters"/>, <see cref="UpdateExtensionsParameters.extensions"/>, or <see cref="ActiveExtensions.data"/> are null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if the bearer token is null, empty, or contains only whitespace.
            /// Thrown if each extension slot for each extension type is empty or null.
            /// Thrown if the name, ID, or version for each specified active extension is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="ArgumentOutOfRangeException">Thrown if the the either (x, y) coordinate for a component extension exceeds the range (0, 0) to (8000, 5000).</exception>
            /// <exception cref="DuplicateExtensionException">Thrown if an extension ID is set in more then one valid slot across all extension types.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes, when specified, does not include the <see cref="Scopes.UserEdit"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<ActiveExtensions>>
            UpdateUserExtensionsAsync(HelixInfo info, UpdateExtensionsParameters parameters)
            {
                HelixResponse<ActiveExtensions> response = new HelixResponse<ActiveExtensions>();

                info.required_scopes = Scopes.UserEditBroadcast;
                if (!ValidateAuthorizationParameters(info, response))
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

                parameters.extensions.data.panel = ValidateExtensionSlots(parameters.extensions.data.panel, ExtensionType.Panel, response, info.settings);
                if (!response.exception.IsNull())
                {
                    return response;
                }

                parameters.extensions.data.overlay = ValidateExtensionSlots(parameters.extensions.data.overlay, ExtensionType.Overlay, response, info.settings);
                if (!response.exception.IsNull())
                {
                    return response;
                }

                parameters.extensions.data.component = ValidateExtensionSlots(parameters.extensions.data.component, ExtensionType.Component, response, info.settings);
                if (!response.exception.IsNull())
                {
                    return response;
                }

                if (!parameters.extensions.data.component.IsValid() && !parameters.extensions.data.panel.IsValid() && !parameters.extensions.data.overlay.IsValid())
                {
                    response.SetInputError(new ArgumentException("At least one extension type must be updated."), info.settings);

                    return response;
                }

                if (!ValidateUniqueExtensionIDs(parameters.extensions.data, response, info.settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("users/extensions", Method.PUT, info);
                request.AddParameters(parameters);

                RestResponse<ActiveExtensions> _response = await CLIENT_HELIX.ExecuteAsync<ActiveExtensions>(request, HandleResponse);
                response = new HelixResponse<ActiveExtensions>(_response);

                return response;
            }

            /// <summary>
            /// Checks to make sure that the required extension members are not null, empty, contain only whitespace, and/or are not out of bounds.
            /// </summary>
            /// <param name="extensions">The extensions to be updated.</param>
            /// <param name="type">The extension type.</param>
            /// <param name="response">The Helix response to handle errors.</param>
            /// <param name="settings">The request settings to determine how errors are handled.</param>
            /// <returns>Returns the filtered extensions that Twitch accepts.</returns>
            /// <exception cref="ArgumentException">Thrown if the name, ID, or version for each specified active extension is null, empty, or contains only whitespace.</exception>
            /// <exception cref="ArgumentOutOfRangeException">Thrown if the the either (x, y) coordinate for a component extension exceeds the range (0, 0) to (8000, 5000).</exception>
            private static Dictionary<string, ActiveExtension>
            ValidateExtensionSlots(Dictionary<string, ActiveExtension> extensions, ExtensionType type, HelixResponse<ActiveExtensions> response, HelixRequestSettings settings)
            {
                // Only look for the keys we care about in case the user set other keys by accident.
                extensions = FilterExtensionSlots(extensions, type);
                if (!extensions.IsValid())
                {
                    return extensions;
                }                

                foreach (ActiveExtension extension in extensions.Values)
                {
                    if (!extension.id.IsValid())
                    {
                        response.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(extension.id)), settings);

                        return extensions;
                    }

                    if (!extension.name.IsValid())
                    {
                        response.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(extension.name)), settings);

                        return extensions;
                    }

                    if (!extension.version.IsValid())
                    {
                        response.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(extension.version)), settings);

                        return extensions;
                    }

                    if (type == ExtensionType.Component)
                    {
                        if (!extension.x.IsNull() && (extension.x.Value < 0 || extension.x.Value > 8000))
                        {
                            response.SetInputError(new ArgumentOutOfRangeException(nameof(extension.x), extension.x.Value, "The x coordinate must be between 0 and 8000, inclusive."), settings);

                            return extensions;
                        }

                        if (!extension.y.IsNull() && (extension.y.Value < 0 || extension.y.Value > 5000))
                        {
                            response.SetInputError(new ArgumentOutOfRangeException(nameof(extension.y), extension.y.Value, "The y coordinate must be between 0 and 8000, inclusive."), settings);

                            return extensions;
                        }
                    }
                }

                return extensions;
            }

            /// <summary>
            /// Filteres through the extensions to be updated and returns only the slots that Twitch accepts.
            /// </summary>
            /// <param name="extensions">The extensions to be updated.</param>
            /// <param name="type">The extension type.</param>
            /// <returns>Returns the filtered extensions that Twitch accepts.</returns>
            private static Dictionary<string, ActiveExtension>
            FilterExtensionSlots(Dictionary<string, ActiveExtension> extensions, ExtensionType type)
            {
                extensions = extensions.RemoveNullValues();
                if (!extensions.IsValid())
                {
                    return extensions;
                }

                string[] keys;
                if (type == ExtensionType.Panel)
                {
                    keys = new string[] { "1", "2", "3" };
                }
                else if (type == ExtensionType.Overlay)
                {
                    keys = new string[] { "1" };
                }
                else
                {
                    keys = new string[] { "1", "2", "3" };
                }

                Dictionary<string, ActiveExtension> result = new Dictionary<string, ActiveExtension>();
                foreach (string key in keys)
                {
                    if(!extensions.TryGetValue(key, out ActiveExtension temp))
                    {
                        continue;
                    }

                    result[key] = temp;
                }

                return result;
            }

            /// <summary>
            /// Checks to make sure that each extension ID is unique across all extension types and slots.
            /// </summary>
            /// <param name="data">The filtered and validated extension data to update.</param>
            /// <param name="response">The Helix response to handle errors.</param>
            /// <param name="settings">The request settings to determine how errors are handled.</param>
            /// <returns>
            /// Returns true if each extension ID is unique across all extension types and slots.
            /// Returns false otherwise.
            /// </returns>
            /// <exception cref="DuplicateExtensionException">Thrown if an extension ID is set in more then one valid slot across all extension types.</exception>
            private static bool
            ValidateUniqueExtensionIDs(ActiveExtensionsData data, HelixResponse<ActiveExtensions> response, HelixRequestSettings settings)
            {                
                List<ActiveExtension> extensions = new List<ActiveExtension>(6);
                if (data.panel.IsValid())
                {
                    extensions.AddRange(data.panel.Values);
                }

                if (data.overlay.IsValid())
                {
                    extensions.AddRange(data.overlay.Values);
                }

                if (data.component.IsValid())
                {
                    extensions.AddRange(data.component.Values);
                }

                extensions.TrimExcess();

                List<string> ids = new List<string>(extensions.Count);
                foreach (ActiveExtension extension in extensions) 
                {
                    if (ids.Contains(extension.id))
                    {
                        response.SetInputError(new DuplicateExtensionException("The extension ID, " + extension.id + ", was attempted to be set in two or more extension slots. An extension can only be set to one slot.", extension), settings);

                        return false;
                    }

                    ids.Add(extension.id);
                }

                return true;
            }

            #endregion

            #region /users/extensions/list

            /// <summary>
            /// <para>
            /// Asynchronously gets a list of all extensions a user has installed, activated or deactivated.
            /// The user is specified by the provided bearer token.
            /// </para>
            /// <para>Required Scope: <see cref="Scopes.UserReadBroadcast"/></para>
            /// </summary>
            /// <param name="info">The information used to authorize and/or authenticate the request.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains extensions the user has instlled, activated or deactivated..
            /// </returns>
            /// <exception cref="ArgumentException">Thrown if the bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes, when specified, does not include the <see cref="Scopes.UserReadBroadcast"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<Extension>>>
            GetUserExtensionsAsync(HelixInfo info)
            {
                HelixResponse<Data<Extension>> response = new HelixResponse<Data<Extension>>();

                info.required_scopes = Scopes.UserReadBroadcast;
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("users/extensions/list", Method.GET, info);

                RestResponse<Data<Extension>> _response = await CLIENT_HELIX.ExecuteAsync<Data<Extension>>(request, HandleResponse);
                response = new HelixResponse<Data<Extension>>(_response);

                return response;
            }

            #endregion

            #region /users/follows        

            /// <summary>
            /// Asynchronously gets a single page of a user's following list.
            /// </summary>
            /// <param name="info">The information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">
            /// A set of rest parameters to add to the request.
            /// The to_id is ignored if specified.
            /// </param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of a user's following list.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if bearer_token and client_id are null, empty, or contains only whitespace.
            /// Thrown if from_id is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingPageAsync(HelixInfo info, FollowsParameters parameters)
            {
                HelixResponse<FollowsDataPage<Follow>> response = new HelixResponse<FollowsDataPage<Follow>>();

                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!parameters.from_id.IsValid())
                {
                    response.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(parameters.from_id)), info.settings);

                    return response;
                }

                // Get rid of any junk the user may have provided.
                // Leave the after alone in case they're requesting a specific page.
                parameters.to_id = string.Empty;

                response = await GetUserRelationshipPageAsync(info, parameters) as HelixResponse<FollowsDataPage<Follow>>;

                return response;
            }

            /// <summary>
            /// Asynchronously gets a user's complete following list.
            /// </summary>
            /// <param name="info">The information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">
            /// A set of rest parameters to add to the request.
            /// The to_id is ignored if specified.
            /// </param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the user's complete following list.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if bearer_token and client_id are null, empty, or contains only whitespace.
            /// Thrown if from_id is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingAsync(HelixInfo info, FollowsParameters parameters)
            {
                HelixResponse<FollowsDataPage<Follow>> response = new HelixResponse<FollowsDataPage<Follow>>();

                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!parameters.from_id.IsValid())
                {
                    response.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(parameters.from_id)), info.settings);

                    return response;
                }

                // Get rid of any junk the user may have provided.
                // Leave the after alone in case they want all users after a specific page.
                parameters.to_id = string.Empty;

                response = await GetUserRelationshipAsync(info, parameters) as HelixResponse<FollowsDataPage<Follow>>;

                return response;
            }

            /// <summary>
            /// Asynchronously gets a single page of a user's followers list.
            /// </summary>
            /// <param name="info">The information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">
            /// A set of rest parameters to add to the request.
            /// The from_id is ignored if specified.
            /// </param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of a user's followers list.
            /// </returns>        
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if bearer_token and client_id are null, empty, or contains only whitespace.
            /// Thrown if to_id is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersPageAsync(HelixInfo info, FollowsParameters parameters)
            {
                HelixResponse<FollowsDataPage<Follow>> response = new HelixResponse<FollowsDataPage<Follow>>();

                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!parameters.to_id.IsValid())
                {
                    response.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(parameters.to_id)), info.settings);

                    return response;
                }

                // Get rid of any junk the user may have provided.
                // Leave the after alone in case they're requesting a specific page.
                parameters.from_id = string.Empty;

                response = await GetUserRelationshipPageAsync(info, parameters) as HelixResponse<FollowsDataPage<Follow>>;

                return response;
            }

            /// <summary>
            /// Asynchronously gets a user's complete follower list.
            /// </summary>
            /// <param name="info">The information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">
            /// A set of rest parameters to add to the request.
            /// The from_id is ignored if specified.
            /// </param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the user's complete follower list.
            /// </returns>  
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if bearer_token and client_id are null, empty, or contains only whitespace.
            /// Thrown if to_id is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersAsync(HelixInfo info, FollowsParameters parameters)
            {
                HelixResponse<FollowsDataPage<Follow>> response = new HelixResponse<FollowsDataPage<Follow>>();

                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!parameters.to_id.IsValid())
                {
                    response.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(parameters.to_id)), info.settings);

                    return response;
                }

                // Get rid of any junk the user may have provided.
                // Leave the after alone in case they want all users after a specific page.
                parameters.from_id = string.Empty;

                response = await GetUserRelationshipAsync(info, parameters) as HelixResponse<FollowsDataPage<Follow>>;

                return response;
            }

            /// <summary>
            /// Asynchronously checks to see if the from_id user is following the to_id user.
            /// </summary>
            /// <param name="info">The information used to authorize and/or authenticate the request.</param>
            /// <param name="from_id">The ID of the following user.</param>
            /// <param name="to_id">The ID of the followed user.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> is set true if from_id is following to_id, otherwise false.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// Thrown if bearer_token and client_id are null, empty, or contains only whitespace.
            /// Thrown if either from_id and to_id are null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<bool>>
            IsUserFollowingAsync(HelixInfo info, string from_id, string to_id)
            {
                HelixResponse<bool> response = new HelixResponse<bool>();

                if (!to_id.IsValid() || !from_id.IsValid())
                {
                    response.SetInputError(new ArgumentException("Both from_id and to_id must be specified and cannot be null, empty, or contain only whitespace."), info.settings);

                    return response;
                }

                FollowsParameters parameters = new FollowsParameters(from_id, to_id);

                IHelixResponse<FollowsDataPage<Follow>> _response = await GetUserRelationshipPageAsync(info, parameters);

                bool result = _response.exception.IsNull() ? _response.result.data.IsValid() : false;
                response = new HelixResponse<bool>(_response, result);

                return response;
            }

            /// <summary>
            /// Asynchronously gets the relationship between two users, or a single page of a user's following/follower list.
            /// </summary>
            /// <param name="info">The information used to authorize and/or authenticate the request.</param>
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
            /// Thrown if bearer_token and client_id are null, empty, or contains only whitespace.
            /// Thrown if both from_id and to_id are null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserRelationshipPageAsync(HelixInfo info, FollowsParameters parameters)
            {
                HelixResponse<FollowsDataPage<Follow>> response = new HelixResponse<FollowsDataPage<Follow>>();

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!parameters.to_id.IsValid() && !parameters.from_id.IsValid())
                {
                    response.SetInputError(new ArgumentException("At least either from_id or to_id must be specified and cannot be null, empty, or contain only whitespace."), info.settings);

                    return response;
                }

                parameters.first = parameters.first.Clamp(1, 100);

                RestRequest request = GetBaseRequest("users/follows", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<FollowsDataPage<Follow>> _response = await CLIENT_HELIX.ExecuteAsync<FollowsDataPage<Follow>>(request, HandleResponse);
                response = new HelixResponse<FollowsDataPage<Follow>>(_response);

                return response;
            }

            /// <summary>
            /// Asynchronously gets the relationship between two users, or a user's complete following/follower list.
            /// </summary>
            /// <param name="info">The information used to authorize and/or authenticate the request.</param>
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
            /// Thrown if bearer_token and client_id are null, empty, or contains only whitespace.
            /// Thrown if both from_id and to_id are null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserRelationshipAsync(HelixInfo info, FollowsParameters parameters)
            {
                HelixResponse<FollowsDataPage<Follow>> response = new HelixResponse<FollowsDataPage<Follow>>();

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!parameters.to_id.IsValid() && !parameters.from_id.IsValid())
                {
                    response.SetInputError(new ArgumentException("At least either from_id or to_id must be specified and cannot be null, empty, or contain only whitespace."), info.settings);

                    return response;
                }

                parameters.first = parameters.first.Clamp(1, 100);

                RestRequest request = GetBaseRequest("users/follows", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<FollowsDataPage<Follow>> _response = await CLIENT_HELIX.TraceExecuteAsync<Follow, FollowsDataPage<Follow>>(request, HandleResponse);
                response = new HelixResponse<FollowsDataPage<Follow>>(_response);

                return response;
            }

            #endregion

            #region /videos

            /// <summary>
            /// Asynchronously gets information about specific videos, or a single page of videos.
            /// </summary>
            /// <param name="info">The information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters to add to the request.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> containts the queried videos.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if bearer_token and client_id are null, empty, or contains only whitespace.
            /// Thrown if both after and before parameters were specified.
            /// Thrown if no video ID's were specified, and both game ID and user ID were null, empty, or contains only whitespace.
            /// Thrown if any mutiple combination of video ID's, game ID, or user ID were specified.
            /// Thrown if more than 100 video ID's were specified.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<Video>>>
            GetVideosPageAsync(HelixInfo info, VideosParameters parameters)
            {
                HelixResponse<DataPage<Video>> response = new HelixResponse<DataPage<Video>>();

                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if(parameters.after.IsValid() && parameters.before.IsValid())
                {
                    response.SetInputError(new ArgumentException("Only one pagination direction can be specified. Only use either 'after' or 'before'."), info.settings);

                    return response;
                }

                parameters.ids.RemoveInvalidAndDuplicateValues();

                if (!parameters.ids.IsValid() && !parameters.user_id.IsValid() && !parameters.game_id.IsValid())
                {
                    response.SetInputError(new ArgumentException("At least one video ID, one user ID, or one game ID must be provided."), info.settings);

                    return response;                         
                }

                if((parameters.ids.IsValid() && (parameters.user_id.IsValid() ||parameters.game_id.IsValid())) ||
                   (parameters.user_id.IsValid() && parameters.game_id.IsValid()))
                {
                    response.SetInputError(new ArgumentException("Only one or more video ID's, one user ID, or one game ID can be provided."), info.settings);

                    return response;
                }

                if (parameters.ids.IsValid() && parameters.ids.Count > 100)
                {
                    response.SetInputError(new ArgumentException("A maximum of 100 video ID's can be specified at one time."), info.settings);

                    return response;
                }

                parameters.first    = parameters.first.Clamp(1, 100);
                parameters.after    = parameters.after.NullIfInvalid();
                parameters.before   = parameters.before.NullIfInvalid();
                parameters.game_id  = parameters.game_id.NullIfInvalid();
                parameters.user_id  = parameters.user_id.NullIfInvalid();

                RestRequest request = GetBaseRequest("videos", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<Video>> _response = await CLIENT_HELIX.ExecuteAsync<DataPage<Video>>(request, HandleResponse);
                response = new HelixResponse<DataPage<Video>>(_response);

                return response;
            }

            /// <summary>
            /// Asynchronously gets information about specific videos, or a complete list of videos.
            /// </summary>
            /// <param name="info">The information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters to add to the request.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> containts the queried videos.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if bearer_token and client_id are null, empty, or contains only whitespace.
            /// Thrown if both after and before parameters were specified.
            /// Thrown if no video ID's were specified, and both game ID and user ID were null, empty, or contains only whitespace.
            /// Thrown if any mutiple combination of video ID's, game ID, or user ID were specified.
            /// Thrown if more than 100 video ID's were specified.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<Video>>>
            GetVideosAsync(HelixInfo info, VideosParameters parameters)
            {
                HelixResponse<DataPage<Video>> response = new HelixResponse<DataPage<Video>>();

                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (parameters.after.IsValid() && parameters.before.IsValid())
                {
                    response.SetInputError(new ArgumentException("Only one pagination direction can be specified. Only use either 'after' or 'before'."), info.settings);

                    return response;
                }

                parameters.ids.RemoveInvalidAndDuplicateValues();

                if (!parameters.ids.IsValid() && !parameters.user_id.IsValid() && !parameters.game_id.IsValid())
                {
                    response.SetInputError(new ArgumentException("At least one video ID, one user ID, or one game ID must be provided."), info.settings);

                    return response;
                }

                if ((parameters.ids.IsValid() && (parameters.user_id.IsValid() || parameters.game_id.IsValid())) ||
                   (parameters.user_id.IsValid() && parameters.game_id.IsValid()))
                {
                    response.SetInputError(new ArgumentException("Only one or more video ID's, one user ID, or one game ID can be provided."), info.settings);

                    return response;
                }

                if(parameters.ids.IsValid() && parameters.ids.Count > 100)
                {
                    response.SetInputError(new ArgumentException("A maximum of 100 video ID's can be specified at one time."), info.settings);

                    return response;
                }

                parameters.first    = parameters.first.Clamp(1, 100);
                parameters.after    = parameters.after.NullIfInvalid();
                parameters.before   = parameters.before.NullIfInvalid();
                parameters.game_id  = parameters.game_id.NullIfInvalid();
                parameters.user_id  = parameters.user_id.NullIfInvalid();

                // TODO: Disallow 'before' until it works properly?
                string direction = parameters.before.IsValid() ? "before" : "after";

                RestRequest request = GetBaseRequest("videos", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<Video>> _response = await CLIENT_HELIX.TraceExecuteAsync<Video, DataPage<Video>>(request, direction, HandleResponse);
                response = new HelixResponse<DataPage<Video>>(_response);

                return response;
            }

            #endregion

            // TODO: Implement /webhook/hub

            // TODO: Implement /webhook/subscriptions
        }
    }
}