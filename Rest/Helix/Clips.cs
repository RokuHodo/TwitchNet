// standard nsamespaces
using System;
using System.Collections.Generic;

// project namespaces

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Helix
{
    #region /clips

    public class
    CreateClipParameters
    {
        /// <summary>
        /// The user ID from which the clip will be made.
        /// </summary>
        [QueryParameter("broadcaster_id")]
        public virtual string broadcaster_id { get; set; }

        /// <summary>
        /// Whether or not a delay is added before the clip is created.
        /// </summary>
        [QueryParameter("has_delay")]
        public virtual bool? has_delay { get; set; }
    }

    public class
    CreatedClip
    {
        /// <summary>
        /// The ID of the clip that was created.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; protected set; }

        /// <summary>
        /// The URL of the edit page for the clip.
        /// </summary>
        [JsonProperty("edit_url")]
        public string edit_url { get; protected set; }
    }

    public class
    ClipsParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// The cursor that tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [QueryParameter("before")]
        public virtual string before { get; set; }

        /// <summary>
        /// <para>The user ID of a broadcaster.</para>
        /// <para>Only one or more clip ID, one broadcaster ID, or one game ID can be provided with each request.</para>
        /// </summary>
        [QueryParameter("broadcaster_id")]
        public virtual string broadcaster_id { get; set; }

        /// <summary>
        /// <para>The ID of a game.</para>
        /// <para>Only one or more clip ID, one broadcaster ID, or one game ID can be provided with each request.</para>
        /// </summary>
        [QueryParameter("game_id")]
        public virtual string game_id { get; set; }

        /// <summary>
        /// <para>
        /// A list of clip ID's, up to 100.
        /// All elements that are null, empty, or contain only whitespace are filtered out and all duplicate elements are removed before calculating the final count.
        /// All other optional parameters are ignored if video ID's are provited.
        /// </para>
        /// <para>Only one or more clip ID, one broadcaster ID, or one game ID can be provided with each request.</para>
        /// </summary>
        [QueryParameter("id", typeof(SeparateQueryConverter))]
        public virtual List<string> ids { get; set; }

        /// <summary>
        /// <para>
        /// The latest date/time that a clip will be returned.
        /// The resolved seconds are ignored.
        /// </para>
        /// <para>
        /// If specified, started_at must also be provided.
        /// If no started_at is specified, the time period is ignored.
        /// </para>
        /// </summary>
        [QueryParameter("ended_at", typeof(RFC3339QueryConverter))]
        public virtual DateTime? ended_at { get; set; }

        /// <summary>
        /// <para>
        /// The earliest date/time that a clip will be returned.
        /// The resolved seconds are ignored.
        /// </para>
        /// <para>
        /// If specified, ended_at should also be provided.
        /// If no ended_at is specified, the end_at defaults to 1 week after started_at.
        /// </para>
        /// </summary>
        [QueryParameter("started_at", typeof(RFC3339QueryConverter))]
        public virtual DateTime? started_at { get; set; }

        public ClipsParameters()
        {
            ids = new List<string>();
        }
    }

    public class
    Clip
    {
        /// <summary>
        /// The ID of the clip being queried.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; protected set; }

        /// <summary>
        /// The URL where the clip can be viewed.
        /// </summary>
        [JsonProperty("url")]
        public string url { get; protected set; }

        /// <summary>
        /// The URL to use if the clip is being embedded.
        /// </summary>
        [JsonProperty("embed_url")]
        public string embed_url { get; protected set; }

        /// <summary>
        /// The user ID of the stream from which the clip was created.
        /// </summary>
        [JsonProperty("broadcaster_id")]
        public string broadcaster_id { get; protected set; }

        /// <summary>
        /// The display name of the stream from which the clip was created.
        /// </summary>
        [JsonProperty("broadcaster_name")]
        public string broadcaster_name { get; protected set; }

        /// <summary>
        /// The user ID of the user who created the clip.
        /// </summary>
        [JsonProperty("creator_id")]
        public string creator_id { get; protected set; }

        /// <summary>
        /// The display name the user who created the clip.
        /// </summary>
        [JsonProperty("creator_name")]
        public string creator_name { get; protected set; }

        /// <summary>
        /// The ID of the video from which the clip was created.
        /// </summary>
        [JsonProperty("video_id")]
        public string video_id { get; protected set; }

        /// <summary>
        /// The ID of the game assigned being played when the clip was created.
        /// </summary>
        [JsonProperty("game_id")]
        public string game_id { get; protected set; }

        /// <summary>
        /// The language of the stream when clip was created.
        /// This is the language selected at the home page, not the language found in the Twitch dashboard.
        /// </summary>
        [JsonProperty("language")]
        public StreamLanguage language { get; protected set; }

        /// <summary>
        /// The title of the clip.
        /// </summary>
        [JsonProperty("title")]
        public string title { get; protected set; }

        /// <summary>
        /// The number of times the clip has been viewed.
        /// </summary>
        [JsonProperty("view_count")]
        public uint view_count { get; protected set; }

        /// <summary>
        /// The date when the clip was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime created_at { get; protected set; }

        /// <summary>
        /// The URL of the clip thumbnail.
        /// </summary>
        [JsonProperty("thumbnail_url")]
        public string thumbnail_url { get; protected set; }
    }

    #endregion
}
