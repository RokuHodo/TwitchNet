// standard namespaces
using System.Collections.Generic;

// project namespaces
using TwitchNet.Enums;
using TwitchNet.Enums.Api;
using TwitchNet.Helpers;

namespace
TwitchNet.Models.Api
{
    public class
    ApiRequestSettings
    {
        #region Fields

        internal Dictionary<ushort, StatusHandlingSettings> _status_handlers_settings;

        // input checking
        private InputHandling                               _input_hanlding;

        // internal error handling
        private ErrorHandling                               _internal_error_handling;

        // default status handling
        private ClampedNumber<short>                        _status_default_retry_limit;
        private StatusHandling                              _status_default_handling;
        private StatusHandlingSettings                      _status_default_hanlding_settings;

        // 429 status handling
        private ClampedNumber<short>                        _status_429_retry_limit;
        private StatusHandling                              _status_429_handling;
        private StatusHandlingSettings                      _status_429_hanlding_settings;

        // 500 status handling
        private ClampedNumber<short>                        _status_500_retry_limit;
        private StatusHandling                              _status_500_handling;
        private StatusHandlingSettings                      _status_500_hanlding_settings;

        // 503 status handling
        private ClampedNumber<short>                        _status_503_retry_limit;
        private StatusHandling                              _status_503_handling;
        private StatusHandlingSettings                      _status_503_hanlding_settings;

        #endregion

        #region Properties        

        /// <summary>
        /// <para>Determine whether or not to check and verify inputs by the user upon request execution.</para>
        /// <para>Default: <see cref="InputHandling.Error"/>.</para>
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
        /// Determine how to handle a status code error that does not already have specific handling settings.
        /// </para>
        /// <para>Default: <see cref="StatusHandling.Error"/>.</para>
        /// </summary>
        public StatusHandling status_default_handling
        {
            get
            {
                return _status_default_handling;
            }
            set
            {
                status_default_handling = value;
            }
        }

        /// <summary>
        /// <para>
        /// How many times to wait and retry making the request if status code '429 - Too Many Requests' is returned.
        /// When set to -1, the request will retry infinitely until the request(s) succeeds/cpmpletes.
        /// If the limit is reached, any obtained data will be returned upon request cancellation.
        /// </para>
        /// <para>
        /// Min:        -1,
        /// Max:        1,
        /// Default:    1.
        /// The retry limit is clamped between the minimum and the maximum values.
        /// </para>
        /// </summary>
        public short status_429_retry_limit
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
        /// <para>Default: <see cref="StatusHandling.Error"/>.</para>
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
        /// <para>
        /// How many times to retry making the request if status code '500 - Internal Server Error' is returned.
        /// When set to -1, the request will retry infinitely until the request(s) succeeds.
        /// If the limit is reached, any obtained data will be returned upon request cancellation.        
        /// </para>
        /// <para>
        /// Min:        -1,
        /// Max:        1,
        /// Default:    1.
        /// The retry limit is clamped between the minimum and the maximum values.
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
        /// <para>Default: <see cref="StatusHandling.Error"/>.</para>
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
        /// <para>Default: <see cref="StatusHandling.Error"/>.</para>
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

            // TODO: change default values in documentation
            _status_429_retry_limit             = new ClampedNumber<short>(-1, 1, 1);
            _status_429_handling                = StatusHandling.Error;
            _status_429_hanlding_settings       = new StatusHandlingSettings(_status_429_retry_limit, _status_429_handling);

            // TODO: change default values in documentation
            _status_500_retry_limit = new ClampedNumber<short>(-1, 1, 1);
            _status_500_handling                = StatusHandling.Error;
            _status_500_hanlding_settings       = new StatusHandlingSettings(_status_500_retry_limit, _status_500_handling);

            // TODO: change default values in documentation
            _status_503_retry_limit = new ClampedNumber<short>(1, 1, 1);
            _status_503_handling                = StatusHandling.Error;
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
