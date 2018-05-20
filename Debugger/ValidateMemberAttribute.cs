// standrad namespaces
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// project namespaces
using TwitchNet.Enums.Debugger;

namespace
TwitchNet.Debugger
{
    [Conditional("DEBUG")]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
    internal class
    ValidateMemberAttribute : Attribute
    {
        public string caller;

        public string source;

        public int line;

        public ErrorLevel level;

        public Check check;

        public object compare_to;

        public ValidateMemberAttribute(Check check, object compare_to = null, [CallerMemberName] string caller = "", [CallerFilePath] string source = "", [CallerLineNumber] int line = -1)
        {
            this.check = check;

            this.compare_to = compare_to;

            this.caller = caller;
            this.line = line;
            this.source = source;
        }

        public ValidateMemberAttribute(ErrorLevel level, Check check, object value = null, [CallerMemberName] string caller = "", [CallerFilePath] string source = "", [CallerLineNumber] int line = 0) : this(check, value, caller, source, line)
        {
            this.level = level;
        }
    }
}
