// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api
{
    public class
    Pagination
    {
        /// <summary>
        /// A string used to tell the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [JsonProperty("cursor")]
        public string cursor { get; internal set; }
    }
}
