﻿// standard namespaces
using System;

// imported .dll's
using RestSharp;

namespace TwitchNet.Rest
{
    public abstract class
    QueryParameterFormatter
    {
        /// <summary>
        /// Formats the value of a member marked with <see cref="QueryParameterAttribute"/> and adds it to the <see cref="RestRequest"/>.
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
        public abstract RestRequest
        FormatAndAdd(RestRequest request, string query_name, Type member_type, object member_value);

        /// <summary>
        /// Determines if the member marked with <see cref="QueryParameterAttribute"/> can/should be formatted.
        /// </summary>
        /// <param name="member_type">
        /// <para>The type of the member marked with <see cref="QueryParameterAttribute"/>.</para>
        /// <para>
        /// If the type of the member is nullable, this type is the underlying type.
        /// The member type will never be null and always be instantiated.
        /// </para>
        /// </param>
        /// <returns>
        /// Returns true of the member can be formatted.
        /// Returns false otherwise.
        /// </returns>
        public abstract bool
        CanFormat(Type member_type);
    }
}
