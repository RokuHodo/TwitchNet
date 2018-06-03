// project namespaces
using TwitchNet.Helpers.Json;

// imported namespaces
using Newtonsoft.Json;

namespace
TwitchNet.Rest.Api.Videos
{
    [JsonConverter(typeof(EnumCacheConverter))]
    public enum
    BroadcasterLanguage
    {
        /// <summary>
        /// Unsupported broadcaster language.
        /// </summary>
        None    = 0,

        /// <summary>
        /// English (English)
        /// </summary>
        En,

        /// <summary>
        /// Dansk (Danish)
        /// </summary>
        Da,

        /// <summary>
        /// Deutsch (German)
        /// </summary>
        De,

        /// <summary>
        /// Español (Spanish)
        /// </summary>
        Es,

        /// <summary>
        /// Français (French)
        /// </summary>
        Fr,

        /// <summary>
        /// Italiano (Italian)
        /// </summary>
        It,

        /// <summary>
        /// Magyar (Hungarian)
        /// </summary>
        Hu,

        /// <summary>
        /// Nederlands (Netherland)
        /// </summary>
        Nl,

        /// <summary>
        /// Norsk (Norwegian)
        /// </summary>
        No,

        /// <summary>
        /// Polski (Polish)
        /// </summary>
        Pl,

        /// <summary>
        /// Português (Portuguese)
        /// </summary>
        Pt,

        /// <summary>
        /// Slovenčina (Slovak)
        /// </summary>
        Sk,

        /// <summary>
        /// Suomi (Finish)
        /// </summary>
        Fi,

        /// <summary>
        /// Svenska (Swedish)
        /// </summary>
        Sv,

        /// <summary>
        /// Tiếng Việt (Vietnamese)
        /// </summary>
        Vi,

        /// <summary>
        /// Türkçe (Turkish)
        /// </summary>
        Tr,

        /// <summary>
        /// Čeština (Czech)
        /// </summary>
        Cs,

        /// <summary>
        /// Ελληνικά (Greek)
        /// </summary>
        El,

        /// <summary>
        /// български (Bulgarian)
        /// </summary>
        Bg,

        /// <summary>
        /// Pусский (Slovenian)
        /// </summary>
        Ru,

        /// <summary>
        /// العربية (Arabic)
        /// </summary>
        Ar,

        /// <summary>
        /// ภาษาไทย (Thai)
        /// </summary>
        Th,

        /// <summary>
        /// 中文 (Chinese)
        /// </summary>
        Zh,

        /// <summary>
        /// 中文 繁體 (Chinese - Hong Kong S.A.R)
        /// </summary>
        ZhHk,

        /// <summary>
        /// 日本語 (Japanese)
        /// </summary>
        Ja,

        /// <summary>
        /// 한국어 (Korean)
        /// </summary>
        Ko,

        /// <summary>
        /// American Sign Language
        /// </summary>
        Asl,

        /// <summary>
        /// Other
        /// </summary>
        Other
    }
}
