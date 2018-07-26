// standard namespaces
using System;
using System.Globalization;

// project namespaces
using TwitchNet.Extensions;

// imported .dll's
using RestSharp;

namespace
TwitchNet.Rest
{
    /// <summary>
    /// Converts a <see cref="DateTime"/> value to a RFC 3339 compliant string and adds it to the <see cref="RestRequest"/> as a query parameter.
    /// </summary>
    public class
    RFC3339QueryConverter : RestParameterConverter
    {
        /// <summary>
        /// Converts a <see cref="DateTime"/> value to a RFC 3339 compliant string and adds it to the <see cref="RestRequest"/> as a query parameter.
        /// </summary>
        /// <param name="request">
        /// <para>The request to be executed.</para>
        /// <para>The request will never be null and will always be instantiated.</para>
        /// </param>
        /// <param name="paramerter">The rest parameter to be converted.</param>
        /// <param name="member_type">
        /// <para>The type of the member marked with <see cref="RestParameterConverter"/>.</para>
        /// <para>
        /// If the type of the member is nullable, this type is the underlying type.
        /// The member type will never be null and always be instantiated.
        /// </para>
        /// </param>
        /// <returns>Returns the rest request to be executed.</returns>
        public override IRestRequest
        AddParameter(IRestRequest request, Parameter parameter, Type member_type)
        {
            if (!parameter.Name.IsValid() || parameter.Value.IsNull())
            {
                return request;
            }

            string rfc_3339 = ((DateTime)parameter.Value).ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", DateTimeFormatInfo.InvariantInfo);

            request.AddQueryParameter(parameter.Name, rfc_3339);

            return request;
        }

        /// <summary>
        /// Determines if the member marked with <see cref="RestParameterConverter"/> can be converted.
        /// </summary>
        /// <param name="paramerter">The rest parameter to be converted.</param>
        /// <param name="member_type">
        /// <para>The type of the member marked with <see cref="RestParameterConverter"/>.</para>
        /// <para>
        /// If the type of the member is nullable, this type is the underlying type.
        /// The member type will never be null and always be instantiated.
        /// </para>
        /// </param>
        /// <returns>
        /// Returns true if the <see cref="Parameter.Type"/> is equal to <see cref="ParameterType.QueryString"/> and the member is a <see cref="DateTime"/> value.
        /// Returns false otherwise.
        /// </returns>
        public override bool
        CanConvert(Parameter parameter, Type member_type)
        {
            return parameter.Type == ParameterType.QueryString && member_type == typeof(DateTime);
        }
    }
}
