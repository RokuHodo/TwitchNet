// project namespaces
using TwitchNet.Models.Api;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Interfaces.Api
{
    public interface
    IDataPage<data_type> : IData<data_type>
    {
        /// <summary>
        /// A string used to tell the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [JsonProperty("pagination")]
        Pagination pagination { get; }
    }
}
