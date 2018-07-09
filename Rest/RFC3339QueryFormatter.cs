// standard namespaces
using System;
using System.Globalization;

// imported .dll's
using RestSharp;

namespace TwitchNet.Rest
{
    /// <summary>
    /// Converts a <see cref="DateTime"/> value to a RFC 3339 compliant string and adds it to the <see cref="RestRequest"/> as a query parameter.
    /// </summary>
    public class
    RFC3339QueryFormatter : QueryParameterFormatter
    {
        /// <summary>
        /// Converts the DateTime value a RFC 3339 compliant string and adds it to the <see cref="RestRequest"/> as query parameter.
        /// </summary>
        /// <param name="request">
        /// <para>The request to be executed.</para>
        /// <para>This request will never be null and will always be instantiated.</para>
        /// </param>
        /// <param name="query_name">
        /// <para>The name of the qyery parameter obtained from the <see cref="QueryParameterAttribute.name"/>.</para>
        /// <para>The name will never be null, empty, or only contain whitespace and will always contain at least one character.</para>
        /// </param>
        /// <param name="member_type">
        /// <para>The type of the member marked with <see cref="QueryParameterAttribute"/>.</para>
        /// <para>
        /// If the type of the member is nullable, this type is the underlying type.
        /// The member type will never be null and always be instantiated.
        /// </para>
        /// </param>
        /// <param name="member_value">
        /// <para>The value of the member marked with <see cref="QueryParameterAttribute"/>.</para>
        /// <para>This member value will never be null and will always be instantiated.</para>
        /// </param>
        /// <returns>Returns the rest request.</returns>
        public override RestRequest
        FormatAndAdd(RestRequest request, string query_name, Type member_type, object member_value)
        {
            string rfc_3339 = ((DateTime)member_value).ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", DateTimeFormatInfo.InvariantInfo);

            request.AddQueryParameter(query_name, rfc_3339);

            return request;
        }

        /// <summary>
        /// Determines if the member marked with <see cref="QueryParameterAttribute"/> can/should be formatted using this formatter.
        /// </summary>
        /// <param name="member_type">
        /// <para>The type of the member marked with <see cref="QueryParameterAttribute"/>.</para>
        /// <para>
        /// If the type of the member is nullable, this type is the underlying type.
        /// The member type will never be null and always be instantiated.
        /// </para>
        /// </param>
        /// <returns>
        /// Returns true of the member is a DateTime.
        /// Returns false otherwise.
        /// </returns>
        public override bool
        CanFormat(Type member_type)
        {
            return member_type == typeof(DateTime);
        }
    }
}
