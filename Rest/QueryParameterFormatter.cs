// standard namespaces
using System;

// imported .dll's
using RestSharp;

namespace TwitchNet.Rest
{
    public abstract class
    QueryParameterFormatter
    {
        /// <summary>
        /// Determines if the object can be formatted.
        /// </summary>
        /// <param name="type">The type of the member marked with <see cref="QueryParameterAttribute"/>.</param>
        /// <returns>
        /// Returns true of the object can be formatted.
        /// Returns false otherwise.
        /// </returns>
        public abstract bool
        CanFormat(Type type);

        /// <summary>
        /// Formats the value of a member marked with <see cref="QueryParameterAttribute"/> and adds it to the <see cref="RestRequest"/>.
        /// </summary>
        /// <param name="request">The request to be executed.</param>
        /// <param name="name">The name of the qyery parameter.</param>
        /// <param name="type">The type of the member marked with <see cref="QueryParameterAttribute"/>.</param>
        /// <param name="value">The value of the member marked with <see cref="QueryParameterAttribute"/>.</param>
        /// <returns>Returns the rest request with added with the formatted query parameters.</returns>
        public abstract RestRequest
        FormatAddValue(RestRequest request, string name, Type type, object value);
    }
}
