// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Users
{
    public class
    ActiveExtension
    {
        /// <summary>
        /// <para>Whether or not the extension is active.</para>
        /// <para>Set to false if no activation state is provided.</para>
        /// </summary>
        [JsonProperty("active")]
        public bool active { get; set; }

        /// <summary>
        /// The extension ID.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; set; }

        /// <summary>
        /// The extension verison.
        /// </summary>
        [JsonProperty("version")]
        public string version { get; set; }

        /// <summary>
        /// The extension name.
        /// </summary>
        [JsonProperty("name")]
        public string name { get; set; }

        /// <summary>
        /// Valid for <see cref="ExtensionType.Component"/> extensions only.
        /// The x-coordinate of the extension.
        /// </summary>
        [JsonProperty("x")]
        public int x { get; set; }

        /// <summary>
        /// Valid for <see cref="ExtensionType.Component"/> extensions only.
        /// The Y-coordinate of the extension.
        /// </summary>
        [JsonProperty("y")]
        public int y { get; set; }
    }
}
