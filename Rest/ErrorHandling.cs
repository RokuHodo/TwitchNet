namespace
TwitchNet.Rest
{
    public enum
    ErrorHandling
    {
        /// <summary>
        /// Throws an <see cref="AggregateException"/> with all errors that were encountered.
        /// </summary>
        Error = 0,

        /// <summary>
        /// Returns the <see cref="AggregateException"/> with all errors that were encountered.
        /// </summary>
        Return = 1,
    }
}