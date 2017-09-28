// standard namespaces
using System;
using System.Reflection;

namespace TwitchNet.Helpers.Paging
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    internal sealed class PagingPropertyAttribute : Attribute
    {
        public string       query_name      { get; set; }
        public PropertyInfo property        { get; set; }

        public PagingPropertyAttribute(string name)
        {
            query_name = name;
        }
    }
}
