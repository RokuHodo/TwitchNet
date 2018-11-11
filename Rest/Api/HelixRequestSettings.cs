// standard namespaces
using System.Collections.Generic;
using System.Threading;

namespace
TwitchNet.Rest.Api
{
    public class
    HelixRequestSettings : RequestSettings
    {
        /// <summary>
        /// <para>How to handle errors experienced while executing the request.</para>
        /// <para>Default: <see cref="ErrorHandling.Error"/>.</para>
        /// </summary>
        public ErrorHandling    error_handling_inputs           { get; set; }

        /// <summary>
        /// <para>
        /// How to handle OAuth tokens with missing scopes needed to authenticate the request.
        /// This setting is only valid when available scopes are specified.
        /// </para>
        /// <para>Default: <see cref="ErrorHandling.Error"/>.</para>
        /// </summary>
        public ErrorHandling    error_handling_missing_scopes   { get; set; }

        /// <summary>
        /// <para>
        /// The scopes that were requested when creating an OAuth token.
        /// The scopes contained within an OAuth token can be obtained by using <see cref="OAuth2.ValidateToken(string, HelixRequestSettings)"/>.
        /// </para>
        /// <para>
        /// If specified when making an authenticated request, the available scopes will be used to check if the required scope(s) to make the request are present.
        /// If not specified when making an authenticated request, the reuqest will be made normally without perfomring preemptive checks.
        /// If specified when making a request that does not require authentication, the available scopes are ignored.
        /// </para>
        /// </summary>
        public Scopes           available_scopes                { get; set; }

        public HelixRequestSettings()
        {
            // Copy paste from Reset() to avoid duplicate instanciation.
            error_handling_inputs = ErrorHandling.Error;
            error_handling_missing_scopes = ErrorHandling.Error;

            available_scopes = Scopes.Other;
        }

        /// <summary>
        /// Sets the request settings back to their default values.
        /// </summary>
        public override void
        Reset()
        {
            error_handling_inputs = ErrorHandling.Error;
            error_handling_missing_scopes = ErrorHandling.Error;

            available_scopes = Scopes.Other;

            base.Reset();
        }
    }
}
