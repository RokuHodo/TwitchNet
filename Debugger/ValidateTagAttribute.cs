﻿// standard namespaces
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
        /// <summary>
        /// The name of the tag to validate.
        /// </summary>
        public string tag { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="ValidateTagAttribute"/> class.
        /// </summary>
        /// <param name="tag">The name of the tag to validate.</param>
        /// <param name="check">How to validate the field or property.</param>
        /// <param name="compare_to">The value to compare the field or property against when the check is set to <see cref="Check.IsEqualTo"/> or <see cref="Check.IsNotEqualTo"/>.</param>
        /// <param name="caller">The name of the field or property.</param>
        /// <param name="source">The source file of the caller.</param>
        /// <param name="line">The line of the caller.</param>
        public ValidateTagAttribute(string tag, Check check = Check.None, object compare_to = null, [CallerMemberName] string caller = "", [CallerFilePath] string source = "", [CallerLineNumber] int line = -1) : base(check, compare_to, caller, source, line)
        {
            this.tag = tag;
        }
    }
}
