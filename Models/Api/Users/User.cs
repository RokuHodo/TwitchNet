// imported .dll's
using Newtonsoft.Json;

namespace TwitchNet.Models.Api.Users
{
    public class User
    {
        [JsonProperty("id")]
        public string   id                  { get; protected set; }

        [JsonProperty("login")]
        public string   login               { get; protected set; }

        [JsonProperty("display_name")]
        public string   display_name        { get; protected set; }

        [JsonProperty("type")]
        public string   type                { get; protected set; }

        [JsonProperty("broadcaster_type")]
        public string   broadcaster_type    { get; protected set; }

        [JsonProperty("description")]
        public string   description         { get; protected set; }

        [JsonProperty("profile_image_url")]
        public string   profile_image_url   { get; protected set; }

        [JsonProperty("offline_image_url")]
        public string   offline_image_url   { get; protected set; }

        [JsonProperty("view_count")]
        public uint     view_count          { get; protected set; }

        // NOTE: (Models.Api.Users) - 'email' property only filled out if user:read:email scope is passed
        [JsonProperty("email")]
        public string   email               { get; protected set; }
    }
}
