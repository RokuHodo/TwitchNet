// standard namespaces
using System.Collections.Generic;

namespace
TwitchNet.Rest.Api.Tags
{
    public class
    BroadcasterSubscribersParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// The user ID of a broadcaster.
        /// The user ID must match the user ID in the provided Bearer token.
        /// </summary>
        [QueryParameter("broadcaster_id")]
        public virtual string broadcaster_id { get; set; }
    }
}
