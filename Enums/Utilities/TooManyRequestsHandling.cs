using System;

namespace TwitchNet.Enums.Utilities
{
    public enum
    TooManyRequestHandling
    {

        /// <summary>
        /// Wait for the rate limit to reset and continue the executing the multi-page request.
        /// </summary>
        Wait  = 0,

        /// <summary>
        /// Ignore the error and returns all returned results.
        /// </summary>
        Ignore          = 1,

        /// <summary>
        /// Throw an <see cref="Exception"/> when the status code '429' - Too Many Requests is returned by Twitch.
        /// </summary>
        Error            = 2
    }
}