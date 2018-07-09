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
        /// <summary>
        /// <para>1st analytic report type.</para>
        /// <para>Default report start date: 90 days before the end date.</para>
        /// </summary>
        [EnumMember(Value = "overview_1")]
        Overview1 = 0,

        /// <summary>
        /// <para>2nd analytic report type.</para>
        /// <para>Default report start date: 365 days before the end date.</para>
        /// </summary>
        [EnumMember(Value = "overview_2")]
        Overview2
    }
}
