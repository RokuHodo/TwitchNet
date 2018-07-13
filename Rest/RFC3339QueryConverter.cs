// standard namespaces
using System;
using System.Globalization;

// imported .dll's
using RestSharp;

namespace TwitchNet.Rest
{
    /// <summary>
    /// Converts a <see cref="DateTime"/> value to a RFC 3339 compliant string and adds it to the <see cref="RestRequest"/> as a query parameter.
    /// </summary>
    public class
    RFC3339QueryConverter : RestParameterConverter
    {
        public override RestRequest
        AddParameter(RestRequest request, Parameter parameter, Type member_type)
        {
            string rfc_3339 = ((DateTime)parameter.Value).ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", DateTimeFormatInfo.InvariantInfo);

            request.AddQueryParameter(parameter.Name, rfc_3339);

            return request;
        }

        public override bool
        CanConvert(Parameter parameter, Type member_type)
        {
            return parameter.Type == ParameterType.QueryString && member_type == typeof(DateTime);
        }
    }
}
