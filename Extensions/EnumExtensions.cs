// project namespaces
using System;
using System.Runtime.Serialization;

namespace TwitchNet.Extensions
{
    internal static class
    EnumExtensions
    {
        /// <summary>
        /// Converts an <see cref="Enum"/> value to a string, first by looking for the <see cref="EnumMemberAttribute.Value"/>, and then by it's actual value if the <see cref="Enum"/> value does not have the <see cref="EnumMemberAttribute"/>.
        /// </summary>
        /// <param name="value">The value of the <see cref="Enum"/></param>
        /// <returns>
        /// Returns an empty <see cref="string"/> if the <see cref="Enum"/> value is null.
        /// Returns <see cref="EnumMemberAttribute.Value"/> if the <see cref="Enum"/> value has the attribute.
        /// Returns the name of <see cref="Enum"/> by using <see cref="Enum.ToString()"/> otherwise.
        /// </returns>
        public static string
        ToEnumString(this Enum value)
        {
            string name = string.Empty;

            if (value.IsNull())
            {
                return name;
            }

            name = value.ToString();

            if (value.TryGetAttribute(out EnumMemberAttribute attribute))
            {
                name = attribute.Value;
            }

            return name;
        }
    }
}
