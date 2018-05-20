// project namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace
TwitchNet.Extensions
{
    internal static class
    ReflectionExtensions
    {   
        /// <summary>
        /// Gets a type's default value.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// Returns null if the type is null.
        /// Returns the type's default value otherwise.
        /// </returns>
        public static object
        GetDefaultValue(this Type type)
        {
            if (type.IsNull())
            {
                return null;
            }

            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
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
                if(property.HasAttribute<attribute_type>())
                {
                    result.Add(property);
                }
            }

            return result.ToArray();
        }

        public static MemberInfo[]
        GetMembers<attribute_type>(this Type type)
        where attribute_type : Attribute
        {
            if (type.IsNull())
            {
                return new MemberInfo[0];
            }

            MemberInfo[] members = type.GetMembers();
            if (!members.IsValid())
            {
                return new MemberInfo[0];
            }

            List<MemberInfo> result = new List<MemberInfo>();
            foreach (MemberInfo member in members)
            {
                if (!member.HasAttribute<attribute_type>())
                {
                    continue;
                }

                result.Add(member);
            }

            return result.ToArray();
        }

        public static MemberInfo[]
        GetMembers<attribute_type>(this Type type, params Type[] exclude)
        where attribute_type : Attribute
        {
            MemberInfo[] members = type.GetMembers<attribute_type>();
            if (!exclude.IsValid())
            {
                return members;
            }

            List<MemberInfo> result = new List<MemberInfo>();
            foreach (MemberInfo member in members)
            {
                foreach (Type element in exclude)
                {
                    if (typeof(Attribute).IsAssignableFrom(element) && member.HasAttribute(element))
                    {
                        continue;
                    }

                    result.Add(member);
                }
            }

            return result.ToArray();
        }

        public static MemberInfo[]
        FilterMembers(this MemberInfo[] members, params Type[] include)
        {
            if (!include.IsValid())
            {
                return members;
            }

            List<MemberInfo> result = new List<MemberInfo>();
            foreach (MemberInfo member in members)
            {
                foreach (Type element in include)
                {
                    if (!member.HasAttribute(element))
                    {
                        continue;
                    }

                    result.Add(member);
                }
            }

            return result.ToArray();
        }

        public static MemberInfo[]
        ExcludeMembers(this MemberInfo[] members, params Type[] exclude)
        {
            if (!exclude.IsValid())
            {
                return members;
            }

            List<MemberInfo> result = new List<MemberInfo>();
            foreach (MemberInfo member in members)
            {
                foreach (Type element in exclude)
                {
                    if (member.HasAttribute(element))
                    {
                        continue;
                    }

                    result.Add(member);
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
            if (type.IsNull())
            {
                return false;
            }

            if (type.MemberType != MemberTypes.Constructor  &&
                type.MemberType != MemberTypes.Property     &&
                type.MemberType != MemberTypes.Event        &&
                type.MemberType != MemberTypes.TypeInfo     &&
                type.MemberType != MemberTypes.Event        &&
                type.MemberType != MemberTypes.Field)
            {
                return false;
            }

            IEnumerable<attribute_type> attributes = type.GetCustomAttributes<attribute_type>();

            bool result = !attributes.IsNull() && attributes.Count() > 0;

            return result;
        }

        public static bool
        HasAttribute(this MemberInfo member, Type attribute)
        {
            if (member.IsNull())
            {
                return false;
            }

            if(!typeof(Attribute).IsAssignableFrom(attribute))
            {
                return false;
            }

            if (member.MemberType != MemberTypes.Constructor &&
                member.MemberType != MemberTypes.Property &&
                member.MemberType != MemberTypes.Event &&
                member.MemberType != MemberTypes.TypeInfo &&
                member.MemberType != MemberTypes.Event &&
                member.MemberType != MemberTypes.Field)
            {
                return false;
            }

            IEnumerable<Attribute> attributes = member.GetCustomAttributes(attribute);

            bool result = !attributes.IsNull() && attributes.Count() > 0;

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
            if (member.IsNull())
            {
                return false;
            }

            if (member.MemberType != MemberTypes.Constructor    &&
                member.MemberType != MemberTypes.Property       &&
                member.MemberType != MemberTypes.Event          &&
                member.MemberType != MemberTypes.TypeInfo       &&
                member.MemberType != MemberTypes.Event          &&
                member.MemberType != MemberTypes.Field)
            {
                return false;
            }

            bool result = Attribute.GetCustomAttributes(member, typeof(attribute_type), false).IsValid();

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
            if (member.IsNull())
            {
                return null;
            }

            if (member.MemberType != MemberTypes.Constructor    &&
                member.MemberType != MemberTypes.Property       &&
                member.MemberType != MemberTypes.Event          &&
                member.MemberType != MemberTypes.TypeInfo       &&
                member.MemberType != MemberTypes.Event          &&
                member.MemberType != MemberTypes.Field)
            {
                return null;
            }

            attribute_type attribute = Attribute.GetCustomAttribute(member, typeof(attribute_type), false) as attribute_type;

            return attribute;
        }

        /// <summary>
        /// Gets all instances of a custom <see cref="Attribute"/> from a <see cref="MemberInfo"/>.
        /// </summary>
        /// <typeparam name="attribute_type">The <see cref="Attribute"/> type.</typeparam>
        /// <param name="member">The member to get the <see cref="Attribute"/>s from.</param>
        /// <returns>
        /// Returns all instances of the custom <see cref="Attribute"/> if the member has it.
        /// Returns an empty array otherwise otherwise.
        /// </returns>
        public static attribute_type[]
        GetAttributes<attribute_type>(this MemberInfo member)
        where attribute_type : Attribute
        {
            if (member.IsNull())
            {
                return new attribute_type[0];
            }

            if (member.MemberType != MemberTypes.Constructor    &&
                member.MemberType != MemberTypes.Property       &&
                member.MemberType != MemberTypes.Event          &&
                member.MemberType != MemberTypes.TypeInfo       &&
                member.MemberType != MemberTypes.Event          &&
                member.MemberType != MemberTypes.Field)
            {
                return new attribute_type[0];
            }

            attribute_type[] attribute = Attribute.GetCustomAttributes(member, typeof(attribute_type)) as attribute_type[];

            return attribute;
        }

        /// <summary>
        /// Attempts to get an instance of a custom <see cref="Attribute"/> from a member.
        /// </summary>
        /// <typeparam name="attribute_type">The <see cref="Attribute"/> type.</typeparam>
        /// <param name="member">The member to get the <see cref="Attribute"/>s from.</param>
        /// <param name="attribute">The instance of the attribute if it exists.</param>
        /// <returns>
        /// Returns true if the member contained an instance of the custom <see cref="Attribute"/>.
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

        /// <summary>
        /// Attempts to get all instances of a custom <see cref="Attribute"/> from a member.
        /// </summary>
        /// <typeparam name="attribute_type">The <see cref="Attribute"/> type.</typeparam>
        /// <param name="member">The member to get the <see cref="Attribute"/>s from.</param>
        /// <param name="attribute">The instances of the attribute if they exists.</param>
        /// <returns>
        /// Returns true if the member contained at least one instance of the custom <see cref="Attribute"/>.
        /// Returns false otherwise.
        /// </returns>
        public static bool
        TryGetAttributes<attribute_type>(this MemberInfo member, out attribute_type[] attribute)
        where attribute_type : Attribute
        {
            attribute = member.GetAttributes<attribute_type>();

            bool success = attribute.IsValid();

            return success;
        }
    }
}
