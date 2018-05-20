// standard namespaces
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// project namespaces
using TwitchNet.Debugger;
using TwitchNet.Enums;
using TwitchNet.Enums.Debugger;
using TwitchNet.Enums.Api;
using TwitchNet.Extensions;
using TwitchNet.Interfaces.Api;
using TwitchNet.Models.Api;

// imported .dll's
using RestSharp;

namespace
TwitchNet.Utilities
{
    internal static class
    RestRequestUtil
    {
        #region Execute methods

        public static async Task<IApiResponse<result_type>>
        ExecutetAsync<result_type>(ClientInfo client_info, RequestInfo request_info, IList<QueryParameter> parameters, ApiRequestSettings settings)
        {
            RestRequest request = CreateRequest(request_info, settings);
            request = PagingUtil.Add(request, parameters);

            IApiResponse<result_type> response = await ExecuteAsync<result_type>(client_info, request, settings);            

            return response;
        }

        public static async Task<IApiResponse<result_type>>
        ExecuteAsync<result_type>(ClientInfo client_info, RequestInfo request_info, object parameters, ApiRequestSettings settings)
        {
            RestRequest request = CreateRequest(request_info, settings);
            request = PagingUtil.Add(request, parameters);

            IApiResponse<result_type> response = await ExecuteAsync<result_type>(client_info, request, settings);

            return response;
        }

        public static async Task<IApiResponse<result_type>>
        ExecutePagesAsync<data_type, result_type>(ClientInfo client_info, RequestInfo request_info, IQueryParameters parameters, ApiRequestSettings settings)
        where result_type : DataPage<data_type>, IDataPage<data_type>, new()
        {
            IApiResponse<result_type> response = new ApiResponse<result_type>();     
            List<data_type> data = new List<data_type>();

            if (parameters.IsNull())
            {
                parameters = new QueryParametersPage();
            }

            if (settings.IsNull())
            {
                settings = ApiRequestSettings.Default;
            }

            bool requesting = true;
            do
            {
                IApiResponse<result_type> page = await ExecuteAsync<result_type>(client_info, request_info, parameters, settings);
                if (page.result.data.IsValid())
                {
                    data.AddRange(page.result.data);
                }                

                requesting = page.result.data.IsValid() && page.result.pagination.cursor.IsValid();
                if (requesting)
                {
                    parameters.after = page.result.pagination.cursor;
                }
                else
                {
                    response = page;
                    response.result.data = data;
                }
            }
            while (requesting);            

            return response;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static async Task<IApiResponse<result_type>>
        ExecuteAsync<result_type>(ClientInfo client_info, IRestRequest request, ApiRequestSettings settings)
        {
            if (settings.IsNull())
            {
                settings = ApiRequestSettings.Default;
            }

            Debug.WriteError(ErrorLevel.Critical, "Test error from API util.");

            RestClient client = CreateClient(client_info);
            IRestResponse<result_type> rest_response = await client.ExecuteTaskAsync<result_type>(request, settings.cancelation_token);

            IApiResponse<result_type> api_response = new ApiResponse<result_type>(rest_response);
            api_response = await HandleResponse(client_info, rest_response, api_response, settings);

            return api_response;
        }

        #endregion

        #region Response handling

        private static async Task<IApiResponse<result_type>>
        HandleResponse<result_type>(ClientInfo client_info, IRestResponse<result_type> rest_response, IApiResponse<result_type> api_response, ApiRequestSettings settings)
        {
            switch (api_response.exception.error_source)
            {
                case ApiErrorSource.None:
                {
                        Debug.WriteLine(api_response.status_code + ". Requests remaining: " + api_response.rate_limit.remaining);

                    return api_response;
                }

                case ApiErrorSource.Internal:
                {
                    if (settings.internal_error_handling == ErrorHandling.Error)
                    {
                        throw api_response.exception;
                    }
                    else
                    {
                        return api_response;
                    }
                }

                case ApiErrorSource.Api:
                {
                    api_response = await HandleApiError(client_info, rest_response, api_response, settings);
                }
                break;                
            }

            return api_response;
        }

        private static async Task<IApiResponse<result_type>>
        HandleApiError<result_type>(ClientInfo client_info, IRestResponse<result_type> rest_response, IApiResponse<result_type> api_response, ApiRequestSettings settings)
        {
            int code = (int)api_response.status_code;
            switch (settings.status_codes[code].handling)
            {
                case StatusHandling.Error:
                {
                    throw api_response.exception;
                }

                case StatusHandling.Return:
                {
                    return api_response;
                }

                case StatusHandling.Retry:
                {
                    ++settings.status_codes[code].retry_count;
                    if (settings.status_codes[code].retry_count > settings.status_codes[code].retry_limit && settings.status_codes[code].retry_limit != -1)
                    {
                        if(settings.status_codes[code].retry_limit_reached_handling == ErrorHandling.Error)
                        {
                            throw api_response.exception;
                        }
                        else
                        {
                            return api_response;
                        }
                    }

                    // this should only ever be a concern when '429 - Too Many Requests' is receieved
                    TimeSpan time = api_response.rate_limit.reset_time - DateTime.Now;
                    if (api_response.rate_limit.remaining == 0 && time.TotalMilliseconds > 0)
                    {
                            Debug.WriteLine(TimeStamp.TimeLong, "Request rate limit reached. Waiting " + time.TotalMilliseconds + "ms to execute the request again.");
                        await Task.Delay(time);
                            Debug.WriteLine(TimeStamp.TimeLong, "Resuming request.");
                    }

                    api_response = await ExecuteAsync<result_type>(client_info, rest_response.Request, settings);
                }
                break;                
            }

            return api_response;
        }

        #endregion

        #region Helper methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static RestRequest
        CreateRequest(RequestInfo info, ApiRequestSettings settings)
        {
            ExceptionUtil.ThrowIfInvalid(info.endpoint, nameof(info.endpoint));

            if (settings.input_hanlding == InputHandling.Error && !info.bearer_token.IsValid() && !info.client_id.IsValid())
            {
                throw new ArgumentException("A valid " + nameof(info.bearer_token) + " or " + nameof(info.client_id) + " must be provided.");
            }

            RestRequest request = new RestRequest(info.endpoint, info.method);
            if (info.bearer_token.IsValid())
            {
                request.AddHeader("Authorization", "Bearer " + info.bearer_token);
            }

            if (info.oauth_token.IsValid())
            {
                request.AddHeader("Authorization", "OAuth " + info.oauth_token);
            }

            if (info.client_id.IsValid())
            {
                request.AddHeader("Client-ID", info.client_id);
            }

            return request;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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