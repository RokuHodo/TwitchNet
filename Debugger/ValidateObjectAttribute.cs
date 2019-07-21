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
        /// <summary>
        /// Creates a new instance of the <see cref="ValidateObjectAttribute"/> class.
        /// </summary>
        public ValidateObjectAttribute()
        {

        }
    }
}
