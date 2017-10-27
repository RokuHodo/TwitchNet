// project namespaces
using TwitchNet.Enums.Utilities;

namespace TwitchNet.Models.Utilities
{
    public class TwitchRequestSettings
    {
        /// <summary>
        /// Determine how to handle the status code '429' - Too Many Requests
        /// </summary>
        public TooManyRequestHandling too_many_request_handling { get; set; }
    }
}
