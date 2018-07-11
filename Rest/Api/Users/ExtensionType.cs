// standard namespaces
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Helpers.Json;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Users
{
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
