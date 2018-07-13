// standard namespaces
using System;

// imported .dll's
using RestSharp;

namespace TwitchNet.Rest
{
    public abstract class
    RestParameterConverter
    {
        /// <summary>
        /// Converts the value of a member marked with <see cref="RestParameterAttribute"/> and adds it to the <see cref="RestRequest"/>.
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
        public abstract RestRequest
        AddParameter(RestRequest request, Parameter paramerter, Type member_type);

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
        /// Returns true of the member can be converted.
        /// Returns false otherwise.
        /// </returns>
        public abstract bool
        CanConvert(Parameter paramerter, Type member_type);
    }
}
