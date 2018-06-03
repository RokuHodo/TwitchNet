// standard namespaces
using System;

// imported namespaces
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace
TwitchNet.Rest.Api.Streams
{
    // TODO: Add custom converter for the deserializer.
    [Flags]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum
    StreamLanguage
    {        
        /// <summary>
        /// Unsupported stream language.
        /// </summary>
        None = 0,

        /// <summary>
        /// Dansk (Danish)
        /// </summary>
        Da      = 1 << 0,

        /// <summary>
        /// Deutsch (German)
        /// </summary>
        De      = 1 << 1,

        /// <summary>
        /// English (English)
        /// </summary>
        En      = 1 << 2,

        /// <summary>
        /// English - UK (English - UK)
        /// </summary>
        EnGb   = 1 << 3,

        /// <summary>
        /// Español - España (Spanish - Spain)
        /// </summary>
        Es      = 1 << 4,

        /// <summary>
        /// Español - Latinoamérica (Spanish - Latin America)
        /// </summary>
        EsMx   = 1 << 5,

        /// <summary>
        /// Français (French)
        /// </summary>
        Fr      = 1 << 6,

        /// <summary>
        /// Italiano (Italian)
        /// </summary>
        It      = 1 << 7,

        /// <summary>
        /// Magyar (Hungarian)
        /// </summary>
        Hu      = 1 << 8,

        /// <summary>
        /// Nederlands (Netherland)
        /// </summary>
        Nl      = 1 << 9,

        /// <summary>
        /// Norsk (Norwegian)
        /// </summary>
        No      = 1 << 10,

        /// <summary>
        /// Polski (Polish)
        /// </summary>
        Pl      = 1 << 11,

        /// <summary>
        /// Português (Portuguese)
        /// </summary>
        Pt      = 1 << 12,

        /// <summary>
        /// Português - Brasil (Portuguese - Brazil)
        /// </summary>
        PtBr   = 1 << 13,

        /// <summary>
        /// Slovenčina (Slovak)
        /// </summary>
        Sk      = 1 << 14,

        /// <summary>
        /// Suomi (Finish)
        /// </summary>
        Fi      = 1 << 15,

        /// <summary>
        /// Svenska (Swedish)
        /// </summary>
        Sv      = 1 << 16,

        /// <summary>
        /// Tiếng Việt (Vietnamese)
        /// </summary>
        Vi      = 1 << 17,

        /// <summary>
        /// Türkçe (Turkish)
        /// </summary>
        Tr      = 1 << 18,

        /// <summary>
        /// Čeština (Czech)
        /// </summary>
        Cs      = 1 << 19,

        /// <summary>
        /// Ελληνικά (Greek)
        /// </summary>
        El      = 1 << 20,

        /// <summary>
        /// български (Bulgarian)
        /// </summary>
        Bg      = 1 << 21,

        /// <summary>
        /// Pусский (Slovenian)
        /// </summary>
        Ru      = 1 << 22,

        /// <summary>
        /// العربية (Arabic)
        /// </summary>
        Ar      = 1 << 23,

        /// <summary>
        /// ภาษาไทย (Thai)
        /// </summary>
        Th      = 1 << 24,

        /// <summary>
        /// 中文 简体 (Simplified Chinese)
        /// </summary>
        ZhCn   = 1 << 25,

        /// <summary>
        /// 中文 繁體 (Traditional Chinese)
        /// </summary>
        ZhTw   = 1 << 26,

        /// <summary>
        /// 日本語 (Japanese)
        /// </summary>
        Ja      = 1 << 27,

        /// <summary>
        /// 한국어 (Korean)
        /// </summary>
        Ko      = 1 << 28,
    }
}
