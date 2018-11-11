using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace TwitchNet.Rest.Api
{
    public class
    HelixException : Exception
    {
        /// <summary>
        /// The HTTP status code of the returned response.
        /// </summary>
        public int      status_code     { get; protected set; }

        /// <summary>
        /// The error associated with the status code, i.e., the status description.
        /// </summary>
        public string   helix_error     { get; protected set; }

        /// <summary>
        /// The descriptive error message that gives more detailed information on the type of error.
        /// </summary>
        public string   helix_message   { get; protected set; }

        public HelixException(string message, string content, Exception inner_exception) : base(message, inner_exception)
        {
            HelixError _error = JsonConvert.DeserializeObject<HelixError>(content);

            status_code     = _error.status;
            helix_error     = _error.error;
            helix_message   = _error.message;
        }
    }
}
