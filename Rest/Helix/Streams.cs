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
    #region /streams

    public class
    StreamsParameters : PagingParameters, IPagingParameters
    {
        /// <summary>
        /// A list of game ID's to query, up to 100.
        /// </summary>
        [QueryParameter("game_id", typeof(SeparateQueryConverter))]
        public virtual List<string> game_ids { get; set; }

        /// <summary>
        /// The language of the stream.
        /// Bitfield enum.
        /// </summary>
        [QueryParameter("language", typeof(SeparateQueryConverter))]
        public virtual StreamLanguage? language { get; set; }

        /// <summary>
        /// A list of user ID's to query, up to 100.
        /// </summary>
        [QueryParameter("user_id", typeof(SeparateQueryConverter))]
        public virtual List<string> user_ids { get; set; }

        /// <summary>
        /// A list of user login names to query, up to 100.
        /// </summary>
        [QueryParameter("user_login", typeof(SeparateQueryConverter))]
        public virtual List<string> user_logins { get; set; }

        /// <summary>
        /// The cursor that tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        [QueryParameter("before")]
        public string before { get; set; }

        public StreamsParameters()
        {
            game_ids = new List<string>();
            user_ids = new List<string>();
            user_logins = new List<string>();
        }
    }

    public class
    Stream
    {
        /// <summary>
        /// The id of the stream.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; protected set; }

        /// <summary>
        /// The id of the user who is streaming.
        /// </summary>
        [JsonProperty("user_id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// The name of the user who is streaming.
        /// </summary>
        [JsonProperty("user_name")]
        public string user_name { get; protected set; }

        /// <summary>
        /// The id of the game being played.
        /// </summary>
        [JsonProperty("game_id")]
        public string game_id { get; protected set; }

        /// <summary>
        /// The type of the stream, i.e., "live", "playlist", etc.
        /// </summary>
        [JsonProperty("type")]
        public StreamType type { get; protected set; }

        /// <summary>
        /// The title of the stream.
        /// </summary>
        [JsonProperty("title")]
        public string title { get; protected set; }

        /// <summary>
        /// The number of people watching the stream.
        /// </summary>
        [JsonProperty("viewer_count")]
        public uint viewer_count { get; protected set; }

        /// <summary>
        /// The time the stream went live.
        /// </summary>
        [JsonProperty("started_at")]
        public DateTime started_at { get; protected set; }

        /// <summary>
        /// The language of the stream.
        /// This is the language selected at the home page, not the language found in the Twitch dashboard.
        /// </summary>
        [JsonProperty("language")]
        public StreamLanguage language { get; protected set; }

        /// <summary>
        /// The template URL for the thumbnail of the stream.
        /// The {width} amd {height} parameters should be replaced with the desired values before navigating to the url.
        /// </summary>
        [JsonProperty("thumbnail_url")]
        public string thumbnail_url { get; protected set; }

        /// <summary>
        /// The stream tags the broadcaster has selected.
        /// </summary>
        [JsonProperty("tag_ids")]
        public List<string> tag_ids { get; protected set; }
    }

    [Flags]
    [JsonConverter(typeof(EnumConverter))]
    public enum
    StreamType
    {
        /// <summary>
        /// The stream is not live or a vodcast.
        /// </summary>
        [EnumMember(Value = "")]
        Other = 0,

        /// <summary>
        /// The stream is live.
        /// </summary>
        [EnumMember(Value = "live")]
        Live = 1 << 0,

        /// <summary>
        /// The stream is a rebroadcast of a past stream.
        /// </summary>
        [EnumMember(Value = "vodcast")]
        Vodcast = 1 << 1,

        /// <summary>
        /// Specifies to return all stream types and is only applicable when providing query parameters.
        /// </summary>
        [EnumMember(Value = "all")]
        All = Live | Vodcast
    }

    /// <summary>
    /// The language selected at the home page, not the language found in the Twitch dashboard or in the video information editor.
    /// </summary>
    [Flags]
    [JsonConverter(typeof(EnumConverter))]
    public enum
    StreamLanguage : ulong
    {
        /// <summary>
        /// Unsupported stream language.
        /// </summary>
        [EnumMember(Value = "")]
        None = 0,

        /// <summary>
        /// Dansk (Danish)
        /// </summary>
        [EnumMember(Value = "da")]
        Da = 1 << 0,

        /// <summary>
        /// Deutsch (German)
        /// </summary>
        [EnumMember(Value = "de")]
        De = 1 << 1,

        /// <summary>
        /// English (English)
        /// </summary>
        [EnumMember(Value = "en")]
        En = 1 << 2,

        /// <summary>
        /// English - UK (English - UK)
        /// </summary>
        [EnumMember(Value = "en-gb")]
        EnGb = 1 << 3,

        /// <summary>
        /// Español - España (Spanish - Spain)
        /// </summary>
        [EnumMember(Value = "es")]
        Es = 1 << 4,

        /// <summary>
        /// Español - Latinoamérica (Spanish - Latin America)
        /// </summary>
        [EnumMember(Value = "es-mx")]
        EsMx = 1 << 5,

        /// <summary>
        /// Français (French)
        /// </summary>
        [EnumMember(Value = "fr")]
        Fr = 1 << 6,

        /// <summary>
        /// Italiano (Italian)
        /// </summary>
        [EnumMember(Value = "it")]
        It = 1 << 7,

        /// <summary>
        /// Magyar (Hungarian)
        /// </summary>
        [EnumMember(Value = "hu")]
        Hu = 1 << 8,

        /// <summary>
        /// Nederlands (Netherland)
        /// </summary>
        [EnumMember(Value = "nl")]
        Nl = 1 << 9,

        /// <summary>
        /// Norsk (Norwegian)
        /// </summary>
        [EnumMember(Value = "no")]
        No = 1 << 10,

        /// <summary>
        /// Polski (Polish)
        /// </summary>
        [EnumMember(Value = "pl")]
        Pl = 1 << 11,

        /// <summary>
        /// Português (Portuguese)
        /// </summary>
        [EnumMember(Value = "pt")]
        Pt = 1 << 12,

        /// <summary>
        /// Português - Brasil (Portuguese - Brazil)
        /// </summary>
        [EnumMember(Value = "pt-br")]
        PtBr = 1 << 13,

        /// <summary>
        /// Slovenčina (Slovak)
        /// </summary>
        [EnumMember(Value = "sk")]
        Sk = 1 << 14,

        /// <summary>
        /// Suomi (Finish)
        /// </summary>
        [EnumMember(Value = "fi")]
        Fi = 1 << 15,

        /// <summary>
        /// Svenska (Swedish)
        /// </summary>
        [EnumMember(Value = "sv")]
        Sv = 1 << 16,

        /// <summary>
        /// Tiếng Việt (Vietnamese)
        /// </summary>
        [EnumMember(Value = "vi")]
        Vi = 1 << 17,

        /// <summary>
        /// Türkçe (Turkish)
        /// </summary>
        [EnumMember(Value = "tr")]
        Tr = 1 << 18,

        /// <summary>
        /// Čeština (Czech)
        /// </summary>
        [EnumMember(Value = "cs")]
        Cs = 1 << 19,

        /// <summary>
        /// Ελληνικά (Greek)
        /// </summary>
        [EnumMember(Value = "el")]
        El = 1 << 20,

        /// <summary>
        /// български (Bulgarian)
        /// </summary>
        [EnumMember(Value = "bg")]
        Bg = 1 << 21,

        /// <summary>
        /// Pусский (Slovenian)
        /// </summary>
        [EnumMember(Value = "ru")]
        Ru = 1 << 22,

        /// <summary>
        /// العربية (Arabic)
        /// </summary>
        [EnumMember(Value = "ar")]
        Ar = 1 << 23,

        /// <summary>
        /// ภาษาไทย (Thai)
        /// </summary>
        [EnumMember(Value = "th")]
        Th = 1 << 24,

        /// <summary>
        /// 中文 简体 (Simplified Chinese)
        /// </summary>
        [EnumMember(Value = "zh-cn")]
        ZhCn = 1 << 25,

        /// <summary>
        /// 中文 繁體 (Traditional Chinese)
        /// </summary>
        [EnumMember(Value = "zh-tw")]
        ZhTw = 1 << 26,

        /// <summary>
        /// 日本語 (Japanese)
        /// </summary>
        [EnumMember(Value = "ja")]
        Ja = 1 << 27,

        /// <summary>
        /// 한국어 (Korean)
        /// </summary>
        [EnumMember(Value = "ko")]
        Ko = 1 << 28,

        /// <summary>
        /// Română (Romanian)
        /// </summary>
        [EnumMember(Value = "ro")]
        Ro = 1 << 29
    }

    #endregion

    #region /streams/markers

    [Body]
    public class
    CreateStreamMarkerParameters
    {
        /// <summary>
        /// The ID of the user who is streaming.
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

    #endregion

    #region /streams/metadata

    public class
    StreamMetadata
    {
        /// <summary>
        /// The id of the user who is streaming.
        /// </summary>
        [JsonProperty("user_id")]
        public string user_id { get; protected set; }

        /// <summary>
        /// The login of the user who is streaming.
        /// </summary>
        [JsonProperty("user_name")]
        public string user_name { get; protected set; }

        /// <summary>
        /// The id of the game being played.
        /// </summary>
        [JsonProperty("game_id")]
        public string game_id { get; protected set; }

        /// <summary>
        /// The Overwatch metadata of the current stream, if that is the game being played.
        /// </summary>
        [JsonProperty("overwatch")]
        public OverwatchMetadata overwatch { get; protected set; }

        /// <summary>
        /// The Hearthstone metadata of the current stream, if that is the game being played.
        /// </summary>
        [JsonProperty("hearthstone")]
        public HearthstoneMetadata hearthstone { get; protected set; }
    }

    public class
    OverwatchMetadata
    {
        /// <summary>
        /// Overwatch metadata about the broadcaster.
        /// </summary>
        [JsonProperty("broadcaster")]
        public Player<OverwatchHero> broadcaster { get; protected set; }
    }

    public class
    OverwatchHero
    {
        /// <summary>
        /// The role of the Overwatch hero.
        /// </summary>
        [JsonProperty("role")]
        public string role { get; protected set; }

        /// <summary>
        /// The name of the Overwatch hero.
        /// </summary>
        [JsonProperty("name")]
        public string name { get; protected set; }

        /// <summary>
        /// The ability being used by the broadcaster.
        /// </summary>
        [JsonProperty("ability")]
        public string ability { get; protected set; }
    }

    public class
    HearthstoneMetadata
    {
        /// <summary>
        /// Hearthstone metadata about the broadcaster.
        /// </summary>
        [JsonProperty("broadcaster")]
        public Player<HearthstoneHero> broadcaster { get; protected set; }

        /// <summary>
        /// Hearthstone metadata about the opponent.
        /// </summary>
        [JsonProperty("opponent")]
        public Player<HearthstoneHero> opponent { get; protected set; }
    }

    public class
    HearthstoneHero
    {
        /// <summary>
        /// The type of the Hearthstone hero.
        /// </summary>
        [JsonProperty("type")]
        public string type { get; protected set; }

        /// <summary>
        /// The name of the Hearthstone hero.
        /// </summary>
        [JsonProperty("name")]
        public string name { get; protected set; }

        /// <summary>
        /// The class of the Hearthstone hero.
        /// </summary>
        [JsonProperty("class")]
        public string @class { get; protected set; }
    }

    public class
    Player<hero_type>
    {
        /// <summary>
        /// The metadata about the hero selected by the player.
        /// </summary>
        [JsonProperty("hero")]
        public hero_type hero { get; protected set; }
    }

    #endregion

    #region /streams/tags

    public class
    StreamsTagsParameters
    {
        /// <summary>
        /// The user ID of a broadcaster.
        /// </summary>
        [QueryParameter("broadcaster_id")]
        public virtual string broadcaster_id { get; set; }
    }

    public class
    SetStreamsTagsParameters
    {
        /// <summary>
        /// A user ID to update the stream tags for.
        /// </summary>
        [QueryParameter("broadcaster_id")]
        public virtual string broadcaster_id { get; set; }

        /// <summary>
        /// <para>A list of tag ID's, up to 5.</para>
        /// <para>Automatic tags cannot be added or removed.</para>
        /// </summary>
        [Body("tag_ids")]
        public virtual List<string> tag_ids { get; set; }

        public SetStreamsTagsParameters()
        {
            tag_ids = new List<string>();
        }
    }

    #endregion
}