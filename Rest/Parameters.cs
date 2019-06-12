// standard namespaces
using System;

namespace TwitchNet.Rest
{
    public class
    QueryParameter
    {
        /// <summary>
        /// The name of the query parameter.
        /// </summary>
        public string name;

        /// <summary>
        /// The value of the query parameter.
        /// </summary>
        public string value;
    }

    public class
    RestParameter
    {
        /// <summary>
        /// The name of the rest parameter.
        /// </summary>
        public string name;

        /// <summary>
        /// <para>The name of a parent JSON object to wrap the body in.</para>
        /// <para>This is to avoid having to manually nest the body in another object and reduce verbosity.</para>
        /// </summary>
        public string root_name;

        /// <summary>
        /// The value of the rest parameter.
        /// </summary>
        public object value;

        /// <summary>
        /// The content type.
        /// </summary>
        public string content_type;

        /// <summary>
        /// The type of rest parameter.
        /// </summary>
        public HttpParameterType parameter_type;

        /// <summary>
        /// <para>
        /// The reflected type of the rest parameter member.
        /// The member type will never be null.
        /// </para>
        /// <para>If the reflected type is nullable, this type is the underlying type.</para>
        /// </summary>
        public Type member_type;
    }
}
