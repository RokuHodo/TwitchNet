// standard namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Enums.Api;
using TwitchNet.Extensions;
using TwitchNet.Helpers.Paging;
using TwitchNet.Models.Api;

// imported .dll's
using RestSharp;

namespace TwitchNet.Utilities
{
    internal static class PagingUtil
    {
        /// <summary>
        /// Adds a set optional of query string parameters to a <see cref="RestRequest"/> to customize the request.
        /// </summary>
        /// <typeparam name="parameters_type">The object type of the parameters class</typeparam>
        /// <param name="request">The rest request to add the query parameters to.</param>
        /// <param name="parameters">The optional query string parameters to be added to the request.</param>
        /// <returns></returns>
        public static RestRequest AddPaging<parameters_type>(RestRequest request, parameters_type parameters)
        where parameters_type : new()
        {
            if (parameters.IsNull())
            {
                parameters = new parameters_type();
            }

            PropertyInfo[] properties = parameters.GetType().GetProperties<QueryParameterAttribute>();
            foreach (PropertyInfo property in properties)
            {                
                object value = property.GetValue(parameters);
                if (value.IsNull())
                {
                    continue;
                }

                Type type = property.PropertyType.isNullable() ? Nullable.GetUnderlyingType(property.PropertyType) : property.PropertyType;
                QueryParameterAttribute attribute = property.GetAttribute<QueryParameterAttribute>();

                if (type.isList())
                {
                    IList list = value as IList;
                    if (!list.IsValid())
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
        private static RestRequest AddQueryParameter(RestRequest request, QueryParameterAttribute attribute, object value)
        {
            if (value.IsNull())
            {
                return request;
            }

            string query_value = value.ToString();
            if (!attribute.query_name.isValid() || !query_value.isValid())
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

        /// <summary>
        /// Get the complete list of objects using the passed request method and parameters.
        /// </summary>
        /// <typeparam name="return_type">The object type that is returned.</typeparam>
        /// <typeparam name="page_return_type">The object type that is returned by the passed request method.</typeparam>
        /// <typeparam name="parameters_type">The object type of the parameters class</typeparam>
        /// <param name="GetPage">The method to request each page.</param>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="parameters">Optional. A set of parameters to customize the requests.</param>
        /// <returns></returns>
        public static async Task<List<return_type>> GetAllPagesAsync<return_type, page_return_type, parameters_type>(Func<Authentication, string, parameters_type, Task<page_return_type>> GetPage, Authentication authentication, string token, parameters_type parameters)
        where parameters_type : new()
        {
            List<return_type> result = new List<return_type>();

            if (parameters.IsNull())
            {
                parameters = new parameters_type();
            }

            bool requesting = true;
            do
            {
                // request the page
                page_return_type page = await GetPage(authentication, token, parameters);
                PropertyInfo page_data = page.GetType().GetProperty("data");

                List<return_type> page_data_value = (List<return_type>)page_data.GetValue(page);
                foreach (return_type element in page_data_value)
                {
                    result.Add(element);
                }

                // check to see if there is a new page to request
                PropertyInfo page_pagination = page.GetType().GetProperty("pagination");
                Pagination pagination = (Pagination)page_pagination.GetValue(page);

                requesting = pagination.cursor.isValid() && page_data_value.IsValid();
                if (requesting)
                {
                    // update the parameter's 'after' property to properly request the next page
                    PropertyInfo parameters_after = parameters.GetType().GetProperty("after");
                    parameters_after.SetValue(parameters, pagination.cursor);
                }
            }
            while (requesting);

            return result;
        }

        /// <summary>
        /// Get the complete list of objects using the passed request method and parameters.
        /// </summary>
        /// <typeparam name="return_type">The object type that is returned.</typeparam>
        /// <typeparam name="page_return_type">The object type that is returned by the passed request method.</typeparam>
        /// <typeparam name="parameters_type">The object type of the parameters class</typeparam>
        /// <param name="GetPage">The method to request each page.</param>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="page_arg_1">A parameter that is passed through to the request method as an argument.</param>
        /// <param name="parameters">Optional. A set of parameters to customize the requests.</param>
        /// <returns></returns>
        public static async Task<List<return_type>> GetAllPagesAsync<return_type, page_return_type, parameters_type>(Func<Authentication, string, string, parameters_type, Task<page_return_type>> GetPage, Authentication authentication, string token, string page_arg_1, parameters_type parameters)
        where parameters_type : new()
        {
            List<return_type> result = new List<return_type>();

            if (parameters.IsNull())
            {
                parameters = new parameters_type();
            }

            bool requesting = true;
            do
            {
                // request the page
                page_return_type page = await GetPage(authentication, token, page_arg_1, parameters);
                PropertyInfo page_data = page.GetType().GetProperty("data");

                List<return_type> page_data_value = (List<return_type>)page_data.GetValue(page);
                foreach (return_type element in page_data_value)
                {
                    result.Add(element);
                }

                // check to see if there is a new page to request
                PropertyInfo page_pagination = page.GetType().GetProperty("pagination");
                Pagination pagination = (Pagination)page_pagination.GetValue(page);

                requesting = pagination.cursor.isValid() && page_data_value.IsValid();
                if (requesting)
                {
                    // update the parameter's 'after' property to properly request the next page
                    PropertyInfo parameters_after = parameters.GetType().GetProperty("after");
                    parameters_after.SetValue(parameters, pagination.cursor);
                }
            }
            while (requesting);

            return result;
        }

        /// <summary>
        /// Get the complete list of objects using the passed request method and parameters.
        /// </summary>
        /// <typeparam name="return_type">The object type that is returned.</typeparam>
        /// <typeparam name="page_return_type">The object type that is returned by the passed request method.</typeparam>
        /// <typeparam name="parameters_type">The object type of the parameters class</typeparam>
        /// <param name="GetPage">The method to request each page.</param>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="page_arg_1">A parameter that is passed through to the request method as an argument.</param>
        /// <param name="page_arg_2">A parameter that is passed through to the request method as an argument.</param>
        /// <param name="parameters">Optional. A set of parameters to customize the requests.</param>
        /// <returns></returns>
        public static async Task<List<return_type>> GetAllPagesAsync<return_type, page_return_type, parameters_type>(Func<Authentication, string, string, string, parameters_type, Task<page_return_type>> GetPage, Authentication authentication, string token, string page_arg_1, string page_arg_2,  parameters_type parameters)
        where parameters_type : new()
        {
            List<return_type> result = new List<return_type>();

            if (parameters.IsNull())
            {
                parameters = new parameters_type();
            }

            bool requesting = true;
            do
            {
                // request the page
                page_return_type page = await GetPage(authentication, token, page_arg_1, page_arg_2, parameters);
                PropertyInfo page_data = page.GetType().GetProperty("data");

                List<return_type> page_data_value = (List<return_type>)page_data.GetValue(page);
                foreach (return_type element in page_data_value)
                {
                    result.Add(element);
                }

                // check to see if there is a new page to request
                PropertyInfo page_pagination = page.GetType().GetProperty("pagination");
                Pagination pagination = (Pagination)page_pagination.GetValue(page);

                requesting = pagination.cursor.isValid() && page_data_value.IsValid();
                if (requesting)
                {
                    // update the parameter's 'after' property to properly request the next page
                    PropertyInfo parameters_after = parameters.GetType().GetProperty("after");
                    parameters_after.SetValue(parameters, pagination.cursor);
                }
            }
            while (requesting);

            return result;
        }
    }
}
