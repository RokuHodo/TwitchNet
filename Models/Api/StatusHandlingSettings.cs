// project namespaces
using TwitchNet.Enums.Api;
using TwitchNet.Helpers;

namespace TwitchNet.Models.Api
{
    internal class
    StatusHandlingSettings
    {
        /// <summary>
        /// How many times the request has been retried.
        /// </summary>
        public ushort                  retry_count { get; set; }

        /// <summary>
        /// The maximum amount of times to retry.
        /// </summary>
        public ClampedNumber<short>    retry_limit { get; set; }

        /// <summary>
        /// How to handle the response status code.
        /// </summary>
        public StatusHandling          handling    { get; set; }

        public StatusHandlingSettings(ClampedNumber<short> retry_limit, StatusHandling handling)
        {
            retry_count         = 0;
            this.retry_limit    = retry_limit;
            this.handling       = handling;
        }
    }
}
