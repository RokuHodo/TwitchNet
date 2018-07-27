// standard namespaces
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Rest;
using TwitchNet.Rest.Api;
using TwitchNet.Rest.OAuth;
using TwitchNet.Rest.OAuth.Validate;
using TwitchNet.Extensions;

// imported .dll's
using Newtonsoft.Json;

using RestSharp;

namespace
TwitchNet.Utilities
{
    public static class
    RestUtil
    {
        private static readonly ConcurrentDictionary<Type, RestParameterConverter> REST_PARAMETER_CONVERTER_CACHE = new ConcurrentDictionary<Type, RestParameterConverter>();

        public static async Task<RestInfo>
        ExecuteAsync(RestInfo info)
        {
            info.response = await info.client.ExecuteTaskAsync(info.request, info.settings.cancelation_token);

            info = await HandleResponse(info);

            return info;
        }

        public static async Task<RestInfo<result_type>>
        ExecuteAsync<result_type>(RestInfo<result_type> info)
        {
            info.response = await info.client.ExecuteTaskAsync<result_type>(info.request, info.settings.cancelation_token);

            info = await HandleResponse(info);

            return info;
        }

        public static async Task<RestInfo<result_type>>
        TraceExecuteAsync<data_type, result_type>(RestInfo<result_type> info, IPagingParameters parameters)
        where result_type : DataPage<data_type>, IDataPage<data_type>, new()
        {
            IRestResponse<result_type> response = new RestResponse<result_type>();

            result_type total_data = new result_type();
            total_data.data = new List<data_type>();

            List<data_type> data = new List<data_type>();

            if (parameters.IsNull())
            {
                parameters = new PagingParameters();
            }

            bool requesting = true;
            do
            {
                info = await ExecuteAsync(info);

                if (info.response.Data.data.IsValid())
                {
                    foreach (data_type element in info.response.Data.data)
                    {
                        data.Add(element);
                    }
                }                

                requesting = info.response.Data.data.IsValid() && info.response.Data.pagination.cursor.IsValid();
                if (requesting)
                {
                    // NOTE: This is a temporary fix and will only work with Helix.
                    info.request = info.request.AddOrUpdateParameter("after", info.response.Data.pagination.cursor, ParameterType.QueryString);
                }
                else
                {
                    // TODO: Clean up
                    total_data = info.response.Data;
                    total_data.data = data;

                    response = info.response;
                    response.Data = total_data;

                    info.response = response;
                }
            }
            while (requesting);

            return info;
        }

        #region Paging  

        public static IRestRequest
        AddPaging(this IRestRequest request, object parameters)
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
                if (!typeof(RestParameterConverter).IsAssignableFrom(attribute.converter))
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

                RestParameterConverter converter = REST_PARAMETER_CONVERTER_CACHE.GetOrAdd(attribute.converter, AddRestConverter);
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

        private static async Task<RestInfo>
        HandleResponse(RestInfo info)
        {
            info.rate_limit = new RateLimit(info.response.Headers);

            if (!info.response.ErrorException.IsNull())
            {
                info.SetExecutionError(info.response.ErrorException);

                return info;
            }

            RestError error = JsonConvert.DeserializeObject<RestError>(info.response.Content);
            if (error.IsNull())
            {
                Debug.WriteLine(ConsoleColor.Green, info.response.StatusCode + " - " + info.rate_limit.remaining + " / " + info.rate_limit.limit);

                return info;
            }

            // Handles StatusHandling.Error
            ushort code = (ushort)info.response.StatusCode;
            info.SetRestError(code, new RestException("An error was returned by Twitch after executing the request.", error));

            // Handles StatusHandling.Return
            if (info.settings.status_codes[code].handling == StatusHandling.Return)
            {
                return info;
            }

            // Handles StatusHandling.Retry
            ++info.settings.status_codes[code].retry_count;
            if (info.settings.status_codes[code].retry_count > info.settings.status_codes[code].retry_limit && info.settings.status_codes[code].retry_limit != -1)
            {
                info.SetRetryError(code, new RetryLimitReachedException("Retry limit reached for status code " + code + ".", info.settings.status_codes[code].retry_limit));

                return info;
            }

            if (info.rate_limit != RateLimit.None)
            {
                TimeSpan time = info.rate_limit.reset_time - DateTime.Now;
                if (info.rate_limit.remaining == 0 && time.TotalMilliseconds > 0)
                {
                    Debug.WriteLine(TimeStamp.TimeLong, "Request rate limit reached. Waiting " + time.TotalMilliseconds + "ms to execute the request again.");
                    await Task.Delay(time);
                    Debug.WriteLine(TimeStamp.TimeLong, "Resuming request.");
                }
            }

            info = await ExecuteAsync(info);

            return info;
        }

        private static async Task<RestInfo<result_type>>
        HandleResponse<result_type>(RestInfo<result_type> info)
        {
            info.rate_limit = new RateLimit(info.response.Headers);            

            if (!info.response.ErrorException.IsNull())
            {
                info.SetExecutionError(info.response.ErrorException);

                return info;
            }

            RestError error = JsonConvert.DeserializeObject<RestError>(info.response.Content);
            if (error.IsNull() || error.error.IsNull())
            {
                Debug.WriteLine(ConsoleColor.Green, info.response.StatusCode + " - " + info.rate_limit.remaining + " / " + info.rate_limit.limit);

                return info;
            }

            // Handles StatusHandling.Error
            ushort code = (ushort)info.response.StatusCode;
            info.SetRestError(code, new RestException("An error was returned by Twitch after executing the request.", error));

            // Handles StatusHandling.Return
            if (info.settings.status_codes[code].handling == StatusHandling.Return)
            {
                return info;
            }

            // Handles StatusHandling.Retry
            ++info.settings.status_codes[code].retry_count;
            if (info.settings.status_codes[code].retry_count > info.settings.status_codes[code].retry_limit && info.settings.status_codes[code].retry_limit != -1)
            {
                info.SetRetryError(code, new RetryLimitReachedException("Retry limit reached for status code " + code + ".", info.settings.status_codes[code].retry_limit));

                return info;
            }

            if (info.rate_limit != RateLimit.None)
            {
                TimeSpan time = info.rate_limit.reset_time - DateTime.Now;
                if (info.rate_limit.remaining == 0 && time.TotalMilliseconds > 0)
                {
                    Debug.WriteLine(TimeStamp.TimeLong, "Request rate limit reached. Waiting " + time.TotalMilliseconds + "ms to execute the request again.");
                    await Task.Delay(time);
                    Debug.WriteLine(TimeStamp.TimeLong, "Resuming request.");
                }
            }

            info = await ExecuteAsync(info);

            return info;
        }

        #endregion

        #region Helper methods        

        public static RestInfo<result_type>
        CreateHelixRequest<result_type>(string endpoint, Method method, RestInfo<result_type> rest_info)
        {
            if (rest_info.settings.IsNullOrDefault())
            {
                rest_info.settings = RequestSettings.Default;
            }

            if(!TryValidateAuthentication(rest_info))
            {
                return rest_info;
            }

            rest_info.request = new RestRequest(endpoint, method);
            if (rest_info.bearer_token.IsValid())
            {
                rest_info.request.AddHeader("Authorization", "Bearer " + rest_info.bearer_token);
            }

            if (rest_info.client_id.IsValid())
            {
                rest_info.request.AddHeader("Client-ID", rest_info.client_id);
            }

            return rest_info;
        }

        private static bool
        TryValidateAuthentication(RestInfo rest_info)
        {
            if (!rest_info.bearer_token.IsValid() && !rest_info.client_id.IsValid())
            {
                rest_info.SetInputError(new ArgumentException("A bearer token or client ID must be provided."));

                return false;
            }
            else if (rest_info.required_scopes == 0)
            {
                return true;
            }
            else if (!rest_info.bearer_token.IsValid())
            {
                Scopes[] missing_scopes = EnumUtil.GetFlagValues<Scopes>(rest_info.required_scopes);
                rest_info.SetScopeError(new MissingScopesException("One or more scopes are missing from the specified OAuth token.", missing_scopes));

                return false;
            }

            Scopes[] available_scopes = rest_info.settings.available_scopes;
            if (!available_scopes.IsValid())
            {
                // If no available scopes were specified, it's not inherently an error.
                // The user might just not want to verify the scopes or didn't provide any.
                return true;
            }

            foreach (Scopes scope in available_scopes)
            {
                if ((scope & rest_info.required_scopes) == scope)
                {
                    rest_info.required_scopes ^= scope;
                }
            }

            if (rest_info.required_scopes != 0)
            {
                Scopes[] missing_scopes = EnumUtil.GetFlagValues<Scopes>(rest_info.required_scopes);
                rest_info.SetScopeError(new MissingScopesException("One or more scopes are missing from the specified OAuth token.", missing_scopes));

                return false;
            }

            return true;
        }

        #endregion
    }
}