// standard namespaces
using System.Runtime.Serialization;

// imported namespaces
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace
TwitchNet.Enums.Api.Users
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum
    UserType
    {
        /// <summary>
        /// The user is a normal user.
        /// </summary>
        [EnumMember(Value = "")]
        Empty       = 0,

        /// <summary>
        /// The user is a Twitch global mod.
        /// </summary>
        [EnumMember(Value = "global_mod")]
        Global_Mod  = 1,

        /// <summary>
        /// The user is a Twitch staff member.
        /// </summary>
        [EnumMember(Value = "staff")]
        Staff       = 2,

        /// <summary>
        /// The user is a Twitch admin.
        /// </summary>
        [EnumMember(Value = "admin")]
        Admin       = 3,
    }
}
