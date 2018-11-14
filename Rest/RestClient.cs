using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using TwitchNet.Extensions;
using TwitchNet.Helpers.Json;

namespace TwitchNet.Rest
{
    

    public class
    RestClient : IDisposable
    {
        private HttpClient client;

        public bool disposed { get; private set; } = true;

        public Uri base_address => client.BaseAddress;

        public HttpRequestHeaders default_headers => client.DefaultRequestHeaders;

        public TimeSpan timeout
        {
            get
            {
                return client.Timeout;
            }
            set
            {
                client.Timeout = value;
            }
        }

        public Dictionary<string, IDeserializer> content_handlers { get; }

        public RestClient(string base_address)
        {
            if (!base_address.IsValid())
            {
                throw new ArgumentException("Value cannot be null, empty, or contain only whitespace.", nameof(base_address));
            }

            base_address = base_address.TrimEnd('/');

            // For some reaosn, Twitch compresses select responses using GZip.
            // Enable automatic decompression for when this shit occurs.
            HttpClientHandler handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            client = new HttpClient(handler);
            client.BaseAddress = new Uri(base_address);

            disposed = false;

            content_handlers = new Dictionary<string, IDeserializer>();
            ResetContentHandlers();
        }

        public void
        Dispose()
        {
            if (disposed)
            {
                return;
            }

            client.Dispose();
            client = null;

            disposed = true;
        }

        public bool
        SetContentHandler(string content_type, IDeserializer deserializer)
        {
            if (disposed)
            {
                return false;
            }

            if (!MediaTypeWithQualityHeaderValue.TryParse(content_type, out MediaTypeWithQualityHeaderValue media_type))
            {
                return false;
            }

            content_handlers[content_type] = deserializer;

            if (!client.DefaultRequestHeaders.Accept.Contains(media_type))
            {
                client.DefaultRequestHeaders.Accept.Add(media_type);
            }

            return true;
        }

        public bool
        RemoveContentHandler(string content_type, IDeserializer deserializer)
        {
            if (disposed)
            {
                return false;
            }

            if (!MediaTypeWithQualityHeaderValue.TryParse(content_type, out MediaTypeWithQualityHeaderValue media_type))
            {
                return false;
            }

            content_handlers.Remove(content_type);
            client.DefaultRequestHeaders.Accept.Remove(media_type);

            return true;
        }

        public void
        ResetContentHandlers()
        {
            SetContentHandler("application/json", new JsonDeserializer());
            SetContentHandler("text/json", new JsonDeserializer());
        }

        public void
        ClearContentHandlers()
        {
            if (!content_handlers.IsNull())
            {
                content_handlers.Clear();
            }

            if (!disposed)
            {
                client.DefaultRequestHeaders.Accept.Clear();
            }
        }

        public async Task<RestResponse<data_type>>
        ExecuteAsync<data_type>(RestRequest request, Func<RestResponse<data_type>, Task<RestResponse<data_type>>> CustomResponseHandler = default)
        {
            RestResponse<data_type> response = default;

            if (disposed)
            {
                return response;
            }

            if (request.IsNull() || request.disposed)
            {
                return response;
            }

            string content = string.Empty;
            data_type data = default;

            HttpRequestMessage _request = request.BuildMessage(base_address);
            HttpResponseMessage _response = default;

            // I hate using try/catch blocks, but my hand is forced here.
            try
            {
                _response = await client.SendAsync(_request, request.settings.cancelation_token);
                if (!_response.Content.IsNull() && _response.Content.Headers.ContentLength > 0)
                {
                    content = await _response.Content.ReadAsStringAsync();
                }
            }
            catch(Exception exception)
            {
                // InvalidOperationException or HttpRequestException.
                response = new RestResponse<data_type>(request, _response, content, exception);

                return await HandleResponse(response);
            }

            StatusException status_exception = default;
            if (!_response.IsSuccessStatusCode)
            {
                status_exception = new StatusException((int)_response.StatusCode + " - " + _response.ReasonPhrase, _response.StatusCode, _response.ReasonPhrase);                
            }
            else if (content.IsValid())
            {
                if (content_handlers.TryGetValue(_response.Content.Headers.ContentType.MediaType, out IDeserializer deserializer))
                {
                    data = deserializer.Deserialize<data_type>(content);
                }
            }                

            response = new RestResponse<data_type>(request, _response, content, data, status_exception);

            // TODO: Implement custom handler for Helix (and OAUth2 whenevr I get around to it).
            // Hack to prevent premature throwing, but it works.
            if (!CustomResponseHandler.IsNull())
            {
                response = await CustomResponseHandler(response);
            }
            else
            {
                response = await HandleResponse(response);
            }

            return response;
        }

        public virtual async Task<RestResponse<data_type>>
        HandleResponse<data_type>(RestResponse<data_type> response)
        {
            if(response.exception.IsNull())
            {
                return response;
            }

            if (response.exception is StatusException)
            {
                int code = (int)response.status_code;
                switch (response.request.settings.status_error[code].handling)
                {
                    case StatusHandling.Error:
                    {
                        // NOTE: Throwing here causes issues down the pipeline for deserializing the content into HelixError.
                        //       Overriding isn't an option because TwitchAPI uses RestClient as a member and doesn't inherit the class.
                        //       The handler can't be manually set because of the use of generics.
                        // TODO: Pass the custom handler with each execution call from outside the RestClient?
                        throw response.exception;
                    };

                    case StatusHandling.Retry:
                    {
                        StatusCodeSetting status_setting = response.request.settings.status_error[code];

                        ++status_setting.retry_count;
                        if (status_setting.retry_count > status_setting.retry_limit && status_setting.retry_limit != -1)
                        {
                            response.exception = new RetryLimitReachedException("The retry limit " + status_setting.retry_limit + " has been reached for status code " + code + ".", status_setting.retry_limit, response.exception);

                            return await HandleResponse(response);
                        }

                        // Clone the message to a new instance because the same instance can't be sent twice.
                        response.request.CloneMessage();
                        response = await ExecuteAsync<data_type>(response.request);
                    };
                    break;

                    case StatusHandling.Return:
                    default:
                    {
                        return response;
                    }
                }
            }
            else
            {
                ErrorHandling handling;
                if(response.exception is InvalidOperationException)
                {
                    handling = response.request.settings.invalid_operation_handling;

                }
                else if (response.exception is HttpRequestException)
                {
                    handling = response.request.settings.request_handling;
                }
                else
                {
                    int code = (int)response.status_code;
                    handling = response.request.settings.status_error[code].handling_rety_limit_reached;
                }

                switch (handling)
                {
                    case ErrorHandling.Error:
                    {
                        throw response.exception;
                    };

                    case ErrorHandling.Return:
                    default:
                    {
                        return response;
                    }
                }
            }            

            return response;
        }
    }
}
