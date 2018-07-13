// standard namespaces
using System;

using TwitchNet.Helpers.Json;

// imported .dll's
using RestSharp;

namespace TwitchNet.Rest
{
    /// <summary>
    /// The default converter for request body parameters.
    /// Adds the value of serializable object as a Json body to a <see cref="RestRequest"/>.
    /// </summary>
    public class DefaultBodyConverter : RestParameterConverter
    {
        public override RestRequest
        AddParameter(RestRequest request, Parameter parameter, Type member_type)
        {
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = new JsonSerializer();
            request.AddBody(parameter.Value);

            return request;
        }

        public override bool
        CanConvert(Parameter parameter, Type member_type)
        {
            return parameter.Type == ParameterType.RequestBody;
        }
    }
}
