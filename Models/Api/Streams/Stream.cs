// standard namespaces
using System;
using System.Collections.Generic;

// project namespaces
using TwitchNet.Enums.Api.Streams;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Models.Api.Streams
{
    public class
    Stream
    {
        /// <summary>
        /// The id of the stream.
        /// </summary>
        [JsonProperty("id")]
        public string           id              { get; protected set; }

        /// <summary>
        /// The id of the user who is streaming.
        /// </summary>
        [JsonProperty("user_id")]
        public string           user_id         { get; protected set; }

        /// <summary>
        /// The id of the game being played.
        /// </summary>
        [JsonProperty("game_id")]
        public string           game_id         { get; protected set; }

        /// <summary>
        /// The community id's the user is part of.
        /// </summary>
        [JsonProperty("community_ids")]
        public List<string>     community_ids   { get; protected set; }

        /// <summary>
        /// The type of the stream, i.e., "live", "playlist", etc.
        /// </summary>
        [JsonProperty("type")]
        public StreamType       type            { get; protected set; }

        /// <summary>
        /// The title of the stream.
        /// </summary>
        [JsonProperty("title")]
        public string           title           { get; protected set; }

        /// <summary>
        /// The number of people watching the stream.
        /// </summary>
        [JsonProperty("viewer_count")]
        public uint             viewer_count    { get; protected set; }

        /// <summary>
        /// The time the stream went live.
        /// </summary>
        [JsonProperty("started_at")]
        public DateTime         started_at      { get; protected set; }

        /// <summary>
        /// The language of the stream.
        /// This is the language selected at the home page, not the language found in the Twitch dashboard.
        /// </summary>
        [JsonProperty("language")]
        public StreamLanguage   language        { get; protected set; }

        /// <summary>
        /// The template URL for the thumbnail of the stream.
        /// The {width} amd {height} parameters should be replaced with the desired values before navigating to the url.
        /// </summary>
        [JsonProperty("thumbnail_url")]
        public string           thumbnail_url   { get; protected set; }
    }
}
