// standard namespaces
using System.Runtime.Serialization;

namespace TwitchNet.Enums.Clients.Irc.Twitch
{
    public enum
    RitualType
    {
        /// <summary>
        /// Invalid ritual type.
        /// </summary>
        [EnumMember(Value = "")]
        None        = 0,

        /// <summary>
        /// A person is new to a channel.
        /// </summary>
        [EnumMember(Value = "new_chatter")]
        NewChatter   = 1
    }
}
