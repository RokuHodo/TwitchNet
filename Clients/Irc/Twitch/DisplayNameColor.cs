// standard namespaces
using System.Runtime.Serialization;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public enum
    DisplayNameColor
    {
        /// <summary>
        /// Unsupported display name color.
        /// </summary>
        [EnumMember(Value = "")]
        Other       = 0,

        /// <summary>
        /// Blue.
        /// </summary>
        [EnumMember(Value = "blue")]
        Blue,

        /// <summary>
        /// Blue violet.
        /// </summary>
        [EnumMember(Value = "blueviolet")]
        BlueViolet,

        /// <summary>
        /// Cadet blue.
        /// </summary>
        [EnumMember(Value = "cadetblue")]
        CadetBlue,

        /// <summary>
        /// Chocolate.
        /// </summary>
        [EnumMember(Value = "chocloate")]
        Chocloate,

        /// <summary>
        /// Coral.
        /// </summary>
        [EnumMember(Value = "coral")]
        Coral,

        /// <summary>
        /// Dodger blue.
        /// </summary>
        [EnumMember(Value = "dodgerblue")]
        DodgerBlue,

        /// <summary>
        /// Fire brick.
        /// </summary>
        [EnumMember(Value = "firebrick")]
        FireBrick,

        /// <summary>
        /// Golden rod.
        /// </summary>
        [EnumMember(Value = "goldenrod")]
        GoldenRod,

        /// <summary>
        /// Green.
        /// </summary>
        [EnumMember(Value = "green")]
        Green,

        /// <summary>
        /// Hot pink.
        /// </summary>
        [EnumMember(Value = "hotpink")]
        HotPink,

        /// <summary>
        /// Orange red.
        /// </summary>
        [EnumMember(Value = "orangered")]
        OrangeRed,

        /// <summary>
        /// Red.
        /// </summary>
        [EnumMember(Value = "red")]
        Red,

        /// <summary>
        /// Sea green.
        /// </summary>
        [EnumMember(Value = "seagreen")]
        SeaGreen,

        /// <summary>
        /// Spring green.
        /// </summary>
        [EnumMember(Value = "springgreen")]
        SpringGreen,

        /// <summary>
        /// Yellow green.
        /// </summary>
        [EnumMember(Value = "yellowgreen")]
        YellowGreen,
    }
}
