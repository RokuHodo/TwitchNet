// standard namespaces
using System;
using System.Collections.Generic;

// project namespaces
using TwitchNet.Extensions;

namespace
TwitchNet.Utilities
{   
    internal static class
    ExceptionUtil
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the object is null.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <param name="obj_name">The name of the object</param>
        /// <param name="callback">The action to perform if an exception would be thrown, before the excpetion is actually thrown.</param>
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
        /// <param name="callback">The action to perform if an exception would be thrown, before the excpetion is actually thrown.</param>
        /// <exception cref="ArgumentNullException">Thrown if the object is null.</exception>
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
        /// <param name="callback">The action to perform if an exception would be thrown, before the excpetion is actually thrown.</param>
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
        /// <param name="callback">The action to perform if an exception would be thrown, before the excpetion is actually thrown.</param>
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
        /// <param name="callback">The action to perform if an exception would be thrown, before the excpetion is actually thrown.</param>
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
        /// <param name="callback">The action to perform if an exception would be thrown, before the excpetion is actually thrown.</param>
        /// <exception cref="ArgumentException">Thrown if the string is null, empty, or whitespace.</exception>
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

        /// <summary>
        /// Throws if an <see cref="ArgumentException"/> if an object that implements <see cref="IList{T}"/> is invalid.
        /// </summary>
        /// <typeparam name="type">The type of the <see cref="IList{T}"/></typeparam>
        /// <param name="parameter">The parameter to check.</param>
        /// <param name="parameter_name">The name of the parameter.</param>
        /// <param name="callback">The action to perform if an exception would be thrown, before the excpetion is actually thrown.</param>
        /// <exception cref="ArgumentException">Thrown if the <see cref="IList{T}"/> is null or empty.</exception>
        public static void
        ThrowIfInvalid<type>(IList<type> parameter, string parameter_name, Action callback = null)
        {
            ThrowIfInvalid(parameter, parameter_name, parameter_name + " cannot be null, empty, or whitespace.", callback);
        }

        /// <summary>
        /// Throws if an <see cref="ArgumentException"/> if an object that implements <see cref="IList{T}"/> is invalid.
        /// </summary>
        /// <typeparam name="type">The type of the <see cref="IList{T}"/></typeparam>
        /// <param name="parameter">The parameter to check.</param>
        /// <param name="parameter_name">The name of the parameter.</param>
        /// <param name="message">The excpetion message.</param>
        /// <param name="callback">The action to perform if an exception would be thrown, before the excpetion is actually thrown.</param>
        /// <exception cref="ArgumentException">Thrown if the <see cref="IList{T}"/> is null or empty.</exception>
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

        /// <summary>
        /// Throws if an <see cref="ArgumentException"/> if an object that implements <see cref="IDictionary{TKey, TValue}"/> is null or does not have at least one key value pair.
        /// </summary>
        /// <typeparam name="key_type">The type of the <see cref="IDictionary"/> key.</typeparam>
        /// <typeparam name="key_value">The type of the <see cref="IDictionary"/> value.</typeparam>
        /// <param name="parameter">The parameter to check.</param>
        /// <param name="parameter_name">The name of the parameter.</param>
        /// <param name="callback">The action to perform if an exception would be thrown, before the excpetion is actually thrown.</param>
        /// <exception cref="ArgumentException">Thrown if an object that implements <see cref="IDictionary{TKey, TValue}"/> is null or does not have at least one key value pair.</exception>
        public static void
        ThrowIfInvalid<key_type, key_value>(IDictionary<key_type, key_value> parameter, string parameter_name, Action callback = null)
        {
            ThrowIfInvalid(parameter, parameter_name, parameter_name + " cannot be null and must havce at least one key value pair.", callback);
        }

        /// <summary>
        /// Throws if an <see cref="ArgumentException"/> if an object that implements <see cref="IDictionary{TKey, TValue}"/> is null or does not have at least one key value pair.
        /// </summary>
        /// <typeparam name="key_type">The type of the <see cref="IDictionary"/> key.</typeparam>
        /// <typeparam name="key_value">The type of the <see cref="IDictionary"/> value.</typeparam>
        /// <param name="parameter">The parameter to check.</param>
        /// <param name="parameter_name">The name of the parameter.</param>
        /// <param name="message">The excpetion message.</param>
        /// <param name="callback">The action to perform if an exception would be thrown, before the excpetion is actually thrown.</param>
        /// <exception cref="ArgumentException">Thrown if an object that implements <see cref="IDictionary{TKey, TValue}"/> is null or does not have at least one key value pair.</exception>
        public static void
        ThrowIfInvalid<key_type, key_value>(IDictionary<key_type, key_value> parameter, string parameter_name, string message, Action callback = null)
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

        /// <summary>
        /// Throws if an <see cref="ArgumentOutOfRangeException"/> if an object that implements <see cref="IComparable{T}"/> is outside of the valid range.
        /// </summary>
        /// <typeparam name="type">The type of the <see cref="IComparable{T}"/>.</typeparam>
        /// <param name="parameter_name">The name of the parameter.</param>
        /// <param name="parameter">The parameter to check.</param>
        /// <param name="minimum">The lowest possible value.</param>
        /// <param name="maximum">The largets possible value.</param>
        /// <param name="callback">The action to perform if an exception would be thrown, before the excpetion is actually thrown.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if if an object that implements <see cref="IComparable{T}"/> is outside of the valid range..</exception>
        public static void
        ThrowIfOutOfRange<type>(string parameter_name, type parameter, type minimum, type maximum, Action callback = null)
        where type : IComparable<type>
        {
            ThrowIfOutOfRange(parameter_name, parameter, minimum, maximum, parameter_name + " is out of range. " + parameter_name + " must be between " + minimum + " and " + maximum);
        }

        /// <summary>
        /// Throws if an <see cref="ArgumentOutOfRangeException"/> if an object that implements <see cref="IComparable{T}"/> is outside of the valid range.
        /// </summary>
        /// <typeparam name="type">The type of the <see cref="IComparable{T}"/>.</typeparam>
        /// <param name="parameter_name">The name of the parameter.</param>
        /// <param name="parameter">The parameter to check.</param>
        /// <param name="minimum">The lowest possible value.</param>
        /// <param name="maximum">The largets possible value.</param>
        /// <param name="message">The excpetion message.</param>
        /// <param name="callback">The action to perform if an exception would be thrown, before the excpetion is actually thrown.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if if an object that implements <see cref="IComparable{T}"/> is outside of the valid range..</exception>
        public static void
        ThrowIfOutOfRange<type>(string parameter_name, type parameter, type minimum, type maximum, string message, Action callback = null)
        where type : IComparable<type>
        {
            if (parameter.IsInRange(minimum, maximum))
            {
                return;
            }

            if (!callback.IsNull())
            {
                callback();
            }

            throw new ArgumentOutOfRangeException(parameter_name, parameter, message);
        }

        /// <summary>
        /// Throws if an <see cref="FormatException"/> if a string does not meet Twitch's user name format requirements.
        /// </summary>
        /// <param name="parameter">The parameter to check.</param>
        /// <param name="parameter_name">The name of the parameter.</param>
        /// <param name="callback">The action to perform if an exception would be thrown, before the excpetion is actually thrown.</param>
        /// <exception cref="FormatException">Thrown if the string is not between 2 and 24 characters long, and does not only contian alpha-numeric characters.</exception>
        public static void
        ThrowIfInvalidNick(string parameter, Action callback = null)
        {
            ThrowIfInvalidNick(parameter, "Invalid IRC nick: " + parameter + ". The nick can only contain lower case alpha-numeric characters and must be between 2 and 24 characters long.", callback);
        }

        /// <summary>
        /// Throws if an <see cref="FormatException"/> if a string does not meet Twitch's user name format requirements.
        /// </summary>
        /// <param name="parameter">The parameter to check.</param>
        /// <param name="parameter_name">The name of the parameter.</param>
        /// <param name="message">The excpetion message.</param>
        /// <param name="callback">The action to perform if an exception would be thrown, before the excpetion is actually thrown.</param>
        /// <exception cref="FormatException">Thrown if the string is not between 2 and 24 characters long, and does not only contian alpha-numeric characters.</exception>
        public static void
        ThrowIfInvalidNick(string parameter, string message, Action callback = null)
        {
            if (parameter.IsValid())
            {
                return;
            }

            if (TwitchUtil.IsValidNick(parameter))
            {
                return;
            }

            if (!callback.IsNull())
            {
                callback();
            }

            throw new FormatException(message);
        }

        /// <summary>
        /// Throws if an <see cref="FormatException"/> if a string does not meet Twitch's /followers duration format requirements.
        /// </summary>
        /// <param name="parameter">The parameter to check.</param>
        /// <param name="parameter_name">The name of the parameter.</param>
        /// <param name="callback">The action to perform if an exception would be thrown, before the excpetion is actually thrown.</param>
        /// <exception cref="FormatException">Thrown if the string does not meet Twitch's /followers duration format requirements.</exception>
        public static void
        ThrowIfInvalidFollowersDuration(string parameter, Action callback = null)
        {
            ThrowIfInvalidFollowersDuration(parameter,  "Invalid /followers duration format: " + parameter + ".", callback);
        }

        /// <summary>
        /// Throws if an <see cref="FormatException"/> if a string does not meet Twitch's /followers duration format requirements.
        /// </summary>
        /// <param name="parameter">The parameter to check.</param>
        /// <param name="parameter_name">The name of the parameter.</param>
        /// <param name="message">The excpetion message.</param>
        /// <param name="callback">The action to perform if an exception would be thrown, before the excpetion is actually thrown.</param>
        /// <exception cref="FormatException">Thrown if the string does not meet Twitch's /followers duration format requirements.</exception>
        public static void
        ThrowIfInvalidFollowersDuration(string parameter, string message, Action callback = null)
        {
            if (parameter.IsValid())
            {
                return;
            }

            if (TwitchUtil.IsValidFollowersDurationFormat(parameter))
            {
                return;
            }

            if (!callback.IsNull())
            {
                callback();
            }

            throw new FormatException(message);
        }
    }
}
