// standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Interfaces.Models.Paging;

// imported .dll's
using Newtonsoft.Json;

namespace TwitchNet.Models.Api.Users
{
    public class
    FollowPage : ITwitchPage<Follow>
    {
        /// <summary>
        /// Contains the retured paged follow data.
        /// </summary>
        [JsonProperty("data")]
        public IList<Follow>    data        { get; protected set; }

        /// <summary>
        /// Contains information used when makling multi-pages requests.
        /// </summary>
        [JsonProperty("pagination")]
        public Pagination       pagination  { get; protected set; }
    }
}
