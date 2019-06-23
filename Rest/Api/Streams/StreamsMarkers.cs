// standard nsamespaces
using System;
using System.Collections.Generic;

// imported .dll's
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Streams
{
    [Body]
    public class
    CreateStreamMarkerParameters
    {
        /// <summary>
        /// The ID of the user who is stremaing.
        /// </summary>
        [JsonProperty("user_id")]
        public virtual string user_id { get; set; }

        /// <summary>
        /// The marker description.
        /// </summary>
        [JsonProperty("description")]
        public virtual string description { get; set; }
    }

    public class
    CreatedStreamMarker
    {
        /// <summary>
        /// The ID of stream marker.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; protected set; }

        /// <summary>
        /// The date when the marker was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime created_at { get; protected set; }

        /// <summary>
        /// The description of the stream marker if one was provided when it was created.
        /// </summary>
        [JsonProperty("description")]
        public string description { get; protected set; }

        /// <summary>
        /// How far into the stream the marker was created.
        /// </summary>
        [JsonProperty("position_seconds")]
        public uint position_seconds { get; protected set; }
    }

    public class
    StreamMarkersParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// <para>The ID of the user who is stremaing/owns the video to get the markers for.</para>
        /// <para>Only one user ID or video ID can be provided at one time.</para>
        /// </summary>
        [QueryParameter("user_id")]
        public virtual string user_id { get; set; }

        /// <summary>
        /// <para>The ID of the video to get the markers from.</para>
        /// <para>Only one user ID or video ID can be provided at one time.</para>
        /// </summary>
        [QueryParameter("video_id")]
        public virtual string video_id { get; set; }

        // NOTE: /streams/markers - StreamMarkersParameters - 'before' is not included because Twitch returns an eror that it is not supported.
        /// <summary>
        /// The cursor that tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        // [QueryParameter("before")]
        // public string before { get; set; }
    }

    public class
    StreamMarkers
    {
        /// <summary>
        /// The ID of the user who is streaming/owns the video.
        /// </summary>
        [JsonProperty("user_id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// The display of the user who is streaming/owns the video.
        /// </summary>
        [JsonProperty("user_name")]
        public string user_name { get; protected set; }

        [JsonProperty("videos")]
        public List<MarkedVideos> videos { get; protected set; }
    }

    public class
    MarkedVideos
    {
        /// <summary>
        /// The ID of the video that contians the marker.
        /// </summary>
        [JsonProperty("video_id")]
        public string video_id { get; protected set; }


        [JsonProperty("markers")]
        public List<StreamMarker> markers { get; protected set; }
    }

    public class
    StreamMarker
    {
        /// <summary>
        /// The ID of stream marker.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; protected set; }

        /// <summary>
        /// The date when the marker was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime created_at { get; protected set; }

        /// <summary>
        /// The description of the stream marker if one was provided when it was created.
        /// </summary>
        [JsonProperty("description")]
        public string description { get; protected set; }

        /// <summary>
        /// How far into the stream the marker was created.
        /// </summary>
        [JsonProperty("position_seconds")]
        public uint position_seconds { get; protected set; }

        /// <summary>
        /// The URL to the video with the included marker time stamp query parameter.
        /// </summary>
        [JsonProperty("URL")]
        public string URL { get; protected set; }
    }
}