﻿// standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Interfaces.Models.Paging;

// imported .dll's
using Newtonsoft.Json;

namespace TwitchNet.Models.Api.Streams
{
    public class
    StreamPage : ITwitchPage<Stream>
    {
        /// <summary>
        /// Contains the retured paged follow data.
        /// </summary>
        [JsonProperty("data")]
        public IList<Stream>    data       { get; protected set; }

        /// <summary>
        /// Contains information used when makling multi-pages requests.
        /// </summary>
        [JsonProperty("pagination")]
        public Pagination       pagination  { get; protected set; }
    }
}
