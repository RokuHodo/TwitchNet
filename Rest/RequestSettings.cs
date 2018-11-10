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
}
