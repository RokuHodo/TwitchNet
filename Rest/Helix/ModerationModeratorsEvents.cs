// standard nsamespaces
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// project namespaces 
using TwitchNet.Helpers.Json;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Helix
{
    public class
    ModeratorEventsParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// The user ID of a broadcaster.
        /// The user ID must match the user ID in the provided Bearer token.
        /// </summary>
        [QueryParameter("broadcaster_id")]
        public virtual string broadcaster_id { get; set; }

        /// <summary>
        /// A list of user ID's who were given or lost moderator (OP) status, up to 100.
        /// </summary>
        [QueryParameter("user_id", typeof(SeparateQueryConverter))]
        public virtual List<string> user_ids { get; set; }

        public ModeratorEventsParameters()
        {
            user_ids = new List<string>();
        }
    }

    public class
    ModeratorEvent
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
        public ModeratorEventType event_type { get; protected set; }

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
        public ModeratorEventData event_data { get; protected set; }
    }

    [JsonConverter(typeof(EnumConverter))]
    public enum
    ModeratorEventType
    {
        /// <summary>
        /// Unknown or unsupported subscription event.
        /// </summary>
        Other = 0,

        /// <summary>
        /// A user lost moderation (OP) status.
        /// </summary>
        [EnumMember(Value = "moderation.moderator.remove")]
        Remove,

        /// <summary>
        /// A user gained moderation (OP) status.
        /// </summary>
        [EnumMember(Value = "moderation.moderator.add")]
        Add
    }

    public class
    ModeratorEventData
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
        /// The ID of the user who gained or lost moderator (OP) status.
        /// </summary>
        [JsonProperty("user_id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// The user display name.
        /// </summary>
        [JsonProperty("user_name")]
        public string user_name { get; protected set; }
    }
}
