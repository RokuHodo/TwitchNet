// standard namespaces
using System;
using System.Collections;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Utilities;

// imported .dll's
using RestSharp;

namespace TwitchNet.Rest
{
    /// <summary>
    /// Adds the elements of an array/list or the set flags of a bitfield enum to the <see cref="RestRequest"/> as separate query parameters.
    /// </summary>
    public class
    SeparateQueryFormatter : QueryParameterFormatter
    {
        /// <summary>
        /// Adds the elements of an array/list or the set flags of a bitfield enum to a <see cref="RestRequest"/> as separate query parameters.
        /// </summary>
        /// <param name="request">
        /// <para>The request to be executed.</para>
        /// <para>This request will never be null and will always be instantiated.</para>
        /// </param>
        /// <param name="query_name">
        /// <para>The name of the qyery parameter obtained from the <see cref="QueryParameterAttribute.name"/>.</para>
        /// <para>The name will never be null, empty, or only contain whitespace and will always contain at least one character.</para>
        /// </param>
        /// <param name="member_type">
        /// <para>The type of the member marked with <see cref="QueryParameterAttribute"/>.</para>
        /// <para>
        /// If the type of the member is nullable, this type is the underlying type.
        /// The member type will never be null and always be instantiated.
        /// </para>
        /// </param>
        /// <param name="member_value">
        /// <para>The value of the member marked with <see cref="QueryParameterAttribute"/>.</para>
        /// <para>This member value will never be null and will always be instantiated.</para>
        /// </param>
        /// <returns>Returns the rest request.</returns>
        public override RestRequest
        FormatAndAdd(RestRequest request, string query_name, Type member_type, object member_value)
        {
            if (member_type.IsEnum)
            {
                if (!EnumUtil.TryGetFlagNames(member_type, member_value, out string[] names))
                {
                    return request;
                }

                foreach(string element in names)
                {
                    if (!element.IsValid())
                    {
                        continue;
                    }

                    request.AddQueryParameter(query_name, element);
                }                
            }
            else
            {
                IList list = member_value as IList;
                if (list.Count == 0)
                {
                    return request;
                }

                Type element_type = member_type.GetElementType();
                if (!element_type.IsValueType && !element_type.IsEnum)
                {
                    return request;
                }

                if (member_type.IsEnum)
                {
                    bool is_flags = member_type.HasAttribute<FlagsAttribute>();

                    foreach(object element in list)
                    {
                        if (element.IsNull())
                        {
                            continue;
                        }

                        if (is_flags)
                        {
                            request = FormatAndAdd(request, query_name, element_type, element);
                        }
                        else if (EnumUtil.TryGetName(element_type, element, out string value) && value.IsValid())
                        {
                            request.AddQueryParameter(query_name, value);
                        }
                    }                    
                }
                else
                {
                    foreach (object element in list)
                    {
                        if (element.IsNull())
                        {
                            continue;
                        }

                        string value = element.ToString();
                        if (!value.IsValid())
                        {
                            continue;
                        }

                        request.AddQueryParameter(query_name, value);
                    }
                }               
            }            

            return request;
        }

        /// <summary>
        /// Determines if the member marked with <see cref="QueryParameterAttribute"/> can/should be formatted using this formatter.
        /// </summary>
        /// <param name="member_type">
        /// <para>The type of the member marked with <see cref="QueryParameterAttribute"/>.</para>
        /// <para>
        /// If the type of the member is nullable, this type is the underlying type.
        /// The member type will never be null and always be instantiated.
        /// </para>
        /// </param>
        /// <returns>
        /// Returns true of the member is an array, list, or bitfield enum.
        /// Returns false otherwise.
        /// </returns>
        public override bool
        CanFormat(Type member_type)
        {
            return member_type.IsArray || member_type.IsList() || (member_type.IsEnum && member_type.HasAttribute<FlagsAttribute>());
        }
    }
}
