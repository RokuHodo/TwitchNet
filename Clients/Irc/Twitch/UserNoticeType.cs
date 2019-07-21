// standard namespaces
using System.Runtime.Serialization;

namespace
TwitchNet.Clients.Irc.Twitch
{
    public enum
    UserNoticeType
    {
        /// <summary>
        /// Unsupported user notice type.
        /// </summary>
        [EnumMember(Value = "")]
        Other    = 0,

        /// <summary>
        /// A user subscribed.
        /// </summary>
        [EnumMember(Value = "sub")]
        Sub,

        /// <summary>
        /// A user resubscribed.
        /// </summary>
        [EnumMember(Value = "resub")]
        Resub,

        /// <summary>
        /// A user gifted a sub to another user.
        /// </summary>
        [EnumMember(Value = "giftsub")]
        GiftSub,

        /// <summary>
        /// A user is rading another user.
        /// </summary>
        [EnumMember(Value = "raid")]
        Raid,

        /// <summary>
        /// A ritual has occured.
        /// </summary>
        [EnumMember(Value = "ritual")]
        Ritual
    }
}
