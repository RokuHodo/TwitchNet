// standard namespaces
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Helpers.Json;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Users
{
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
}