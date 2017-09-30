// standard namespaces
using System;

namespace TwitchNet.Enums.Debug
{
    [Flags]
    internal enum LogLevel
    {
        /// <summary>
        /// Print no debug information. Overrides all other bitfields if set.
        /// </summary>
        None = 0,

        /// <summary>
        /// Print error messages.
        /// </summary>
        Errors      = 1,

        /// <summary>
        /// Print warning messages. 
        /// </summary>
        Warnings    = 2,

        /// <summary>
        /// Print success messages.
        /// </summary>
        Success     = 4,

        /// <summary>
        /// Prints header messages. Signifies and separates debug blocks.
        /// </summary>
        Headers     = 8,

        /// <summary>
        /// Print normal messages. Gives detailed non-critical information on what is happening at any given moment.
        /// </summary>
        Detailed    = 16,

        /// <summary>
        /// Print all debug information. Overrides all other bitfields if set, except 'None'.
        /// </summary>
        All         = 32
    }
}
