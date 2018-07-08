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
        /// The name of the query parameter.
        /// </summary>
        public string               name        { get; private set; }

        /// <summary>
        /// The function used to convert the reflected value into a string.
        /// </summary>
        public Type       formatter   { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="QueryParameterAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the query parameter.</param>
        public QueryParameterAttribute(string name)
        {
            this.name = name;
        }

        /// <param name="type">The type of query parameter, and how to add it to the request.</param>
        public QueryParameterAttribute(string name, Type formatter) : this(name)
        {
            this.formatter = formatter;
        }
    }
}
