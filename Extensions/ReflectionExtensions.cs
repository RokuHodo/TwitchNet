// project namespaces
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

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
        /// Returns all public properties of the current <see cref="Type"/> with a specified <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="attribute_type">The type of the attribute to search for.</typeparam>
        /// <param name="type">The object's type.</param>
        /// <returns>
        /// Returns all public properties marked with the specified <see cref="Attribute"/>.
        /// Returns an empty array otherwise.
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
                if(property.HasAttribute(typeof(attribute_type)))
                {
                    result.Add(property);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Returns all public members of the current <see cref="Type"/> with a specified <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="attribute_type">The type of the attribute to search for.</typeparam>
        /// <param name="type">The object's type.</param>
        /// <returns>
        /// Returns all public members marked with the specified <see cref="Attribute"/>.
        /// Returns an empty array otherwise.
        /// </returns>
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

        /// <summary>
        /// Filters and keeps members that are marked with at least one of the <see cref="Attribute"/> types to include.
        /// </summary>
        /// <param name="members">The members to filter.</param>
        /// <param name="include">The <see cref="Attribute"/> types to keep.</param>
        /// <returns>
        /// Returns an array of all members that are marked with at least one of the <see cref="Attribute"/> types to include.
        /// Returns the members if no <see cref="Attribute"/> types are specified.
        /// Returns an empty array otherwise.
        /// </returns>
        public static MemberInfo[]
        FilterMembersByAttribute(this MemberInfo[] members, params Type[] include)
        {
            if (members.IsNull())
            {
                return new MemberInfo[0];
            }

            if (include.IsNull())
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

                    break;
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Filters and removes members that are marked with at least one of the <see cref="Attribute"/> types to exclude.
        /// </summary>
        /// <param name="members">The members to filter.</param>
        /// <param name="exclude">The <see cref="Attribute"/> types to filter out.</param>
        /// <returns>
        /// Returns an array of all members that are not marked with any of the <see cref="Attribute"/> types to exclude.
        /// Returns the members if no <see cref="Attribute"/> types are specified.
        /// Returns an empty array otherwise.
        /// </returns>
        public static MemberInfo[]
        ExcludeMembersByAttribute(this MemberInfo[] members, params Type[] exclude)
        {
            if (!exclude.IsValid())
            {
                return members;
            }

            if (members.IsNull())
            {
                return new MemberInfo[0];
            }

            List<MemberInfo> result = new List<MemberInfo>();
            foreach (MemberInfo member in members)
            {
                bool add = true;
                foreach (Type element in exclude)
                {
                    if (member.HasAttribute(element))
                    {
                        add = false;

                        break;
                    }
                }

                if (add)
                {
                    result.Add(member);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Checks to see if a <see cref="Type"/> is marked with a custom <see cref="Attribute"/>.
        /// </summary>
        /// <param name="member">The member to check.</param>
        /// <param name="attribute">The <see cref="Attribute"/> type.</param>
        /// <returns></returns>
        public static bool
        HasAttribute(this MemberInfo member, Type attribute)
        {
            if (member.IsNull() || attribute.IsNull())
            {
                return false;
            }

            bool result = member.IsDefined(attribute, false);

            return result;
        }

        /// <summary>
        /// Checks to see if a <see cref="MemberInfo"/> is marked with a custom <see cref="Attribute"/>.
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
            bool result = member.HasAttribute(typeof(attribute_type));

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object
        GetAttribute(this MemberInfo member, Type type)
        {
            if (member.IsNull())
            {
                return null;
            }

            if (member.MemberType != MemberTypes.Constructor &&
                member.MemberType != MemberTypes.Property &&
                member.MemberType != MemberTypes.Event &&
                member.MemberType != MemberTypes.TypeInfo &&
                member.MemberType != MemberTypes.Event &&
                member.MemberType != MemberTypes.Field)
            {
                return null;
            }

            object attribute = Attribute.GetCustomAttribute(member, type, false);

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
