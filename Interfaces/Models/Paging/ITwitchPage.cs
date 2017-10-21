// standard namespaces
using System.Collections.Generic;

// project namepspaces
using TwitchNet.Models.Api;

// imported .dll's
using Newtonsoft.Json;

namespace TwitchNet.Interfaces.Models.Paging
{
    public interface
    ITwitchPage<type>
    where type : class, new()
    {
        /// <summary>
        /// Contains the retured paged data.
        /// </summary>
        [JsonProperty("data")]
        IList<type> data        { get; }

        /// <summary>
        /// Contains information used when makling multi-pages requests.
        /// </summary>
        [JsonProperty("pagination")]
        Pagination  pagination  { get; }
    }
}
