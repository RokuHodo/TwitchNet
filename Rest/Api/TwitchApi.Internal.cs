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
            internal static readonly RestClient client = GetHelixClient();

            internal static readonly DateTime UNIX_EPOCH_MIN = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

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
            ValidateAuthorizationParameters(HelixInfo info, HelixResponse response, bool app_access_required = false)
            {
                bool bearer_valid = info.bearer_token.IsValid();
                bool client_id_valid = info.bearer_token.IsValid();                

                // An App Access Token is required.
                if (app_access_required && !bearer_valid)
                {
                    response.SetInputError(new ArgumentException("An App Access Token must be provided to authenticate the request."), info.settings);

                    return false;
                }

                // A set of scopes is required for permission.
                if (info.required_scopes != 0)
                {
                    // Bearer token was not provided.
                    if (!bearer_valid)
                    {
                        Scopes[] missing_scopes = EnumUtil.GetFlagValues<Scopes>(info.required_scopes);
                        AvailableScopesException inner_exception = new AvailableScopesException("One or more scopes are required for authentication.", missing_scopes);

                        response.SetInputError(new ArgumentException("A Bearer token must be provided to authenticate the request. See the inner exception for the lost of required scopes.", nameof(info.bearer_token), inner_exception), info.settings);

                        return false;
                    }
                    // Available scopes have been specified.
                    else if (info.settings.available_scopes != Scopes.Other)
                    {
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
                            response.SetScopesError(new AvailableScopesException("One or more scopes are missing from the provided available scopes associated with the Bearer token.", missing_scopes), info.settings);

                            return false;
                        }
                    }
                }

                // At this point we know that either no authentication is needed, or authentication is required and all checks passed.
                // Only the client ID really needs to checked here, but just for the sake of completion.
                if (!bearer_valid && !client_id_valid)
                {
                    response.SetInputError(new ArgumentException("A Bearer token or Client ID must be provided to authenticate the request."), info.settings);

                    return info.bearer_token.IsValid() || client_id_valid;
                }

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
                            response = await client.ExecuteAsync<data_type>(response.request, HandleResponse);
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

            /// <summary>
            /// <para>Asynchronously gets analytic urls for one or more devloper extensions.</para>
            /// <para>Required Scope: <see cref="Scopes.AnalyticsReadExtensions"/>.</para>
            /// </summary>
            public static async Task<IHelixResponse<DataPage<ExtensionAnalytics>>>
            GetExtensionAnalyticsPageAsync(HelixInfo info, ExtensionAnalyticsParameters parameters)
            {
                info.required_scopes = Scopes.AnalyticsReadExtensions;

                HelixResponse<DataPage<ExtensionAnalytics>> response = new HelixResponse<DataPage<ExtensionAnalytics>>();
                if (!ValidateAuthorizationParameters(info, response, true))
                {
                    return response;
                }

                if (!parameters.IsNull())
                {
                    parameters.after = parameters.after.NullIfInvalid();

                    if (parameters.started_at.HasValue && !parameters.ended_at.HasValue)
                    {
                        response.SetInputError(new ArgumentException("ended_at must be provided if started_at is provided."), info.settings);

                        return response;
                    }
                    else if (!parameters.started_at.HasValue && parameters.ended_at.HasValue)
                    {
                        response.SetInputError(new ArgumentException("started_at must be provided if ended_at is provided."), info.settings);

                        return response;
                    }
                    else if (parameters.started_at.HasValue && parameters.ended_at.HasValue)
                    {
                        parameters.started_at = parameters.started_at.Value.ToUniversalTime();
                        parameters.ended_at = parameters.ended_at.Value.ToUniversalTime();

                        if (parameters.started_at > DateTime.UtcNow)
                        {
                            response.SetInputError(new ArgumentOutOfRangeException(nameof(parameters.started_at), parameters.started_at.Value, "The started_at date cannot be greater than the current date."), info.settings);

                            return response;
                        }

                        if (parameters.ended_at > DateTime.UtcNow)
                        {
                            response.SetInputError(new ArgumentOutOfRangeException(nameof(parameters.ended_at), parameters.ended_at.Value, "The ended_at date cannot be greater than the current date."), info.settings);

                            return response;
                        }

                        if (parameters.started_at.Value > parameters.ended_at.Value)
                        {
                            response.SetInputError(new ArgumentException("The started_at date cannot be later than the ended_at date."), info.settings);

                            return response;
                        }
                    }

                    if (parameters.extension_id.IsValid())
                    {
                        // Just to make sure if something was actually set
                        parameters.after = null;
                    }
                    else
                    {
                        parameters.extension_id = null;
                    }

                    parameters.first = parameters.first.Clamp(1, 100);
                }

                RestRequest request = GetBaseRequest("analytics/extensions", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<ExtensionAnalytics>> _response = await client.ExecuteAsync<DataPage<ExtensionAnalytics>>(request, HandleResponse);
                response = new HelixResponse<DataPage<ExtensionAnalytics>>(_response);

                return response;
            }

            public static async Task<IHelixResponse<DataPage<ExtensionAnalytics>>>
            GetExtensionAnalyticsAsync(HelixInfo info, ExtensionAnalyticsParameters parameters)
            {
                info.required_scopes = Scopes.AnalyticsReadExtensions;

                HelixResponse<DataPage<ExtensionAnalytics>> response = new HelixResponse<DataPage<ExtensionAnalytics>>();
                if (!ValidateAuthorizationParameters(info, response, true))
                {
                    return response;
                }

                if (!parameters.IsNull())
                {
                    parameters.after = parameters.after.NullIfInvalid();

                    if (parameters.started_at.HasValue && !parameters.ended_at.HasValue)
                    {
                        response.SetInputError(new ArgumentException("ended_at must be provided if started_at is provided."), info.settings);

                        return response;
                    }
                    else if (!parameters.started_at.HasValue && parameters.ended_at.HasValue)
                    {
                        response.SetInputError(new ArgumentException("started_at must be provided if ended_at is provided."), info.settings);

                        return response;
                    }
                    else if (parameters.started_at.HasValue && parameters.ended_at.HasValue)
                    {
                        parameters.started_at = parameters.started_at.Value.ToUniversalTime();
                        parameters.ended_at = parameters.ended_at.Value.ToUniversalTime();

                        if (parameters.started_at > DateTime.UtcNow)
                        {
                            response.SetInputError(new ArgumentOutOfRangeException(nameof(parameters.started_at), parameters.started_at.Value, "The started_at date cannot be greater than the current date."), info.settings);

                            return response;
                        }

                        if (parameters.ended_at > DateTime.UtcNow)
                        {
                            response.SetInputError(new ArgumentOutOfRangeException(nameof(parameters.ended_at), parameters.ended_at.Value, "The ended_at date cannot be greater than the current date."), info.settings);

                            return response;
                        }

                        if (parameters.started_at.Value > parameters.ended_at.Value)
                        {
                            response.SetInputError(new ArgumentException("The started_at date cannot be later than the ended_at date."), info.settings);

                            return response;
                        }
                    }

                    if (parameters.extension_id.IsValid())
                    {
                        // Just to make sure if something was actually set
                        parameters.after = null;
                    }
                    else
                    {
                        parameters.extension_id = null;
                    }

                    parameters.first = parameters.first.Clamp(1, 100);
                }

                RestRequest request = GetBaseRequest("analytics/extensions", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<ExtensionAnalytics>> _response = await client.TraceExecuteAsync<ExtensionAnalytics, DataPage<ExtensionAnalytics>>(request, "after", HandleResponse);
                response = new HelixResponse<DataPage<ExtensionAnalytics>>(_response);

                return response;
            }

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

            #region /bits/leaderboard

            /// <summary>
            /// <para>
            /// Asynchronously gets a ranked list of bits leaderboard information for a user.
            /// The user is implicitly specified by the provided Bearer token.
            /// </para>
            /// <para>Required Scope: <see cref="Scopes.BitsRead"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains ranked list of bits leaderboard information.
            /// </returns>
            /// <exception cref="ArgumentException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="ArgumentOutOfRangeException">Thrown if started_at is newer than <see cref="DateTime.Now"/>.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.BitsRead"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<BitsLeaderboardData<BitsUser>>>
            GetBitsLeaderboardAsync(HelixInfo info, BitsLeaderboardParameters parameters)
            {
                info.required_scopes = Scopes.BitsRead;

                HelixResponse<BitsLeaderboardData<BitsUser>> response = new HelixResponse<BitsLeaderboardData<BitsUser>>();
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                if (!parameters.IsNull())
                {
                    parameters.count = parameters.count.Clamp(1, 100);
                    parameters.user_id = parameters.user_id.NullIfInvalid();

                    if(parameters.period == BitsLeaderboardPeriod.All)
                    {
                        parameters.started_at = null;
                    }

                    if(parameters.started_at > DateTime.Now)
                    {
                        response.SetInputError(new ArgumentOutOfRangeException(nameof(parameters.started_at), parameters.started_at, "The started_at date cannot be later than the current date."), info.settings);

                        return response;
                    }
                }

                RestRequest request = GetBaseRequest("bits/leaderboard", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<BitsLeaderboardData<BitsUser>> _response = await client.ExecuteAsync<BitsLeaderboardData<BitsUser>>(request, HandleResponse);
                response = new HelixResponse<BitsLeaderboardData<BitsUser>>(_response);


                return response;
            }

            #endregion

            #region /clips

            /// <summary>
            /// <para>Asynchronously creates a clip.</para>
            /// <para>Required Scope: <see cref="Scopes.ClipsEdit"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the created clip ID and URL to edit the clip.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.
            /// Thrown if the broadcaster ID is null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<CreatedClip>>>
            CreateClipAsync(HelixInfo info, ClipCreationParameters parameters)
            {
                info.required_scopes = Scopes.ClipsEdit;

                HelixResponse<Data<CreatedClip>> response = new HelixResponse<Data<CreatedClip>>();
                if (!ValidateAuthorizationParameters(info, response, true))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!parameters.broadcaster_id.IsValid())
                {
                    response.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(parameters.broadcaster_id)), info.settings);

                    return response;
                }

                RestRequest request = GetBaseRequest("clips", Method.POST, info);
                request.AddParameters(parameters);

                RestResponse<Data<CreatedClip>> _response = await client.ExecuteAsync<Data<CreatedClip>>(request, HandleResponse);
                response = new HelixResponse<Data<CreatedClip>>(_response);

                return response;
            }

            /// <summary>
            /// Asynchronously gets specific clips, or a single page of clips.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> containts the specific clips, or the single page of clips.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.
            /// Thrown if no clip ID's, broadcaster ID, or game ID are provided.
            /// Thrown if any multiple combination of clip ID's, broadcaster ID, or game ID is provided.
            /// Thrown if all clip ID's are are null, empty, or contains only whitespace, if Provided.
            /// Thrown if the broadcaster ID is null, empty, or contains only whitespace, if Provided.
            /// Thrown if the game ID is null, empty, or contains only whitespace, if Provided.
            /// Thrown if started_at is later than ended_at, if provided.
            /// </exception>
            /// <exception cref="ParameterCountException">Thrown if more than 100 total clip ID's are provided.</exception>
            /// <exception cref="ArgumentOutOfRangeException">Thrown if started_at or ended_at is earlier than the UNIX Epoch minimum value or later than the current date.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<Clip>>>
            GetClipsPageAsync(HelixInfo info, ClipsParameters parameters)
            {
                HelixResponse<DataPage<Clip>> response = new HelixResponse<DataPage<Clip>>();
                if (!ValidateAuthorizationParameters(info, response, true))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                parameters.after = parameters.after.NullIfInvalid();
                parameters.before = parameters.before.NullIfInvalid();
                if (parameters.after.IsValid() && parameters.before.IsValid())
                {
                    response.SetInputError(new ArgumentException("Only one pagination direction can be specified. Only use either 'after' or 'before'."), info.settings);

                    return response;
                }

                if (parameters.ids.IsValid())
                {
                    // If clip ID's are provided, assume it's intentional and check for these errors up front for better error messages.
                    parameters.ids = parameters.ids.RemoveInvalidAndDuplicateValues();

                    if (parameters.ids.Count == 0)
                    {
                        response.SetInputError(new ArgumentException("All provided clip ID's were null, empty, or contained only whitespace.", nameof(parameters.ids)), info.settings);

                        return response;
                    }
                    else if (parameters.ids.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total clip ID's can be provided at one time.", nameof(parameters.ids), 100, parameters.ids.Count), info.settings);

                        return response;
                    }

                    parameters.after = null;
                    parameters.before = null;
                    parameters.first = null;
                    parameters.ended_at = null;
                    parameters.started_at = null;
                }

                // S E
                // 0 0 - Don't care, both null
                // 1 0 - Allowed
                // 0 1 - Not allowed

                // ended_at is ignored if no started_at is provided
                if (parameters.ended_at.HasValue && !parameters.started_at.HasValue)
                {
                    parameters.ended_at = null;
                }
                else if (parameters.ended_at.HasValue && !parameters.ended_at.Value.IsInRange(UNIX_EPOCH_MIN, DateTime.Now))
                {
                    response.SetInputError(new ArgumentOutOfRangeException(nameof(parameters.ended_at), parameters.ended_at.Value, "The ended_at date cannot be less than the Unix Epoch minimum or later than the current date."), info.settings);

                    return response;
                }
                else if (parameters.started_at.HasValue && !parameters.started_at.Value.IsInRange(UNIX_EPOCH_MIN, DateTime.Now))
                {
                    response.SetInputError(new ArgumentOutOfRangeException(nameof(parameters.started_at), parameters.started_at.Value, "The started_at date cannot be less than the Unix Epoch minimum or later than the current date."), info.settings);

                    return response;
                }
                else if (parameters.started_at.HasValue && parameters.ended_at.HasValue && parameters.started_at.Value > parameters.ended_at.Value)
                {
                    response.SetInputError(new ArgumentException("The started_at date cannot be later than the ended_at date."), info.settings);

                    return response;
                }

                parameters.broadcaster_id = parameters.broadcaster_id.NullIfInvalid();
                parameters.game_id = parameters.game_id.NullIfInvalid();
                if (!parameters.ids.IsValid() && !parameters.broadcaster_id.IsValid() && !parameters.game_id.IsValid())
                {
                    response.SetInputError(new ArgumentException("At least one or more clip ID, one broadcaster ID, or one game ID must be provided."), info.settings);

                    return response;
                }

                if ((parameters.ids.IsValid() && (parameters.broadcaster_id.IsValid() || parameters.game_id.IsValid())) ||
                   (parameters.broadcaster_id.IsValid() && parameters.game_id.IsValid()))
                {
                    response.SetInputError(new ArgumentException("Only one or more clip ID's, one broadcaster ID, or one game ID can be provided."), info.settings);

                    return response;
                }

                parameters.first = parameters.first.Clamp(1, 100);

                RestRequest request = GetBaseRequest("clips", Method.GET, info);
                request.AddParameters(parameters);

                string direction = parameters.before.IsValid() ? "before" : "after";

                RestResponse<DataPage<Clip>> _response = await client.ExecuteAsync<DataPage<Clip>>(request, HandleResponse);
                response = new HelixResponse<DataPage<Clip>>(_response);

                return response;
            }

            /// <summary>
            /// Asynchronously gets specific clips, or a complete list of clips.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> containts the specific clips, or the single page of clips.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.
            /// Thrown if no clip ID's, broadcaster ID, or game ID are provided.
            /// Thrown if any multiple combination of clip ID's, broadcaster ID, or game ID is provided.
            /// Thrown if all clip ID's are are null, empty, or contains only whitespace, if Provided.
            /// Thrown if the broadcaster ID is null, empty, or contains only whitespace, if Provided.
            /// Thrown if the game ID is null, empty, or contains only whitespace, if Provided.
            /// Thrown if started_at is later than ended_at, if provided.
            /// </exception>
            /// <exception cref="ParameterCountException">Thrown if more than 100 total clip ID's are provided.</exception>
            /// <exception cref="ArgumentOutOfRangeException">Thrown if started_at or ended_at is earlier than the UNIX Epoch minimum value or later than the current date.</exception>
            /// <exception cref="NotSupportedException">Thrown if before is provided.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<Clip>>>
            GetClipsAsync(HelixInfo info, ClipsParameters parameters)
            {
                HelixResponse<DataPage<Clip>> response = new HelixResponse<DataPage<Clip>>();
                if (!ValidateAuthorizationParameters(info, response, true))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                // NOTE: /clips - Temporarily disabling using 'before' while requesting all pages until it works properly.
                // TODO: /clips - Reimplement 'before' when it works propery.
                if (parameters.before.IsValid())
                {
                    response.SetInputError(new NotSupportedException("The pagination direction 'before' is temporarily not supported. Following the cursor using 'before' returns incorrect results and does not work properly on Twitch's back end."), info.settings);

                    return response;
                }

                parameters.after = parameters.after.NullIfInvalid();
                parameters.before = parameters.before.NullIfInvalid();
                if (parameters.after.IsValid() && parameters.before.IsValid())
                {
                    response.SetInputError(new ArgumentException("Only one pagination direction can be specified. Only use either 'after' or 'before'."), info.settings);

                    return response;
                }

                if (parameters.ids.IsValid())
                {
                    // If clip ID's are provided, assume it's intentional and check for these errors up front for better error messages.
                    parameters.ids = parameters.ids.RemoveInvalidAndDuplicateValues();

                    if (parameters.ids.Count == 0)
                    {
                        response.SetInputError(new ArgumentException("All provided clip ID's were null, empty, or contained only whitespace.", nameof(parameters.ids)), info.settings);

                        return response;
                    }
                    else if (parameters.ids.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total clip ID's can be provided at one time.", nameof(parameters.ids), 100, parameters.ids.Count), info.settings);

                        return response;
                    }

                    parameters.after            = null;
                    parameters.before           = null;
                    parameters.first            = null;
                    parameters.ended_at         = null;
                    parameters.started_at       = null;
                }

                // S E
                // 0 0 - Don't care, both null
                // 1 0 - Allowed
                // 0 1 - Not allowed

                // TODO: Clamp each value instead of setting an error?
                // ended_at is ignored if no started_at is provided
                if (parameters.ended_at.HasValue && !parameters.started_at.HasValue)
                {
                    parameters.ended_at = null;
                }
                else if (parameters.ended_at.HasValue && !parameters.ended_at.Value.IsInRange(UNIX_EPOCH_MIN, DateTime.Now))
                {
                    response.SetInputError(new ArgumentOutOfRangeException(nameof(parameters.ended_at), parameters.ended_at.Value, "The ended_at date cannot be less than the Unix Epoch minimum or later than the current date."), info.settings);

                    return response;
                }
                else if (parameters.started_at.HasValue && !parameters.started_at.Value.IsInRange(UNIX_EPOCH_MIN, DateTime.Now))
                {
                    response.SetInputError(new ArgumentOutOfRangeException(nameof(parameters.started_at), parameters.started_at.Value, "The started_at date cannot be less than the Unix Epoch minimum or later than the current date."), info.settings);

                    return response;
                }
                else if (parameters.started_at.HasValue && parameters.ended_at.HasValue && parameters.started_at.Value > parameters.ended_at.Value)
                {
                    response.SetInputError(new ArgumentException("The started_at date cannot be later than the ended_at date."), info.settings);

                    return response;
                }

                parameters.broadcaster_id = parameters.broadcaster_id.NullIfInvalid();
                parameters.game_id = parameters.game_id.NullIfInvalid();
                if (!parameters.ids.IsValid() && !parameters.broadcaster_id.IsValid() && !parameters.game_id.IsValid())
                {
                    response.SetInputError(new ArgumentException("At least one or more clip ID, one broadcaster ID, or one game ID must be provided."), info.settings);

                    return response;
                }

                if ((parameters.ids.IsValid() && (parameters.broadcaster_id.IsValid() || parameters.game_id.IsValid())) ||
                   (parameters.broadcaster_id.IsValid() && parameters.game_id.IsValid()))
                {
                    response.SetInputError(new ArgumentException("Only one or more clip ID's, one broadcaster ID, or one game ID can be provided."), info.settings);

                    return response;
                }

                parameters.first = parameters.first.Clamp(1, 100);

                RestRequest request = GetBaseRequest("clips", Method.GET, info);
                request.AddParameters(parameters);

                string direction = parameters.before.IsValid() ? "before" : "after";

                RestResponse<DataPage<Clip>> _response = await client.TraceExecuteAsync<Clip, DataPage<Clip>>(request, direction, HandleResponse);
                response = new HelixResponse<DataPage<Clip>>(_response);

                return response;
            }

            #endregion

            // TODO: Implement /entitlements/codes

            #region /entitlements/upload

            /// <summary>
            /// <para>Asynchronously creates a URL where you can upload a manifest file and notify users that they have an entitlement.</para>
            /// <para>Required Authorization: App Access Token.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the entitlement upload URL.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if the App Access Token is null, empty, or contains only whitespace.
            /// Thrown if the manifest ID is null, empty, or contains only whitespace.
            /// Thrown if the manifest ID is longer than 64 characters.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<EntitlementUploadUrl>>>
            CreateEntitlementGrantsUploadUrlAsync(HelixInfo info, EntitlementsUploadParameters parameters)
            {
                HelixResponse<Data<EntitlementUploadUrl>> response = new HelixResponse<Data<EntitlementUploadUrl>>();
                if (!ValidateAuthorizationParameters(info, response, true))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!parameters.manifest_id.IsValid())
                {
                    response.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(parameters.manifest_id)), info.settings);

                    return response;
                }

                if (!parameters.manifest_id.Length.IsInRange(1, 64))
                {
                    response.SetInputError(new ArgumentException("The manifest ID must be between 1 and 64 characters long, inclusive.", nameof(parameters.manifest_id)), info.settings);

                    return response;
                }

                RestRequest request = GetBaseRequest("entitlements/upload", Method.POST, info);
                request.AddParameters(parameters);

                RestResponse<Data<EntitlementUploadUrl>> _response = await client.ExecuteAsync<Data<EntitlementUploadUrl>>(request, HandleResponse);
                response = new HelixResponse<Data<EntitlementUploadUrl>>(_response);

                return response;
            }

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

                parameters.ids = parameters.ids.RemoveInvalidAndDuplicateValues();
                parameters.names = parameters.names.RemoveInvalidAndDuplicateValues();

                count = parameters.ids.Count + parameters.names.Count;
                if (count == 0)
                {
                    response.SetInputError(new ArgumentException("All provided game ID's and/or game names were null, empty, or contained only whitespace."), info.settings);

                    return response;
                }
                else if (count > 100)
                {
                    response.SetInputError(new ParameterCountException("A maximum of 100 total game ID's and/or names can be provided at one time.", 100, count), info.settings);

                    return response;
                }

                RestRequest request = GetBaseRequest("games", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<Data<Game>> _response = await client.ExecuteAsync<Data<Game>>(request, HandleResponse);
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
                    parameters.after = parameters.after.NullIfInvalid();
                    parameters.before = parameters.before.NullIfInvalid();
                    if (parameters.after.IsValid() && parameters.before.IsValid())
                    {
                        response.SetInputError(new ArgumentException("Only one pagination direction can be specified. Provide either 'after' or 'before', not both."), info.settings);

                        return response;
                    }

                    parameters.first = parameters.first.Clamp(1, 100);
                }

                RestRequest request = GetBaseRequest("games/top", Method.GET, info);
                request.AddParameters(parameters);

                // TODO: /games/top - Sanitize the list based on the game ID and return a distinct list.
                RestResponse<DataPage<Game>> _response = await client.ExecuteAsync<DataPage<Game>>(request, HandleResponse);
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
            /// <exception cref="ArgumentException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="NotSupportedException">Thrown if before is provided.</exception>
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
                    // NOTE: /games/top - Temporarily disabling using 'before' while requesting all pages until it works properly.
                    // TODO: /games.top - Reimplement 'before' when it works propery.
                    if (parameters.before.IsValid())
                    {
                        response.SetInputError(new NotSupportedException("The pagination direction 'before' is temporarily not supported. Following the cursor using 'before' returns incorrect results and does not work properly on Twitch's back end."), info.settings);

                        return response;
                    }

                    // This error will never be triggered, but keep it just for when/if 'before' works properly.
                    parameters.after = parameters.after.NullIfInvalid();
                    parameters.before = parameters.before.NullIfInvalid();
                    if (parameters.after.IsValid() && parameters.before.IsValid())
                    {
                        response.SetInputError(new ArgumentException("Only one pagination direction can be specified. Provide either 'after' or 'before', not both."), info.settings);

                        return response;
                    }

                    if (parameters.before.IsValid())
                    {
                        direction = "before";
                    }

                    parameters.first = parameters.first.Clamp(1, 100);
                }

                RestRequest request = GetBaseRequest("games/top", Method.GET, info);
                request.AddParameters(parameters);

                // TODO: /games/top - Sanitize the list based on the game ID and return a distinct list.
                RestResponse<DataPage<Game>> _response = await client.TraceExecuteAsync<Game, DataPage<Game>>(request, direction, HandleResponse);
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
                        response.SetInputError(new ParameterCountException("A maximum of 100 total community ID's can be provided at one time.", nameof(parameters.community_ids), 100, parameters.community_ids.Count), info.settings);

                        return response;
                    }

                    parameters.game_ids = parameters.game_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.game_ids.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total game ID's can be provided at one time.", nameof(parameters.game_ids), 100, parameters.game_ids.Count), info.settings);

                        return response;
                    }

                    parameters.user_ids = parameters.user_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.user_ids.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total user ID's can be provided at one time.", nameof(parameters.user_ids), 100, parameters.user_ids.Count), info.settings);

                        return response;
                    }

                    parameters.user_logins = parameters.user_logins.RemoveInvalidAndDuplicateValues();
                    if (parameters.user_logins.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total user logins can be provided at one time.", nameof(parameters.user_logins), 100, parameters.user_logins.Count), info.settings);

                        return response;
                    }

                    parameters.first = parameters.first.Clamp(1, 100);
                }

                RestRequest request = GetBaseRequest("streams", Method.GET, info);
                request.AddParameters(parameters);

                // TODO: /streams - Sanitize the list based on the stream ID and return a distinct list.
                RestResponse<DataPage<Stream>> _response = await client.ExecuteAsync<DataPage<Stream>>(request, HandleResponse);
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
            /// <exception cref="ArgumentException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="ParameterCountException">
            /// Thrown if more than 100 total community ID's are provided.
            /// Thrown if more than 100 total game ID's are provided.
            /// Thrown if more than 100 total user ID's are provided.
            /// Thrown if more than 100 total user logins are provided.
            /// </exception>
            /// <exception cref="NotSupportedException">Thrown if before is provided.</exception>
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
                    // NOTE: /streams - Temporarily disabling using 'before' while requesting all pages until it works properly.
                    // TODO: /streams - Reimplement 'before' when it works propery.
                    if (parameters.before.IsValid())
                    {
                        response.SetInputError(new NotSupportedException("The pagination direction 'before' is temporarily not supported. Following the cursor using 'before' returns incorrect results and does not work properly on Twitch's back end."), info.settings);

                        return response;
                    }

                    // This error will never be triggered, but keep it just for when/if 'before' works properly.
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
                        response.SetInputError(new ParameterCountException("A maximum of 100 total community ID's can be provided at one time.", nameof(parameters.community_ids), 100, parameters.community_ids.Count), info.settings);

                        return response;
                    }

                    parameters.game_ids = parameters.game_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.game_ids.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total game ID's can be provided at one time.", nameof(parameters.game_ids), 100, parameters.game_ids.Count), info.settings);

                        return response;
                    }

                    parameters.user_ids = parameters.user_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.user_ids.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total user ID's can be provided at one time.", nameof(parameters.user_ids), 100, parameters.user_ids.Count), info.settings);

                        return response;
                    }

                    parameters.user_logins = parameters.user_logins.RemoveInvalidAndDuplicateValues();
                    if (parameters.user_logins.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total user logins can be provided at one time.", nameof(parameters.user_logins), 100, parameters.user_logins.Count), info.settings);

                        return response;
                    }

                    if (parameters.before.IsValid())
                    {
                        direction = "before";
                    }

                    parameters.first = parameters.first.Clamp(1, 100);
                }

                RestRequest request = GetBaseRequest("streams", Method.GET, info);
                request.AddParameters(parameters);

                // TODO: /streams - Sanitize the list based on the stream ID and return a distinct list.
                RestResponse<DataPage<Stream>> _response = await client.TraceExecuteAsync<Stream, DataPage<Stream>>(request, direction, HandleResponse);
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

            #region /streams/metadata

            /// <summary>
            /// Asynchronously gets a single page of streams metadata.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of streams metadata.
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
            public static async Task<IHelixResponse<DataPage<StreamMetadata>>>
            GetStreamsMetadataPageAsync(HelixInfo info, StreamsParameters parameters)
            {
                HelixResponse<DataPage<StreamMetadata>> response = new HelixResponse<DataPage<StreamMetadata>>();
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
                        response.SetInputError(new ParameterCountException("A maximum of 100 total community ID's can be provided at one time.", nameof(parameters.community_ids), 100, parameters.community_ids.Count), info.settings);

                        return response;
                    }

                    parameters.game_ids = parameters.game_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.game_ids.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total game ID's can be provided at one time.", nameof(parameters.game_ids), 100, parameters.game_ids.Count), info.settings);

                        return response;
                    }

                    parameters.user_ids = parameters.user_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.user_ids.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total user ID's can be provided at one time.", nameof(parameters.user_ids), 100, parameters.user_ids.Count), info.settings);

                        return response;
                    }

                    parameters.user_logins = parameters.user_logins.RemoveInvalidAndDuplicateValues();
                    if (parameters.user_logins.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total user logins can be provided at one time.", nameof(parameters.user_logins), 100, parameters.user_logins.Count), info.settings);

                        return response;
                    }

                    parameters.first = parameters.first.Clamp(1, 100);
                }

                RestRequest request = GetBaseRequest("streams/metadata", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<StreamMetadata>> _response = await client.ExecuteAsync<DataPage<StreamMetadata>>(request, HandleResponse);
                response = new HelixResponse<DataPage<StreamMetadata>>(_response);

                return response;
            }

            /// <summary>
            /// Asynchronously gets a complete list of streams metadata.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the complete list of streams metadata.
            /// </returns>
            /// <exception cref="ArgumentException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="ParameterCountException">
            /// Thrown if more than 100 total community ID's are provided.
            /// Thrown if more than 100 total game ID's are provided.
            /// Thrown if more than 100 total user ID's are provided.
            /// Thrown if more than 100 total user logins are provided.
            /// </exception>
            /// <exception cref="NotSupportedException">Thrown if before is provided.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<StreamMetadata>>>
            GetStreamsMetadataAsync(HelixInfo info, StreamsParameters parameters)
            {
                HelixResponse<DataPage<StreamMetadata>> response = new HelixResponse<DataPage<StreamMetadata>>();
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                string direction = "after";
                if (!parameters.IsNull())
                {
                    // NOTE: /streams/metadata - Temporarily disabling using 'before' while requesting all pages until it works properly.
                    // TODO: /streams/metadata - Reimplement 'before' when it works propery.
                    if (parameters.before.IsValid())
                    {
                        response.SetInputError(new NotSupportedException("The pagination direction 'before' is temporarily not supported. Following the cursor using 'before' returns incorrect results and does not work properly on Twitch's back end."), info.settings);

                        return response;
                    }

                    // This error will never be triggered, but keep it just for when/if 'before' works properly.
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
                        response.SetInputError(new ParameterCountException("A maximum of 100 total community ID's can be provided at one time.", nameof(parameters.community_ids), 100, parameters.community_ids.Count), info.settings);

                        return response;
                    }

                    parameters.game_ids = parameters.game_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.game_ids.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total game ID's can be provided at one time.", nameof(parameters.game_ids), 100, parameters.game_ids.Count), info.settings);

                        return response;
                    }

                    parameters.user_ids = parameters.user_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.user_ids.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total user ID's can be provided at one time.", nameof(parameters.user_ids), 100, parameters.user_ids.Count), info.settings);

                        return response;
                    }

                    parameters.user_logins = parameters.user_logins.RemoveInvalidAndDuplicateValues();
                    if (parameters.user_logins.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total user logins can be provided at one time.", nameof(parameters.user_logins), 100, parameters.user_logins.Count), info.settings);

                        return response;
                    }

                    if (parameters.before.IsValid())
                    {
                        direction = "before";
                    }

                    parameters.first = parameters.first.Clamp(1, 100);
                }

                RestRequest request = GetBaseRequest("streams/metadata", Method.GET, info);
                request.AddParameters(parameters);
                
                RestResponse<DataPage<StreamMetadata>> _response = await client.TraceExecuteAsync<StreamMetadata, DataPage<StreamMetadata>>(request, direction, HandleResponse);
                response = new HelixResponse<DataPage<StreamMetadata>>(_response);

                return response;
            }

            #endregion

            // TODO: Implement /streams/tags

            // TODO: Implement /subscriptions

            // TODO: Implement /tags/streams

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
            /// Thrown if no user ID's and user logins are provided, or all elements are null, empty, or contains only whitespace.
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
                        response.SetInputError(new ParameterCountException("A maximum of 100 total user ID's and/or user logins can be provided at one time.", 100, count), info.settings);

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

                RestResponse<Data<User>> _response = await client.ExecuteAsync<Data<User>>(request, HandleResponse);
                response = new HelixResponse<Data<User>>(_response);

                return response;
            }

            /// <summary>
            /// <para>
            /// Asynchronously sets the description of a user. 
            /// The user is implicitly specified by the provided Bearer token.
            /// </para>
            /// <para>Required scope: <see cref="Scopes.UserEdit"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="description">The text to set the user's description to.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains updated user information.
            /// </returns>
            /// <exception cref="ArgumentException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
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
            /// <para>
            /// Asynchronously sets the description of a user.
            /// The user is implicitly specified by the provided Bearer token.
            /// </para>
            /// <para>Required scope: <see cref="Scopes.UserEdit"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains updated user information.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="ArgumentException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
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

                RestResponse<Data<User>> _response = await client.ExecuteAsync<Data<User>>(request, HandleResponse);
                response = new HelixResponse<Data<User>>(_response);

                return response;
            }

            #endregion

            #region /users/extensions

            /// <summary>
            /// <para>
            /// Asynchronously gets a list of extensions a user has active.
            /// The user is implicitly specified by the provided Bearer token.
            /// </para>
            /// <para>Optional scopes: <see cref="Scopes.UserReadBroadcast"/> or <see cref="Scopes.UserEditBroadcast"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">
            /// A set of rest parameters.
            /// If no user ID is specified, the user is implicityly specified from the bearer token.
            /// </param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the list of extensions a user has active.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null and no Bearer token is provided.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.
            /// Thrown if the user ID is null, empty, or contains only whitespace.
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

                if (!parameters.IsNull())
                {
                    // If the user provided parameters, they did it for a reason.
                    if (!parameters.user_id.IsValid())
                    {
                        response.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(parameters.user_id)), info.settings);

                        return response;
                    }
                }
                else if (!info.bearer_token.IsValid())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters), "Parameters cannot be null when no Bearer token is provided."), info.settings);

                    return response;
                }

                RestRequest request = GetBaseRequest("users/extensions", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<ActiveExtensions> _response = await client.ExecuteAsync<ActiveExtensions>(request, HandleResponse);
                response = new HelixResponse<ActiveExtensions>(_response);

                return response;
            }

            /// <summary>
            /// <para>
            /// Asynchronously updates a user's active extensions.
            /// The user is implicitly specified by the provided Bearer token.
            /// </para>
            /// <para>Required scope: <see cref="Scopes.UserEditBroadcast"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">
            /// <para>A set of rest parameters.</para>
            /// <para>
            /// Any extensions specified outside of the supported extension slots for each type are ignored.
            /// The supported extension slots for each type are specified under each <see cref="ActiveExtensionsData"/> member.
            /// </para>
            /// </param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the updated active extensions information.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters, parameters.extensions, or parameters.extensions.data are null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if the Bearer token is null, empty, or contains only whitespace.
            /// Thrown if the extension ID or extension version for any active supported extension slot is null, empty, or contains only whitespace.
            /// Thrown if no supported extension slots are found across all extension types.
            /// </exception>
            /// <exception cref="ArgumentOutOfRangeException">Thrown if either (x, y) coordinate for an active supported component extension slot exceeds the range (0, 0) to (8000, 5000).</exception>
            /// <exception cref="DuplicateExtensionException">Thrown if an extension ID is found in more then one active supported slot across all extension types.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.UserEditBroadcast"/> scope</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<ActiveExtensions>>
            UpdateUserActiveExtensionsAsync(HelixInfo info, UpdateExtensionsParameters parameters)
            {
                info.required_scopes = Scopes.UserEditBroadcast;

                HelixResponse<ActiveExtensions> response = new HelixResponse<ActiveExtensions>();
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
                    response.SetInputError(new ArgumentException("No supported extension slots were provided, or all supported extension slots were null."), info.settings);

                    return response;
                }

                if (!ValidateUniqueExtensionIDs(parameters.extensions.data, response, info.settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("users/extensions", Method.PUT, info);
                request.AddParameters(parameters);

                RestResponse<ActiveExtensions> _response = await client.ExecuteAsync<ActiveExtensions>(request, HandleResponse);
                response = new HelixResponse<ActiveExtensions>(_response);

                return response;
            }

            /// <summary>
            /// Validates that the supported extension slot members are not null, empty, contain only whitespace, and/or are not out of bounds.
            /// </summary>
            /// <param name="extensions">The extensions to be validated.</param>
            /// <param name="type">The extension type.</param>
            /// <param name="response">The Helix response to handle errors.</param>
            /// <param name="settings">The request settings to determine how errors are handled.</param>
            /// <returns>Returns the supported and validated extensions.</returns>
            /// <exception cref="ArgumentException">Thrown if the extension ID or extension version for any active supported extension slot is null, empty, or contains only whitespace.</exception>
            /// <exception cref="ArgumentOutOfRangeException">Thrown if either (x, y) coordinate for an active supported component extension slot exceeds the range (0, 0) to (8000, 5000).</exception>
            private static Dictionary<string, ActiveExtension>
            ValidateExtensionSlots(Dictionary<string, ActiveExtension> extensions, ExtensionType type, HelixResponse<ActiveExtensions> response, HelixRequestSettings settings)
            {
                extensions = GetSupportedSlots(extensions, type);
                if (!extensions.IsValid())
                {
                    return extensions;
                }

                foreach (ActiveExtension extension in extensions.Values)
                {
                    // If a slot is being deactivated, none of the other parameters matter.
                    if (!extension.active)
                    {
                        continue;
                    }

                    // If a slot is already active and is being updated or is being activated for the first time, the ID and version are mandatory.
                    if (!extension.id.IsValid())
                    {
                        response.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(extension.id)), settings);

                        return extensions;
                    }

                    if (!extension.version.IsValid())
                    {
                        response.SetInputError(new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(extension.version)), settings);

                        return extensions;
                    }

                    // Twitch only cares about these when a component is being updated.
                    if (type == ExtensionType.Component)
                    {
                        if (extension.x.HasValue && (extension.x.Value < 0 || extension.x.Value > 8000))
                        {
                            response.SetInputError(new ArgumentOutOfRangeException(nameof(extension.x), extension.x.Value, "The x coordinate must be between 0 and 8000, inclusive."), settings);

                            return extensions;
                        }

                        if (extension.y.HasValue && (extension.y.Value < 0 || extension.y.Value > 5000))
                        {
                            response.SetInputError(new ArgumentOutOfRangeException(nameof(extension.y), extension.y.Value, "The y coordinate must be between 0 and 8000, inclusive."), settings);

                            return extensions;
                        }
                    }
                }

                return extensions;
            }

            /// <summary>
            /// Gets the extension slots that can be updated per type.
            /// </summary>
            /// <param name="extensions">The extensions to be validated.</param>
            /// <param name="type">The extension type.</param>
            /// <returns>Returns the supported extensions that Twitch accepts.</returns>
            private static Dictionary<string, ActiveExtension>
            GetSupportedSlots(Dictionary<string, ActiveExtension> extensions, ExtensionType type)
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
                    keys = new string[] { "1", "2" };
                }

                Dictionary<string, ActiveExtension> result = new Dictionary<string, ActiveExtension>();
                foreach (string key in keys)
                {
                    if (!extensions.TryGetValue(key, out ActiveExtension temp))
                    {
                        continue;
                    }

                    result[key] = temp;
                }

                return result;
            }

            /// <summary>
            /// Validates that each extension ID is unique across all extension types and slots.
            /// </summary>
            /// <param name="data">The extension data to validate.</param>
            /// <param name="response">The Helix response to handle errors.</param>
            /// <param name="settings">The request settings to determine how errors are handled.</param>
            /// <returns>
            /// Returns true if each extension ID is unique across all extension types and slots.
            /// Returns false otherwise.
            /// </returns>
            /// <exception cref="DuplicateExtensionException">Thrown if an extension ID is found in more then one active supported slot across all extension types.</exception>
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

                List<string> ids = new List<string>(extensions.Count);
                foreach (ActiveExtension extension in extensions)
                {
                    if (!extension.active)
                    {
                        continue;
                    }

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
            /// Asynchronously gets a complete list of extensions a user has installed, activated or deactivated.
            /// The user is implicitly specified by the provided Bearer token.
            /// </para>
            /// <para>Required Scope: <see cref="Scopes.UserReadBroadcast"/></para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the complete list of extensions the user has instlled, activated or deactivated..
            /// </returns>
            /// <exception cref="ArgumentException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.UserReadBroadcast"/> scope</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<Extension>>>
            GetUserExtensionsAsync(HelixInfo info)
            {
                info.required_scopes = Scopes.UserReadBroadcast;

                HelixResponse<Data<Extension>> response = new HelixResponse<Data<Extension>>();
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("users/extensions/list", Method.GET, info);

                RestResponse<Data<Extension>> _response = await client.ExecuteAsync<Data<Extension>>(request, HandleResponse);
                response = new HelixResponse<Data<Extension>>(_response);

                return response;
            }

            #endregion

            #region /users/follows        

            /// <summary>
            /// Asynchronously gets a single page of a user's following list.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">
            /// A set of rest parameters.
            /// If provided, to_id is ignored.
            /// </param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of a user's following list.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.
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

                parameters.to_id = null;

                response = await GetUserRelationshipPageAsync(info, parameters) as HelixResponse<FollowsDataPage<Follow>>;

                return response;
            }

            /// <summary>
            /// Asynchronously gets a user's complete following list.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">
            /// A set of rest parameters.
            /// If provided, to_id is ignored.
            /// </param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the user's complete following list.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.
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

                parameters.to_id = null;

                response = await GetUserRelationshipAsync(info, parameters) as HelixResponse<FollowsDataPage<Follow>>;

                return response;
            }

            /// <summary>
            /// Asynchronously gets a single page of a user's followers list.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">
            /// A set of rest parameters.
            /// If provided, from_id is ignored.
            /// </param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of a user's followers list.
            /// </returns>        
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.
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

                parameters.from_id = null;

                response = await GetUserRelationshipPageAsync(info, parameters) as HelixResponse<FollowsDataPage<Follow>>;

                return response;
            }

            /// <summary>
            /// Asynchronously gets a user's complete follower list.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">
            /// A set of rest parameters.
            /// If provided, from_id is ignored.
            /// </param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the user's complete follower list.
            /// </returns>  
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.
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

                parameters.from_id = null;

                response = await GetUserRelationshipAsync(info, parameters) as HelixResponse<FollowsDataPage<Follow>>;

                return response;
            }

            /// <summary>
            /// Asynchronously checks to see if the from_id user is following the to_id user.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="from_id">The ID of the following user.</param>
            /// <param name="to_id">The ID of the followed user.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> is set true if from_id is following to_id, otherwise false.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.
            /// Thrown if either from_id and to_id are null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<bool>>
            IsUserFollowingAsync(HelixInfo info, string from_id, string to_id)
            {
                HelixResponse<bool> response = new HelixResponse<bool>();
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

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
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">
            /// <para>A set of rest parameters.</para>
            /// <para>At minimum, from_id or to_id must be provided.</para>
            /// </param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the user relationship page, or a single page of the following/follower list of one user.
            /// </returns> 
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.
            /// Thrown if both from_id and to_id are null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserRelationshipPageAsync(HelixInfo info, FollowsParameters parameters)
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

                if (!parameters.to_id.IsValid() && !parameters.from_id.IsValid())
                {
                    response.SetInputError(new ArgumentException("At minimum, from_id or to_id must be provided and cannot be null, empty, or contain only whitespace."), info.settings);

                    return response;
                }

                parameters.first = parameters.first.Clamp(1, 100);

                RestRequest request = GetBaseRequest("users/follows", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<FollowsDataPage<Follow>> _response = await client.ExecuteAsync<FollowsDataPage<Follow>>(request, HandleResponse);
                response = new HelixResponse<FollowsDataPage<Follow>>(_response);

                return response;
            }

            /// <summary>
            /// Asynchronously gets the relationship between two users, or a user's complete following/follower list.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">
            /// <para>A set of rest parameters.</para>
            /// <para>At minimum, from_id or to_id must be provided.</para>
            /// </param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the user relationship, or the complete following/follower list of one user.
            /// </returns>        
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.
            /// Thrown if both from_id and to_id are null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserRelationshipAsync(HelixInfo info, FollowsParameters parameters)
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

                if (!parameters.to_id.IsValid() && !parameters.from_id.IsValid())
                {
                    response.SetInputError(new ArgumentException("At minimum, from_id or to_id must be provided and cannot be null, empty, or contain only whitespace."), info.settings);

                    return response;
                }

                parameters.first = parameters.first.Clamp(1, 100);

                RestRequest request = GetBaseRequest("users/follows", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<FollowsDataPage<Follow>> _response = await client.TraceExecuteAsync<Follow, FollowsDataPage<Follow>>(request, HandleResponse);
                response = new HelixResponse<FollowsDataPage<Follow>>(_response);

                return response;
            }

            #endregion

            #region /videos

            /// <summary>
            /// Asynchronously gets specific videos, or a single page of videos.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> containts the specific videos, or the single page of videos.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.
            /// Thrown if after and before are provided.
            /// Thrown if no video ID's, game ID, or user ID are provided.
            /// Thrown if any multiple combination of video ID's, game ID, or user ID is provided.
            /// Thrown if all video ID's are are null, empty, or contains only whitespace, if Provided.
            /// Thrown if the user ID is null, empty, or contains only whitespace, if Provided.
            /// Thrown if the game ID is null, empty, or contains only whitespace, if Provided.
            /// </exception>
            /// <exception cref="ParameterCountException">Thrown if more than 100 total video ID's are provided.</exception>
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

                parameters.after = parameters.after.NullIfInvalid();
                parameters.before = parameters.before.NullIfInvalid();
                if (parameters.after.IsValid() && parameters.before.IsValid())
                {
                    response.SetInputError(new ArgumentException("Only one pagination direction can be specified. Only use either 'after' or 'before'."), info.settings);

                    return response;
                }

                if (parameters.ids.IsValid())
                {
                    // If video ID's are provided, assume it's intentional and check for these errors up front for better error messages.
                    parameters.ids = parameters.ids.RemoveInvalidAndDuplicateValues();

                    if(parameters.ids.Count == 0)
                    {
                        response.SetInputError(new ArgumentException("All provided video ID's were null, empty, or contained only whitespace.", nameof(parameters.ids)), info.settings);

                        return response;
                    }
                    else if (parameters.ids.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total video ID's can be provided at one time.", nameof(parameters.ids), 100, parameters.ids.Count), info.settings);

                        return response;
                    }

                    parameters.after    = null;
                    parameters.before   = null;
                    parameters.first    = null;
                    parameters.language = null;
                    parameters.period   = null;
                    parameters.type     = null;
                }

                parameters.game_id = parameters.game_id.NullIfInvalid();
                parameters.user_id = parameters.user_id.NullIfInvalid();
                if (!parameters.ids.IsValid() && !parameters.user_id.IsValid() && !parameters.game_id.IsValid())
                {
                    response.SetInputError(new ArgumentException("At least one or more video ID, one user ID, or one game ID must be provided."), info.settings);

                    return response;
                }

                if ((parameters.ids.IsValid() && (parameters.user_id.IsValid() || parameters.game_id.IsValid())) ||
                   (parameters.user_id.IsValid() && parameters.game_id.IsValid()))
                {
                    response.SetInputError(new ArgumentException("Only one or more video ID's, one user ID, or one game ID can be provided."), info.settings);

                    return response;
                }

                parameters.first = parameters.first.Clamp(1, 100);

                RestRequest request = GetBaseRequest("videos", Method.GET, info);
                request.AddParameters(parameters);

                // TODO: /videos - Resort the videos based on sort. Sometimes videos can be out of order.
                RestResponse<DataPage<Video>> _response = await client.ExecuteAsync<DataPage<Video>>(request, HandleResponse);
                response = new HelixResponse<DataPage<Video>>(_response);

                return response;
            }

            /// <summary>
            /// Asynchronously gets specific videos, or a complete list of videos.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> containts the specific videos, or the complete list of videos.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="ArgumentException">
            /// Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.
            /// Thrown if no video ID's, game ID, or user ID are provided.
            /// Thrown if any multiple combination of video ID's, game ID, or user ID is provided.
            /// Thrown if all video ID's are are null, empty, or contains only whitespace, if Provided.
            /// Thrown if the user ID is null, empty, or contains only whitespace, if Provided.
            /// Thrown if the game ID is null, empty, or contains only whitespace, if Provided.
            /// </exception>
            /// <exception cref="NotSupportedException">Thrown if before is provided.</exception>
            /// <exception cref="ParameterCountException">Thrown if more than 100 total video ID's are provided.</exception>
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

                // NOTE: /videos - Temporarily disabling using 'before' while requesting all pages until it works properly.
                // TODO: /videos - Reimplement 'before' when it works propery.
                if (parameters.before.IsValid())
                {
                    response.SetInputError(new NotSupportedException("The pagination direction 'before' is temporarily not supported. Following the cursor using 'before' returns incorrect results and does not work properly on Twitch's back end."), info.settings);

                    return response;
                }

                if (parameters.ids.IsValid())
                {
                    // If video ID's are provided, assume it's intentional and check for these errors up front for better error messages.
                    parameters.ids = parameters.ids.RemoveInvalidAndDuplicateValues();

                    if (parameters.ids.Count == 0)
                    {
                        response.SetInputError(new ArgumentException("All provided video ID's were null, empty, or contained only whitespace.", nameof(parameters.ids)), info.settings);

                        return response;
                    }
                    else if (parameters.ids.Count > 100)
                    {
                        response.SetInputError(new ParameterCountException("A maximum of 100 total video ID's can be provided at one time.", nameof(parameters.ids), 100, parameters.ids.Count), info.settings);

                        return response;
                    }

                    parameters.after    = null;
                    parameters.before   = null;
                    parameters.first    = null;
                    parameters.language = null;
                    parameters.period   = null;
                    parameters.type     = null;
                }

                parameters.game_id = parameters.game_id.NullIfInvalid();
                parameters.user_id = parameters.user_id.NullIfInvalid();
                if (!parameters.ids.IsValid() && !parameters.user_id.IsValid() && !parameters.game_id.IsValid())
                {
                    response.SetInputError(new ArgumentException("At least one or more video ID, one user ID, or one game ID must be provided."), info.settings);

                    return response;
                }

                if ((parameters.ids.IsValid() && (parameters.user_id.IsValid() || parameters.game_id.IsValid())) ||
                   (parameters.user_id.IsValid() && parameters.game_id.IsValid()))
                {
                    response.SetInputError(new ArgumentException("Only one or more video ID's, one user ID, or one game ID can be provided."), info.settings);

                    return response;
                }

                parameters.first = parameters.first.Clamp(1, 100);

                RestRequest request = GetBaseRequest("videos", Method.GET, info);
                request.AddParameters(parameters);

                string direction = parameters.before.IsValid() ? "before" : "after";

                // TODO: /videos - Resort the videos based on sort. Sometimes videos can be out of order.
                RestResponse<DataPage<Video>> _response = await client.TraceExecuteAsync<Video, DataPage<Video>>(request, direction, HandleResponse);
                response = new HelixResponse<DataPage<Video>>(_response);

                return response;
            }

            #endregion

            // TODO: Implement /webhook/hub

            // TODO: Implement /webhook/subscriptions
        }
    }
}