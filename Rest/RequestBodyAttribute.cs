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
    RequestBodyAttribute : RestParameterAttribute
    {
        public RequestBodyAttribute(Type converter = null) : base(ParameterType.RequestBody)
        {
            this.converter = converter.IsNull() ? typeof(DefaultBodyConverter) : converter;
        }
    }
}
