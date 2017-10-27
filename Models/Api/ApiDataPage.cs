// project namespaces
using TwitchNet.Interfaces.Api;

// project namespaces
using Newtonsoft.Json;

namespace TwitchNet.Models.Api
{
    public class
    ApiDataPage<type> : ApiData<type>, IApiDataPage<type>
    where type : class, new()
    {
        /// <summary>
        /// Contains information used when makling multi-page requests.
        /// </summary>
        [JsonProperty("pagination")]
        public Pagination pagination { get; internal set; }
    }
}
