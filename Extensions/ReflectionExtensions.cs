// project namespaces
using System;
using System.Collections.Generic;
using System.Reflection;

namespace TwitchNet.Extensions
{
    internal static class ReflectionExtensions
    {
        /// <summary>
        /// Returns all the public properties of the current <see cref="Type"/> with a specified <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="attribute_type">The type of the attribute to search for.</typeparam>
        /// <param name="type">The current type.</param>
        /// <returns>
        /// Returns an array of all public properties of a <see cref="Type"/> with a specified <see cref="Attribute"/>.
        /// Returns an empty property array otherwise.
        /// </returns>
        public static PropertyInfo[] GetProperties<attribute_type>(this Type type)
        where attribute_type : Attribute
        {
            List<PropertyInfo> result = new List<PropertyInfo>();

            if (type.IsNull())
            {
                return result.ToArray();
            }

            PropertyInfo[] properties = type.GetProperties();
            if (!properties.IsValid())
            {
                return result.ToArray();
            }

            foreach(PropertyInfo property in properties)
            {
                if(!Attribute.GetCustomAttribute(property, typeof(attribute_type)).IsNull())
                {
                    result.Add(property);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Gets a custom <see cref="Attribute"/> from a property.
        /// </summary>
        /// <typeparam name="attribute_type">The <see cref="Attribute"/> type.</typeparam>
        /// <param name="property">The property to get the <see cref="Attribute"/> from.</param>
        /// <returns>
        /// Returns the custom <see cref="Attribute"/> if the property has it.
        /// Returns the default <see cref="Attribute"/> otherwise.
        /// </returns>
        public static attribute_type GetAttribute<attribute_type>(this PropertyInfo property)
        where attribute_type : Attribute
        {
            attribute_type attribute = default(attribute_type);

            if (property.IsNull())
            {
                return attribute;
            }

            attribute = Attribute.GetCustomAttribute(property, typeof(attribute_type)) as attribute_type;

            return attribute;
        }
    }
}
