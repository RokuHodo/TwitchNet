﻿// standard namespaces
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;

// project namespaces
using TwitchNet.Extensions;
using TwitchNet.Helpers.Json;
using TwitchNet.Utilities;

namespace TwitchNet.Rest
{
    public enum
    Method
    {
        GET,

        POST,

        PUT,
    }

    public class
    RestRequest : IDisposable
    {
        public bool disposed { get; private set; } = true;

        public string endpoint;

        public Method method { get; set; }

        public List<QueryParameter> query_parameters { get; private set; }

        public HttpRequestMessage message { get; private set; }

        public ISerializer json_serialzier { get; set; }

        public RequestSettings settings { get; set; }

        public RestRequest(RequestSettings settings = default)
        {
            json_serialzier = new JsonSerializer();

            message = new HttpRequestMessage();

            query_parameters = new List<QueryParameter>();

            this.settings = settings.IsNull() ? new RequestSettings() : settings;
        }

        public RestRequest(Method method, RequestSettings settings = default) : this(settings)
        {
            this.method = method;

            disposed = false;
        }

        public RestRequest(string endpoint, Method method, RequestSettings settings = default) : this(method, settings)
        {
            this.endpoint = endpoint;
        }

        public void
        Dispose()
        {
            if (disposed)
            {
                return;
            }

            message.Dispose();
            message = null;

            disposed = true;
        }

        public HttpRequestMessage
        BuildMessage(Uri client_uri)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(message));
            }

            if (client_uri.IsNull())
            {
                throw new ArgumentNullException(nameof(client_uri));
            }

            message.RequestUri = BuildUri(client_uri);
            message.Method = new HttpMethod(EnumUtil.GetName(method));

            return message;
        }

        public HttpRequestMessage
        CloneMessage()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(message));
            }

            HttpRequestMessage clone = new HttpRequestMessage(message.Method, message.RequestUri);
            clone.Content = message.Content;

            foreach(KeyValuePair<string, IEnumerable<string>> header in message.Headers)
            {
                clone.Headers.Add(header.Key, header.Value);
            }

            message = clone;

            return message;
        }

        private Uri
        BuildUri(Uri client_uri)
        {
            if (client_uri.IsNull())
            {
                return default;
            }

            UriBuilder builder = new UriBuilder(client_uri);

            // ------------------------------------------
            // Append the endpoint (resource) to the URL
            // ------------------------------------------

            if (endpoint.IsValid())
            {
                builder.Path += '/' + endpoint;
            }

            // ------------------------------------------
            // Append any query parameters to the URL
            // ------------------------------------------

            if (query_parameters.IsValid())
            {
                // While UriBuilder.Uri will encode the URI for us, it does not encode the characters '&', '=', or '+' since they are URL delineators and must remain unencoded.
                // There are certain instances where the name or value of a query parameter *might* include these. We must encode these corner cases before hand.
                // If we don't encode these corner cases, the receiving parser will see these query parameters as invalid and fail to parse them correctly.
                if (query_parameters.Count == 1)
                {
                    string value = query_parameters[0].value.IsNull() ? string.Empty : query_parameters[0].value;
                    builder.Query = EncodeSpecialCharacters(query_parameters[0].name) + "=" + EncodeSpecialCharacters(value);
                }
                else
                {
                    List<string> concat = new List<string>();

                    foreach (QueryParameter parameter in query_parameters)
                    {
                        string value = parameter.value.IsNull() ? string.Empty : parameter.value;

                        concat.Add(EncodeSpecialCharacters(parameter.name) + "=" + EncodeSpecialCharacters(value));
                    }

                    builder.Query = string.Join("&", concat);
                }
            }

            return builder.Uri;
        }

        private string EncodeSpecialCharacters(string str)
        {
            // TODO: This is incredibly hacky. Fine a more proper way to encode special characters.
            return str.Replace("&", "%26").Replace("=", "%3D").Replace("+", "%2B");
        }

        public bool
        AddQueryParameter(string name, string value)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(message));
            }

            // Disallow null values but still allow empty values.
            if (!name.IsValid() || value.IsNull())
            {
                return false;
            }

            QueryParameter parameter = new QueryParameter();
            parameter.name = name;
            parameter.value = value;

            query_parameters.Add(parameter);

            return true;
        }

        public bool
        RemoveQueryParameter(string name)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(message));
            }

            // There might be a time where an empty or null value *might* be desired.
            // Don't check for it here. Leave that up to the the user.
            if (!name.IsValid())
            {
                return false;
            }

            int count = query_parameters.RemoveAll(query => query.name == name);

            return count > 0;
        }

        public void
        ClearQueryParameters()
        {
            if (query_parameters.IsNull())
            {
                return;
            }

            query_parameters.Clear();
        }

        public void
        AddParameters(object parameters)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(message));
            }

            if (parameters.IsNull())
            {
                return;
            }

            BodyAttribute body = parameters.GetType().GetAttribute<BodyAttribute>();
            if (!body.IsNull())
            {
                body.reflected_type = parameters.GetType().GetTrueType();
                if (body.reflected_type.IsNull())
                {
                    return;
                }

                RestParameterConverter converter = RestConverterCache.GetOradd(body.converter);
                if (converter.IsNull())
                {
                    return;
                }

                RestParameter parameter = new RestParameter();
                parameter.name = body.name;
                parameter.root_name = body.root_name;
                parameter.value = parameters;
                parameter.parameter_type = body.parameter_type;
                parameter.content_type = body.content_type;
                parameter.member_type = body.reflected_type;

                if (!converter.CanConvert(parameter))
                {
                    return;
                }

                converter.AddParameter(this, parameter);

                return;
            }

            MemberInfo[] members = parameters.GetType().GetMembers<RestParameterAttribute>();
            if (!members.IsValid())
            {
                return;
            }

            foreach (MemberInfo member in members)
            {
                if (member.IsNull())
                {
                    continue;
                }

                RestParameterAttribute attribute;

                PropertyInfo    property    = member as PropertyInfo;
                FieldInfo       field       = member as FieldInfo;
                if (!property.IsNull())
                {
                    attribute                   = property.GetAttribute<RestParameterAttribute>();
                    attribute.reflected_type    = property.PropertyType;
                    attribute.value             = property.GetValue(parameters);
                }
                else if (!field.IsNull())
                {
                    attribute                   = field.GetAttribute<RestParameterAttribute>();
                    attribute.reflected_type    = field.FieldType;
                    attribute.value             = field.GetValue(parameters);
                }
                else
                {
                    continue;
                }

                attribute.reflected_type = attribute.reflected_type.GetTrueType();
                if (attribute.reflected_type.IsNull())
                {
                    continue;
                }

                RestParameterConverter converter = RestConverterCache.GetOradd(attribute.converter);
                if (converter.IsNull())
                {
                    continue;
                }

                RestParameter parameter = new RestParameter();
                parameter.name              = attribute.name;
                parameter.root_name         = attribute.root_name;
                parameter.value             = attribute.value;
                parameter.parameter_type    = attribute.parameter_type;
                parameter.content_type      = attribute.content_type;
                parameter.member_type       = attribute.reflected_type;

                if (!converter.CanConvert(parameter))
                {
                    continue;
                }

                converter.AddParameter(this, parameter);
            }
        }        

        public bool
        AddHeader(string name, string value)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(message));
            }

            // There might be a time where an empty or null value *might* be desired.
            // Don't check for it here. Leave that up to the the user.
            if (!name.IsValid())
            {
                return false;
            }

            message.Headers.Add(name, value);

            return true;
        }

        public bool
        RemoveHeader(string name)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(message));
            }

            if (!name.IsValid())
            {
                return false;
            }

            return message.Headers.Remove(name);
        }

        public bool
        RemoveHeaders(string name)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(message));
            }

            if (!name.IsValid())
            {
                return false;
            }

            if (!message.Headers.TryGetValues(name, out IEnumerable<string> values))
            {
                return false;
            }

            // We actually don't care about each value.
            // This is just a small hack to avoid using values.Count() via Linq.
            foreach (string value in values)
            {
                message.Headers.Remove(name);
            }

            return true;
        }

        public bool
        ClearHeaders()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(message));
            }

            message.Headers.Clear();

            return true;
        }

        public void
        SetBody(object obj, string root_name = null)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(message));
            }

            // This will only ever happen if the user intentionally sets this to null.
            if (json_serialzier.IsNull())
            {
                json_serialzier = new JsonSerializer();
            }

            string content = json_serialzier.Serialize(obj);
            if (root_name.IsValid())
            {
                // This is what i'll do until proven this is unstable/breaks under certain situations.
                content = "{\"" + root_name + "\":" + content + "}";
            }

            message.Content = new StringContent(content, Encoding.UTF8, json_serialzier.content_type);
        }

        public void
        ClearBody()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(message));
            }

            message.Content = default;
        }
    }

    public class
    QueryParameter
    {
        /// <summary>
        /// The name of the query parameter.
        /// </summary>
        public string name;

        /// <summary>
        /// The value of the query parameter.
        /// </summary>
        public string value;
    }

    public class
    RestParameter
    {
        /// <summary>
        /// The name of the rest parameter.
        /// </summary>
        public string name;

        /// <summary>
        /// <para>The name of a parent JSON object to wrap the body in.</para>
        /// <para>This is to avoid having to manually nest the body in another object and reduce verbosity.</para>
        /// </summary>
        public string root_name;

        /// <summary>
        /// The value of the rest parameter.
        /// </summary>
        public object value;

        /// <summary>
        /// The content type.
        /// </summary>
        public string content_type;

        /// <summary>
        /// The type of rest parameter.
        /// </summary>
        public HttpParameterType parameter_type;

        /// <summary>
        /// <para>
        /// The reflected type of the rest parameter member.
        /// The member type will never be null.
        /// </para>
        /// <para>If the reflected type is nullable, this type is the underlying type.</para>
        /// </summary>
        public Type member_type;
    }
}