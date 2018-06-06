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
        Other = 0,

        /// <summary>
        /// analytics:read:games
        /// </summary>
        AnalyticsReadGames  = 1 << 0,

        /// <summary>
        /// bits:read
        /// </summary>
        BitsRead            = 1 << 1,

        /// <summary>
        /// clips:edit
        /// </summary>
        ClipsEdit           = 1<< 2,

        /// <summary>
        /// user:edit
        /// </summary>
        UserEdit            = 1 << 3,

        /// <summary>
        /// user:read:email
        /// </summary>
        UserReadEmail       = 1 << 4
    }
}
