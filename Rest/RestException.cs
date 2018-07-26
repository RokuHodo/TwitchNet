using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TwitchNet.Extensions;

namespace TwitchNet.Rest
{
    public class
    RestException : Exception
    {
        /// <summary>
        /// The error associated with the status code, i.e., the status description.
        /// </summary>
        public string rest_error { get; protected set; }

        /// <summary>
        /// The HTTP status code of the returned response.
        /// </summary>
        public ushort rest_status { get; protected set; }

        /// <summary>
        /// The descriptive error message that gives more detailed information on the type of error.
        /// </summary>
        public string rest_message { get; protected set; }

        public RestException()
        {

        }

        public RestException(string message) : base(message)
        {

        }

        public RestException(RestError error)
        {
            if (error.IsNull())
            {
                return;
            }

            rest_error = error.error;
            rest_status = error.status;
            rest_message = error.message;
        }

        public RestException(string message, RestError error) : base(message)
        {
            if (error.IsNull())
            {
                return;
            }

            rest_error      = error.error;
            rest_status     = error.status;
            rest_message    = error.message;
        }
    }
}
