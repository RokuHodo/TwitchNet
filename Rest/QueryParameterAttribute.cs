// standard namespaces
using System;

namespace
TwitchNet.Rest
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    internal sealed class
    QueryParameterAttribute : Attribute
    {
        /// <summary>
        /// Whether or not to make the query parameter value lower case.
        /// </summary>
        public bool                 to_lower    { get; private set; }

        /// <summary>
        /// The name of the query parameter.
        /// </summary>
        public string               name        { get; private set; }

        /// <summary>
        /// The type of query parameter, and how to add it to the request.
        /// </summary>
        public QueryParameterType   type        { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="QueryParameterAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the query parameter.</param>
        public QueryParameterAttribute(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="QueryParameterAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the query parameter.</param>
        /// <param name="type">The type of query parameter, and how to add it to the request.</param>
        public QueryParameterAttribute(string name, QueryParameterType type) : this(name)
        {
            this.type = type;
        }

        /// <summary>
        /// Creates a new blank instance of the <see cref="QueryParameterAttribute"/> class.
        /// </summary>
        public QueryParameterAttribute()
        {

        }
    }
}
