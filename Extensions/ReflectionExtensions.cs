// project namespaces
using System;
using System.Collections.Generic;
using System.Reflection;

namespace
TwitchNet.Extensions
{
    internal static class
    ReflectionExtensions
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
                if(property.HasAttribute<attribute_type>())
                {
                    result.Add(property);
                }
            }

            return result.ToArray();
        }

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
        /// Checks to see if a member has a custom attribute.
        /// </summary>
        /// <typeparam name="attribute_type">The <see cref="Attribute"/> type.</typeparam>
        /// <param name="member">The member to check.</param>
        /// <returns>
        /// Returns true if the member has the attribute.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        HasAttribute<attribute_type>(this MemberInfo member)
        where attribute_type : Attribute
        {
            bool result = !Attribute.GetCustomAttribute(member, typeof(attribute_type)).IsNull();

            return result;
        }

        /// <summary>
        /// Gets a custom <see cref="Attribute"/> from a <see cref="MemberInfo"/>.
        /// </summary>
        /// <typeparam name="attribute_type">The <see cref="Attribute"/> type.</typeparam>
        /// <param name="member">The member to get the <see cref="Attribute"/> from.</param>
        /// <returns>
        /// Returns the custom <see cref="Attribute"/> if the member has it.
        /// Returns <see cref="null"/> otherwise.
        /// </returns>
        public static attribute_type
        GetAttribute<attribute_type>(this MemberInfo member)
        where attribute_type : Attribute
        {
            attribute_type attribute = null;

            if (member.IsNull())
            {
                return attribute;
            }

            attribute = Attribute.GetCustomAttribute(member, typeof(attribute_type)) as attribute_type;

            return attribute;
        }

        /// <summary>
        /// Tries to get a custom <see cref="Attribute"/> from a <see cref="PropertyInfo"/>.
        /// </summary>
        /// <typeparam name="attribute_type">The <see cref="Attribute"/> type.</typeparam>
        /// <param name="property">The value of the <see cref="Enum"/>.</param>
        /// <param name="attribute">
        /// The object value of <see cref="Enum"/> if the <see cref="Attribute"/> is successfully found.
        /// If the <see cref="Attribute"/> is not found, the value is <see cref="null"/>.</param>
        /// <returns>
        /// Returns true if the <see cref="Enum"/> value has the custom <see cref="Attribute"/> property.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        TryGetAttribute<attribute_type>(this MemberInfo member, out attribute_type attribute)
        where attribute_type : Attribute
        {
            attribute = member.GetAttribute<attribute_type>();

            bool success = !attribute.IsNull();

            return success;
        }
    }
}
