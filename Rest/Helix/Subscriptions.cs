// standard nsamespaces
using System.Collections.Generic;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Helix
{
    public class
    SubscriptionParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// The user ID of a broadcaster.
        /// The user ID must match the user ID in the provided Bearer token.
        /// </summary>
        [QueryParameter("broadcaster_id")]
        public virtual string broadcaster_id { get; set; }
    }

    public class
    Subscription
    {
        /// <summary>
        /// The user ID of the broadcaster the user is subscribed to.
        /// </summary>
        [JsonProperty("broadcaster_id")]
        public string broadcaster_id { get; protected set; }

        /// <summary>
        /// The display name of the broadcaster the user is subscribed to.
        /// </summary>
        [JsonProperty("broadcaster_name")]
        public string broadcaster_name { get; protected set; }

        /// <summary>
        /// Whether or not the subscription was gifted.
        /// </summary>
        [JsonProperty("is_gift")]
        public bool is_gift { get; protected set; }

        /// <summary>
        /// The subscription tier.
        /// </summary>
        [JsonProperty("tier")]
        public SubscriptionTier tier { get; protected set; }

        /// <summary>
        /// The name of the subscription plan.
        /// </summary>
        [JsonProperty("plan_name")]
        public string plan_name { get; protected set; }

        /// <summary>
        /// The ID of the user who is subscribed to the broadcaster.
        /// </summary>
        [JsonProperty("user_id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// The display name of the user who is subscribed to the broadcaster.
        /// </summary>
        [JsonProperty("user_name")]
        public string user_name { get; protected set; }
    }

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
        public virtual List<string> user_id { get; set; }

        public SubscriptionRelationshipParameters()
        {
            user_id = new List<string>();
        }
    }
}
