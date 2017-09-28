// imported .dll's
using Newtonsoft.Json;

namespace TwitchNet.Models.Api
{
    public class Pagination
    {
        [JsonProperty("cursor")]
        public string cursor { get; protected set; }
    }
}
