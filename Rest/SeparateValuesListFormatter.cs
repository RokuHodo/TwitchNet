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
    /// Adds the elements of a collection to a <see cref="RestRequest"/> as separate query parameters.
    /// </summary>
    public class SeparateValuesListFormatter : QueryParameterFormatter
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

            if (type.IsEnum)
            {
                // TODO: Finish implementing this in here and in other converters.
                EnumUtil.TryGetFlagNames(type, value, out string[] _result);
            }
            else
            {
                IList _list = value as IList;
                if (_list.Count == 0)
                {
                    return request;
                }

                foreach (object element in _list)
                {
                    if (element.IsNull())
                    {
                        continue;
                    }

                    // TODO: Handle cases for Enum arrays/lists?
                    string _value = element.ToString();
                    if (!_value.IsValid())
                    {
                        continue;
                    }

                    request.AddQueryParameter(name, _value);
                }
            }            

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
        public override bool CanFormat(Type type)
        {
            return type.IsArray || type.IsList() || type.IsEnum;
        }
    }
}
