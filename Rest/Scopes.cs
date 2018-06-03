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
        AnalyticsReadGames,

        /// <summary>
        /// bits:read
        /// </summary>
        BitsRead,

        /// <summary>
        /// clips:edit
        /// </summary>
        ClipsEdit,

        /// <summary>
        /// user:edit
        /// </summary>
        UserEdit,

        /// <summary>
        /// user:read:email
        /// </summary>
        UserReadEmail
    }
}
