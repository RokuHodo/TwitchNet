// standard namespaces
using System.Runtime.Serialization;

// projetc namespaces
using TwitchNet.Helpers.Json;

// imported .dll's
using Newtonsoft.Json;

namespace TwitchNet.Rest.Api.Analytics
{
    [JsonConverter(typeof(EnumConverter))]
    public enum
    GameAnalyticsType
    {
        [EnumMember(Value = "overview_1")]
        Overview1 = 0,

        [EnumMember(Value = "overview_2")]
        Overview2
    }
}
