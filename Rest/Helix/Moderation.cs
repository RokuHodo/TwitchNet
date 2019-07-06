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
    #region /moderation/banned

    public class
    BannedUsersParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// The cursor that tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [QueryParameter("before")]
        public string before { get; set; }

        /// <summary>
        /// The user ID of a broadcaster.
        /// The user ID must match the user ID in the provided Bearer token.
        /// </summary>
        [QueryParameter("broadcaster_id")]
        public virtual string broadcaster_id { get; set; }

        /// <summary>
        /// A list of user ID's who were banned, up to 100.
        /// </summary>
        [QueryParameter("user_id", typeof(SeparateQueryConverter))]
        public virtual List<string> user_ids { get; set; }

        public BannedUsersParameters()
        {
            user_ids = new List<string>();
        }
    }

    public class
    BannedUser
    {
        /// <summary>
        /// The ID of the banned user.
        /// </summary>
        [JsonProperty("user_id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// The user display name.
        /// </summary>
        [JsonProperty("user_name")]
        public string user_name { get; protected set; }

        // NOTE: This seems like the date they were banned in some cases, since sometimes this returnes dates that are not in the future?

        /// <summary>
        /// The date and time when the ban ends.
        /// Set to <see cref="DateTime.MinValue"/> if the ban is permanent.
        /// </summary>
        [JsonProperty("expires_at")]
        public DateTime expires_at { get; protected set; }
    }

    #endregion

    #region /moderation/banned/events

    public class
    BannedEventsParameters : PagingParameters, IPagingParameters
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
        /// A list of user ID's who were banned or unbanned, up to 100.
        /// </summary>
        [QueryParameter("user_id", typeof(SeparateQueryConverter))]
        public virtual List<string> user_ids { get; set; }

        public BannedEventsParameters()
        {
            user_ids = new List<string>();
        }
    }

    public class
    BannedEvent
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
        public BannedEventType event_type { get; protected set; }

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
        public BannedEventData event_data { get; protected set; }
    }

    [JsonConverter(typeof(EnumConverter))]
    public enum
    BannedEventType
    {
        /// <summary>
        /// Unknown or unsupported subscription event.
        /// </summary>
        Other = 0,

        /// <summary>
        /// A user was banned.
        /// </summary>
        [EnumMember(Value = "moderation.user.ban")]
        Ban,

        /// <summary>
        /// A user was unbanned.
        /// </summary>
        [EnumMember(Value = "moderation.user.unban")]
        Unban
    }

    public class
    BannedEventData
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
        /// The ID of the user who was banned or unbanned.
        /// </summary>
        [JsonProperty("user_id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// The user display name.
        /// </summary>
        [JsonProperty("user_name")]
        public string user_name { get; protected set; }

        /// <summary>
        /// The date and time when the ban ends.
        /// Set to <see cref="DateTime.MinValue"/> if the ban is permanent or the user was unbanned.
        /// </summary>
        [JsonProperty("expires_at")]
        public DateTime expires_at { get; protected set; }
    }

    #endregion

    #region /moderation/enforcements/status

    public class
    AutoModMessageStatusParameters
    {
        /// <summary>
        /// The user ID of a broadcaster.
        /// The user ID must match the user ID in the provided Bearer token.
        /// </summary>
        [QueryParameter("broadcaster_id")]
        public virtual string broadcaster_id { get; set; }

        /// <summary>
        /// A list of messages to check if they meet the AutoMod requirements, up to 100.
        /// </summary>
        [Body("data")]
        public virtual List<AutoModMessage> data { get; set; }

        public AutoModMessageStatusParameters()
        {
            data = new List<AutoModMessage>();
        }
    }

    public class
    AutoModMessage
    {
        [JsonProperty("msg_id")]
        public string msg_id { get; set; }

        [JsonProperty("msg_text")]
        public string msg_text { get; set; }

        [JsonProperty("user_id")]
        public string user_id { get; set; }
    }

    public class
    AutoModMessageStatus
    {
        [JsonProperty("msg_id")]
        public string msg_id { get; protected set; }

        [JsonProperty("is_permitted")]
        public bool is_permitted { get; protected set; }
    }

    #endregion

    #region /moderation/moderators

    public class
    ModeratorsParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// The user ID of a broadcaster.
        /// The user ID must match the user ID in the provided Bearer token.
        /// </summary>
        [QueryParameter("broadcaster_id")]
        public virtual string broadcaster_id { get; set; }

        /// <summary>
        /// A list of user ID's who were banned, up to 100.
        /// </summary>
        [QueryParameter("user_id", typeof(SeparateQueryConverter))]
        public virtual List<string> user_ids { get; set; }

        public ModeratorsParameters()
        {
            user_ids = new List<string>();
        }
    }

    public class
    Moderator
    {
        /// <summary>
        /// The ID of the banned user.
        /// </summary>
        [JsonProperty("user_id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// The user display name.
        /// </summary>
        [JsonProperty("user_name")]
        public string user_name { get; protected set; }
    }

    #endregion

    #region /moderation/moderators/events

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
        /// The event ID.
        /// </summary>
        [QueryParameter("id")]
        public virtual string id { get; set; }

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

    #endregion
}
