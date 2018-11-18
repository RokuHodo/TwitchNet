using System;

using TwitchNet.Extensions;

namespace TwitchNet.Rest.Api
{
    public class
    HelixInfo
    {
        public string client_id;

        public string bearer_token;

        public HelixRequestSettings settings;

        public Scopes required_scopes;

        public HelixInfo(HelixRequestSettings settings)
        {
            this.settings = settings ?? new HelixRequestSettings();
        }
    }
}