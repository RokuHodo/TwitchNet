namespace TwitchNet.Enums.Api
{
    public enum
    InternalServerErrorHandling
    {
        /// <summary>
        /// Ignore the error and return any previously obtained data in the case of a multi-page request.
        /// </summary>
        Ignore  = 0,

        /// <summary>
        /// Execute the request one more time.
        /// If 'Internal Server Error' is returned again, it will be handled under the conditions of <see cref="Ignore"/>.
        /// </summary>
        Retry   = 1,

        /// <summary>
        /// Throw an <see cref="Exception"/> with the status code and status description.
        /// </summary>
        Error   = 2
    }
}