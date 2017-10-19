﻿// standard namespaces
using System;
using System.Collections;
using System.Reflection;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Models.Paging;
using TwitchNet.Interfaces.Helpers.Paging;

// imported .dll's
using RestSharp;

namespace TwitchNet.Utilities
{
    internal static class
    PagingUtil
    {
        /// <summary>
        /// Adds a set optional of query string parameters to a customize the <see cref="RestRequest"/>.
        /// </summary>
        /// <param name="request">The rest request to add the query parameters to.</param>
        /// <param name="query_parameters">The optional query string parameters to be added to the request.</param>
        /// <returns></returns>
        public static IRestRequest
        AddPaging(IRestRequest request, params QueryParameter[] query_parameters)
        {
            if (!query_parameters.IsValid())
            {
                return request;
            }

            foreach(QueryParameter query_parameter in query_parameters)
            {
                if (!query_parameter.name.IsValid() || !query_parameter.value.IsValid())
                {
                    continue; ;
                }

                request.AddQueryParameter(query_parameter.name, query_parameter.value);
            }

            return request;
        }

        // TODO: AddPaging - Find a better way to implement endpoint paging by using QueryParameter or something similar?

        /// <summary>
        /// Adds a set optional of query string parameters to a customize the <see cref="RestRequest"/>.
        /// </summary>
        /// <typeparam name="parameters_type">The object type of the parameters class</typeparam>
        /// <param name="request">The rest request to add the query parameters to.</param>
        /// <param name="query_parameters">The optional query string parameters to be added to the request.</param>
        /// <returns></returns>
        public static IRestRequest
        AddPaging<parameters_type>(IRestRequest request, parameters_type query_parameters)
        where parameters_type : ITwitchQueryParameters, new()
        {
            if (query_parameters.IsNull())
            {
                query_parameters = new parameters_type();
            }

            PropertyInfo[] properties = query_parameters.GetType().GetProperties<QueryParameterAttribute>();
            foreach (PropertyInfo property in properties)
            {                
                object value = property.GetValue(query_parameters);
                if (value.IsNull())
                {
                    continue;
                }

                QueryParameterAttribute attribute = property.GetAttribute<QueryParameterAttribute>();

                Type type = property.PropertyType.IsNullable() ? Nullable.GetUnderlyingType(property.PropertyType) : property.PropertyType;
                if (type.IsList())
                {
                    IList list = value as IList;
                    if (list.IsNull() || list.Count == 0)
                    {
                        continue;
                    }

                    foreach (object element in list)
                    {
                        request = AddQueryParameter(request, attribute, element);
                    }
                }
                else if (type.IsEnum)
                {
                    if (type.HasAttribute<FlagsAttribute>())
                    {
                        // enum is a bit field, loop through and add all flags
                        Enum property_value_enum = (Enum)value;
                        Array flags = Enum.GetValues(type);
                        foreach(Enum flag in flags)
                        {
                            if (property_value_enum.HasFlag(flag))
                            {
                                string enum_value = flag.ToEnumString();
                                request = AddQueryParameter(request, attribute, enum_value);
                            }
                        }
                    }
                    else
                    {
                        // enum is a single value
                        string enum_value = ((Enum)value).ToEnumString();
                        request = AddQueryParameter(request, attribute, enum_value);
                    }
                }
                else
                {
                    request = AddQueryParameter(request, attribute, value);
                }
            }

            return request;
        }

        /// <summary>
        /// Adds an object as a query parameter to a <see cref="RestRequest"/>.
        /// </summary>
        /// <param name="request">The rest request to add the query parameters to.</param>
        /// <param name="attribute">The attribute that contains the query name and conversion settings.</param>
        /// <param name="value">The object value to be added as a query parameter.</param>
        /// <returns></returns>
        private static IRestRequest
        AddQueryParameter(IRestRequest request, QueryParameterAttribute attribute, object value)
        {
            if (value.IsNull())
            {
                return request;
            }

            string query_value = value.ToString();
            if (!attribute.query_name.IsValid() || !query_value.IsValid())
            {
                return request;
            }

            if (attribute.to_lower)
            {
                query_value = query_value.ToLower();
            }

            request.AddQueryParameter(attribute.query_name, query_value);

            return request;
        }
    }
}
