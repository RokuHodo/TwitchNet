// standard namespaces
using System.Runtime.Serialization;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public enum
    CommercialLength
    {
        /// <summary>
        /// Unsupported commercial length.
        /// </summary>
        [EnumMember(Value = "")]
        Other       = 0,

        /// <summary>
        /// 30 seconds.
        /// </summary>
        [EnumMember(Value = "30")]
        Seconds30   = 30,

        /// <summary>
        /// 1 minute.
        /// </summary>
        [EnumMember(Value = "60")]
        Seconds60   = 60,

        /// <summary>
        /// 1 minute, 30 seconds
        /// </summary>
        [EnumMember(Value = "90")]
        Seconds90   = 90,

        /// <summary>
        /// 2 minutes
        /// </summary>
        [EnumMember(Value = "120")]
        Seconds120  = 120,

        /// <summary>
        /// 2 minutes, 30 seconds
        /// </summary>
        [EnumMember(Value = "150")]
        Seconds150  = 150,

        /// <summary>
        /// 3 minutes
        /// </summary>
        [EnumMember(Value = "180")]
        Seconds180  = 180
    }
}
