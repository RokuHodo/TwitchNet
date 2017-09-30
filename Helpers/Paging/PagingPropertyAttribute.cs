// standard namespaces
using System;
using System.Reflection;

namespace TwitchNet.Helpers.Paging
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    internal sealed class PagingPropertyAttribute : Attribute
    {
        /// <summary>
        /// The name of the query parameter.
        /// </summary>
        public string       query_name      { get; set; }

        /// <summary>
        /// The underlying property type of the parameter.
        /// </summary>
        public PropertyInfo property        { get; set; }

        public PagingPropertyAttribute(string name)
        {
            query_name = name;
        }
    }
}
