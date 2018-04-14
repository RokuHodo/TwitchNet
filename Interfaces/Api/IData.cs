// standard namespaces
using System.Collections.Generic;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Interfaces.Api
{
    public interface
    IData<data_type>
    {
        /// <summary>
        /// Contains the response data.
        /// </summary>
        [JsonProperty("data")]
        IList<data_type> data { get; }
    }
}
