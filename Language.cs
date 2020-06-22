// standard namespaces
using System;
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Helpers.Json;

// imported namespaces
using Newtonsoft.Json;

namespace
TwitchNet
{
    /// <summary>
    /// The video language, or stream language if set.
    /// </summary>
    [Flags]
    [JsonConverter(typeof(EnumConverter))]    
    public enum
    Language : ulong
    {
        /// <summary>
        /// Unknown or unsupported broadcaster language.
        /// </summary>
        [EnumMember(Value = "")]
        Unknown = 0u,

        /// <summary>
        /// English (English)
        /// </summary>
        [EnumMember(Value = "en")]
        En = 1u << 0,

        /// <summary>
        /// Bahasa Indonesia (Indonesian)
        /// </summary>
        [EnumMember(Value = "id")]
        Id = 1u << 1,

        /// <summary>
        /// Català (Catalan)
        /// </summary>
        [EnumMember(Value = "ca")]
        Ca = 1u << 2,

        /// <summary>
        /// Dansk (Danish)
        /// </summary>
        [EnumMember(Value = "da")]
        Da = 1u << 3,

        /// <summary>
        /// Deutsch (German)
        /// </summary>
        [EnumMember(Value = "de")]
        De = 1u << 4,

        /// <summary>
        /// Español (Spanish)
        /// </summary>
        [EnumMember(Value = "es")]
        Es = 1u << 5,

        /// <summary>
        /// Français (French)
        /// </summary>
        [EnumMember(Value = "fr")]
        Fr = 1u << 6,

        /// <summary>
        /// Italiano (Italian)
        /// </summary>
        [EnumMember(Value = "it")]
        It = 1u << 7,

        /// <summary>
        /// Magyar (Hungarian)
        /// </summary>
        [EnumMember(Value = "hu")]
        Hu = 1u << 8,

        /// <summary>
        /// Nederlands (Netherland)
        /// </summary>
        [EnumMember(Value = "nl")]
        Nl = 1u << 9,

        /// <summary>
        /// Norsk (Norwegian)
        /// </summary>
        [EnumMember(Value = "no")]
        No = 1u << 10,

        /// <summary>
        /// Polski (Polish)
        /// </summary>
        [EnumMember(Value = "pl")]
        Pl = 1u << 11,

        /// <summary>
        /// Português (Portuguese)
        /// </summary>
        [EnumMember(Value = "pt")]
        Pt = 1u << 11,

        /// <summary>
        /// Română (Romanian)
        /// </summary>
        [EnumMember(Value = "ro")]
        Ro = 1u << 12,

        /// <summary>
        /// Slovenčina (Slovak)
        /// </summary>
        [EnumMember(Value = "sk")]
        Sk = 1u << 13,

        /// <summary>
        /// Suomi (Finish)
        /// </summary>
        [EnumMember(Value = "fi")]
        Fi = 1u << 14,

        /// <summary>
        /// Svenska (Swedish)
        /// </summary>
        [EnumMember(Value = "sv")]
        Sv = 1u << 15,

        /// <summary>
        /// Tagalog (Austronesian Language, Filipino)
        /// </summary>
        [EnumMember(Value = "tl")]
        Tl = 1u << 16,

        /// <summary>
        /// Tiếng Việt (Vietnamese)
        /// </summary>
        [EnumMember(Value = "vi")]
        Vi = 1u << 17,

        /// <summary>
        /// Türkçe (Turkish)
        /// </summary>
        [EnumMember(Value = "tr")]
        Tr = 1u << 18,

        /// <summary>
        /// Čeština (Czech)
        /// </summary>
        [EnumMember(Value = "cs")]
        Cs = 1u << 19,

        /// <summary>
        /// Ελληνικά (Greek)
        /// </summary>
        [EnumMember(Value = "el")]
        El = 1u << 20,

        /// <summary>
        /// български (Bulgarian)
        /// </summary>
        [EnumMember(Value = "bg")]
        Bg = 1u << 21,

        /// <summary>
        /// Pусский (Slovenian)
        /// </summary>
        [EnumMember(Value = "ru")]
        Ru = 1u << 22,

        /// <summary>
        /// Українська (Ukrainian)
        /// </summary>
        [EnumMember(Value = "uk")]
        Uk = 1u << 23,

        /// <summary>
        /// العربية (Arabic)
        /// </summary>
        [EnumMember(Value = "ar")]
        Ar = 1u << 24,

        /// <summary>
        /// Malay (Austronesian Language, Malayan)
        /// </summary>
        [EnumMember(Value = "ms")]
        Ms = 1u << 25,

        /// <summary>
        /// हिन्दी (Hindi)
        /// </summary>
        [EnumMember(Value = "hi")]
        Hi = 1u << 26,

        /// <summary>
        /// ภาษาไทย (Thai)
        /// </summary>
        [EnumMember(Value = "th")]
        Th = 1u << 27,

        /// <summary>
        /// 中文 (Chinese)
        /// </summary>
        [EnumMember(Value = "zh")]
        Zh = 1u << 28,

        /// <summary>
        /// 中文 繁體 (Chinese - Hong Kong S.A.R)
        /// </summary>
        [EnumMember(Value = "zh-hk")]
        ZhHk = 1u << 29,

        /// <summary>
        /// 日本語 (Japanese)
        /// </summary>
        [EnumMember(Value = "ja")]
        Ja = 1u << 30,

        /// <summary>
        /// 한국어 (Korean)
        /// </summary>
        [EnumMember(Value = "ko")]
        Ko = 1u << 31,

        /// <summary>
        /// American Sign Language
        /// </summary>
        [EnumMember(Value = "asl")]
        Asl = 1u << 32,

        /// <summary>
        /// Other
        /// </summary>
        [EnumMember(Value = "other")]
        Other = 1u << 33
    }
}
