namespace
TwitchNet.Rest
{
    public enum
    InputHandling
    {
        /// <summary>
        /// Check and verify parameters input by the user.
        /// An <see cref="Exception"/> is thrown when the parameters are invalid.
        /// </summary>
        Error = 0,

        /// <summary>
        /// Don't check and verify parameters input by the user.
        /// </summary>
        Ignore  = 1,
    }
}