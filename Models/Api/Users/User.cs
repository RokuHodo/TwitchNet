// project namespaces
using TwitchNet.Enums;
using TwitchNet.Enums.Api.Users;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Models.Api.Users
{
    public class
    User
    {
        /// <summary>
        /// The id of the user.
        /// </summary>
        [JsonProperty("id")]
        public string           id                  { get; protected set; }

        /// <summary>
        /// The name of the user.
        /// </summary>
        [JsonProperty("login")]
        public string           login               { get; protected set; }

        /// <summary>
        /// The formatted name of the user.
        /// </summary>
        [JsonProperty("display_name")]
        public string           display_name        { get; protected set; }

        /// <summary>
        /// The user-type of the user.
        /// </summary>
        [JsonProperty("type")]
        public UserType         type                { get; protected set; }

        /// <summary>
        /// The broadcast-type of the user.
        /// </summary>
        [JsonProperty("broadcaster_type")]
        public BroadcasterType  broadcaster_type    { get; protected set; }

        /// <summary>
        /// The description located on the user's profile page.
        /// </summary>
        [JsonProperty("description")]
        public string           description         { get; protected set; }

        /// <summary>
        /// The URL of the profile image of the user.
        /// </summary>
        [JsonProperty("profile_image_url")]
        public string           profile_image_url   { get; protected set; }

        /// <summary>
        /// The URL of the offline image of the user.
        /// </summary>
        [JsonProperty("offline_image_url")]
        public string           offline_image_url   { get; protected set; }

        /// <summary>
        /// Number of users to visit the user's channel.
        /// </summary>
        [JsonProperty("view_count")]
        public uint             view_count          { get; protected set; }

        /// <summary>
        /// The user's email adress listed on their profile.
        /// This email is included only if the 'user:read:email' scope was specified when the Bearer token was created.
        /// Required Scope: 'user:read:email'
        /// </summary>
        [JsonProperty("email")]
        public string           email               { get; protected set; }
    }
}
