// standard namespaces
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
    #region /videos

    public class
    VideosParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// The cursor that tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [QueryParameter("before")]
        public virtual string               before      { get; set; }

        /// <summary>
        /// <para>The ID of a user.</para>
        /// <para>Only one or more video ID, one user ID, or one game ID can be provided with each request.</para>
        /// </summary>
        [QueryParameter("user_id")]
        public virtual string               user_id     { get; set; }

        /// <summary>
        /// The ID of a game.
        /// <para>Only one or more video ID, one user ID, or one game ID can be provided with each request.</para>
        /// </summary>
        [QueryParameter("game_id")]
        public virtual string               game_id     { get; set; }

        /// <summary>
        /// <para>
        /// A list of video ID's, up to 100.
        /// All other optional parameters are ignored if video ID's are provited.
        /// </para>
        /// <para>Only one or more video ID, one user ID, or one game ID can be provided with each request.</para>       
        /// </summary>
        [QueryParameter("id", typeof(SeparateQueryConverter))]
        public virtual List<string>         ids         { get; set; }

        /// <summary>
        /// The language of the video.
        /// </summary>
        [QueryParameter("language")]
        public virtual Language?            language    { get; set; }

        /// <summary>
        /// The period when the video was created.
        /// </summary>
        [QueryParameter("period")]
        public virtual VideoPeriod?         period      { get; set; }

        /// <summary>
        /// The soprt order of the videos.
        /// </summary>
        [QueryParameter("sort")]
        public virtual VideoSort?           sort        { get; set; }

        /// <summary>
        /// The type of the video.
        /// </summary>
        [QueryParameter("type")]
        public virtual VideoType?           type        { get; set; }

        public VideosParameters()
        {
            ids = new List<string>();
        }
    }

    public class
    Video
    {
        /// <summary>
        /// The id of the video.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; protected set; }

        /// <summary>
        /// The id of the user who owns the video.
        /// </summary>
        [JsonProperty("user_id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// The display name of the user who owns the video.
        /// </summary>
        [JsonProperty("user_name")]
        public string user_name { get; protected set; }

        /// <summary>
        /// The title of the video.
        /// </summary>
        [JsonProperty("title")]
        public string title { get; protected set; }

        /// <summary>
        /// The description of the video.
        /// </summary>
        [JsonProperty("description")]
        public string description { get; protected set; }

        /// <summary>
        /// The url of the video.
        /// </summary>
        [JsonProperty("url")]
        public string url { get; protected set; }

        /// <summary>
        /// The date when the video was created
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime created_at { get; protected set; }

        /// <summary>
        /// The date when the video was published.
        /// </summary>
        [JsonProperty("published_at")]
        public DateTime published_at { get; protected set; }

        /// <summary>
        /// The template URL for the thumbnail of the video.
        /// The {width} amd {height} parameters should be replaced with the desired values before navigating to the url.
        /// </summary>
        [JsonProperty("thumbnail_url")]
        public string thumbnail_url { get; protected set; }

        /// <summary>
        /// The viewable state of the video.
        /// </summary>
        [JsonProperty("viewable")]
        public string viewable { get; protected set; }

        /// <summary>
        /// The number of times the video has been viewed.
        /// </summary>
        [JsonProperty("view_count")]
        public uint view_count { get; protected set; }

        /// <summary>
        /// The language of the video.
        /// </summary>
        [JsonProperty("language")]
        public Language language { get; protected set; }

        /// <summary>
        /// The type of the video.
        /// </summary>
        [JsonProperty("type")]
        public VideoType type { get; protected set; }

        /// <summary>
        /// The duration of the video.
        /// </summary>
        [JsonProperty("duration")]
        [JsonConverter(typeof(VideoDurationConverter))]
        public TimeSpan duration { get; protected set; }
    }

    [JsonConverter(typeof(EnumConverter))]
    public enum
    VideoPeriod
    {
        /// <summary>
        /// Returns all videos, regardless of when they were created.
        /// </summary>
        [EnumMember(Value = "all")]
        All = 0,

        /// <summary>
        /// Returns videos that were created today.
        /// </summary>
        [EnumMember(Value = "day")]
        Day,

        /// <summary>
        /// Returns videos that were created this week.
        /// </summary>
        [EnumMember(Value = "week")]
        Week,

        /// <summary>
        /// Returns videos that were created this month.
        /// </summary>
        [EnumMember(Value = "month")]
        Month
    }

    [JsonConverter(typeof(EnumConverter))]
    public enum
    VideoSort
    {
        /// <summary>
        /// Returns videos sorted by time, newest to oldest.
        /// </summary>
        [EnumMember(Value = "time")]
        Time = 0,

        /// <summary>
        /// Returns videos sorted by which ones are trending.
        /// </summary>
        [EnumMember(Value = "trending")]
        Trending,

        /// <summary>
        /// Returns videos sorted by view count, highest to lowest.
        /// </summary>
        [EnumMember(Value = "views")]
        Views
    }

    [JsonConverter(typeof(EnumConverter))]
    public enum
    VideoType
    {
        /// <summary>
        /// Returns uploaded videos.
        /// </summary>
        [EnumMember(Value = "upload")]
        Upload,

        /// <summary>
        /// Returns archived videos.
        /// </summary>
        [EnumMember(Value = "archive")]
        Archive,

        /// <summary>
        /// Returns highlighted videos.
        /// </summary>
        [EnumMember(Value = "highlight")]
        Highlight,

        /// <summary>
        /// Returns all videos.
        /// </summary>
        [EnumMember(Value = "all")]
        All
    }

    #endregion
}