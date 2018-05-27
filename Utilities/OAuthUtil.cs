
using TwitchNet.Enums.Api;
using TwitchNet.Models.Api;
using TwitchNet.Extensions;

using RestSharp;

namespace TwitchNet.Utilities
{
    public static class
    OAuthUtil
    {
        private static ClientInfo client_info = ClientInfo.DefaultOAuth2;

        public static object
        RevokeToken(string client_id, string token, RestSettings settings = default(RestSettings))
        {
            if(settings.IsNull())
            {
                settings = RestSettings.Default;
            }

            if(settings.input_hanlding == InputHandling.Error)
            {
                ExceptionUtil.ThrowIfInvalid(client_id, nameof(client_id));
                ExceptionUtil.ThrowIfInvalid(token, nameof(token));
            }

            RestRequest request = new RestRequest("revoke", Method.POST);
            request.AddQueryParameter("client_id", client_id);
            request.AddQueryParameter("token", token);

            RestClient client = new RestClient(client_info.base_url);
            foreach(ClientHandler hanbdler in client_info.handlers)
            {
                client.AddHandler(hanbdler.content_type, hanbdler.deserializer);
            }

            IRestResponse response =  client.Execute(request);

            return response;
        }
    }
}
