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
    SeparateQueryConverter : RestParameterConverter
    {
        public override RestRequest
        AddParameter(RestRequest request, Parameter parameter, Type member_type)
        {
            if (member_type.IsEnum)
            {
                if (!EnumUtil.TryGetFlagNames(member_type, parameter.Value, out string[] names))
                {
                    return request;
                }

                foreach(string element in names)
                {
                    if (!element.IsValid())
                    {
                        continue;
                    }

                    request.AddQueryParameter(parameter.Name, element);
                }                
            }
            else
            {
                IList list = parameter.Value as IList;
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
                            parameter.Value = element;
                            request = AddParameter(request, parameter, element_type);
                        }
                        else if (EnumUtil.TryGetName(element_type, element, out string value) && value.IsValid())
                        {
                            request.AddQueryParameter(parameter.Name, value);
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

                        request.AddQueryParameter(parameter.Name, value);
                    }
                }               
            }            

            return request;
        }

        public override bool
        CanConvert(Parameter parameter, Type member_type)
        {
            return  parameter.Type == ParameterType.QueryString && (member_type.IsArray || member_type.IsList() || (member_type.IsEnum && member_type.HasAttribute<FlagsAttribute>()));
        }
    }
}
