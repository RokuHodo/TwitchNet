using System;
using System.Collections.Generic;

using TwitchNet.Rest.Api;
using TwitchNet.Extensions;

using RestSharp;

namespace TwitchNet.Rest
{
    public class
    RestInfo
    {
        public string client_id { get; set; }

        public string bearer_token { get; set; }

        public string oauth_token { get; set; }

        public Scopes required_scopes { get; set; }

        public RestClient client { get; set; }

        public IRestRequest request { get; set; }

        public IRestResponse response { get; set; }

        public RateLimit rate_limit { get; set; }

        public RequestSettings settings { get; set; }

        public IList<Exception> exceptions { get; set; }

        public RestErrorSource exception_source { get; set; }

        public RestInfo(RestClient client, RequestSettings settings)
        {
            client_id           = string.Empty;
            bearer_token        = string.Empty;
            oauth_token         = string.Empty;

            this.client         = client;
            request             = default;
            response            = default;

            rate_limit          = RateLimit.None;

            if (settings.IsNull())
            {
                settings        = RequestSettings.Default;
            }

            this.settings       = settings;

            exceptions          = new List<Exception>();
            exception_source    = RestErrorSource.None;
        }

        public void
        SetScopeError(MissingScopesException exception)
        {
            exceptions.Add(exception);
            exception_source = RestErrorSource.ScopeValidaiton;

            if (settings.error_handling_missing_scopes == ErrorHandling.Error)
            {
                throw new AggregateException("One or more errors occurred while making the rest request.", exceptions);
            }
        }

        public void
        SetInputError(ArgumentException exception)
        {
            exceptions.Add(exception);
            exception_source = RestErrorSource.InputValidaiton;

            if (settings.error_handling_inputs == ErrorHandling.Error)
            {
                throw new AggregateException("One or more errors occurred while making the rest request.", exceptions);
            }
        }

        public void
        SetExecutionError(Exception exception)
        {
            exceptions.Add(exception);
            exception_source = RestErrorSource.Execution;

            if (settings.error_handling_execution == ErrorHandling.Error)
            {
                throw new AggregateException("One or more errors occurred while making the rest request.", exceptions);
            }
        }

        public void
        SetRestError(ushort code, RestException exception)
        {
            exceptions.Add(exception);
            exception_source = RestErrorSource.Api;

            if (settings.status_code_settings[code].handling == StatusHandling.Error)
            {
                throw new AggregateException("One or more errors occurred while making the rest request.", exceptions);
            }
        }

        public void
        SetRetryError(ushort code, RetryLimitReachedException exception)
        {
            exceptions.Add(exception);
            exception_source = RestErrorSource.Api;

            if (settings.status_code_settings[code].retry_limit_reached_handling == ErrorHandling.Error)
            {
                throw new AggregateException("One or more errors occurred while making the rest request.", exceptions);
            }
        }
    }

    public class
    RestInfo<result_type> : RestInfo
    {
        public new IRestResponse<result_type> response { get; set; }

        public RestInfo(RestClient client, RequestSettings settings) : base(client, settings)
        {
            response = default;
        }
    }
}
