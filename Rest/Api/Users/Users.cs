// standard namespaces
using System.Collections.Generic;
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Helpers.Json;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Users
{
    public class
    UsersParameters
    {
        /// <summary>
        /// <para>A list of user ID's to query.</para>
        /// <para>
        /// A maximum of 100 total elements can be specified between ids and logins.
        /// All elements that are null, empty, or contain only whitespace are filtered out and all duplicate elements are removed before calculating the final count.
        /// </para>
        /// </summary>
        [QueryParameter("id", typeof(SeparateQueryConverter))]
        public virtual List<string> ids     { get; set; }

        /// <summary>
        /// <para>A list of user login names to query.</para>
        /// <para>
        /// A maximum of 100 total elements can be specified between ids and logins.
        /// All elements that are null, empty, or contain only whitespace are filtered out and all duplicate elements are removed before calculating the final count.
        /// </para>
        /// </summary>
        [QueryParameter("login", typeof(SeparateQueryConverter))]
        public virtual List<string> logins  { get; set; }

        public UsersParameters()
        {
            ids     = new List<string>();
            logins  = new List<string>();
        }
    }

    public class
    User
    {
        /// <summary>
        /// The id of the user.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; protected set; }

        /// <summary>
        /// The name of the user.
        /// </summary>
        [JsonProperty("login")]
        public string login { get; protected set; }

        /// <summary>
        /// The formatted name of the user.
        /// </summary>
        [JsonProperty("display_name")]
        public string display_name { get; protected set; }

        /// <summary>
        /// The user-type of the user.
        /// </summary>
        [JsonProperty("type")]
        public UserType type { get; protected set; }

        /// <summary>
        /// The broadcast-type of the user.
        /// </summary>
        [JsonProperty("broadcaster_type")]
        public BroadcasterType broadcaster_type { get; protected set; }

        /// <summary>
        /// The description located on the user's profile page.
        /// </summary>
        [JsonProperty("description")]
        public string description { get; protected set; }

        /// <summary>
        /// The URL of the profile image of the user.
        /// </summary>
        [JsonProperty("profile_image_url")]
        public string profile_image_url { get; protected set; }

        /// <summary>
        /// The URL of the offline image of the user.
        /// </summary>
        [JsonProperty("offline_image_url")]
        public string offline_image_url { get; protected set; }

        /// <summary>
        /// Number of users to visit the user's channel.
        /// </summary>
        [JsonProperty("view_count")]
        public uint view_count { get; protected set; }

        /// <summary>
        /// The user's email adress listed on their profile.
        /// This email is included only if the 'user:read:email' scope was specified when the Bearer token was created.
        /// Required Scope: 'user:read:email'
        /// </summary>
        [JsonProperty("email")]
        public string email { get; protected set; }
    }

    [JsonConverter(typeof(EnumConverter))]
    public enum
    BroadcasterType
    {
        /// <summary>
        /// The broadcaster is a normal user.
        /// </summary>
        [EnumMember(Value = "")]
        Empty = 0,

        /// <summary>
        /// The broadcaster is a Twitch partner.
        /// </summary>
        [EnumMember(Value = "partner")]
        Partner = 1,

        /// <summary>
        /// The broadcaster is a Twitch affiliate.
        /// </summary>
        [EnumMember(Value = "affiliate")]
        Affiliate = 2
    }

    public class
    DescriptionParameters
    {
        /// <summary>
        /// The text to set the user's description to.
        /// </summary>
        [QueryParameter("description")]
        public virtual string description { get; set; }
    }
}
