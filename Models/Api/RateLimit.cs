// standard namespaces
using System;
using System.Collections.Generic;

// project namespaces
using TwitchNet.Extensions;

// imported .dll's
using RestSharp;

namespace
TwitchNet.Models.Api
{
    public class
    RateLimit
    {
        #region Properties

        public ushort   limit       { get; protected set; }
        public ushort   remaining   { get; protected set; }

        public DateTime reset       { get; protected set; }

        #endregion

        #region Constructors

        public RateLimit(IRestResponse response)
        {
            object _limit           = GetHeader(response.Headers, "Ratelimit-Limit").Value;
            limit                   = Convert.ToUInt16(_limit);

            object _remaining       = GetHeader(response.Headers, "Ratelimit-Remaining").Value;
            remaining               = Convert.ToUInt16(_remaining);

            object _reset           = GetHeader(response.Headers, "Ratelimit-Reset").Value;
            double _reset_double    = Convert.ToDouble(_reset);
            reset                   = _reset_double.ToDateTimeFromUnixEpoch();
        }

        #endregion

        #region Methods

        private static Parameter GetHeader(IList<Parameter> headers, string name)
        {
            Parameter _header = null;

            foreach (Parameter header in headers)
            {
                if (header.Name == name)
                {
                    _header = header;

                    break;
                }
            }

            return _header;
        }

        #endregion
    }
}
