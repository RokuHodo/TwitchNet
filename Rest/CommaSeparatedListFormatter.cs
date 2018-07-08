// standard namespaces
using System;
using System.Collections;

// project namespaces
using TwitchNet.Extensions;

// imported .dll's
using RestSharp;

namespace TwitchNet.Rest
{
    /// <summary>
    /// Formats the elements of a collection into a comma separated string and added to a <see cref="RestRequest"/> as a single query parameter.
    /// </summary>
    public class CommaSeparatedListFormatter : QueryParameterFormatter
    {
        /// <summary>
        /// Formats the value of a member marked with <see cref="QueryParameterAttribute"/> and adds it to the <see cref="RestRequest"/>.
        /// </summary>
        /// <param name="request">The request to be executed.</param>
        /// <param name="name">The name of the qyery parameter.</param>
        /// <param name="type">The type of the member marked with <see cref="QueryParameterAttribute"/>.</param>
        /// <param name="value">The value of the member marked with <see cref="QueryParameterAttribute"/>.</param>
        /// <returns>Returns the rest request with added with the formatted query parameters.</returns>
        public override RestRequest
        FormatAddValue(RestRequest request, string name, Type type, object list)
        {
            IList _list = list as IList;
            if (_list.Count == 0)
            {
                return request;
            }

            // This assumes the array is single dimensional and that the underlying type of each array/list is a value type that can be printed. 
            string result = string.Join(",", list);
            request.AddQueryParameter(name, result);

            return request;
        }

        /// <summary>
        /// Determines if the object can be formatted.
        /// </summary>
        /// <param name="type">The type of the member marked with <see cref="QueryParameterAttribute"/>.</param>
        /// <returns>
        /// Returns true of the object is a list or an array.
        /// Returns false otherwise.
        /// </returns>
        public override bool
        CanFormat(Type type)
        {
            return type.IsArray || type.IsList();
        }
    }
}