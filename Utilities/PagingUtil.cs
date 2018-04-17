// standard namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Models.Api;

// imported .dll's
using RestSharp;

namespace
TwitchNet.Utilities
{
    internal static class
    PagingUtil
    {
        /// <summary>
        /// Adds query parameters to the <see cref="RestRequest"/>.
        /// </summary>
        /// <param name="request">The rest request.</param>
        /// <param name="parameters">The query string parameters to add.</param>
        /// <returns>Returns the <see cref="RestRequest"/> with the added <paramref name="parameters"/>.</returns>
        public static RestRequest
        AddPaging(RestRequest request, IList<QueryParameter> parameters)
        {
            if (!parameters.IsValid())
            {
                return request;
            }

            foreach(QueryParameter query_parameter in parameters)
            {
                if (!query_parameter.name.IsValid() || !query_parameter.value.IsValid())
                {
                    continue;
                }

                request.AddQueryParameter(query_parameter.name, query_parameter.value);
            }

            return request;
        }

        /// <summary>
        /// Adds query parameters to the <see cref="RestRequest"/>.
        /// </summary>
        /// <typeparam name="parameters_type">The object type of the parameters class</typeparam>
        /// <param name="request">The rest request.</param>
        /// <param name="parameters">The query string parameters to add.</param>
        /// <returns>Returns the <see cref="RestRequest"/> with the added <paramref name="parameters"/>.</returns>
        public static RestRequest
        AddPaging(RestRequest request, object parameters)
        {
            if (parameters.IsNull())
            {
                return request;
            }

            PropertyInfo[] properties = parameters.GetType().GetProperties<QueryParameterAttribute>();
            if (!properties.IsValid())
            {
                return request;
            }

            foreach (PropertyInfo property in properties)
            {                
                object value = property.GetValue(parameters);
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
                                // TODO: This is EXTREMELY slow. Go through any enum used in paging and add it to the cache.
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
        /// <param name="request">The rest request.</param>
        /// <param name="attribute">The attribute that contains the query name and conversion settings.</param>
        /// <param name="value">The query parameter value.</param>
        /// <returns>Returns the <see cref="RestRequest"/> with the added query parameters.</returns>
        private static RestRequest
        AddQueryParameter(RestRequest request, QueryParameterAttribute attribute, object value)
        {
            if (attribute.IsNull() || !attribute.query_name.IsValid())
            {
                return request;
            }

            string query_value = value.ToString();
            if (value.IsNull() || !query_value.IsValid())
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
