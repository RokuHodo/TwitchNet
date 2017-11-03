// standrad namespaces
using System.Collections.Generic;

// imported .dll's
using Newtonsoft.Json;

namespace TwitchNet.Interfaces.Api
{
    public interface
    IApiValue<type>
    {
        /// <summary>
        /// Contains the response data.
        /// </summary>
        [JsonProperty("data")]
        IList<type> data { get; }
    }
}
