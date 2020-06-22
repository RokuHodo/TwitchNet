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

            #region /analytics/extensions - DONE

            /// <summary>
            /// <para>Asynchronously gets a specific extension analytic report, a single page, or a complete list of extension analytic reports.</para>
            /// <para>Required Scope: <see cref="HelixScopes.AnalyticsReadExtensions"/>.</para>
            /// </summary>
            /// <param name="pages">How many pages to request.</param>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific extension analytic report, the single page, or the complete list of extension analytic reports.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the extension ID is empty or contains only white space, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// Thrown if started_at or ended at are provided without the other.
            /// Thrown if started_at or ended_at is later than <see cref="DateTime.UtcNow"/>, or started_at is later than ended_at, if provided.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="HelixScopes.AnalyticsReadExtensions"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<ExtensionAnalytics>>>
            GetExtensionAnalyticsAsync(RequestedPages pages, HelixAuthorization authorization, ExtensionAnalyticsParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<DataPage<ExtensionAnalytics>> response = new HelixResponse<DataPage<ExtensionAnalytics>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response, HelixScopes.AnalyticsReadExtensions))
                {
                    return response;
                }

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    parameters.first = parameters.first.Clamp(1, 100);

                    if (!ValidateOptionalQueryParameter(nameof(parameters.extension_id), parameters.extension_id, response, settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, settings))
                    {
                        return response;
                    }

                    if (!parameters.extension_id.IsNull())
                    {
                        parameters.after = null;
                    }

                    if (parameters.started_at.HasValue && !parameters.ended_at.HasValue)
                    {
                        response.SetInputError(new QueryParameterException("ended_at must be provided if started_at is provided.", nameof(parameters.ended_at)), settings);

                        return response;
                    }
                    else if (!parameters.started_at.HasValue && parameters.ended_at.HasValue)
                    {
                        response.SetInputError(new QueryParameterException("started_at must be provided if ended_at is provided.", nameof(parameters.started_at)), settings);

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

                    if (!ValidateOptionalQueryParameter(nameof(parameters.ended_at), parameters.ended_at, DateTime.UtcNow, response, settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.started_at), parameters.started_at, DateTime.UtcNow, response, settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.started_at), parameters.started_at, parameters.ended_at, response, settings))
                    {
                        return response;
                    }
                }

                RestRequest request = GetBaseRequest("analytics/extensions", Method.GET, pages, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<DataPage<ExtensionAnalytics>> _response = await client.GetPagesAsync<ExtensionAnalytics, DataPage<ExtensionAnalytics>>(request, HandleResponse);
                response = new HelixResponse<DataPage<ExtensionAnalytics>>(_response);

                return response;
            }

            #endregion

            #region /analytics/games - DONE

            /// <summary>
            /// <para>Asynchronously gets a specific game analytic report, a single page, or a complete list of game analytic reports.</para>
            /// <para>Required Scope: <see cref="HelixScopes.AnalyticsReadGames"/>.</para>
            /// </summary>
            /// <param name="pages">How many pages to request.</param>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific game analytic report, the single page, or the complete list of game analytic reports.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the game ID is empty or contains only white space, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// Thrown if started_at or ended at are provided without the other.
            /// Thrown if started_at or ended_at is later than <see cref="DateTime.UtcNow"/>, or started_at is later than ended_at, if provided.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="HelixScopes.AnalyticsReadGames"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<GameAnalytics>>>
            GetGameAnalyticsAsync(RequestedPages pages, HelixAuthorization authorization, GameAnalyticsParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<DataPage<GameAnalytics>> response = new HelixResponse<DataPage<GameAnalytics>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response, HelixScopes.AnalyticsReadGames))
                {
                    return response;
                }

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    parameters.first = parameters.first.Clamp(1, 100);

                    if (!ValidateOptionalQueryParameter(nameof(parameters.game_id), parameters.game_id, response, settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, settings))
                    {
                        return response;
                    }

                    if (!parameters.game_id.IsNull())
                    {
                        parameters.after = null;
                    }

                    if (parameters.started_at.HasValue && !parameters.ended_at.HasValue)
                    {
                        response.SetInputError(new QueryParameterException("ended_at must be provided if started_at is provided.", nameof(parameters.ended_at)), settings);

                        return response;
                    }
                    else if (!parameters.started_at.HasValue && parameters.ended_at.HasValue)
                    {
                        response.SetInputError(new QueryParameterException("started_at must be provided if ended_at is provided.", nameof(parameters.started_at)), settings);

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

                    if (!ValidateOptionalQueryParameter(nameof(parameters.ended_at), parameters.ended_at, DateTime.UtcNow, response, settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.started_at), parameters.started_at, DateTime.UtcNow, response, settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.started_at), parameters.started_at, parameters.ended_at, response, settings))
                    {
                        return response;
                    }
                }

                RestRequest request = GetBaseRequest("analytics/games", Method.GET, pages, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<DataPage<GameAnalytics>> _response = await client.GetPagesAsync<GameAnalytics, DataPage<GameAnalytics>>(request, HandleResponse);
                response = new HelixResponse<DataPage<GameAnalytics>>(_response);

                return response;
            }

            #endregion

            #region /bits/leaderboard - DONE

            /// <summary>
            /// <para>
            /// Asynchronously gets a ranked list of bits leaderboard information for a user.
            /// The user is implicitly specified by the provided Bearer token.
            /// </para>
            /// <para>Required Scope: <see cref="HelixScopes.BitsRead"/>.</para>
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains ranked list of bits leaderboard information.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the user ID is empty or contains only white space, if provided.
            /// Thrown if started_at is later than <see cref="DateTime.Now"/>, if provided.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="HelixScopes.BitsRead"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<BitsLeaderboardData<BitsUser>>>
            GetBitsLeaderboardAsync(HelixAuthorization authorization, BitsLeaderboardParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<BitsLeaderboardData<BitsUser>> response = new HelixResponse<BitsLeaderboardData<BitsUser>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response, HelixScopes.BitsRead))
                {
                    return response;
                }

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    parameters.count = parameters.count.Clamp(1, 100);

                    if (!ValidateOptionalQueryParameter(nameof(parameters.user_id), parameters.user_id, response, settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.started_at), parameters.started_at, DateTime.Now, response, settings))
                    {
                        return response;
                    }

                    if (parameters.period == BitsLeaderboardPeriod.All)
                    {
                        parameters.started_at = null;
                    }
                }

                RestRequest request = GetBaseRequest("bits/leaderboard", Method.GET, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<BitsLeaderboardData<BitsUser>> _response = await client.ExecuteAsync<BitsLeaderboardData<BitsUser>>(request, HandleResponse);
                response = new HelixResponse<BitsLeaderboardData<BitsUser>>(_response);


                return response;
            }

            #endregion

            #region /clips - DONE

            /// <summary>
            /// <para>Asynchronously creates a clip.</para>
            /// <para>Required Scope: <see cref="HelixScopes.ClipsEdit"/>.</para>
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the created clip ID and URL to edit the clip.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="HelixScopes.ClipsEdit"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<CreatedClip>>>
            CreateClipAsync(HelixAuthorization authorization, CreateClipParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<Data<CreatedClip>> response = new HelixResponse<Data<CreatedClip>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response, HelixScopes.ClipsEdit))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                if (!ValidateRequiredQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("clips", Method.POST, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<Data<CreatedClip>> _response = await client.ExecuteAsync<Data<CreatedClip>>(request, HandleResponse);
                response = new HelixResponse<Data<CreatedClip>>(_response);

                return response;
            }

            /// <summary>
            /// Asynchronously gets specific clips, a single page, or a complete list of clips.
            /// </summary>
            /// <param name="pages">How many pages to request.</param>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> containts the specific clips, the single page, or the complete list  of clips.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
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
            GetClipsAsync(RequestedPages pages, HelixAuthorization authorization, ClipsParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<DataPage<Clip>> response = new HelixResponse<DataPage<Clip>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                if (!parameters.ids.IsValid() && parameters.broadcaster_id.IsNull() && parameters.game_id.IsNull())
                {
                    response.SetInputError(new QueryParameterException("At least one or more clip ID, one broadcaster ID, or one game ID must be provided."), settings);

                    return response;
                }

                if ((parameters.ids.IsValid() && (!parameters.broadcaster_id.IsNull() || !parameters.game_id.IsNull())) ||
                   (!parameters.broadcaster_id.IsNull() && !parameters.game_id.IsNull()))
                {
                    response.SetInputError(new QueryParameterException("Only one or more clip ID's, one broadcaster ID, or one game ID can be provided."), settings);

                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.game_id), parameters.game_id, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.ids), parameters.ids, 100, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, settings))
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

                if (!ValidateOptionalQueryParameter(nameof(parameters.ended_at), parameters.ended_at, DateTime.Now, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.started_at), parameters.started_at, DateTime.Now, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.started_at), parameters.started_at, parameters.ended_at, response, settings))
                {
                    return response;
                }

                // ended_at is ignored if no started_at is provided
                if (!parameters.started_at.HasValue)
                {
                    parameters.ended_at = null;
                }

                RestRequest request = GetBaseRequest("clips", Method.GET, pages, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<DataPage<Clip>> _response = await client.GetPagesAsync<Clip, DataPage<Clip>>(request, HandleResponse);
                response = new HelixResponse<DataPage<Clip>>(_response);

                return response;
            }

            #endregion

            #region /entitlements/codes - DONE

            /// <summary>
            /// <para>Asynchronously gets the status of one or more entitlement codes for the authenticated user.</para>
            /// <para>Required Authorization: App Access Token.</para>
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
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
            GetEntitlementCodeStatusAsync(HelixAuthorization authorization, EntitlementsCodeParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<Data<CodeStatus>> response = new HelixResponse<Data<CodeStatus>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                if(!ValidateRequiredQueryParameter(nameof(parameters.user_id), parameters.user_id, response, settings) ||
                   !ValidateRequiredQueryParameter(nameof(parameters.codes), parameters.codes, 20, response, settings))
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
                    response.SetInputError(new QueryParameterException(message, nameof(parameters.codes), _codes, inner_exception), settings);

                    return response;
                }

                RestRequest request = GetBaseRequest("entitlements/codes", Method.GET, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<Data<CodeStatus>> _response = await client.ExecuteAsync<Data<CodeStatus>>(request, HandleResponse);
                response = new HelixResponse<Data<CodeStatus>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously redeems one or more entitlement codes to the authenticated user.</para>
            /// <para>Required Authorization: App Access Token.</para>
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
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
            RedeemEntitlementCodeStatusAsync(HelixAuthorization authorization, EntitlementsCodeParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<Data<CodeStatus>> response = new HelixResponse<Data<CodeStatus>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                if (!ValidateRequiredQueryParameter(nameof(parameters.user_id), parameters.user_id, response, settings) ||
                   !ValidateRequiredQueryParameter(nameof(parameters.codes), parameters.codes, 20, response, settings))
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
                    response.SetInputError(new QueryParameterException(message, nameof(parameters.codes), _codes, inner_exception), settings);

                    return response;
                }

                RestRequest request = GetBaseRequest("entitlements/codes", Method.POST, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<Data<CodeStatus>> _response = await client.ExecuteAsync<Data<CodeStatus>>(request, HandleResponse);
                response = new HelixResponse<Data<CodeStatus>>(_response);

                return response;
            }

            #endregion

            #region /entitlements/upload - DONE

            /// <summary>
            /// <para>Asynchronously creates a URL where you can upload a manifest file and notify users that they have an entitlement.</para>
            /// <para>Required Authorization: App Access Token.</para>
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
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
            CreateEntitlementGrantsUploadUrlAsync(HelixAuthorization authorization, EntitlementsUploadParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<Data<EntitlementUploadUrl>> response = new HelixResponse<Data<EntitlementUploadUrl>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                if(!ValidateRequiredQueryParameter(nameof(parameters.manifest_id), parameters.manifest_id, response, settings))
                {
                    return response;
                }

                if (!parameters.manifest_id.Length.IsInRange(1, 64))
                {
                    response.SetInputError(new QueryParameterException("The manifest ID must be between 1 and 64 characters long, inclusive.", nameof(parameters.manifest_id)), settings);

                    return response;
                }

                RestRequest request = GetBaseRequest("entitlements/upload", Method.POST, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<Data<EntitlementUploadUrl>> _response = await client.ExecuteAsync<Data<EntitlementUploadUrl>>(request, HandleResponse);
                response = new HelixResponse<Data<EntitlementUploadUrl>>(_response);

                return response;
            }

            #endregion

            #region /extensions/transactions - DONE

            /// <summary>
            /// <para>Asynchronously gets specific extension transactions, a single page, or a complete list of extension transactions.</para>
            /// <para>Required Authorization: App Access Token.</para>
            /// </summary>
            /// <param name="pages">How many pages to request.</param>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific extension transactions, the single page, or the complete list of extension transactions.
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
            GetExtensionTransactionsAsync(RequestedPages pages, HelixAuthorization authorization, ExtensionTransactionsParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<DataPage<ExtensionTransaction>> response = new HelixResponse<DataPage<ExtensionTransaction>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                if(!ValidateRequiredQueryParameter(nameof(parameters.extension_id), parameters.extension_id, response, settings))
                {
                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.ids), parameters.ids, 100, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("extensions/transactions", Method.GET, pages, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<DataPage<ExtensionTransaction>> _response = await client.GetPagesAsync<ExtensionTransaction, DataPage<ExtensionTransaction>>(request, HandleResponse);
                response = new HelixResponse<DataPage<ExtensionTransaction>>(_response);

                return response;
            }

            #endregion

            #region /games - DONE

            /// <summary>
            /// Asynchronously gets a list of games.
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the list of games.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if any game ID or game name is null, empty, or contains only white space.
            /// Thrown if any duplicate game ID's or game names are found.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if none or more than 100 total game ID's and/or game names are provided.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<Game>>>
            GetGamesAsync(HelixAuthorization authorization, GamesParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<Data<Game>> response = new HelixResponse<Data<Game>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                if (!parameters.ids.IsValid() && !parameters.names.IsValid())
                {
                    response.SetInputError(new QueryParameterCountException("At least one game ID or game name must be provided.", 100, 0), settings);

                    return response;
                }

                int count = parameters.ids.Count + parameters.names.Count;
                if (count > 100)
                {
                    response.SetInputError(new QueryParameterCountException("A maximum of 100 total game ID's and/or names can be provided at one time.", 100, count), settings);

                    return response;
                }

                // This will perform count checks again, but they will never be triggered if we get this far.
                // This is really for checking for duplicates and no content indicies.
                if (!ValidateOptionalQueryParameter(nameof(parameters.ids), parameters.ids, 100, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.names), parameters.names, 100, response, settings))
                {
                    return response;
                }         

                RestRequest request = GetBaseRequest("games", Method.GET, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<Data<Game>> _response = await client.ExecuteAsync<Data<Game>>(request, HandleResponse);
                response = new HelixResponse<Data<Game>>(_response);

                return response;
            }

            #endregion

            #region /games/top - DONE

            /// <summary>
            /// Asynchronously gets a single page or a complete list of top games, most popular first.
            /// </summary>
            /// <param name="pages">How many pages to request.</param>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page or the complete list of top videos.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
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
            GetTopGamesAsync(RequestedPages pages, HelixAuthorization authorization, TopGamesParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<DataPage<Game>> response = new HelixResponse<DataPage<Game>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response))
                {
                    return response;
                }

                string direction = string.Empty;

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    parameters.first = parameters.first.Clamp(1, 100);

                    if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.before), parameters.before, response, settings) ||
                        !ValidateCursorDiection(parameters.after, parameters.before, response, settings, out direction))
                    {
                        return response;
                    }

                    // TODO: GetTopGamesAsync(...) - Temporarily disabling using 'before' while requesting all pages until it works properly. Reimplement 'before' when it works propery.
                    if (parameters.before.IsValid())
                    {
                        response.SetInputError(new NotSupportedException("The pagination direction 'before' is temporarily not supported. Following the cursor using 'before' returns incorrect results and does not work properly on Twitch's back end."), settings);

                        return response;
                    }
                }

                RestRequest request = GetBaseRequest("games/top", Method.GET, pages, authorization, settings);
                request.AddParameters(parameters);

                // TODO: GetTopGamesAsync(...) - Sanitize the list based on the game ID and return a distinct list.
                RestResponse<DataPage<Game>> _response = await client.GetPagesAsync<Game, DataPage<Game>>(request, direction, HandleResponse);
                response = new HelixResponse<DataPage<Game>>(_response);

                return response;
            }

            #endregion

            #region /moderation/banned - DONE

            /// <summary>
            /// <para>Asynchronously gets specific banned users, a single page, or a complete list of banned users for a given broadcaster.</para>
            /// <para>Required Scope: <see cref="HelixScopes.ModerationRead"/>.</para>
            /// </summary>
            /// <param name="pages">How many pages to request.</param>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific banned users, the single page, or the complete list of banned users for the given broadcaster.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the broadcaster ID is not provided, empty, or contains only white space.
            /// Thrown if any user ID is null, empty, or contains only white space any duplicate user ID's are found, if provided.
            /// Thrown if the after or before cursor is empty or contains only white space, if provided.
            /// Thrown if both after and before cursors are provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if more than 100 user ID's are provided.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="HelixScopes.ModerationRead"/> scope.</exception>
            /// <exception cref="NotSupportedException">
            /// Thrown if a before cursor is provided.
            /// This is a temporary error and will be removed once Twitch fixes reverse pagination.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<BannedUser>>>
            GetBannedUsersAsync(RequestedPages pages, HelixAuthorization authorization, BannedUsersParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<DataPage<BannedUser>> response = new HelixResponse<DataPage<BannedUser>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response, HelixScopes.ModerationRead))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                // Required parameter checks
                if (!ValidateRequiredQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, settings))
                {
                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.before), parameters.before, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.user_ids), parameters.user_ids, 100, response, settings) ||
                    !ValidateCursorDiection(parameters.after, parameters.before, response, settings, out string direction))
                {
                    return response;
                }

                // TODO: GetBannedUsersAsync(...) - Temporarily disabling using 'before' while requesting all pages until it works properly. Reimplement 'before' when it works propery.
                if (parameters.before.IsValid())
                {
                    response.SetInputError(new NotSupportedException("The pagination direction 'before' is temporarily not supported. Following the cursor using 'before' returns incorrect results and does not work properly on Twitch's back end."), settings);

                    return response;
                }

                RestRequest request = GetBaseRequest("moderation/banned", Method.GET, pages, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<DataPage<BannedUser>> _response = await client.GetPagesAsync<BannedUser, DataPage<BannedUser>>(request, direction, HandleResponse);
                response = new HelixResponse<DataPage<BannedUser>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously checks to see if a user is banned by a broadcaster.</para>
            /// <para>Required Scope: <see cref="HelixScopes.ModerationRead"/>.</para>
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="broadcaster_id">The ID of the broadcaster.</param>
            /// <param name="user_id">The ID of the user to check.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> is set true if the user is banned by the broadcaster, otherwise false.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID or user ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<bool>>
            IsUserBannedAsync(HelixAuthorization authorization, string broadcaster_id, string user_id, HelixRequestSettings settings)
            {
                HelixResponse<bool> response = new HelixResponse<bool>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response, HelixScopes.ModerationRead))
                {
                    return response;
                }

                // Explicity check for this out here since it's optional in the underlying wrapper.
                if (!ValidateRequiredQueryParameter(nameof(user_id), user_id, response, settings))
                {
                    return response;
                }

                BannedUsersParameters parameters = new BannedUsersParameters();
                parameters.broadcaster_id = broadcaster_id;
                parameters.user_ids.Add(user_id);

                IHelixResponse<DataPage<BannedUser>> _response = await GetBannedUsersAsync(RequestedPages.Single, authorization, parameters, settings);

                bool result = _response.exception.IsNull() ? _response.result.data.IsValid() : false;
                response = new HelixResponse<bool>(_response, result);

                return response;
            }

            #endregion

            #region /moderation/banned/events - DONE

            /// <summary>
            /// <para>
            /// Asynchronously gets a specific banned event, a single page, or a complete list of banned events.
            /// A banned event occurs when a user is banned or unbanned.
            /// </para>
            /// <para>Required Scope: <see cref="HelixScopes.ModerationRead"/>.</para>
            /// </summary>
            /// <param name="pages">How many pages to request.</param>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific banned event, the single page, or the complete list of banned events.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if neither broadcaster ID or event ID are provided or are provided at the same time.
            /// Thrown if the broadcaster ID is empty or contains only white space, if provided.
            /// Thrown if the event ID is empty or contains only white space, if provided.
            /// Thrown if any user ID is null, empty, or contains only white space or any duplicate user ID's are found, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if more than 100 user ID's are provided.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="HelixScopes.ModerationRead"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<BannedEvent>>>
            GetBannedEventsAsync(RequestedPages pages, HelixAuthorization authorization, BannedEventsParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<DataPage<BannedEvent>> response = new HelixResponse<DataPage<BannedEvent>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response, HelixScopes.ModerationRead))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                // Required parameter checks
                if (parameters.broadcaster_id.IsNull() && parameters.id.IsNull())
                {
                    response.SetInputError(new QueryParameterException("A boradcaster ID or event ID must be provided."), settings);

                    return response;
                }
                else if (!parameters.broadcaster_id.IsNull() && !parameters.id.IsNull())
                {
                    response.SetInputError(new QueryParameterException("Only a boradcaster ID or event ID can be provided at one time."), settings);

                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.id), parameters.id, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.user_ids), parameters.user_ids, 100, response, settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("moderation/banned/events", Method.GET, pages, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<DataPage<BannedEvent>> _response = await client.GetPagesAsync<BannedEvent, DataPage<BannedEvent>>(request, HandleResponse);
                response = new HelixResponse<DataPage<BannedEvent>>(_response);

                return response;
            }

            #endregion

            #region /moderation/enforcements/status - DONE

            /// <summary>
            /// <para>Asynchronously checks to see of a chat message meets the AutoMod requirements to be posted in chat.</para>
            /// <para>Required Scope: <see cref="HelixScopes.ModerationRead"/>.</para>
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the AutoMod status of each message on whether or not the message meets the requirements to be posted in chat.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID is not provided, empty, or contains only white space.</exception>            
            /// <exception cref="BodyParameterException">
            /// Thrown if data is not provided.
            /// Thrown if any AutoMod message is null.
            /// Thrown if any AutoMod message ID is not provided or duplicate message ID's are found.
            /// Thrown if any AutoMod message text is not provided, empty, or contains only white space.
            /// Thrown if any AutoMod user ID is not provided, empty, or contains only white space.
            /// </exception>
            /// <exception cref="BodyParameterCountException">Thrown if none or more than 100 AutoMod messages are provided.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="HelixScopes.ModerationRead"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<AutoModMessageStatus>>>
            CheckAutoModMessageStatus(HelixAuthorization authorization, AutoModMessageStatusParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<Data<AutoModMessageStatus>> response = new HelixResponse<Data<AutoModMessageStatus>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response, HelixScopes.ModerationRead))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                // Required parameter checks
                if (!ValidateRequiredQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, settings))
                {
                    return response;
                }

                if (parameters.data.IsNull())
                {
                    response.SetInputError(new BodyParameterException("A required parameter is missing: " + nameof(parameters.data).WrapQuotes(), nameof(parameters.data)), settings);

                    return response;
                }

                if (parameters.data.Count == 0)
                {
                    response.SetInputError(new BodyParameterCountException("At least one element must be provided for the parameter: " + nameof(parameters.data).WrapQuotes(), nameof(parameters.data), 100, parameters.data.Count), settings);

                    return response;
                }

                if (parameters.data.Count > 100)
                {
                    response.SetInputError(new BodyParameterCountException("A maximum of " + 100 + " elements can be provided at one time for the parameter: " + nameof(parameters.data).WrapQuotes(), nameof(parameters.data), 100, parameters.data.Count), settings);

                    return response;
                }                

                List<int> indicies_null = new List<int>(parameters.data.Count);
                List<int> indicies_msg_ids_null = new List<int>(parameters.data.Count);
                List<int> indicies_msg_text_no_content = new List<int>(parameters.data.Count);
                List<int> indicies_user_ids_no_content = new List<int>(parameters.data.Count);

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
                    response.SetInputError(new BodyParameterException(message, nameof(parameters.data), _indicies), settings);

                    return response;
                }

                if (indicies_msg_ids_null.Count > 0)
                {
                    string _indicies = string.Join(", ", indicies_msg_ids_null);

                    string message = "One or more AutoMod messages had a \"msg_id\" that was null." + Environment.NewLine + Environment.NewLine +
                                     "Indicies : " + _indicies;
                    response.SetInputError(new BodyParameterException(message, "msg_id", _indicies), settings);

                    return response;
                }

                if (msg_ids_duplicates.Count > 0)
                {
                    string _duplicates = string.Join(", ", msg_ids_duplicates);

                    string message = "One or more AutoMod messages had a duplicate \"msg_id\"." + Environment.NewLine + Environment.NewLine +
                                     "Values : " + _duplicates;
                    response.SetInputError(new BodyParameterException(message, "msg_id", _duplicates), settings);

                    return response;
                }

                if (indicies_msg_text_no_content.Count > 0)
                {
                    string _indicies = string.Join(", ", indicies_msg_text_no_content);

                    string message = "One or more AutoMod messages had a \"msg_txt\" that was null, empty, or contained only white space." + Environment.NewLine + Environment.NewLine +
                                     "Indicies : " + _indicies;
                    response.SetInputError(new BodyParameterException(message, "msg_text", _indicies), settings);

                    return response;
                }

                if (indicies_user_ids_no_content.Count > 0)
                {
                    string _indicies = string.Join(", ", indicies_user_ids_no_content);

                    string message = "One or more AutoMod messages had a \"user_id\" that was null, empty, or contained only white space." + Environment.NewLine + Environment.NewLine +
                                     "Indicies : " + _indicies;
                    response.SetInputError(new BodyParameterException(message, "user_id", _indicies), settings);

                    return response;
                }

                RestRequest request = GetBaseRequest("moderation/enforcements/status", Method.POST, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<Data<AutoModMessageStatus>> _response = await client.ExecuteAsync<Data<AutoModMessageStatus>>(request, HandleResponse);
                response = new HelixResponse<Data<AutoModMessageStatus>>(_response);

                return response;
            }

            #endregion

            #region /moderation/moderators - DONE

            /// <summary>
            /// <para>Asynchronously gets specific moderators, a single page, or a complete list of moderators users for a given broadcaster.</para>
            /// <para>Required Scope: <see cref="HelixScopes.ModerationRead"/>.</para>
            /// </summary>
            /// <param name="pages">How many pages to request.</param>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific moderators, the single page, or the complete list of moderators for the given broadcaster.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the broadcaster ID is not ptovided, empty, or contains only white space.
            /// Thrown if any user ID is null, empty, or contains only white space or any duplicate user ID's are found, if provided.
            /// Thrown if the after or before cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if more than 100 user ID's are provided.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="HelixScopes.ModerationRead"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<Moderator>>>
            GetModeratorsAsync(RequestedPages pages, HelixAuthorization authorization, ModeratorsParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<DataPage<Moderator>> response = new HelixResponse<DataPage<Moderator>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response, HelixScopes.ModerationRead))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                // Required parameter checks
                if (!ValidateRequiredQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, settings))
                {
                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.user_ids), parameters.user_ids, 100, response, settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("moderation/moderators", Method.GET, pages, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<DataPage<Moderator>> _response = await client.GetPagesAsync<Moderator, DataPage<Moderator>>(request, HandleResponse);
                response = new HelixResponse<DataPage<Moderator>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously checks to see if a user is a moderator for a given broadcaster.</para>
            /// <para>Required Scope: <see cref="HelixScopes.ModerationRead"/>.</para>
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="broadcaster_id">The ID of the broadcaster.</param>
            /// <param name="user_id">The ID of the user to check.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> is set true if the user is a moderator for the given broadcaster, otherwise false.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID or user ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="HelixScopes.ModerationRead"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<bool>>
            IsUserModeratorAsync(HelixAuthorization authorization, string broadcaster_id, string user_id, HelixRequestSettings settings)
            {
                HelixResponse<bool> response = new HelixResponse<bool>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response, HelixScopes.ModerationRead))
                {
                    return response;
                }

                // Explicity check for this out here since it's optional in the underlying wrapper.
                if (!ValidateRequiredQueryParameter(nameof(user_id), user_id, response, settings))
                {
                    return response;
                }

                ModeratorsParameters parameters = new ModeratorsParameters();
                parameters.broadcaster_id = broadcaster_id;
                parameters.user_ids.Add(user_id);

                IHelixResponse<DataPage<Moderator>> _response = await GetModeratorsAsync(RequestedPages.Single, authorization, parameters, settings);

                bool result = _response.exception.IsNull() ? _response.result.data.IsValid() : false;
                response = new HelixResponse<bool>(_response, result);

                return response;
            }

            #endregion

            #region /moderation/moderators/events - DONE

            /// <summary>
            /// <para>
            /// Asynchronously gets a specific moderator event, a single page, or a complete list of moderator events.
            /// A modewrator event occurs when a user gains or loses moderator (OP) status.
            /// </para>
            /// <para>Required Scope: <see cref="HelixScopes.ModerationRead"/>.</para>
            /// </summary>
            /// <param name="pages">How many pages to request.</param>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific moderator event, the single page, or the complete list of moderator events.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if neither broadcaster ID or event ID are provided or are provided at the same time.
            /// Thrown if the broadcaster ID is empty or contains only white space, if provided.
            /// Thrown if the event ID is empty or contains only white space, if provided.
            /// Thrown if any user ID is null, empty, or contains only white space or any duplicate user ID's are found, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if more than 100 user ID's are provided.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="HelixScopes.ModerationRead"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<ModeratorEvent>>>
            GetModeratorEventsAsync(RequestedPages pages, HelixAuthorization authorization, ModeratorEventsParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<DataPage<ModeratorEvent>> response = new HelixResponse<DataPage<ModeratorEvent>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response, HelixScopes.ModerationRead))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                // Required parameter checks
                if (parameters.broadcaster_id.IsNull() && parameters.id.IsNull())
                {
                    response.SetInputError(new QueryParameterException("A boradcaster ID or event ID must be provided."), settings);

                    return response;
                }
                else if (!parameters.broadcaster_id.IsNull() && !parameters.id.IsNull())
                {
                    response.SetInputError(new QueryParameterException("Only a boradcaster ID or event ID can be provided at one time."), settings);

                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.id), parameters.id, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.user_ids), parameters.user_ids, 100, response, settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("moderation/moderators/events", Method.GET, pages, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<DataPage<ModeratorEvent>> _response = await client.GetPagesAsync<ModeratorEvent, DataPage<ModeratorEvent>>(request, HandleResponse);
                response = new HelixResponse<DataPage<ModeratorEvent>>(_response);

                return response;
            }

            #endregion

            #region /streams - DONE

            /// <summary>
            /// Asynchronously gets a single page or a complete list of streams.
            /// </summary>
            /// <param name="pages">How many pages to request.</param>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page or the complete list of streams.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
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
            GetStreamsAsync(RequestedPages pages, HelixAuthorization authorization, StreamsParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<DataPage<Stream>> response = new HelixResponse<DataPage<Stream>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response))
                {
                    return response;
                }

                string direction = string.Empty;

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    parameters.first = parameters.first.Clamp(1, 100);

                    if (!ValidateOptionalQueryParameter(nameof(parameters.game_ids), parameters.game_ids, 100, response, settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.user_ids), parameters.user_ids, 100, response, settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.user_logins), parameters.user_logins, 100, response, settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.before), parameters.before, response, settings) ||
                        !ValidateCursorDiection(parameters.after, parameters.before, response, settings, out direction))
                    {
                        return response;
                    }

                    // TODO: /streams - GetStreamsAsync(...) - Temporarily disabling using 'before' while requesting all pages until it works properly. Reimplement 'before' when it works propery.
                    if (parameters.before.IsValid())
                    {
                        response.SetInputError(new NotSupportedException("The pagination direction 'before' is temporarily not supported. Following the cursor using 'before' returns incorrect results and does not work properly on Twitch's back end."), settings);

                        return response;
                    }
                }

                RestRequest request = GetBaseRequest("streams", Method.GET, pages, authorization, settings);
                request.AddParameters(parameters);

                // TODO: GetStreamsAsync(...) - Sanitize the list based on the stream ID and return a distinct list.
                RestResponse<DataPage<Stream>> _response = await client.GetPagesAsync<Stream, DataPage<Stream>>(request, direction, HandleResponse);
                response = new HelixResponse<DataPage<Stream>>(_response);

                return response;
            }

            /// <summary>
            /// Asynchronously checks to see if a user is streaming by their user ID.
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="user_id">The ID of the user to check.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> is set to true is the user is streaming, otherwise false.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">Thrown if the user ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<bool>>
            IsStreamLiveAsync_UserID(HelixAuthorization authorization, string user_id, HelixRequestSettings settings)
            {
                HelixResponse<bool> response = new HelixResponse<bool>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response))
                {
                    return response;
                }

                if(!ValidateRequiredQueryParameter(nameof(user_id), user_id, response, settings))
                {
                    return response;
                }

                StreamsParameters parameters = new StreamsParameters();
                parameters.user_ids.Add(user_id);

                IHelixResponse<DataPage<Stream>> _response = await GetStreamsAsync(RequestedPages.Single, authorization, parameters, settings);

                bool result = _response.exception.IsNull() ? _response.result.data.IsValid() : false;
                response = new HelixResponse<bool>(_response, result);

                return response;
            }

            /// <summary>
            /// Asynchronously checks to see if a user is streaming by their login.
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="user_login">The login of the user to check.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> is set to true is the user is streaming, otherwise false.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">Thrown if the user login is not provided, empty, or contains only white space.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<bool>>
            IsStreamLiveAsync_Login(HelixAuthorization authorization, string user_login, HelixRequestSettings settings)
            {
                HelixResponse<bool> response = new HelixResponse<bool>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response))
                {
                    return response;
                }

                if (!ValidateRequiredQueryParameter(nameof(user_login), user_login, response, settings))
                {
                    return response;
                }

                StreamsParameters parameters = new StreamsParameters();
                parameters.user_logins.Add(user_login);

                IHelixResponse<DataPage<Stream>> _response = await GetStreamsAsync(RequestedPages.Single, authorization, parameters, settings);

                bool result = _response.exception.IsNull() ? _response.result.data.IsValid() : false;
                response = new HelixResponse<bool>(_response, result);

                return response;
            }

            #endregion

            #region /streams/markers - DONE

            /// <summary>
            /// <para>
            /// Asynchronously creates a stream marker (an arbitray time spamp) in a stream specified by the provided user ID.
            /// Stream markers can be created by the person streaming or any of their editors.
            /// </para>
            /// <para>Required Scope: <see cref="HelixScopes.UserEditBroadcast"/>.</para>
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the contains stream marker.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the user ID not provided, empty, or contains only white space.
            /// Thrown if the description is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="HelixScopes.UserEditBroadcast"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<CreatedStreamMarker>>>
            CreateStreamMarkerAsync(HelixAuthorization authorization, CreateStreamMarkerParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<DataPage<CreatedStreamMarker>> response = new HelixResponse<DataPage<CreatedStreamMarker>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response, HelixScopes.UserEditBroadcast))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                if (!ValidateRequiredBodyParameter(nameof(parameters.user_id), parameters.user_id, response, settings))
                {
                    return response;
                }

                // Optional parameter checks
                if (!ValidateOptionalBodyParameter(nameof(parameters.description), parameters.description, response, settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("streams/markers", Method.POST, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<DataPage<CreatedStreamMarker>> _response = await client.ExecuteAsync<DataPage<CreatedStreamMarker>>(request, HandleResponse);
                response = new HelixResponse<DataPage<CreatedStreamMarker>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously gets a single page or a complete list of stream markers (arbitray time spamps) for a user or a specific video.</para>
            /// <para>Required Scope: <see cref="HelixScopes.UserReadBroadcast"/>.</para>
            /// </summary>
            /// <param name="pages">How many pages to request.</param>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page or the complete list of stream markers for the user or specified video.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if neither user ID or video ID are provided or are provided at the same time.
            /// Thrown if the user ID or video ID is empty or contains only white space, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="HelixScopes.UserReadBroadcast"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<StreamMarkers>>>
            GetStreamMarkersAsync(RequestedPages pages, HelixAuthorization authorization, StreamMarkersParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<DataPage<StreamMarkers>> response = new HelixResponse<DataPage<StreamMarkers>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response, HelixScopes.UserReadBroadcast))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                if (parameters.user_id.IsValid() && parameters.video_id.IsValid())
                {
                    response.SetInputError(new QueryParameterException("Only a user ID or a video ID can be provided."), settings);

                    return response;
                }
                else if (!parameters.user_id.IsValid() && !parameters.video_id.IsValid())
                {
                    response.SetInputError(new QueryParameterException("A user ID or video ID must be provided."), settings);

                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.user_id), parameters.user_id, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.video_id), parameters.video_id, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("streams/markers", Method.GET, pages, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<DataPage<StreamMarkers>> _response = await client.GetPagesAsync<StreamMarkers, DataPage<StreamMarkers>>(request, HandleResponse);
                response = new HelixResponse<DataPage<StreamMarkers>>(_response);

                return response;
            }

            #endregion

            #region /streams/tags - DONE

            /// <summary>
            /// Asynchronously gets the tags set on a stream.
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the stream tags the broadcaster has set.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<StreamTag>>>
            GetSetStreamTagsAsync(HelixAuthorization authorization, StreamsTagsParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<Data<StreamTag>> response = new HelixResponse<Data<StreamTag>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                if(!ValidateRequiredQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("streams/tags", Method.GET, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<Data<StreamTag>> _response = await client.ExecuteAsync<Data<StreamTag>>(request, HandleResponse);
                response = new HelixResponse<Data<StreamTag>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously sets and overwrites a stream's tags.</para>
            /// <para>
            /// If no stream tags are specified, all stream tags are removed.
            /// The automatic tags that Twitch sets are not affected and cannot be added/removed.
            /// The set stream tags expire after 72 hours of being applied, or 72 hours after a stream goes offline if the stream was live during the initial 72 hour expriation window.
            /// </para>
            /// <para>Required scope: <see cref="HelixScopes.UserEditBroadcast"/>.</para>
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the broadcaster's set stream tags.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="BodyParameterException">Thrown if any tag ID is null, empty, or contains only white space or any duplicate tag ID's are found.</exception>
            /// <exception cref="BodyParameterCountException">Thrown if more than 5 total tag ID's are provided.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="HelixScopes.UserEditBroadcast"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse>
            SetStreamTagsAsync(HelixAuthorization authorization, SetStreamsTagsParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<Data<StreamTag>> response = new HelixResponse<Data<StreamTag>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response, HelixScopes.UserEditBroadcast))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                if (!ValidateRequiredQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, settings))
                {
                    return response;
                }

                // Optional parameter checks
                if(!ValidateOptionalBodyParameter(nameof(parameters.tag_ids), parameters.tag_ids, 5, response, settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("streams/tags", Method.PUT, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<Data<StreamTag>> _response = await client.ExecuteAsync<Data<StreamTag>>(request, HandleResponse);
                response = new HelixResponse<Data<StreamTag>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously removes all tags set on a stream.</para>
            /// <para>The automatic tags that Twitch sets are not affected and cannot be removed.</para>
            /// <para>Required scope: <see cref="HelixScopes.UserEditBroadcast"/>.</para>
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">
            /// A set of rest parameters.
            /// The tag ID's are ignored if provide.
            /// </param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the broadcaster's set stream tags.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="HelixScopes.UserEditBroadcast"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse>
            RemoveStreamTagsAsync(HelixAuthorization authorization, SetStreamsTagsParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<Data<StreamTag>> response = new HelixResponse<Data<StreamTag>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response, HelixScopes.UserEditBroadcast))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                if (!ValidateRequiredQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, settings))
                {
                    return response;
                }

                // Optional parameter "checks"
                parameters.tag_ids = null;

                RestRequest request = GetBaseRequest("streams/tags", Method.PUT, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<Data<StreamTag>> _response = await client.ExecuteAsync<Data<StreamTag>>(request, HandleResponse);
                response = new HelixResponse<Data<StreamTag>>(_response);

                return response;
            }

            #endregion

            #region /subscriptions - DONE

            /// <summary>
            /// <para>Asynchronously gets a single page or a complete list of a broadcaster's subscribers list.</para>
            /// <para>Required Scope: <see cref="HelixScopes.ChannelReadSubscriptions"/>.</para>
            /// </summary>
            /// <param name="pages">How many pages to request.</param>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page or the complete list of a broadcaster's subscribers list.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the broadcaster ID is not provided, empty, or contains only white space.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="HelixScopes.ChannelReadSubscriptions"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<Subscription>>>
            GetBroadcasterSubscribersAsync(RequestedPages pages, HelixAuthorization authorization, SubscriptionParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<DataPage<Subscription>> response = new HelixResponse<DataPage<Subscription>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response, HelixScopes.ChannelReadSubscriptions))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                if (!ValidateRequiredQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, settings))
                {
                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("subscriptions", Method.GET, pages, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<DataPage<Subscription>> _response = await client.GetPagesAsync<Subscription, DataPage<Subscription>>(request, HandleResponse);
                response = new HelixResponse<DataPage<Subscription>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously gets the subscription relationship between a broadcaster and a list of users.</para>
            /// <para>
            /// If a user is subscribed to the broadcater, the subscription information for that user is returned in the response.
            /// If a user is not subscribed to the broadcater, that user is omitted from the response.
            /// </para>
            /// <para>Required Scope: <see cref="HelixScopes.ChannelReadSubscriptions"/>.</para>
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
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
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the broadcaster ID is not provided, empty, or contains only white space.
            /// Thrown if any user ID is null, empty, or contains only white space or any duplicate user ID's are found.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if none or more than 100 total user ID's are provided.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="HelixScopes.ChannelReadSubscriptions"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<Subscription>>>
            GetSubscriptionRelationshipAsync(HelixAuthorization authorization, SubscriptionRelationshipParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<Data<Subscription>> response = new HelixResponse<Data<Subscription>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response, HelixScopes.ChannelReadSubscriptions))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                if (!ValidateRequiredQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, settings) ||
                    !ValidateRequiredQueryParameter(nameof(parameters.user_ids), parameters.user_ids, 100, response, settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("subscriptions", Method.GET, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<Data<Subscription>> _response = await client.ExecuteAsync<Data<Subscription>>(request, HandleResponse);
                response = new HelixResponse<Data<Subscription>>(_response);

                return response;
            }

            /// <summary>
            /// <para>Asynchronously checks to see if the from_id user is following the to_id user.</para>
            /// <para>Required Scope: <see cref="HelixScopes.ChannelReadSubscriptions"/>.</para>
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="broadcaster_id">The user ID of the broadcaster.</param>
            /// <param name="user_id">The ID of the possibly subscribed user.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> is set true if from_id is following to_id, otherwise false.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">Thrown if the broadcaster ID or user ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<bool>>
            IsUserSubscribedAsync(HelixAuthorization authorization, string broadcaster_id, string user_id, HelixRequestSettings settings)
            {
                HelixResponse<bool> response = new HelixResponse<bool>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response, HelixScopes.ChannelReadSubscriptions))
                {
                    return response;
                }

                // Required parameter checks

                // No need to check broadcaster ID here since it's checked later before the request is executed.
                if (!ValidateRequiredQueryParameter(nameof(user_id), user_id, response, settings))
                {
                    return response;
                }

                SubscriptionRelationshipParameters parameters = new SubscriptionRelationshipParameters();
                parameters.broadcaster_id = broadcaster_id;
                parameters.user_ids.Add(user_id);

                IHelixResponse<Data<Subscription>> _response = await GetSubscriptionRelationshipAsync(authorization, parameters, settings);

                bool result = _response.exception.IsNull() ? _response.result.data.IsValid() : false;
                response = new HelixResponse<bool>(_response, result);

                return response;
            }

            #endregion

            #region /subscriptions/events - DONE

            /// <summary>
            /// <para>
            /// Asynchronously gets a specific subscription event, a single page, or a complete list of subscription events over the last 5 days.
            /// A subscription event occurs when a user subscribed, unsubscribes, or send a notification message in chat.
            /// </para>
            /// <para>Required Scope: <see cref="HelixScopes.ChannelReadSubscriptions"/>.</para>
            /// </summary>
            /// <param name="pages">How many pages to request.</param>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the specific subscription event, the single page, or the complete list of subscription events.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if neither broadcaster ID or event ID are provided or are provided at the same time.
            /// Thrown if the broadcaster ID is empty or contains only white space, if provided.
            /// Thrown if the event ID is empty or contains only white space, if provided.
            /// Thrown if the user ID is empty or contains only white space, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="HelixScopes.ChannelReadSubscriptions"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<SubscriptionEvent>>>
            GetSubscriptionEventsAsync(RequestedPages pages, HelixAuthorization authorization, SubscriptionEventsParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<DataPage<SubscriptionEvent>> response = new HelixResponse<DataPage<SubscriptionEvent>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response, HelixScopes.ChannelReadSubscriptions))
                {
                    return response;
                }

                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                // Required parameter checks 
                if (parameters.broadcaster_id.IsNull() && parameters.id.IsNull())
                {
                    response.SetInputError(new QueryParameterException("A boradcaster ID or event ID must be provided."), settings);

                    return response;
                }
                else if (!parameters.broadcaster_id.IsNull() && !parameters.id.IsNull())
                {
                    response.SetInputError(new QueryParameterException("Only a boradcaster ID or event ID can be provided at one time."), settings);

                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (parameters.user_id.HasContent())
                {
                    // Pagination is ignored if a user ID was provided.
                    parameters.after = null;
                }

                if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.broadcaster_id), parameters.broadcaster_id, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.id), parameters.id, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.user_id), parameters.user_id, response, settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("subscriptions/events", Method.GET, pages, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<DataPage<SubscriptionEvent>> _response = await client.GetPagesAsync<SubscriptionEvent, DataPage<SubscriptionEvent>>(request, HandleResponse);
                response = new HelixResponse<DataPage<SubscriptionEvent>>(_response);

                return response;
            }

            #endregion

            #region /tags/streams - DONE

            /// <summary>
            /// Asynchronously gets specific stream tags, a single page, or a complete list of available stream stream tags.
            /// </summary>
            /// <param name="pages">How many pages to request.</param>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> containts the specific stream tags, the single page, or the complete list of available stream tags.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if any tag ID is null, empty, or contains only white space or any duplicate tag ID's are found, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if more than 100 total tag ID's are provided.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<StreamTag>>>
            GetStreamTagsAsync(RequestedPages pages, HelixAuthorization authorization, StreamTagsParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<DataPage<StreamTag>> response = new HelixResponse<DataPage<StreamTag>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response))
                {
                    return response;
                }

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    
                    if (!ValidateOptionalQueryParameter(nameof(parameters.tag_ids), parameters.tag_ids, 100, response, settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, settings))
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

                RestRequest request = GetBaseRequest("tags/streams", Method.GET, pages, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<DataPage<StreamTag>> _response = await client.GetPagesAsync<StreamTag, DataPage<StreamTag>>(request, HandleResponse);
                response = new HelixResponse<DataPage<StreamTag>>(_response);

                return response;
            }

            #endregion

            #region /users - DONE

            /// <summary>
            /// <para>Asynchronously gets a list of users.</para>
            /// <para>
            /// Optional scope: <see cref="HelixScopes.UserReadEmail"/>.
            /// If a Bearer token is provided provided, the email of the associated user is included in the response.
            /// </para>
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">
            /// A set of rest parameters.
            /// If parameters are not provided, the user is looked up by the specified bearer token.            
            /// </param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the list of users.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null and no Bearer token is provided.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if any user ID or user login is null, empty, or contains only white space.
            /// Thrown if any duplicate user ID's or user names are found.
            /// </exception>
            /// <exception cref="QueryParameterCountException">Thrown if none or more than 100 total user ID's and/or user logins are provided.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<User>>>
            GetUsersAsync(HelixAuthorization authorization, UsersParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<Data<User>> response = new HelixResponse<Data<User>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response))
                {
                    return response;
                }

                // Required parameter checks
                if (authorization.bearer_token.IsNull() && parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException("Parameters must be provided when no Bearer token is provided.", nameof(parameters)), settings);

                    return response;
                }

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    if (!parameters.ids.IsValid() && !parameters.logins.IsValid())
                    {
                        response.SetInputError(new QueryParameterCountException("At least one user ID or user login must be provided.", 100, 0), settings);

                        return response;
                    }

                    int count = parameters.ids.Count + parameters.logins.Count;
                    if (count > 100)
                    {
                        response.SetInputError(new QueryParameterCountException("A maximum of 100 total user ID's and/or logins can be provided at one time.", 100, count), settings);

                        return response;
                    }

                    // This will perform count checks again, but they will never be triggered if we get this far.
                    // This is really for checking for duplicates and no content indicies.
                    if (!ValidateOptionalQueryParameter(nameof(parameters.ids), parameters.ids, 100, response, settings) ||
                        !ValidateOptionalQueryParameter(nameof(parameters.logins), parameters.logins, 100, response, settings))
                    {
                        return response;
                    }
                }                

                RestRequest request = GetBaseRequest("users", Method.GET, authorization, settings);
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
            /// <para>Required scope: <see cref="HelixScopes.UserEdit"/>.</para>
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="description">The text to set the user's description to.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains updated user information.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="HelixScopes.UserEdit"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<User>>>
            SetUserDescriptionAsync(HelixAuthorization authorization, string description, HelixRequestSettings settings)
            {
                DescriptionParameters parameters = new DescriptionParameters();
                parameters.description = description;

                IHelixResponse<Data<User>> response = await SetUserDescriptionAsync(authorization, parameters, settings);

                return response;
            }

            /// <summary>
            /// <para>
            /// Asynchronously sets the description of a user.
            /// The user is implicitly specified by the provided Bearer token.
            /// </para>
            /// <para>Required scope: <see cref="HelixScopes.UserEdit"/>.</para>
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains updated user information.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="HelixScopes.UserEdit"/> scope.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<User>>>
            SetUserDescriptionAsync(HelixAuthorization authorization, DescriptionParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<Data<User>> response = new HelixResponse<Data<User>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response, HelixScopes.UserEdit))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                // Optional parameter checks
                if (!parameters.description.IsValid())
                {
                    parameters.description = string.Empty;
                }

                RestRequest request = GetBaseRequest("users", Method.PUT, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<Data<User>> _response = await client.ExecuteAsync<Data<User>>(request, HandleResponse);
                response = new HelixResponse<Data<User>>(_response);

                return response;
            }

            #endregion

            #region /users/extensions - DONE

            /// <summary>
            /// <para>
            /// Asynchronously gets a list of extensions a user has active.
            /// The user is implicitly specified by the provided Bearer token.
            /// </para>
            /// <para>Optional scopes: <see cref="HelixScopes.UserReadBroadcast"/> or <see cref="HelixScopes.UserEditBroadcast"/>.</para>
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">
            /// A set of rest parameters.
            /// If no user ID is specified, the user is implicityly specified from the bearer token.
            /// </param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the list of extensions a user has active.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null and no Bearer token is provided.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">Thrown if the user ID is empty or contains only white space, if provided.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<ActiveExtensions>>
            GetUserActiveExtensionsAsync(HelixAuthorization authorization, ActiveExtensionsParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<ActiveExtensions> response = new HelixResponse<ActiveExtensions>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response))
                {
                    return response;
                }

                // Required parameter checks
                if (authorization.bearer_token.IsNull() && parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException("Parameters must be provided when no Bearer token is provided.", nameof(parameters)), settings);

                    return response;
                }

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    if (!ValidateOptionalQueryParameter(nameof(parameters.user_id), parameters.user_id, response, settings))
                    {
                        return response;
                    }
                }

                RestRequest request = GetBaseRequest("users/extensions", Method.GET, authorization, settings);
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
            /// <para>Required scope: <see cref="HelixScopes.UserEditBroadcast"/>.</para>
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">
            /// <para>A set of rest parameters.</para>
            /// <para>
            /// Any extensions specified outside of the supported extension slots for each type are ignored.
            /// The supported extension slots for each type are specified under each <see cref="ActiveExtensionsData"/> member.
            /// </para>
            /// </param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the updated active extensions information.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="BodyParameterException">
            /// Thrown if no supported extension slots are found across all extension types.
            /// Thrown if thwe parameters data is null.
            /// Thrown if the extension ID or extension version for any active supported extension slot is null, empty, or contains only white space.            
            /// Thrown if either (x, y) coordinate for an active supported component extension slot exceeds the range (0, 0) to (8000, 5000).
            /// </exception>
            /// <exception cref="DuplicateExtensionException">Thrown if an extension ID is found in more then one active supported slot across all extension types.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="HelixScopes.UserEditBroadcast"/> scope</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<ActiveExtensions>>
            UpdateUserActiveExtensionsAsync(HelixAuthorization authorization, UpdateExtensionsParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<ActiveExtensions> response = new HelixResponse<ActiveExtensions>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response, HelixScopes.UserEditBroadcast))
                {
                    return response;
                }

                // Everything below here is valid going by the API and effectively behaves the same as performing a 'GET'.
                // It shouldn't be allowed. Don't allow any of this.
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                if (parameters.data.IsNull())
                {
                    response.SetInputError(new BodyParameterException("Value cannot be null", nameof(parameters.data), parameters.data), settings);

                    return response;
                }

                parameters.data.panel = ValidateExtensionSlots(parameters.data.panel, ExtensionType.Panel, response, settings);
                if (!response.exception.IsNull())
                {
                    return response;
                }

                parameters.data.overlay = ValidateExtensionSlots(parameters.data.overlay, ExtensionType.Overlay, response, settings);
                if (!response.exception.IsNull())
                {
                    return response;
                }

                parameters.data.component = ValidateExtensionSlots(parameters.data.component, ExtensionType.Component, response, settings);
                if (!response.exception.IsNull())
                {
                    return response;
                }

                if (!parameters.data.component.IsValid() && !parameters.data.panel.IsValid() && !parameters.data.overlay.IsValid())
                {
                    response.SetInputError(new BodyParameterException("No supported extension slots were provided, or all supported extension slots were null."), settings);

                    return response;
                }

                if (!ValidateUniqueExtensionIDs(parameters.data, response, settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("users/extensions", Method.PUT, authorization, settings);
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
            /// <para>Required Scope: <see cref="HelixScopes.UserReadBroadcast"/></para>
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the complete list of extensions the user has instlled, activated or deactivated..
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="AvailableScopesException">Thrown if the available scopes does not include the <see cref="HelixScopes.UserReadBroadcast"/> scope</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<Data<Extension>>>
            GetUserExtensionsAsync(HelixAuthorization authorization, HelixRequestSettings settings)
            {
                HelixResponse<Data<Extension>> response = new HelixResponse<Data<Extension>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response, HelixScopes.UserReadBroadcast))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("users/extensions/list", Method.GET, authorization, settings);

                RestResponse<Data<Extension>> _response = await client.ExecuteAsync<Data<Extension>>(request, HandleResponse);
                response = new HelixResponse<Data<Extension>>(_response);

                return response;
            }

            #endregion

            #region /users/follows - DONE

            /// <summary>
            /// Asynchronously gets a single page or the complete list of a user's following list.
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">
            /// A set of rest parameters.
            /// If provided, to_id is ignored.
            /// </param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page or the complete user's following list.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the from_id is not provided, empty, or contains only white space.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowingAsync(RequestedPages pages, HelixAuthorization authorization, FollowsParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<FollowsDataPage<Follow>> response = new HelixResponse<FollowsDataPage<Follow>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                if(!ValidateRequiredQueryParameter(nameof(parameters.from_id), parameters.from_id, response, settings))
                {
                    return response;
                }

                parameters.to_id = null;

                response = await GetUserFollowsRelationshipAsync(pages, authorization, parameters, settings) as HelixResponse<FollowsDataPage<Follow>>;

                return response;
            }

            /// <summary>
            /// Asynchronously gets a single page or a complete list of a user's followers list.
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">
            /// A set of rest parameters.
            /// If provided, from_id is ignored.
            /// </param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page or the complete user's followers list.
            /// </returns>        
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if the to_id is not provided, empty, or contains only white space.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowersAsync(RequestedPages pages, HelixAuthorization authorization, FollowsParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<FollowsDataPage<Follow>> response = new HelixResponse<FollowsDataPage<Follow>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                if (!ValidateRequiredQueryParameter(nameof(parameters.to_id), parameters.to_id, response, settings))
                {
                    return response;
                }

                parameters.from_id = null;

                response = await GetUserFollowsRelationshipAsync(pages, authorization, parameters, settings) as HelixResponse<FollowsDataPage<Follow>>;

                return response;
            }

            /// <summary>
            /// Asynchronously checks to see if the from_id user is following the to_id user.
            /// </summary>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="from_id">The ID of the following user.</param>
            /// <param name="to_id">The ID of the followed user.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> is set true if from_id is following to_id, otherwise false.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">Thrown if from_id or to_id are not provided, empty, or contains only white space.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<bool>>
            IsUserFollowingAsync(HelixAuthorization authorization, string from_id, string to_id, HelixRequestSettings settings)
            {
                HelixResponse<bool> response = new HelixResponse<bool>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response))
                {
                    return response;
                }

                // Required parameter checks
                if (!ValidateRequiredQueryParameter(nameof(to_id), to_id, response, settings) ||
                    !ValidateRequiredQueryParameter(nameof(from_id), from_id, response, settings))
                {
                    return response;
                }

                FollowsParameters parameters = new FollowsParameters(from_id, to_id);

                IHelixResponse<FollowsDataPage<Follow>> _response = await GetUserFollowsRelationshipAsync(RequestedPages.Single, authorization, parameters, settings);

                bool result = _response.exception.IsNull() ? _response.result.data.IsValid() : false;
                response = new HelixResponse<bool>(_response, result);

                return response;
            }

            /// <summary>
            /// Asynchronously gets the follow relationship between two users, a single page, or a complete list of a user's following/follower list.
            /// </summary>
            /// <param name="pages">How many pages to request.</param>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">
            /// <para>A set of rest parameters.</para>
            /// <para>At minimum, from_id or to_id must be provided.</para>
            /// </param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the user relationship page, the single page, or the complete following/follower list of one user.
            /// </returns> 
            /// <exception cref="ArgumentNullException">Thrown if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">
            /// Thrown if neither from_id and to_id are provided.
            /// Thrown if from_id or to_id are empty or contains only white space, if provided.
            /// Thrown if the after cursor is empty or contains only white space, if provided.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<FollowsDataPage<Follow>>>
            GetUserFollowsRelationshipAsync(RequestedPages pages, HelixAuthorization authorization, FollowsParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<FollowsDataPage<Follow>> response = new HelixResponse<FollowsDataPage<Follow>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                if (!parameters.to_id.IsValid() && !parameters.from_id.IsValid())
                {
                    response.SetInputError(new QueryParameterException("At minimum, a from_id or to_id must be provided and cannot be empty or contain only white space."), settings);

                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                if (!ValidateOptionalQueryParameter(nameof(parameters.to_id), parameters.to_id, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.from_id), parameters.from_id, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, settings))
                {
                    return response;
                }

                RestRequest request = GetBaseRequest("users/follows", Method.GET, pages, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<FollowsDataPage<Follow>> _response = await client.GetPagesAsync<Follow, FollowsDataPage<Follow>>(request, HandleResponse);
                response = new HelixResponse<FollowsDataPage<Follow>>(_response);

                return response;
            }

            #endregion

            #region /videos

            /// <summary>
            /// Asynchronously gets specific videos, a single page, or a complete list of videos.
            /// </summary>
            /// <param name="pages">How many pages to request.</param>
            /// <param name="authorization">Information used to authorize and/or authenticate the request.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <param name="settings">Information used to detemrtine how to handle assembling the requst and process response.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> containts the specific videos, the single page, or complete list of videos.
            /// </returns>
            /// <exception cref="ArgumentNullException">Throw if parameters is null.</exception>
            /// <exception cref="HeaderParameterException">Thrown if the Bearer token or Client ID is not provided, empty, or contains only white space.</exception>
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
            /// <exception cref="NotSupportedException">
            /// Thrown if a before cursor is provided.
            /// This is a temporary error and will be removed once Twitch fixes reverse pagination.
            /// </exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<DataPage<Video>>>
            GetVideosAsync(RequestedPages pages, HelixAuthorization authorization, VideosParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<DataPage<Video>> response = new HelixResponse<DataPage<Video>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response))
                {
                    return response;
                }

                // Required parameter checks
                if (parameters.IsNull())
                {
                    response.SetInputError(new ArgumentNullException(nameof(parameters)), settings);

                    return response;
                }

                if (!parameters.ids.IsValid() && parameters.user_id.IsNull() && parameters.game_id.IsNull())
                {
                    response.SetInputError(new QueryParameterException("At least one or more video ID, one user ID, or one game ID must be provided."), settings);

                    return response;
                }

                if ((parameters.ids.IsValid() && (!parameters.user_id.IsNull() || !parameters.game_id.IsNull())) ||
                   (!parameters.user_id.IsNull() && !parameters.game_id.IsNull()))
                {
                    response.SetInputError(new QueryParameterException("Only one or more video ID's, one user ID, or one game ID can be provided."), settings);

                    return response;
                }

                // Optional parameter checks
                parameters.first = parameters.first.Clamp(1, 100);

                string direction = string.Empty;

                if (!ValidateOptionalQueryParameter(nameof(parameters.user_id), parameters.user_id, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.game_id), parameters.game_id, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.ids), parameters.ids, 100, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, settings) ||
                    !ValidateOptionalQueryParameter(nameof(parameters.before), parameters.before, response, settings) ||
                    !ValidateCursorDiection(parameters.after, parameters.before, response, settings, out direction))
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

                // TODO: GetVideosAsync(...) - Temporarily disabling using 'before' while requesting all pages until it works properly. Reimplement 'before' when it works propery.
                if (parameters.before.IsValid())
                {
                    response.SetInputError(new NotSupportedException("The pagination direction 'before' is temporarily not supported. Following the cursor using 'before' returns incorrect results and does not work properly on Twitch's back end."), settings);

                    return response;
                }

                RestRequest request = GetBaseRequest("videos", Method.GET, pages, authorization, settings);
                request.AddParameters(parameters);

                // TODO: GetVideosAsync(...) - Resort the videos based on sort. Sometimes videos can be out of order.
                RestResponse<DataPage<Video>> _response = await client.GetPagesAsync<Video, DataPage<Video>>(request, direction, HandleResponse);
                response = new HelixResponse<DataPage<Video>>(_response);

                return response;
            }

            #endregion

            // Move to Rest.Helix.WebHooks or keep it on the same level?
            // TODO: Implement /webhook/hub

            #region /webhooks/subscriptions

            /// <summary>
            /// <para>Asynchronously gets a single page or a complete list of webhooks that a client is subscribed too.</para>
            /// <para>Required Authorization: App Access Token.</para>
            /// </summary>
            /// <param name="info">Information used to authorize and/or authenticate the request, and how to handle assembling the requst and process response.</param>
            /// <param name="parameters">A set of rest parameters.</param>
            /// <returns>
            /// Returns data that adheres to the <see cref="IHelixResponse{result_type}"/> interface.
            /// <see cref="IHelixResponse{result_type}.result"/> contains the single page or the complete list of webhooks that a client is subscribed too.
            /// </returns>
            /// <exception cref="HeaderParameterException">Thrown if the App Access token or Client ID is not provided, empty, or contains only white space.</exception>
            /// <exception cref="QueryParameterException">Thrown if the after cursor is empty or contains only white space, if provided.</exception>
            /// <exception cref="HelixException">Thrown if an error was returned by Twitch after executing the request.</exception>
            /// <exception cref="RetryLimitReachedException">Thrown if the retry limit was reached.</exception>
            /// <exception cref="HttpRequestException">Thrown if an underlying network error occurred.</exception>
            public static async Task<IHelixResponse<WebhookDataPage<WebhookSubscription>>>
            GetWebhookSubscriptionsAsync(RequestedPages pages, HelixAuthorization authorization, PagingParameters parameters, HelixRequestSettings settings)
            {
                HelixResponse<WebhookDataPage<WebhookSubscription>> response = new HelixResponse<WebhookDataPage<WebhookSubscription>>();
                if (!ValidateAuthorizatioHeaders(authorization, ref settings, response))
                {
                    return response;
                }

                // Optional parameter checks
                if (!parameters.IsNull())
                {
                    parameters.first = parameters.first.Clamp(1, 100);

                    if (!ValidateOptionalQueryParameter(nameof(parameters.after), parameters.after, response, settings))
                    {
                        return response;
                    }
                }

                RestRequest request = GetBaseRequest("webhooks/subscriptions", Method.GET, pages, authorization, settings);
                request.AddParameters(parameters);

                RestResponse<WebhookDataPage<WebhookSubscription>> _response = await client.GetPagesAsync<WebhookSubscription, WebhookDataPage<WebhookSubscription>>(request, HandleResponse);
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
            GetBaseRequest(string endpoint, Method method, HelixAuthorization authorization, HelixRequestSettings settings)
            {
                RestRequest request = new RestRequest(endpoint, method);
                request.AddHeader("Authorization", "Bearer " + authorization.bearer_token);
                request.AddHeader("Client-ID", authorization.client_id);
                request.settings = settings;

                return request;
            }

            internal static RestRequest
            GetBaseRequest(string endpoint, Method method, RequestedPages pages, HelixAuthorization authorization, HelixRequestSettings settings)
            {
                RestRequest request = new RestRequest(endpoint, method);
                request.AddHeader("Authorization", "Bearer " + authorization.bearer_token);
                request.AddHeader("Client-ID", authorization.client_id);
                request.pages = pages;
                request.settings = settings;

                return request;
            }

            #endregion

            #region Helpers - Request Validation 

            internal static bool
            ValidateAuthorizatioHeaders(HelixAuthorization authorization, ref HelixRequestSettings settings, HelixResponse response, HelixScopes required_scopes = default)
            {
                if (settings.IsNull())
                {
                    settings = new HelixRequestSettings();
                }

                // The most we can do is see if something was provided.
                // To be complete we would need to validate the OAuth token to see if the extarcted Clinet ID matches the provided Client ID
                // This is possible, but this would be adding an extra request and make it *significantly* more expensive.
                if (authorization.bearer_token.IsNull() || authorization.bearer_token.IsEmptyOrWhiteSpace())
                {
                    response.SetInputError(new HeaderParameterException("Value cannot be null, empty, or contain only whitespace.", nameof(authorization.bearer_token), authorization.bearer_token), settings);

                    return false;
                }

                if (authorization.client_id.IsNull() && authorization.client_id.IsEmptyOrWhiteSpace())
                {
                    response.SetInputError(new HeaderParameterException("Value cannot be empty or contain only whitespace.", nameof(authorization.client_id), authorization.client_id), settings);

                    return false;
                }

                // At least one scope is required to authorize the requets.
                if (required_scopes != 0)
                {
                    // Bearer token was not provided.
                    if (authorization.bearer_token.IsNull())
                    {
                        HelixScopes[] missing_scopes = EnumUtil.GetFlagValues<HelixScopes>(required_scopes);
                        AvailableScopesException inner_exception = new AvailableScopesException("One or more scopes are required to authorize the request.", missing_scopes);

                        response.SetInputError(new HeaderParameterException("A Bearer token must be provided to authenticate the request. See the inner exception for the list of required scopes.", nameof(authorization.bearer_token), inner_exception), settings);

                        return false;
                    }
                    // Available scopes have been specified.
                    else if (authorization.available_scopes != HelixScopes.Other)
                    {
                        HelixScopes[] available_scopes = EnumUtil.GetFlagValues<HelixScopes>(authorization.available_scopes);
                        foreach (HelixScopes scope in available_scopes)
                        {
                            if ((scope & required_scopes) == scope)
                            {
                                required_scopes ^= scope;
                            }
                        }

                        if (required_scopes != 0)
                        {
                            HelixScopes[] missing_scopes = EnumUtil.GetFlagValues<HelixScopes>(required_scopes);
                            response.SetScopesError(new AvailableScopesException("One or more scopes are missing from the provided available scopes associated with the Bearer token.", missing_scopes), settings);

                            return false;
                        }
                    }
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