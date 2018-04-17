namespace
TwitchNet.Enums.Api
{
    public enum
    StatusHandling
    {
        /// <summary>
        /// Throw an intenral <see cref="Exception"/> when an API error is returned.
        /// </summary>
        Error   = 0,

        /// <summary>
        /// Don't throw the the <see cref="Exception"/> but cancel the request with the error attached.
        /// </summary>
        Return = 1,

        /// <summary>
        /// Execute the request until a limit is reached (if any) or the request succeeds.
        /// If the retry limit is exceeded, an error is thrown.
        /// </summary>
        Retry   = 2,
    }
}