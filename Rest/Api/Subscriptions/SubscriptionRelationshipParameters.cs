using System.Collections.Generic;

namespace
TwitchNet.Rest.Api.Tags
{
    public class
    SubscriptionRelationshipParameters
    {
        /// <summary>
        /// The user ID of a broadcaster.
        /// The user ID must match the user ID in the provided Bearer token.
        /// </summary>
        [QueryParameter("broadcaster_id")]
        public virtual string broadcaster_id { get; set; }

        [QueryParameter("user_id", typeof(SeparateQueryConverter))]
        public virtual List<string> user_id  { get; set; }

        public SubscriptionRelationshipParameters()
        {
            user_id = new List<string>();
        }
    }
}
