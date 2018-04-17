// project namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Enums.Api;
using TwitchNet.Utilities;

namespace TwitchNet.Models.Api
{
    public class
    StatusCodeSettings
    {
        private static readonly int STATUS_CODE_MIN = 100;
        private static readonly int STATUS_CODE_MAX = 599;

        private Dictionary<int, StatusCodeSetting> handlers;

        /// <summary>
        /// Returns the default status code handlers.
        /// </summary>
        public static readonly StatusCodeSettings Default = new StatusCodeSettings();

        /// <summary>
        /// Access the handling settings for a specific status code.
        /// </summary>
        /// <param name="status_code">The status code to access.</param>
        /// <returns>Returns the handling settings for a specific status code.</returns>
        /// <exception cref="ArgumentException">Thrown if no handlers have been initialized.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the status code is less than 100 or more than 599.</exception>
        public StatusCodeSetting
        this[int status_code]
        {
            get
            {
                ExceptionUtil.ThrowIfInvalid(handlers, nameof(handlers));
                ExceptionUtil.ThrowIfOutOfRange(nameof(status_code), status_code, STATUS_CODE_MIN, STATUS_CODE_MAX);
                return handlers[status_code];
            }
            set
            {
                ExceptionUtil.ThrowIfInvalid(handlers, nameof(handlers));
                ExceptionUtil.ThrowIfOutOfRange(nameof(status_code), status_code, STATUS_CODE_MIN, STATUS_CODE_MAX);
                this[status_code] = value;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StatusCodeSettings"/> class.
        /// </summary>
        public StatusCodeSettings()
        {
            Reset();
        }

        /// <summary>
        /// Sets all status code handlers back to their default value.
        /// </summary>
        public void
        Reset()
        {
            handlers = new Dictionary<int, StatusCodeSetting>();
            for (int status_code = STATUS_CODE_MIN; status_code <= STATUS_CODE_MAX; ++status_code)
            {
                handlers[status_code] = StatusCodeSetting.Default;
            }
        }
    }

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
        internal    int             retry_count { get; set; }

        /// <summary>
        /// <para>The maximum amount of times to retry the request.</para>
        /// <para>When set to -1, the request will be retried infinitely until it succeedes.</para>
        /// </summary>
        public      int             retry_limit { get; set; }

        /// <summary>
        /// How to handle the status code.
        /// </summary>
        public      StatusHandling  handling    { get; set; }

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
            retry_count = 0;
            retry_limit = -1;
            handling = StatusHandling.Error;
        }
    }
}
