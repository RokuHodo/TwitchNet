// standard namespaces
using System;
using System.Collections;
using System.Collections.Generic;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Utilities;

// imported .dll's
using RestSharp;

namespace
TwitchNet.Rest
{
    /// <summary>
    /// <para>
    /// Converts the elements of an array/list or the set flags of a bitfield enum into a deliniated string.
    /// The delineated string is then added to the <see cref="RestRequest"/> as a single query parameter.
    /// </para>
    /// <para>By default, the  delineator is null and needs to set before the converter can be used.</para>
    /// </summary>
    public class
    DelineatedQueryConverter : RestParameterConverter
    {
        /// <summary>
        /// The query parameter value delineator (separator).
        /// </summary>
        protected string delineator = string.Empty;

        /// <summary>
        /// Converts the elements of an array/list or the set flags of a bitfield enum into a deliniated string.
        /// The delineated string is then added to the <see cref="RestRequest"/> as a single query parameter.
        /// </summary>
        /// <param name="request">
        /// <para>The request to be executed.</para>
        /// <para>The request will never be null and will always be instantiated.</para>
        /// </param>
        /// <param name="paramerter">The rest parameter to be converted.</param>
        /// <param name="member_type">
        /// <para>The type of the member marked with <see cref="RestParameterConverter"/>.</para>
        /// <para>
        /// If the type of the member is nullable, this type is the underlying type.
        /// The member type will never be null and always be instantiated.
        /// </para>
        /// </param>
        /// <returns>Returns the rest request to be executed.</returns>
        public override IRestRequest
        AddParameter(IRestRequest request, Parameter parameter, Type member_type)
        {
            if (!parameter.Name.IsValid() || parameter.Value.IsNull())
            {
                return request;
            }

            if (delineator.IsNull())
            {
                return request;
            }

            if (member_type.IsEnum)
            {
                if (!EnumUtil.TryGetFlagNames(member_type, parameter.Value, out string[] names))
                {
                    return request;
                }

                if (!names.IsValid())
                {
                    return request;
                }

                string value = string.Join(delineator, names);
                request.AddQueryParameter(parameter.Name, value);
            }
            else
            {
                IList list = parameter.Value as IList;
                if (list.Count == 0)
                {
                    return request;
                }

                Type element_type = member_type.IsList() ? member_type.GetGenericArguments()[0] : member_type.GetElementType();
                if (!element_type.IsValueType && element_type != typeof(string) && !element_type.IsEnum)
                {
                    return request;
                }

                if (member_type.IsEnum)
                {
                    List<string> temp = new List<string>();

                    bool is_flags = member_type.HasAttribute<FlagsAttribute>();

                    foreach(object element in temp)
                    {
                        if (element.IsNull())
                        {
                            continue;
                        }

                        if (is_flags)
                        {
                            if(EnumUtil.TryGetFlagNames(element_type, element, out string[] names) && names.IsValid())
                            {
                                temp.AddRange(names);
                            }
                        }
                        else if (EnumUtil.TryGetName(element_type, element, out string _name) && _name.IsValid())
                        {
                            temp.Add(_name);
                        }
                    }

                    if (!temp.IsValid())
                    {
                        return request;
                    }

                    string value = string.Join(delineator, temp);
                    if (!value.IsValid())
                    {
                        return request;
                    }

                    request.AddQueryParameter(parameter.Name, value);
                }
                else
                {
                    string value = string.Join(delineator, list);
                    if (!value.IsValid())
                    {
                        return request;
                    }

                    request.AddQueryParameter(parameter.Name, value);
                }
            }

            return request;
        }

        /// <summary>
        /// Determines if the member marked with <see cref="RestParameterConverter"/> can be converted.
        /// </summary>
        /// <param name="paramerter">The rest parameter to be converted.</param>
        /// <param name="member_type">
        /// <para>The type of the member marked with <see cref="RestParameterConverter"/>.</para>
        /// <para>
        /// If the type of the member is nullable, this type is the underlying type.
        /// The member type will never be null and always be instantiated.
        /// </para>
        /// </param>
        /// <returns>
        /// Returns true if the <see cref="Parameter.Type"/> is equal to <see cref="ParameterType.QueryString"/> and the member is an array/list of value types or a bitfield enum.
        /// Returns false otherwise.
        /// </returns>
        public override bool
        CanConvert(Parameter parameter, Type member_type)
        {
            return parameter.Type == ParameterType.QueryString && (member_type.IsArray || member_type.IsList() || (member_type.IsEnum && member_type.HasAttribute<FlagsAttribute>()));
        }
    }
}
