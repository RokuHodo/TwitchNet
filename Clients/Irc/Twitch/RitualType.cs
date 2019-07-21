// standard namespaces
using System.Runtime.Serialization;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public enum
    RitualType
    {
        /// <summary>
        /// Unsupported ritual type.
        /// </summary>
        [EnumMember(Value = "")]
        Other        = 0,

        /// <summary>
        /// A person is new to a channel.
        /// </summary>
        [EnumMember(Value = "new_chatter")]
        NewChatter   = 1
    }
}
