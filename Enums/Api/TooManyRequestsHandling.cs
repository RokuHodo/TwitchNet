namespace
TwitchNet.Enums.Api
{
    public enum
    TooManyRequestHandling
    {

        /// <summary>
        /// Wait for the rate limit to reset and continue the executing the multi-page request.
        /// </summary>
        Wait    = 0,

        /// <summary>
        /// Ignore the error and return any previously obtained data in the case of a multi-page request.
        /// </summary>
        Ignore  = 1,

        /// <summary>
        /// Throw an <see cref="Exception"/> with the status code and status description.
        /// </summary>
        Error   = 2
    }
}