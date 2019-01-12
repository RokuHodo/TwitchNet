// project namespaces
using TwitchNet.Helpers;

namespace
TwitchNet.Rest.Api
{
    public class
    PagingParameters : IPagingParameters
    {
        /// <summary>
        /// <para>Maximum number of objects to return.</para>
        /// <para>
        /// Min:        1,
        /// Max:        100,
        /// Default:    20.
        /// The value is clamped between the minimum and the maximum values.
        /// </para>
        /// </summary>
        [QueryParameter("first")]
        public int? first { get; set; }

        /// <summary>
        /// The cursor that tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [QueryParameter("after")]
        public string after { get; set; }
    }
}
