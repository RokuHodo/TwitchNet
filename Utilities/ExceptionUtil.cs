// standard namespaces
using System;

// project namespaces
using TwitchNet.Extensions;

namespace TwitchNet.Utilities
{
    internal static class
    ExceptionUtil
    {
        public static void
        ThrowIf(bool condition, string message)
        {
            if (!condition)
            {
                return;
            }

            throw new Exception(message);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the object is null.
        /// </summary>
        /// <param name="parameter_name">The name of the parameter.</param>
        /// <param name="parameter">The parameter to check.</param>
        public static void
        ThrowIfNull(object parameter, string parameter_name)
        {
            if (!parameter.IsNull())
            {
                return;
            }

            throw new ArgumentNullException(parameter_name, parameter_name + " cannot be null.");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the parameter is equal to its default value.
        /// </summary>
        /// <typeparam name="type">The parameter's type.</typeparam>
        /// <param name="parameter_name">The name of the parameter.</param>
        /// <param name="parameter">The parameter to check.</param>
        public static void
        ThrowIfDefault<type>(type parameter, string parameter_name)
        {
            if (!parameter.IsDefault())
            {
                return;
            }

            throw new ArgumentException(parameter_name + " cannot be the default value, " + default(type), parameter_name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the parameter is null or equal to its default value.
        /// </summary>
        /// <typeparam name="type">The parameter's type.</typeparam>
        /// <param name="parameter_name">The name of the parameter.</param>
        /// <param name="parameter">The parameter to check.</param>
        public static void
        ThrowIfNullOrDefault<type>(type parameter, string parameter_name)
        {
            if (parameter.IsNullOrDefault())
            {
                return;
            }

            throw new ArgumentException(parameter_name + " cannot be null or the default value, " + default(type), parameter_name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the string is null, empty, or whitespace
        /// </summary>
        /// <param name="parameter_name">The name of the parameter.</param>
        /// <param name="parameter">The parameter to check.</param>
        public static void
        ThrowIfInvalid(string parameter, string parameter_name)
        {
            if(parameter.IsValid())
            {
                return;
            }

            throw new ArgumentException(parameter_name + " cannot be null, empty, or whitspace.", parameter_name);
        }        
    }
}
