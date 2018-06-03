// project namespaces
using System;
using System.Runtime.Serialization;

namespace
TwitchNet.Extensions
{
    internal static class
    EnumExtensions
    {
        /// <summary>
        /// <para>
        /// Converts a string into an <see cref="Enum"/> value.
        /// This supports the use of <see cref="EnumMemberAttribute"/>.
        /// </para>
        /// <para>WARNING: This method is significantly slower than <see cref="Enum.Parse(Type, string)"/> and should only be used if the enum implemements of <see cref="EnumMemberAttribute"/>.</para>
        /// </summary>
        /// <typeparam name="enum_type">The type of the enum to parse.</typeparam>
        /// <param name="str">The string to parse into an enum.</param>
        /// <returns>
        /// Returns the enum value whose name matches the provided string.
        /// Returns null if no match was found.
        /// </returns>
        public static object
        ToEnum<enum_type>(this string str)
        where enum_type : struct, IConvertible
        {
            Type type = typeof(enum_type);
            if (!type.IsEnum)
            {
                throw new ArgumentException(nameof(enum_type) + " must be an enum.", nameof(enum_type));
            }

            object value = null;

            // Don't do !IsValid() since the value could acutally be white space
            if (str.IsNull())
            {
                return value;
            }            

            foreach(enum_type element in Enum.GetValues(type))
            {
                string name = Enum.GetName(type, element);
                if (name == str)
                {
                    return element;
                }

                if(type.GetField(name).TryGetAttribute(out EnumMemberAttribute attribute) && str == attribute.Value)
                {
                    return element;
                }
            }

            return value;
        }
    }
}
