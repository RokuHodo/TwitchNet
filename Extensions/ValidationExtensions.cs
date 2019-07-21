// project namespaces
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

// project namespaces
using TwitchNet.Utilities;

namespace
TwitchNet.Extensions
{
    internal static class
    ValidationExtensions
    {
        #region Value checks

        /// <summary>
        /// Verifies that an <see cref="object"/> is null.
        /// </summary>  
        /// <param name="obj">The object to be checked.</param>
        /// <returns>
        /// Returns true if the <see cref="object"/> is null.
        /// Returns false otherwise.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool
        IsNull(this object obj)
        {
            return obj == null;
        }

        /// <summary>
        /// Checks to see if a value is null.
        /// </summary>
        /// <typeparam name="type">The type of the value.</typeparam>
        /// <param name="value">The vlaue to chekc.</param>
        /// <returns>
        /// Returns true if the value is null.
        /// Returns false otherwise.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool
        IsNull<type>(this type value)
        {
            return value == null;
        }

        /// <summary>
        /// Verifies that an <see cref="object"/> of an implided <see cref="Type"/> is equal to its default value.
        /// </summary>
        /// <typeparam name="type">The implied type of the object.</typeparam>
        /// <param name="value">The value of the <see cref="object"/>.</param>
        /// <returns>
        /// Returns true if the value of the <see cref="object"/> is equal to the type's default value.
        /// Returns false otherwise.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool
        IsDefault<type>(this type value)
        {
            bool result = EqualityComparer<type>.Default.Equals(value, default);

            return result;
        }

        /// <summary>
        /// Verifies that an <see cref="object"/> of an implided <see cref="Type"/> is null or equal to its default value.
        /// </summary>
        /// <typeparam name="type">The implied type of the object.</typeparam>
        /// <param name="value">The value of the <see cref="object"/>.</param>
        /// <returns>
        /// Returns true if the value of the <see cref="object"/> is null or equal to the type's default value.
        /// Returns false otherwise.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool
        IsNullOrDefault<type>(this type value)
        {
            bool result = value.IsNull() || value.IsDefault();

            return result;
        }

        /// <summary>
        /// Verifies that an <see cref="object"/> that adheres to the <see cref="IList{T}"/> interface is not null and has at least one element.
        /// </summary>
        /// <typeparam name="type"></typeparam>
        /// <param name="list"></param>
        /// <returns>
        /// Returns true if the <see cref="IList{T}"/> is not null and has at least one element.
        /// Returns false otherwise.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool
        IsValid<type>(this IList<type> list)
        {
            bool result = !list.IsNull() && list.Count > 0;

            return result;
        }        

        /// <summary>
        /// Verifies that a <see cref="string"/> is not null, empty, or contains only whitespace.
        /// </summary>
        /// <param name="str">The <see cref="string"/> to be validated.</param>
        /// <returns>
        /// Returns true if the string is not not, not whitespace, and not empty.
        /// Returns false otherwise.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool
        IsValid(this string str)
        {
            bool result = !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool
        IsEmpty(this string str)
        {
            return str.Length == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool
        IsWhiteSpace(this string str)
        {
            for (int index = 0; index < str.Length; index++)
            {
                if (!char.IsWhiteSpace(str[index]))
                {
                    return false;
                }
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool
        IsEmptyOrWhiteSpace(this string str)
        {
            return str.IsEmpty() || str.IsWhiteSpace();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool
        HasContent(this string str)
        {
            return !str.IsNull() && !str.IsEmpty() && !str.IsWhiteSpace();
        }

        /// <summary>
        /// Verifies that an object that implements <see cref="IDictionary{TKey, TValue}"/> is not null and has at least one <see cref="KeyValuePair{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="key_type">The dictionary key type.</typeparam>
        /// <typeparam name="value_type">The dictionary value type.</typeparam>
        /// <param name="dictionary">The dictionary to verify.</param>
        /// <returns>
        /// Returns true if the dictionary is not null and has at least one <see cref="KeyValuePair{TKey, TValue}"/>.
        /// Returns false otherwise.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool
        IsValid<key_type, value_type>(this IDictionary<key_type, value_type> dictionary)
        {
            bool result = !dictionary.IsNull() && dictionary.Keys.Count > 0;

            return result;
        }

        #endregion

        #region Type checks

        /// <summary>
        /// Checks to see if an <see cref="object"/> can be convereted to certain type.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to be checked.</param>
        /// <param name="type">The type </param>
        /// <returns>
        /// Returns true if the <see cref="object"/> can be converted to the specified type.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if the obj is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool
        CanCovertTo(this object obj, Type type)
        {
            ExceptionUtil.ThrowIfNull(obj, nameof(obj));

            bool result = TypeDescriptor.GetConverter(type).IsValid(obj);

            return result;
        }

        #endregion

        #region Regex checks

        /// <summary>
        /// Check to see if the string matches the HTML (hex) color format #FFFFFF.
        /// </summary>
        /// <param name="html_color">The color to check.</param>
        /// <returns>
        /// Returns true if the string matches the HTML (hex) color format.
        /// Returns false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if the html color string is null.</exception>
        public static bool
        IsValidHtmlColor(this string html_color)
        {
            Regex regex = new Regex("^#([A-Fa-f0-9]{6})$");

            bool result = html_color.IsValid() && regex.IsMatch(html_color);

            return result;
        }

        #endregion

        #region Search checks

        /// <summary>
        /// Checks to see if an <see cref="IList{T}"/> contains a specified value.
        /// </summary>
        /// <typeparam name="type">The generic type of the <see cref="IList{T}"/>.</typeparam>
        /// <param name="list">The list to search in.</param>
        /// <param name="value">The value to search for.</param>
        /// <returns>
        /// Returns true of the list contains the value.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        Contains<type>(this IList<type> list, type value)
        {
            bool result = list.Any(temp => EqualityComparer<type>.Default.Equals(temp, value));

            return result;
        }

        #endregion
    }
}