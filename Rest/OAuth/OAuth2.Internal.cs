using System.Threading.Tasks;

using TwitchNet.Extensions;
using TwitchNet.Rest.OAuth.Token;

namespace
TwitchNet.Rest.OAuth
{
    public static partial class
	OAuth2
    {
		internal class
        Internal
        {
            internal static readonly RestClient client = GetOAuth2Client();

            #region Helpers

            internal static RestClient
            GetOAuth2Client()
            {
                RestClient client = new RestClient("https://id.twitch.tv/oauth2");

                return client;
            }

            internal static RestRequest
            GetBaseRequest(string endpoint, Method method, OAuth2Info info)
            {
                RestRequest request = new RestRequest(endpoint, method);
                request.settings = info.settings;

                if (info.client_id.IsValid())
                {
                    request.AddQueryParameter("client_id", info.client_id);
                }

                if (info.client_secret.IsValid())
                {
                    request.AddQueryParameter("client_secret", info.client_secret);
                }

                return request;
            }

            #endregion

            public static async Task<IRestResponse<AppAccessTokenData>>
			CreateAppAccessToken(OAuth2Info info, AppAccessTokenParameters parameters)
            {
				// TODO: Error checking.
                RestRequest request = GetBaseRequest("token", Method.POST, info);
                request.AddParameters(parameters);

                RestResponse<AppAccessTokenData> response = await client.ExecuteAsync<AppAccessTokenData>(request);

                return response;
            }
        }
    }
}
