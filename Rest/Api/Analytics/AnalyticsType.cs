// standard namespaces
using System.Runtime.Serialization;

// projetc namespaces
using TwitchNet.Helpers.Json;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Analytics
{
    [JsonConverter(typeof(EnumConverter))]
    public enum
    AnalyticsType
    {
        /// <summary>
        /// The 1st analytic report type.
        /// </summary>
        [EnumMember(Value = "overview_v1")]
        OverviewV1 = 0,

        /// <summary>
        /// The 2nd analytic report type.
        /// </summary>
        [EnumMember(Value = "overview_v2")]
        OverviewV2
    }
}
