// project namespaces
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TwitchNet.Extensions
{
    internal static class
    ValidationExtensions
    {
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
            bool result = obj == null;

            return result;
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
            bool result = EqualityComparer<type>.Default.Equals(value, default(type));

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

        /// <summary>
        /// Checks to see if an <see cref="object"/> can be convereted to certain type.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to be checked.</param>
        /// <param name="type">The type </param>
        /// <returns>
        /// Returns true if the <see cref="object"/> can be converted to the specified type.
        /// Returns false otherwise.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool
        CanCovertTo(this object obj, Type type)
        {
            bool result = TypeDescriptor.GetConverter(type).IsValid(obj);

            return result;
        }  
    }
}