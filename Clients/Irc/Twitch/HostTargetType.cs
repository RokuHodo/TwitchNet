// standard namespaces
using System.Runtime.Serialization;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public enum
    HostTargetType
    {
        /// <summary>
        /// There was an error parsing the host target message.
        /// </summary>
        [EnumMember(Value = "")]
        None    = 0,

        /// <summary>
        /// The hosting channel started hosting another channel.
        /// </summary>
        Start   = 1,

        /// <summary>
        /// The hosting channel stopped hosting another channel.
        /// </summary>
        Stop    = 2
    }
}
