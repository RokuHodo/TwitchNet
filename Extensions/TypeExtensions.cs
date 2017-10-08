// project namespaces
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TwitchNet.Extensions
{
    internal static class TypeExtensions
    {
        /// <summary>
        /// Checks to see if an object's type is nullable.
        /// </summary>
        /// <param name="type">The object type to check.</param>
        /// <returns>
        /// Returns <see cref="true"/> if the type is <see cref="Nullable{T}"/>.
        /// Returns <see cref="false"/> otherwise.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool isNullable(this Type type)
        {
            bool result = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

            return result;
        }

        /// <summary>
        /// Checks to see if an <see cref="object"/>'s type is a generic list.
        /// </summary>
        /// <param name="type">The object type to check.</param>
        /// <returns>
        /// Returns <see cref="true"/> if the type is a <see cref="List{T}"/>.
        /// Returns <see cref="false"/> otherwise.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool isList(this Type type)
        {
            bool result = type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));

            return result;
        }
    }
}
