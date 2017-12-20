// project namespaces
using TwitchNet.Interfaces.Api;

// project namespaces
using Newtonsoft.Json;

namespace
TwitchNet.Models.Api
{
    public class
    DataPage<data_type> : Data<data_type>, IDataPage<data_type>
    {
        /// <summary>
        /// Contains information used when makling multi-page requests.
        /// </summary>
        [JsonProperty("pagination")]
        public Pagination pagination { get; internal set; }
    }
}
