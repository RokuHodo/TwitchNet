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
    ApiData<type> : IApiValue<type>
    where type : class, new()
    {
        /// <summary>
        /// Contains the response data.
        /// </summary>
        [JsonProperty("data")]
        public IList<type> data { get; internal set; }
    }
}
