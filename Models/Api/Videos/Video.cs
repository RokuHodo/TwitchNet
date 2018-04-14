// standard namespaces
using System;

// project namespaces
using TwitchNet.Enums.Api.Videos;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Models.Api.Videos
{
    public class
    Video
    {
        /// <summary>
        /// The id of the video.
        /// </summary>
        [JsonProperty("id")]
        public string           id              { get; protected set; }

        /// <summary>
        /// The id of the user.
        /// </summary>
        [JsonProperty("user_id")]
        public string           user_id         { get; protected set; }

        /// <summary>
        /// The title of the video.
        /// </summary>
        [JsonProperty("title")]
        public string           title           { get; protected set; }

        /// <summary>
        /// The description of the video.
        /// </summary>
        [JsonProperty("description")]
        public string           description     { get; protected set; }

        /// <summary>
        /// The url of the video.
        /// </summary>
        [JsonProperty("url")]
        public string           url             { get; protected set; }

        /// <summary>
        /// The date when the video was created
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime         created_at      { get ; protected set; }

        /// <summary>
        /// The date when the video was published.
        /// </summary>
        [JsonProperty("published_at")]
        public DateTime         published_at    { get; protected set; }

        /// <summary>
        /// The template URL for the thumbnail of the video.
        /// The {width} amd {height} parameters should be replaced with the desired values before navigating to the url.
        /// </summary>
        [JsonProperty("thumbnail_url")]
        public string           thumbnail_url   { get; protected set; }

        /// <summary>
        /// The vieable state of the video.
        /// </summary>
        [JsonProperty("viewable")]
        public string           viewable        { get; protected set; }

        /// <summary>
        /// The number of times the video has been viewed.
        /// </summary>
        [JsonProperty("view_count")]
        public uint             view_count      { get; protected set; }

        /// <summary>
        /// The language of the video.
        /// This is the language selected in the Twitch dashboard or in the video information editor, not the language selected at the home page.
        /// </summary>
        [JsonProperty("language")]
        public BroadcasterLanguage    language        { get; protected set; }

        /// <summary>
        /// The type of the video.
        /// </summary>
        [JsonProperty("type")]
        public VideoType        type            { get; protected set; }

        /// <summary>
        /// The duration of the video.
        /// </summary>
        [JsonProperty("duration")]
        public TimeSpan           duration        { get; protected set; }
    }
}
