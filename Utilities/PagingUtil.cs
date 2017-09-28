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
        public static RestRequest AddPaging<paging_parameters_class>(RestRequest request, paging_parameters_class paging_parameters)
        where paging_parameters_class : new()
        {
            if (paging_parameters.isNull())
            {
                paging_parameters = new paging_parameters_class();
            }

            List<PagingPropertyAttribute> attributes = GetPagingAttributes(paging_parameters);

            foreach (PagingPropertyAttribute attribute in attributes)
            {
                object property_object = attribute.property.GetValue(paging_parameters);
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

        private static List<PagingPropertyAttribute> GetPagingAttributes<paging_parameters_class>(paging_parameters_class paging_parameters)
        where paging_parameters_class : new()
        {
            List<PagingPropertyAttribute> attributes = new List<PagingPropertyAttribute>();

            PropertyInfo[] properties = paging_parameters.GetType().GetProperties();
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

        public static async Task<List<return_type>> GetAllPagesAsync<return_type, page_return_type, paging_parameters>(Func<Authentication, string, paging_parameters, Task<page_return_type>> GetPage, Authentication authentication, string token, paging_parameters parameters)
        where paging_parameters : new()
        {
            List<return_type> result = new List<return_type>();

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
                if (pagination.cursor.isValid())
                {
                    // TODO: (PagingUtil.GetAllPagesAsync) - Fix invalid cursor
                    // update the parameter's 'after' property to properly request the next page
                    PropertyInfo parameters_after = parameters.GetType().GetProperty("after");
                    parameters_after.SetValue(parameters, pagination.cursor);
                }
                else
                {
                    requesting = false;
                }
            }
            while (requesting);

            return result;
        }
    }
}
