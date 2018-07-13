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
    /// The default converter for query string parameters.
    /// Adds value types and strings to the <see cref="RestRequest"/> as query parameters.
    /// </summary>
    public class DefaultQueryConverter : RestParameterConverter
    {
        /// <summary>
        /// Adds value types and strings to the <see cref="RestRequest"/> as query parameters.
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
        public override RestRequest
        AddParameter(RestRequest request, Parameter parameter, Type member_type)
        {
            if (!parameter.Name.IsValid() || parameter.Value.IsNull())
            {
                return request;
            }

            string result = string.Empty;
            if (member_type.IsEnum)
            {
                EnumUtil.TryGetName(member_type, parameter.Value, out result);
            }
            else
            {
                result = parameter.Value.ToString();
            }

            if (!result.IsValid())
            {
                return request;
            }

            request.AddQueryParameter(parameter.Name, result);

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
        /// Returns true if the <see cref="Parameter.Type"/> is equal to <see cref="ParameterType.QueryString"/> and the member is a value type, enum, or string.
        /// Returns false otherwise.
        /// </returns>
        public override bool
        CanConvert(Parameter parameter, Type member_type)
        {
            return parameter.Type == ParameterType.QueryString && (member_type.IsEnum || member_type.IsValueType || member_type == typeof(string));
        }
    }
}
