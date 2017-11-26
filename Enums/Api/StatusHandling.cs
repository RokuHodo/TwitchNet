namespace
TwitchNet.Enums.Api
{
    public enum
    StatusHandling
    {
        /// <summary>
        /// Throw an <see cref="Exception"/> when an API error is returned.
        /// </summary>
        Error   = 0,

        /// <summary>
        /// Ignore the API error and return any previously obtained data in the case of a multi-page request.
        /// </summary>
        Ignore  = 1,

        /// <summary>
        /// Execute the request until a limit is reached (if any) or the request succeeds.
        /// </summary>
        Retry   = 2,
    }
}