// standard namespaces
using System;

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
        public QueryParameterAttribute(string name, Type converter = null) : base(name, HttpParameterType.Query)
        {
            base.converter = converter ?? typeof(DefaultQueryConverter);
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = false)]
    public sealed class
    BodyAttribute : RestParameterAttribute
    {
        public BodyAttribute(Type converter = null) : base(HttpParameterType.Body)
        {
            this.converter = converter ?? typeof(DefaultBodyConverter);
        }

        public BodyAttribute(string root_name, Type converter = null) : base(HttpParameterType.Body)
        {
            this.root_name = root_name;
            this.converter = converter ?? typeof(DefaultBodyConverter);
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class
    RestParameterAttribute : Attribute
    {
        public string name { get; internal set; }

        public string root_name { get; internal set; }

        public object value { get; internal set; }

        public string content_type { get; internal set; }

        public HttpParameterType parameter_type { get; internal set; }

        public Type converter { get; internal set; }

        public Type reflected_type { get; internal set; }

        public RestParameterAttribute(Type converter = null)
        {
            this.converter = converter;
        }

        public RestParameterAttribute(HttpParameterType type, Type converter = null)
        {
            this.parameter_type = type;

            this.converter = converter;
        }

        public RestParameterAttribute(string name, HttpParameterType type, Type converter = null)
        {
            this.name = name;

            this.parameter_type = type;

            this.converter = converter;
        }

        public RestParameterAttribute(string name, string content_type, HttpParameterType type, Type converter = null)
        {
            this.name = name;
            this.content_type = content_type;

            this.parameter_type = type;

            this.converter = converter;
        }
    }

    public enum
    HttpParameterType
    {
        Query,

        Body,

        Header
    }
}
