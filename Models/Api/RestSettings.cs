// standard namespaces
using System.Threading;

// project namespaces
using TwitchNet.Enums;
using TwitchNet.Enums.Api;

namespace
TwitchNet.Models.Api
{
    public class
    RestSettings
    {
        /// <summary>
        /// Returns the default rest request settings.
        /// </summary>
        public static readonly RestSettings Default = new RestSettings();

        /// <summary>
        /// <para>The token used to cancel the request.</para>
        /// <para>Default: <see cref="CancellationToken.None"/>.</para>
        /// </summary>
        public CancellationToken    cancelation_token                   { get; set; }

        /// <summary>
        /// <para>The settings to handle the various HTTP status codes.</para>
        /// <para>Default: <see cref="StatusCodeSettings.Default"/>.</para>
        /// </summary>
        public StatusCodeSettings   status_codes                        { get; set; }        

        /// <summary>
        /// <para>Determine whether or not to check and verify inputs by the user upon request execution.</para>
        /// <para>Default: <see cref="InputHandling.Error"/>.</para>
        /// </summary>
        public InputHandling        input_hanlding                      { get; set; }

        /// <summary>
        /// <para>Determine how to handle any exceptions that are encountered internally witin the library.</para>
        /// <para>Default: <see cref="ErrorHandling.Error"/>.</para>
        /// </summary>
        public ErrorHandling        internal_error_handling             { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="RestSettings"/> class.
        /// </summary>
        public RestSettings()
        {
            Reset();
        }

        /// <summary>
        /// Sets the request settings back to their default values.
        /// </summary>
        public void
        Reset()
        {           
            cancelation_token       = CancellationToken.None;

            status_codes            = StatusCodeSettings.Default;

            input_hanlding          = InputHandling.Error;
            internal_error_handling = ErrorHandling.Error;
        }
    }
}
