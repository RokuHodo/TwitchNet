// standrad namespaces
using System;
using System.Diagnostics;

namespace
TwitchNet.Debugger
{
    [Conditional("DEBUG")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    internal class
    ValidateObjectAttribute : Attribute
    {
        public ValidateObjectAttribute()
        {

        }
    }
}
