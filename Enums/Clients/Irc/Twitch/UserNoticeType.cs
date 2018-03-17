// standard namespaces
using System.Runtime.Serialization;

namespace TwitchNet.Enums.Clients.Irc.Twitch
{
    public enum
    UserNoticeType
    {
        /// <summary>
        /// No room state has changed.
        /// </summary>
        [EnumMember(Value = "")]
        None    = 0,

        /// <summary>
        /// A user subscribed.
        /// </summary>
        [EnumMember(Value = "sub")]
        Sub     = 1,

        /// <summary>
        /// A user resubscribed.
        /// </summary>
        [EnumMember(Value = "resub")]
        Resub   = 2,

        /// <summary>
        /// A user gifted a sub to another user.
        /// </summary>
        [EnumMember(Value = "giftsub")]
        GiftSub = 3,

        /// <summary>
        /// A user is rading another user.
        /// </summary>
        [EnumMember(Value = "raid")]
        Raid    = 4,

        /// <summary>
        /// A ritual has occured.
        /// </summary>
        [EnumMember(Value = "ritual")]
        Ritual  = 5
    }
}
