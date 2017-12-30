// standard namespaces
using System;

// project namespaces
using TwitchNet.Enums.Api.Streams;

// imported .dll's
using Newtonsoft.Json;

namespace TwitchNet.Models.Api.Clips
{
    public class
    Clip
    {
        /// <summary>
        /// The ID of the clip being queried.
        /// </summary>
        [JsonProperty("id")]
        public string           id              { get; protected set; }

        /// <summary>
        /// The URL where the clip can be viewed.
        /// </summary>
        [JsonProperty("url")]
        public string           url             { get; protected set; }

        /// <summary>
        /// The URL to use if the clip is being embedded.
        /// </summary>
        [JsonProperty("embed_url")]
        public string           embed_url       { get; protected set; }

        /// <summary>
        /// The user ID of the stream from which the clip was created.
        /// </summary>
        [JsonProperty("broadcaster_id")]
        public string           broadcaster_id  { get; protected set; }

        /// <summary>
        /// The ID of the user who created the clip.
        /// </summary>
        [JsonProperty("creator_id")]
        public string           creator_id      { get; protected set; }

        /// <summary>
        /// The ID of the video from which the clip was created.
        /// </summary>
        [JsonProperty("video_id")]
        public string           video_id        { get; protected set; }

        /// <summary>
        /// The ID of the game assigned being played when the clip was created.
        /// </summary>
        [JsonProperty("game_id")]
        public string           game_id         { get; protected set; }

        /// <summary>
        /// The language of the stream when clip was created.
        /// This is the language selected at the home page, not the language found in the Twitch dashboard.
        /// </summary>
        [JsonProperty("language")]
        public StreamLanguage   language        { get; protected set; }

        /// <summary>
        /// The title of the clip.
        /// </summary>
        [JsonProperty("title")]
        public string           title           { get; protected set; }

        /// <summary>
        /// The number of times the clip has been viewed.
        /// </summary>
        [JsonProperty("view_count")]
        public uint             view_count      { get; protected set; }

        /// <summary>
        /// The date when the clip was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime         created_at      { get; protected set; }

        /// <summary>
        /// The URL of the clip thumbnail.
        /// </summary>
        [JsonProperty("thumbnail_url")]
        public string           thumbnail_url   { get; protected set; }
    }
}
