// standard namespaces
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Helpers.Json;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest
{
    [JsonConverter(typeof(EnumCacheConverter))]
    public enum
    Scopes
    {
        /// <summary>
        /// Unsupported scope.
        /// </summary>
        [EnumMember(Value = "")]
        Other = 0,

        /// <summary>
        /// analytics:read:games
        /// </summary>
        [EnumMember(Value = "analytics:read:games")]
        AnalyticsReadGames  = 1 << 0,

        /// <summary>
        /// bits:read
        /// </summary>
        [EnumMember(Value = "bits:read")]
        BitsRead            = 1 << 1,

        /// <summary>
        /// clips:edit
        /// </summary>
        [EnumMember(Value = "clips:edit")]
        ClipsEdit           = 1<< 2,

        /// <summary>
        /// user:edit
        /// </summary>
        [EnumMember(Value = "user:edit")]
        UserEdit            = 1 << 3,

        /// <summary>
        /// user:read:email
        /// </summary>
        [EnumMember(Value = "user:read:email")]
        UserReadEmail       = 1 << 4
    }
}
