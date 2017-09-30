// standard namespaces
using System;
using System.Collections.Generic;
using System.Reflection;
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
        /// <typeparam name="paging_parameters_class">The object type of the parameters class</typeparam>
        /// <param name="request">The rest request to be executed.</param>
        /// <param name="parameters">The optional query string parameters to be added to the request.</param>
        /// <returns></returns>
        public static RestRequest AddPaging<paging_parameters_class>(RestRequest request, paging_parameters_class parameters)
        where paging_parameters_class : new()
        {
            if (parameters.isNull())
            {
                parameters = new paging_parameters_class();
            }

            List<PagingPropertyAttribute> attributes = GetPagingAttributes(parameters);

            foreach (PagingPropertyAttribute attribute in attributes)
            {
                object property_object = attribute.property.GetValue(parameters);
                if (property_object.isNull())
                {
                    continue;
                }

                string property_string = property_object.ToString();

                if (!property_string.isValid())
                {
                    continue;
                }

                request.AddQueryParameter(attribute.query_name, property_string.ToLower());
            }


            return request;
        }

        /// <summary>
        /// Gets the object properties marked by <see cref="PagingPropertyAttribute"/> to signify that they should be added to the <see cref="RestRequest"/> as optional query string parameters.
        /// </summary>
        /// <typeparam name="paging_parameters_class">The object type of the parameters class</typeparam>
        /// <param name="parameters">The optional query string parameters to be added to the request.</param>
        /// <returns></returns>
        private static List<PagingPropertyAttribute> GetPagingAttributes<paging_parameters_class>(paging_parameters_class parameters)
        where paging_parameters_class : new()
        {
            List<PagingPropertyAttribute> attributes = new List<PagingPropertyAttribute>();

            PropertyInfo[] properties = parameters.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.isNull())
                {
                    continue;
                }

                PagingPropertyAttribute attribute = (PagingPropertyAttribute)Attribute.GetCustomAttribute(property, typeof(PagingPropertyAttribute));

                if (attribute.isNull() || attribute.isDefault())
                {
                    continue;
                }

                attribute.property = property;

                attributes.Add(attribute);
            }

            return attributes;
        }

        /// <summary>
        /// Get the complete list of objects using the passed request method and parameters.
        /// </summary>
        /// <typeparam name="return_type">The object type that is returned.</typeparam>
        /// <typeparam name="page_return_type">The object type that is returned by the passed request method.</typeparam>
        /// <typeparam name="paging_parameters_class">The object type of the parameters class</typeparam>
        /// <param name="GetPage">The method to request each page.</param>
        /// <param name="authentication">How to authorize the request.</param>
        /// <param name="token">The OAuth token or Client Id to authorize the request.</param>
        /// <param name="param_1">A parameter that is passed through to the request method.</param>
        /// <param name="param_2">A parameter that is passed through to the request method.</param>
        /// <param name="parameters">Optional. A set of parameters to customize the requests.</param>
        /// <returns></returns>
        public static async Task<List<return_type>> GetAllPagesAsync<return_type, page_return_type, paging_parameters_class>(Func<Authentication, string, string, string, paging_parameters_class, Task<page_return_type>> GetPage, Authentication authentication, string token, string param_1, string param_2,  paging_parameters_class parameters)
        where paging_parameters_class : new()
        {
            List<return_type> result = new List<return_type>();

            bool requesting = true;
            do
            {
                // request the page
                page_return_type page = await GetPage(authentication, token, param_1, param_2, parameters);
                PropertyInfo page_data = page.GetType().GetProperty("data");

                List<return_type> page_data_value = (List<return_type>)page_data.GetValue(page);
                foreach (return_type element in page_data_value)
                {
                    result.Add(element);
                }

                // check to see if there is a new page to request
                PropertyInfo page_pagination = page.GetType().GetProperty("pagination");
                Pagination pagination = (Pagination)page_pagination.GetValue(page);

                requesting = pagination.cursor.isValid();
                if(requesting)
                {
                    // TODO: (PagingUtil.GetAllPagesAsync) - Fix invalid cursor
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
