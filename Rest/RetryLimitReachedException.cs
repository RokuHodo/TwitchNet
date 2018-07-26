// standard namespaces
using System;

namespace TwitchNet.Rest
{
    public class
    RetryLimitReachedException : Exception
    {
        public int retry_limit { get; protected set; }

        public RetryLimitReachedException()
        {

        }

        public RetryLimitReachedException(string message) : base(message)
        {

        }

        public RetryLimitReachedException(int retry_limit)
        {
            this.retry_limit = retry_limit;
        }

        public RetryLimitReachedException(string message, int retry_limit) : base(message)
        {
            this.retry_limit = retry_limit;
        }
    }
}
