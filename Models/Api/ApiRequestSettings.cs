// project namespaces
using TwitchNet.Enums.Api;
using TwitchNet.Helpers;

namespace TwitchNet.Models.Api
{
    public class ApiRequestSettings
    {
        #region Fields

        internal ushort                         _too_many_request_retry_count       = 0;
        internal ushort                         _internal_server_error_retry_count  = 0;

        internal ClampedNumber<short>           _too_many_request_retry_limit       = new ClampedNumber<short>(-1, 3, -1);
        internal ClampedNumber<short>           _internal_server_error_retry_limit  = new ClampedNumber<short>(-1, 3, 1);

        internal TooManyRequestHandling         _too_many_request_handling          = TooManyRequestHandling.Wait;
        internal InternalServerErrorHandling    _internal_server_error_handling     = InternalServerErrorHandling.Ignore;

        #endregion

        #region Properties        

        /// <summary>
        /// <para>
        /// How many times to retry making the request if status code '429' - Too Many Requests is returned.
        /// When set to -1, the request will wait as many times as needed before completing and returning the request(s).
        /// If the limit is reached, any obtained data will be returned upon request cancellation.
        /// </para>
        /// <para>
        /// Min:        -1,
        /// Max:        3,
        /// Default:    -1.
        /// </para>
        /// </summary>
        public short too_many_request_retry_limit
        {
            get
            {
                return _too_many_request_retry_limit.value;
            }
            set
            {
                _too_many_request_retry_limit.value = value;
            }
        }

        /// <summary>
        /// Determine how to handle the status code '429' - Too Many Requests
        /// </summary>
        public TooManyRequestHandling too_many_request_handling
        {
            get
            {
                return _too_many_request_handling;
            }
            set
            {
                _too_many_request_handling = value;
            }
        }

        /// <summary>
        /// <para>How many times to retry making the request if status code '500' - Internal Server Error is returned.
        /// When set to -1, the request will retry infinitely until the request(s) succeeds.
        /// If the limit is reached, any obtained data will be returned upon request cancellation.
        /// </para>
        /// <para>
        /// Min:        -1,
        /// Max:        3,
        /// Default:    1.
        /// </para>
        /// </summary>
        public short internal_server_error_retry_limit
        {
            get
            {
                return _internal_server_error_retry_limit.value;
            }
            set
            {
                _internal_server_error_retry_limit.value = value;
            }
        }

        /// <summary>
        /// Determine how to handle the status code '500' - Intenral Server Error
        /// </summary>
        public InternalServerErrorHandling internal_server_error_handling
        {
            get
            {
                return _internal_server_error_handling;
            }
            set
            {
                _internal_server_error_handling = value;
            }
        }

        #endregion
    }
}
