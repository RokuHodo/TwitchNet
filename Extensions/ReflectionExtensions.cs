// project namespaces
using System;
using System.Collections.Generic;
using System.Reflection;

namespace TwitchNet.Extensions
{
    internal static class
    ReflectionExtensions
    {
        /// <summary>
        /// Checks to see if a <see cref="Type"/> has a custom attribute.
        /// </summary>
        /// <typeparam name="attribute_type">The <see cref="Attribute"/> type.</typeparam>
        /// <param name="type">The <see cref="Type"/> of the object</param>
        /// <returns>
        /// Returns true if the object <see cref="Type"/> has the attribute.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        HasAttribute<attribute_type>(this Type type)
        where attribute_type : Attribute
        {
            bool result = !type.GetCustomAttribute<attribute_type>().IsNull();

            return result;
        }

        /// <summary>
        /// Returns all the public properties of the current <see cref="Type"/> with a specified <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="attribute_type">The type of the attribute to search for.</typeparam>
        /// <param name="type">The current type.</param>
        /// <returns>
        /// Returns an array of all public properties of a <see cref="Type"/> with a specified <see cref="Attribute"/>.
        /// Returns an empty property array otherwise.
        /// </returns>
        public static PropertyInfo[]
        GetProperties<attribute_type>(this Type type)
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
        public static attribute_type
        GetAttribute<attribute_type>(this PropertyInfo property)
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

        /// <summary>
        /// Gets a custom <see cref="Attribute"/> from a property.
        /// </summary>
        /// <typeparam name="attribute_type">The <see cref="Attribute"/> type.</typeparam>
        /// <param name="field">The field to get the <see cref="Attribute"/> from.</param>
        /// <returns>
        /// Returns the custom <see cref="Attribute"/> if the property has it.
        /// Returns the default <see cref="Attribute"/> otherwise.
        /// </returns>
        public static attribute_type
        GetAttribute<attribute_type>(this FieldInfo field)
        where attribute_type : Attribute
        {
            attribute_type attribute = default(attribute_type);

            if (field.IsNull())
            {
                return attribute;
            }

            attribute = Attribute.GetCustomAttribute(field, typeof(attribute_type)) as attribute_type;

            return attribute;
        }

        /// <summary>
        /// Tries to get a custom <see cref="Attribute"/> from an enum value.
        /// </summary>
        /// <typeparam name="attribute_type">The <see cref="Attribute"/> type.</typeparam>
        /// <param name="value">The value of the <see cref="Enum"/>.</param>
        /// <param name="attribute">
        /// The object value of <see cref="Enum"/> if the <see cref="Attribute"/> is successfully found.
        /// If the <see cref="Attribute"/> is not found, the value is <see cref="null"/>.</param>
        /// <returns>
        /// Returns true if the <see cref="Enum"/> value has the custom <see cref="Attribute"/> property.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        TryGetAttribute<attribute_type>(this Enum value, out attribute_type attribute)
        where attribute_type : Attribute
        {
            Type enum_type = value.GetType();
            string field = Enum.GetName(enum_type, value);

            attribute = enum_type.GetField(field).GetAttribute<attribute_type>();

            bool success = !attribute.IsNull();

            return success;
        }
    }
}
