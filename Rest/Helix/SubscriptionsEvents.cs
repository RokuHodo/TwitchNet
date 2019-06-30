// standard nsamespaces
using System;
using System.Runtime.Serialization;

// project namespaces 
using TwitchNet.Helpers.Json;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Helix
{
    public class
    SubscriptionEventsParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// The user ID of a broadcaster.
        /// The user ID must match the user ID in the provided Bearer token.
        /// </summary>
        [QueryParameter("broadcaster_id")]
        public virtual string broadcaster_id { get; set; }

        /// <summary>
        /// The event ID.
        /// </summary>
        [QueryParameter("id")]
        public virtual string id { get; set; }

        /// <summary>
        /// <para>The Id of the subscribed user.</para>
        /// <para>If this is provided, any initial pagination that is provided is ignored.</para>
        /// </summary>
        [QueryParameter("user_id")]
        public virtual string user_id { get; set; }
    }

    public class
    SubscriptionEvent
    {
        /// <summary>
        /// The event ID.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; protected set; }

        /// <summary>
        /// The type of the event.
        /// </summary>
        [JsonProperty("event_type")]
        public SubscriptionEventType event_type { get; protected set; }

        /// <summary>
        /// The date and time when the event occured.
        /// </summary>
        [JsonProperty("event_timestamp")]
        public DateTime event_timestamp { get; protected set; }

        /// <summary>
        /// The event version.
        /// </summary>
        [JsonProperty("version")]
        public string version { get; protected set; }

        /// <summary>
        /// The event data.
        /// </summary>
        [JsonProperty("event_data")]
        public SubscriptionEventData event_data { get; protected set; }
    }

    [JsonConverter(typeof(EnumConverter))]
    public enum
    SubscriptionEventType
    {
        /// <summary>
        /// Unknown or unsupported subscription event.
        /// </summary>
        Other = 0,

        /// <summary>
        /// A user subscribed and their payment has processed.
        /// </summary>
        [EnumMember(Value ="subscriptions.subscribe")]
        Subscribe,

        /// <summary>
        /// A user unsubscribe or their subscription ended.
        /// </summary>
        [EnumMember(Value = "subscriptions.unsubscribe")]
        Unsubscribe,

        /// <summary>
        /// A suscribed user sends a subscription notification message in the broadcaster's chat.
        /// </summary>
        [EnumMember(Value = "subscriptions.notification")]
        Notification
    }

    public class
    SubscriptionEventData
    {
        /// <summary>
        /// The user ID of a broadcaster.
        /// </summary>
        [JsonProperty("broadcaster_id")]
        public string broadcaster_id { get; protected set; }

        /// <summary>
        /// The broadcaster display name.
        /// </summary>
        [JsonProperty("broadcaster_name")]
        public string broadcaster_name { get; protected set; }

        /// <summary>
        /// Whether or not the subscription was gifted.
        /// </summary>
        [JsonProperty("is_gift")]
        public bool is_gift { get; protected set; }

        /// <summary>
        /// The subscription plan name.
        /// </summary>
        [JsonProperty("plan_name")]
        public string plan_name { get; protected set; }

        /// <summary>
        /// The subscription tier.
        /// </summary>
        [JsonProperty("tier")]
        public SubscriptionTier tier { get; protected set; }

        /// <summary>
        /// The ID of the user who triggered the event.
        /// </summary>
        [JsonProperty("user_id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// The user display name.
        /// </summary>
        [JsonProperty("user_name")]
        public string user_name { get; protected set; }

        /// <summary>
        /// The message typed in chat for a notification event.
        /// </summary>
        [JsonProperty("message")]
        public string message { get; protected set; }
    }
}
