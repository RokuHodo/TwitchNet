// standard namespaces
using System.Collections.Generic;

// imported .dll's
using Newtonsoft.Json;

namespace TwitchNet.Models.Api.Streams
{
    public class Streams
    {
        /// <summary>
        /// Contains the retured paged follow data.
        /// </summary>
        [JsonProperty("data")]
        public List<Stream>   data        { get; protected set; }

        /// <summary>
        /// Contains information used when makling multi-pages requests.
        /// </summary>
        [JsonProperty("pagination")]
        public Pagination       pagination  { get; protected set; }
    }
}
