// standard namespaces
using System.Runtime.Serialization;

namespace TwitchNet.Enums.Api
{
    public enum UserType
    {
        [EnumMember(Value = "")]
        Empty       = 0,
        [EnumMember(Value = "staff")]
        Staff       = 1,
        [EnumMember(Value = "admin")]
        Admin       = 2,
        [EnumMember(Value = "global_mod")]
        GlobalMod   = 3
    }
}
