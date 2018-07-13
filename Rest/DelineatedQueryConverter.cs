// standard namespaces
using System;
using System.Collections;
using System.Collections.Generic;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Utilities;

// imported .dll's
using RestSharp;

namespace TwitchNet.Rest
{
    /// <summary>
    /// <para>
    /// Converts the elements of an array/list or the set flags of a bitfield enum into a deliniated string.
    /// The delineated string is then added to the <see cref="RestRequest"/> as a single query parameter.
    /// </para>
    /// <para>By default, the  delineator is empty and needs to set before the formatter can be used.</para>
    /// </summary>
    public class
    DelineatedQueryConverter : RestParameterConverter
    {
        /// <summary>
        /// The query parameter value delineator (separator).
        /// </summary>
        protected string delineator = string.Empty;

        public override RestRequest
        AddParameter(RestRequest request, Parameter parameter, Type member_type)
        {
            if (!delineator.IsValid())
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

                Type element_type = member_type.GetElementType();
                if(!element_type.IsValueType && !element_type.IsEnum)
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
                    if (value.IsValid())
                    {
                        return request;
                    }

                    request.AddQueryParameter(parameter.Name, value);
                }
                else
                {
                    string value = string.Join(delineator, list);
                    if (value.IsValid())
                    {
                        return request;
                    }

                    request.AddQueryParameter(parameter.Name, value);
                }
            }

            return request;
        }

        public override bool
        CanConvert(Parameter parameter, Type member_type)
        {
            return  parameter.Type == ParameterType.QueryString &&  (member_type.IsArray || member_type.IsList() || (member_type.IsEnum && member_type.HasAttribute<FlagsAttribute>()));
        }
    }
}
