﻿// standard namespaces
using System;
using System.Collections.Generic;

// project namespaces
using TwitchNet.Extensions;

namespace TwitchNet.Utilities
{
    internal static class
    ExceptionUtil
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the object is null.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <param name="obj_name">The name of the object</param>
        /// <exception cref="ArgumentNullException">Thrown if the object is null.</exception>
        public static void
        ThrowIfNull(object obj, string obj_name, Action callback  = null)
        {
            ThrowIfNull(obj, obj_name, obj_name + " cannot be null.", callback);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the object is null.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <param name="obj_name">The name of the object</param>
        /// <param name="message">The excpetion message.</param>
        public static void
        ThrowIfNull(object obj, string obj_name, string message, Action callback = null)
        {
            if (!obj.IsNull())
            {
                return;
            }

            if (!callback.IsNull())
            {
                callback();
            }

            throw new ArgumentNullException(obj_name, message);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the object is null or equal to its default value.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <param name="obj_name">The name of the object</param>
        /// <exception cref="ArgumentException">Thrown if the object is null or equal to its default value.</exception>
        public static void
        ThrowIfNullOrDefault(object obj, string obj_name, Action callback = null)
        {
            ThrowIfNullOrDefault(obj, obj_name, obj_name + " cannot be null or equal to its default value.", callback);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the object is null or equal to its default value.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <param name="obj_name">The name of the object</param>
        /// <param name="message">The excpetion message.</param>
        /// <exception cref="ArgumentException">Thrown if the object is null or equal to its default value.</exception>
        public static void
        ThrowIfNullOrDefault(object obj, string obj_name, string message, Action callback = null)
        {
            if (!obj.IsNullOrDefault())
            {
                return;
            }

            if (!callback.IsNull())
            {
                callback();
            }

            throw new ArgumentException(message, obj_name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the string is null, empty, or whitespace.
        /// </summary>
        /// <param name="parameter">The parameter to check.</param>
        /// <param name="parameter_name">The name of the parameter.</param>
        /// <exception cref="ArgumentException">Thrown if the string is null, empty, or whitespace.</exception>
        public static void
        ThrowIfInvalid(string parameter, string parameter_name, Action callback = null)
        {
            ThrowIfInvalid(parameter, parameter_name, parameter_name + " cannot be null, empty, or whitespace.", callback);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the string is null, empty, or whitespace
        /// </summary>
        /// <param name="parameter">The parameter to check.</param>
        /// <param name="parameter_name">The name of the parameter.</param>
        /// <param name="message">The excpetion message.</param>
        public static void
        ThrowIfInvalid(string parameter, string parameter_name, string message, Action callback = null)
        {
            if (parameter.IsValid())
            {
                return;
            }

            if (!callback.IsNull())
            {
                callback();
            }

            throw new ArgumentException(message, parameter_name);
        }

        public static void
        ThrowIfInvalid<type>(IList<type> parameter, string parameter_name, Action callback = null)
        {
            ThrowIfInvalid(parameter, parameter_name, parameter_name + " cannot be null, empty, or whitespace.", callback);
        }

        public static void
        ThrowIfInvalid<type>(IList<type> parameter, string parameter_name, string message, Action callback = null)
        {
            if (parameter.IsValid())
            {
                return;
            }

            if (!callback.IsNull())
            {
                callback();
            }

            throw new ArgumentException(message, parameter_name);
        }
    }
}
