// standard namespaces
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading;

namespace
TwitchNet.Rest
{
    public class
    RequestSettings
    {
        /// <summary>
        /// <para>The token used to cancel the request.</para>
        /// <para>Default: <see cref="CancellationToken.None"/>.</para>
        /// </summary>
        public CancellationToken                    cancelation_token               { get; set; }

        /// <summary>
        /// Determines how to handle the <see cref="InvalidOperationException"/>.
        /// </summary>
        public ErrorHandling                        invalid_operation_handling      { get; set; }

        /// <summary>
        /// Determines how to handle the <see cref="HttpRequestException"/>.
        /// </summary>
        public ErrorHandling                        request_handling                { get; set; }

        /// <summary>
        /// <para>The settings usded to handle the different status codes when an error is returned.</para>
        /// </summary>
        public Dictionary<int, StatusCodeSetting>   status_error                    { get; set; }

        public RequestSettings()
        {
            // Copy paste from Reset() to avoid duplicate instantiation when derived classes instantiated.
            // Keep these synchronized at all times!

            cancelation_token = CancellationToken.None;

            // This covers practically everything under the sun used by major sites.
            // If this doesn't cover a status code receieved, what is that site doing?
            // Just in case, 000 is reserved as a default fallback handler.
            int[] status_codes_0XX = { 000 };
            int[] status_codes_1XX = { 100, 101, 102, 103 };
            int[] status_codes_2XX = { 200, 201, 202, 203, 204, 205, 206, 207, 208, 218, 226 };
            int[] status_codes_3XX = { 300, 301, 302, 303, 304, 305, 306, 307, 308 };
            int[] status_codes_4XX = { 400, 401, 402, 403, 404, 405, 406, 407, 408, 409, 410, 411, 412, 413, 414, 415, 416, 417, 418, 419, 420, 421, 422, 423, 424, 425, 426, 428, 429, 431, 440, 444, 449, 450, 451, 494, 495, 496, 497, 498, 499 };
            int[] status_codes_5XX = { 500, 501, 502, 503, 504, 505, 506, 507, 508, 509, 510, 511, 520, 521, 522, 523, 524, 525, 526, 527, 530, 598, 599 };

            status_error = new Dictionary<int, StatusCodeSetting>(1000);

            foreach (int code in status_codes_0XX)
            {
                status_error[code] = new StatusCodeSetting();
            }

            foreach (int code in status_codes_1XX)
            {
                status_error[code] = new StatusCodeSetting();
            }

            foreach (int code in status_codes_2XX)
            {
                status_error[code] = new StatusCodeSetting();
            }

            foreach (int code in status_codes_3XX)
            {
                status_error[code] = new StatusCodeSetting();
            }

            foreach (int code in status_codes_4XX)
            {
                status_error[code] = new StatusCodeSetting();
            }

            foreach (int code in status_codes_5XX)
            {
                status_error[code] = new StatusCodeSetting();
            }
        }

        /// <summary>
        /// Sets the request settings back to their default values.
        /// </summary>
        public virtual void
        Reset()
        {
            cancelation_token = CancellationToken.None;

            // This covers practically everything under the sun used by major sites.
            // If this doesn't cover a status code receieved, what is that site doing?
            // Just in case, 000 is reserved as a default fallback handler.
            int[] status_codes_0XX = { 000 };
            int[] status_codes_1XX = { 100, 101, 102, 103 };
            int[] status_codes_2XX = { 200, 201, 202, 203, 204, 205, 206, 207, 208, 218, 226 };
            int[] status_codes_3XX = { 300, 301, 302, 303, 304, 305, 306, 307, 308 };
            int[] status_codes_4XX = { 400, 401, 402, 403, 404, 405, 406, 407, 408, 409, 410, 411, 412, 413, 414, 415, 416, 417, 418, 419, 420, 421, 422, 423, 424, 425, 426, 428, 429, 431, 440, 444, 449, 450, 451, 494, 495, 496, 497, 498, 499 };
            int[] status_codes_5XX = { 500, 501, 502, 503, 504, 505, 506, 507, 508, 509, 510, 511, 520, 521, 522, 523, 524, 525, 526, 527, 530, 598, 599 };

            status_error = new Dictionary<int, StatusCodeSetting>(1000);

            foreach(int code in status_codes_0XX)
            {
                status_error[code] = new StatusCodeSetting();
            }

            foreach (int code in status_codes_1XX)
            {
                status_error[code] = new StatusCodeSetting();
            }

            foreach (int code in status_codes_2XX)
            {
                status_error[code] = new StatusCodeSetting();
            }

            foreach (int code in status_codes_3XX)
            {
                status_error[code] = new StatusCodeSetting();
            }

            foreach (int code in status_codes_4XX)
            {
                status_error[code] = new StatusCodeSetting();
            }

            foreach (int code in status_codes_5XX)
            {
                status_error[code] = new StatusCodeSetting();
            }
        }
    }

    public class
    StatusCodeSetting
    {
        /// <summary>
        /// How many times the request has been retried.
        /// </summary>
        public int retry_count { get; internal set; }

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
        public int retry_limit { get; set; }

        /// <summary>
        /// <para>How to handle when the maximum number of retries is reached.</para>
        /// <para>
        /// This setting is only valid when <see cref="handling"/> is set to <see cref="StatusHandling.Retry"/>.
        /// Otherwise, it is ignored.
        /// </para>
        /// <para>Default: <see cref="ErrorHandling.Error"/>.</para>
        /// </summary>
        public ErrorHandling handling_rety_limit_reached { get; set; }

        /// <summary>
        /// <para>How to handle errors with a specific status code.</para>
        /// <para>Default: <see cref="StatusHandling.Error"/>.</para>
        /// </summary>
        public StatusHandling handling { get; set; }

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
            retry_limit = 1;

            handling_rety_limit_reached = ErrorHandling.Error;
            handling = StatusHandling.Error;
        }
    }

    public enum
    ErrorHandling
    {
        Error = 0,

        Return
    }

    public enum
    StatusHandling
    {
        Error = 0,

        Return,

        Retry
    }
}
