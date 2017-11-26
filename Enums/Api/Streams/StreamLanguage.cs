// standard namespaces
using System;
using System.Runtime.Serialization;

// imported namespaces
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace
TwitchNet.Enums.Api.Streams
{
    [Flags]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum
    StreamLanguage
    {        
        /// <summary>
        /// Dansk (Danish)
        /// </summary>
        [EnumMember(Value = "da")]
        Da      = 1 << 0,

        /// <summary>
        /// Deutsch (German)
        /// </summary>
        [EnumMember(Value = "de")]
        De      = 1 << 1,

        /// <summary>
        /// English (English)
        /// </summary>
        [EnumMember(Value = "en")]
        En      = 1 << 2,

        /// <summary>
        /// English - UK (English - UK)
        /// </summary>
        [EnumMember(Value = "en-gb")]
        En_Gb   = 1 << 3,

        /// <summary>
        /// Español - España (Spanish - Spain)
        /// </summary>
        [EnumMember(Value = "es")]
        Es      = 1 << 4,

        /// <summary>
        /// Español - Latinoamérica (Spanish - Latin America)
        /// </summary>
        [EnumMember(Value = "es-mx")]
        Es_Mx   = 1 << 5,

        /// <summary>
        /// Français (French)
        /// </summary>
        [EnumMember(Value = "fr")]
        Fr      = 1 << 6,

        /// <summary>
        /// Italiano (Italian)
        /// </summary>
        [EnumMember(Value = "it")]
        It      = 1 << 7,

        /// <summary>
        /// Magyar (Hungarian)
        /// </summary>
        [EnumMember(Value = "hu")]
        Hu      = 1 << 8,

        /// <summary>
        /// Nederlands (Netherland)
        /// </summary>
        [EnumMember(Value = "nl")]
        Nl      = 1 << 9,

        /// <summary>
        /// Norsk (Norwegian)
        /// </summary>
        [EnumMember(Value = "no")]
        No      = 1 << 10,

        /// <summary>
        /// Polski (Polish)
        /// </summary>
        [EnumMember(Value = "pl")]
        Pl      = 1 << 11,

        /// <summary>
        /// Português (Portuguese)
        /// </summary>
        [EnumMember(Value = "pt")]
        Pt      = 1 << 12,

        /// <summary>
        /// Português - Brasil (Portuguese - Brazil)
        /// </summary>
        [EnumMember(Value = "pt-br")]
        Pt_Br   = 1 << 13,

        /// <summary>
        /// Slovenčina (Slovak)
        /// </summary>
        [EnumMember(Value = "sk")]
        Sk      = 1 << 14,

        /// <summary>
        /// Suomi (Finish)
        /// </summary>
        [EnumMember(Value = "fi")]
        Fi      = 1 << 15,

        /// <summary>
        /// Svenska (Swedish)
        /// </summary>
        [EnumMember(Value = "sv")]
        Sv      = 1 << 16,

        /// <summary>
        /// Tiếng Việt (Vietnamese)
        /// </summary>
        [EnumMember(Value = "vi")]
        Vi      = 1 << 17,

        /// <summary>
        /// Türkçe (Turkish)
        /// </summary>
        [EnumMember(Value = "tr")]
        Tr      = 1 << 18,

        /// <summary>
        /// Čeština (Czech)
        /// </summary>
        [EnumMember(Value = "cs")]
        Cs      = 1 << 19,

        /// <summary>
        /// Ελληνικά (Greek)
        /// </summary>
        [EnumMember(Value = "el")]
        El      = 1 << 20,

        /// <summary>
        /// български (Bulgarian)
        /// </summary>
        [EnumMember(Value = "bg")]
        Bg      = 1 << 21,

        /// <summary>
        /// Pусский (Slovenian)
        /// </summary>
        [EnumMember(Value = "ru")]
        Ru      = 1 << 22,

        /// <summary>
        /// العربية (Arabic)
        /// </summary>
        [EnumMember(Value = "ar")]
        Ar      = 1 << 23,

        /// <summary>
        /// ภาษาไทย (Thai)
        /// </summary>
        [EnumMember(Value = "th")]
        Th      = 1 << 24,

        /// <summary>
        /// 中文 简体 (Simplified Chinese)
        /// </summary>
        [EnumMember(Value = "zh-cn")]
        Zh_Cn   = 1 << 25,

        /// <summary>
        /// 中文 繁體 (Traditional Chinese)
        /// </summary>
        [EnumMember(Value = "zh-tw")]
        Zh_Tw   = 1 << 26,

        /// <summary>
        /// 日本語 (Japanese)
        /// </summary>
        [EnumMember(Value = "ja")]
        Ja      = 1 << 27,

        /// <summary>
        /// 한국어 (Korean)
        /// </summary>
        [EnumMember(Value = "ko")]
        Ko      = 1 << 28,
    }
}
