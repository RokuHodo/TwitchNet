// standard namespaces
using System;

// project namespaces
using TwitchNet.Extensions;

// imported .dll's
using RestSharp;

namespace
TwitchNet.Rest
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class
    QueryParameterAttribute : RestParameterAttribute
    {
        /// <summary>
        /// Creates a new instance of the <see cref="QueryParameterAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the query parameter.</param>
        public QueryParameterAttribute(string name, Type converter = null) : base(name, ParameterType.QueryString)
        {
            base.converter = converter.IsNull() ? typeof(DefaultQueryConverter) : converter;
        }
    }
}
