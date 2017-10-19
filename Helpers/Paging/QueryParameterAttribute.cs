// standard namespaces
using System;

namespace TwitchNet.Helpers.Paging
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    internal sealed class
    QueryParameterAttribute : Attribute
    {
        /// <summary>
        /// Whether or not to make the query parameter value lower case.
        /// </summary>
        public bool                 to_lower            { get; set; }

        /// <summary>
        /// The name of the query parameter.
        /// </summary>
        public string               query_name          { get; set; }

        public QueryParameterAttribute(string query_name, bool to_lower = true)
        {
            this.query_name = query_name;
            this.to_lower = to_lower;
        }
    }
}
