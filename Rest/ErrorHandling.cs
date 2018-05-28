namespace
TwitchNet.Rest
{
    public enum
    ErrorHandling
    {
        /// <summary>
        /// Throw an <see cref="Exception"/> where one is encountered or where one id manually thrown.
        /// </summary>
        Error   = 0,

        /// <summary>
        /// Don't throw the the <see cref="Exception"/> but cancel the request with the error attached.
        /// </summary>
        Return  = 1,
    }
}