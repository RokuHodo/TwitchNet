using System;

namespace
TwitchNet.Rest
{
    public enum
    StatusHandling
    {        
        /// <summary>
        /// Throws an <see cref="AggregateException"/> with all errors that were encountered.
        /// </summary>
        Error   = 0,

        /// <summary>
        /// Returns the <see cref="AggregateException"/> with all errors that were encountered.
        /// </summary>
        Return  = 1,

        /// <summary>
        /// <para>Executes the request again.</para>
        /// <para>
        /// Any errors that were encountered are still added to the <see cref="AggregateException"/> but will not be thrown.
        /// If the retry limit is reached, the <see cref="AggregateException"/> will be thrown or returned depending on what <see cref="StatusCodeSetting.retry_limit_reached_handling"/> is set to.
        /// </para>
        /// </summary>
        Retry = 2,
    }
}