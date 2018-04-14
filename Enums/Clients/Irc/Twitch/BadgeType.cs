// standard namespaces
using System.Runtime.Serialization;

namespace TwitchNet.Enums.Clients.Irc.Twitch
{
    public enum
    BadgeType
    {
        /// <summary>
        /// No badge.
        /// </summary>
        [EnumMember(Value = "")]
        None        = 0,

        /// <summary>
        /// Admin badge.
        /// </summary>
        [EnumMember(Value = "admin")]
        Admin       = 1,

        /// <summary>
        /// Bits badge.
        /// </summary>
        [EnumMember(Value = "bits")]
        Bits        = 2,

        /// <summary>
        /// Broadcaster badge.
        /// </summary>
        [EnumMember(Value = "broadcaster")]
        Broadcaster = 3,

        /// <summary>
        /// Global mod badge.
        /// </summary>
        [EnumMember(Value = "global_mod")]
        GlobalMod   = 4,

        /// <summary>
        /// Moderator badge.
        /// </summary>
        [EnumMember(Value = "moderator")]
        Moderator   = 5,

        /// <summary>
        /// Subscriber badge.
        /// </summary>
        [EnumMember(Value = "subscriber")]
        Subscriber  = 6,

        /// <summary>
        /// Staff badge.
        /// </summary>
        [EnumMember(Value = "staff")]
        Staff       = 7,

        /// <summary>
        /// Trwitch Prime badge.
        /// </summary>
        [EnumMember(Value = "premium")]
        Premium     = 8,

        /// <summary>
        /// Twitch Turbo badge.
        /// </summary>
        [EnumMember(Value = "turbo")]
        Turbo       = 9,
    }
}
