using System;
using System.Net;
using System.Net.Http.Headers;

namespace TwitchNet.Rest
{
    public interface
    IRestResponse
    {
        bool                is_successful       { get; }

        HttpStatusCode      status_code         { get; }

        string              status_description  { get; }

        string              content             { get; }

        HttpResponseHeaders headers             { get; }

        HttpContentHeaders  content_headers     { get; }

        Version             version             { get; }

        RestRequest         request             { get; }

        Exception           exception           { get; }
    }

    public interface
    IRestResponse<data_type> : IRestResponse
    {
        data_type           data                { get; }
    }
}
