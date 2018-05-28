// standard namespaces
using System;

namespace
TwitchNet.Rest
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    internal sealed class
    QueryParameterAttribute : Attribute
    {
        /// <summary>
        /// Whether or not to make the query parameter value lower case.
        /// </summary>
        public bool     to_lower    { get; private set; }

        /// <summary>
        /// The name of the query parameter.
        /// </summary>
        public string   name        { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="QueryParameterAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the query parameter.</param>
        public QueryParameterAttribute(string name) : this(name, false)
        {
            this.name = name;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="QueryParameterAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the query parameter.</param>
        /// <param name="to_lower">Whether or not to make the name lower case.</param>
        public QueryParameterAttribute(string name, bool to_lower)
        {
            this.name = name;
            this.to_lower = to_lower;
        }
    }
}
