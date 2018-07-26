// project namespaces
using TwitchNet.Helpers.Json;

using RestSharp;

namespace
TwitchNet.Rest
{
    public static class
    RestClients
    {
        public static readonly RestClient Helix = CreateHelixClient();

        public static readonly RestClient OAuth2 = CreateOAuth2Client();

        private static RestClient
        CreateHelixClient()
        {
            RestClient client = new RestClient("https://api.twitch.tv/helix");
            client.AddHandler("application/json", new JsonDeserializer());
            client.AddHandler("application/xml", new JsonDeserializer());

            return client;
        }

        private static RestClient
        CreateOAuth2Client()
        {
            RestClient client = new RestClient("https://id.twitch.tv/oauth2");
            client.AddHandler("application/json", new JsonDeserializer());
            client.AddHandler("application/xml", new JsonDeserializer());

            return client;
        }
    }
}
