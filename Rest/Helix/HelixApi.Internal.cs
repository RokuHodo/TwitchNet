// standard namespaces
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Utilities;

namespace
TwitchNet.Rest.Helix
{
    public static partial class
    HelixApi
    {
        internal static class
        Internal
        {
            internal static readonly RestClient client = GetHelixClient();

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
                if (app_access_required)
                {
                    if (!bearer_valid)
                    {
                        response.SetInputError(new HeaderParameterException("An App Access Token must be provided to authenticate the request."), info.settings);

                        return false;
                    }

                    if (!client_id_valid)
                    {
                        response.SetInputError(new HeaderParameterException("A Client ID must be provided when an App Access Token is required."), info.settings);

                        return false;
                    }
                }

                // A set of scopes is required for permission.
                if (info.required_scopes != 0)
                {
                    // Bearer token was not provided.
                    if (!bearer_valid)
                    {
                        Scopes[] missing_scopes = EnumUtil.GetFlagValues<Scopes>(info.required_scopes);
                        AvailableScopesException inner_exception = new AvailableScopesException("One or more scopes are required for authentication.", missing_scopes);

                        response.SetInputError(new HeaderParameterException("A Bearer token must be provided to authenticate the request. See the inner exception for the list of required scopes.", nameof(info.bearer_token), inner_exception), info.settings);

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
                    response.SetInputError(new HeaderParameterException("A Bearer token or Client ID must be provided to authenticate the request."), info.settings);

                    return false;
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

            #region /analytics/extensions

            /// <summary>
            /// <para>Asynchronously gets a specific extension analytic report, or a single page of extension analytic reports.</para>
            /// <para>Required Scope: <see cref="Scopes.AnalyticsReadExtensions"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific extension analytic report, or the single page of extension analytic reports.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if started_at or ended at is provided without the other.</exception>
            /// <exception cref="QueryParameterValueException">
            /// Thrown if started_at or ended_at is later than <see cref="DateTime.UtcNow"/>.
            /// Thrown if started_at is later than ended_at.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.AnalyticsReadExtensions"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
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
                        response.SetInputError(new QueryParameterException(nameof(parameters.ended_at), "ended_at must be provided if started_at is provided."), info.settings);

                        return response;
                    }
                    else if (!parameters.started_at.HasValue && parameters.ended_at.HasValue)
                    {
                        response.SetInputError(new QueryParameterException(nameof(parameters.started_at), "started_at must be provided if started_at is provided."), info.settings);

                        return response;
                    }
                    else if (parameters.started_at.HasValue && parameters.ended_at.HasValue)
                    {
                        parameters.started_at = parameters.started_at.Value.ToUniversalTime();
                        parameters.ended_at = parameters.ended_at.Value.ToUniversalTime();

                        if (parameters.started_at > DateTime.UtcNow)
                        {
                            response.SetInputError(new QueryParameterValueException(nameof(parameters.started_at), parameters.started_at.Value, "The started_at date cannot be greater than the current date."), info.settings);

                            return response;
                        }

                        if (parameters.ended_at > DateTime.UtcNow)
                        {
                            response.SetInputError(new QueryParameterValueException(nameof(parameters.ended_at), parameters.ended_at.Value, "The ended_at date cannot be greater than the current date."), info.settings);

                            return response;
                        }

                        if (parameters.started_at.Value > parameters.ended_at.Value)
                        {
                            response.SetInputError(new QueryParameterValueException(nameof(parameters.started_at), parameters.started_at.Value, "started_at cannot be later than the ended_at."), info.settings);

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

            /// <summary>
            /// <para>Asynchronously gets a specific extension analytic report, or a complete list of extension analytic reports.</para>
            /// <para>Required Scope: <see cref="Scopes.AnalyticsReadExtensions"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific extension analytic report, or the complete list of extension analytic reports.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if started_at or ended at is provided without the other.</exception>
            /// <exception cref="QueryParameterValueException">
            /// Thrown if started_at or ended_at is later than <see cref="DateTime.UtcNow"/>.
            /// Thrown if started_at is later than ended_at.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.AnalyticsReadExtensions"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
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
                        response.SetInputError(new QueryParameterException(nameof(parameters.ended_at), "ended_at must be provided if started_at is provided."), info.settings);

                        return response;
                    }
                    else if (!parameters.started_at.HasValue && parameters.ended_at.HasValue)
                    {
                        response.SetInputError(new QueryParameterException(nameof(parameters.started_at), "started_at must be provided if started_at is provided."), info.settings);

                        return response;
                    }
                    else if (parameters.started_at.HasValue && parameters.ended_at.HasValue)
                    {
                        parameters.started_at = parameters.started_at.Value.ToUniversalTime();
                        parameters.ended_at = parameters.ended_at.Value.ToUniversalTime();

                        if (parameters.started_at > DateTime.UtcNow)
                        {
                            response.SetInputError(new QueryParameterValueException(nameof(parameters.started_at), parameters.started_at.Value, "The started_at date cannot be greater than the current date."), info.settings);

                            return response;
                        }

                        if (parameters.ended_at > DateTime.UtcNow)
                        {
                            response.SetInputError(new QueryParameterValueException(nameof(parameters.ended_at), parameters.ended_at.Value, "The ended_at date cannot be greater than the current date."), info.settings);

                            return response;
                        }

                        if (parameters.started_at.Value > parameters.ended_at.Value)
                        {
                            response.SetInputError(new QueryParameterValueException(nameof(parameters.started_at), parameters.started_at.Value, "started_at cannot be later than the ended_at."), info.settings);

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

                RestResponse<DataPage<ExtensionAnalytics>> _response = await client.TraceExecuteAsync<ExtensionAnalytics, DataPage<ExtensionAnalytics>>(request, HandleResponse);
                response = new HelixResponse<DataPage<ExtensionAnalytics>>(_response);

                return response;
            }

            #endregion

            #region /analytics/games

            /// <summary>
            /// <para>Asynchronously gets a specific game analytic report, or a single page of game analytic reports.</para>
            /// <para>Required Scope: <see cref="Scopes.AnalyticsReadGames"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific game analytic report, or the single page of game analytic reports.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if started_at or ended at is provided without the other.</exception>
            /// <exception cref="QueryParameterValueException">
            /// Thrown if started_at or ended_at is later than <see cref="DateTime.UtcNow"/>.
            /// Thrown if started_at is later than ended_at.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.AnalyticsReadGames"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<GameAnalytics>>>
            GetGameAnalyticsPageAsync(HelixInfo info, GameAnalyticsParameters parameters)
            {
                info.required_scopes = Scopes.AnalyticsReadGames;

                HelixResponse<DataPage<GameAnalytics>> response = new HelixResponse<DataPage<GameAnalytics>>();
                if (!ValidateAuthorizationParameters(info, response, true))
                {
                    return response;
                }

                if (!parameters.IsNull())
                {
                    parameters.after = parameters.after.NullIfInvalid();

                    if (parameters.started_at.HasValue && !parameters.ended_at.HasValue)
                    {
                        response.SetInputError(new QueryParameterException(nameof(parameters.ended_at), "ended_at must be provided if started_at is provided."), info.settings);

                        return response;
                    }
                    else if (!parameters.started_at.HasValue && parameters.ended_at.HasValue)
                    {
                        response.SetInputError(new QueryParameterException(nameof(parameters.started_at), "started_at must be provided if started_at is provided."), info.settings);

                        return response;
                    }
                    else if (parameters.started_at.HasValue && parameters.ended_at.HasValue)
                    {
                        parameters.started_at = parameters.started_at.Value.ToUniversalTime();
                        parameters.ended_at = parameters.ended_at.Value.ToUniversalTime();

                        if (parameters.started_at > DateTime.UtcNow)
                        {
                            response.SetInputError(new QueryParameterValueException(nameof(parameters.started_at), parameters.started_at.Value, "The started_at date cannot be greater than the current date."), info.settings);

                            return response;
                        }

                        if (parameters.ended_at > DateTime.UtcNow)
                        {
                            response.SetInputError(new QueryParameterValueException(nameof(parameters.ended_at), parameters.ended_at.Value, "The ended_at date cannot be greater than the current date."), info.settings);

                            return response;
                        }

                        if (parameters.started_at.Value > parameters.ended_at.Value)
                        {
                            response.SetInputError(new QueryParameterValueException(nameof(parameters.started_at), parameters.started_at.Value, "started_at cannot be later than the ended_at."), info.settings);

                            return response;
                        }
                    }

                    if (parameters.game_id.IsValid())
                    {
                        // Just to make sure if something was actually set
                        parameters.after = null;
                    }
                    else
                    {
                        parameters.game_id = null;
                    }

                    parameters.first = parameters.first.Clamp(1, 100);
                }

                RestRequest request = GetBaseRequest("analytics/games", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<GameAnalytics>> _response = await client.ExecuteAsync<DataPage<GameAnalytics>>(request, HandleResponse);
                response = new HelixResponse<DataPage<GameAnalytics>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously gets a specific game analytic report, or a complete list of game analytic reports.</para>
            /// <para>Required Scope: <see cref="Scopes.AnalyticsReadGames"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific game analytic report, or the complete list of game analytic reports.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if started_at or ended at is provided without the other.</exception>
            /// <exception cref="QueryParameterValueException">
            /// Thrown if started_at or ended_at is later than <see cref="DateTime.UtcNow"/>.
            /// Thrown if started_at is later than ended_at.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.AnalyticsReadGames"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<GameAnalytics>>>
            GetGameAnalyticsAsync(HelixInfo info, GameAnalyticsParameters parameters)
            {
                info.required_scopes = Scopes.AnalyticsReadGames;

                HelixResponse<DataPage<GameAnalytics>> response = new HelixResponse<DataPage<GameAnalytics>>();
                if (!ValidateAuthorizationParameters(info, response, true))
                {
                    return response;
                }

                if (!parameters.IsNull())
                {
                    parameters.after = parameters.after.NullIfInvalid();

                    if (parameters.started_at.HasValue && !parameters.ended_at.HasValue)
                    {
                        response.SetInputError(new QueryParameterException(nameof(parameters.ended_at), "ended_at must be provided if started_at is provided."), info.settings);

                        return response;
                    }
                    else if (!parameters.started_at.HasValue && parameters.ended_at.HasValue)
                    {
                        response.SetInputError(new QueryParameterException(nameof(parameters.started_at), "started_at must be provided if started_at is provided."), info.settings);

                        return response;
                    }
                    else if (parameters.started_at.HasValue && parameters.ended_at.HasValue)
                    {
                        parameters.started_at = parameters.started_at.Value.ToUniversalTime();
                        parameters.ended_at = parameters.ended_at.Value.ToUniversalTime();

                        if (parameters.started_at > DateTime.UtcNow)
                        {
                            response.SetInputError(new QueryParameterValueException(nameof(parameters.started_at), parameters.started_at.Value, "The started_at date cannot be greater than the current date."), info.settings);

                            return response;
                        }

                        if (parameters.ended_at > DateTime.UtcNow)
                        {
                            response.SetInputError(new QueryParameterValueException(nameof(parameters.ended_at), parameters.ended_at.Value, "The ended_at date cannot be greater than the current date."), info.settings);

                            return response;
                        }

                        if (parameters.started_at.Value > parameters.ended_at.Value)
                        {
                            response.SetInputError(new QueryParameterValueException(nameof(parameters.started_at), parameters.started_at.Value, "started_at cannot be later than the ended_at."), info.settings);

                            return response;
                        }
                    }

                    if (parameters.game_id.IsValid())
                    {
                        // Just to make sure if something was actually set
                        parameters.after = null;
                    }
                    else
                    {
                        parameters.game_id = null;
                    }

                    parameters.first = parameters.first.Clamp(1, 100);
                }

                RestRequest request = GetBaseRequest("analytics/games", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<GameAnalytics>> _response = await client.TraceExecuteAsync<GameAnalytics, DataPage<GameAnalytics>>(request, HandleResponse);
                response = new HelixResponse<DataPage<GameAnalytics>>(_response);

                return response;
            }

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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterValueException">Thrown if started_at is later than <see cref="DateTime.Now"/>.</exception>
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

                    if (parameters.period == BitsLeaderboardPeriod.All)
                    {
                        parameters.started_at = null;
                    }

                    if (parameters.started_at > DateTime.Now)
                    {
                        response.SetInputError(new QueryParameterValueException(nameof(parameters.started_at), parameters.started_at.Value, "started_at cannot be later than the current date."), info.settings);

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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token not provided.</exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID is not provided.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.ClipsEdit"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<CreatedClip>>>
            CreateClipAsync(HelixInfo info, CreateClipParameters parameters)
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
                    response.SetInputError(new QueryParameterException(nameof(parameters.broadcaster_id), "Parameter is required and the value cannot be null, empty, or contain only whitespace."), info.settings);

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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if both after and before cursors are provided.
            /// Thrown if no clip ID's, broadcaster ID, or game ID are provided.
            /// Thrown if any multiple combination of clip ID's, broadcaster ID, or game ID is provided.            
            /// Thrown if the broadcaster ID is null, empty, or contains only whitespace, if Provided.
            /// Thrown if the game ID is null, empty, or contains only whitespace, if Provided.            
            /// </exception>
            /// <exception cref="QueryParameterValueException">
            /// Thrown if started_at or ended_at is later than the current date.
            /// Thrown if started_at is later than ended_at.
            /// </exception>
            /// <exception cref="QueryParameterCountException">
            /// Thrown if all clip ID's are are null, empty, or contains only whitespace.
            /// Thrown if more than 100 total clip ID's are provided.
            /// </exception>
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
                    response.SetInputError(new QueryParameterException("Only one pagination direction can be specified. Only use either 'after' or 'before'."), info.settings);

                    return response;
                }

                if (parameters.ids.IsValid())
                {
                    // If clip ID's are provided, assume it's intentional and check for these errors up front for better error messages.
                    parameters.ids = parameters.ids.RemoveInvalidAndDuplicateValues();

                    if (parameters.ids.Count == 0)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.ids), 100, parameters.ids.Count, "All provided clip ID's were null, empty, or contained only whitespace."), info.settings);

                        return response;
                    }
                    else if (parameters.ids.Count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.ids), 100, parameters.ids.Count, "A maximum of 100 total clip ID's can be provided at one time."), info.settings);

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
                else if (parameters.ended_at.HasValue && parameters.ended_at.Value > DateTime.Now)
                {
                    response.SetInputError(new QueryParameterValueException(nameof(parameters.ended_at), parameters.ended_at.Value, "ended_at cannot be later than the current date."), info.settings);

                    return response;
                }
                else if (parameters.started_at.HasValue && parameters.started_at.Value > DateTime.Now)
                {
                    response.SetInputError(new QueryParameterValueException(nameof(parameters.started_at), parameters.started_at.Value, "started_at cannot be later than the current date."), info.settings);

                    return response;
                }
                else if (parameters.started_at.HasValue && parameters.ended_at.HasValue && parameters.started_at.Value > parameters.ended_at.Value)
                {
                    response.SetInputError(new QueryParameterValueException(nameof(parameters.started_at), parameters.started_at.Value, "started_at cannot be later than ended_at."), info.settings);

                    return response;
                }

                parameters.broadcaster_id = parameters.broadcaster_id.NullIfInvalid();
                parameters.game_id = parameters.game_id.NullIfInvalid();
                if (!parameters.ids.IsValid() && !parameters.broadcaster_id.IsValid() && !parameters.game_id.IsValid())
                {
                    response.SetInputError(new QueryParameterException("At least one or more clip ID, one broadcaster ID, or one game ID must be provided."), info.settings);

                    return response;
                }

                if ((parameters.ids.IsValid() && (parameters.broadcaster_id.IsValid() || parameters.game_id.IsValid())) ||
                   (parameters.broadcaster_id.IsValid() && parameters.game_id.IsValid()))
                {
                    response.SetInputError(new QueryParameterException("Only one or more clip ID's, one broadcaster ID, or one game ID can be provided."), info.settings);

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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if both after and before cursors are provided.
            /// Thrown if no clip ID's, broadcaster ID, or game ID are provided.
            /// Thrown if any multiple combination of clip ID's, broadcaster ID, or game ID is provided.            
            /// Thrown if the broadcaster ID is null, empty, or contains only whitespace, if Provided.
            /// Thrown if the game ID is null, empty, or contains only whitespace, if Provided.            
            /// </exception>
            /// <exception cref="QueryParameterValueException">
            /// Thrown if started_at or ended_at is later than the current date.
            /// Thrown if started_at is later than ended_at.
            /// </exception>
            /// <exception cref="QueryParameterCountException">
            /// Thrown if all clip ID's are are null, empty, or contains only whitespace.
            /// Thrown if more than 100 total clip ID's are provided.
            /// </exception>            
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

                // NOTE: /clips - GetClipsAsync(...) - Temporarily disabling using 'before' while requesting all pages until it works properly.
                // TODO: /clips - GetClipsAsync(...) - Reimplement 'before' when it works propery.
                if (parameters.before.IsValid())
                {
                    response.SetInputError(new NotSupportedException("The pagination direction 'before' is temporarily not supported. Following the cursor using 'before' returns incorrect results and does not work properly on Twitch's back end."), info.settings);

                    return response;
                }

                parameters.after = parameters.after.NullIfInvalid();
                parameters.before = parameters.before.NullIfInvalid();
                if (parameters.after.IsValid() && parameters.before.IsValid())
                {
                    response.SetInputError(new QueryParameterException("Only one pagination direction can be specified. Only use either 'after' or 'before'."), info.settings);

                    return response;
                }

                if (parameters.ids.IsValid())
                {
                    // If clip ID's are provided, assume it's intentional and check for these errors up front for better error messages.
                    parameters.ids = parameters.ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.ids.Count == 0)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.ids), 100, parameters.ids.Count, "All provided clip ID's were null, empty, or contained only whitespace."), info.settings);

                        return response;
                    }
                    else if (parameters.ids.Count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.ids), 100, parameters.ids.Count, "A maximum of 100 total clip ID's can be provided at one time."), info.settings);

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
                else if (parameters.ended_at.HasValue && parameters.ended_at.Value > DateTime.Now)
                {
                    response.SetInputError(new QueryParameterValueException(nameof(parameters.ended_at), parameters.ended_at.Value, "ended_at cannot be later than the current date."), info.settings);

                    return response;
                }
                else if (parameters.started_at.HasValue && parameters.started_at.Value > DateTime.Now)
                {
                    response.SetInputError(new QueryParameterValueException(nameof(parameters.started_at), parameters.started_at.Value, "started_at cannot be later than the current date."), info.settings);

                    return response;
                }
                else if (parameters.started_at.HasValue && parameters.ended_at.HasValue && parameters.started_at.Value > parameters.ended_at.Value)
                {
                    response.SetInputError(new QueryParameterValueException(nameof(parameters.started_at), parameters.started_at.Value, "started_at cannot be later than ended_at."), info.settings);

                    return response;
                }

                parameters.broadcaster_id = parameters.broadcaster_id.NullIfInvalid();
                parameters.game_id = parameters.game_id.NullIfInvalid();
                if (!parameters.ids.IsValid() && !parameters.broadcaster_id.IsValid() && !parameters.game_id.IsValid())
                {
                    response.SetInputError(new QueryParameterException("At least one or more clip ID, one broadcaster ID, or one game ID must be provided."), info.settings);

                    return response;
                }

                if ((parameters.ids.IsValid() && (parameters.broadcaster_id.IsValid() || parameters.game_id.IsValid())) ||
                   (parameters.broadcaster_id.IsValid() && parameters.game_id.IsValid()))
                {
                    response.SetInputError(new QueryParameterException("Only one or more clip ID's, one broadcaster ID, or one game ID can be provided."), info.settings);

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

            #region /entitlements/codes

            /// <summary>
            /// <para>Asynchronously gets the status of one or more entitlement codes for the authenticated user.</para>
            /// <para>Required Authorization: App Access Token.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the statuses of the specified codes.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the App Access Token or Client ID is null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if the user ID is not provided.</exception>
            /// <exception cref="QueryParameterValueException">Thrown if any code does not match the regex: ^[a-zA-Z0-9]{5}-?[a-zA-Z0-9]{5}-?[a-zA-Z0-9]{5}$</exception>
            /// <exception cref="QueryParameterCountException">            
            /// Thrown if all codes are are null, empty, or contains only whitespace.
            /// Thrown if no codes or more than 20 codes ID's are provided.            
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<CodeStatus>>>
            GetEntitlementCodeStatusAsync(HelixInfo info, EntitlementsCodeParameters parameters)
            {
                HelixResponse<Data<CodeStatus>> response = new HelixResponse<Data<CodeStatus>>();
                if (!ValidateAuthorizationParameters(info, response, true))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!parameters.user_id.IsValid())
                {
                    response.SetInputError(new QueryParameterException(nameof(parameters.user_id), "Parameter is required and the value cannot be null, empty, or contain only whitespace."), info.settings);

                    return response;
                }

                if (!parameters.codes.IsValid())
                {
                    response.SetInputError(new QueryParameterCountException(nameof(parameters.user_id), 20, parameters.codes.Count, "At leats one code must be provided."), info.settings);

                    return response;
                }                

                parameters.codes = parameters.codes.RemoveInvalidAndDuplicateValues();
                if (parameters.codes.Count == 0)
                {
                    response.SetInputError(new QueryParameterCountException(nameof(parameters.codes), 20, parameters.codes.Count, "All provided codes were null, empty, or contained only whitespace."), info.settings);

                    return response;
                }
                else if (parameters.codes.Count > 20)
                {
                    response.SetInputError(new QueryParameterCountException(nameof(parameters.codes), 20, parameters.codes.Count, "A maximum of 20 total codes can be provided at one time."), info.settings);

                    return response;
                }

                Regex regex = new Regex(RegexPatternUtil.ENTITLEMENT_CODE);
                foreach (string code in parameters.codes)
                {
                    if (regex.IsMatch(code))
                    {
                        continue;
                    }

                    FormatException inner_exception = new FormatException("Value must match the regular expression: " + RegexPatternUtil.ENTITLEMENT_CODE);

                    string message = "The status code must be a 15 character alpha-numeric string, with optional dashes after every 5th character." + Environment.NewLine +
                                     "Example: ABCDE-12345-F6G7H";
                    response.SetInputError(new QueryParameterValueException(nameof(parameters.codes), code, message, inner_exception), info.settings);

                    return response;
                }

                RestRequest request = GetBaseRequest("entitlements/codes", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<Data<CodeStatus>> _response = await client.ExecuteAsync<Data<CodeStatus>>(request, HandleResponse);
                response = new HelixResponse<Data<CodeStatus>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously redeems one or more entitlement codes to the authenticated user.</para>
            /// <para>Required Authorization: App Access Token.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the statuses of the specified codes.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the App Access Token or Client ID is null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if the user ID is not provided.</exception>
            /// <exception cref="QueryParameterValueException">Thrown if any code does not match the regex: ^[a-zA-Z0-9]{5}-?[a-zA-Z0-9]{5}-?[a-zA-Z0-9]{5}$</exception>
            /// <exception cref="QueryParameterCountException">            
            /// Thrown if all codes are are null, empty, or contains only whitespace.
            /// Thrown if no codes or more than 20 codes ID's are provided.            
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<CodeStatus>>>
            RedeemEntitlementCodeStatusAsync(HelixInfo info, EntitlementsCodeParameters parameters)
            {
                HelixResponse<Data<CodeStatus>> response = new HelixResponse<Data<CodeStatus>>();
                if (!ValidateAuthorizationParameters(info, response, true))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!parameters.user_id.IsValid())
                {
                    response.SetInputError(new QueryParameterException(nameof(parameters.user_id), "Parameter is required and the value cannot be null, empty, or contain only whitespace."), info.settings);

                    return response;
                }

                if (!parameters.codes.IsValid())
                {
                    response.SetInputError(new QueryParameterCountException(nameof(parameters.user_id), 20, parameters.codes.Count, "At leats one code must be provided."), info.settings);

                    return response;
                }

                parameters.codes = parameters.codes.RemoveInvalidAndDuplicateValues();
                if (parameters.codes.Count == 0)
                {
                    response.SetInputError(new QueryParameterCountException(nameof(parameters.codes), 20, parameters.codes.Count, "All provided codes were null, empty, or contained only whitespace."), info.settings);

                    return response;
                }
                else if (parameters.codes.Count > 20)
                {
                    response.SetInputError(new QueryParameterCountException(nameof(parameters.codes), 20, parameters.codes.Count, "A maximum of 20 total codes can be provided at one time."), info.settings);

                    return response;
                }

                Regex regex = new Regex(RegexPatternUtil.ENTITLEMENT_CODE);
                foreach (string code in parameters.codes)
                {
                    if (regex.IsMatch(code))
                    {
                        continue;
                    }

                    FormatException inner_exception = new FormatException("Value must match the regular expression: " + RegexPatternUtil.ENTITLEMENT_CODE);

                    string message = "The status code must be a 15 character alpha-numeric string, with optional dashes after every 5th character." + Environment.NewLine +
                                     "Example: ABCDE-12345-F6G7H";
                    response.SetInputError(new QueryParameterValueException(nameof(parameters.codes), code, message, inner_exception), info.settings);

                    return response;
                }

                RestRequest request = GetBaseRequest("entitlements/codes", Method.POST, info);
                request.AddParameters(parameters);

                RestResponse<Data<CodeStatus>> _response = await client.ExecuteAsync<Data<CodeStatus>>(request, HandleResponse);
                response = new HelixResponse<Data<CodeStatus>>(_response);

                return response;
            }

            #endregion

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
            /// <exception cref="HeaderParameterException">Thrown if the App Access Token or Client ID is null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if the manifest ID is not provided.</exception>
            /// <exception cref="QueryParameterValueException">Thrown if the manifest ID is longer than 64 characters.</exception>
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
                    response.SetInputError(new QueryParameterException(nameof(parameters.manifest_id), "Parameter is required and the value cannot be null, empty, or contain only whitespace."), info.settings);

                    return response;
                }

                if (!parameters.manifest_id.Length.IsInRange(1, 64))
                {
                    response.SetInputError(new QueryParameterValueException(nameof(parameters.manifest_id), "The manifest ID must be between 1 and 64 characters long, inclusive."), info.settings);

                    return response;
                }

                RestRequest request = GetBaseRequest("entitlements/upload", Method.POST, info);
                request.AddParameters(parameters);

                RestResponse<Data<EntitlementUploadUrl>> _response = await client.ExecuteAsync<Data<EntitlementUploadUrl>>(request, HandleResponse);
                response = new HelixResponse<Data<EntitlementUploadUrl>>(_response);

                return response;
            }

            #endregion

            // TODO: Implement /extensions/transactions

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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterCountException">
            /// Thrown if no game ID's or game names are provided.
            /// Thrown if all provided game ID's and game names are null, empty, or contains only whitespace.
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
                    response.SetInputError(new QueryParameterCountException(100, count, "At least one game ID or game name must be provided."), info.settings);

                    return response;
                }

                parameters.ids = parameters.ids.RemoveInvalidAndDuplicateValues();
                parameters.names = parameters.names.RemoveInvalidAndDuplicateValues();

                count = parameters.ids.Count + parameters.names.Count;
                if (count == 0)
                {
                    response.SetInputError(new QueryParameterCountException(100, count, "All provided game ID's and/or game names were null, empty, or contained only whitespace."), info.settings);

                    return response;
                }
                else if (count > 100)
                {
                    response.SetInputError(new QueryParameterCountException(100, count, "A maximum of 100 total game ID's and/or names can be provided at one time."), info.settings);

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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if both after and before cursors are provided.</exception>
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
                        response.SetInputError(new QueryParameterException("Only one pagination direction can be specified. Only use either 'after' or 'before'."), info.settings);

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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if both after and before cursors are provided.</exception>
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
                    // NOTE: /games/top - GetTopGamesAsync(...) - Temporarily disabling using 'before' while requesting all pages until it works properly.
                    // TODO: /games/top - GetTopGamesAsync(...) - Reimplement 'before' when it works propery.
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
                        response.SetInputError(new QueryParameterException("Only one pagination direction can be specified. Only use either 'after' or 'before'."), info.settings);

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

            // TODO: Implement /moderation/banned

            // TODO: Implement /moderation/banned/events

            // TODO: Implement /moderation/enforcements/status

            // TODO: Implement /moderation/moderators

            // TODO: Implement /moderation/moderators/events

            #region /moderation/moderators/events

            public static async Task<IHelixResponse<DataPage<ModeratorEvent>>>
            GetModeratorEventsPageAsync(HelixInfo info, ModeratorEventsParameters parameters)
            {
                info.required_scopes = Scopes.ModerationRead;

                HelixResponse<DataPage<ModeratorEvent>> response = new HelixResponse<DataPage<ModeratorEvent>>();
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                // Required parameters checks
                if(!ValidateRequiredQueryString(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, info.settings))
                {
                    return response;
                }

                // Optional parameters checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryString(nameof(parameters.after), parameters.after, response, info.settings) ||
                    !ValidateOptionalQueryString(nameof(parameters.user_ids), parameters.user_ids, 100, response, info.settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("moderation/moderators/events", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<ModeratorEvent>> _response = await client.ExecuteAsync<DataPage<ModeratorEvent>>(request, HandleResponse);
                response = new HelixResponse<DataPage<ModeratorEvent>>(_response);

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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if both after and before cursors are provided.</exception>
            /// <exception cref="QueryParameterCountException">
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
                        response.SetInputError(new QueryParameterException("Only one pagination direction can be specified. Only use either 'after' or 'before'."), info.settings);

                        return response;
                    }

                    parameters.community_ids = parameters.community_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.community_ids.Count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.community_ids), 100, parameters.community_ids.Count, "A maximum of 100 total community ID's can be provided at one time."), info.settings);

                        return response;
                    }

                    parameters.game_ids = parameters.game_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.game_ids.Count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.game_ids), 100, parameters.game_ids.Count, "A maximum of 100 total game ID's can be provided at one time."), info.settings);

                        return response;
                    }

                    parameters.user_ids = parameters.user_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.user_ids.Count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.user_ids), 100, parameters.user_ids.Count, "A maximum of 100 total user ID's can be provided at one time."), info.settings);

                        return response;
                    }

                    parameters.user_logins = parameters.user_logins.RemoveInvalidAndDuplicateValues();
                    if (parameters.user_logins.Count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.user_logins), 100, parameters.user_logins.Count, "A maximum of 100 total user logins can be provided at one time."), info.settings);

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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if both after and before cursors are provided.</exception>
            /// <exception cref="QueryParameterCountException">
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
                    // NOTE: /streams - GetStreamsAsync(...) - Temporarily disabling using 'before' while requesting all pages until it works properly.
                    // TODO: /streams - GetStreamsAsync(...) - Reimplement 'before' when it works propery.
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
                        response.SetInputError(new QueryParameterException("Only one pagination direction can be specified. Only use either 'after' or 'before'."), info.settings);

                        return response;
                    }

                    parameters.community_ids = parameters.community_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.community_ids.Count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.community_ids), 100, parameters.community_ids.Count, "A maximum of 100 total community ID's can be provided at one time."), info.settings);

                        return response;
                    }

                    parameters.game_ids = parameters.game_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.game_ids.Count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.game_ids), 100, parameters.game_ids.Count, "A maximum of 100 total game ID's can be provided at one time."), info.settings);

                        return response;
                    }

                    parameters.user_ids = parameters.user_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.user_ids.Count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.user_ids), 100, parameters.user_ids.Count, "A maximum of 100 total user ID's can be provided at one time."), info.settings);

                        return response;
                    }

                    parameters.user_logins = parameters.user_logins.RemoveInvalidAndDuplicateValues();
                    if (parameters.user_logins.Count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.user_logins), 100, parameters.user_logins.Count, "A maximum of 100 total user logins can be provided at one time."), info.settings);

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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if the user ID is not provided.</exception>
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
                    response.SetInputError(new QueryParameterException(nameof(user_id), "Parameter is required and the value cannot be null, empty, or contain only whitespace."), info.settings);

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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if the user login is null, empty, or contains only whitespace.</exception>
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
                    response.SetInputError(new QueryParameterException(nameof(user_login), "Parameter is required and the value cannot be null, empty, or contain only whitespace."), info.settings);

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

            #region /streams/markers            

            /// <summary>
            /// <para>
            /// Asynchronously creates a stream marker (an arbitray time spamp) in a stream specified by the provided user ID.
            /// Stream markers can be created by the person streaming or any of their editors.
            /// </para>
            /// <para>Required Scope: <see cref="Scopes.UserEditBroadcast"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the contains stream marker.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if the user ID is null, empty, or contains only whitespace.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.UserEditBroadcast"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<CreatedStreamMarker>>>
            CreateStreamMarkerAsync(HelixInfo info, CreateStreamMarkerParameters parameters)
            {
                info.required_scopes = Scopes.UserEditBroadcast;

                HelixResponse<DataPage<CreatedStreamMarker>> response = new HelixResponse<DataPage<CreatedStreamMarker>>();
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!parameters.user_id.IsValid())
                {
                    response.SetInputError(new QueryParameterException(nameof(parameters.user_id), "Parameter is required and the value cannot be null, empty, or contain only whitespace."), info.settings);

                    return response;
                }                

                RestRequest request = GetBaseRequest("streams/markers", Method.POST, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<CreatedStreamMarker>> _response = await client.ExecuteAsync<DataPage<CreatedStreamMarker>>(request, HandleResponse);
                response = new HelixResponse<DataPage<CreatedStreamMarker>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously gets a single page of stream markers (arbitray time spamps) for a user or a specific video.</para>
            /// <para>Required Scope: <see cref="Scopes.UserReadBroadcast"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of stream markers for the user or specified video.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if a user ID and video ID are provided.
            /// Thrown if the user ID and video ID are null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.UserReadBroadcast"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<StreamMarkers>>>
            GetStreamMarkersPageAsync(HelixInfo info, StreamMarkersParameters parameters)
            {
                info.required_scopes = Scopes.UserReadBroadcast;

                HelixResponse<DataPage<StreamMarkers>> response = new HelixResponse<DataPage<StreamMarkers>>();
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (parameters.user_id.IsValid() && parameters.video_id.IsValid())
                {
                    response.SetInputError(new QueryParameterException("Only a user ID or a video ID can be provided."), info.settings);

                    return response;
                }
                else if (!parameters.user_id.IsValid() && !parameters.video_id.IsValid())
                {
                    response.SetInputError(new QueryParameterException("A user ID or video ID must be provided."), info.settings);

                    return response;
                }

                parameters.after = parameters.after.NullIfInvalid();
                parameters.first = parameters.first.Clamp(1, 100);

                RestRequest request = GetBaseRequest("streams/markers", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<StreamMarkers>> _response = await client.ExecuteAsync<DataPage<StreamMarkers>>(request, HandleResponse);
                response = new HelixResponse<DataPage<StreamMarkers>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously gets a complete list of stream markers (arbitray time spamps) for a user or a specific video.</para>
            /// <para>Required Scope: <see cref="Scopes.UserReadBroadcast"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the complete list of stream markers for the user or specified video.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if a user ID and video ID are provided.
            /// Thrown if the user ID and video ID are null, empty, or contains only whitespace.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.UserReadBroadcast"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<StreamMarkers>>>
            GetStreamMarkersAsync(HelixInfo info, StreamMarkersParameters parameters)
            {
                info.required_scopes = Scopes.UserReadBroadcast;

                HelixResponse<DataPage<StreamMarkers>> response = new HelixResponse<DataPage<StreamMarkers>>();
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (parameters.user_id.IsValid() && parameters.video_id.IsValid())
                {
                    response.SetInputError(new QueryParameterException("Only a user ID or a video ID can be provided."), info.settings);

                    return response;
                }
                else if (!parameters.user_id.IsValid() && !parameters.video_id.IsValid())
                {
                    response.SetInputError(new QueryParameterException("A user ID or video ID must be provided."), info.settings);

                    return response;
                }

                parameters.after = parameters.after.NullIfInvalid();
                parameters.first = parameters.first.Clamp(1, 100);

                RestRequest request = GetBaseRequest("streams/markers", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<StreamMarkers>> _response = await client.TraceExecuteAsync<StreamMarkers, DataPage<StreamMarkers>>(request, HandleResponse);
                response = new HelixResponse<DataPage<StreamMarkers>>(_response);

                return response;
            }

            #endregion

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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if both after and before cursors are provided.</exception>
            /// <exception cref="QueryParameterCountException">
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
                        response.SetInputError(new QueryParameterException("Only one pagination direction can be specified. Only use either 'after' or 'before'."), info.settings);

                        return response;
                    }

                    parameters.community_ids = parameters.community_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.community_ids.Count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.community_ids), 100, parameters.community_ids.Count, "A maximum of 100 total community ID's can be provided at one time."), info.settings);

                        return response;
                    }

                    parameters.game_ids = parameters.game_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.game_ids.Count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.game_ids), 100, parameters.game_ids.Count, "A maximum of 100 total game ID's can be provided at one time."), info.settings);

                        return response;
                    }

                    parameters.user_ids = parameters.user_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.user_ids.Count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.user_ids), 100, parameters.user_ids.Count, "A maximum of 100 total user ID's can be provided at one time."), info.settings);

                        return response;
                    }

                    parameters.user_logins = parameters.user_logins.RemoveInvalidAndDuplicateValues();
                    if (parameters.user_logins.Count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.user_logins), 100, parameters.user_logins.Count, "A maximum of 100 total user logins can be provided at one time."), info.settings);

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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if both after and before cursors are provided.</exception>
            /// <exception cref="QueryParameterCountException">
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
                    // NOTE: /streams/metadata - GetStreamsMetadataAsync(...) - Temporarily disabling using 'before' while requesting all pages until it works properly.
                    // TODO: /streams/metadata - GetStreamsMetadataAsync(...) - Reimplement 'before' when it works propery.
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
                        response.SetInputError(new QueryParameterException("Only one pagination direction can be specified. Only use either 'after' or 'before'."), info.settings);

                        return response;
                    }

                    parameters.community_ids = parameters.community_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.community_ids.Count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.community_ids), 100, parameters.community_ids.Count, "A maximum of 100 total community ID's can be provided at one time."), info.settings);

                        return response;
                    }

                    parameters.game_ids = parameters.game_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.game_ids.Count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.game_ids), 100, parameters.game_ids.Count, "A maximum of 100 total game ID's can be provided at one time."), info.settings);

                        return response;
                    }

                    parameters.user_ids = parameters.user_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.user_ids.Count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.user_ids), 100, parameters.user_ids.Count, "A maximum of 100 total user ID's can be provided at one time."), info.settings);

                        return response;
                    }

                    parameters.user_logins = parameters.user_logins.RemoveInvalidAndDuplicateValues();
                    if (parameters.user_logins.Count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.user_logins), 100, parameters.user_logins.Count, "A maximum of 100 total user logins can be provided at one time."), info.settings);

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

            #region /streams/tags

            /// <summary>
            /// Asynchronously gets the stream tags a broadcaster has set.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the stream tags the broadcaster has set.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID is null, empty, or contains only whitespace.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<StreamTag>>>
            GetStreamsTagsAsync(HelixInfo info, StreamsTagsParameters parameters)
            {
                HelixResponse<Data<StreamTag>> response = new HelixResponse<Data<StreamTag>>();
                if (!ValidateAuthorizationParameters(info, response))
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
                    response.SetInputError(new QueryParameterException(nameof(parameters.broadcaster_id), "Parameter is required and the value cannot be null, empty, or contain only whitespace."), info.settings);

                    return response;
                }

                RestRequest request = GetBaseRequest("streams/tags", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<Data<StreamTag>> _response = await client.ExecuteAsync<Data<StreamTag>>(request, HandleResponse);
                response = new HelixResponse<Data<StreamTag>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously sets and overwrites a broadcaster's stream tags.</para>
            /// <para>
            /// If no stream tags are specified, all stream tags are removed.
            /// The automatic tags that Twitch sets are not affected and cannot be added/removed.
            /// The set stream tags expire after 72 hours of being applied, or 72 hours after a stream goes offline if the stream was live during the initial 72 hour expriation window.
            /// </para>
            /// <para>Required scope: <see cref="Scopes.UserEditBroadcast"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the broadcaster's set stream tags.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID is null, empty, or contains only whitespace.</exception>
            /// <exception cref="BodyParameterCountException">
            /// Thrown if all provided tag ID's are null, empty, or contains only whitespace.
            /// Thrown if more than 5 total tag ID's are provided.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.UserEditBroadcast"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse>
            SetStreamsTagsAsync(HelixInfo info, SetStreamsTagsParameters parameters)
            {
                info.required_scopes = Scopes.UserEditBroadcast;

                HelixResponse<Data<StreamTag>> response = new HelixResponse<Data<StreamTag>>();
                if (!ValidateAuthorizationParameters(info, response))
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
                    response.SetInputError(new QueryParameterException(nameof(parameters.broadcaster_id), "Parameter is required and the value cannot be null, empty, or contain only whitespace."), info.settings);

                    return response;
                }

                if (parameters.tag_ids.IsValid())
                {
                    // If tag ID's are provided, assume it's intentional and check for these errors up front for better error messages.
                    parameters.tag_ids = parameters.tag_ids.RemoveInvalidAndDuplicateValues();
                    if (parameters.tag_ids.Count == 0)
                    {
                        response.SetInputError(new BodyParameterCountException(nameof(parameters.tag_ids), 5, parameters.tag_ids.Count, "All provided tag ID's were null, empty, or contained only whitespace."), info.settings);

                        return response;
                    }
                    else if (parameters.tag_ids.Count > 5)
                    {
                        response.SetInputError(new BodyParameterCountException(nameof(parameters.tag_ids), 5, parameters.tag_ids.Count, "A maximum of 5 total tag ID's can be provided at one time."), info.settings);

                        return response;
                    }
                }

                RestRequest request = GetBaseRequest("streams/tags", Method.PUT, info);
                request.AddParameters(parameters);

                RestResponse<Data<StreamTag>> _response = await client.ExecuteAsync<Data<StreamTag>>(request, HandleResponse);
                response = new HelixResponse<Data<StreamTag>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously removes the stream tags a broadcaster has set.</para>
            /// <para>
            /// The automatic tags that Twitch sets are not affected and cannot be removed.
            /// </para>
            /// <para>Required scope: <see cref="Scopes.UserEditBroadcast"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">
            /// A set of rest parameters.
            /// The tag ID's are ignored, if provided.
            /// </param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the broadcaster's set stream tags.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID is null, empty, or contains only whitespace.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.UserEditBroadcast"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse>
            RemoveStreamsTagsAsync(HelixInfo info, SetStreamsTagsParameters parameters)
            {
                info.required_scopes = Scopes.UserEditBroadcast;

                HelixResponse<Data<StreamTag>> response = new HelixResponse<Data<StreamTag>>();
                if (!ValidateAuthorizationParameters(info, response))
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
                    response.SetInputError(new QueryParameterException(nameof(parameters.broadcaster_id), "Parameter is required and the value cannot be null, empty, or contain only whitespace."), info.settings);

                    return response;
                }

                parameters.tag_ids = null;

                RestRequest request = GetBaseRequest("streams/tags", Method.PUT, info);
                request.AddParameters(parameters);

                RestResponse<Data<StreamTag>> _response = await client.ExecuteAsync<Data<StreamTag>>(request, HandleResponse);
                response = new HelixResponse<Data<StreamTag>>(_response);

                return response;
            }

            // TODO: Add RemoveStreamsTags

            #endregion

            #region /subscriptions

            /// <summary>
            /// <para>Asynchronously gets a single page of a broadcaster's subscribers list.</para>
            /// <para>Required Scope: <see cref="Scopes.ChannelReadSubscriptions"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of a broadcaster's subscribers list.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID is null, empty, or contains only whitespace.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.ChannelReadSubscriptions"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<Subscription>>>
            GetBroadcasterSubscribersPageAsync(HelixInfo info, SubscriptionParameters parameters)
            {
                info.required_scopes = Scopes.ChannelReadSubscriptions;

                HelixResponse<DataPage<Subscription>> response = new HelixResponse<DataPage<Subscription>>();
                if (!ValidateAuthorizationParameters(info, response))
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
                    response.SetInputError(new QueryParameterException(nameof(parameters.broadcaster_id), "Parameter is required and the value cannot be null, empty, or contain only whitespace."), info.settings);

                    return response;
                }

                parameters.after = parameters.after.NullIfInvalid();
                parameters.first = parameters.first.Clamp(1, 100);

                RestRequest request = GetBaseRequest("subscriptions", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<Subscription>> _response = await client.ExecuteAsync<DataPage<Subscription>>(request, HandleResponse);
                response = new HelixResponse<DataPage<Subscription>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously gets a broadcaster's complete subscriber list.</para>
            /// <para>Required Scope: <see cref="Scopes.ChannelReadSubscriptions"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the broadcaster's complete subscriber list.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID is null, empty, or contains only whitespace.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.ChannelReadSubscriptions"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<Subscription>>>
            GetBroadcasterSubscribersAsync(HelixInfo info, SubscriptionParameters parameters)
            {
                info.required_scopes = Scopes.ChannelReadSubscriptions;

                HelixResponse<DataPage<Subscription>> response = new HelixResponse<DataPage<Subscription>>();
                if (!ValidateAuthorizationParameters(info, response))
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
                    response.SetInputError(new QueryParameterException(nameof(parameters.broadcaster_id), "Parameter is required and the value cannot be null, empty, or contain only whitespace."), info.settings);

                    return response;
                }

                parameters.after = parameters.after.NullIfInvalid();
                parameters.first = parameters.first.Clamp(1, 100);

                RestRequest request = GetBaseRequest("subscriptions", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<Subscription>> _response = await client.TraceExecuteAsync<Subscription, DataPage<Subscription>>(request, HandleResponse);
                response = new HelixResponse<DataPage<Subscription>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously gets the subscription relationship between a broadcaster and a list of users.</para>
            /// <para>
            /// If a user is subscribed to the broadcater, the subscription information for that user is returned in the response.
            /// If a user is not subscribed to the broadcater, that user is omitted from the response.
            /// </para>
            /// <para>Required Scope: <see cref="Scopes.ChannelReadSubscriptions"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// <para>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the subscription relationship between the broadcaster and the list of users.
            /// </para>
            /// <para>
            /// If a user is subscribed to the broadcater, the subscription information for that user is returned in the response.
            /// If a user is not subscribed to the broadcater, that user is omitted from the response.
            /// </para>
            /// </returns>        
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID is null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterCountException">
            /// Thrown if no user ID's or game names are provided.
            /// Thrown if all provided user ID's and game names are null, empty, or contains only whitespace.
            /// Thrown if more than 100 total user ID's and/or game names are provided.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.ChannelReadSubscriptions"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<Subscription>>>
            GetSubscriptionRelationshipAsync(HelixInfo info, SubscriptionRelationshipParameters parameters)
            {
                info.required_scopes = Scopes.ChannelReadSubscriptions;

                HelixResponse<Data<Subscription>> response = new HelixResponse<Data<Subscription>>();
                if (!ValidateAuthorizationParameters(info, response))
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
                    response.SetInputError(new QueryParameterException(nameof(parameters.broadcaster_id), "Parameter is required and the value cannot be null, empty, or contain only whitespace."), info.settings);

                    return response;
                }

                if (!parameters.user_id.IsValid())
                {
                    response.SetInputError(new QueryParameterCountException(nameof(parameters.user_id), 100, parameters.user_id.Count, "At leats one user ID must be provided."), info.settings);

                    return response;
                }

                parameters.user_id = parameters.user_id.RemoveInvalidAndDuplicateValues();
                if (parameters.user_id.Count == 0)
                {
                    response.SetInputError(new QueryParameterCountException(nameof(parameters.user_id), 100, parameters.user_id.Count, "All provided user ID's were null, empty, or contained only whitespace."), info.settings);

                    return response;
                }
                else if (parameters.user_id.Count > 100)
                {
                    response.SetInputError(new QueryParameterCountException(nameof(parameters.user_id), 100, parameters.user_id.Count, "A maximum of 100 total user ID's can be provided at one time."), info.settings);

                    return response;
                }

                RestRequest request = GetBaseRequest("subscriptions", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<Data<Subscription>> _response = await client.ExecuteAsync<Data<Subscription>>(request, HandleResponse);
                response = new HelixResponse<Data<Subscription>>(_response);

                return response;
            }

            /// <summary>
            /// Asynchronously checks to see if the from_id user is following the to_id user.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="broadcaster_id">The user ID of the broadcaster.</param>
            /// <param name="user_id">The ID of the possibly subscribed user.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> is set true if from_id is following to_id, otherwise false.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID or user ID is null, empty, or contains only whitespace.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<bool>>
            IsUserSubscribedAsync(HelixInfo info, string broadcaster_id, string user_id)
            {
                HelixResponse<bool> response = new HelixResponse<bool>();
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                if (!broadcaster_id.IsValid() || !user_id.IsValid())
                {
                    response.SetInputError(new QueryParameterException("Both broadcaster_id and user_id are required and cannot be null, empty, or contain only whitespace."), info.settings);

                    return response;
                }

                SubscriptionRelationshipParameters parameters = new SubscriptionRelationshipParameters();
                parameters.broadcaster_id = broadcaster_id;
                parameters.user_id.Add(user_id);

                IHelixResponse<Data<Subscription>> _response = await GetSubscriptionRelationshipAsync(info, parameters);

                bool result = _response.exception.IsNull() ? _response.result.data.IsValid() : false;
                response = new HelixResponse<bool>(_response, result);

                return response;
            }

            #endregion

            #region /subscriptions/events - New error handling

            /// <summary>
            /// <para>Asynchronously gets a specific subscription event or a single page subscription events.</para>
            /// <para>Required Scope: <see cref="Scopes.ChannelReadSubscriptions"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific subscription event or the single page subscription events.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID and event ID are not provided.</exception>
            /// <exception cref="QueryParameterValueException">
            /// Thrown if the broadcaster ID is empty or only contains white space, if provided.
            /// Thrown if the event ID is empty or only contains white space, if provided.
            /// Thrown if the user ID is empty or only contains white space, if provided.
            /// Thrown if the after cursor is empty or only contains white space, if provided.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.ChannelReadSubscriptions"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<SubscriptionEvent>>>
            GetSubscriptionEventsPageAsync(HelixInfo info, SubscriptionEventsParameters parameters)
            {
                info.required_scopes = Scopes.ChannelReadSubscriptions;

                HelixResponse<DataPage<SubscriptionEvent>> response = new HelixResponse<DataPage<SubscriptionEvent>>();
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                // Required parameters checks 
                if (parameters.broadcaster_id.IsNull() && parameters.id.IsNull())
                {
                    response.SetInputError(new QueryParameterException("A boradcaster ID or event ID must be provided."), info.settings);

                    return response;
                }
                else if (!parameters.broadcaster_id.IsNull() && !parameters.id.IsNull())
                {
                    response.SetInputError(new QueryParameterException("Only a boradcaster ID or event ID can be provided at one time."), info.settings);

                    return response;
                }

                if (!parameters.broadcaster_id.IsNull() && parameters.broadcaster_id.IsEmptyOrWhiteSpace())
                {
                    response.SetInputError(new QueryParameterValueException(nameof(parameters.broadcaster_id), parameters.broadcaster_id, "Value cannot be empty or contain only whitespace."), info.settings);

                    return response;
                }
                else if (!parameters.id.IsNull() && parameters.id.IsEmptyOrWhiteSpace())
                {
                    response.SetInputError(new QueryParameterValueException(nameof(parameters.id), parameters.id, "Value cannot be empty or contain only whitespace."), info.settings);

                    return response;
                }

                // Optional parameters checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (parameters.user_id.HasContent())
                {
                    // Pagination is ignored if a user ID was provided.
                    parameters.after = null;
                }

                if (!ValidateOptionalQueryString(nameof(parameters.after), parameters.after, response, info.settings) ||
                    !ValidateOptionalQueryString(nameof(parameters.user_id), parameters.user_id, response, info.settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("subscriptions/events", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<SubscriptionEvent>> _response = await client.ExecuteAsync<DataPage<SubscriptionEvent>>(request, HandleResponse);
                response = new HelixResponse<DataPage<SubscriptionEvent>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously gets a specific subscription event or a complete list of subscription events.</para>
            /// <para>Required Scope: <see cref="Scopes.ChannelReadSubscriptions"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific subscription event or the complete list of subscription events.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID and event ID are not provided.</exception>
            /// <exception cref="QueryParameterValueException">
            /// Thrown if the broadcaster ID is empty or only contains white space, if provided.
            /// Thrown if the event ID is empty or only contains white space, if provided.
            /// Thrown if the user ID is empty or only contains white space, if provided.
            /// Thrown if the after cursor is empty or only contains white space, if provided.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.ChannelReadSubscriptions"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<SubscriptionEvent>>>
            GetSubscriptionEventsAsync(HelixInfo info, SubscriptionEventsParameters parameters)
            {
                info.required_scopes = Scopes.ChannelReadSubscriptions;

                HelixResponse<DataPage<SubscriptionEvent>> response = new HelixResponse<DataPage<SubscriptionEvent>>();
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                // Required parameters checks 
                if (parameters.broadcaster_id.IsNull() && parameters.id.IsNull())
                {
                    response.SetInputError(new QueryParameterException("A boradcaster ID or event ID must be provided."), info.settings);

                    return response;
                }
                else if (!parameters.broadcaster_id.IsNull() && !parameters.id.IsNull())
                {
                    response.SetInputError(new QueryParameterException("Only a boradcaster ID or event ID can be provided at one time."), info.settings);

                    return response;
                }

                if (!parameters.broadcaster_id.IsNull() && parameters.broadcaster_id.IsEmptyOrWhiteSpace())
                {
                    response.SetInputError(new QueryParameterValueException(nameof(parameters.broadcaster_id), parameters.broadcaster_id, "Value cannot be empty or contain only whitespace."), info.settings);

                    return response;
                }
                else if (!parameters.id.IsNull() && parameters.id.IsEmptyOrWhiteSpace())
                {
                    response.SetInputError(new QueryParameterValueException(nameof(parameters.id), parameters.id, "Value cannot be empty or contain only whitespace."), info.settings);

                    return response;
                }

                // Optional parameters checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (parameters.user_id.HasContent())
                {
                    // Pagination is ignored if a user ID was provided.
                    parameters.after = null;
                }

                if (!ValidateOptionalQueryString(nameof(parameters.after), parameters.after, response, info.settings) ||
                    !ValidateOptionalQueryString(nameof(parameters.user_id), parameters.user_id, response, info.settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("subscriptions/events", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<SubscriptionEvent>> _response = await client.TraceExecuteAsync<SubscriptionEvent, DataPage<SubscriptionEvent>>(request, HandleResponse);
                response = new HelixResponse<DataPage<SubscriptionEvent>>(_response);

                return response;
            }

            #endregion

            #region /tags/streams

            /// <summary>
            /// Asynchronously gets specific stream tags, or a single page of stream tags.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> containts the specific stream tags, or the single page of stream tags.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterCountException">
            /// Thrown if more than 100 total atg ID's are provided.
            /// Thrown if all tag ID's are are null, empty, or contains only whitespace, if Provided.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<StreamTag>>>
            GetStreamTagsPageAsync(HelixInfo info, StreamTagsParameters parameters)
            {
                HelixResponse<DataPage<StreamTag>> response = new HelixResponse<DataPage<StreamTag>>();
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                if (!parameters.IsNull())
                {
                    parameters.after = parameters.after.NullIfInvalid();

                    if (parameters.tag_ids.IsValid())
                    {
                        // If tag ID's are provided, assume it's intentional and check for these errors up front for better error messages.
                        parameters.tag_ids = parameters.tag_ids.RemoveInvalidAndDuplicateValues();
                        if (parameters.tag_ids.Count == 0)
                        {
                            response.SetInputError(new QueryParameterCountException(nameof(parameters.tag_ids), 100, parameters.tag_ids.Count, "All provided tag ID's were null, empty, or contained only whitespace."), info.settings);

                            return response;
                        }
                        else if (parameters.tag_ids.Count > 100)
                        {
                            response.SetInputError(new QueryParameterCountException(nameof(parameters.tag_ids), 100, parameters.tag_ids.Count, "A maximum of 100 total tag ID's can be provided at one time."), info.settings);

                            return response;
                        }

                        parameters.first = null;
                        parameters.after = null;
                    }

                    parameters.first = parameters.first.Clamp(1, 100);
                }                

                RestRequest request = GetBaseRequest("tags/streams", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<StreamTag>> _response = await client.ExecuteAsync<DataPage<StreamTag>>(request, HandleResponse);
                response = new HelixResponse<DataPage<StreamTag>>(_response);

                return response;
            }

            /// <summary>
            /// Asynchronously gets specific stream tags, or a complete list of stream tags.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> containts the specific stream tags, or the complete list of stream tags.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterCountException">
            /// Thrown if more than 100 total atg ID's are provided.
            /// Thrown if all tag ID's are are null, empty, or contains only whitespace, if Provided.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<StreamTag>>>
            GetStreamTagsAsync(HelixInfo info, StreamTagsParameters parameters)
            {
                HelixResponse<DataPage<StreamTag>> response = new HelixResponse<DataPage<StreamTag>>();
                if (!ValidateAuthorizationParameters(info, response))
                {
                    return response;
                }

                if (!parameters.IsNull())
                {
                    parameters.after = parameters.after.NullIfInvalid();

                    if (parameters.tag_ids.IsValid())
                    {
                        // If tag ID's are provided, assume it's intentional and check for these errors up front for better error messages.
                        parameters.tag_ids = parameters.tag_ids.RemoveInvalidAndDuplicateValues();
                        if (parameters.tag_ids.Count == 0)
                        {
                            response.SetInputError(new QueryParameterCountException(nameof(parameters.tag_ids), 100, parameters.tag_ids.Count, "All provided tag ID's were null, empty, or contained only whitespace."), info.settings);

                            return response;
                        }
                        else if (parameters.tag_ids.Count > 100)
                        {
                            response.SetInputError(new QueryParameterCountException(nameof(parameters.tag_ids), 100, parameters.tag_ids.Count, "A maximum of 100 total tag ID's can be provided at one time."), info.settings);

                            return response;
                        }

                        parameters.first = null;
                        parameters.after = null;
                    }

                    parameters.first = parameters.first.Clamp(1, 100);
                }

                RestRequest request = GetBaseRequest("tags/streams", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<StreamTag>> _response = await client.TraceExecuteAsync<StreamTag, DataPage<StreamTag>>(request, HandleResponse);
                response = new HelixResponse<DataPage<StreamTag>>(_response);

                return response;
            }

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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterCountException">
            /// Thrown if no user ID's and user logins are provided, or all elements are null, empty, or contains only whitespace when parameters are provided.
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
                        response.SetInputError(new QueryParameterCountException(100, count, "At least one user ID or user login must be provided."), info.settings);

                        return response;
                    }

                    parameters.ids = parameters.ids.RemoveInvalidAndDuplicateValues();
                    parameters.logins = parameters.logins.RemoveInvalidAndDuplicateValues();

                    count = parameters.ids.Count + parameters.logins.Count;
                    if (count == 0)
                    {
                        response.SetInputError(new QueryParameterCountException(100, count, "All provided user ID's and/or user logins were null, empty, or contained only whitespace."), info.settings);

                        return response;
                    }
                    else if (count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException(100, count, "A maximum of 100 total user ID's and/or user logins can be provided at one time."), info.settings);

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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if the user ID is null, empty, or contains only whitespace.</exception>
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
                        response.SetInputError(new QueryParameterException(nameof(parameters.user_id), "Parameter is required and the value cannot be null, empty, or contain only whitespace."), info.settings);

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
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
            /// <exception cref="BodyParameterException">
            /// Thrown if no supported extension slots are found across all extension types.
            /// </exception>
            /// <exception cref="BodyParameterValueException">
            /// Thrown if thwe parameters data is null.
            /// Thrown if the extension ID or extension version for any active supported extension slot is null, empty, or contains only whitespace.            
            /// Thrown if either (x, y) coordinate for an active supported component extension slot exceeds the range (0, 0) to (8000, 5000).
            /// </exception>
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

                if (parameters.data.IsNull())
                {
                    response.SetInputError(new BodyParameterValueException(nameof(parameters.data), parameters.data, "Value cannot be null"), info.settings);

                    return response;
                }

                parameters.data.panel = ValidateExtensionSlots(parameters.data.panel, ExtensionType.Panel, response, info.settings);
                if (!response.exception.IsNull())
                {
                    return response;
                }

                parameters.data.overlay = ValidateExtensionSlots(parameters.data.overlay, ExtensionType.Overlay, response, info.settings);
                if (!response.exception.IsNull())
                {
                    return response;
                }

                parameters.data.component = ValidateExtensionSlots(parameters.data.component, ExtensionType.Component, response, info.settings);
                if (!response.exception.IsNull())
                {
                    return response;
                }

                if (!parameters.data.component.IsValid() && !parameters.data.panel.IsValid() && !parameters.data.overlay.IsValid())
                {
                    response.SetInputError(new BodyParameterException("No supported extension slots were provided, or all supported extension slots were null."), info.settings);

                    return response;
                }

                if (!ValidateUniqueExtensionIDs(parameters.data, response, info.settings))
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
            /// <exception cref="BodyParameterValueException">
            /// Thrown if the extension ID or extension version for any active supported extension slot is null, empty, or contains only whitespace.
            /// Thrown if either (x, y) coordinate for an active supported component extension slot exceeds the range (0, 0) to (8000, 5000).
            /// </exception>
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
                        response.SetInputError(new BodyParameterValueException(nameof(extension.id), extension.id, "Value cannot be null, empty, or contain only whitespace."), settings);

                        return extensions;
                    }

                    if (!extension.version.IsValid())
                    {
                        response.SetInputError(new BodyParameterValueException(nameof(extension.version), extension.version, "Value cannot be null, empty, or contain only whitespace."), settings);

                        return extensions;
                    }

                    // Twitch only cares about these when a component is being updated.
                    if (type == ExtensionType.Component)
                    {
                        if (extension.x.HasValue && (extension.x.Value < 0 || extension.x.Value > 8000))
                        {
                            response.SetInputError(new BodyParameterValueException(nameof(extension.x), extension.x.Value, "The x coordinate must be between 0 and 8000, inclusive."), settings);

                            return extensions;
                        }

                        if (extension.y.HasValue && (extension.y.Value < 0 || extension.y.Value > 5000))
                        {
                            response.SetInputError(new BodyParameterValueException(nameof(extension.y), extension.y.Value, "The y coordinate must be between 0 and 8000, inclusive."), settings);

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
                        response.SetInputError(new DuplicateExtensionException(extension, "The extension ID, " + extension.id + ", was attempted to be set in two or more extension slots. An extension can only be set to one slot."), settings);

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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token is null, empty, or contains only whitespace.</exception>
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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if from_id is null, empty, or contains only whitespace.</exception>
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
                    response.SetInputError(new QueryParameterException(nameof(parameters.from_id), "Parameter is required and the value cannot be null, empty, or contain only whitespace."), info.settings);

                    return response;
                }

                parameters.to_id = null;

                response = await GetUserFollowsRelationshipPageAsync(info, parameters) as HelixResponse<FollowsDataPage<Follow>>;

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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if from_id is null, empty, or contains only whitespace.</exception>
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
                    response.SetInputError(new QueryParameterException(nameof(parameters.from_id), "Parameter is required and the value cannot be null, empty, or contain only whitespace."), info.settings);

                    return response;
                }

                parameters.to_id = null;

                response = await GetUserFollowsRelationshipAsync(info, parameters) as HelixResponse<FollowsDataPage<Follow>>;

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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if to_id is null, empty, or contains only whitespace.</exception>
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
                    response.SetInputError(new QueryParameterException(nameof(parameters.to_id), "Parameter is required and the value cannot be null, empty, or contain only whitespace."), info.settings);

                    return response;
                }

                parameters.from_id = null;

                response = await GetUserFollowsRelationshipPageAsync(info, parameters) as HelixResponse<FollowsDataPage<Follow>>;

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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if to_id is null, empty, or contains only whitespace.</exception>
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
                    response.SetInputError(new QueryParameterException(nameof(parameters.to_id), "Parameter is required and the value cannot be null, empty, or contain only whitespace."), info.settings);

                    return response;
                }

                parameters.from_id = null;

                response = await GetUserFollowsRelationshipAsync(info, parameters) as HelixResponse<FollowsDataPage<Follow>>;

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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if either from_id and to_id are null, empty, or contains only whitespace.</exception>
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
                    response.SetInputError(new QueryParameterException("Both from_id and to_id are required and cannot be null, empty, or contain only whitespace."), info.settings);

                    return response;
                }

                FollowsParameters parameters = new FollowsParameters(from_id, to_id);

                IHelixResponse<FollowsDataPage<Follow>> _response = await GetUserFollowsRelationshipPageAsync(info, parameters);

                bool result = _response.exception.IsNull() ? _response.result.data.IsValid() : false;
                response = new HelixResponse<bool>(_response, result);

                return response;
            }

            /// <summary>
            /// Asynchronously gets the follow relationship between two users, or a single page of a user's following/follower list.
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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if both from_id and to_id are null, empty, or contains only whitespace.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowsRelationshipPageAsync(HelixInfo info, FollowsParameters parameters)
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
                    response.SetInputError(new QueryParameterException("At minimum, from_id or to_id must be provided and cannot be null, empty, or contain only whitespace."), info.settings);

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
            /// Asynchronously gets the follow relationship between two users, or a user's complete following/follower list.
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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">Thrown if both from_id and to_id are null, empty, or contains only whitespace.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowsRelationshipAsync(HelixInfo info, FollowsParameters parameters)
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
                    response.SetInputError(new QueryParameterException("At minimum, from_id or to_id must be provided and cannot be null, empty, or contain only whitespace."), info.settings);

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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if after and before are provided.
            /// Thrown if no video ID's, game ID, or user ID are provided.
            /// Thrown if any multiple combination of video ID's, game ID, or user ID is provided.
            /// Thrown if the user ID is null, empty, or contains only whitespace, if Provided.
            /// Thrown if the game ID is null, empty, or contains only whitespace, if Provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">            
            /// Thrown if all video ID's are are null, empty, or contains only whitespace, if Provided.
            /// Thrown if more than 100 video ID's are provided.            
            /// </exception>
            /// <exception cref="RestParameterCountException">Thrown if more than 100 total video ID's are provided.</exception>
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
                    response.SetInputError(new QueryParameterException("Only one pagination direction can be specified. Only use either 'after' or 'before'."), info.settings);

                    return response;
                }

                if (parameters.ids.IsValid())
                {
                    // If video ID's are provided, assume it's intentional and check for these errors up front for better error messages.
                    parameters.ids = parameters.ids.RemoveInvalidAndDuplicateValues();
                    if(parameters.ids.Count == 0)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.ids), 100, parameters.ids.Count, "All provided video ID's were null, empty, or contained only whitespace."), info.settings);

                        return response;
                    }
                    else if (parameters.ids.Count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.ids), 100, parameters.ids.Count, "A maximum of 100 total video ID's can be provided at one time."), info.settings);

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
                    response.SetInputError(new QueryParameterException("At least one or more video ID, one user ID, or one game ID must be provided."), info.settings);

                    return response;
                }

                if ((parameters.ids.IsValid() && (parameters.user_id.IsValid() || parameters.game_id.IsValid())) ||
                   (parameters.user_id.IsValid() && parameters.game_id.IsValid()))
                {
                    response.SetInputError(new QueryParameterException("Only one or more video ID's, one user ID, or one game ID can be provided."), info.settings);

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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token and Client ID are null, empty, or contains only whitespace.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if after and before are provided.</exception>
            /// Thrown if no video ID's, game ID, or user ID are provided.
            /// Thrown if any multiple combination of video ID's, game ID, or user ID is provided.
            /// Thrown if the user ID is null, empty, or contains only whitespace, if Provided.
            /// Thrown if the game ID is null, empty, or contains only whitespace, if Provided.
            /// <exception cref="QueryParameterCountException">            
            /// Thrown if all video ID's are are null, empty, or contains only whitespace, if Provided.
            /// Thrown if more than 100 video ID's are provided.            
            /// </exception>
            /// <exception cref="RestParameterCountException">Thrown if more than 100 total video ID's are provided.</exception>
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

                // NOTE: /videos - GetVideosAsync(...) - Temporarily disabling using 'before' while requesting all pages until it works properly.
                // TODO: /videos - GetVideosAsync(...) - Reimplement 'before' when it works propery.
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
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.ids), 100, parameters.ids.Count, "All provided video ID's were null, empty, or contained only whitespace."), info.settings);

                        return response;
                    }
                    else if (parameters.ids.Count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException(nameof(parameters.ids), 100, parameters.ids.Count, "A maximum of 100 total video ID's can be provided at one time."), info.settings);

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
                    response.SetInputError(new QueryParameterException("At least one or more video ID, one user ID, or one game ID must be provided."), info.settings);

                    return response;
                }

                if ((parameters.ids.IsValid() && (parameters.user_id.IsValid() || parameters.game_id.IsValid())) ||
                   (parameters.user_id.IsValid() && parameters.game_id.IsValid()))
                {
                    response.SetInputError(new QueryParameterException("Only one or more video ID's, one user ID, or one game ID can be provided."), info.settings);

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

        private static bool
        ValidateRequiredQueryString<data_type>(string name, string value, HelixResponse<data_type> response, HelixRequestSettings settings)
        {
            if (value.IsNull())
            {
                response.SetInputError(new QueryParameterException(name, "A required query parameter is missing: " + name.WrapQuotes()), settings);

                return false;
            }

            if (value.IsEmptyOrWhiteSpace())
            {
                response.SetInputError(new QueryParameterValueException(name, value, "Value cannot be empty or contain only whitespace."), settings);

                return false;
            }

            return true;
        }

        private static bool
        ValidateRequiredQueryString(string name, List<string> values, int maximum_count, HelixResponse response, HelixRequestSettings settings)
        {
            if (values == null)
            {
                response.SetInputError(new QueryParameterException(name, "A required query parameter is missing: " + name.WrapQuotes()), settings);

                return false;
            }

            if(values.Count == 0)
            {
                response.SetInputError(new QueryParameterCountException(name, maximum_count, values.Count, "At least one query string must be provided for the parameter: " + name.WrapQuotes()), settings);

                return false;
            }

            if (values.Count > maximum_count)
            {
                response.SetInputError(new QueryParameterCountException(name, maximum_count, values.Count, "A maximum of " + maximum_count + " query strings cant be provided at one time for the parameter: " + name.WrapQuotes()), settings);

                return false;
            }

            List<int> indicies = values.GetNoContentIndicies();
            if (indicies.Count > 0)
            {
                string message = "One or more query strings were null, empty, or contained only white space for the parameter: " + name.WrapQuotes() + Environment.NewLine + Environment.NewLine +
                                 "Indicies: " + string.Join(", ", indicies);
                response.SetInputError(new QueryParameterValueException(name, message), settings);

                return false;
            }

            List<string> duplicates = values.GetDuplicateElements();
            if (duplicates.Count > 0)
            {
                string message = "One or more duplicate query string values were found for the parameter: " + name.WrapQuotes() + Environment.NewLine + Environment.NewLine +
                                 "Values : " + string.Join(", ", duplicates);
                response.SetInputError(new QueryParameterValueException(name, message), settings);

                return false;
            }

            return true;
        }

        private static bool
        ValidateOptionalQueryString(string name, string value, HelixResponse response, HelixRequestSettings settings)
        {
            if (value.IsNull())
            {
                return true;
            }

            if (value.IsEmptyOrWhiteSpace())
            {
                response.SetInputError(new QueryParameterValueException(name, value, "Value cannot be empty or contain only whitespace."), settings);

                return false;
            }

            return true;
        }

        private static bool
        ValidateOptionalQueryString(string name, List<string> values, int maximum_count, HelixResponse response, HelixRequestSettings settings)
        {
            // An empty list isn't inherently an error, especially since every list is instantiated for each parameter type for the user's convenience.
            if (values == null || values.Count == 0)
            {
                return true;
            }

            if (values.Count > maximum_count)
            {
                response.SetInputError(new QueryParameterCountException(name, maximum_count, values.Count, "A maximum of " + maximum_count + " query strings cant be provided at one time for the parameter: " + name.WrapQuotes()), settings);

                return false;
            }

            List<int> indicies = values.GetNoContentIndicies();
            if (indicies.Count > 0)
            {
                string message = "One or more query strings were null, empty, or contained only white space for the parameter: " + name.WrapQuotes() + Environment.NewLine + Environment.NewLine +
                                 "Indicies: " + string.Join(", ", indicies);
                response.SetInputError(new QueryParameterValueException(name, message), settings);

                return false;
            }

            List<string> duplicates = values.GetDuplicateElements();
            if (duplicates.Count > 0)
            {
                string message = "One or more duplicate query string values were found for the parameter: " + name.WrapQuotes() + Environment.NewLine + Environment.NewLine +
                                 "Values : " + string.Join(", ", duplicates);
                response.SetInputError(new QueryParameterValueException(name, message), settings);

                return false;
            }

            return true;
        }
    }
}