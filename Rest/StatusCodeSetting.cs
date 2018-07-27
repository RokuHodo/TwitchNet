namespace
TwitchNet.Rest
{
    public class
    StatusCodeSetting
    {
        /// <summary>
        /// Returns the default value of a <see cref="StatusCodeSetting"/>.
        /// </summary>
        public static readonly StatusCodeSetting Default = new StatusCodeSetting();

        /// <summary>
        /// How many times the request has been retried.
        /// </summary>
        public int              retry_count                     { get; internal set; }

        /// <summary>
        /// <para>The maximum number of times to retry executing the request.</para>
        /// <para>
        /// This setting is only valid when <see cref="handling"/> is set to <see cref="StatusHandling.Retry"/>.
        /// Otherwise, it is ignored.
        /// </para>
        /// <para>
        /// Default: 1.
        /// When set to -1, the request will be retried infinitely until it succeedes.
        /// </para>
        /// </summary>
        public int              retry_limit                     { get; set; }

        /// <summary>
        /// <para>How to handle when the maximum number of retries is reached.</para>
        /// <para>
        /// This setting is only valid when <see cref="handling"/> is set to <see cref="StatusHandling.Retry"/>.
        /// Otherwise, it is ignored.
        /// </para>
        /// <para>Default: <see cref="ErrorHandling.Error"/>.</para>
        /// </summary>
        public ErrorHandling    handling_rety_limit_reached     { get; set; }

        /// <summary>
        /// <para>How to handle errors with a specific status code.</para>
        /// <para>Default: <see cref="StatusHandling.Error"/>.</para>
        /// </summary>
        public StatusHandling   handling                        { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="StatusCodeSetting"/> struct.
        /// </summary>
        public StatusCodeSetting()
        {
            Reset();
        }

        /// <summary>
        /// Resets the status code handler back to its default value.
        /// </summary>
        public void
        Reset()
        {
            retry_count                 = 0;
            retry_limit                 = 1;

            handling_rety_limit_reached = ErrorHandling.Error;
            handling                    = StatusHandling.Error;
        }
    }
}
