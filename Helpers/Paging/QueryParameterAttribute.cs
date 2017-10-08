// standard namespaces
using System;

// project namespaces
using TwitchNet.Enums.Helpers.Paging;

namespace TwitchNet.Helpers.Paging
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    internal sealed class QueryParameterAttribute : Attribute
    {
        /// <summary>
        /// Whether or not to make the query parameter value lower case.
        /// </summary>
        public bool                 to_lower            { get; set; }

        /// <summary>
        /// The name of the query parameter.
        /// </summary>
        public string               query_name          { get; set; }

        /// <summary>
        /// How to handle undersfores when adding the value as a query parameter.
        /// </summary>
        public UnderscoreHandling   underscore_handling { get; set; }

        public QueryParameterAttribute(string query_name, bool to_lower = true)
        {
            this.query_name = query_name;
            this.to_lower = to_lower;
        }

        public QueryParameterAttribute(string query_name, UnderscoreHandling underscore_handling, bool to_lower = true)
        {
            this.query_name = query_name;
            this.to_lower = to_lower;

            this.underscore_handling = underscore_handling;
        }
    }
}
