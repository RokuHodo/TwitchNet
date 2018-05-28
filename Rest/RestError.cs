// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest
{
    public class
    RestError
    {
        /// <summary>
        /// The error associated with the status code, i.e., the status description.
        /// </summary>
        [JsonProperty("error")]
        public string error     { get; protected set; }

        /// <summary>
        /// The HTTP status code of the returned response.
        /// </summary>
        [JsonProperty("status")]
        public ushort status    { get; protected set; }

        /// <summary>
        /// The descriptive error message that gives more detailed information on the type of error.
        /// </summary>
        [JsonProperty("message")]
        public string message   { get; protected set; }
    }
}
