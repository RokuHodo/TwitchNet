// standard namespaces
using System.Collections.Generic;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Users
{
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
}