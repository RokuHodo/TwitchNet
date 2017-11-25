// standard namespaces
using System;

// project namespaces
using TwitchNet.Extensions;

namespace TwitchNet.Utilities
{
    internal static class
    ExceptionUtil
    {
        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the string is null, empty, or whitespace
        /// </summary>
        /// <param name="parameter">The parameter to check.</param>
        /// <param name="parameter_name">The name of the parameter.</param>
        public static void
        ThrowIfInvalid(string parameter, string parameter_name)
        {
            ThrowIfInvalid(parameter, parameter_name, parameter_name + " cannot be null, empty, or whitspace.");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the string is null, empty, or whitespace
        /// </summary>
        /// <param name="parameter">The parameter to check.</param>
        /// <param name="parameter_name">The name of the parameter.</param>
        /// <param name="message">The excpetion message.</param>
        public static void
        ThrowIfInvalid(string parameter, string parameter_name, string message)
        {
            if (parameter.IsValid())
            {
                return;
            }

            throw new ArgumentException(message, parameter_name);
        }
    }
}
