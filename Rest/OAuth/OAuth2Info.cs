using System;

using TwitchNet.Extensions;

namespace TwitchNet.Rest.OAuth
{
    public class
    OAuth2Info
    {
        public string client_id;

        public string client_secret;

        public RequestSettings settings;

        public OAuth2Info(RequestSettings settings)
        {
            this.settings = settings ?? new RequestSettings();
        }
    }
}