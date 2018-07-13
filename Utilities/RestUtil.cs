// standard namespaces
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Rest;
using TwitchNet.Rest.Api;
using TwitchNet.Extensions;

// imported .dll's
using Newtonsoft.Json;

using RestSharp;

namespace
TwitchNet.Utilities
{
    internal static class
    RestUtil
    {
        private static readonly ConcurrentDictionary<Type, RestParameterConverter> QUERY_FORMATTER_CACHE = new ConcurrentDictionary<Type, RestParameterConverter>();

        public static async Task<Tuple<IRestResponse, RestException, RateLimit>>
        ExecuteAsync(ClientInfo client_info, IRestRequest request, RequestSettings settings)
        {
            RestClient client = CreateClient(client_info);
            IRestResponse response = await client.ExecuteTaskAsync(request, settings.cancelation_token);

            Tuple<IRestResponse, RestException, RateLimit> tuple = await HandleResponse(client_info, response, settings);

            return tuple;
        }

        public static async Task<Tuple<IRestResponse<result_type>, RestException, RateLimit>>
        ExecuteAsync<result_type>(ClientInfo client_info, IRestRequest request, RequestSettings settings)
        {
            RestClient client = CreateClient(client_info);
            IRestResponse<result_type> response = await client.ExecuteTaskAsync<result_type>(request, settings.cancelation_token);

            Tuple<IRestResponse<result_type>, RestException, RateLimit> tuple = await HandleResponse(client_info, response, settings);

            return tuple;
        }

        // NOTE: Whenever a request needs to be delayed or retried, the total number elements is off by 1*n where n is number of retries/pauses. Why?
        public static async Task<Tuple<IRestResponse<result_type>, RestException, RateLimit>>
        TraceExecuteAsync<data_type, result_type>(ClientInfo client_info, IRestRequest request, IHelixQueryParameters parameters, RequestSettings settings)
        where result_type : DataPage<data_type>, IDataPage<data_type>, new()
        {
            IRestResponse<result_type> response = new RestResponse<result_type>();

            result_type total_data = new result_type();
            total_data.data = new List<data_type>();

            List<data_type> data = new List<data_type>();

            RestException exception = new RestException();
            RateLimit rate_limit = new RateLimit();

            if (parameters.IsNull())
            {
                parameters = new HelixQueryParameters();
            }

            if (settings.IsNull())
            {
                settings = RequestSettings.Default;
            }

            bool requesting = true;
            do
            {
                Tuple<IRestResponse<result_type>, RestException, RateLimit> tuple = await ExecuteAsync<result_type>(client_info, request, settings);
                foreach(data_type element in tuple.Item1.Data.data)
                {
                    data.Add(element);
                }

                exception = tuple.Item2;
                rate_limit = tuple.Item3;

                requesting = tuple.Item1.Data.data.IsValid() && tuple.Item1.Data.pagination.cursor.IsValid();
                if (requesting)
                {
                    // NOTE: This is a temporary fix and will only work with Helix.
                    request = request.AddOrUpdateParameter("after", tuple.Item1.Data.pagination.cursor, ParameterType.QueryString);
                }
                else
                {
                    total_data = tuple.Item1.Data;
                    total_data.data = data;

                    response = tuple.Item1;
                    response.Data = total_data;
                }
            }
            while (requesting);

            return Tuple.Create(response, exception, rate_limit);
        }

        #region Paging  

        public static RestRequest
        AddPaging(this RestRequest request, object parameters)
        {
            if (request.IsNull() || parameters.IsNull())
            {
                return request;
            }

            MemberInfo[] members = parameters.GetType().GetMembers<RestParameterAttribute>();
            if (!members.IsValid())
            {
                return request;
            }

            foreach (MemberInfo member in members)
            {
                if (member.IsNull())
                {
                    continue;
                }

                RestParameterAttribute attribute;               

                Type    member_type;
                object  member_value;

                PropertyInfo    property    = member as PropertyInfo;
                FieldInfo       field       = member as FieldInfo;
                if (!property.IsNull())
                {
                    attribute       = property.GetAttribute<RestParameterAttribute>();

                    member_type     = property.PropertyType;
                    member_value    = property.GetValue(parameters);
                }
                else if (!field.IsNull())
                {
                    attribute       = field.GetAttribute<RestParameterAttribute>();

                    member_type     = field.FieldType;
                    member_value    = field.GetValue(parameters);
                }
                else
                {
                    continue;
                }

                // There might actually be times where value = null might be intentional. Allow it.
                // IsAssignableFrom also takes care of null so we don't need to explicitly check for that
                if (!attribute.name.IsValid() || !typeof(RestParameterConverter).IsAssignableFrom(attribute.converter))
                {
                    continue;
                }

                member_type = member_type.IsNullable() ? Nullable.GetUnderlyingType(member_type) : member_type;
                if (member_type.IsNull())
                {
                    continue;
                }

                Parameter parameter     = new Parameter();
                parameter.Name          = attribute.name;
                parameter.Type          = attribute.type;
                parameter.ContentType   = attribute.content_type;
                parameter.Value         = member_value;

                RestParameterConverter converter = QUERY_FORMATTER_CACHE.GetOrAdd(attribute.converter, AddRestConverter);
                if (!converter.CanConvert(parameter, member_type))
                {
                    continue;
                }

                request = converter.AddParameter(request, parameter, member_type);
            }

            return request;
        }

        private static RestParameterConverter
        AddRestConverter(Type type)
        {
            return (RestParameterConverter)Activator.CreateInstance(type);
        }

        #endregion

        #region Response handling

        private static async Task<Tuple<IRestResponse, RestException, RateLimit>>
        HandleResponse(ClientInfo client_info, IRestResponse response, RequestSettings settings)
        {
            RestException exception = GetRestException(response);
            RateLimit rate_limit = new RateLimit(response.Headers);
            Tuple<IRestResponse, RestException, RateLimit> tuple = Tuple.Create(response, exception, rate_limit);

            switch (exception.error_source)
            {
                case RestErrorSource.None:
                {
                    Debug.WriteLine(response.StatusCode + ", " + rate_limit.remaining);

                    return tuple;
                }

                case RestErrorSource.Internal:
                {
                    if (settings.internal_error_handling == ErrorHandling.Error)
                    {
                        throw exception;
                    }
                    else
                    {
                        return tuple;
                    }
                }

                case RestErrorSource.Request:
                {
                    tuple = await HandleRequestError(client_info, response, exception, rate_limit, settings);
                }
                break;                
            }

            return tuple;
        }

        private static async Task<Tuple<IRestResponse, RestException, RateLimit>>
        HandleRequestError(ClientInfo client_info, IRestResponse response, RestException exception, RateLimit rate_limit, RequestSettings settings)
        {            
            Tuple<IRestResponse, RestException, RateLimit> tuple = Tuple.Create(response, exception, rate_limit);

            int code = (int)response.StatusCode;
            switch (settings.status_codes[code].handling)
            {
                case StatusHandling.Error:
                {
                    throw exception;
                }

                case StatusHandling.Return:
                {
                    return tuple;
                }

                case StatusHandling.Retry:
                {
                    ++settings.status_codes[code].retry_count;
                    if (settings.status_codes[code].retry_count > settings.status_codes[code].retry_limit && settings.status_codes[code].retry_limit != -1)
                    {
                        if(settings.status_codes[code].retry_limit_reached_handling == ErrorHandling.Error)
                        {
                            throw exception;
                        }
                        else
                        {
                            return tuple;
                        }
                    }
                    
                    if(rate_limit != RateLimit.None)
                    {
                        TimeSpan time = rate_limit.reset_time - DateTime.Now;
                        if (rate_limit.remaining == 0 && time.TotalMilliseconds > 0)
                        {
                            Debug.WriteLine(TimeStamp.TimeLong, "Request rate limit reached. Waiting " + time.TotalMilliseconds + "ms to execute the request again.");
                            await Task.Delay(time);
                            Debug.WriteLine(TimeStamp.TimeLong, "Resuming request.");
                        }
                    }

                    tuple = await ExecuteAsync(client_info, response.Request, settings);
                }
                break;                
            }

            return tuple;
        }

        private static async Task<Tuple<IRestResponse<result_type>, RestException, RateLimit>>
        HandleResponse<result_type>(ClientInfo client_info, IRestResponse<result_type> response, RequestSettings settings)
        {
            // TODO: Exception thrown is not descriptive enough. 
            RestException exception = GetRestException(response);
            RateLimit rate_limit = new RateLimit(response.Headers);
            Tuple<IRestResponse<result_type>, RestException, RateLimit> tuple = Tuple.Create(response, exception, rate_limit);

            switch (exception.error_source)
            {
                case RestErrorSource.None:
                {
                    Debug.WriteLine(response.StatusCode + ", " + rate_limit.remaining);

                    return tuple;
                }

                case RestErrorSource.Internal:
                {
                    if (settings.internal_error_handling == ErrorHandling.Error)
                    {
                        throw exception;
                    }
                    else
                    {
                        return Tuple.Create(response, exception, RateLimit.None);
                    }
                }

                case RestErrorSource.Request:
                {
                    tuple = await HandleRequestError(client_info, response, exception, rate_limit, settings);
                }
                break;
            }

            return tuple;
        }

        private static async Task<Tuple<IRestResponse<result_type>, RestException, RateLimit>>
        HandleRequestError<result_type>(ClientInfo client_info, IRestResponse<result_type> response, RestException exception, RateLimit rate_limit, RequestSettings settings)
        {
            Tuple<IRestResponse<result_type>, RestException, RateLimit> tuple = Tuple.Create(response, exception, rate_limit);

            int code = (int)response.StatusCode;
            switch (settings.status_codes[code].handling)
            {
                case StatusHandling.Error:
                {
                    throw exception;
                }

                case StatusHandling.Return:
                {
                    return tuple;
                }

                case StatusHandling.Retry:
                {
                    ++settings.status_codes[code].retry_count;
                    if (settings.status_codes[code].retry_count > settings.status_codes[code].retry_limit && settings.status_codes[code].retry_limit != -1)
                    {
                        if (settings.status_codes[code].retry_limit_reached_handling == ErrorHandling.Error)
                        {
                            throw exception;
                        }
                        else
                        {
                            return tuple;
                        }
                    }

                    if (rate_limit != RateLimit.None)
                    {
                        TimeSpan time = rate_limit.reset_time - DateTime.Now;
                        if (rate_limit.remaining == 0 && time.TotalMilliseconds > 0)
                        {
                            Debug.WriteLine(TimeStamp.TimeLong, "Request rate limit reached. Waiting " + time.TotalMilliseconds + "ms to execute the request again.");
                            await Task.Delay(time);
                            Debug.WriteLine(TimeStamp.TimeLong, "Resuming request.");
                        }
                    }

                    tuple = await ExecuteAsync<result_type>(client_info, response.Request, settings);
                }
                break;
            }

            return tuple;
        }

        private static RestException
        GetRestException(IRestResponse response)
        {
            RestException exception = new RestException();

            RestError error = JsonConvert.DeserializeObject<RestError>(response.Content);
            if (error.IsNull())
            {
                return exception;
            }

            if (!response.ErrorException.IsNull())
            {
                exception = new RestException(response, RestErrorSource.Internal, "An error was encountered by RestSharp while making the request. See the inner exception for more detials.", response.ErrorException);
            }
            else if (error.status != 0 || error.message.IsValid())
            {
                exception = new RestException(response, RestErrorSource.Request, error);
            }
            else
            {
                exception = new RestException();
            }

            return exception;
        }

        #endregion

        #region Helper methods

        public static RestRequest
        CreateHelixRequest(string endpoint, Method method, HelixInfo info, RequestSettings settings)
        {
            if (settings.IsNullOrDefault())
            {
                settings = RequestSettings.Default;
            }

            if (settings.input_hanlding == InputHandling.Error && !info.bearer_token.IsValid() && !info.client_id.IsValid())
            {
                throw new ArgumentException("A valid " + nameof(info.bearer_token) + " or " + nameof(info.client_id) + " must be provided.");
            }

            RestRequest request = new RestRequest(endpoint, method);
            if (info.bearer_token.IsValid())
            {
                request.AddHeader("Authorization", "Bearer " + info.bearer_token);
            }

            if (info.client_id.IsValid())
            {
                request.AddHeader("Client-ID", info.client_id);
            }

            return request;
        }

        private static RestClient
        CreateClient(ClientInfo info)
        {
            ExceptionUtil.ThrowIfInvalid(info.base_url, nameof(info.base_url));
            ExceptionUtil.ThrowIfInvalid(info.handlers, nameof(info.handlers));

            RestClient client = new RestClient(info.base_url);
            foreach (ClientHandler handler in info.handlers)
            {
                if (!handler.content_type.IsValid() || handler.deserializer.IsNull())
                {
                    continue;
                }

                client.AddHandler(handler.content_type, handler.deserializer);
            }

            return client;
        }

        #endregion
    }
}