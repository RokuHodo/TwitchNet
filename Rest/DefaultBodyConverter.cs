// standard namespaces
using System;

// project namespaces
using TwitchNet.Helpers.Json;

// imported .dll's
using RestSharp;

namespace
TwitchNet.Rest
{
    /// <summary>
    /// The default converter for request body parameters.
    /// Adds the value of serializable object as a Json body to a <see cref="RestRequest"/>.
    /// </summary>
    public class DefaultBodyConverter : RestParameterConverter
    {
        /// <summary>
        /// Adds the value of serializable object as a Json body to a <see cref="RestRequest"/>.
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
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = new JsonSerializer();
            request.AddBody(parameter.Value);

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
        /// Returns true if the <see cref="Parameter.Type"/> is equal to <see cref="ParameterType.RequestBody"/>.
        /// Returns false otherwise.
        /// </returns>
        public override bool
        CanConvert(Parameter parameter, Type member_type)
        {
            return parameter.Type == ParameterType.RequestBody;
        }
    }
}
