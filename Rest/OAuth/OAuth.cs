using System;
using System.Threading.Tasks;
using System.Web;
using TwitchNet.Rest.OAuth.Validate;
using TwitchNet.Extensions;
// project namespaces
using TwitchNet.Utilities;

// imported .dll's
using RestSharp;

namespace TwitchNet.Rest.OAuth
{
    public static class
    OAuthUtil
    {
        private static ClientInfo client_info = ClientInfo.DefaultOAuth2;

        #region /revoke

        public static IOAuthResponse
        RevokeToken(string client_id, string oauth_token, RestSettings settings = default(RestSettings))
        {
            IOAuthResponse response = RevokeTokenAsync(client_id, oauth_token, settings).Result;

            return response;
        }

        public static async Task<IOAuthResponse>
        RevokeTokenAsync(string client_id, string oauth_token, RestSettings settings = default(RestSettings))
        {
            if(settings.IsNull())
            {
                settings = RestSettings.Default;
            }

            if(settings.input_hanlding == InputHandling.Error)
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

        #region /refresh

        public static IOAuthResponse
        RefreshToken(string client_id, string client_secret, string refresh_token, RestSettings settings = default(RestSettings))
        {
            IOAuthResponse response = RefreshTokenAsync(client_secret, client_secret, refresh_token, settings).Result;

            return response;
        }

        public static async Task<IOAuthResponse>
        RefreshTokenAsync(string client_id, string client_secret, string refresh_token, RestSettings settings = default(RestSettings))
        {
            if (settings.IsNull())
            {
                settings = RestSettings.Default;
            }

            if (settings.input_hanlding == InputHandling.Error)
            {
                ExceptionUtil.ThrowIfInvalid(client_id, nameof(client_id));
                ExceptionUtil.ThrowIfInvalid(client_secret, nameof(client_secret));
                ExceptionUtil.ThrowIfInvalid(refresh_token, nameof(refresh_token));
            }

            RestRequest request = new RestRequest("token", Method.POST);
            request.AddQueryParameter("client_id", client_id);
            request.AddQueryParameter("client_secret", client_secret);
            request.AddQueryParameter("grant_type", "refresh_token");

            string refresh_token_encoded = HttpUtility.UrlEncode(refresh_token);
            request.AddQueryParameter("refresh_token", refresh_token_encoded);

            Tuple<IRestResponse, RestException, RateLimit> tuple = await RestUtil.ExecuteAsync(client_info, request, settings);

            IOAuthResponse response = new OAuthResponse(tuple.Item1, tuple.Item2);

            return response;
        }

        #endregion

        #region /validate

        public static IOAuthResponse<OAuthTokenInfo>
        ValidateToken(string oauth_token, RestSettings settings = default(RestSettings))
        {
            IOAuthResponse<OAuthTokenInfo> response = ValidateTokenAsync(oauth_token, settings).Result;

            return response;
        }

        public static async Task<IOAuthResponse<OAuthTokenInfo>>
        ValidateTokenAsync(string oauth_token, RestSettings settings = default(RestSettings))
        {
            if (settings.IsNull())
            {
                settings = RestSettings.Default;
            }

            if (settings.input_hanlding == InputHandling.Error)
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
    }
}
