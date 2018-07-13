// standard namespaces
using System;

using RestSharp;

namespace
TwitchNet.Rest
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class
    RestParameterAttribute : Attribute
    {
        /// <summary>
        /// The MIME content type of the parameter.
        /// </summary>
        public string           content_type { get; protected set; }

        /// <summary>
        /// The name of the parameter.
        /// </summary>
        public string           name        { get; protected set; }

        /// <summary>
        /// The type of the parameter.
        /// </summary>
        public ParameterType    type        { get; protected set; }

        /// <summary>
        /// The type of the method used to convert and add the reflected parameter value.
        /// </summary>
        public Type             converter   { get; protected set; }

        public RestParameterAttribute(Type converter = null)
        {
            this.converter = converter;
        }

        public RestParameterAttribute(ParameterType type, Type converter = null)
        {
            this.type = type;

            this.converter = converter;
        }

        public RestParameterAttribute(string name, ParameterType type, Type converter = null)
        {
            this.name = name;

            this.type = type;

            this.converter = converter;
        }

        public RestParameterAttribute(string name, string content_type, ParameterType type, Type converter = null)
        {
            this.name = name;
            this.content_type = content_type;

            this.type = type;

            this.converter = converter;
        }
    }
}
