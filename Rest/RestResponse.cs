using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using TwitchNet.Extensions;

namespace TwitchNet.Rest
{
    public class
    RestResponse : IRestResponse
    {
        public bool                 is_successful       { get; internal set; }

        public HttpStatusCode       status_code         { get; internal set; }

        public string               status_description  { get; internal set; }

        public string               content             { get; internal set; }

        public HttpResponseHeaders  headers             { get; internal set; }

        public HttpContentHeaders   content_headers     { get; internal set; }

        public Version              version             { get; internal set; }

        public RestRequest          request             { get; internal set; }

        public Exception            exception           { get; internal set; }

        public RestResponse(RestRequest request, HttpResponseMessage response, string content, Exception exception = default) : this(request, response, exception)
        {
            this.content            = content;
        }

        public RestResponse(RestRequest request, HttpResponseMessage response, Exception exception = default) : this(exception)
        {
            if (!response.IsNull())
            {
                is_successful       = response.IsSuccessStatusCode;

                status_code         = response.StatusCode;
                status_description  = response.ReasonPhrase;


                headers             = response.Headers;

                content_headers     = response.Content.Headers;

                version             = response.Version;
            }            

            this.request            = request;
        }

        public RestResponse(Exception exception = default)
        {
            this.exception          = exception;
        }
    }

    public class
    RestResponse<data_type> : RestResponse, IRestResponse<data_type>
    {
        public data_type data { get; internal set; }                

        public RestResponse(RestRequest request, HttpResponseMessage response, string content, data_type data, Exception exception = default) : base(request, response, content, exception)
        {
            this.data = data;
        }

        public RestResponse(RestRequest request, HttpResponseMessage response, string content, Exception exception = default) : base(request, response, content, exception)
        {

        }

        public RestResponse(Exception exception = default) : base(exception)
        {

        }
    }
}
