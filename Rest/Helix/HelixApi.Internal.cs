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

            private static readonly string REGEX_PATTERN_ENTITLEMENT_CODE = "^[a-zA-Z0-9]{5}-?[a-zA-Z0-9]{5}-?[a-zA-Z0-9]{5}$";

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
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the extension ID is empty or contains only white space, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// Thrown if started_at or ended at are provided without the other.
            /// Thrown if started_at or ended_at is later than <see cref="DateTime.UtcNow"/>, or started_at is later than ended_at, if provided.
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
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    parameters.first = parameters.first.Clamp(1, 100);

                    if (!ValidateOptionalQueryParameter(nameof(parameters.extension_id), parameters.extension_id, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings))
                    {
                        return response;
                    }

                    if (!parameters.extension_id.IsNull())
                    {
                        parameters.after = null;
                    }

                    if (parameters.started_at.HasValue && !parameters.ended_at.HasValue)
                    {
                        response.SetInputError(new QueryParameterException("ended_at must be provided if started_at is provided.", nameof(parameters.ended_at)), info.settings);

                        return response;
                    }
                    else if (!parameters.started_at.HasValue && parameters.ended_at.HasValue)
                    {
                        response.SetInputError(new QueryParameterException("started_at must be provided if ended_at is provided.", nameof(parameters.started_at)), info.settings);

                        return response;
                    }

                    if (parameters.started_at.HasValue)
                    {
                        parameters.started_at = parameters.started_at.Value.ToUniversalTime();
                        parameters.started_at = new DateTime(parameters.started_at.Value.Year, parameters.started_at.Value.Month, parameters.started_at.Value.Day);
                    }

                    if (parameters.ended_at.HasValue)
                    {
                        parameters.ended_at = parameters.ended_at.Value.ToUniversalTime();
                        parameters.ended_at = new DateTime(parameters.ended_at.Value.Year, parameters.ended_at.Value.Month, parameters.ended_at.Value.Day);
                    }

                    parameters.ended_at = parameters.ended_at?.ToUniversalTime();

                    if (!ValidateOptionalQueryParameter(nameof(parameters.ended_at), parameters.ended_at, DateTime.UtcNow, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.started_at), parameters.started_at, DateTime.UtcNow, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.started_at), parameters.started_at, parameters.ended_at, response, info.settings))
                    {
                        return response;
                    }
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the extension ID is empty or contains only white space, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// Thrown if started_at or ended at are provided without the other.
            /// Thrown if started_at or ended_at is later than <see cref="DateTime.UtcNow"/>, or started_at is later than ended_at, if provided.
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
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    parameters.first = parameters.first.Clamp(1, 100);

                    if (!ValidateOptionalQueryParameter(nameof(parameters.extension_id), parameters.extension_id, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings))
                    {
                        return response;
                    }

                    if (!parameters.extension_id.IsNull())
                    {
                        parameters.after = null;
                    }

                    if (parameters.started_at.HasValue && !parameters.ended_at.HasValue)
                    {
                        response.SetInputError(new QueryParameterException("ended_at must be provided if started_at is provided.", nameof(parameters.ended_at)), info.settings);

                        return response;
                    }
                    else if (!parameters.started_at.HasValue && parameters.ended_at.HasValue)
                    {
                        response.SetInputError(new QueryParameterException("started_at must be provided if ended_at is provided.", nameof(parameters.started_at)), info.settings);

                        return response;
                    }

                    if (parameters.started_at.HasValue)
                    {
                        parameters.started_at = parameters.started_at.Value.ToUniversalTime();
                        parameters.started_at = new DateTime(parameters.started_at.Value.Year, parameters.started_at.Value.Month, parameters.started_at.Value.Day);
                    }

                    if (parameters.ended_at.HasValue)
                    {
                        parameters.ended_at = parameters.ended_at.Value.ToUniversalTime();
                        parameters.ended_at = new DateTime(parameters.ended_at.Value.Year, parameters.ended_at.Value.Month, parameters.ended_at.Value.Day);
                    }

                    if (!ValidateOptionalQueryParameter(nameof(parameters.ended_at), parameters.ended_at, DateTime.UtcNow, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.started_at), parameters.started_at, DateTime.UtcNow, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.started_at), parameters.started_at, parameters.ended_at, response, info.settings))
                    {
                        return response;
                    }
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the game ID is empty or contains only white space, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// Thrown if started_at or ended at are provided without the other.
            /// Thrown if started_at or ended_at is later than <see cref="DateTime.UtcNow"/>, or started_at is later than ended_at, if provided.
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
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    parameters.first = parameters.first.Clamp(1, 100);

                    if (!ValidateOptionalQueryParameter(nameof(parameters.game_id), parameters.game_id, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings))
                    {
                        return response;
                    }

                    if (!parameters.game_id.IsNull())
                    {
                        parameters.after = null;
                    }

                    if (parameters.started_at.HasValue && !parameters.ended_at.HasValue)
                    {
                        response.SetInputError(new QueryParameterException("ended_at must be provided if started_at is provided.", nameof(parameters.ended_at)), info.settings);

                        return response;
                    }
                    else if (!parameters.started_at.HasValue && parameters.ended_at.HasValue)
                    {
                        response.SetInputError(new QueryParameterException("started_at must be provided if ended_at is provided.", nameof(parameters.started_at)), info.settings);

                        return response;
                    }

                    if (parameters.started_at.HasValue)
                    {
                        parameters.started_at = parameters.started_at.Value.ToUniversalTime();
                        parameters.started_at = new DateTime(parameters.started_at.Value.Year, parameters.started_at.Value.Month, parameters.started_at.Value.Day);
                    }

                    if (parameters.ended_at.HasValue)
                    {
                        parameters.ended_at = parameters.ended_at.Value.ToUniversalTime();
                        parameters.ended_at = new DateTime(parameters.ended_at.Value.Year, parameters.ended_at.Value.Month, parameters.ended_at.Value.Day);
                    }

                    if (!ValidateOptionalQueryParameter(nameof(parameters.ended_at), parameters.ended_at, DateTime.UtcNow, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.started_at), parameters.started_at, DateTime.UtcNow, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.started_at), parameters.started_at, parameters.ended_at, response, info.settings))
                    {
                        return response;
                    }
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the game ID is empty or contains only white space, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// Thrown if started_at or ended at are provided without the other.
            /// Thrown if started_at or ended_at is later than <see cref="DateTime.UtcNow"/>, or started_at is later than ended_at, if provided.
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
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    parameters.first = parameters.first.Clamp(1, 100);

                    if (!ValidateOptionalQueryParameter(nameof(parameters.game_id), parameters.game_id, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings))
                    {
                        return response;
                    }

                    if (!parameters.game_id.IsNull())
                    {
                        parameters.after = null;
                    }

                    if (parameters.started_at.HasValue && !parameters.ended_at.HasValue)
                    {
                        response.SetInputError(new QueryParameterException("ended_at must be provided if started_at is provided.", nameof(parameters.ended_at)), info.settings);

                        return response;
                    }
                    else if (!parameters.started_at.HasValue && parameters.ended_at.HasValue)
                    {
                        response.SetInputError(new QueryParameterException("started_at must be provided if ended_at is provided.", nameof(parameters.started_at)), info.settings);

                        return response;
                    }

                    if (parameters.started_at.HasValue)
                    {
                        parameters.started_at = parameters.started_at.Value.ToUniversalTime();
                        parameters.started_at = new DateTime(parameters.started_at.Value.Year, parameters.started_at.Value.Month, parameters.started_at.Value.Day);
                    }

                    if (parameters.ended_at.HasValue)
                    {
                        parameters.ended_at = parameters.ended_at.Value.ToUniversalTime();
                        parameters.ended_at = new DateTime(parameters.ended_at.Value.Year, parameters.ended_at.Value.Month, parameters.ended_at.Value.Day);
                    }

                    if (!ValidateOptionalQueryParameter(nameof(parameters.ended_at), parameters.ended_at, DateTime.UtcNow, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.started_at), parameters.started_at, DateTime.UtcNow, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.started_at), parameters.started_at, parameters.ended_at, response, info.settings))
                    {
                        return response;
                    }
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the user ID is empty or contains only white space, if provided.
            /// Thrown if started_at is later than <see cref="DateTime.Now"/>, if provided.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.BitsRead"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<BitsLeaderboardData<BitsUser>>>
            GetBitsLeaderboardAsync(HelixInfo info, BitsLeaderboardParameters parameters)
            {
                info.required_scopes = Scopes.BitsRead;

                HelixResponse<BitsLeaderboardData<BitsUser>> response = new HelixResponse<BitsLeaderboardData<BitsUser>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    parameters.count = parameters.count.Clamp(1, 100);

                    if (!ValidateOptionalQueryParameter(nameof(parameters.user_id), parameters.user_id, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.started_at), parameters.started_at, DateTime.Now, response, info.settings))
                    {
                        return response;
                    }

                    if (parameters.period == BitsLeaderboardPeriod.All)
                    {
                        parameters.started_at = null;
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.ClipsEdit"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<CreatedClip>>>
            CreateClipAsync(HelixInfo info, CreateClipParameters parameters)
            {
                info.required_scopes = Scopes.ClipsEdit;

                HelixResponse<Data<CreatedClip>> response = new HelixResponse<Data<CreatedClip>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!ValidateRequiredQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, info.settings))
                {
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if no clip ID's, broadcaster ID, or game ID is provided.
            /// Thrown if any combination of clip ID's, broadcaster ID, or game ID is provided.            
            /// Thrown if the broadcaster ID or game ID is empty or contains only white space, if Provided.
            /// Thrown if any clip ID is null, empty, or contains only white space or any duplicate clip ID's are found, if provided.
            /// Thrown if the after or before cursors are empty or contain only white space, if provided.
            /// Thrown if both after and before cursors are provided.
            /// Thrown if started_at or ended_at is later than <see cref="DateTime.Now"/>, or started_at is later than ended_at, if provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if more than 100 total clip ID's are provided.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<Clip>>>
            GetClipsPageAsync(HelixInfo info, ClipsParameters parameters)
            {
                HelixResponse<DataPage<Clip>> response = new HelixResponse<DataPage<Clip>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!parameters.ids.IsValid() && parameters.broadcaster_id.IsNull() && parameters.game_id.IsNull())
                {
                    response.SetInputError(new QueryParameterException("At least one or more clip ID, one broadcaster ID, or one game ID must be provided."), info.settings);

                    return response;
                }

                if ((parameters.ids.IsValid() && (!parameters.broadcaster_id.IsNull() || !parameters.game_id.IsNull())) ||
                   (!parameters.broadcaster_id.IsNull() && !parameters.game_id.IsNull()))
                {
                    response.SetInputError(new QueryParameterException("Only one or more clip ID's, one broadcaster ID, or one game ID can be provided."), info.settings);

                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.game_id), parameters.game_id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.ids), parameters.ids, 100, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings))
                {
                    return response;
                }

                if (parameters.ids.IsValid())
                {
                    parameters.after = null;
                    parameters.first = null;
                    parameters.ended_at = null;
                    parameters.started_at = null;
                }                

                if (!ValidateOptionalQueryParameter(nameof(parameters.ended_at), parameters.ended_at, DateTime.Now, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.started_at), parameters.started_at, DateTime.Now, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.started_at), parameters.started_at, parameters.ended_at, response, info.settings))
                {
                    return response;
                }

                // ended_at is ignored if no started_at is provided
                if (!parameters.started_at.HasValue)
                {
                    parameters.ended_at = null;
                }

                RestRequest request = GetBaseRequest("clips", Method.GET, info);
                request.AddParameters(parameters);

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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if no clip ID's, broadcaster ID, or game ID is provided.
            /// Thrown if any combination of clip ID's, broadcaster ID, or game ID is provided.            
            /// Thrown if the broadcaster ID or game ID is empty or contains only white space, if Provided.
            /// Thrown if any clip ID is null, empty, or contains only white space or any duplicate clip ID's are found, if provided.
            /// Thrown if the after or before cursors are empty or contain only white space, if provided.
            /// Thrown if both after and before cursors are provided.
            /// Thrown if started_at or ended_at is later than <see cref="DateTime.Now"/>, or started_at is later than ended_at, if provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if more than 100 total clip ID's are provided.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<Clip>>>
            GetClipsAsync(HelixInfo info, ClipsParameters parameters)
            {
                HelixResponse<DataPage<Clip>> response = new HelixResponse<DataPage<Clip>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!parameters.ids.IsValid() && parameters.broadcaster_id.IsNull() && parameters.game_id.IsNull())
                {
                    response.SetInputError(new QueryParameterException("At least one or more clip ID, one broadcaster ID, or one game ID must be provided."), info.settings);

                    return response;
                }

                if ((parameters.ids.IsValid() && (!parameters.broadcaster_id.IsNull() || !parameters.game_id.IsNull())) ||
                   (!parameters.broadcaster_id.IsNull() && !parameters.game_id.IsNull()))
                {
                    response.SetInputError(new QueryParameterException("Only one or more clip ID's, one broadcaster ID, or one game ID can be provided."), info.settings);

                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.game_id), parameters.game_id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.ids), parameters.ids, 100, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings))
                {
                    return response;
                }

                if (parameters.ids.IsValid())
                {
                    parameters.after = null;
                    parameters.first = null;
                    parameters.ended_at = null;
                    parameters.started_at = null;
                }

                if (!ValidateOptionalQueryParameter(nameof(parameters.ended_at), parameters.ended_at, DateTime.Now, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.started_at), parameters.started_at, DateTime.Now, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.started_at), parameters.started_at, parameters.ended_at, response, info.settings))
                {
                    return response;
                }

                // ended_at is ignored if no started_at is provided
                if (!parameters.started_at.HasValue)
                {
                    parameters.ended_at = null;
                }

                RestRequest request = GetBaseRequest("clips", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<Clip>> _response = await client.TraceExecuteAsync<Clip, DataPage<Clip>>(request, HandleResponse);
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
            /// <exception cref="HeaderParameterException">Thrown if the App Access token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the user ID is not provided, empty, or contains only white space.
            /// Thrown if any code is null, empty, or contains only white space, any duplicate codes are found, or any code does not match the regex: ^[a-zA-Z0-9]{5}-?[a-zA-Z0-9]{5}-?[a-zA-Z0-9]{5}$.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if none or more than 20 codes are provided.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<CodeStatus>>>
            GetEntitlementCodeStatusAsync(HelixInfo info, EntitlementsCodeParameters parameters)
            {
                HelixResponse<Data<CodeStatus>> response = new HelixResponse<Data<CodeStatus>>();
                if (!ValidateAuthorizatioHeaders(info, response, true))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if(!ValidateRequiredQueryParameter(nameof(parameters.user_id), parameters.user_id, response, info.settings) ||
                   !ValidateRequiredQueryParameter(nameof(parameters.codes), parameters.codes, 20, response, info.settings))
                {
                    return response;
                }

                List<string> malformed_codes = new List<string>(parameters.codes.Count);

                Regex regex = new Regex(REGEX_PATTERN_ENTITLEMENT_CODE);
                foreach (string code in parameters.codes)
                {
                    if (regex.IsMatch(code))
                    {
                        continue;
                    }

                    malformed_codes.Add(code);                   
                }

                if (malformed_codes.Count > 0)
                {
                    FormatException inner_exception = new FormatException("Values must match the regular expression: " + REGEX_PATTERN_ENTITLEMENT_CODE);

                    string _codes = string.Join(", ", malformed_codes);

                    string message = "One or more codes were malformed. Each code must be a 15 character alpha-numeric string, with optional dashes after every 5th character. Example: ABCDE-12345-F6G7H" + Environment.NewLine + Environment.NewLine +
                                     "Codes: " + _codes;
                    response.SetInputError(new QueryParameterException(message, nameof(parameters.codes), _codes, inner_exception), info.settings);

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
            /// <exception cref="HeaderParameterException">Thrown if the App Access token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the user ID is not provided, empty, or contains only white space.
            /// Thrown if any code is null, empty, or contains only white space, any duplicate codes are found, or any code does not match the regex: ^[a-zA-Z0-9]{5}-?[a-zA-Z0-9]{5}-?[a-zA-Z0-9]{5}$.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if none or more than 20 codes are provided.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<CodeStatus>>>
            RedeemEntitlementCodeStatusAsync(HelixInfo info, EntitlementsCodeParameters parameters)
            {
                HelixResponse<Data<CodeStatus>> response = new HelixResponse<Data<CodeStatus>>();
                if (!ValidateAuthorizatioHeaders(info, response, true))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!ValidateRequiredQueryParameter(nameof(parameters.user_id), parameters.user_id, response, info.settings) ||
                   !ValidateRequiredQueryParameter(nameof(parameters.codes), parameters.codes, 20, response, info.settings))
                {
                    return response;
                }

                List<string> malformed_codes = new List<string>(parameters.codes.Count);

                Regex regex = new Regex(REGEX_PATTERN_ENTITLEMENT_CODE);
                foreach (string code in parameters.codes)
                {
                    if (regex.IsMatch(code))
                    {
                        continue;
                    }

                    malformed_codes.Add(code);
                }

                if (malformed_codes.Count > 0)
                {
                    FormatException inner_exception = new FormatException("Values must match the regular expression: " + REGEX_PATTERN_ENTITLEMENT_CODE);

                    string _codes = string.Join(", ", malformed_codes);

                    string message = "One or more codes were malformed. Each code must be a 15 character alpha-numeric string, with optional dashes after every 5th character. Example: ABCDE-12345-F6G7H" + Environment.NewLine + Environment.NewLine +
                                     "Codes: " + _codes;
                    response.SetInputError(new QueryParameterException(message, nameof(parameters.codes), _codes, inner_exception), info.settings);

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
            /// <exception cref="HeaderParameterException">Thrown if the App Access token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the manifest ID is not provided, empty, or contains only white space.
            /// Thrown if the manifest ID is longer than 64 characters.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<EntitlementUploadUrl>>>
            CreateEntitlementGrantsUploadUrlAsync(HelixInfo info, EntitlementsUploadParameters parameters)
            {
                HelixResponse<Data<EntitlementUploadUrl>> response = new HelixResponse<Data<EntitlementUploadUrl>>();
                if (!ValidateAuthorizatioHeaders(info, response, true))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if(!ValidateRequiredQueryParameter(nameof(parameters.manifest_id), parameters.manifest_id, response, info.settings))
                {
                    return response;
                }

                if (!parameters.manifest_id.Length.IsInRange(1, 64))
                {
                    response.SetInputError(new QueryParameterException("The manifest ID must be between 1 and 64 characters long, inclusive.", nameof(parameters.manifest_id)), info.settings);

                    return response;
                }

                RestRequest request = GetBaseRequest("entitlements/upload", Method.POST, info);
                request.AddParameters(parameters);

                RestResponse<Data<EntitlementUploadUrl>> _response = await client.ExecuteAsync<Data<EntitlementUploadUrl>>(request, HandleResponse);
                response = new HelixResponse<Data<EntitlementUploadUrl>>(_response);

                return response;
            }

            #endregion

            #region /extensions/transactions

            /// <summary>
            /// <para>Asynchronously gets specific extension transactions or a single page of extension transactions.</para>
            /// <para>Required Authorization: App Access Token.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific extension transactions or the single page of extension transactions.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the App Access token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the extension ID is not provided, empty, or contains only white space.
            /// Thrown if any transaction ID is null, empty, or contains only white space, any duplicate codes are found, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if more than 100 transaction ID's are provided.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<ExtensionTransaction>>>
            GetExtensionTransactionsPageAsync(HelixInfo info, ExtensionTransactionsParameters parameters)
            {
                HelixResponse<DataPage<ExtensionTransaction>> response = new HelixResponse<DataPage<ExtensionTransaction>>();
                if (!ValidateAuthorizatioHeaders(info, response, true))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if(!ValidateRequiredQueryParameter(nameof(parameters.extension_id), parameters.extension_id, response, info.settings))
                {
                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.ids), parameters.ids, 100, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("extensions/transactions", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<ExtensionTransaction>> _response = await client.ExecuteAsync<DataPage<ExtensionTransaction>>(request, HandleResponse);
                response = new HelixResponse<DataPage<ExtensionTransaction>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously gets specific extension transactions or a complete list of extension transactions.</para>
            /// <para>Required Authorization: App Access Token.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific extension transactions or the complete list of extension transactions.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the App Access token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the extension ID is not provided, empty, or contains only white space.
            /// Thrown if any transaction ID is null, empty, or contains only white space, any duplicate codes are found, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if more than 100 transaction ID's are provided.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<ExtensionTransaction>>>
            GetExtensionTransactionsAsync(HelixInfo info, ExtensionTransactionsParameters parameters)
            {
                HelixResponse<DataPage<ExtensionTransaction>> response = new HelixResponse<DataPage<ExtensionTransaction>>();
                if (!ValidateAuthorizatioHeaders(info, response, true))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!ValidateRequiredQueryParameter(nameof(parameters.extension_id), parameters.extension_id, response, info.settings))
                {
                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.ids), parameters.ids, 100, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("extensions/transactions", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<ExtensionTransaction>> _response = await client.TraceExecuteAsync<ExtensionTransaction, DataPage<ExtensionTransaction>>(request, HandleResponse);
                response = new HelixResponse<DataPage<ExtensionTransaction>>(_response);

                return response;
            }

            #endregion

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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if any game ID or game name is null, empty, or contains only white space.
            /// Thrown if any duplicate game ID's or game names are found.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if none or more than 100 total game ID's and/or game names are provided.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<Game>>>
            GetGamesAsync(HelixInfo info, GamesParameters parameters)
            {
                HelixResponse<Data<Game>> response = new HelixResponse<Data<Game>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!parameters.ids.IsValid() && !parameters.names.IsValid())
                {
                    response.SetInputError(new QueryParameterCountException("At least one game ID or game name must be provided.", 100, 0), info.settings);

                    return response;
                }

                int count = parameters.ids.Count + parameters.names.Count;
                if (count > 100)
                {
                    response.SetInputError(new QueryParameterCountException("A maximum of 100 total game ID's and/or names can be provided at one time.", 100, count), info.settings);

                    return response;
                }

                // This will perform count checks again, but they will never be triggered if we get this far.
                // This is really for checking for duplicates and no content indicies.
                if (!ValidateOptionalQueryParameter(nameof(parameters.ids), parameters.ids, 100, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.names), parameters.names, 100, response, info.settings))
                {
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the after or before cursors are empty or contain only white space, if provided.
            /// Thrown if both after and before cursors are provided.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<Game>>>
            GetTopGamesPageAsync(HelixInfo info, TopGamesParameters parameters)
            {
                HelixResponse<DataPage<Game>> response = new HelixResponse<DataPage<Game>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    parameters.first = parameters.first.Clamp(1, 100);

                    if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.before), parameters.before, response, info.settings) ||
                        !ValidateCursorDiection(parameters.after, parameters.before, response, info.settings, out string direction))
                    {
                        return response;
                    }
                }

                RestRequest request = GetBaseRequest("games/top", Method.GET, info);
                request.AddParameters(parameters);

                // TODO: GetTopGamesPageAsync(...) - Sanitize the list based on the game ID and return a distinct list.
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the after or before cursors are empty or contain only white space, if provided.
            /// Thrown if both after and before cursors are provided.
            /// </exception>
            /// <exception cref="NotSupportedException">
            /// Thrown if a before cursor is provided.
            /// This is a temporary error and will be removed once Twitch fixes reverse pagination.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<Game>>>
            GetTopGamesAsync(HelixInfo info, TopGamesParameters parameters)
            {
                HelixResponse<DataPage<Game>> response = new HelixResponse<DataPage<Game>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                string direction = "after";

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    parameters.first = parameters.first.Clamp(1, 100);

                    if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.before), parameters.before, response, info.settings) ||
                        !ValidateCursorDiection(parameters.after, parameters.before, response, info.settings, out direction))
                    {
                        return response;
                    }

                    // TODO: GetTopGamesAsync(...) - Temporarily disabling using 'before' while requesting all pages until it works properly. Reimplement 'before' when it works propery.
                    if (parameters.before.IsValid())
                    {
                        response.SetInputError(new NotSupportedException("The pagination direction 'before' is temporarily not supported. Following the cursor using 'before' returns incorrect results and does not work properly on Twitch's back end."), info.settings);

                        return response;
                    }
                }

                RestRequest request = GetBaseRequest("games/top", Method.GET, info);
                request.AddParameters(parameters);

                // TODO: GetTopGamesAsync(...) - Sanitize the list based on the game ID and return a distinct list.
                RestResponse<DataPage<Game>> _response = await client.TraceExecuteAsync<Game, DataPage<Game>>(request, direction, HandleResponse);
                response = new HelixResponse<DataPage<Game>>(_response);

                return response;
            }

            #endregion

            #region /moderation/banned

            /// <summary>
            /// <para>Asynchronously gets specific banned users or a single page of banned users for a given broadcaster.</para>
            /// <para>Required Scope: <see cref="Scopes.ModerationRead"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific banned users or the single page of banned users for the given broadcaster.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the broadcaster ID is not provided, empty, or contains only white space.
            /// Thrown if any user ID is null, empty, or contains only white space any duplicate user ID's are found, if provided.
            /// Thrown if the after or before cursor is empty or contains only white space, if provided.
            /// Thrown if both after and before cursors are provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if more than 100 user ID's are provided.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.ModerationRead"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<BannedUser>>>
            GetBannedUsersPageAsync(HelixInfo info, BannedUsersParameters parameters)
            {
                info.required_scopes = Scopes.ModerationRead;

                HelixResponse<DataPage<BannedUser>> response = new HelixResponse<DataPage<BannedUser>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                // Required parameter checks
                if (!ValidateRequiredQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, info.settings))
                {
                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.before), parameters.before, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.user_ids), parameters.user_ids, 100, response, info.settings) ||
                    !ValidateCursorDiection(parameters.after, parameters.before, response, info.settings, out string direction))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("moderation/banned", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<BannedUser>> _response = await client.ExecuteAsync<DataPage<BannedUser>>(request, HandleResponse);
                response = new HelixResponse<DataPage<BannedUser>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously gets specific banned users or a complete list of banned users for a given broadcaster.</para>
            /// <para>Required Scope: <see cref="Scopes.ModerationRead"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific banned users or the complete list of banned users for the given broadcaster.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the broadcaster ID is not provided, empty, or contains only white space.
            /// Thrown if any user ID is null, empty, or contains only white space any duplicate user ID's are found, if provided.
            /// Thrown if the after or before cursor is empty or contains only white space, if provided.
            /// Thrown if both after and before cursors are provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if more than 100 user ID's are provided.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.ModerationRead"/> scope.</exception>
            /// <exception cref="NotSupportedException">
            /// Thrown if a before cursor is provided.
            /// This is a temporary error and will be removed once Twitch fixes reverse pagination.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<BannedUser>>>
            GetBannedUsersAsync(HelixInfo info, BannedUsersParameters parameters)
            {
                info.required_scopes = Scopes.ModerationRead;

                HelixResponse<DataPage<BannedUser>> response = new HelixResponse<DataPage<BannedUser>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                // Required parameter checks
                if (!ValidateRequiredQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, info.settings))
                {
                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);                

                if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.before), parameters.before, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.user_ids), parameters.user_ids, 100, response, info.settings) ||
                    !ValidateCursorDiection(parameters.after, parameters.before, response, info.settings, out string direction))
                {
                    return response;
                }

                // TODO: GetBannedUsersAsync(...) - Temporarily disabling using 'before' while requesting all pages until it works properly. Reimplement 'before' when it works propery.
                if (parameters.before.IsValid())
                {
                    response.SetInputError(new NotSupportedException("The pagination direction 'before' is temporarily not supported. Following the cursor using 'before' returns incorrect results and does not work properly on Twitch's back end."), info.settings);

                    return response;
                }

                RestRequest request = GetBaseRequest("moderation/banned", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<BannedUser>> _response = await client.TraceExecuteAsync<BannedUser, DataPage<BannedUser>>(request, direction, HandleResponse);
                response = new HelixResponse<DataPage<BannedUser>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously checks to see if a user is banned by a broadcaster.</para>
            /// <para>Required Scope: <see cref="Scopes.ModerationRead"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="broadcaster_id">The ID of the broadcaster.</param>
            /// <param name="user_id">The ID of the user to check.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> is set true if the user is banned by the broadcaster, otherwise false.
            /// </returns>
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID or user ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<bool>>
            IsUserBannedAsync(HelixInfo info, string broadcaster_id, string user_id)
            {
                HelixResponse<bool> response = new HelixResponse<bool>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Explicity check for this out here since it's optional in the underlying wrapper.
                if (!ValidateRequiredQueryParameter(nameof(user_id), user_id, response, info.settings))
                {
                    return response;
                }

                BannedUsersParameters parameters = new BannedUsersParameters();
                parameters.broadcaster_id = broadcaster_id;
                parameters.user_ids.Add(user_id);

                IHelixResponse<DataPage<BannedUser>> _response = await GetBannedUsersPageAsync(info, parameters);

                bool result = _response.exception.IsNull() ? _response.result.data.IsValid() : false;
                response = new HelixResponse<bool>(_response, result);

                return response;
            }

            #endregion

            #region /moderation/banned/events

            /// <summary>
            /// <para>
            /// Asynchronously gets a specific banned event or a single page of banned events.
            /// A banned event occurs when a user is banned or unbanned.
            /// </para>
            /// <para>Required Scope: <see cref="Scopes.ModerationRead"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific banned event or the single page of banned events.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if neither broadcaster ID or event ID are provided or are provided at the same time.
            /// Thrown if the broadcaster ID is empty or contains only white space, if provided.
            /// Thrown if the event ID is empty or contains only white space, if provided.
            /// Thrown if any user ID is null, empty, or contains only white space or any duplicate user ID's are found, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if more than 100 user ID's are provided.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.ModerationRead"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<BannedEvent>>>
            GetBannedEventsPageAsync(HelixInfo info, BannedEventsParameters parameters)
            {
                info.required_scopes = Scopes.ModerationRead;

                HelixResponse<DataPage<BannedEvent>> response = new HelixResponse<DataPage<BannedEvent>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                // Required parameter checks
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

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.id), parameters.id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.user_ids), parameters.user_ids, 100, response, info.settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("moderation/banned/events", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<BannedEvent>> _response = await client.ExecuteAsync<DataPage<BannedEvent>>(request, HandleResponse);
                response = new HelixResponse<DataPage<BannedEvent>>(_response);

                return response;
            }

            /// <summary>
            /// <para>
            /// Asynchronously gets a specific banned event or complete list of banned events.
            /// A banned event occurs when a user is banned or unbanned.
            /// </para>
            /// <para>Required Scope: <see cref="Scopes.ModerationRead"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific banned event or the complete list of banned events.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if neither broadcaster ID or event ID are provided or are provided at the same time.
            /// Thrown if the broadcaster ID is empty or contains only white space, if provided.
            /// Thrown if the event ID is empty or contains only white space, if provided.
            /// Thrown if any user ID is null, empty, or contains only white space or any duplicate user ID's are found, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if more than 100 user ID's are provided.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.ModerationRead"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<BannedEvent>>>
            GetBannedEventsAsync(HelixInfo info, BannedEventsParameters parameters)
            {
                info.required_scopes = Scopes.ModerationRead;

                HelixResponse<DataPage<BannedEvent>> response = new HelixResponse<DataPage<BannedEvent>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                // Required parameter checks
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

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.id), parameters.id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.user_ids), parameters.user_ids, 100, response, info.settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("moderation/banned/events", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<BannedEvent>> _response = await client.TraceExecuteAsync<BannedEvent, DataPage<BannedEvent>>(request, HandleResponse);
                response = new HelixResponse<DataPage<BannedEvent>>(_response);

                return response;
            }

            #endregion

            #region /moderation/enforcements/status

            /// <summary>
            /// <para>Asynchronously checks to see of a chat message meets the AutoMod requirements to be posted in chat.</para>
            /// <para>Required Scope: <see cref="Scopes.ModerationRead"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the AutoMod status of each message on whether or not the message meets the requirements to be posted in chat.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID is not provided, empty, or contains only white space.</exception>            
            /// <exception cref="BodyParameterException">
            /// Thrown if data is not provided.
            /// Thrown if any AutoMod message is null.
            /// Thrown if any AutoMod message ID is not provided or duplicate message ID's are found.
            /// Thrown if any AutoMod message text is not provided, empty, or contains only white space.
            /// Thrown if any AutoMod user ID is not provided, empty, or contains only white space.
            /// </exception>
            /// <exception cref="BodyParameterCountException">Thrown if none or more than 100 AutoMod messages are provided.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.ModerationRead"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<AutoModMessageStatus>>>
            CheckAutoModMessageStatus(HelixInfo info, AutoModMessageStatusParameters parameters)
            {
                info.required_scopes = Scopes.ModerationRead;

                HelixResponse<Data<AutoModMessageStatus>> response = new HelixResponse<Data<AutoModMessageStatus>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                // Required parameter checks
                if (!ValidateRequiredQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, info.settings))
                {
                    return response;
                }

                // TODO: CheckAutoModMessageStatus(...) - See if the saved overhead by not making each check it's own function worth the bloat.
                if (parameters.data.IsNull())
                {
                    response.SetInputError(new BodyParameterException("A required parameter is missing: " + nameof(parameters.data).WrapQuotes(), nameof(parameters.data)), info.settings);

                    return response;
                }

                if (parameters.data.Count == 0)
                {
                    response.SetInputError(new BodyParameterCountException("At least one element must be provided for the parameter: " + nameof(parameters.data).WrapQuotes(), nameof(parameters.data), 100, parameters.data.Count), info.settings);

                    return response;
                }

                if (parameters.data.Count > 100)
                {
                    response.SetInputError(new BodyParameterCountException("A maximum of " + 100 + " elements can be provided at one time for the parameter: " + nameof(parameters.data).WrapQuotes(), nameof(parameters.data), 100, parameters.data.Count), info.settings);

                    return response;
                }                

                List<int> indicies_null = new List<int>();
                List<int> indicies_msg_ids_null = new List<int>();
                List<int> indicies_msg_text_no_content = new List<int>();
                List<int> indicies_user_ids_no_content = new List<int>();

                HashSet<string> msg_ids_hash = new HashSet<string>();
                HashSet<string> msg_ids_duplicates = new HashSet<string>();

                for (int index = 0; index < parameters.data.Count; ++index) 
                {
                    if (parameters.data[index] == null)
                    {
                        indicies_null.Add(index);

                        continue;
                    }

                    // No !HasContent() check because who's to say what the ID should be. That's up to the dev.
                    if (parameters.data[index].msg_id.IsNull())
                    {
                        indicies_msg_ids_null.Add(index);
                    }
                    else if(!msg_ids_hash.Add(parameters.data[index].msg_id))
                    {
                        msg_ids_duplicates.Add(parameters.data[index].msg_id);
                    }

                    if (!parameters.data[index].msg_text.HasContent())
                    {
                        indicies_msg_text_no_content.Add(index);
                    }                    

                    if (!parameters.data[index].user_id.HasContent())
                    {
                        indicies_user_ids_no_content.Add(index);
                    }
                }

                indicies_null.TrimExcess();
                indicies_msg_ids_null.TrimExcess();
                indicies_msg_text_no_content.TrimExcess();
                indicies_user_ids_no_content.TrimExcess();

                indicies_user_ids_no_content.TrimExcess();

                if (indicies_null.Count > 0)
                {
                    string _indicies = string.Join(", ", indicies_null);

                    string message = "One or more AutoMod messages were null parameter: " + nameof(parameters.data).WrapQuotes() + Environment.NewLine + Environment.NewLine +
                                     "Indicies: " + _indicies;
                    response.SetInputError(new BodyParameterException(message, nameof(parameters.data), _indicies), info.settings);

                    return response;
                }

                if (indicies_msg_ids_null.Count > 0)
                {
                    string _indicies = string.Join(", ", indicies_msg_ids_null);

                    string message = "One or more AutoMod messages had a \"msg_id\" that was null." + Environment.NewLine + Environment.NewLine +
                                     "Indicies : " + _indicies;
                    response.SetInputError(new BodyParameterException(message, "msg_id", _indicies), info.settings);

                    return response;
                }

                if (msg_ids_duplicates.Count > 0)
                {
                    string _duplicates = string.Join(", ", msg_ids_duplicates);

                    string message = "One or more AutoMod messages had a duplicate \"msg_id\"." + Environment.NewLine + Environment.NewLine +
                                     "Values : " + _duplicates;
                    response.SetInputError(new BodyParameterException(message, "msg_id", _duplicates), info.settings);

                    return response;
                }

                if (indicies_msg_text_no_content.Count > 0)
                {
                    string _indicies = string.Join(", ", indicies_msg_text_no_content);

                    string message = "One or more AutoMod messages had a \"msg_txt\" that was null, empty, or contained only white space." + Environment.NewLine + Environment.NewLine +
                                     "Indicies : " + _indicies;
                    response.SetInputError(new BodyParameterException(message, "msg_text", _indicies), info.settings);

                    return response;
                }

                if (indicies_user_ids_no_content.Count > 0)
                {
                    string _indicies = string.Join(", ", indicies_user_ids_no_content);

                    string message = "One or more AutoMod messages had a \"user_id\" that was null, empty, or contained only white space." + Environment.NewLine + Environment.NewLine +
                                     "Indicies : " + _indicies;
                    response.SetInputError(new BodyParameterException(message, "user_id", _indicies), info.settings);

                    return response;
                }

                RestRequest request = GetBaseRequest("moderation/enforcements/status", Method.POST, info);
                request.AddParameters(parameters);

                RestResponse<Data<AutoModMessageStatus>> _response = await client.ExecuteAsync<Data<AutoModMessageStatus>>(request, HandleResponse);
                response = new HelixResponse<Data<AutoModMessageStatus>>(_response);

                return response;
            }

            #endregion

            #region /moderation/moderators

            /// <summary>
            /// <para>Asynchronously gets specific moderators or a single page of moderators users for a given broadcaster.</para>
            /// <para>Required Scope: <see cref="Scopes.ModerationRead"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific moderators or the single page of moderators for the given broadcaster.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the broadcaster ID is not ptovided, empty, or contains only white space.
            /// Thrown if any user ID is null, empty, or contains only white space or any duplicate user ID's are found, if provided.
            /// Thrown if the after or before cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if more than 100 user ID's are provided.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.ModerationRead"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<Moderator>>>
            GetModeratorsPageAsync(HelixInfo info, ModeratorsParameters parameters)
            {
                info.required_scopes = Scopes.ModerationRead;

                HelixResponse<DataPage<Moderator>> response = new HelixResponse<DataPage<Moderator>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                // Required parameter checks
                if (!ValidateRequiredQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, info.settings))
                {
                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.user_ids), parameters.user_ids, 100, response, info.settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("moderation/moderators", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<Moderator>> _response = await client.ExecuteAsync<DataPage<Moderator>>(request, HandleResponse);
                response = new HelixResponse<DataPage<Moderator>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously gets specific moderators or a complete list of moderators for a given broadcaster.</para>
            /// <para>Required Scope: <see cref="Scopes.ModerationRead"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific moderators or the complete list of moderators for the given broadcaster.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the broadcaster ID is not provided, empty, or contains only white space.
            /// Thrown if any user ID is null, empty, or contains only white space or any duplicate user ID's are found, if provided.
            /// Thrown if the after or before cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if more than 100 user ID's are provided.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.ModerationRead"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<Moderator>>>
            GetModeratorsAsync(HelixInfo info, ModeratorsParameters parameters)
            {
                info.required_scopes = Scopes.ModerationRead;

                HelixResponse<DataPage<Moderator>> response = new HelixResponse<DataPage<Moderator>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                // Required parameter checks
                if (!ValidateRequiredQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, info.settings))
                {
                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.user_ids), parameters.user_ids, 100, response, info.settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("moderation/moderators", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<Moderator>> _response = await client.TraceExecuteAsync<Moderator, DataPage<Moderator>>(request, HandleResponse);
                response = new HelixResponse<DataPage<Moderator>>(_response);

                return response;
            }

            /// <summary>
            /// Asynchronously checks to see if a user is a moderator for a given broadcaster.
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="broadcaster_id">The ID of the broadcaster.</param>
            /// <param name="user_id">The ID of the user to check.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> is set true if the user is a moderator for the given broadcaster, otherwise false.
            /// </returns>
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID or user ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<bool>>
            IsUserModeratorAsync(HelixInfo info, string broadcaster_id, string user_id)
            {
                HelixResponse<bool> response = new HelixResponse<bool>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Explicity check for this out here since it's optional in the underlying wrapper.
                if (!ValidateRequiredQueryParameter(nameof(user_id), user_id, response, info.settings))
                {
                    return response;
                }

                ModeratorsParameters parameters = new ModeratorsParameters();
                parameters.broadcaster_id = broadcaster_id;
                parameters.user_ids.Add(user_id);

                IHelixResponse<DataPage<Moderator>> _response = await GetModeratorsPageAsync(info, parameters);

                bool result = _response.exception.IsNull() ? _response.result.data.IsValid() : false;
                response = new HelixResponse<bool>(_response, result);

                return response;
            }

            #endregion

            #region /moderation/moderators/events

            /// <summary>
            /// <para>
            /// Asynchronously gets a specific moderator event or a single page of moderator events.
            /// A modewrator event occurs when a user gains or loses moderator (OP) status.
            /// </para>
            /// <para>Required Scope: <see cref="Scopes.ModerationRead"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific moderator event or the single page of moderator events.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if neither broadcaster ID or event ID are provided or are provided at the same time.
            /// Thrown if the broadcaster ID is empty or contains only white space, if provided.
            /// Thrown if the event ID is empty or contains only white space, if provided.
            /// Thrown if any user ID is null, empty, or contains only white space or any duplicate user ID's are found, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if more than 100 user ID's are provided.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.ModerationRead"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<ModeratorEvent>>>
            GetModeratorEventsPageAsync(HelixInfo info, ModeratorEventsParameters parameters)
            {
                info.required_scopes = Scopes.ModerationRead;

                HelixResponse<DataPage<ModeratorEvent>> response = new HelixResponse<DataPage<ModeratorEvent>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                // Required parameter checks
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

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.id), parameters.id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.user_ids), parameters.user_ids, 100, response, info.settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("moderation/moderators/events", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<ModeratorEvent>> _response = await client.ExecuteAsync<DataPage<ModeratorEvent>>(request, HandleResponse);
                response = new HelixResponse<DataPage<ModeratorEvent>>(_response);

                return response;
            }

            /// <summary>
            /// <para>
            /// Asynchronously gets a specific moderator event or complete list of moderator events.
            /// A modewrator event occurs when a user gains or loses moderator (OP) status.
            /// </para>
            /// <para>Required Scope: <see cref="Scopes.ModerationRead"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific moderator event or the complete list of moderator events.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if neither broadcaster ID or event ID are provided or are provided at the same time.
            /// Thrown if the broadcaster ID is empty or contains only white space, if provided.
            /// Thrown if the event ID is empty or contains only white space, if provided.
            /// Thrown if any user ID is empty or contains only white space or any duplicate user ID's are found, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if more than 100 user ID's are provided.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.ModerationRead"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<ModeratorEvent>>>
            GetModeratorEventsAsync(HelixInfo info, ModeratorEventsParameters parameters)
            {
                info.required_scopes = Scopes.ModerationRead;

                HelixResponse<DataPage<ModeratorEvent>> response = new HelixResponse<DataPage<ModeratorEvent>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                // Required parameter checks
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

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.id), parameters.id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.user_ids), parameters.user_ids, 100, response, info.settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("moderation/moderators/events", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<DataPage<ModeratorEvent>> _response = await client.TraceExecuteAsync<ModeratorEvent, DataPage<ModeratorEvent>>(request, HandleResponse);
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if any game ID is null, empty, or contains only white space or any duplicate game ID's are found, if provided.
            /// Thrown if any user ID is null, empty, or contains only white space or any duplicate user ID's are found, if provided.
            /// Thrown if any user login is null, empty, or contains only white space or any duplicate user logins are found, if provided.
            /// Thrown if the after or before cursors are empty or contain only white space, if provided.
            /// Thrown if both after and before cursors are provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">
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
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    parameters.first = parameters.first.Clamp(1, 100);

                    if (!ValidateOptionalQueryParameter(nameof(parameters.game_ids), parameters.game_ids, 100, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.user_ids), parameters.user_ids, 100, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.user_logins), parameters.user_logins, 100, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.before), parameters.before, response, info.settings) ||
                        !ValidateCursorDiection(parameters.after, parameters.before, response, info.settings, out string direction))
                    {
                        return response;
                    }
                }

                RestRequest request = GetBaseRequest("streams", Method.GET, info);
                request.AddParameters(parameters);

                // TODO: GetStreamsPageAsync(...) - Sanitize the list based on the stream ID and return a distinct list.
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if any game ID is null, empty, or contains only white space or any duplicate game ID's are found, if provided.
            /// Thrown if any user ID is null, empty, or contains only white space or any duplicate user ID's are found, if provided.
            /// Thrown if any user login is null, empty, or contains only white space or any duplicate user logins are found, if provided.
            /// Thrown if the after or before cursors are empty or contain only white space, if provided.
            /// Thrown if both after and before cursors are provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">
            /// Thrown if more than 100 total game ID's are provided.
            /// Thrown if more than 100 total user ID's are provided.
            /// Thrown if more than 100 total user logins are provided.
            /// </exception>
            /// <exception cref="NotSupportedException">
            /// Thrown if a before cursor is provided.
            /// This is a temporary error and will be removed once Twitch fixes reverse pagination.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<Stream>>>
            GetStreamsAsync(HelixInfo info, StreamsParameters parameters)
            {
                HelixResponse<DataPage<Stream>> response = new HelixResponse<DataPage<Stream>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }   

                string direction = "after";

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    parameters.first = parameters.first.Clamp(1, 100);

                    if (!ValidateOptionalQueryParameter(nameof(parameters.game_ids), parameters.game_ids, 100, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.user_ids), parameters.user_ids, 100, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.user_logins), parameters.user_logins, 100, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.before), parameters.before, response, info.settings) ||
                        !ValidateCursorDiection(parameters.after, parameters.before, response, info.settings, out direction))
                    {
                        return response;
                    }

                    // TODO: /streams - GetStreamsAsync(...) - Temporarily disabling using 'before' while requesting all pages until it works properly. Reimplement 'before' when it works propery.
                    if (parameters.before.IsValid())
                    {
                        response.SetInputError(new NotSupportedException("The pagination direction 'before' is temporarily not supported. Following the cursor using 'before' returns incorrect results and does not work properly on Twitch's back end."), info.settings);

                        return response;
                    }
                }

                RestRequest request = GetBaseRequest("streams", Method.GET, info);
                request.AddParameters(parameters);

                // TODO: GetStreamsAsync(...) - Sanitize the list based on the stream ID and return a distinct list.
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">Thrown if the user ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<bool>>
            IsStreamLiveByUserIDAsync(HelixInfo info, string user_id)
            {
                HelixResponse<bool> response = new HelixResponse<bool>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                if(!ValidateRequiredQueryParameter(nameof(user_id), user_id, response, info.settings))
                {
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">Thrown if the user login is not provided, empty, or contains only white space.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<bool>>
            IsStreamLiveByUserLoginAsync(HelixInfo info, string user_login)
            {
                HelixResponse<bool> response = new HelixResponse<bool>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                if (!ValidateRequiredQueryParameter(nameof(user_login), user_login, response, info.settings))
                {
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the user ID not provided, empty, or contains only white space.
            /// Thrown if the description is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.UserEditBroadcast"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<CreatedStreamMarker>>>
            CreateStreamMarkerAsync(HelixInfo info, CreateStreamMarkerParameters parameters)
            {
                info.required_scopes = Scopes.UserEditBroadcast;

                HelixResponse<DataPage<CreatedStreamMarker>> response = new HelixResponse<DataPage<CreatedStreamMarker>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!ValidateRequiredBodyParameter(nameof(parameters.user_id), parameters.user_id, response, info.settings))
                {
                    return response;
                }

                // Optional parameter checks
                if (!ValidateOptionalBodyParameter(nameof(parameters.description), parameters.description, response, info.settings))
                {
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if neither user ID or video ID are provided or are provided at the same time.
            /// Thrown if the user ID or video ID is empty or contains only white space, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
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
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
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

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.user_id), parameters.user_id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.video_id), parameters.video_id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings))
                {
                    return response;
                }

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
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if neither user ID or video ID are provided or are provided at the same time.
            /// Thrown if the user ID or video ID is empty or contains only white space, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
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
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
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

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.user_id), parameters.user_id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.video_id), parameters.video_id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings))
                {
                    return response;
                }

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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if any game ID is null, empty, or contains only white space or any duplicate game ID's are found, if provided.
            /// Thrown if any user ID is null, empty, or contains only white space or any duplicate user ID's are found, if provided.
            /// Thrown if any user login is null, empty, or contains only white space or any duplicate user logins are found, if provided.
            /// Thrown if the after or before cursors are empty or contain only white space, if provided.
            /// Thrown if both after and before cursors are provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">
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
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    parameters.first = parameters.first.Clamp(1, 100);

                    if (!ValidateOptionalQueryParameter(nameof(parameters.game_ids), parameters.game_ids, 100, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.user_ids), parameters.user_ids, 100, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.user_logins), parameters.user_logins, 100, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.before), parameters.before, response, info.settings) ||
                        !ValidateCursorDiection(parameters.after, parameters.before, response, info.settings, out string direction))
                    {
                        return response;
                    }
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if any game ID is null, empty, or contains only white space or any duplicate game ID's are found, if provided.
            /// Thrown if any user ID is null, empty, or contains only white space or any duplicate user ID's are found, if provided.
            /// Thrown if any user login is null, empty, or contains only white space or any duplicate user logins are found, if provided.
            /// Thrown if the after or before cursors are empty or contain only white space, if provided.
            /// Thrown if both after and before cursors are provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">
            /// Thrown if more than 100 total game ID's are provided.
            /// Thrown if more than 100 total user ID's are provided.
            /// Thrown if more than 100 total user logins are provided.
            /// </exception>
            /// <exception cref="NotSupportedException">
            /// Thrown if a before cursor is provided.
            /// This is a temporary error and will be removed once Twitch fixes reverse pagination.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<StreamMetadata>>>
            GetStreamsMetadataAsync(HelixInfo info, StreamsParameters parameters)
            {
                HelixResponse<DataPage<StreamMetadata>> response = new HelixResponse<DataPage<StreamMetadata>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                string direction = "after";

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    parameters.first = parameters.first.Clamp(1, 100);

                    if (!ValidateOptionalQueryParameter(nameof(parameters.game_ids), parameters.game_ids, 100, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.user_ids), parameters.user_ids, 100, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.user_logins), parameters.user_logins, 100, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.before), parameters.before, response, info.settings) ||
                        !ValidateCursorDiection(parameters.after, parameters.before, response, info.settings, out direction))
                    {
                        return response;
                    }

                    // TODO: GetStreamsMetadataAsync(...) - Temporarily disabling using 'before' while requesting all pages until it works properly. Reimplement 'before' when it works propery.
                    if (parameters.before.IsValid())
                    {
                        response.SetInputError(new NotSupportedException("The pagination direction 'before' is temporarily not supported. Following the cursor using 'before' returns incorrect results and does not work properly on Twitch's back end."), info.settings);

                        return response;
                    }
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<StreamTag>>>
            GetStreamsTagsAsync(HelixInfo info, StreamsTagsParameters parameters)
            {
                HelixResponse<Data<StreamTag>> response = new HelixResponse<Data<StreamTag>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if(!ValidateRequiredQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, info.settings))
                {
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="BodyParameterException">Thrown if any tag ID is null, empty, or contains only white space or any duplicate tag ID's are found.</exception>
            /// <exception cref="BodyParameterCountException">Thrown if more than 5 total tag ID's are provided.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.UserEditBroadcast"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse>
            SetStreamsTagsAsync(HelixInfo info, SetStreamsTagsParameters parameters)
            {
                info.required_scopes = Scopes.UserEditBroadcast;

                HelixResponse<Data<StreamTag>> response = new HelixResponse<Data<StreamTag>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!ValidateRequiredQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, info.settings))
                {
                    return response;
                }

                // Optional parameter checks
                if(!ValidateOptionalBodyParameter(nameof(parameters.tag_ids), parameters.tag_ids, 5, response, info.settings))
                {
                    return response;
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.UserEditBroadcast"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse>
            RemoveStreamsTagsAsync(HelixInfo info, SetStreamsTagsParameters parameters)
            {
                info.required_scopes = Scopes.UserEditBroadcast;

                HelixResponse<Data<StreamTag>> response = new HelixResponse<Data<StreamTag>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!ValidateRequiredQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, info.settings))
                {
                    return response;
                }

                // Optional parameter "checks"
                parameters.tag_ids = null;

                RestRequest request = GetBaseRequest("streams/tags", Method.PUT, info);
                request.AddParameters(parameters);

                RestResponse<Data<StreamTag>> _response = await client.ExecuteAsync<Data<StreamTag>>(request, HandleResponse);
                response = new HelixResponse<Data<StreamTag>>(_response);

                return response;
            }

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
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the broadcaster ID is not provided, empty, or contains only white space.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.ChannelReadSubscriptions"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<Subscription>>>
            GetBroadcasterSubscribersPageAsync(HelixInfo info, SubscriptionParameters parameters)
            {
                info.required_scopes = Scopes.ChannelReadSubscriptions;

                HelixResponse<DataPage<Subscription>> response = new HelixResponse<DataPage<Subscription>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!ValidateRequiredQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, info.settings))
                {
                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings))
                {
                    return response;
                }

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
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the broadcaster ID is not provided, empty, or contains only white space.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.ChannelReadSubscriptions"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<Subscription>>>
            GetBroadcasterSubscribersAsync(HelixInfo info, SubscriptionParameters parameters)
            {
                info.required_scopes = Scopes.ChannelReadSubscriptions;

                HelixResponse<DataPage<Subscription>> response = new HelixResponse<DataPage<Subscription>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!ValidateRequiredQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, info.settings))
                {
                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings))
                {
                    return response;
                }

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
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the broadcaster ID is not provided, empty, or contains only white space.
            /// Thrown if any user ID is null, empty, or contains only white space or any duplicate user ID's are found.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if none or more than 100 total user ID's are provided.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.ChannelReadSubscriptions"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<Subscription>>>
            GetSubscriptionRelationshipAsync(HelixInfo info, SubscriptionRelationshipParameters parameters)
            {
                info.required_scopes = Scopes.ChannelReadSubscriptions;

                HelixResponse<Data<Subscription>> response = new HelixResponse<Data<Subscription>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!ValidateRequiredQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, info.settings) ||
                    !ValidateRequiredQueryParameter(nameof(parameters.user_ids), parameters.user_ids, 100, response, info.settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("subscriptions", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<Data<Subscription>> _response = await client.ExecuteAsync<Data<Subscription>>(request, HandleResponse);
                response = new HelixResponse<Data<Subscription>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously checks to see if the from_id user is following the to_id user.</para>
            /// <para>Required Scope: <see cref="Scopes.ChannelReadSubscriptions"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="broadcaster_id">The user ID of the broadcaster.</param>
            /// <param name="user_id">The ID of the possibly subscribed user.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> is set true if from_id is following to_id, otherwise false.
            /// </returns>
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID or user ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<bool>>
            IsUserSubscribedAsync(HelixInfo info, string broadcaster_id, string user_id)
            {
                HelixResponse<bool> response = new HelixResponse<bool>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks

                // No need to check broadcaster ID here since it's checked later before the request is executed.
                if (!ValidateRequiredQueryParameter(nameof(user_id), user_id, response, info.settings))
                {
                    return response;
                }

                SubscriptionRelationshipParameters parameters = new SubscriptionRelationshipParameters();
                parameters.broadcaster_id = broadcaster_id;
                parameters.user_ids.Add(user_id);

                IHelixResponse<Data<Subscription>> _response = await GetSubscriptionRelationshipAsync(info, parameters);

                bool result = _response.exception.IsNull() ? _response.result.data.IsValid() : false;
                response = new HelixResponse<bool>(_response, result);

                return response;
            }

            #endregion

            #region /subscriptions/events

            /// <summary>
            /// <para>
            /// Asynchronously gets a specific subscription event or a single page of subscription events over the last 5 days.
            /// A subscription event occurs when a user subscribed, unsubscribes, or send a notification message in chat.
            /// </para>
            /// <para>Required Scope: <see cref="Scopes.ChannelReadSubscriptions"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific subscription event or the single page of subscription events.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if neither broadcaster ID or event ID are provided or are provided at the same time.
            /// Thrown if the broadcaster ID is empty or contains only white space, if provided.
            /// Thrown if the event ID is empty or contains only white space, if provided.
            /// Thrown if the user ID is empty or contains only white space, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
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
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                // Required parameter checks 
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

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (parameters.user_id.HasContent())
                {
                    // Pagination is ignored if a user ID was provided.
                    parameters.after = null;
                }

                if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.id), parameters.id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.user_id), parameters.user_id, response, info.settings))
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
            /// <para>
            /// Asynchronously gets a specific subscription event or a complete list of subscription events.
            /// A subscription event occurs when a user subscribed, unsubscribes, or send a notification message in chat.
            /// </para>
            /// <para>Required Scope: <see cref="Scopes.ChannelReadSubscriptions"/>.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific subscription event or the complete list of subscription events.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token is null, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if neither broadcaster ID or event ID are provided or are provided at the same time.
            /// Thrown if the broadcaster ID is empty or contains only white space, if provided.
            /// Thrown if the event ID is empty or contains only white space, if provided.
            /// Thrown if the user ID is empty or contains only white space, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
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
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                // Required parameter checks 
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

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (parameters.user_id.HasContent())
                {
                    // Pagination is ignored if a user ID was provided.
                    parameters.after = null;
                }

                if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.id), parameters.id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.user_id), parameters.user_id, response, info.settings))
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if any tag ID is null, empty, or contains only white space or any duplicate tag ID's are found, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if more than 100 total tag ID's are provided.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<StreamTag>>>
            GetStreamTagsPageAsync(HelixInfo info, StreamTagsParameters parameters)
            {
                HelixResponse<DataPage<StreamTag>> response = new HelixResponse<DataPage<StreamTag>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    
                    if (!ValidateOptionalQueryParameter(nameof(parameters.tag_ids), parameters.tag_ids, 100, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings))
                    {
                        return response;
                    }

                    if (parameters.tag_ids.IsValid())
                    {
                        parameters.first = null;
                        parameters.after = null;
                    }
                    else
                    {
                        parameters.first = parameters.first.Clamp(1, 100);
                    }
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if any tag ID is null, empty, or contains only white space or any duplicate tag ID's are found, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if more than 100 total tag ID's are provided.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<StreamTag>>>
            GetStreamTagsAsync(HelixInfo info, StreamTagsParameters parameters)
            {
                HelixResponse<DataPage<StreamTag>> response = new HelixResponse<DataPage<StreamTag>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Optional parameter checks
                if (!parameters.IsNull())
                {

                    if (!ValidateOptionalQueryParameter(nameof(parameters.tag_ids), parameters.tag_ids, 100, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings))
                    {
                        return response;
                    }

                    if (parameters.tag_ids.IsValid())
                    {
                        parameters.first = null;
                        parameters.after = null;
                    }
                    else
                    {
                        parameters.first = parameters.first.Clamp(1, 100);
                    }
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if any user ID or user login is null, empty, or contains only white space.
            /// Thrown if any duplicate user ID's or user names are found.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if none or more than 100 total user ID's and/or user logins are provided.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<User>>>
            GetUsersAsync(HelixInfo info, UsersParameters parameters)
            {
                HelixResponse<Data<User>> response = new HelixResponse<Data<User>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
                if (info.bearer_token.IsNull() && parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException("Parameters must be provided when no Bearer token is provided.", nameof(parameters)), info.settings);

                    return response;
                }

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    if (!parameters.ids.IsValid() && !parameters.logins.IsValid())
                    {
                        response.SetInputError(new QueryParameterCountException("At least one user ID or user login must be provided.", 100, 0), info.settings);

                        return response;
                    }

                    int count = parameters.ids.Count + parameters.logins.Count;
                    if (count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException("A maximum of 100 total user ID's and/or logins can be provided at one time.", 100, count), info.settings);

                        return response;
                    }

                    // This will perform count checks again, but they will never be triggered if we get this far.
                    // This is really for checking for duplicates and no content indicies.
                    if (!ValidateOptionalQueryParameter(nameof(parameters.ids), parameters.ids, 100, response, info.settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.logins), parameters.logins, 100, response, info.settings))
                    {
                        return response;
                    }
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.UserEdit"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<User>>>
            SetUserDescriptionAsync(HelixInfo info, DescriptionParameters parameters)
            {
                info.required_scopes = Scopes.UserEdit;

                HelixResponse<Data<User>> response = new HelixResponse<Data<User>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                // Optional parameter checks
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">Thrown if the user ID is empty or contains only white space, if provided.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<ActiveExtensions>>
            GetUserActiveExtensionsAsync(HelixInfo info, ActiveExtensionsParameters parameters)
            {
                HelixResponse<ActiveExtensions> response = new HelixResponse<ActiveExtensions>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
                if (info.bearer_token.IsNull() && parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException("Parameters must be provided when no Bearer token is provided.", nameof(parameters)), info.settings);

                    return response;
                }

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    if (!ValidateOptionalQueryParameter(nameof(parameters.user_id), parameters.user_id, response, info.settings))
                    {
                        return response;
                    }
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="BodyParameterException">
            /// Thrown if no supported extension slots are found across all extension types.
            /// Thrown if thwe parameters data is null.
            /// Thrown if the extension ID or extension version for any active supported extension slot is null, empty, or contains only white space.            
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
                if (!ValidateAuthorizatioHeaders(info, response))
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
                    response.SetInputError(new BodyParameterException("Value cannot be null", nameof(parameters.data), parameters.data), info.settings);

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
            /// <exception cref="BodyParameterException">
            /// Thrown if the extension ID or extension version for any active supported extension slot is null, empty, or contains only white space.
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
                        response.SetInputError(new BodyParameterException("Value cannot be null, empty, or contain only whitespace.", nameof(extension.id), extension.id), settings);

                        return extensions;
                    }

                    if (!extension.version.IsValid())
                    {
                        response.SetInputError(new BodyParameterException("Value cannot be null, empty, or contain only whitespace.", nameof(extension.version), extension.version), settings);

                        return extensions;
                    }

                    // Twitch only cares about these when a component is being updated.
                    if (type == ExtensionType.Component)
                    {
                        if (extension.x.HasValue && (extension.x.Value < 0 || extension.x.Value > 8000))
                        {
                            response.SetInputError(new BodyParameterException("The x coordinate must be between 0 and 8000, inclusive.", nameof(extension.x), extension.x.Value), settings);

                            return extensions;
                        }

                        if (extension.y.HasValue && (extension.y.Value < 0 || extension.y.Value > 5000))
                        {
                            response.SetInputError(new BodyParameterException("The y coordinate must be between 0 and 8000, inclusive.", nameof(extension.y), extension.y.Value), settings);

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
            /// <exception cref="HeaderParameterException">
            /// Thrown if the Bearer token is not provided, empty, or contains only white space.
            /// Thrown if the Client ID is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="Scopes.UserReadBroadcast"/> scope</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<Extension>>>
            GetUserExtensionsAsync(HelixInfo info)
            {
                info.required_scopes = Scopes.UserReadBroadcast;

                HelixResponse<Data<Extension>> response = new HelixResponse<Data<Extension>>();
                if (!ValidateAuthorizatioHeaders(info, response))
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the from_id is not provided, empty, or contains only white space.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingPageAsync(HelixInfo info, FollowsParameters parameters)
            {
                HelixResponse<FollowsDataPage<Follow>> response = new HelixResponse<FollowsDataPage<Follow>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if(!ValidateRequiredQueryParameter(nameof(parameters.from_id), parameters.from_id, response, info.settings))
                {
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the from_id is not provided, empty, or contains only white space.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingAsync(HelixInfo info, FollowsParameters parameters)
            {
                HelixResponse<FollowsDataPage<Follow>> response = new HelixResponse<FollowsDataPage<Follow>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!ValidateRequiredQueryParameter(nameof(parameters.from_id), parameters.from_id, response, info.settings))
                {
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the to_id is not provided, empty, or contains only white space.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersPageAsync(HelixInfo info, FollowsParameters parameters)
            {
                HelixResponse<FollowsDataPage<Follow>> response = new HelixResponse<FollowsDataPage<Follow>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!ValidateRequiredQueryParameter(nameof(parameters.to_id), parameters.to_id, response, info.settings))
                {
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the to_id is not provided, empty, or contains only white space.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersAsync(HelixInfo info, FollowsParameters parameters)
            {
                HelixResponse<FollowsDataPage<Follow>> response = new HelixResponse<FollowsDataPage<Follow>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!ValidateRequiredQueryParameter(nameof(parameters.to_id), parameters.to_id, response, info.settings))
                {
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">Thrown if from_id or to_id are not provided, empty, or contains only white space.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<bool>>
            IsUserFollowingAsync(HelixInfo info, string from_id, string to_id)
            {
                HelixResponse<bool> response = new HelixResponse<bool>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
                if (!ValidateRequiredQueryParameter(nameof(to_id), to_id, response, info.settings) ||
                    !ValidateRequiredQueryParameter(nameof(from_id), from_id, response, info.settings))
                {
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if neither from_id and to_id are provided.
            /// Thrown if from_id or to_id are empty or contains only white space, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowsRelationshipPageAsync(HelixInfo info, FollowsParameters parameters)
            {
                HelixResponse<FollowsDataPage<Follow>> response = new HelixResponse<FollowsDataPage<Follow>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!parameters.to_id.IsValid() && !parameters.from_id.IsValid())
                {
                    response.SetInputError(new QueryParameterException("At minimum, a from_id or to_id must be provided and cannot be empty or contain only white space."), info.settings);

                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.to_id), parameters.to_id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.from_id), parameters.from_id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings))
                {
                    return response;
                }

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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if neither from_id and to_id are provided.
            /// Thrown if from_id or to_id are empty or contains only white space, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowsRelationshipAsync(HelixInfo info, FollowsParameters parameters)
            {
                HelixResponse<FollowsDataPage<Follow>> response = new HelixResponse<FollowsDataPage<Follow>>();
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!parameters.to_id.IsValid() && !parameters.from_id.IsValid())
                {
                    response.SetInputError(new QueryParameterException("At minimum, a from_id or to_id must be provided and cannot be empty or contain only white space."), info.settings);

                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.to_id), parameters.to_id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.from_id), parameters.from_id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings))
                {
                    return response;
                }

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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if after and before are provided.
            /// Thrown if no video ID's, game ID, or user ID are provided.
            /// Thrown if any multiple combination of video ID's, game ID, or user ID is provided.
            /// Thrown if the user ID is null, empty, or contains only white space, if Provided.
            /// Thrown if the game ID is null, empty, or contains only white space, if Provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">            
            /// Thrown if all video ID's are are null, empty, or contains only white space, if Provided.
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
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!parameters.ids.IsValid() && parameters.user_id.IsNull() && parameters.game_id.IsNull())
                {
                    response.SetInputError(new QueryParameterException("At least one or more video ID, one user ID, or one game ID must be provided."), info.settings);

                    return response;
                }

                if ((parameters.ids.IsValid() && (!parameters.user_id.IsNull() || !parameters.game_id.IsNull())) ||
                   (!parameters.user_id.IsNull() && !parameters.game_id.IsNull()))
                {
                    response.SetInputError(new QueryParameterException("Only one or more video ID's, one user ID, or one game ID can be provided."), info.settings);

                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.user_id), parameters.user_id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.game_id), parameters.game_id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.ids), parameters.ids, 100, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.before), parameters.before, response, info.settings) ||
                    !ValidateCursorDiection(parameters.after, parameters.before, response, info.settings, out string direction))
                {
                    return response;
                }

                if (parameters.ids.IsValid())
                {
                    parameters.after    = null;
                    parameters.before   = null;
                    parameters.first    = null;
                    parameters.language = null;
                    parameters.period   = null;
                    parameters.type     = null;
                }

                RestRequest request = GetBaseRequest("videos", Method.GET, info);
                request.AddParameters(parameters);

                // TODO: GetVideosPageAsync(...) - Resort the videos based on sort. Sometimes videos can be out of order.
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
            /// <exception cref="HeaderParameterException">
            /// Thrown if neither Bearer token or Client ID are provided.
            /// Thrown if the Bearer token or Client ID are empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if after and before are provided.</exception>
            /// Thrown if no video ID's, game ID, or user ID are provided.
            /// Thrown if any multiple combination of video ID's, game ID, or user ID is provided.
            /// Thrown if the user ID is null, empty, or contains only white space, if Provided.
            /// Thrown if the game ID is null, empty, or contains only white space, if Provided.
            /// <exception cref="QueryParameterCountException">            
            /// Thrown if all video ID's are are null, empty, or contains only white space, if Provided.
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
                if (!ValidateAuthorizatioHeaders(info, response))
                {
                    return response;
                }                

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), info.settings);

                    return response;
                }

                if (!parameters.ids.IsValid() && parameters.user_id.IsNull() && parameters.game_id.IsNull())
                {
                    response.SetInputError(new QueryParameterException("At least one or more video ID, one user ID, or one game ID must be provided."), info.settings);

                    return response;
                }

                if ((parameters.ids.IsValid() && (!parameters.user_id.IsNull() || !parameters.game_id.IsNull())) ||
                   (!parameters.user_id.IsNull() && !parameters.game_id.IsNull()))
                {
                    response.SetInputError(new QueryParameterException("Only one or more video ID's, one user ID, or one game ID can be provided."), info.settings);

                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.user_id), parameters.user_id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.game_id), parameters.game_id, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.ids), parameters.ids, 100, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.before), parameters.before, response, info.settings) ||
                    !ValidateCursorDiection(parameters.after, parameters.before, response, info.settings, out string direction))
                {
                    return response;
                }

                // TODO: GetVideosAsync(...) - Temporarily disabling using 'before' while requesting all pages until it works properly. Reimplement 'before' when it works propery.
                if (parameters.before.IsValid())
                {
                    response.SetInputError(new NotSupportedException("The pagination direction 'before' is temporarily not supported. Following the cursor using 'before' returns incorrect results and does not work properly on Twitch's back end."), info.settings);

                    return response;
                }

                if (parameters.ids.IsValid())
                {
                    parameters.after = null;
                    parameters.before = null;
                    parameters.first = null;
                    parameters.language = null;
                    parameters.period = null;
                    parameters.type = null;
                }

                RestRequest request = GetBaseRequest("videos", Method.GET, info);
                request.AddParameters(parameters);

                // TODO: GetVideosAsync(...) - Resort the videos based on sort. Sometimes videos can be out of order.
                RestResponse<DataPage<Video>> _response = await client.TraceExecuteAsync<Video, DataPage<Video>>(request, direction, HandleResponse);
                response = new HelixResponse<DataPage<Video>>(_response);

                return response;
            }

            #endregion

            // Move to Rest.Helix.WebHooks or keep it on the same level?
            // TODO: Implement /webhook/hub

            #region /webhooks/subscriptions

            /// <summary>
            /// <para>Asynchronously gets a single page of webhook subscriptions.</para>
            /// <para>Required Authorization: App Access Token.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page of webhook subscriptions.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the App Access token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">Thrown if the after cursor is empty or contains only white space, if provided.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<WebhookDataPage<WebhookSubscription>>>
            GetWebhookSubscriptionsPageAsync(HelixInfo info, PagingParameters parameters)
            {
                HelixResponse<WebhookDataPage<WebhookSubscription>> response = new HelixResponse<WebhookDataPage<WebhookSubscription>>();
                if (!ValidateAuthorizatioHeaders(info, response, true))
                {
                    return response;
                }

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    parameters.first = parameters.first.Clamp(1, 100);

                    if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings))
                    {
                        return response;
                    }
                }

                RestRequest request = GetBaseRequest("webhooks/subscriptions", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<WebhookDataPage<WebhookSubscription>> _response = await client.ExecuteAsync<WebhookDataPage<WebhookSubscription>>(request, HandleResponse);
                response = new HelixResponse<WebhookDataPage<WebhookSubscription>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously gets a complete list of webhook subscriptions.</para>
            /// <para>Required Authorization: App Access Token.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the complete list of webhook subscriptions.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the App Access token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">Thrown if the after cursor is empty or contains only white space, if provided.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<WebhookDataPage<WebhookSubscription>>>
            GetWebhookSubscriptionsAsync(HelixInfo info, PagingParameters parameters)
            {
                HelixResponse<WebhookDataPage<WebhookSubscription>> response = new HelixResponse<WebhookDataPage<WebhookSubscription>>();
                if (!ValidateAuthorizatioHeaders(info, response, true))
                {
                    return response;
                }

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    parameters.first = parameters.first.Clamp(1, 100);

                    if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, info.settings))
                    {
                        return response;
                    }
                }

                RestRequest request = GetBaseRequest("webhooks/subscriptions", Method.GET, info);
                request.AddParameters(parameters);

                RestResponse<WebhookDataPage<WebhookSubscription>> _response = await client.TraceExecuteAsync<WebhookSubscription, WebhookDataPage<WebhookSubscription>>(request, HandleResponse);
                response = new HelixResponse<WebhookDataPage<WebhookSubscription>>(_response);

                return response;
            }

            #endregion

            #region Helpers - Request Building

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
                request.AddHeader("Authorization", "Bearer " + info.bearer_token);
                request.AddHeader("Client-ID", info.client_id);
                request.settings = info.settings;

                return request;
            }

            #endregion

            #region Helpers - Request Validation 

            internal static bool
            ValidateAuthorizatioHeaders(HelixInfo info, HelixResponse response, bool app_access_required = false)
            {
                bool is_null_bearer = info.bearer_token.IsNull();
                bool is_null_client_id = info.client_id.IsNull();

                if(!is_null_bearer && info.bearer_token.IsEmptyOrWhiteSpace())
                {
                    response.SetInputError(new HeaderParameterException("Value cannot be empty or contain only whitespace.", nameof(info.bearer_token), info.bearer_token), info.settings);

                    return false;
                }

                if (!is_null_client_id && info.client_id.IsEmptyOrWhiteSpace())
                {
                    response.SetInputError(new HeaderParameterException("Value cannot be empty or contain only whitespace.", nameof(info.client_id), info.client_id), info.settings);

                    return false;
                }

                // An App Access Token is required.
                if (app_access_required)
                {
                    if (is_null_bearer)
                    {
                        response.SetInputError(new HeaderParameterException("An App Access Token must be provided to authenticate the request.", nameof(info.bearer_token)), info.settings);

                        return false;
                    }

                    if (is_null_client_id)
                    {
                        response.SetInputError(new HeaderParameterException("A Client ID must be provided when an App Access Token is required.", nameof(info.client_id)), info.settings);

                        return false;
                    }
                }

                // Some level of authorization is required.
                if (info.required_scopes != 0)
                {
                    // Bearer token was not provided.
                    if (is_null_bearer)
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
                // No authorixation is required so only a and only a Client ID is needed.
                // We don't care if a Bearer token is null.
                else if (is_null_bearer && is_null_client_id)
                {
                    response.SetInputError(new HeaderParameterException("A Client ID or Bearer token must be provided to authenticate the request."), info.settings);

                    return false;
                }

                return true;
            }            

            private static bool
            ValidateRequiredQueryParameter(string name, string value, HelixResponse response, HelixRequestSettings settings)
            {
                return ValidateQueryParameter(true, name, value, response, settings);
            }

            private static bool
            ValidateOptionalQueryParameter(string name, string value, HelixResponse response, HelixRequestSettings settings)
            {
                return ValidateQueryParameter(false, name, value, response, settings);
            }

            private static bool
            ValidateQueryParameter(bool required, string name, string value, HelixResponse response, HelixRequestSettings settings)
            {
                if (value.IsNull())
                {
                    if (required)
                    {
                        response.SetInputError(new QueryParameterException("A required parameter is missing: " + name.WrapQuotes(), name), settings);

                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

                if (value.IsEmptyOrWhiteSpace())
                {
                    response.SetInputError(new QueryParameterException("The parameter " + name.WrapQuotes() + " cannot be empty or contain only whitespace.", name, value), settings);

                    return false;
                }

                return true;
            }

            private static bool
            ValidateRequiredBodyParameter(string name, string value, HelixResponse response, HelixRequestSettings settings)
            {
                return ValidateBodyParameter(true, name, value, response, settings);
            }

            private static bool
            ValidateOptionalBodyParameter(string name, string value, HelixResponse response, HelixRequestSettings settings)
            {
                return ValidateBodyParameter(false, name, value, response, settings);
            }

            private static bool
            ValidateBodyParameter(bool required, string name, string value, HelixResponse response, HelixRequestSettings settings)
            {
                if (value.IsNull())
                {
                    if (required)
                    {
                        response.SetInputError(new BodyParameterException("A required parameter is missing: " + name.WrapQuotes(), name), settings);

                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

                if (value.IsEmptyOrWhiteSpace())
                {
                    response.SetInputError(new BodyParameterException("The parameter " + name.WrapQuotes() + " cannot be empty or contain only whitespace.", name, value), settings);

                    return false;
                }

                return true;
            }

            private static bool
            ValidateRequiredQueryParameter(string name, in DateTime? value, in DateTime? maximum, HelixResponse response, HelixRequestSettings settings)
            {
                return ValidateQueryParameter(true, name, in value, in maximum, response, settings);
            }

            private static bool
            ValidateOptionalQueryParameter(string name, in DateTime? value, in DateTime? maximum, HelixResponse response, HelixRequestSettings settings)
            {
                return ValidateQueryParameter(false, name, in value, in maximum, response, settings);
            }

            private static bool
            ValidateQueryParameter(bool required, string name, in DateTime? value, in DateTime? maximum, HelixResponse response, HelixRequestSettings settings)
            {
                DateTime? minimum = null;

                return ValidateQueryParameter(required, name, in value, in minimum, in maximum, response, settings);
            }

            private static bool
            ValidateQueryParameter(bool required, string name, in DateTime? value, in DateTime? minimum, in DateTime? maximum, HelixResponse response, HelixRequestSettings settings)
            {
                if (!value.HasValue)
                {
                    if (required)
                    {
                        response.SetInputError(new QueryParameterException("A required parameter is missing: " + name.WrapQuotes(), name), settings);

                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

                if(maximum.HasValue && value.Value > maximum.Value)
                {
                    response.SetInputError(new QueryParameterException("The parameter " + name.WrapQuotes() + " cannot be later than " + maximum.Value.ToLongDateString(), name, value.Value), settings);

                    return false;
                }

                if (minimum.HasValue && value.Value < maximum.Value)
                {
                    response.SetInputError(new QueryParameterException("The parameter " + name.WrapQuotes() + " cannot be earlier than " + maximum.Value.ToLongDateString(), name, value.Value), settings);

                    return false;
                }

                return true;
            }

            private static bool
            ValidateRequiredQueryParameter(string name, List<string> values, int maximum_count, HelixResponse response, HelixRequestSettings settings)
            {
                return ValidateQueryParameter(true, name, values, maximum_count, response, settings);
            }

            private static bool
            ValidateOptionalQueryParameter(string name, List<string> values, int maximum_count, HelixResponse response, HelixRequestSettings settings)
            {
                return ValidateQueryParameter(false, name, values, maximum_count, response, settings);
            }

            private static bool
            ValidateQueryParameter(bool required, string name, List<string> values, int maximum_count, HelixResponse response, HelixRequestSettings settings)
            {
                if (values == null || values.Count == 0)
                {
                    if (required)
                    {
                        response.SetInputError(new QueryParameterException("A required parameter is missing: " + name.WrapQuotes(), name), settings);

                        return false;
                    }
                    else
                    {
                        return true;
                    }                    
                }

                if (values.Count == 0)
                {
                    response.SetInputError(new QueryParameterCountException("At least one element must be provided for the parameter: " + name.WrapQuotes(), name, maximum_count, values.Count), settings);

                    return false;
                }

                if (values.Count > maximum_count)
                {
                    response.SetInputError(new QueryParameterCountException("A maximum of " + maximum_count + " elements can be provided at one time for the parameter: " + name.WrapQuotes(), name, maximum_count, values.Count), settings);

                    return false;
                }

                List<int> indicies = values.GetNoContentIndicies();
                if (indicies.Count > 0)
                {
                    string _indicies = string.Join(", ", indicies);

                    string message = "One or more elements were null, empty, or contained only white space for the parameter: " + name.WrapQuotes() + Environment.NewLine + Environment.NewLine +
                                     "Indicies: " + _indicies;
                    response.SetInputError(new QueryParameterException(message, name, _indicies), settings);

                    return false;
                }

                List<string> duplicates = values.GetDuplicateElements();
                if (duplicates.Count > 0)
                {
                    string _duplicates = string.Join(", ", duplicates);

                    string message = "One or more duplicate elements were found for the parameter: " + name.WrapQuotes() + Environment.NewLine + Environment.NewLine +
                                     "Duplicates : " + _duplicates;
                    response.SetInputError(new QueryParameterException(message, name, _duplicates), settings);

                    return false;
                }

                return true;
            }

            private static bool
            ValidateRequiredBodyParameter(string name, List<string> values, int maximum_count, HelixResponse response, HelixRequestSettings settings)
            {
                return ValidateBodyParameter(true, name, values, maximum_count, response, settings);
            }

            private static bool
            ValidateOptionalBodyParameter(string name, List<string> values, int maximum_count, HelixResponse response, HelixRequestSettings settings)
            {
                return ValidateBodyParameter(false, name, values, maximum_count, response, settings);
            }

            private static bool
            ValidateBodyParameter(bool required, string name, List<string> values, int maximum_count, HelixResponse response, HelixRequestSettings settings)
            {
                if (values == null || values.Count == 0)
                {
                    if (required)
                    {
                        response.SetInputError(new BodyParameterException("A required parameter is missing: " + name.WrapQuotes(), name), settings);

                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }                

                if (values.Count == 0)
                {
                    response.SetInputError(new BodyParameterCountException("At least one element must be provided for the parameter: " + name.WrapQuotes(), name, maximum_count, values.Count), settings);

                    return false;
                }

                if (values.Count > maximum_count)
                {
                    response.SetInputError(new BodyParameterCountException("A maximum of " + maximum_count + " elements can be provided at one time for the parameter: " + name.WrapQuotes(), name, maximum_count, values.Count), settings);

                    return false;
                }

                List<int> indicies = values.GetNoContentIndicies();
                if (indicies.Count > 0)
                {
                    string _indicies = string.Join(", ", indicies);

                    string message = "One or more elements were null, empty, or contained only white space for the parameter: " + name.WrapQuotes() + Environment.NewLine + Environment.NewLine +
                                     "Indicies: " + _indicies;
                    response.SetInputError(new BodyParameterException(message, name, _indicies), settings);

                    return false;
                }

                List<string> duplicates = values.GetDuplicateElements();
                if (duplicates.Count > 0)
                {
                    string _duplicates = string.Join(", ", duplicates);

                    string message = "One or more duplicate elements were found for the parameter: " + name.WrapQuotes() + Environment.NewLine + Environment.NewLine +
                                     "Duplicates : " + _duplicates;
                    response.SetInputError(new BodyParameterException(message, name, _duplicates), settings);

                    return false;
                }

                return true;
            }

            private static bool
            ValidateCursorDiection(string after, string before, HelixResponse response, HelixRequestSettings settings, out string direction)
            {
                direction = "after";
                if (!after.IsNull() && !before.IsNull())
                {
                    direction = null;

                    response.SetInputError(new QueryParameterException("Only one pagination direction can be specified. Only use either 'after' or 'before'."), settings);

                    return false;
                }

                if (!before.IsNull())
                {
                    direction = "before";
                }

                return true;
            }

            #endregion

            #region Helpers - Response Handling

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
        }
    }
}