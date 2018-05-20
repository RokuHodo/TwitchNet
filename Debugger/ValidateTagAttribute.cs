// standard namespaces
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using TwitchNet.Enums.Debugger;

namespace
TwitchNet.Debugger
{
    [Conditional("DEBUG")]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    internal class
    ValidateTagAttribute : ValidateMemberAttribute
    {

        public string tag;

        public ValidateTagAttribute(string tag, Check check = Check.None, object value = null, [CallerMemberName] string caller = "", [CallerFilePath] string source = "", [CallerLineNumber] int line = -1) : base(check, value, caller, source, line)
        {
            this.tag = tag;
        }
    }
}
