// project namespaces
using TwitchNet.Helpers.Json;

namespace TwitchNet.Models.Api
{
    public struct
    ClientInfo
    {
        /// <summary>
        /// The base API URL.
        /// </summary>
        public string           base_url;

        /// <summary>
        /// The handlers used while deserializing the HTTP response.
        /// </summary>
        public ClientHandler[]  handlers;

        /// <summary>
        /// Creates a new instance of the <see cref="ClientInfo"/> struct.
        /// </summary>
        /// <param name="base_url">The base API URL.</param>
        /// <param name="handlers">The handlers used while deserializing the HTTP response.</param>
        public ClientInfo(string base_url, ClientHandler[] handlers)
        {
            this.base_url   = base_url;

            this.handlers   = handlers;
        }

        /// <summary>
        /// Returns the default Helix API <see cref="ClientInfo"/>.
        /// </summary>
        public static readonly ClientInfo DefaultHelix = new ClientInfo()
        {
            base_url = "https://api.twitch.tv/helix",
            handlers = new ClientHandler[]
            {
                new ClientHandler("application/json", new JsonDeserializer()),
                new ClientHandler("application/xml", new JsonDeserializer()),
            }
        };

        /// <summary>
        /// Returns the default OAuth2 <see cref="ClientInfo"/>.
        /// </summary>
        public static readonly ClientInfo DefaultOAuth2 = new ClientInfo()
        {
            base_url = "https://id.twitch.tv/oauth2",
            handlers = new ClientHandler[]
            {
                new ClientHandler("application/json", new JsonDeserializer()),
                new ClientHandler("application/xml", new JsonDeserializer()),
            }
        };
    }
}
