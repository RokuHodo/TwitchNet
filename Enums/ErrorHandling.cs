namespace
TwitchNet.Enums
{
    public enum
    ErrorHandling
    {
        /// <summary>
        /// Throw an <see cref="Exception"/> where one is encountered.
        /// </summary>
        Error   = 0,

        /// <summary>
        /// Ignore the <see cref="Exception"/> and attempt to continue.
        /// </summary>
        Ignore  = 1,
    }
}