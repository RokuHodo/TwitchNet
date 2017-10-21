// standard namespaces
using System.Collections.Generic;

// projetc namespaces
using TwitchNet.Interfaces.Models.Paging;

// imported .dll's
using Newtonsoft.Json;

namespace TwitchNet.Models.Api.Streams
{
    public class
    MetadataPage : ITwitchPage<Metadata>
    {
        /// <summary>
        /// The Hearthstone metadata of the current stream, if that is the game being played.
        /// </summary>
        [JsonProperty("data")]
        public IList<Metadata>  data        { get; protected set; }

        /// <summary>
        /// Contains information used when makling multi-pages requests.
        /// </summary>
        [JsonProperty("pagination")]
        public Pagination       pagination  { get; protected set; }
    }
}
