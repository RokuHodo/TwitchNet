// standard namespaces
using System;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Utilities;

// imported .dll's
using RestSharp;

namespace TwitchNet.Rest
{
    /// <summary>
    /// The default converter for query string parameters.
    /// Adds value types and strings to the <see cref="RestRequest"/> as a single query parameters.
    /// </summary>
    public class DefaultQueryConverter : RestParameterConverter
    {
        public override RestRequest
        AddParameter(RestRequest request, Parameter parameter, Type member_type)
        {
            string result = string.Empty;
            if (member_type.IsEnum)
            {
                EnumUtil.TryGetName(member_type, parameter.Value, out result);
            }
            else
            {
                result = parameter.Value.ToString();
            }

            if (!result.IsValid())
            {
                return request;
            }

            request.AddQueryParameter(parameter.Name, result);

            return request;
        }

        public override bool
        CanConvert(Parameter parameter, Type member_type)
        {
            return parameter.Type == ParameterType.QueryString && (member_type.IsEnum || member_type.IsValueType || member_type == typeof(string));
        }
    }
}
