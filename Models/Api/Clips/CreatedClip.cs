// imported .dll's
using Newtonsoft.Json;

namespace TwitchNet.Models.Api.Clips
{
    public class
    CreatedClip
    {
        /// <summary>
        /// The ID of the clip that was created.
        /// </summary>
        [JsonProperty("id")]
        public string id        { get; protected set; }

        /// <summary>
        /// The URL of the edit page for the clip.
        /// </summary>
        [JsonProperty("edit_url")]
        public string edit_url  { get; protected set; }
    }
}
