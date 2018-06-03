// imported .dll's
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace
TwitchNet.Rest
{
    // TODO: Add custom converter for the deserializer.
    [JsonConverter(typeof(StringEnumConverter))]
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
