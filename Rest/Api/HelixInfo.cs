using System;

namespace TwitchNet.Rest.Api
{
    public class
    HelixInfo
    {
        public string client_id;

        public string bearer_token;

        public HelixRequestSettings settings;

        public Scopes required_scopes;

        public Exception exception;

        public void
        SetInputError(ArgumentException exception)
        {
            this.exception = exception;

            if (settings.invalid_operation_handling == ErrorHandling.Error)
            {
                throw exception;
            }
        }

        public void
        SetScopesError(MissingScopesException exception)
        {
            this.exception = exception;

            if (settings.error_handling_missing_scopes == ErrorHandling.Error)
            {
                throw exception;
            }
        }

    }
}
