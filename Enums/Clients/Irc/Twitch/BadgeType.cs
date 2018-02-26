// standard namespaces
using System.Runtime.Serialization;

namespace TwitchNet.Enums.Clients.Irc.Twitch
{
    public enum
    BadgeType
    {
        [EnumMember(Value = "")]
        None        = 0,

        [EnumMember(Value = "admin")]
        Admin       = 1,

        [EnumMember(Value = "bits")]
        Bits        = 2,

        [EnumMember(Value = "broadcaster")]
        Broadcaster = 3,

        [EnumMember(Value = "global_mod")]
        GlobalMod   = 4,

        [EnumMember(Value = "moderator")]
        Moderator   = 5,

        [EnumMember(Value = "subscriber")]
        Subscriber  = 6,

        [EnumMember(Value = "staff")]
        Staff       = 7,

        [EnumMember(Value = "premium")]
        Premium     = 8,

        [EnumMember(Value = "turbo")]
        Turbo       = 9,
    }
}
