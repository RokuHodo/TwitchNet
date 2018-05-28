// standard namespaces
using System.Runtime.Serialization;

// imported .dll's
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace
TwitchNet.Rest.Api
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum
    Scopes
    {
        [EnumMember(Value = "")]
        None = 0,

        [EnumMember(Value = "analytics:read:games")]
        AnalyticsReadGames,

        [EnumMember(Value = "bits:read")]
        BitsRead,

        [EnumMember(Value = "clips:edit")]
        ClipsEdit,

        [EnumMember(Value = "user:edit")]
        UserEdit,

        [EnumMember(Value = "user:read:email")]
        UserReadEmail
    }
}
