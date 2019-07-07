// standard namespaces
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Helpers.Json;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Helix
{
    #region /users

    public class
    UsersParameters
    {
        /// <summary>
        /// <para>A list of user ID's to query.</para>
        /// <para>A maximum of 100 total elements can be specified between ids and logins.</para>
        /// </summary>
        [QueryParameter("id", typeof(SeparateQueryConverter))]
        public virtual List<string> ids     { get; set; }

        /// <summary>
        /// <para>A list of user login names to query.</para>
        /// <para>A maximum of 100 total elements can be specified between ids and logins.</para>
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

    #endregion

    #region /users/extensions

    public class
    ActiveExtensionsParameters
    {
        /// <summary>
        /// The ID of the user to query.
        /// </summary>
        [QueryParameter("user_id")]
        public virtual string user_id { get; set; }
    }

    public class
    ActiveExtensions
    {
        /// <summary>
        /// The active extension data to update.
        /// </summary>
        [JsonProperty("data")]
        public ActiveExtensionsData data { get; set; }
    }

    public class
    ActiveExtension
    {
        /// <summary>
        /// <para>Whether or not the extension is active.</para>
        /// <para>Set to false if no activation state is provided.</para>
        /// </summary>
        [JsonProperty("active")]
        public bool active { get; set; }

        /// <summary>
        /// The extension ID.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; set; }

        /// <summary>
        /// The extension verison.
        /// </summary>
        [JsonProperty("version")]
        public string version { get; set; }

        /// <summary>
        /// The extension name.
        /// </summary>
        [JsonProperty("name")]
        public string name { get; set; }

        /// <summary>
        /// Valid for <see cref="ExtensionType.Component"/> extensions only.
        /// The x-coordinate of the extension.
        /// </summary>
        [JsonProperty("x")]
        public int? x { get; set; }

        /// <summary>
        /// Valid for <see cref="ExtensionType.Component"/> extensions only.
        /// The Y-coordinate of the extension.
        /// </summary>
        [JsonProperty("y")]
        public int? y { get; set; }
    }

    public class
    UpdateExtensionsParameters
    {
        /// <summary>
        /// The active extension data to update.
        /// </summary>
        [Body("data")]
        public ActiveExtensionsData data { get; set; }

        public
        UpdateExtensionsParameters()
        {
            data = new ActiveExtensionsData();
        }
    }

    public class
    ActiveExtensionsData
    {
        /// <summary>
        /// <para>Contains data for panel extensions.</para>
        /// <para>Valid keys: 1, 2, 3.</para>
        /// </summary>
        [JsonProperty("panel")]
        public Dictionary<string, ActiveExtension> panel { get; set; }

        /// <summary>
        /// <para>Contains data for overlay extensions.</para>
        /// <para>Valid keys: 1.</para>
        /// </summary>
        [JsonProperty("overlay")]
        public Dictionary<string, ActiveExtension> overlay { get; set; }

        /// <summary>
        /// <para>Contains data for component extensions.</para>
        /// <para>Valid keys: 1, 2.</para>
        /// </summary>
        [JsonProperty("component")]
        public Dictionary<string, ActiveExtension> component { get; set; }

        public
        ActiveExtensionsData()
        {
            component = new Dictionary<string, ActiveExtension>();
            panel = new Dictionary<string, ActiveExtension>();
            overlay = new Dictionary<string, ActiveExtension>();
        }
    }

    public class
    DuplicateExtensionException : BodyParameterException
    {
        public string extension_name { get; protected set; }
        public string extension_id { get; protected set; }
        public string extension_version { get; protected set; }

        public DuplicateExtensionException(ActiveExtension extension, string message = null) : base(message)
        {
            extension_id = extension.id;
            extension_name = extension.name;
            extension_version = extension.version;
        }
    }

    #endregion

    #region /users/extensions/list

    public class
    Extension
    {
        /// <summary>
        /// The extension ID.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; protected set; }

        /// <summary>
        /// The extension verison.
        /// </summary>
        [JsonProperty("version")]
        public string version { get; protected set; }

        /// <summary>
        /// The extension name.
        /// </summary>
        [JsonProperty("name")]
        public string name { get; protected set; }

        /// <summary>
        /// Determines if the extension can be activated.
        /// </summary>
        [JsonProperty("can_activate")]
        public bool can_activate { get; protected set; }

        /// <summary>
        /// The different types that the extension can be activated.
        /// </summary>
        [JsonProperty("type")]
        public ExtensionType[] type { get; protected set; }
    }

    [JsonConverter(typeof(EnumConverter))]
    public enum
    ExtensionType
    {
        /// <summary>
        /// Unsupported extension type.
        /// </summary>
        [EnumMember(Value = "")]
        Other = 0,

        /// <summary>
        /// Component extension.
        /// </summary>
        [EnumMember(Value = "component")]
        Component,

        /// <summary>
        /// Mobile extension.
        /// </summary>
        [EnumMember(Value = "mobile")]
        Mobile,

        /// <summary>
        /// Panel extension.
        /// </summary>
        [EnumMember(Value = "panel")]
        Panel,

        /// <summary>
        /// Stream overlay extension.
        /// </summary>
        [EnumMember(Value = "overlay")]
        Overlay
    }

    #endregion

    #region /users/follows

    public class
    FollowsParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// A user's id.
        /// The request returns information about users who are being followed by the this user.
        /// </summary>
        [QueryParameter("from_id")]
        public virtual string from_id { get; set; }

        /// <summary>
        /// A user's id.
        /// The request returns information about users who are following this user.
        /// </summary>
        [QueryParameter("to_id")]
        public virtual string to_id { get; set; }

        public FollowsParameters()
        {

        }

        public FollowsParameters(string from_id, string to_id)
        {
            this.from_id = from_id;
            this.to_id = to_id;
        }
    }

    public class
    FollowsDataPage<data_type> : DataPage<data_type>
    {
        /// <summary>
        /// When providing only 'from_id', 'total' represents the number of users a user is following.
        /// When providing only 'to_id', 'total' represents the total number of followers the user has.
        /// When providing both 'from_id' and 'to_id', 'total' is either '1' or '0' depending if 'from_id' is following 'to_id'.
        /// </summary>
        [JsonProperty("total")]
        public int total { get; protected set; }
    }

    public class
    FollowsData<data_type> : Data<data_type>
    {
        /// <summary>
        /// When providing only 'from_id', 'total' represents the number of users a user is following.
        /// When providing only 'to_id', 'total' represents the total number of followers the user has.
        /// When providing both 'from_id' and 'to_id', 'total' is either '1' or '0' depending if 'from_id' is following 'to_id'.
        /// </summary>
        [JsonProperty("total")]
        public string total { get; protected set; }
    }

    public class
    Follow
    {
        /// <summary>
        /// <para>The ID of the following user.</para>
        /// <para>
        /// This value will be constant when a user's following list is returned.
        /// This value will vary when a user's follower list is returned.
        /// </para>
        /// </summary>
        [JsonProperty("from_id")]
        public string from_id { get; protected set; }

        /// <summary>
        /// <para>The login name of the following user.</para>
        /// <para>
        /// This value will be constant when a user's following list is returned.
        /// This value will vary when a user's follower list is returned.
        /// </para>
        /// </summary>
        [JsonProperty("from_name")]
        public string from_name { get; protected set; }

        /// <summary>
        /// <para>The ID of the followed user.</para>
        /// <para>
        /// This value will vary when a user's following list is returned.
        /// This value will be constant when a user's follower list is returned.
        /// </para>
        /// </summary>
        [JsonProperty("to_id")]
        public string to_id { get; protected set; }

        /// <summary>
        /// <para>The login name of the followed user.</para>
        /// <para>
        /// This value will vary when a user's following list is returned.
        /// This value will be constant when a user's follower list is returned.
        /// </para>
        /// </summary>
        [JsonProperty("to_name")]
        public string to_name { get; protected set; }

        /// <summary>
        /// The date that the from_id user followed the to_id user.
        /// </summary>
        [JsonProperty("followed_at")]
        public DateTime followed_at { get; protected set; }
    }

    #endregion
}
