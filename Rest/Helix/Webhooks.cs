// standard nsamespaces
using System;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Helix
{
    #region /webhooks/subscriptions

    public class WebhookDataPage<data_type> : DataPage<data_type>
    {
        /// <summary>
        /// The expected number of pages to be returned.
        /// This is only an approximation and could change if subscriptions are added or expire while paging through the results.
        /// </summary>
        [JsonProperty("total")]
        public int total { get; protected set; }
    }

    public class
    WebhookSubscription
    {
        /// <summary>
        /// The callback URL for the subscription.
        /// </summary>
        [JsonProperty("callback")]
        public string callback { get; protected set; }

        /// <summary>
        /// When the subscription expires.
        /// </summary>
        [JsonProperty("expires_at")]
        public DateTime expires_at { get; protected set; }

        /// <summary>
        /// The topic that was subscribed to.
        /// </summary>
        [JsonProperty("topic")]
        public string topic { get; protected set; }        
    }

    #endregion
}
