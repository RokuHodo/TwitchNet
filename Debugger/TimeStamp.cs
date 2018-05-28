namespace
TwitchNet.Debugger
{
    internal enum
    TimeStamp
    {
        /// <summary>
        /// Print no time stamp.
        /// </summary>
        None        = 0,

        /// <summary>
        /// Prints the date and time in normal format - 9/30/2017 4:14:29 PM
        /// </summary>
        Default     = 1,

        /// <summary>
        /// Prints the date in short format - 9/30/2017
        /// </summary>
        DateShort   = 2,

        /// <summary>
        /// Prints the date in long format - Saturday, September 30, 2017
        /// </summary>
        DateLong    = 3,

        /// <summary>
        /// Prints the time in short format - 4:14 PM
        /// </summary>
        TimeShort   = 4,

        /// <summary>
        /// Prints the time in long format - 4:14:29 PM
        /// </summary>
        TimeLong    = 5   
    }
}
