// standard namespaces
using System;

namespace
TwitchNet.Enums.Debugger
{
    [Flags]
    internal enum
    ErrorLevel
    {
        None = 0,

        Minor = 1 << 0,

        Major = 1 << 1,

        Critical = 1 << 2,

        All = Minor | Major | Critical
    }
}
