// standard namespaces
using System.Collections.Generic;
using System.Threading;

namespace
TwitchNet.Rest
{
    public class
    RequestSettings
    {
        /// <summary>
        /// Returns the default rest request settings.
        /// </summary>
        public static readonly RequestSettings Default = new RequestSettings();

        /// <summary>
        /// <para>The token used to cancel the request.</para>
        /// <para>Default: <see cref="CancellationToken.None"/>.</para>
        /// </summary>
        public CancellationToken                    cancelation_token               { get; set; }

        /// <summary>
        /// <para>The settings usded to handle the different status codes when an API error is returned.</para>
        /// </summary>
        public Dictionary<int, StatusCodeSetting>   status_codes                    { get; set; }

        /// <summary>
        /// <para>How to handle errors experienced while executing the request.</para>
        /// <para>Default: <see cref="ErrorHandling.Error"/>.</para>
        /// </summary>
        public ErrorHandling                        error_handling_inputs           { get; set; }

        /// <summary>
        /// <para>How to handle wrong or improperly formatted user inputs.</para>
        /// <para>Default: <see cref="ErrorHandling.Error"/>.</para>
        /// </summary>
        public ErrorHandling                        error_handling_execution        { get; set; }

        /// <summary>
        /// <para>
        /// How to handle OAuth tokens with missing scopes needed to authenticate the request.
        /// This setting is only valid when available scopes are specified.
        /// </para>
        /// <para>Default: <see cref="ErrorHandling.Error"/>.</para>
        /// </summary>
        public ErrorHandling                        error_handling_missing_scopes   { get; set; }

        /// <summary>
        /// <para>
        /// The scopes that were requested when creating an OAuth token.
        /// The scopes contained within an OAuth token can be obtained by using <see cref="OAuth2.ValidateToken(string, RequestSettings)"/>.
        /// </para>
        /// <para>
        /// If specified when making an authenticated request, the available scopes will be used to check if the required scope(s) to make the request are present.
        /// If not specified when making an authenticated request, the reuqest will be made normally without perfomring preemptive checks.
        /// If specified when making a request that does not require authentication, the available scopes are ignored.
        /// </para>
        /// </summary>
        public Scopes[]                             available_scopes                { get; set; }

        public RequestSettings()
        {
            Reset();
        }

        /// <summary>
        /// Sets the request settings back to their default values.
        /// </summary>
        public void
        Reset()
        {
            cancelation_token = CancellationToken.None;

            status_codes = new Dictionary<int, StatusCodeSetting>();
            for(int code = 100; code < 600; ++code)
            {
                status_codes[code] = StatusCodeSetting.Default;
            }

            error_handling_inputs = ErrorHandling.Error;
            error_handling_execution = ErrorHandling.Error;
            error_handling_missing_scopes = ErrorHandling.Error;

            available_scopes = new Scopes[0];
        }
    }
}
