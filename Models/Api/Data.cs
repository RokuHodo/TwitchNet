// standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Interfaces.Api;

// project namespaces
using Newtonsoft.Json;

namespace
TwitchNet.Models.Api
{
    public class
    Data<data_type> : IData<data_type>
    {
        /// <summary>
        /// Contains the response data.
        /// </summary>
        [JsonProperty("data")]
        public IList<data_type> data { get; internal set; }
    }
}
