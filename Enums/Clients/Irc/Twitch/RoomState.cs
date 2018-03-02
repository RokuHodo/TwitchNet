// standard namespaces
using System;
using System.Runtime.Serialization;

namespace TwitchNet.Enums.Clients.Irc.Twitch
{
    [Flags]
    public enum
    RoomState
    {
        /// <summary>
        /// No room state has changed.
        /// </summary>
        None            = 0,

        /// <summary>
        /// Emote only mode has been enabled or disabled.
        /// </summary>
        EmoteOnly       = 1 << 0,

        /// <summary>
        /// Mercury mode as been enabled or disabled.
        /// </summary>
        Mercury         = 1 << 1,

        /// <summary>
        /// R9K mode has been enabled or disabled.
        /// </summary>
        R9K             = 1 << 2,

        /// <summary>
        /// Rituals has been enabled or disabled.
        /// </summary>
        Rituals         = 1 << 3,

        /// <summary>
        /// Subscriber mode has been enabled or disabled.
        /// </summary>
        SubsOnly        = 1 << 4,

        /// <summary>
        /// Follower only mode has been enabled, disabled, or the duration has changed.
        /// </summary>
        FollowersOnly   = 1 << 5,

        /// <summary>
        /// Slow mode has been enabled, disabled, or the duration has changed.
        /// </summary>
        Slow            = 1 << 6,

        /// <summary>
        /// The restricted broadcaster language has been enabled or disabled.
        /// </summary>
        BroadcasterLang = 1 << 7,
    }
}
