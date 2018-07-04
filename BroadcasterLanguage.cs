// standard namespaces
using System.Runtime.Serialization;

// project namespaces
using TwitchNet.Helpers.Json;

// imported namespaces
using Newtonsoft.Json;

namespace
TwitchNet
{
    /// <summary>
    /// The language selected in the Twitch dashboard or in the video information editor, not the language selected at the home page.
    /// </summary>
    [JsonConverter(typeof(EnumCacheConverter))]    
    public enum
    BroadcasterLanguage
    {
        /// <summary>
        /// Unsupported broadcaster language.
        /// </summary>
        [EnumMember(Value = "")]
        None    = 0,

        /// <summary>
        /// English (English)
        /// </summary>
        [EnumMember(Value = "en")]
        En,

        /// <summary>
        /// Dansk (Danish)
        /// </summary>
        [EnumMember(Value = "da")]
        Da,

        /// <summary>
        /// Deutsch (German)
        /// </summary>
        [EnumMember(Value = "de")]
        De,

        /// <summary>
        /// Español (Spanish)
        /// </summary>
        [EnumMember(Value = "es")]
        Es,

        /// <summary>
        /// Français (French)
        /// </summary>
        [EnumMember(Value = "fr")]
        Fr,

        /// <summary>
        /// Italiano (Italian)
        /// </summary>
        [EnumMember(Value = "it")]
        It,

        /// <summary>
        /// Magyar (Hungarian)
        /// </summary>
        [EnumMember(Value = "hu")]
        Hu,

        /// <summary>
        /// Nederlands (Netherland)
        /// </summary>
        [EnumMember(Value = "nl")]
        Nl,

        /// <summary>
        /// Norsk (Norwegian)
        /// </summary>
        [EnumMember(Value = "no")]
        No,

        /// <summary>
        /// Polski (Polish)
        /// </summary>
        [EnumMember(Value = "pl")]
        Pl,

        /// <summary>
        /// Português (Portuguese)
        /// </summary>
        [EnumMember(Value = "pt")]
        Pt,

        /// <summary>
        /// Slovenčina (Slovak)
        /// </summary>
        [EnumMember(Value = "sk")]
        Sk,

        /// <summary>
        /// Suomi (Finish)
        /// </summary>
        [EnumMember(Value = "fi")]
        Fi,

        /// <summary>
        /// Svenska (Swedish)
        /// </summary>
        [EnumMember(Value = "sv")]
        Sv,

        /// <summary>
        /// Tiếng Việt (Vietnamese)
        /// </summary>
        [EnumMember(Value = "vi")]
        Vi,

        /// <summary>
        /// Türkçe (Turkish)
        /// </summary>
        [EnumMember(Value = "tr")]
        Tr,

        /// <summary>
        /// Čeština (Czech)
        /// </summary>
        [EnumMember(Value = "cs")]
        Cs,

        /// <summary>
        /// Ελληνικά (Greek)
        /// </summary>
        [EnumMember(Value = "el")]
        El,

        /// <summary>
        /// български (Bulgarian)
        /// </summary>
        [EnumMember(Value = "bg")]
        Bg,

        /// <summary>
        /// Pусский (Slovenian)
        /// </summary>
        [EnumMember(Value = "")]
        Ru,

        /// <summary>
        /// العربية (Arabic)
        /// </summary>
        [EnumMember(Value = "ru")]
        Ar,

        /// <summary>
        /// ภาษาไทย (Thai)
        /// </summary>
        [EnumMember(Value = "ar")]
        Th,

        /// <summary>
        /// 中文 (Chinese)
        /// </summary>
        [EnumMember(Value = "th")]
        Zh,

        /// <summary>
        /// 中文 繁體 (Chinese - Hong Kong S.A.R)
        /// </summary>
        [EnumMember(Value = "zh-hk")]
        ZhHk,

        /// <summary>
        /// 日本語 (Japanese)
        /// </summary>
        [EnumMember(Value = "ja")]
        Ja,

        /// <summary>
        /// 한국어 (Korean)
        /// </summary>
        [EnumMember(Value = "ko")]
        Ko,

        /// <summary>
        /// American Sign Language
        /// </summary>
        [EnumMember(Value = "asl")]
        Asl,

        /// <summary>
        /// Other
        /// </summary>
        [EnumMember(Value = "other")]
        Other
    }
}
