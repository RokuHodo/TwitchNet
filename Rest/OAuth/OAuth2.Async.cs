// standard namespaces
using System;
using System.Threading.Tasks;
using System.Web;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Rest.OAuth.Token;
using TwitchNet.Rest.OAuth.Validate;
using TwitchNet.Utilities;

namespace
TwitchNet.Rest.OAuth
{
    public static partial class
    OAuth2
    {
        /*
        #region /revoke

        /// <summary>
        /// Revokes the use of an OAuth token.
        /// Once revoked, the OAuth token is invalidated and can no longer be used to make requests.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="oauth_token">The OAuthg token to revoke.</param>
        /// <param name="settings">Settings to customize how the request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IOAuthResponse"/> interface.</returns>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="oauth_token"/> or <paramref name="client_id"/> is null, empty, or only white space.</exception>
        public static IOAuthResponse
        RevokeToken(string client_id, string oauth_token, RequestSettings settings = default)
        {
            IOAuthResponse response = RevokeTokenAsync(client_id, oauth_token, settings).Result;

            return response;
        }

        /// <summary>
        /// Asynchronously revokes the use of an OAuth token.
        /// Once revoked, the OAuth token is invalidated and can no longer be used to make requests.
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="oauth_token">The OAuthg token to revoke.</param>
        /// <param name="settings">Settings to customize how the request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IOAuthResponse"/> interface.</returns>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="oauth_token"/> or <paramref name="client_id"/> is null, empty, or only white space.</exception>
        public static async Task<IOAuthResponse>
        RevokeTokenAsync(string client_id, string oauth_token, RequestSettings settings = default)
        {
            if(settings.IsNull())
            {
                settings = RequestSettings.Default;
            }

            if(settings.error_handling_inputs == ErrorHandling.Error)
            {
                ExceptionUtil.ThrowIfInvalid(client_id, nameof(client_id));
                ExceptionUtil.ThrowIfInvalid(oauth_token, nameof(oauth_token));
            }

            RestRequest request = new RestRequest("revoke", Method.POST);
            request.AddQueryParameter("client_id", client_id);
            request.AddQueryParameter("token", oauth_token);

            Tuple<IRestResponse, RestException, RateLimit> tuple = await RestUtil.ExecuteAsync(client_info, request, settings);

            IOAuthResponse response = new OAuthResponse(tuple.Item1, tuple.Item2);

            return response;
        }

        #endregion

        #region /token

        /// <summary>
        /// <para>Tefreshes when an OAuth token expires, extending how long it is valid/can be used.</para>
        /// <para>This only works on user access tokens and not on app access tokens or ID tokens.</para>
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="refresh_token">
        /// The token used to refresh the OAuth token.
        /// This is NOT the actual OAuth token being refreshed.
        /// </param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IOAuthResponse"/> interface.</returns>
        /// 
        public static IOAuthResponse
        RefreshToken(string client_id, string refresh_token, RefreshTokenParameters parameters, RequestSettings settings = default)
        {
            IOAuthResponse response = RefreshTokenAsync(client_id, refresh_token, parameters, settings).Result;

            return response;
        }

        /// <summary>
        /// <para>Asynchronously refreshes when an OAuth token expires, extending how long it is valid/can be used.</para>
        /// <para>This only works on user access tokens and not on app access tokens or ID tokens.</para>
        /// </summary>
        /// <param name="client_id">The Client ID to identify the application making the request.</param>
        /// <param name="refresh_token">
        /// The token used to refresh the OAuth token.
        /// This is NOT the actual OAuth token being refreshed.
        /// </param>
        /// <param name="parameters">A set of rest parameters to customize the request.</param>
        /// <param name="settings">Settings to customize how the request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IOAuthResponse"/> interface.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="parameters"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="client_id"/>, <paramref name="refresh_token"/>, or <see cref="RefreshTokenParameters.client_secret"/> is null, empty, or only white space.</exception>
        public static async Task<IOAuthResponse>
        RefreshTokenAsync(string client_id, string refresh_token, RefreshTokenParameters parameters, RequestSettings settings = default)
        {
            if (settings.IsNull())
            {
                settings = RequestSettings.Default;
            }

            if (settings.error_handling_inputs == ErrorHandling.Error)
            {
                ExceptionUtil.ThrowIfInvalid(client_id, nameof(client_id));
                ExceptionUtil.ThrowIfInvalid(refresh_token, nameof(refresh_token));
                ExceptionUtil.ThrowIfNull(parameters, nameof(parameters));
                ExceptionUtil.ThrowIfInvalid(parameters.client_secret, nameof(parameters.client_secret));
            }

            RestRequest request = new RestRequest("token", Method.POST);
            request.AddQueryParameter("client_id", client_id);
            request.AddQueryParameter("grant_type", "refresh_token");

            string refresh_token_encoded = HttpUtility.UrlEncode(refresh_token);
            request.AddQueryParameter("refresh_token", refresh_token_encoded);

            request = request.AddPaging(parameters);

            Tuple<IRestResponse, RestException, RateLimit> tuple = await RestUtil.ExecuteAsync(client_info, request, settings);

            IOAuthResponse response = new OAuthResponse(tuple.Item1, tuple.Item2);

            return response;
        }

        #endregion

        #region /validate

        /// <summary>
        /// Validates an OAuth token and returns information associated with the token.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to validate.</param>
        /// <param name="settings">Settings to customize how the request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IOAuthResponse{result_type}"/> interface.</returns>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="oauth_token"/> is null, empty, or only white space.</exception>
        public static IOAuthResponse<OAuthTokenInfo>
        ValidateToken(string oauth_token, RequestSettings settings = default)
        {
            IOAuthResponse<OAuthTokenInfo> response = ValidateTokenAsync(oauth_token, settings).Result;

            return response;
        }

        /// <summary>
        /// Asynchronously validates an OAuth token and returns information associated with the token.
        /// </summary>
        /// <param name="oauth_token">The OAuth token to validate.</param>
        /// <param name="settings">Settings to customize how the request is handled.</param>
        /// <returns>Returns data that adheres to the <see cref="IOAuthResponse{result_type}"/> interface.</returns>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="oauth_token"/> is null, empty, or only white space.</exception>
        public static async Task<IOAuthResponse<OAuthTokenInfo>>
        ValidateTokenAsync(string oauth_token, RequestSettings settings = default)
        {
            if (settings.IsNull())
            {
                settings = RequestSettings.Default;
            }

            if (settings.error_handling_inputs == ErrorHandling.Error)
            {
                ExceptionUtil.ThrowIfInvalid(oauth_token, nameof(oauth_token));
            }

            RestRequest request = new RestRequest("validate", Method.GET);
            request.AddHeader("Authorization", "OAuth " + oauth_token);

            Tuple<IRestResponse<OAuthTokenInfo>, RestException, RateLimit> tuple = await RestUtil.ExecuteAsync<OAuthTokenInfo>(client_info, request, settings);

            IOAuthResponse<OAuthTokenInfo> response = new OAuthResponse<OAuthTokenInfo>(tuple.Item1, tuple.Item2);

            return response;
        }

        #endregion
        */

        public static async Task<IRestResponse<AppAccessTokenData>>
        CreateAppAccessToken(string client_id, string client_secret, AppAccessTokenParameters parameters, RequestSettings settings = default)
        {
            OAuth2Info info = new OAuth2Info(settings);
            info.client_id = client_id;
            info.client_secret = client_secret;

            IRestResponse<AppAccessTokenData> response = await Internal.CreateAppAccessToken(info, parameters);

            return response;
        }
    }
}
