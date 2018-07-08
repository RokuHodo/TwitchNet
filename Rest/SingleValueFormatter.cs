// standard namespaces
using System;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Utilities;

// imported .dll's
using RestSharp;

namespace TwitchNet.Rest
{
    /// <summary>
    /// Adds the value of an object to a <see cref="RestRequest"/> as a single query parameter.
    /// </summary>
    public class SingleValueFormatter : QueryParameterFormatter
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
        FormatAddValue(RestRequest request, string name, Type type, object value)
        {
            string result = string.Empty;

            if (value.IsNull())
            {
                return request;
            }

            if (type.IsEnum)
            {
                EnumUtil.TryGetName(type, value, out result);
            }
            else
            {
                result = value.ToString();
            }

            if (!result.IsValid())
            {
                return request;
            }

            request.AddQueryParameter(name, result);

            return request;
        }

        /// <summary>
        /// Determines if the object can be formatted.
        /// </summary>
        /// <param name="type">The type of the member marked with <see cref="QueryParameterAttribute"/>.</param>
        /// <returns>
        /// Returns true of the object is an enum, value type, or a string.
        /// Returns false otherwise.
        /// </returns>
        public override bool
        CanFormat(Type type)
        {
            return type.IsEnum || type.IsValueType || type == typeof(string);
        }
    }
}
