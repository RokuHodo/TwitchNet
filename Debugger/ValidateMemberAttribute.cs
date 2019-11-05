// standrad namespaces
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace
TwitchNet.Debugger
{
    [Conditional("DEBUG")]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
    internal class
    ValidateMemberAttribute : Attribute
    {
        /// <summary>
        /// The severity of the warning or error if the check validation fails.
        /// </summary>
        public ErrorLevel level         { get; protected set; }

        /// <summary>
        /// How to validate the field or property.
        /// </summary>
        public Check        check       { get; protected set; }

        /// <summary>
        /// The value to compare the field or property against when the check is set to <see cref="Check.IsEqualTo"/> or <see cref="Check.IsNotEqualTo"/>.
        /// </summary>
        public object       compare_to  { get; protected set; }

        /// <summary>
        /// The name of the field or property.
        /// </summary>
        public string       caller      { get; protected set; }

        /// <summary>
        /// The source file of the caller.
        /// </summary>
        public string       source      { get; protected set; }

        /// <summary>
        /// The line of the caller.
        /// </summary>
        public int          line        { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="ValidateMemberAttribute"/> class.
        /// </summary>
        /// <param name="check">How to validate the field or property.</param>
        /// <param name="compare_to">The value to compare the field or property against when the check is set to <see cref="Check.IsEqualTo"/> or <see cref="Check.IsNotEqualTo"/>.</param>
        /// <param name="caller">The name of the field or property.</param>
        /// <param name="source">The source file of the caller.</param>
        /// <param name="line">The line of the caller.</param>
        public ValidateMemberAttribute(Check check = Check.None, object compare_to = null, [CallerMemberName] string caller = "", [CallerFilePath] string source = "", [CallerLineNumber] int line = -1)
        {
            this.check = check;

            this.compare_to = compare_to;

            this.caller = caller;
            this.line = line;
            this.source = source;

            level = ErrorLevel.Critical | ErrorLevel.Major;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ValidateMemberAttribute"/> class.
        /// </summary>
        /// <param name="level">The severity of the warning or error if the check validation fails.</param>
        /// <param name="check">How to validate the field or property.</param>
        /// <param name="compare_to">The value to compare the field or property against when the check is set to <see cref="Check.IsEqualTo"/> or <see cref="Check.IsNotEqualTo"/>.</param>
        /// <param name="caller">The name of the field or property.</param>
        /// <param name="source">The source file of the caller.</param>
        /// <param name="line">The line of the caller.</param>
        public ValidateMemberAttribute(ErrorLevel level, Check check, object value = null, [CallerMemberName] string caller = "", [CallerFilePath] string source = "", [CallerLineNumber] int line = 0) : this(check, value, caller, source, line)
        {
            this.level = level;
        }
    }
}
