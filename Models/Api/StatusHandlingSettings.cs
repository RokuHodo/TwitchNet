// project namespaces
using TwitchNet.Enums.Api;
using TwitchNet.Helpers;

namespace TwitchNet.Models.Api
{
    internal class
    StatusHandlingSettings
    {
        public ushort                  retry_count { get; set; }
        public ClampedNumber<short>    retry_limit { get; set; }
        public StatusHandling          handling    { get; set; }

        public StatusHandlingSettings(ClampedNumber<short> retry_limit, StatusHandling handling)
        {
            retry_count         = 0;
            this.retry_limit    = retry_limit;
            this.handling       = handling;
        }
    }
}
