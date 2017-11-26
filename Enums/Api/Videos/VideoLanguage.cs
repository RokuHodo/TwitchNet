// standard namespaces
using System.Runtime.Serialization;

// imported namespaces
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace
TwitchNet.Enums.Api.Videos
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum
    VideoLanguage
    {
        /// <summary>
        /// English (English)
        /// </summary>
        [EnumMember(Value = "en")]
        En      = 0,

        /// <summary>
        /// Dansk (Danish)
        /// </summary>
        [EnumMember(Value = "da")]
        Da      = 1,

        /// <summary>
        /// Deutsch (German)
        /// </summary>
        [EnumMember(Value = "de")]
        De      = 2,

        /// <summary>
        /// Español (Spanish)
        /// </summary>
        [EnumMember(Value = "es")]
        Es      = 3,

        /// <summary>
        /// Français (French)
        /// </summary>
        [EnumMember(Value = "fr")]
        Fr      = 4,

        /// <summary>
        /// Italiano (Italian)
        /// </summary>
        [EnumMember(Value = "it")]
        It      = 5,

        /// <summary>
        /// Magyar (Hungarian)
        /// </summary>
        [EnumMember(Value = "hu")]
        Hu      = 6,

        /// <summary>
        /// Nederlands (Netherland)
        /// </summary>
        [EnumMember(Value = "nl")]
        Nl      = 7,

        /// <summary>
        /// Norsk (Norwegian)
        /// </summary>
        [EnumMember(Value = "no")]
        No      = 8,

        /// <summary>
        /// Polski (Polish)
        /// </summary>
        [EnumMember(Value = "pl")]
        Pl      = 9,

        /// <summary>
        /// Português (Portuguese)
        /// </summary>
        [EnumMember(Value = "pt")]
        Pt      = 10,

        /// <summary>
        /// Slovenčina (Slovak)
        /// </summary>
        [EnumMember(Value = "sk")]
        Sk      = 11,

        /// <summary>
        /// Suomi (Finish)
        /// </summary>
        [EnumMember(Value = "fi")]
        Fi      = 12,

        /// <summary>
        /// Svenska (Swedish)
        /// </summary>
        [EnumMember(Value = "sv")]
        Sv      = 13,

        /// <summary>
        /// Tiếng Việt (Vietnamese)
        /// </summary>
        [EnumMember(Value = "vi")]
        Vi      = 14,

        /// <summary>
        /// Türkçe (Turkish)
        /// </summary>
        [EnumMember(Value = "tr")]
        Tr      = 15,

        /// <summary>
        /// Čeština (Czech)
        /// </summary>
        [EnumMember(Value = "cs")]
        Cs      = 16,

        /// <summary>
        /// Ελληνικά (Greek)
        /// </summary>
        [EnumMember(Value = "el")]
        El      = 17,

        /// <summary>
        /// български (Bulgarian)
        /// </summary>
        [EnumMember(Value = "bg")]
        Bg      = 18,

        /// <summary>
        /// Pусский (Slovenian)
        /// </summary>
        [EnumMember(Value = "ru")]
        Ru      = 19,

        /// <summary>
        /// العربية (Arabic)
        /// </summary>
        [EnumMember(Value = "ar")]
        Ar      = 20,

        /// <summary>
        /// ภาษาไทย (Thai)
        /// </summary>
        [EnumMember(Value = "th")]
        Th      = 21,

        /// <summary>
        /// 中文 (Chinese)
        /// </summary>
        [EnumMember(Value = "zh")]
        Zh      = 22,

        /// <summary>
        /// 中文 繁體 (Chinese - Hong Kong S.A.R)
        /// </summary>
        [EnumMember(Value = "zh-hk")]
        Zh_Hk   = 23,

        /// <summary>
        /// 日本語 (Japanese)
        /// </summary>
        [EnumMember(Value = "ja")]
        Ja      = 24,

        /// <summary>
        /// 한국어 (Korean)
        /// </summary>
        [EnumMember(Value = "ko")]
        Ko      = 25,

        /// <summary>
        /// American Sign Language
        /// </summary>
        [EnumMember(Value = "asl")]
        Asl     = 25,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember(Value = "other")]
        Other   = 26
    }
}
