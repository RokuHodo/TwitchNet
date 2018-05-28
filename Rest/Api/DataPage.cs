// project namespaces
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api
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
