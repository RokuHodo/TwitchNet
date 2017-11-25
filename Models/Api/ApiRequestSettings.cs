// standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Enums;
using TwitchNet.Enums.Api;
using TwitchNet.Helpers;

namespace
TwitchNet.Models.Api
{
    public class ApiRequestSettings
    {
        #region Fields

        internal Dictionary<ushort, StatusHandlingSettings> _status_handlers_settings;

        // input checking
        internal InputHandling                              _input_hanlding;

        // internal error handling
        internal ErrorHandling                              _internal_error_handling;

        // default status handling
        internal ClampedNumber<short>                       _status_default_retry_limit;
        internal StatusHandling                             _status_default_handling;
        StatusHandlingSettings                              _status_default_hanlding_settings;

        // 429 status handling
        internal ClampedNumber<short>                       _status_429_retry_limit;
        internal StatusHandling                             _status_429_handling;
        StatusHandlingSettings                              _status_429_hanlding_settings;

        // 500 status handling
        internal ClampedNumber<short>                       _status_500_retry_limit;
        internal StatusHandling                             _status_500_handling;
        StatusHandlingSettings                              _status_500_hanlding_settings;

        // 503 status handling
        internal ClampedNumber<short>                       _status_503_retry_limit;
        internal StatusHandling                             _status_503_handling;
        StatusHandlingSettings                              _status_503_hanlding_settings;

        #endregion

        #region Properties        

        /// <summary>
        /// <para>Determine whether or not to checkand verify api inputs by the user.</para>
        /// <para>Default: <see cref="ErrorHandling.Error"/>.</para>
        /// </summary>
        public InputHandling input_hanlding
        {
            get
            {
                return _input_hanlding;
            }
            set
            {
                _input_hanlding = value;
            }
        }

        /// <summary>
        /// <para>Determine how to handle any exceptions that are encountered internally witin the library.</para>
        /// <para>Default: <see cref="ErrorHandling.Error"/>.</para>
        /// </summary>
        public ErrorHandling internal_error_handling
        {
            get
            {
                return _internal_error_handling;
            }
            set
            {
                _internal_error_handling = value;
            }
        }

        /// <summary>
        /// <para>
        /// Determine how to handle any error returned that is returned by the twitchg API.
        /// This is the fall back handling if an error is not already handled by another handler setting.
        /// </para>
        /// <para>Default: <see cref="ApiErrorHandling.Error"/>.</para>
        /// </summary>
        public StatusHandling api_error_handling
        {
            get
            {
                return _status_default_handling;
            }
            set
            {
                api_error_handling = value;
            }
        }

        /// <summary>
        /// <para>
        /// How many times to wait and retry making the request if status code '429 - Too Many Requests' is returned.
        /// When set to -1, the request will wait as many times as needed before completing and returning the request(s).
        /// If the limit is reached, any obtained data will be returned upon request cancellation.
        /// </para>
        /// <para>
        /// Min:        -1,
        /// Max:        5,
        /// Default:    -1.
        /// </para>
        /// </summary>
        public short status_429_wait_limit
        {
            get
            {
                return _status_429_retry_limit.value;
            }
            set
            {
                _status_429_retry_limit.value = value;
            }
        }

        /// <summary>
        /// <para>Determine how to handle the status code '429 - Too Many Requests'.</para>
        /// <para>Default: <see cref="Status429Handling.Wait"/>.</para>
        /// </summary>
        public StatusHandling status_429_handling
        {
            get
            {
                return _status_429_handling;
            }
            set
            {
                _status_429_handling = value;
            }
        }

        /// <summary>
        /// <para>How many times to retry making the request if status code '500 - Internal Server Error' is returned.
        /// When set to -1, the request will retry infinitely until the request(s) succeeds.
        /// If the limit is reached, any obtained data will be returned upon request cancellation.
        /// </para>
        /// <para>
        /// Min:        -1,
        /// Max:        1,
        /// Default:    1.
        /// </para>
        /// </summary>
        public short status_500_retry_limit
        {
            get
            {
                return _status_500_retry_limit.value;
            }
            set
            {
                _status_500_retry_limit.value = value;
            }
        }

        /// <summary>
        /// <para>Determine how to handle the status code '500 - Intenral Server Error'.</para>
        /// <para>Default: <see cref="Status500Handling.Error"/>.</para>
        /// </summary>
        public StatusHandling status_500_handling
        {
            get
            {
                return _status_500_handling;
            }
            set
            {
                _status_500_handling = value;
            }
        }

        /// <summary>
        /// <para>Determine how to handle the status code '503 - Service Unavailable'.</para>
        /// <para>Default: <see cref="StatusHandling.Retry"/>.</para>
        /// </summary>
        public StatusHandling status_503_handling
        {
            get
            {
                return _status_503_handling;
            }
            set
            {
                _status_503_handling = value;
            }
        }

        #endregion

        #region Constructors

        public ApiRequestSettings()
        {
            Default();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resets all API request settings back to their default values.
        /// </summary>
        public void
        Default()
        {
            _input_hanlding                     = InputHandling.Error;

            _internal_error_handling            = ErrorHandling.Error;

            _status_default_retry_limit         = new ClampedNumber<short>(1, 1, 1);
            _status_default_handling            = StatusHandling.Error;
            _status_default_hanlding_settings   = new StatusHandlingSettings(_status_default_retry_limit, _status_default_handling);

            _status_429_retry_limit             = new ClampedNumber<short>(-1, 5, -1);
            _status_429_handling                = StatusHandling.Retry;
            _status_429_hanlding_settings       = new StatusHandlingSettings(_status_429_retry_limit, _status_429_handling);

            _status_500_retry_limit             = new ClampedNumber<short>(-1, 1, 1);
            _status_500_handling                = StatusHandling.Error;
            _status_500_hanlding_settings       = new StatusHandlingSettings(_status_500_retry_limit, _status_500_handling);

            _status_503_retry_limit             = new ClampedNumber<short>(1, 1, 1);
            _status_503_handling                = StatusHandling.Retry;
            _status_503_hanlding_settings       = new StatusHandlingSettings(_status_503_retry_limit, _status_503_handling);

            _status_handlers_settings = new Dictionary<ushort, StatusHandlingSettings>();
            _status_handlers_settings.Add(000, _status_default_hanlding_settings);
            _status_handlers_settings.Add(429, _status_429_hanlding_settings);
            _status_handlers_settings.Add(500, _status_500_hanlding_settings);
            _status_handlers_settings.Add(503, _status_503_hanlding_settings);
        }

        #endregion
    }
}
