// standard namespaces
using System;

namespace
TwitchNet.Enums.Debug
{
    [Flags]
    internal enum
    LogLevel
    {
        /// <summary>
        /// Print no debug information. Overrides all other bitfields if set.
        /// </summary>
        None        = 0,

        /// <summary>
        /// Print error messages.
        /// </summary>
        Errors      = 1 << 0,

        /// <summary>
        /// Print warning messages. 
        /// </summary>
        Warnings    = 1 << 1,

        /// <summary>
        /// Print success messages.
        /// </summary>
        Success     = 1 << 2,

        /// <summary>
        /// Prints header messages. Signifies and separates debug blocks.
        /// </summary>
        Headers     = 1 << 3,

        /// <summary>
        /// Print normal messages. Gives detailed non-critical information on what is happening at any given moment.
        /// </summary>
        Detailed    = 1 << 4,

        /// <summary>
        /// Print all debug information. Overrides all other bitfields if set, except 'None'.
        /// </summary>
        All         = Errors | Warnings | Success | Headers | Detailed
    }
}
