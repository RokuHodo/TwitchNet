// standard namespaces
using System;

namespace
TwitchNet.Debugger
{
    [Flags]
    internal enum
    DebugLevel
    {
        /// <summary>
        /// Print no debug information. Overrides all other bitfields if set.
        /// </summary>
        None        = 0,

        /// <summary>
        /// Normal messages.
        /// </summary>
        General    = 1 << 0,

        /// <summary>
        /// Informative messages.
        /// </summary>
        Info        = 1 << 1,

        /// <summary>
        /// Error messages.
        /// </summary>
        Error       = 1 << 2,

        /// <summary>
        /// Warning messages. 
        /// </summary>
        Warning     = 1 << 3,

        /// <summary>
        /// Print all debug information. Overrides all other bitfields if set, except 'None'.
        /// </summary>
        All         = General | Error | Warning
    }
}
